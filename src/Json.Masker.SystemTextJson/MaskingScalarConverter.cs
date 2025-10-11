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
    private readonly JsonConverter<T>? _inner;
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
        string? pattern,
        JsonConverter? inner = null)
    {
        _maskingService = maskingService;
        _strategy = strategy;
        _pattern = pattern;
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
            masked = _maskingService.Mask(valueAsString, _strategy, _pattern, ctx);
        }
        else
        {
            using var buffer = new PooledByteBufferWriter();
            using var tmpWriter = new Utf8JsonWriter(buffer);

            if (_inner is not null)
            {
                _inner.Write(tmpWriter, value!, options);
            }
            else
            {
                JsonSerializer.Serialize(tmpWriter, value, options);
            }

            tmpWriter.Flush();

            var writtenSpan = buffer.WrittenSpan;
            string rawValue;

            if (writtenSpan.Length >= 2 && writtenSpan[0] == '"' && writtenSpan[^1] == '"')
            {
                rawValue = JsonSerializer.Deserialize<string>(writtenSpan) ?? string.Empty;
            }
            else
            {
                rawValue = Encoding.UTF8.GetString(writtenSpan);
            }

            masked = _maskingService.Mask(rawValue, _strategy, _pattern, ctx);
        }

        // Always emit as string so the masked output is valid JSON
        writer.WriteStringValue(masked);
    }

    private sealed class PooledByteBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private byte[] _buffer;
        private int _index;

        public PooledByteBufferWriter(int initialCapacity = 256)
        {
            _buffer = ArrayPool<byte>.Shared.Rent(initialCapacity);
        }

        public ReadOnlySpan<byte> WrittenSpan => _buffer.AsSpan(0, _index);

        public void Advance(int count)
        {
            _index += count;
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            EnsureCapacity(sizeHint);
            return _buffer.AsMemory(_index);
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            EnsureCapacity(sizeHint);
            return _buffer.AsSpan(_index);
        }

        public void Dispose()
        {
            var buffer = _buffer;
            _buffer = Array.Empty<byte>();
            if (buffer.Length > 0)
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private void EnsureCapacity(int sizeHint)
        {
            if (sizeHint < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeHint));
            }

            if (sizeHint == 0)
            {
                sizeHint = 1;
            }

            var required = _index + sizeHint;
            if (required <= _buffer.Length)
            {
                return;
            }

            var newSize = Math.Max(required, _buffer.Length * 2);
            var newBuffer = ArrayPool<byte>.Shared.Rent(newSize);
            _buffer.AsSpan(0, _index).CopyTo(newBuffer);
            ArrayPool<byte>.Shared.Return(_buffer);
            _buffer = newBuffer;
        }
    }
}
