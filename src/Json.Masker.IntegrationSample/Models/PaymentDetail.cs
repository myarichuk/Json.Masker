using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

public class PaymentDetail
{
    public string Method { get; set; } = string.Empty;

    [Sensitive]
    public string CardIssuer { get; set; } = string.Empty;

    [Sensitive]
    public string CardLast4 { get; set; } = string.Empty;

    [Sensitive]
    public string TransactionReference { get; set; } = string.Empty;
    public DateTimeOffset? PaidAt { get; set; }
    public bool Refunded { get; set; }
    [Sensitive]
    public string CustomerEmail { get; set; } = string.Empty;
}
