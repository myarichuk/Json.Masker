using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using Json.Masker.Abstract;
using Json.Masker.SystemTextJson;
using Xunit;

namespace Json.Masker.Tests;

public partial class BasicTestsSystemTextJson
{
    private readonly JsonSerializerOptions _options;

    public BasicTestsSystemTextJson()
    {
        var maskingService = new DefaultMaskingService();
        _options = new JsonSerializerOptions
        {
            // make sure that for tests it won't generate silly things like "\u003Credacted\u003E"
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new MaskingTypeInfoModifier(maskingService).Modify
                }
            }
        };
    }

    [Theory]
    [InlineData("4111111111111234", "creditcard", "****-****-****-1234")]
    [InlineData("123456789", "ssn", "***-**-6789")]
    [InlineData("swordfish", "default", "****")]
    public void Should_properly_mask_per_strategy_json_text(string raw, string strategy, string expected)
    {
        var customer = new Customer
        {
            Name = "Alice",
            CreditCard = strategy == "creditcard" ? raw : "4111111111111234",
            SSN = strategy == "ssn" ? raw : "123456789"
        };

        MaskingContextAccessor.Set(new MaskingContext { Enabled = true });

        var json = JsonSerializer.Serialize(customer, _options);

        Assert.Contains(expected, json);
        Assert.DoesNotContain(raw, json);
    }
    
    [Fact]
    public void Should_properly_mask_collection_per_strategy()
    {
        List<Customer> collection = [
            new()
            {
                Name = "Alice",
                CreditCard = "4111111111111234",
                SSN = "123456789",
                Age = 35,
                Hobbies = ["Knitting", "Skiing"]
            }, new()
            {
                Name = "Bob",
                CreditCard = "4111111111111234",
                SSN = "123456789",
                Age = 20,
                Hobbies = ["Jogging", "Skiing"]                
            }, new()
            {
                Name = "Jack",
                CreditCard = "4111111111111234",
                SSN = "123456789",
                Age = 30,
                Hobbies = ["Sleeping", "Gaming"]
            }];
        
        MaskingContextAccessor.Set(new MaskingContext { Enabled = true });

        var json = JsonSerializer.Serialize(collection, _options);
    
        AssertSubstringCount("****-****-****-1234", collection.Count, json);
        AssertSubstringCount("***-**-6789", collection.Count, json);
        AssertSubstringCount("\"****\"", collection.Count, json);
        AssertSubstringCount("<redacted>", collection.Count * 2, json);
        
        void AssertSubstringCount(string expectedStr, int expectedCount, string actualStr)
        {
            var searchPattern = Regex.Escape(expectedStr);
            var actualSubstringCount = Regex.Matches(actualStr, searchPattern).Count;
            Assert.Equal(expectedCount, actualSubstringCount);
        }
    }    
}