using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents payment information captured for an order.
/// </summary>
public class PaymentDetail
{
    /// <summary>
    /// Gets or sets the payment method used for the transaction.
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the issuer of the payment card, when applicable.
    /// </summary>
    [Sensitive]
    public string CardIssuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment card number.
    /// </summary>
    [Sensitive(MaskingStrategy.Creditcard)]
    public string CardNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the last four digits of the card number, when available.
    /// </summary>
    public string CardLast4 =>
        string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 4
            ? string.Empty
            : CardNumber[^4..];

    /// <summary>
    /// Gets or sets the transaction reference for the payment.
    /// </summary>
    [Sensitive("####-****-####")]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the International Bank Account Number associated with the payment.
    /// </summary>
    [Sensitive(MaskingStrategy.Iban)]
    public string BankAccountIban { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp indicating when the payment was captured.
    /// </summary>
    public DateTimeOffset? PaidAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the payment was refunded.
    /// </summary>
    public bool Refunded { get; set; }

    /// <summary>
    /// Gets or sets the customer's email address associated with the payment.
    /// </summary>
    [Sensitive(MaskingStrategy.Email)]
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets internal notes related to the payment.
    /// </summary>
    [Sensitive(MaskingStrategy.Redacted)]
    public string InternalNotes { get; set; } = string.Empty;
}
