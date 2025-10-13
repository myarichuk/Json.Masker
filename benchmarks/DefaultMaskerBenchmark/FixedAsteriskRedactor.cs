using System;
using Microsoft.Extensions.Compliance.Redaction;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Provides a simple redactor that replaces any input with a fixed asterisk mask.
/// </summary>
public sealed class FixedAsteriskRedactor : Redactor
{
    private const string Mask = "***";

    /// <inheritdoc />
    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        Mask.AsSpan().CopyTo(destination);
        return Mask.Length;
    }

    /// <inheritdoc />
    public override int GetRedactedLength(ReadOnlySpan<char> input) => Mask.Length;
}
