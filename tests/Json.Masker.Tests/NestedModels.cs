using System.Collections.Generic;
using Json.Masker.Abstract;

namespace Json.Masker.Tests;

public class Household
{
    public string Name { get; set; } = string.Empty;

    public List<Dependent> Dependents { get; set; } = [];
}

public class Dependent
{
    public string Name { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Ssn)]
    public string SSN { get; set; } = string.Empty;

    public ContactInfo Contact { get; set; } = new();
}

public class ContactInfo
{
    [Sensitive]
    public string PhoneNumber { get; set; } = string.Empty;
}
