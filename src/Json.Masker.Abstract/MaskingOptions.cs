namespace Json.Masker.Abstract;

/// <summary>
/// Options used to configure JSON masking services.
/// </summary>
public sealed class MaskingOptions
{
    /// <summary>
    /// Gets the masking service that should be used.
    /// </summary>
    public IMaskingService? MaskingService { get; init; }
}
