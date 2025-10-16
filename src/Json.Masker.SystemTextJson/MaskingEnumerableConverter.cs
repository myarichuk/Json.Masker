using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Masks elements within enumerable collections when masking is enabled for the current context.
/// </summary>
/// <typeparam name="TCollection">The collection type to serialize.</typeparam>
/// <typeparam name="TElement">The element type stored in the collection.</typeparam>
public class MaskingEnumerableConverter<TCollection, TElement>(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    string? pattern)
    : JsonConverter<TCollection>
    where TCollection : IEnumerable<TElement>
{
    private static readonly bool ElementIsScalar = IsScalar(typeof(TElement));

    /// <summary>
    /// Reads the collection by delegating to the default serializer behavior.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The target collection type.</param>
    /// <param name="options">Serializer options supplied by System.Text.Json.</param>
    /// <returns>The deserialized collection.</returns>
    public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TCollection>(ref reader, options);

    /// <summary>
    /// Writes the collection while masking scalar elements when masking is enabled.
    /// </summary>
    /// <param name="writer">The target writer.</param>
    /// <param name="value">The collection to serialize.</param>
    /// <param name="options">Serializer options supplied by System.Text.Json.</param>
    public override void Write(Utf8JsonWriter writer, TCollection? value, JsonSerializerOptions options)
    {
        // short-circuit
        if (value is null)
        {
            writer.WriteStartArray();
            writer.WriteEndArray();
            return;
        }
        
        writer.WriteStartArray();
        foreach (var item in value)
        {
            if (ElementIsScalar && item.TryConvertToString(out var itemAsString))
            {
                var masked = maskingService.Mask(itemAsString, strategy, pattern);
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