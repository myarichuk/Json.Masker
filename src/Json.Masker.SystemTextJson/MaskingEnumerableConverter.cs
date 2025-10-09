using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// JSON converter that masks sensitive values within enumerable collections.
/// </summary>
/// <typeparam name="T">The element type of the enumerable.</typeparam>
/// <param name="maskingService">The masking service used to produce masked values.</param>
/// <param name="strategy">The strategy that determines how masking is applied.</param>
public class MaskingEnumerableConverter<T>(
    IMaskingService maskingService,
    MaskingStrategy strategy)
    : JsonConverter<IEnumerable<T>>
{
    /// <summary>
    /// Deserialization is not supported for sensitive values.
    /// </summary>
    /// <param name="reader">The reader that would supply JSON content.</param>
    /// <param name="typeToConvert">The type that would be deserialized.</param>
    /// <param name="options">The serializer options in use.</param>
    /// <returns>This method always throws a <see cref="NotSupportedException"/>.</returns>
    /// <exception cref="NotSupportedException">Always thrown because deserialization is not supported.</exception>
    public override IEnumerable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Sensitive values should not be deserialized");

    /// <summary>
    /// Writes the masked representation of the enumerable to the provided writer.
    /// </summary>
    /// <param name="writer">The writer to receive the masked values.</param>
    /// <param name="value">The enumerable containing potentially sensitive values.</param>
    /// <param name="options">The serializer options in use.</param>
    public override void Write(Utf8JsonWriter writer, IEnumerable<T>? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        foreach (var v in value)
        {
            var masked = maskingService.Mask(v, strategy, MaskingContextAccessor.Current);
            writer.WriteStringValue(masked);
        }

        writer.WriteEndArray();
    }
}
