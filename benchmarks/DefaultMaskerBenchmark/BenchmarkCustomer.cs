using System.Collections.Generic;
using Byndyusoft.MaskedSerialization.Annotations.Attributes;
using Json.Masker.Abstract;
using JsonDataMasking.Attributes;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Shared DTO used across serialization benchmarks so each framework masks identical payloads.
/// </summary>
public sealed class BenchmarkCustomer
{
    /// <summary>
    /// Gets or sets the customer's name, which is never masked.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's credit card number. Only the last four digits should remain visible.
    /// </summary>
    [Sensitive(MaskingStrategy.Creditcard)]
    [Masked]
    [SensitiveData(ShowLast = 4)]
    public string CreditCard { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's social security number. Only the last four digits should remain visible.
    /// </summary>
    [Sensitive(MaskingStrategy.Ssn)]
    [Masked]
    [SensitiveData(ShowLast = 4)]
    public string SSN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's email address. The user and domain segments are masked during serialization.
    /// </summary>
    [Sensitive(MaskingStrategy.Email)]
    [Masked]
    [SensitiveData(SubstituteText = "***@***.***")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's International Bank Account Number (IBAN).
    /// </summary>
    [Sensitive(MaskingStrategy.Iban)]
    [Masked]
    [SensitiveData(ShowLast = 4)]
    public string Iban { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's personal notes. These are masked using the default strategy.
    /// </summary>
    [Sensitive]
    [Masked]
    [SensitiveData(SubstituteText = "***")]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's age. JsonDataMasking works with string payloads, so the value is modeled as text.
    /// </summary>
    [Sensitive]
    [Masked]
    [SensitiveData(SubstituteText = "***")]
    public string Age { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's hobbies. Each entry is redacted.
    /// </summary>
    [Sensitive(MaskingStrategy.Redacted)]
    [Masked]
    [SensitiveData(SubstituteText = "REDACTED")]
    public List<string> Hobbies { get; set; } = new();
}
