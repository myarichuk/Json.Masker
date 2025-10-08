using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

public class MaskingEnumerableConverterFactory(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    JsonConverter? originalConverter = null)
    : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (originalConverter != null && !originalConverter.CanConvert(typeToConvert))
        {
            return false;
        }

        return typeToConvert != typeof(string) &&
               typeof(System.Collections.IEnumerable)
                   .IsAssignableFrom(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var elementType = type.IsArray
            ? type.GetElementType()!
            : type.GetGenericArguments().FirstOrDefault() ?? typeof(object);

        var converterType = typeof(MaskingEnumerableConverter<>).MakeGenericType(elementType);
        return (JsonConverter)Activator.CreateInstance(converterType, maskingService, strategy)!;
    }
}