namespace Json.Masker.Abstract;

/// <summary>
/// Defines the available masking strategies.
/// </summary>
public enum MaskingStrategy : byte
{
    /// <summary>
    /// Masks a value using the default strategy.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Masks a credit card number, preserving only the last four digits.
    /// </summary>
    Creditcard,

    /// <summary>
    /// Masks a social security number, preserving only the last four digits.
    /// </summary>
    Ssn,

    /// <summary>
    /// Replaces the value with a redacted placeholder.
    /// </summary>
    Redacted,
}
