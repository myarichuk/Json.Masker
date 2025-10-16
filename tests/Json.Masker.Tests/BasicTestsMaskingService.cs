using Json.Masker.Abstract;
using Xunit;

namespace Json.Masker.Tests;

public class BasicTestsMaskingService
{
    private readonly DefaultMaskingService _svc = new();

    private static readonly MaskingContext Enabled = new() { Enabled = true };

    [Theory]
    [InlineData("abc123def", "123")]
    [InlineData("4-5-6", "456")]
    [InlineData("NoDigitsHere", "")]
    [InlineData("123456", "123456")]
    [InlineData("混合123テキスト", "123")] // includes Unicode noise
    [InlineData(" 9 8 7 ", "987")]
    [InlineData("", "")]
    public void NormalizeDigits_ShouldExtractOnlyDigits(string input, string expected)
    {
        Span<char> normalizedRaw = stackalloc char[input.Length];
        var digitsCount = DigitNormalizer.Normalize(input, normalizedRaw);
        var result = normalizedRaw[..digitsCount];
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("4111 1111 1111 1234", "****-****-****-1234")]   // spaced
    [InlineData("4111-1111-1111-1234", "****-****-****-1234")]   // dashed
    [InlineData("4111111111111234", "****-****-****-1234")]      // clean
    [InlineData("abcd4111111111111234xyz", "****-****-****-1234")] // garbage inside
    public void Should_normalize_and_mask_creditcard(string raw, string expected)
    {
        var result = _svc.Mask(raw, MaskingStrategy.Creditcard, null);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123-45-6789", "***-**-6789")]
    [InlineData("123456789", "***-**-6789")]
    [InlineData("  123 45 6789  ", "***-**-6789")]
    public void Should_normalize_and_mask_ssn(string raw, string expected)
    {
        var result = _svc.Mask(raw, MaskingStrategy.Ssn, null);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("john.doe@example.com", "j*****@e****.com")]
    [InlineData("a@example.org", "*@e****.org")]
    [InlineData("weird.email@sub.domain.co.uk", "w*****@s****.domain.co.uk")]
    [InlineData("invalid-email", "****@****")]
    public void Should_mask_email_correctly(string raw, string expected)
    {
        var result = _svc.Mask(raw, MaskingStrategy.Email, null);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("DE44500105175407324931", "DE** **** **** **** 4931")]
    [InlineData("de44 5001 0517 5407 3249 31", "DE** **** **** **** 4931")]
    [InlineData("GB33BUKB20201555555555", "GB** **** **** **** 5555")]
    [InlineData("INVALIDIBAN", "****")]
    public void Should_mask_iban_correctly(string raw, string expected)
    {
        var result = _svc.Mask(raw, MaskingStrategy.Iban, null);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("<redacted>", MaskingStrategy.Redacted)]
    [InlineData("****", MaskingStrategy.Default)]
    public void Should_return_expected_default_or_redacted_masks(string expected, MaskingStrategy strategy)
    {
        var result = _svc.Mask("anything", strategy, null);
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("Fo1234", "##****", "Fo****")]
    [InlineData("Fo1234", "##********", "Fo****")]// extra pattern ignored
    [InlineData("Fo1234", "##**", "Fo****")] // pattern -> rest masked
    [InlineData("1234567", "###-####", "123-4567")] // insert literal
    [InlineData("1234-456-789", "*##*-***-***", "*23*-***-***")] // literals sync when present
    [InlineData("2024-7-12", "**##-*-**", "**24-*-**")]   // mixed chars/literals
    [InlineData("01:12:23", "**:##:***********", "**:12:**")] // literals sync, placeholders stop
    public void Should_mask_pattern_correctly(string raw, string? pattern, string expected)
    {
        var result = _svc.Mask(raw, MaskingStrategy.Iban, pattern);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_use_default_mask_when_pattern_missing(string? pattern)
    {
        var result = _svc.Mask("ABC1234", MaskingStrategy.Iban, pattern);
        Assert.Equal(_svc.DefaultMask, result);
    }
}
