namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents diagnostic information exposed by the sample error view.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the current request identifier.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request identifier should be displayed.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
