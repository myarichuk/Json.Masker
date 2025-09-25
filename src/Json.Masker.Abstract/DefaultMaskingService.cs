namespace Json.Masker.Abstract;



public sealed class DefaultMaskingService : IMaskingService
{
    public string Mask(object? value, string strategy, MaskingContext ctx)
    {
        return !ctx.Enabled || value is null
            ? value?.ToString() ?? string.Empty
            : strategy switch
            {
                "creditcard" => "****-****-****-" + value.ToString()!.TakeLast(4),
                "ssn" => "***-**-" + value.ToString()!.TakeLast(4),
                _ => "****",
            };
    }
}