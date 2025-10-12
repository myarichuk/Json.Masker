using Bogus;

namespace Json.Masker.IntegrationSample.Services;

/// <summary>
/// Provides helper methods that extend the <see cref="Faker"/> API with finance-related utilities.
/// </summary>
public static class FinanceExtensions
{
    private static readonly string[] Issuers = { "Visa", "MasterCard", "American Express", "Discover" };

    /// <summary>
    /// Picks a random credit card issuer from a predefined list.
    /// </summary>
    /// <param name="faker">The Faker instance to extend.</param>
    /// <returns>A random credit card issuer name.</returns>
    public static string CreditCardIssuer(this Faker faker)
        => faker.PickRandom(Issuers);
}
