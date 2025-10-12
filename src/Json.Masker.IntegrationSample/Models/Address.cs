using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents a physical mailing address.
/// </summary>
public class Address
{
    /// <summary>
    /// Gets or sets the first line of the street address.
    /// </summary>
    [Sensitive]
    public string Line1 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional second line of the street address.
    /// </summary>
    [Sensitive]
    public string? Line2 { get; set; }

    /// <summary>
    /// Gets or sets the city associated with the address.
    /// </summary>
    [Sensitive]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state or province for the address.
    /// </summary>
    [Sensitive]
    public string StateOrProvince { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the postal or ZIP code for the address.
    /// </summary>
    [Sensitive]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country for the address.
    /// </summary>
    [Sensitive]
    public string Country { get; set; } = string.Empty;
}
