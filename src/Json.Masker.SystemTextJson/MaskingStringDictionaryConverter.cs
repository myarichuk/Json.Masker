using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Masks values in dictionaries with string keys when masking is enabled for the current context.
/// </summary>
/// <typeparam name="TDict">The dictionary type being converted.</typeparam>
/// <typeparam name="TValue">The value type stored in the dictionary.</typeparam>
public class MaskingStringDictionaryConverter<TDict, TValue>(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    string? pattern)
    : JsonConverter<TDict>
    where TDict : IEnumerable<KeyValuePair<string, TValue>>
{
    private static readonly bool ValueIsScalar = typeof(TValue).IsProperPrimitive();

    /// <summary>
    /// Reads the dictionary by delegating to the default serializer behavior.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The target dictionary type.</param>
    /// <param name="options">Serializer options supplied by System.Text.Json.</param>
    /// <returns>The deserialized dictionary instance.</returns>
    public override TDict? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TDict>(ref reader, options);

    /// <summary>
    /// Writes the dictionary while masking scalar values when masking is enabled.
    /// </summary>
    /// <param name="writer">The target writer.</param>
    /// <param name="value">The dictionary to serialize.</param>
    /// <param name="options">Serializer options supplied by System.Text.Json.</param>
    public override void Write(Utf8JsonWriter writer, TDict value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var (k, v) in value ?? Enumerable.Empty<KeyValuePair<string, TValue>>())
        {
            writer.WritePropertyName(k);
            if (ValueIsScalar)
            {
                var masked = maskingService.Mask(v?.ToString(), strategy, pattern);
                writer.WriteStringValue(masked);
            }
            else
            {
                JsonSerializer.Serialize(writer, v, options);
            }
        }
        
        writer.WriteEndObject();
    }
}
