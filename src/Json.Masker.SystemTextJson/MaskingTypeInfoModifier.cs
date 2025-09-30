using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

public class MaskingTypeInfoModifier(IMaskingService maskingService)
{
    public void Modify(JsonTypeInfo typeInfo)
    {
        // TODO: profile this and check how much memory this wastes
        foreach (var prop in typeInfo.Properties)
        {
            var attr = prop.AttributeProvider?
                .GetCustomAttributes(typeof(SensitiveAttribute), inherit: true)
                .OfType<SensitiveAttribute>()
                .FirstOrDefault();

            if (attr is null)
            {
                continue;
            }

            // just in case, should never happen
            if (prop.Get == null)
            {
                continue;
            }
            
            var originalGetter = prop.Get;
            prop.Get = (obj) =>
            {
                var value = originalGetter(obj);
                return maskingService.Mask(value, attr.Strategy, MaskingContextAccessor.Current);
            };
        }
    }
}
