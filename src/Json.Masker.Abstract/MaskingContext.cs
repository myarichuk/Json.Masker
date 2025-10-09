namespace Json.Masker.Abstract;

/// <summary>
/// Represents request-scoped information that influences masking behavior.
/// </summary>
public sealed class MaskingContext
{
    /// <summary>
    /// Gets a value indicating whether masking is enabled for the current context.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Gets the role associated with the current context, if any.
    /// </summary>
    public string? Role { get; init; }
}
