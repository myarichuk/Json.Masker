namespace Json.Masker.Abstract;

public sealed class DefaultMaskingService : IMaskingService
{
    public string Mask(object? value, MaskingStrategy strategy, MaskingContext ctx)
    {
        if (!ctx.Enabled || value is null)
        {
            return value?.ToString() ?? string.Empty;
        }

        var str = value.ToString();
        return string.IsNullOrEmpty(str)
            ? string.Empty
            : strategy switch
            {
                MaskingStrategy.Creditcard => MaskCreditCard(str),
                MaskingStrategy.Ssn => MaskSsn(str),
                MaskingStrategy.Redacted => "<redacted>",
                _ => "****",
            };
    }

    private static string MaskCreditCard(string raw)
    {
        // avoid multiple substrings — slice from the back
        var span = raw.AsSpan();
        var last4 = span.Length >= 4 ? span[^4..] : span;
        return $"****-****-****-{last4}";
    }

    private static string MaskSsn(string raw)
    {
        var span = raw.AsSpan();
        var last4 = span.Length >= 4 ? span[^4..] : span;
        return $"***-**-{last4}";
    }
}
