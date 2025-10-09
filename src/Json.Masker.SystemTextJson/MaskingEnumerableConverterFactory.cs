using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Produces converters that mask enumerable values marked as sensitive.
/// </summary>
/// <param name="maskingService">The masking service used to mask values.</param>
/// <param name="strategy">The masking strategy to apply.</param>
/// <param name="originalConverter">An optional original converter to consult for compatibility.</param>
public class MaskingEnumerableConverterFactory(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    JsonConverter? originalConverter = null)
    : JsonConverterFactory
{
    /// <summary>
    /// Determines whether the factory can create a converter for the provided type.
    /// </summary>
    /// <param name="typeToConvert">The type that is being evaluated.</param>
    /// <returns><see langword="true"/> when the type is a supported enumerable; otherwise, <see langword="false"/>.</returns>
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

    /// <summary>
    /// Creates a masking converter for the specified enumerable type.
    /// </summary>
    /// <param name="type">The enumerable type to convert.</param>
    /// <param name="options">The serializer options in use.</param>
    /// <returns>A masking converter for the given enumerable type.</returns>
    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var elementType = type.IsArray
            ? type.GetElementType()!
            : type.GetGenericArguments().FirstOrDefault() ?? typeof(object);

        var converterType = typeof(MaskingEnumerableConverter<>).MakeGenericType(elementType);
        return (JsonConverter)Activator.CreateInstance(converterType, maskingService, strategy)!;
    }
}