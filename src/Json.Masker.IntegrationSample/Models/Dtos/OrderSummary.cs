namespace Json.Masker.IntegrationSample.Models.Dtos;

using Json.Masker.IntegrationSample.Models;

public class OrderSummary
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public DateTimeOffset OrderedAt { get; init; }
    public DateTimeOffset? FulfilledAt { get; init; }
    public decimal Total { get; init; }
    public string Currency { get; init; } = string.Empty;
    public Guid CustomerId { get; init; }
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
