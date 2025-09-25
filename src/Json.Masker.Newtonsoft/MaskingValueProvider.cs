using Json.Masker.Abstract;
using Newtonsoft.Json.Serialization;

namespace Json.Masker.Newtonsoft;

public sealed class MaskingValueProvider : IValueProvider
{
    private readonly IValueProvider _inner;
    private readonly IMaskingService _maskingService;
    private readonly SensitiveAttribute _attr;

    public MaskingValueProvider(IValueProvider inner, IMaskingService maskingService, SensitiveAttribute attr)
    {
        _inner = inner;
        _maskingService = maskingService;
        _attr = attr;
    }

    public object? GetValue(object target)
    {
        var value = _inner.GetValue(target);
        var masked = _maskingService.Mask(value, _attr.Strategy, MaskingContextAccessor.Current);
        return masked;
    }

    public void SetValue(object target, object? value) => _inner.SetValue(target, value);
}