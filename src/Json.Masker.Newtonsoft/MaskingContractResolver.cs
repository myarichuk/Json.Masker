using System.Collections;
using System.Collections.Generic;
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
            prop.ValueProvider = new MaskingCollectionValueProvider(inner, maskingService, sensitiveAttr);

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
    internal class MaskingCollectionValueProvider(
        IValueProvider inner,
        IMaskingService maskingService,
        SensitiveAttribute attr)
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
                    masked.Add(maskingService.Mask(item, attr.Strategy, MaskingContextAccessor.Current) ?? "****");
                }

                return masked;
            }

            return maskingService.Mask(raw, attr.Strategy, MaskingContextAccessor.Current) ?? "****";
        }

        /// <summary>
        /// Sets the underlying value on the target object without additional processing.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(object target, object? value) => inner.SetValue(target, value);
    }
}
