namespace Json.Masker.Abstract;

public sealed class DefaultMaskingService : IMaskingService
{
    public string Mask(object? value, string strategy, MaskingContext ctx)
    {
        if (!ctx.Enabled || value is null) return value?.ToString() ?? string.Empty;

        return strategy switch
        {
            "creditcard" => "****-****-****-" + value.ToString()!.TakeLast(4),
            "ssn"        => "***-**-" + value.ToString()!.TakeLast(4),
            _            => "****"
        };
    }
}