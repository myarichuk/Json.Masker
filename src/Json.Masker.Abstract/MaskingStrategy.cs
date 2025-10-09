namespace Json.Masker.Abstract;

/// <summary>
/// Defines the available masking strategies for sensitive data values.
/// </summary>
public enum MaskingStrategy : byte
{
    /// <summary>
    /// The default masking strategy. Applies a generic mask (e.g. "****") without format-specific logic.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Masks a credit card number, preserving only the last four digits.
    /// Non-numeric characters are ignored during masking.
    /// </summary>
    Creditcard,

    /// <summary>
    /// Masks a social security number (SSN), preserving only the last four digits.
    /// Non-numeric characters are ignored during masking.
    /// </summary>
    Ssn,

    /// <summary>
    /// Replaces the value entirely with a redacted placeholder ("&lt;redacted&gt;").
    /// </summary>
    Redacted,

    /// <summary>
    /// Masks an email address, preserving part of the username and the domain suffix (e.g. "j****@d***.com").
    /// </summary>
    Email,

    /// <summary>
    /// Masks an International Bank Account Number (IBAN), preserving the country code and last four characters.
    /// </summary>
    Iban,
}