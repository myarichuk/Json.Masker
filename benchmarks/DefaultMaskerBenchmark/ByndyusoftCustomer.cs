using Byndyusoft.MaskedSerialization.Annotations.Attributes;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Represents the customer contract configured for the Byndyusoft masked serialization benchmarks.
/// </summary>
public sealed class ByndyusoftCustomer
{
    /// <summary>
    /// Gets or sets the customer's name, which remains unmasked.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's credit card number and masks it entirely during serialization.
    /// </summary>
    [Masked]
    public string CreditCard { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's social security number and masks it entirely during serialization.
    /// </summary>
    [Masked]
    public string SSN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's age, which is masked when serialized.
    /// </summary>
    [Masked]
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the customer's hobbies and masks the serialized collection.
    /// </summary>
    [Masked]
    public List<string> Hobbies { get; set; } = ["Knitting", "Fishing", "Coding"];
}
