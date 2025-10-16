using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// JSON converter that masks scalar values marked as sensitive.
/// </summary>
/// <typeparam name="T">The type being converted.</typeparam>
public sealed class MaskingScalarConverter<T> : JsonConverter<T>
{
    private readonly IMaskingService _maskingService;
    private readonly MaskingStrategy _strategy;
    private readonly string? _pattern;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaskingScalarConverter{T}"/> class.
    /// </summary>
    /// <param name="maskingService">The masking service used to mask values.</param>
    /// <param name="strategy">The strategy that determines how masking is applied.</param>
    /// <param name="pattern">Custom masking pattern to apply non-standard masking.</param>
    /// <param name="inner">An optional inner converter for delegating serialization.</param>
    public MaskingScalarConverter(
        IMaskingService maskingService,
        MaskingStrategy strategy,
        string? pattern)
    {
        _maskingService = maskingService;
        _strategy = strategy;
        _pattern = pattern;
    }

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => 
        typeToConvert == typeof(T);

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

        // short-circuit
        if (!ctx.Enabled) 
        {
            JsonSerializer.Serialize(writer, value, options);
            return;
        }

        var masked = // fallback, can't convert value to string
            value.TryConvertToString(out var valueAsString) ? 
            _maskingService.Mask(valueAsString ?? string.Empty, _strategy, _pattern) : 
            _maskingService.DefaultMask;

        writer.WriteStringValue(masked);
    }
}
