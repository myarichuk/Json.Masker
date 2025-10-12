using System;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.SystemTextJson;
using Newtonsoft.Json;

namespace DefaultMaskerBenchmark;

[MemoryDiagnoser]
public class DefaultMaskingServiceSerializationBenchmark
{
    private static readonly MaskingContext DisabledMaskingContext = new();

    private readonly MaskingContext _maskingContext = new() { Enabled = true };
    private readonly SampleCustomer _customer = new()
    {
        Name = "Alice",
        CreditCard = "4111111111111234",
        SSN = "123456789",
        Age = 35,
        Hobbies =
        [
            "Knitting",
            "Skiing",
            "Gardening"
        ]
    };

    private JsonSerializerSettings _newtonsoftSettings = default!;
    private JsonSerializerOptions _systemTextOptions = default!;

    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        _newtonsoftSettings = new JsonSerializerSettings();
        new NewtonsoftJsonMaskingConfigurator(maskingService).Configure(_newtonsoftSettings);

        _systemTextOptions = new JsonSerializerOptions();
        new SystemTextJsonMaskingConfigurator(maskingService).Configure(_systemTextOptions);
    }

    [Benchmark]
    public string NewtonsoftSerialization() =>
        ExecuteWithMasking(() => JsonConvert.SerializeObject(_customer, Formatting.None, _newtonsoftSettings));

    [Benchmark]
    public string SystemTextJsonSerialization() =>
        ExecuteWithMasking(() => JsonSerializer.Serialize(_customer, _systemTextOptions));

    private string ExecuteWithMasking(Func<string> serializer)
    {
        MaskingContextAccessor.Set(_maskingContext);

        try
        {
            return serializer();
        }
        finally
        {
            MaskingContextAccessor.Set(DisabledMaskingContext);
        }
    }
}
