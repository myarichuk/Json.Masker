using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

public class MaskingEnumerableConverter<TCollection, TElement>(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    string? pattern)
    : JsonConverter<TCollection>
    where TCollection : IEnumerable<TElement>
{
    private static readonly bool ElementIsScalar = IsScalar(typeof(TElement));

    public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TCollection>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value ?? Enumerable.Empty<TElement>())
        {
            if (ElementIsScalar)
            {
                var masked = maskingService.Mask(item?.ToString(), strategy, pattern);
                writer.WriteStringValue(masked);
            }
            else
            {
                JsonSerializer.Serialize(writer, item, options);
            }
        }
        
        writer.WriteEndArray();
    }

    private static bool IsScalar(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        return t.IsPrimitive || t == typeof(string) || t == typeof(decimal)
               || t == typeof(DateTime) || t == typeof(DateTimeOffset) || t == typeof(Guid) || t.IsEnum;
    }
}