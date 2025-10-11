using Json.Masker.Abstract;
using Json.Masker.IntegrationSample.Models;

namespace Json.Masker.IntegrationSample.Models.Dtos;

public class OrderSummary
{
    public Guid Id { get; init; }
    [Sensitive("SO-***###")]
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public DateTimeOffset OrderedAt { get; init; }
    public DateTimeOffset? FulfilledAt { get; init; }
    public decimal Total { get; init; }
    public string Currency { get; init; } = string.Empty;
    public Guid CustomerId { get; init; }
    [Sensitive]
    public string CustomerName { get; init; } = string.Empty;

    [Sensitive(MaskingStrategy.Email)]
    public string CustomerEmail { get; init; } = string.Empty;

    [Sensitive("####-****-####")]
    public string TransactionReference { get; init; } = string.Empty;

    [Sensitive(MaskingStrategy.Creditcard)]
    public string? CardNumber { get; init; }

    [Sensitive(MaskingStrategy.Iban)]
    public string? BankAccountIban { get; init; }

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
            BankAccountIban = string.IsNullOrWhiteSpace(payment.BankAccountIban) ? null : payment.BankAccountIban
        };
    }
}
