using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using Json.Masker.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Json.Masker.Newtonsoft;

/// <summary>
/// Contract resolver that masks properties marked with <see cref="SensitiveAttribute"/> during serialization.
/// </summary>
/// <param name="maskingService">The masking service used to mask sensitive values.</param>
public class MaskingContractResolver(IMaskingService maskingService) : DefaultContractResolver
{
    /// <inheritdoc />
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);

        var sensitiveAttr = member.GetCustomAttribute<SensitiveAttribute>();
        if (sensitiveAttr is not null && prop.ValueProvider is { } inner)
        {
#pragma warning disable CS0618 //note: MemberConverter is obsolete but still required for legacy JsonConverter discovery
            var converter = prop.Converter ?? prop.MemberConverter;
#pragma warning restore CS0618

            prop.ValueProvider = new MaskingCollectionValueProvider(inner, maskingService, sensitiveAttr, converter);
            prop.Converter = null;
#pragma warning disable CS0618 // MemberConverter is obsolete but still required for legacy JsonConverter discovery
            prop.MemberConverter = null;
#pragma warning restore CS0618

            if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
            {
                prop.PropertyType = typeof(IEnumerable<string>);
            }
            else
            {
                prop.PropertyType = typeof(string);
            }
        }

        return prop;
    }

    /// <summary>
    /// Value provider that applies masking to values retrieved from the decorated provider.
    /// </summary>
    /// <param name="inner">The underlying value provider.</param>
    /// <param name="maskingService">The masking service used to mask values.</param>
    /// <param name="attr">The sensitive attribute that defines masking behavior.</param>
    /// <param name="converter">An optional converter defined on the member.</param>
    internal class MaskingCollectionValueProvider(
        IValueProvider inner,
        IMaskingService maskingService,
        SensitiveAttribute attr,
        JsonConverter? converter)
        : IValueProvider
    {
        /// <summary>
        /// Retrieves the masked value from the target object.
        /// </summary>
        /// <param name="target">The source object.</param>
        /// <returns>The masked value.</returns>
        public object? GetValue(object target)
        {
            var raw = inner.GetValue(target);
            if (raw is IEnumerable enumerable and not string)
            {
                var masked = new List<string>();
                foreach (var item in enumerable)
                {
                    masked.Add(maskingService.Mask(item, attr.Strategy, attr.Pattern, MaskingContextAccessor.Current) ?? "****");
                }

                return masked;
            }

            if (converter is not null && raw is not null)
            {
                using var sw = new StringWriter(CultureInfo.InvariantCulture);
                using var writer = new JsonTextWriter(sw);
                var serializer = JsonSerializer.CreateDefault();

                converter.WriteJson(writer, raw, serializer);
                writer.Flush();

                var rawJson = sw.ToString();

                if (rawJson.Length >= 2 && rawJson[0] == '"' && rawJson[^1] == '"')
                {
                    using var sr = new StringReader(rawJson);
                    using var reader = new JsonTextReader(sr);

                    // if we have DateTime value -> this will preserve the current culture
                    reader.DateParseHandling = DateParseHandling.None;
                    
                    if (reader.Read())
                    {
                        raw = reader.Value?.ToString();
                    }
                }
                else
                {
                    raw = rawJson;
                }
            }

            return maskingService.Mask(raw, attr.Strategy, attr.Pattern, MaskingContextAccessor.Current) ?? "****";
        }

        /// <summary>
        /// Sets the underlying value on the target object without additional processing.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(object target, object? value) => inner.SetValue(target, value);
    }
}
