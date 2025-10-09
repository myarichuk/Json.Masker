using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;
using Microsoft.IO;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// JSON converter that masks scalar values marked as sensitive.
/// </summary>
/// <typeparam name="T">The type being converted.</typeparam>
public sealed class MaskingScalarConverter<T> : JsonConverter<T>
{
    private static readonly RecyclableMemoryStreamManager StreamManager = new();

    private readonly IMaskingService _maskingService;
    private readonly MaskingStrategy _strategy;
    private readonly JsonConverter<T>? _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaskingScalarConverter{T}"/> class.
    /// </summary>
    /// <param name="maskingService">The masking service used to mask values.</param>
    /// <param name="strategy">The strategy that determines how masking is applied.</param>
    /// <param name="inner">An optional inner converter for delegating serialization.</param>
    public MaskingScalarConverter(
        IMaskingService maskingService,
        MaskingStrategy strategy,
        JsonConverter? inner = null)
    {
        _maskingService = maskingService;
        _strategy = strategy;
        _inner = inner as JsonConverter<T>;
    }

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) =>
        _inner?.CanConvert(typeToConvert) ?? true;

    /// <summary>
    /// Deserialization is not supported for sensitive values.
    /// </summary>
    /// <param name="reader">The reader that would supply JSON content.</param>
    /// <param name="typeToConvert">The type that would be deserialized.</param>
    /// <param name="options">The serializer options in use.</param>
    /// <returns>This method always throws a <see cref="NotSupportedException"/>.</returns>
    /// <exception cref="NotSupportedException">Always thrown because deserialization is not supported.</exception>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Sensitive values should not be deserialized");

    /// <summary>
    /// Writes a masked representation of the supplied value.
    /// </summary>
    /// <param name="writer">The writer that receives the masked value.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options in use.</param>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        var ctx = MaskingContextAccessor.Current;

        if (!ctx.Enabled)
        {
            if (_inner is not null)
            {
                _inner.Write(writer, value!, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, value, options);
            }

            return;
        }

        string masked;
        if (value is string valueAsString)
        {
            masked = _maskingService.Mask(valueAsString, _strategy, ctx);
        }
        else
        {
            using var buffer = StreamManager.GetStream();
            using var tmpWriter = new Utf8JsonWriter((Stream)buffer);

            if (_inner is not null)
            {
                _inner.Write(tmpWriter, value!, options);
            }
            else
            {
                JsonSerializer.Serialize(tmpWriter, value, options);
            }

            var rawJson = Encoding.UTF8.GetString(buffer.ToArray());
            masked = _maskingService.Mask(rawJson, _strategy, ctx);
        }

        // Always emit as string so the masked output is valid JSON
        writer.WriteStringValue(masked);
    }
}
