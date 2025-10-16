using Json.Masker.Abstract;
using Xunit;

namespace Json.Masker.Tests;

public class OverrideTestsMaskingService
{
    private readonly CustomMaskingService _svc = new();

    private static readonly MaskingContext Enabled = new() { Enabled = true };

    [Fact]
    public void Override_should_be_correctly_applied()
    {
        var resultCreditCard = _svc.Mask("1234-456-789", MaskingStrategy.Creditcard, null);
        var resultDefault = _svc.Mask("1234-456-789", MaskingStrategy.Default, null);
        var resultPattern = _svc.Mask("1234-456-789", MaskingStrategy.Default, "*##*-***-***");

        Assert.Equal("!redacted!", resultCreditCard);
        Assert.Equal(_svc.DefaultMask, resultDefault);
        Assert.Equal("*23*-***-***", resultPattern);

    }

    private class CustomMaskingService : DefaultMaskingService
    {
        protected override string MaskCreditCard(ReadOnlySpan<char> raw) => "!redacted!";
    }
}