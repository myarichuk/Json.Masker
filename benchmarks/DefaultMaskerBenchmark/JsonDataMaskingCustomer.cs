using JsonDataMasking.Attributes;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Represents the customer contract configured for the JsonDataMasking benchmarks.
/// </summary>
public sealed class JsonDataMaskingCustomer
{
    /// <summary>
    /// Gets or sets the customer's name, which remains unmasked.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's credit card number and masks all but the last four digits.
    /// </summary>
    [SensitiveData(ShowLast = 4)]
    public string CreditCard { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's social security number and masks all but the last four digits.
    /// </summary>
    [SensitiveData(ShowLast = 4)]
    public string SSN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's age. The value is represented as a string to align with JsonDataMasking's supported types.
    /// </summary>
    [SensitiveData(SubstituteText = "***")]
    public string Age { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's hobbies and replaces each entry with a redacted placeholder.
    /// </summary>
    [SensitiveData(SubstituteText = "REDACTED")]
    public List<string> Hobbies { get; set; } = ["Knitting", "Fishing", "Coding"];
}
