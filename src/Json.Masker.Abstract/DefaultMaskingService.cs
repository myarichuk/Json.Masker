using System.Numerics;
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
        var digitsCount = NormalizeDigits(raw, normalizedRaw);
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
        var digitsCount = NormalizeDigits(raw, normalizedRaw);
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
        var match = EmailRegex().Match(new string(raw));
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

    /// <summary>
    /// Masks an International Bank Account Number (IBAN), revealing only key sections.
    /// </summary>
    /// <param name="raw">The raw IBAN value to mask.</param>
    /// <returns>The masked IBAN.</returns>
    protected virtual string MaskIban(ReadOnlySpan<char> raw)
    {
        Span<char> buffer = stackalloc char[raw.Length];
        var count = 0;
        foreach (var c in raw)
        {
            if (c != ' ')
            {
                buffer[count++] = char.ToUpperInvariant(c);
            }
        }
        
        var normalizedIban = buffer[..count];
        
        if (!IbanRegex().IsMatch(normalizedIban))
        {
            return DefaultMask;
        }

        var visible = normalizedIban.Length >= 4 ? normalizedIban[^4..] : normalizedIban;
        return $"{normalizedIban[..2]}** **** **** **** {visible}";
    }
    
    // the idea: input-driven masking, pattern applies to input, leftovers get masked, output never longer than input
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

        var maxLen = Math.Max(pattern.Length, input.Length);
        Span<char> buffer = stackalloc char[maxLen];

        var outputIndex = 0;
        var inputIndex = 0;

        for (; outputIndex < pattern.Length && inputIndex < input.Length; outputIndex++)
        {
            var p = pattern[outputIndex];
            switch (p)
            {
                case '#':
                    buffer[outputIndex] = input[inputIndex++];
                    break;

                case '*':
                    buffer[outputIndex] = '*';
                    inputIndex++;
                    break;

                default:
                    buffer[outputIndex] = p;
                    if (input[inputIndex] == p)
                    {
                        inputIndex++;
                    }

                    break;
            }
        }

        // remaining chars? mask them
        while (inputIndex < input.Length && outputIndex < buffer.Length)
        {
            buffer[outputIndex++] = '*';
            inputIndex++;
        }

        return new string(buffer[..outputIndex]);
    }

    [GeneratedRegex(@"^(?<user>[^@\s]+)@(?<domain>[^@\s]+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();
    
    [GeneratedRegex(@"^[A-Z]{2}\d{2}[A-Z0-9]{4}\d{7}([A-Z0-9]?){0,16}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex IbanRegex();
    
    internal static int NormalizeDigits(ReadOnlySpan<char> input, Span<char> outputBuffer)
    {
        if (outputBuffer.Length < input.Length)
        {
            throw new ArgumentException("Output buffer too small", nameof(outputBuffer));
        }
        
        var count = 0;

        if (Vector.IsHardwareAccelerated)
        {
            var zero = new Vector<ushort>('0');
            var nine = new Vector<ushort>('9');
            var simdLength = Vector<ushort>.Count;

            var i = 0;
            for (; i <= input.Length - simdLength; i += simdLength)
            {
                // load 8 or 16 chars at once
                var chunk = new Vector<ushort>(MemoryMarshal.Cast<char, ushort>(input.Slice(i, simdLength)));

                // true = 0xFFFF, false = 0x0000
                var geZero = Vector.GreaterThanOrEqual(chunk, zero);
                var leNine = Vector.LessThanOrEqual(chunk, nine);
                var mask = Vector.BitwiseAnd(geZero, leNine);

                for (var j = 0; j < simdLength; j++)
                {
                    if (mask[j] != 0)
                    {
                        outputBuffer[count++] = (char)chunk[j];
                    }
                }
            }

            // tail
            for (; i < input.Length; i++)
            {
                var c = input[i];
                if (c is >= '0' and <= '9')
                {
                    outputBuffer[count++] = c;
                }
            }
        }
        else
        {
            foreach (var c in input)
            {
                if (c is >= '0' and <= '9')
                {
                    outputBuffer[count++] = c;
                }
            }
        }

        return count;
    }    
}
