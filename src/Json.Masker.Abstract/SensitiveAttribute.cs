namespace Json.Masker.Abstract;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SensitiveAttribute : Attribute
{
    public string Strategy { get; }
    public SensitiveAttribute(string strategy = "default") => Strategy = strategy;
}