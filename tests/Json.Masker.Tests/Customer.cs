using Json.Masker.Abstract;

namespace Json.Masker.Tests;

public class Customer
{
    public string Name { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Creditcard)]
    public string CreditCard { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Ssn)]
    public string SSN { get; set; } = string.Empty;

    [Sensitive]
    public int Age { get; set; }

    [Sensitive(MaskingStrategy.Redacted)]
    public List<string> Hobbies { get; set; } = [];
}