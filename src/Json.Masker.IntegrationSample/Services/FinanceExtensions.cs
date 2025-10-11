using Bogus;

namespace Json.Masker.IntegrationSample.Services;

public static class FinanceExtensions
{
    private static readonly string[] Issuers = { "Visa", "MasterCard", "American Express", "Discover" };

    public static string CreditCardIssuer(this Faker faker)
        => faker.PickRandom(Issuers);
}