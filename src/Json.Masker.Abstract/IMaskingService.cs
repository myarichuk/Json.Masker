namespace Json.Masker.Abstract;

/// <summary>
/// Provides masking capabilities for sensitive values.
/// </summary>
public interface IMaskingService
{
    /// <summary>
    /// Produces a masked representation of the provided value using the supplied context and strategy.
    /// </summary>
    /// <param name="str">The value to mask.</param>
    /// <param name="strategy">The masking strategy to apply.</param>
    /// <param name="pattern">Custom masking pattern to apply.</param>
    /// <returns>The masked value.</returns>
    string Mask(ReadOnlySpan<char> str, MaskingStrategy strategy, string? pattern);

    /// <summary>
    /// Gets the default mask applied when no specific strategy is provided.
    /// </summary>
    string DefaultMask { get; }
}
