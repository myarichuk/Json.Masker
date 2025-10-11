using System.Linq;
using Json.Masker.Abstract;

namespace Json.Masker.IntegrationSample.Models;

public class Customer
{
    public Guid Id { get; set; }
    [Sensitive]
    public string FirstName { get; set; } = string.Empty;

    [Sensitive]
    public string LastName { get; set; } = string.Empty;

    [Sensitive]
    public string Email { get; set; } = string.Empty;

    [Sensitive]
    public string PhoneNumber { get; set; } = string.Empty;

    [Sensitive]
    public Address BillingAddress { get; set; } = new();

    [Sensitive]
    public Address ShippingAddress { get; set; } = new();
    public DateTimeOffset RegisteredAt { get; set; }
    public CustomerStatus Status { get; set; }
    public List<Order> Orders { get; set; } = new();
    public decimal LifetimeValue => Orders.Sum(order => order.Total);
    public string FullName => $"{FirstName} {LastName}".Trim();
}
