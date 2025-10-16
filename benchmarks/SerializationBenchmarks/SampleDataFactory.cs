namespace SerializationBenchmarks;

/// <summary>
/// Provides factory methods for creating shared benchmark sample data.
/// </summary>
internal static class SampleDataFactory
{
    /// <summary>
    /// Creates a fresh <see cref="BenchmarkCustomer"/> used by each serialization scenario.
    /// </summary>
    public static BenchmarkCustomer CreateCustomer() => new()
    {
        Name = "Alice",
        CreditCard = "4111111111111234",
        SSN = "123-45-6789",
        Email = "alice@example.com",
        Iban = "DE89370400440532013000",
        Notes = "SensitiveValue",
        Age = "35",
        Hobbies =
        [
            "Knitting",
            "Skiing",
            "Gardening"
        ],
    };

    /// <summary>
    /// Creates a JSON payload representing <see cref="BenchmarkCustomer"/> for the JsonMasking benchmark.
    /// </summary>
    public static string CreateJsonMaskingPayload() => System.Text.Json.JsonSerializer.Serialize(CreateCustomer());

    /// <summary>
    /// Provides the target paths used by the JsonMasking library to redact fields.
    /// </summary>
    public static string[] CreateJsonMaskingTargets() =>
    [
        nameof(BenchmarkCustomer.CreditCard),
        nameof(BenchmarkCustomer.SSN),
        nameof(BenchmarkCustomer.Email),
        nameof(BenchmarkCustomer.Iban),
        nameof(BenchmarkCustomer.Notes),
        nameof(BenchmarkCustomer.Age),
        nameof(BenchmarkCustomer.Hobbies),
        $"{nameof(BenchmarkCustomer.Hobbies)}.*"
    ];
}
