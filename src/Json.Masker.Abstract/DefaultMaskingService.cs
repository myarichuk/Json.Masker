using System.Text;
using System.Text.RegularExpressions;

namespace Json.Masker.Abstract;

/// <summary>
/// Default implementation of <see cref="IMaskingService"/> that provides masking strategies for sensitive data.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public partial class DefaultMaskingService : IMaskingService
{
    /// <summary>
    /// The default mask applied when no specific strategy is provided.
    /// </summary>
    public virtual string DefaultMask => "****";

    /// <inheritdoc />
    public virtual string Mask(object? value, MaskingStrategy strategy, string? pattern, MaskingContext ctx)
    {
        if (!ctx.Enabled || value is null)
        {
            return value?.ToString() ?? string.Empty;
        }

        var str = value.ToString() ?? string.Empty;
        
        if (!string.IsNullOrWhiteSpace(pattern))
        {
            return ApplyCustomPattern(str, pattern);
        }
        
        return string.IsNullOrEmpty(str)
            ? string.Empty
            : strategy switch
            {
                MaskingStrategy.Creditcard => MaskCreditCard(str),
                MaskingStrategy.Ssn => MaskSsn(str),
                MaskingStrategy.Email => MaskEmail(str),
                MaskingStrategy.Iban => MaskIban(str),
                MaskingStrategy.Redacted => "<redacted>",
                _ => DefaultMask,
            };
    }
    
    private static string ApplyCustomPattern(string input, string pattern)
    {
        var sb = new StringBuilder(input.Length);
        int i = 0;

        foreach (var c in pattern)
        {
            if (i >= input.Length)
            {
                break;
            }

            switch (c)
            {
                case '#':
                    sb.Append(input[i++]);
                    break;
                case '*':
                    sb.Append('*');
                    i++;
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        while (i++ < input.Length)
        {
            sb.Append('*');
        }

        return sb.ToString();
    }
    
    private static string NormalizeDigits(ReadOnlySpan<char> raw)
    {
        var sb = new StringBuilder(raw.Length);
        foreach (var c in raw)
        {
            if (c is >= '0' and <= '9')
            {
                sb.Append(c);
            }
        }
        
        return sb.ToString();
    }    

    protected virtual string MaskCreditCard(string raw)
    {
        var digits = NormalizeDigits(raw);
        if (digits.Length == 0)
        {
            return DefaultMask;
        }

        var span = digits.AsSpan();
        var last4 = span.Length >= 4 ? span[^4..] : span;
        return $"****-****-****-{last4}";
    }

    protected virtual string MaskSsn(string raw)
    {
        var digits = NormalizeDigits(raw);
        if (digits.Length == 0)
        {
            return "***-**-****";
        }

        var span = digits.AsSpan();
        var last4 = span.Length >= 4 ? span[^4..] : span;

        return $"***-**-{last4.ToString().PadLeft(4, '*')}";
    }
    
    protected virtual string MaskEmail(string raw)
    {
        var match = EmailRegex().Match(raw);
        if (!match.Success)
        {
            return "****@****";
        }

        var user = match.Groups["user"].Value;
        var domain = match.Groups["domain"].Value;

        var maskedUser = user.Length > 1
            ? user[0] + new string('*', Math.Min(user.Length - 1, 5))
            : "*";

        var parts = domain.Split('.');
        if (parts.Length < 2)
        {
            return $"{maskedUser}@****";
        }

        var first = parts[0];
        var maskedFirst =
            first.Length switch
            {
                <= 1 => "*",
                <= 3 => first[0] + new string('*', 4),
                _ => first[0] + new string('*', Math.Min(first.Length - 1, 4)),
            };

        var maskedDomain = string.Join('.', new[] { maskedFirst }.Concat(parts.Skip(1)));

        return $"{maskedUser}@{maskedDomain}";
    }

    protected virtual string MaskIban(string raw)
    {
        var compact = raw.Replace(" ", string.Empty).ToUpperInvariant();
        if (!IbanRegex().IsMatch(compact))
        {
            return DefaultMask;
        }

        var visible = compact.Length >= 4 ? compact[^4..] : compact;
        return $"{compact[..2]}** **** **** **** {visible}";
    }

    [GeneratedRegex(@"^(?<user>[^@\s]+)@(?<domain>[^@\s]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();
    
    [GeneratedRegex(@"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex IbanRegex();
}
