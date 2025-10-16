using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

public class MaskingStringDictionaryConverter<TDict, TValue>(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    string? pattern)
    : JsonConverter<TDict>
    where TDict : IEnumerable<KeyValuePair<string, TValue>>
{
    private static readonly bool ValueIsScalar = typeof(TValue).IsProperPrimitive();

    public override TDict? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TDict>(ref reader, options);

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