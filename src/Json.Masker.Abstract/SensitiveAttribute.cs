namespace Json.Masker.Abstract;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SensitiveAttribute : Attribute
{
    public MaskingStrategy Strategy { get; }

    public SensitiveAttribute(MaskingStrategy strategy = MaskingStrategy.Default) => Strategy = strategy;
}