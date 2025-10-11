using Json.Masker.Abstract;
using Json.Masker.IntegrationSample.Models;

namespace Json.Masker.IntegrationSample.Models.Dtos;

public class CustomerOverview
{
    public Guid Id { get; init; }
    [Sensitive]
    public string FullName { get; init; } = string.Empty;

    [Sensitive(MaskingStrategy.Email)]
    public string Email { get; init; } = string.Empty;

    [Sensitive("***-***-####")]
    public string PhoneNumber { get; init; } = string.Empty;

    [Sensitive(MaskingStrategy.Ssn)]
    public string NationalId { get; init; } = string.Empty;

    [Sensitive("LOY-***-####")]
    public string LoyaltyNumber { get; init; } = string.Empty;
    public CustomerStatus Status { get; init; }
    public DateTimeOffset RegisteredAt { get; init; }
    public int OrderCount { get; init; }
    public decimal LifetimeValue { get; init; }
}
