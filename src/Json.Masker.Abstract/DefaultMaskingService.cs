namespace Json.Masker.Abstract;

public sealed class DefaultMaskingService : IMaskingService
{
    public string Mask(object? value, MaskingStrategy strategy, MaskingContext ctx)
    {
        return !ctx.Enabled || value is null
            ? value?.ToString() ?? string.Empty
            : strategy switch
            {
                MaskingStrategy.Creditcard => "****-****-****-" + value.ToString()!.TakeLast(4),
                MaskingStrategy.Ssn => "***-**-" + value.ToString()!.TakeLast(4),
                _ => "****",
            };
    }
}