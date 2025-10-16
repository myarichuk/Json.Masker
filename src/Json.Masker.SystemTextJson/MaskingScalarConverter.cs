using System;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

public class MaskingScalarConverter<T>(
    IMaskingService maskingService,
    MaskingStrategy strategy,
    string? pattern,
    JsonConverter<T>? inner = null)
    : JsonConverter<T>
{
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        var ctx = MaskingContextAccessor.Current;

        if (!ctx.Enabled)
        {
            if (inner != null)
            {
                inner.Write(writer, value!, options);
            }
            else
            {
                // fallback
                JsonSerializer.Serialize(writer, value, typeof(T), options);
            }

            return;
        }

        if (value is null)
        {
            writer.WriteStringValue(maskingService.DefaultMask);
            return;
        }

        string canonical;

        if (inner is not null)
        {
            MaskViaInner(writer, value, inner, maskingService, strategy, pattern, options);
        }
        else
        {
            // just in case
            if (value is string s)
            {
                canonical = s;
            }
            else if (value.TryConvertToString(out var vs))
            {
                canonical = vs ?? string.Empty;
            }
            else
            {
                canonical = value.ToString() ?? string.Empty;
            }
    
            var masked = maskingService.Mask(canonical, strategy, pattern);
            writer.WriteStringValue(masked);
        }
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Deserialization not supported for a sensitive type");

    private static void MaskViaInner(
        Utf8JsonWriter writer,
        T value,
        JsonConverter<T> inner,
        IMaskingService maskingService,
        MaskingStrategy strategy,
        string? pattern,
        JsonSerializerOptions options)
    {
        // Serialize the value into a pooled UTF-8 buffer.
        var buffer = ArrayPool<byte>.Shared.Rent(512);
        var rented = buffer;
        try
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            var spanWriter =
                new Utf8JsonWriter(
                    bufferWriter); // replace with custom IBufferWriter if you really care
            inner.Write(spanWriter, value, options);
            spanWriter.Flush();
            var utf8 = bufferWriter.WrittenSpan;

            var reader = new Utf8JsonReader(utf8, true, default);
            if (!reader.Read())
            {
                writer.WriteStringValue(maskingService.DefaultMask);
                return;
            }

            ReadOnlySpan<byte> utf8Value;
            string masked;

            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    utf8Value = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                    
                    // decode to stackalloc buffer
                    Span<char> chars = stackalloc char[utf8Value.Length]; // upper bound: 1 char per byte
                    int written = Encoding.UTF8.GetChars(utf8Value, chars);
                    masked = maskingService.Mask(chars[..written], strategy, pattern);
                    break;

                case JsonTokenType.Number:
                case JsonTokenType.True:
                case JsonTokenType.False:
                case JsonTokenType.Null:
                    utf8Value = reader.HasValueSequence ? 
                        reader.ValueSequence.ToArray() : reader.ValueSpan;
                    
                    Span<char> digits = stackalloc char[utf8Value.Length];
                    written = Encoding.UTF8.GetChars(utf8Value, digits);
                    masked = maskingService.Mask(digits[..written], strategy, pattern);
                    break;

                default:
                    // non-scalar!
                    masked = maskingService.DefaultMask;
                    break;
            }

            writer.WriteStringValue(masked);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented);
        }
    }

}
