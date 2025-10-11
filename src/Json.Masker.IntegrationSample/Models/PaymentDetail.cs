using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

public class PaymentDetail
{
    public string Method { get; set; } = string.Empty;

    [Sensitive]
    public string CardIssuer { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Creditcard)]
    public string CardNumber { get; set; } = string.Empty;

    public string CardLast4 =>
        string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 4
            ? string.Empty
            : CardNumber[^4..];

    [Sensitive("####-****-####")]
    public string TransactionReference { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Iban)]
    public string BankAccountIban { get; set; } = string.Empty;
    public DateTimeOffset? PaidAt { get; set; }
    public bool Refunded { get; set; }
    [Sensitive(MaskingStrategy.Email)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Redacted)]
    public string InternalNotes { get; set; } = string.Empty;
}
