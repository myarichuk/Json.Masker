using System.Collections;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Applies masking converters to properties that are marked with <see cref="SensitiveAttribute"/>.
/// </summary>
/// <param name="maskingService">The masking service that will generate masked values.</param>
public class MaskingTypeInfoModifier(IMaskingService maskingService)
{
    /// <summary>
    /// Configures the provided <see cref="JsonTypeInfo"/> to use masking converters for sensitive members.
    /// </summary>
    /// <param name="typeInfo">The type information to modify.</param>
    public void Modify(JsonTypeInfo typeInfo)
    {
        foreach (var prop in typeInfo.Properties)
        {
            var attr = prop.AttributeProvider?
                .GetCustomAttributes(typeof(SensitiveAttribute), true)
                .OfType<SensitiveAttribute>()
                .FirstOrDefault();

            if (attr is null)
            {
                continue;
            }

            if (IsCollection(prop.PropertyType))
            {
                prop.CustomConverter =
                    new MaskingEnumerableConverterFactory(
                            maskingService, attr.Strategy, prop.CustomConverter)
                        .CreateConverter(prop.PropertyType, typeInfo.Options);
            }
            else
            {
                var convType = typeof(MaskingScalarConverter<>).MakeGenericType(prop.PropertyType);
                prop.CustomConverter = (JsonConverter)Activator.CreateInstance(
                    convType, maskingService, attr.Strategy, prop.CustomConverter)!;
            }
        }
    }
    
    private static bool IsCollection(Type t) =>
        typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string);
}