namespace Json.Masker.IntegrationSample.Models;

public class PaymentDetail
{
    public string Method { get; set; } = string.Empty;
    public string CardIssuer { get; set; } = string.Empty;
    public string CardLast4 { get; set; } = string.Empty;
    public string TransactionReference { get; set; } = string.Empty;
    public DateTimeOffset? PaidAt { get; set; }
    public bool Refunded { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
}
