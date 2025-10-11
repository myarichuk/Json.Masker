using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

public class Address
{
    [Sensitive]
    public string Line1 { get; set; } = string.Empty;

    [Sensitive]
    public string? Line2 { get; set; }

    [Sensitive]
    public string City { get; set; } = string.Empty;

    [Sensitive]
    public string StateOrProvince { get; set; } = string.Empty;

    [Sensitive]
    public string PostalCode { get; set; } = string.Empty;

    [Sensitive]
    public string Country { get; set; } = string.Empty;
}
