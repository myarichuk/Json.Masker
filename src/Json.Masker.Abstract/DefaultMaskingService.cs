using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace Json.Masker.Abstract;

/// <summary>
/// Default implementation of <see cref="IMaskingService"/> that provides masking strategies for sensitive data.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public partial class DefaultMaskingService : IMaskingService
{
    /// <inheritdoc/>
    public string DefaultMask => "****";

    /// <inheritdoc />
    public virtual string Mask(ReadOnlySpan<char> str, MaskingStrategy strategy, string? pattern)
    {
        
        if (!string.IsNullOrWhiteSpace(pattern))
        {
            return ApplyCustomPattern(str, pattern);
        }
        
        return str.IsEmpty
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
    
    /// <summary>
    /// Masks a credit card number, keeping the last four digits visible.
    /// </summary>
    /// <param name="raw">The raw credit card value to mask.</param>
    /// <returns>The masked credit card number.</returns>
    protected virtual string MaskCreditCard(ReadOnlySpan<char> raw)
    {
        Span<char> normalizedRaw = stackalloc char[raw.Length];
        var digitsCount = DigitNormalizer.Normalize(raw, normalizedRaw);
        var digits = normalizedRaw[..digitsCount];
        if (digits.Length == 0)
        {
            return DefaultMask;
        }
        
        var last4 = digits.Length >= 4 ? digits[^4..] : digits;
        return $"****-****-****-{last4}";
    }

    /// <summary>
    /// Masks a Social Security Number, revealing only the last four digits.
    /// </summary>
    /// <param name="raw">The raw SSN value to mask.</param>
    /// <returns>The masked SSN.</returns>
    protected virtual string MaskSsn(ReadOnlySpan<char> raw)
    {
        Span<char> normalizedRaw = stackalloc char[raw.Length];
        var digitsCount = DigitNormalizer.Normalize(raw, normalizedRaw);
        var digits = normalizedRaw[..digitsCount];
        if (digits.Length == 0)
        {
            return "***-**-****";
        }

        var last4 = digits.Length >= 4 ? digits[^4..] : digits;
        Span<char> properLast4 = stackalloc char[4];
        PadLeft(properLast4, last4, '*');
        return $"***-**-{properLast4}";
    }

    private static void PadLeft(Span<char> destination, ReadOnlySpan<char> source, char paddingChar)
    {
        if (source.Length > destination.Length)
        {
            throw new ArgumentException("Source is longer than destination.");
        }

        var padLength = destination.Length - source.Length;
        destination[..padLength].Fill(paddingChar);
        source.CopyTo(destination[padLength..]);
    }
    
    /// <summary>
    /// Masks an email address by obscuring parts of the user and domain sections.
    /// </summary>
    /// <param name="raw">The raw email value to mask.</param>
    /// <returns>The masked email address.</returns>
    protected virtual string MaskEmail(ReadOnlySpan<char> raw)
    {
        if (!TrySplitEmail(raw, out var user, out var domain))
        {
            return "****@****";
        }
        
        var maskedUserLength = user.IsEmpty ? 1 : (user.Length > 1 ? 1 + Math.Min(user.Length - 1, 5) : 1);

        // domain: mask only the first label; remainder unchanged
        var dot = domain.IndexOf('.');
        if (dot < 0)
        {
            // no dot => maskedUser + "@****"
            var total = maskedUserLength + 1 + 4;
            Span<char> buf = stackalloc char[total];
            var index = 0;

            buf[index++] = user.Length > 0 ? user[0] : '*';
            buf.Slice(index, maskedUserLength - 1).Fill('*');
            index += maskedUserLength - 1;

            buf[index++] = '@';
            buf[index++] = '*';
            buf[index++] = '*';
            buf[index++] = '*';
            buf[index] = '*';
            return new string(buf);
        }

        var beforeDot = domain[..dot];
        var afterDot = domain[dot..]; // should include '.'

        var maskedRestLength = beforeDot.Length switch
        {
            <= 1 => 1,
            <= 3 => 5, // first[0] then 4 stars
            _ => 1 + Math.Min(beforeDot.Length - 1, 4),
        };

        var totalLength = maskedUserLength + 1 + maskedRestLength + afterDot.Length;
        var buffer = totalLength <= 512 ? 
            stackalloc char[totalLength] : 
            new char[totalLength]; // unlikely but rare...;
        
        var outputIndex = 0;

        buffer[outputIndex++] = user.Length == 1 ? '*' : user.Length > 0 ? user[0] : '*';
        buffer.Slice(outputIndex, maskedUserLength - 1).Fill('*');
        outputIndex += maskedUserLength - 1;

        buffer[outputIndex++] = '@';

        buffer[outputIndex++] = beforeDot.Length > 0 ? beforeDot[0] : '*';
        buffer.Slice(outputIndex, maskedRestLength - 1).Fill('*');
        outputIndex += maskedRestLength - 1;

        // rest --> .foobar.com
        afterDot.CopyTo(buffer[outputIndex..]);
        outputIndex += afterDot.Length;

        return new string(buffer[..outputIndex]);
    }

    private static bool TrySplitEmail(ReadOnlySpan<char> email, out ReadOnlySpan<char> user, out ReadOnlySpan<char> domain)
    {
        user = default;
        domain = default;

        var at = email.IndexOf('@');
        if (at <= 0 || at >= email.Length - 1)
        {
            return false;
        }

        if (email.Slice(at + 1).IndexOf('@') >= 0)
        {
            return false;                 // must be exactly one '@'
        }

        if (email.IndexOfAny([' ', '\t', '\r', '\n']) >= 0)
        {
            return false;           // no whitespace
        }

        user = email[..at];
        domain = email[(at + 1)..];
        return true;
    }

    /// <summary>
    /// Masks an International Bank Account Number (IBAN), revealing only key sections.
    /// </summary>
    /// <param name="rawIban">The raw IBAN value to mask.</param>
    /// <returns>The masked IBAN.</returns>
    protected virtual string MaskIban(ReadOnlySpan<char> rawIban)
    {
        Span<char> tmp = stackalloc char[rawIban.Length];
        var n = NormalizeIban(rawIban, tmp); // remove spaces + uppercase
        var iban = tmp[..n];

        if (!LooksLikeIban(iban /* fast format check */))
        {
            return DefaultMask;
        }

        var tail = iban.Length >= 4 ? iban[^4..] : iban;

        // Output like: "CC** **** **** **** 1234"
        Span<char> outBuf = stackalloc char[2 + 18 + tail.Length];
        var k = 0;

        // country code
        outBuf[k++] = iban[0];
        outBuf[k++] = iban[1];

        // loop unroll :) "** **** **** **** "
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = ' ';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = ' ';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = ' ';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = '*';
        outBuf[k++] = ' ';
        tail.CopyTo(outBuf[k..]);
        k += tail.Length;

        return new string(outBuf[..k]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int NormalizeIban(ReadOnlySpan<char> src, Span<char> dst)
    {
        var c = 0;
        foreach (var ch in src)
        {
            if (ch == ' ')
            {
                continue;
            }

            dst[c++] = char.ToUpperInvariant(ch);
        }
        return c;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool LooksLikeIban(ReadOnlySpan<char> iban)
    {
        // IBAN length 15..34
        if ((uint)(iban.Length - 15) > 19u)
        {
            return false;
        }

        // CCdd...
        if ((uint)(iban[0] - 'A') > 25u || (uint)(iban[1] - 'A') > 25u)
        {
            return false;
        }

        if ((uint)(iban[2] - '0') > 9u || (uint)(iban[3] - '0') > 9u)
        {
            return false;
        }

        // all alnum ASCII
        foreach (var ch in iban)
        {
            var digit = (uint)(ch - '0') <= 9u;
            var upper = (uint)(ch - 'A') <= 25u;
            if (!(digit || upper))
            {
                return false;
            }
        }
        
        return true;
    }

    // the idea: input-driven masking, pattern applies to input, leftovers get masked, output never longer than input
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static string ApplyCustomPattern(ReadOnlySpan<char> input, ReadOnlySpan<char> pattern)
    {
        if (input.IsEmpty)
        {
            return string.Empty;
        }

        if (pattern.IsEmpty)
        {
            return new string(input);
        }

        var inLen = input.Length;
        var patLen = pattern.Length;
        var outLen = Math.Max(inLen, patLen);

        // TODO: rent array from pool!
        var buffer = outLen <= 512 ? stackalloc char[outLen] : new char[outLen];

        // note - those are managed "pointers"
        ref var in0 = ref MemoryMarshal.GetReference(input);
        ref var pat0 = ref MemoryMarshal.GetReference(pattern);
        ref var out0 = ref MemoryMarshal.GetReference(buffer);

        int i = 0, j = 0;

        for (; i < patLen && j < inLen; i++)
        {
            var p = Unsafe.Add(ref pat0, i);

            switch (p)
            {
                case '#':
                    Unsafe.Add(ref out0, i) = Unsafe.Add(ref in0, j++);
                    break;
                case '*':
                    Unsafe.Add(ref out0, i) = '*';
                    j++;
                    break;
                default:
                {
                    Unsafe.Add(ref out0, i) = p;
                    if (Unsafe.Add(ref in0, j) == p)
                    {
                        j++;
                    }

                    break;
                }
            }
        }

        if (i < inLen)
        {
            buffer[i..inLen].Fill('*');
            return new string(buffer[..inLen]);
        }

        return new string(buffer[..i]);
    }
    
    [GeneratedRegex(@"^(?<user>[^@\s]+)@(?<domain>[^@\s]+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();
    
    [GeneratedRegex(@"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex IbanRegex();
    
}
