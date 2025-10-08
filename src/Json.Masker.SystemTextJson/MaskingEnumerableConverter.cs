using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

public class MaskingEnumerableConverter<T>(
    IMaskingService maskingService,
    MaskingStrategy strategy)
    : JsonConverter<IEnumerable<T>>
{
    public override IEnumerable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Sensitive values should not be deserialized");

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