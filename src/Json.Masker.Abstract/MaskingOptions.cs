namespace Json.Masker.Abstract;

/// <summary>
/// Options used to configure JSON masking services.
/// </summary>
public class MaskingOptions
{
    /// <summary>
    /// Gets or sets the masking service that should be used.
    /// </summary>
    public IMaskingService? MaskingService { get; set; }
}
