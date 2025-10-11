using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

public class Order
{
    public Guid Id { get; set; }
    [Sensitive("SO-***###")]
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    [Sensitive]
    public string CustomerName { get; set; } = string.Empty;
    public DateTimeOffset OrderedAt { get; set; }
    public DateTimeOffset? FulfilledAt { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    [Sensitive]
    public PaymentDetail Payment { get; set; } = new();

    [Sensitive(MaskingStrategy.Redacted)]
    public string InternalReviewNotes { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; } = "USD";
    public string FulfillmentChannel { get; set; } = string.Empty;
}
