using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents an order placed by a customer within the sample data set.
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the unique identifier for the order.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the human-readable order number.
    /// </summary>
    [Sensitive("SO-***###")]
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the customer that placed the order.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the customer that placed the order.
    /// </summary>
    [Sensitive]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the order was created.
    /// </summary>
    public DateTimeOffset OrderedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the order was fulfilled, if applicable.
    /// </summary>
    public DateTimeOffset? FulfilledAt { get; set; }

    /// <summary>
    /// Gets or sets the current status of the order.
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the collection of items included in the order.
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the payment details associated with the order.
    /// </summary>
    [Sensitive]
    public PaymentDetail Payment { get; set; } = new();

    /// <summary>
    /// Gets or sets the internal notes captured for the order.
    /// </summary>
    [Sensitive(MaskingStrategy.Redacted)]
    public string InternalReviewNotes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subtotal for the order before taxes.
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Gets or sets the tax amount applied to the order.
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// Gets or sets the total amount charged for the order.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets or sets the currency the order was processed in.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Gets or sets the fulfillment channel used to complete the order.
    /// </summary>
    public string FulfillmentChannel { get; set; } = string.Empty;
}
