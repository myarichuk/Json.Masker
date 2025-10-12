namespace Json.Masker.IntegrationSample.Models;

/// <summary>
/// Represents the lifecycle states that a customer account can be in.
/// </summary>
public enum CustomerStatus
{
    /// <summary>
    /// The customer has not yet converted and is considered a prospect.
    /// </summary>
    Prospect,

    /// <summary>
    /// The customer is active and in good standing.
    /// </summary>
    Active,

    /// <summary>
    /// The customer has been temporarily suspended.
    /// </summary>
    Suspended,

    /// <summary>
    /// The customer account has been closed.
    /// </summary>
    Closed,
}
