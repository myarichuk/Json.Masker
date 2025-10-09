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
    public SensitiveAttribute(MaskingStrategy strategy = MaskingStrategy.Default) => Strategy = strategy;

    /// <summary>
    /// Gets the masking strategy that should be applied to the decorated member.
    /// </summary>
    public MaskingStrategy Strategy { get; }
}
