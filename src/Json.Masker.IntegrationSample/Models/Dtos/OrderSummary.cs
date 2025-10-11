using Json.Masker.Abstract;
using Json.Masker.IntegrationSample.Models;

namespace Json.Masker.IntegrationSample.Models.Dtos;

/// <summary>
/// Represents a summary view of an order suitable for list endpoints.
/// </summary>
public class OrderSummary
{
    /// <summary>
    /// Gets the unique identifier for the order.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the human-readable order number.
    /// </summary>
    [Sensitive("SO-***###")]
    public string OrderNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the current status of the order.
    /// </summary>
    public OrderStatus Status { get; init; }

    /// <summary>
    /// Gets the timestamp when the order was placed.
    /// </summary>
    public DateTimeOffset OrderedAt { get; init; }

    /// <summary>
    /// Gets the timestamp when the order was fulfilled, if available.
    /// </summary>
    public DateTimeOffset? FulfilledAt { get; init; }

    /// <summary>
    /// Gets the total value of the order.
    /// </summary>
    public decimal Total { get; init; }

    /// <summary>
    /// Gets the currency associated with the order.
    /// </summary>
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Gets the identifier of the customer that placed the order.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets the name of the customer that placed the order.
    /// </summary>
    [Sensitive]
    public string CustomerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the customer's email address associated with the order.
    /// </summary>
    [Sensitive(MaskingStrategy.Email)]
    public string CustomerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets the transaction reference recorded for the order's payment.
    /// </summary>
    [Sensitive("####-****-####")]
    public string TransactionReference { get; init; } = string.Empty;

    /// <summary>
    /// Gets the payment card number associated with the order, when available.
    /// </summary>
    [Sensitive(MaskingStrategy.Creditcard)]
    public string? CardNumber { get; init; }

    /// <summary>
    /// Gets the IBAN associated with the payment, when available.
    /// </summary>
    [Sensitive(MaskingStrategy.Iban)]
    public string? BankAccountIban { get; init; }

    /// <summary>
    /// Creates a <see cref="OrderSummary"/> from a full <see cref="Order"/> instance.
    /// </summary>
    /// <param name="order">The order to convert.</param>
    /// <returns>A populated <see cref="OrderSummary"/>.</returns>
    public static OrderSummary FromOrder(Order order)
    {
        var payment = order.Payment ?? new PaymentDetail();

        return new OrderSummary
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            OrderedAt = order.OrderedAt,
            FulfilledAt = order.FulfilledAt,
            Total = order.Total,
            Currency = order.Currency,
            CustomerId = order.CustomerId,
            CustomerName = order.CustomerName,
            CustomerEmail = payment.CustomerEmail,
            TransactionReference = payment.TransactionReference,
            CardNumber = string.IsNullOrWhiteSpace(payment.CardNumber) ? null : payment.CardNumber,
            BankAccountIban = string.IsNullOrWhiteSpace(payment.BankAccountIban) ? null : payment.BankAccountIban,
        };
    }
}
