using Json.Masker.Abstract;
using Newtonsoft.Json.Serialization;

namespace Json.Masker.Newtonsoft;

/// <summary>
/// Wraps another <see cref="IValueProvider"/> to mask values before serialization.
/// </summary>
public sealed class MaskingValueProvider : IValueProvider
{
    private readonly IValueProvider _inner;
    private readonly IMaskingService _maskingService;
    private readonly SensitiveAttribute _attr;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaskingValueProvider"/> class.
    /// </summary>
    /// <param name="inner">The underlying value provider.</param>
    /// <param name="maskingService">The service that masks values.</param>
    /// <param name="attr">The attribute that describes the masking strategy.</param>
    public MaskingValueProvider(IValueProvider inner, IMaskingService maskingService, SensitiveAttribute attr)
    {
        _inner = inner;
        _maskingService = maskingService;
        _attr = attr;
    }

    /// <summary>
    /// Gets the masked value from the target object.
    /// </summary>
    /// <param name="target">The source object.</param>
    /// <returns>The masked value.</returns>
    public object? GetValue(object target)
    {
        var value = _inner.GetValue(target);
        var masked = _maskingService.Mask(value, _attr.Strategy, MaskingContextAccessor.Current);
        return masked;
    }

    /// <summary>
    /// Sets the underlying value on the target object.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="value">The value to assign.</param>
    public void SetValue(object target, object? value) => _inner.SetValue(target, value);
}
