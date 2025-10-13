using Json.Masker.Abstract;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Represents the customer contract used to exercise the default masking service.
/// </summary>
public sealed class SampleCustomer
{
    /// <summary>
    /// Gets or sets the customer's name, which remains unmasked.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's credit card number and masks all but the last four digits.
    /// </summary>
    [Sensitive(MaskingStrategy.Creditcard)]
    public string CreditCard { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's social security number and masks all but the last four digits.
    /// </summary>
    [Sensitive(MaskingStrategy.Ssn)]
    public string SSN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's age and masks the numeric value.
    /// </summary>
    [Sensitive]
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the customer's hobbies and redacts each entry.
    /// </summary>
    [Sensitive(MaskingStrategy.Redacted)]
    public List<string> Hobbies { get; set; } = ["Knitting", "Fishing", "Coding"];
}
