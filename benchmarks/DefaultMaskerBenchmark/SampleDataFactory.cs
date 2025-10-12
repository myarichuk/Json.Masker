namespace DefaultMaskerBenchmark;

/// <summary>
/// Provides factory methods for creating shared benchmark sample data.
/// </summary>
internal static class SampleDataFactory
{
    /// <summary>
    /// Creates the <see cref="SampleCustomer"/> instance used by the default masking service benchmarks.
    /// </summary>
    public static SampleCustomer CreateSampleCustomer() => new()
    {
        Name = "Alice",
        CreditCard = "4111111111111234",
        SSN = "123456789",
        Age = 35,
        Hobbies =
        [
            "Knitting",
            "Skiing",
            "Gardening"
        ],
    };

    /// <summary>
    /// Creates the <see cref="JsonDataMaskingCustomer"/> instance used by the JsonDataMasking comparison benchmark.
    /// </summary>
    public static JsonDataMaskingCustomer CreateJsonDataMaskingCustomer() => new()
    {
        Name = "Alice",
        CreditCard = "4111111111111234",
        SSN = "123456789",
        Age = "35",
        Hobbies =
        [
            "Knitting",
            "Skiing",
            "Gardening"
        ],
    };
}
