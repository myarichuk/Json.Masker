namespace Json.Masker.IntegrationSample.Models.Dtos;

using Json.Masker.IntegrationSample.Models;

public class CustomerOverview
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public CustomerStatus Status { get; init; }
    public DateTimeOffset RegisteredAt { get; init; }
    public int OrderCount { get; init; }
    public decimal LifetimeValue { get; init; }
}
