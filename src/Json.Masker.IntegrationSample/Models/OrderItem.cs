namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents a single line item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Gets or sets the stock keeping unit for the item.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity ordered for the item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price for the item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to the item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets the total value of the line item after discounts.
    /// </summary>
    public decimal LineTotal => decimal.Round((UnitPrice - Discount) * Quantity, 2);
}
