namespace Json.Masker.Abstract;

public enum MaskingStrategy: byte
{
    Default = 0,
    Creditcard,
    Ssn,
    Redacted
}