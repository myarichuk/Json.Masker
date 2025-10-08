using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Masker.Abstract;
using Microsoft.IO;

namespace Json.Masker.SystemTextJson;

public sealed class MaskingScalarConverter<T> : JsonConverter<T>
{
    private readonly IMaskingService _maskingService;
    private readonly MaskingStrategy _strategy;
    private readonly JsonConverter<T>? _inner;

    public MaskingScalarConverter(IMaskingService maskingService,
        MaskingStrategy strategy,
        JsonConverter? inner = null)
    {
        _maskingService = maskingService;
        _strategy = strategy;
        _inner = inner as JsonConverter<T>;
    }

    private static readonly RecyclableMemoryStreamManager _streamManager = new();
    
    public override bool CanConvert(Type typeToConvert) =>
        _inner?.CanConvert(typeToConvert) ?? true;

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new NotSupportedException("Sensitive values should not be deserialized");
    
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
            using var buffer = _streamManager.GetStream();
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
