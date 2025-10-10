using System.Text.RegularExpressions;
using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.SystemTextJson;
using Newtonsoft.Json;
using Xunit;

namespace Json.Masker.Tests;

public partial class BasicTestsNewtonsoft
{
    private readonly JsonSerializerSettings _options;

    public BasicTestsNewtonsoft()
    {
        var maskingService = new DefaultMaskingService();
        _options = new()
        {
            ContractResolver = new MaskingContractResolver(maskingService)
        };
    }

    [Theory]
    [InlineData("4111111111111234", "creditcard", "****-****-****-1234")]
    [InlineData("123456789", "ssn", "***-**-6789")]
    [InlineData("swordfish", "default", "****")]
    public void Should_properly_mask_per_strategy_newtonsoft(string raw, string strategy, string expected)
    {
        var customer = new Customer
        {
            Name = "Alice",
            CreditCard = strategy == "creditcard" ? raw : "4111111111111234",
            SSN = strategy == "ssn" ? raw : "123456789"
        };

        MaskingContextAccessor.Set(new MaskingContext { Enabled = true });

        var json = JsonConvert.SerializeObject(customer, Formatting.Indented, _options);

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

        var json = JsonConvert.SerializeObject(collection, Formatting.Indented, _options);
    
        AssertSubstringCount("****-****-****-1234", collection.Count, json);
        AssertSubstringCount("***-**-6789", collection.Count, json);
        AssertSubstringCount("****", collection.Count, json);
        AssertSubstringCount("<redacted>", collection.Count * 2, json);
        
        void AssertSubstringCount(string expectedStr, int expectedCount, string actualStr)
        {
            var actualSubstringCount = Regex.Matches(actualStr, Regex.Escape($"\"{expectedStr}\"")).Count;
            Assert.Equal(expectedCount, actualSubstringCount);
        }
    }

    [Fact]
    public void Should_mask_using_existing_converter_pattern()
    {
        var customer = new CustomerWithNewtonsoftConverter
        {
            Anniversary = new DateTime(2024, 3, 15)
        };

        MaskingContextAccessor.Set(new MaskingContext { Enabled = true });

        var json = JsonConvert.SerializeObject(customer, Formatting.None, _options);

        Assert.Contains("\"Anniversary\":\"2024-**-****\"", json);
        Assert.DoesNotContain("2024-03-15", json);
    }
}

