using System.Collections;
using System.Reflection;
using Json.Masker.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Json.Masker.Newtonsoft;

public class MaskingContractResolver(IMaskingService maskingService) : DefaultContractResolver
{
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

    internal class MaskingCollectionValueProvider(
        IValueProvider inner,
        IMaskingService maskingService,
        SensitiveAttribute attr)
        : IValueProvider
    {
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

        public void SetValue(object target, object? value) => inner.SetValue(target, value);
    }
}
