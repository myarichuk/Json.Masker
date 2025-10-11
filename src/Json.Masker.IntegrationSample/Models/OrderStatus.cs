namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents the processing states that an order can move through.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// The order has been created but not yet submitted.
    /// </summary>
    Draft,

    /// <summary>
    /// The order is awaiting payment confirmation.
    /// </summary>
    PendingPayment,

    /// <summary>
    /// The order has been paid for.
    /// </summary>
    Paid,

    /// <summary>
    /// The order is currently being processed.
    /// </summary>
    Processing,

    /// <summary>
    /// The order has been shipped to the customer.
    /// </summary>
    Shipped,

    /// <summary>
    /// The order has been fulfilled.
    /// </summary>
    Fulfilled,

    /// <summary>
    /// The order has been cancelled.
    /// </summary>
    Cancelled,
}
