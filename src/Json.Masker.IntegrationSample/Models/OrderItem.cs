namespace Json.Masker.IntegrationSample.Models;

public class OrderItem
{
    public string Sku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal LineTotal => decimal.Round((UnitPrice - Discount) * Quantity, 2);
}
