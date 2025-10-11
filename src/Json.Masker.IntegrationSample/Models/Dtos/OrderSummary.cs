using Json.Masker.Abstract;
using Json.Masker.IntegrationSample.Models;

namespace Json.Masker.IntegrationSample.Models.Dtos;

public class OrderSummary
{
    public Guid Id { get; init; }
    [Sensitive]
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public DateTimeOffset OrderedAt { get; init; }
    public DateTimeOffset? FulfilledAt { get; init; }
    public decimal Total { get; init; }
    public string Currency { get; init; } = string.Empty;
    public Guid CustomerId { get; init; }
    [Sensitive]
    public string CustomerName { get; init; } = string.Empty;

    public static OrderSummary FromOrder(Order order)
    {
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
            CustomerName = order.CustomerName
        };
    }
}
