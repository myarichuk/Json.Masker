using System.Text;
using System.Text.RegularExpressions;

namespace Json.Masker.Abstract;

/// <summary>
/// Default implementation of <see cref="IMaskingService"/> that provides masking strategies for sensitive data.
/// </summary>
public sealed class DefaultMaskingService : IMaskingService
{
    // Precompiled regexes for efficiency.
    private static readonly Regex EmailRegex = new(
        @"^(?<user>[^@\s]+)@(?<domain>[^@\s]+)$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private static readonly Regex IbanRegex = new(
        @"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    /// <inheritdoc />
    public string Mask(object? value, MaskingStrategy strategy, string? pattern, MaskingContext ctx)
    {
        if (!ctx.Enabled || value is null)
        {
            return value?.ToString() ?? string.Empty;
        }

        var str = value.ToString() ?? string.Empty;
        
        if (pattern is not null)
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
                _ => "****",
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

    private static string MaskCreditCard(string raw)
    {
        var digits = NormalizeDigits(raw);
        if (digits.Length == 0)
        {
            return "****";
        }

        var span = digits.AsSpan();
        var last4 = span.Length >= 4 ? span[^4..] : span;
        return $"****-****-****-{last4}";
    }

    private static string MaskSsn(string raw)
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
    
    private static string MaskEmail(string raw)
    {
        var match = EmailRegex.Match(raw);
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

    private static string MaskIban(string raw)
    {
        var compact = raw.Replace(" ", string.Empty).ToUpperInvariant();
        if (!IbanRegex.IsMatch(compact))
        {
            return "****";
        }

        var visible = compact.Length >= 4 ? compact[^4..] : compact;
        return $"{compact[..2]}** **** **** **** {visible}";
    }    
}
