namespace Json.Masker.Abstract;

/// <summary>
/// Identifies members whose values should be masked when serialized.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SensitiveAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SensitiveAttribute"/> class.
    /// </summary>
    /// <param name="strategy">The masking strategy to apply.</param>
    public SensitiveAttribute(MaskingStrategy strategy = MaskingStrategy.Default)
    {
        Pattern = null;
        Strategy = strategy;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SensitiveAttribute"/> class.
    /// </summary>
    /// <param name="pattern">Custom masking pattern to apply.</param>
    public SensitiveAttribute(string? pattern)
    {
        Pattern = pattern;
        Strategy = MaskingStrategy.Default;
    }

    /// <summary>
    /// Gets the masking strategy that should be applied to the decorated member.
    /// </summary>
    public MaskingStrategy Strategy { get; }
    
    /// <summary>
    /// Gets the custom masking pattern that should be applied to the decorated member.
    /// </summary>
    public string? Pattern { get; }
}
