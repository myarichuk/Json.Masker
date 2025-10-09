namespace Json.Masker.Abstract;

/// <summary>
/// Provides masking capabilities for sensitive values.
/// </summary>
public interface IMaskingService
{
    /// <summary>
    /// Produces a masked representation of the provided value using the supplied context and strategy.
    /// </summary>
    /// <param name="value">The value to mask.</param>
    /// <param name="strategy">The masking strategy to apply.</param>
    /// <param name="ctx">The contextual information about the current masking request.</param>
    /// <returns>The masked value.</returns>
    string Mask(object? value, MaskingStrategy strategy, MaskingContext ctx);
}
