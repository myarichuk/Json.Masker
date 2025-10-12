using System;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.SystemTextJson;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DefaultMaskerBenchmark;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class DefaultMaskingServiceSerializationBenchmark
{
    private static readonly MaskingContext DisabledMaskingContext = new();

    private readonly MaskingContext maskingContext = new() { Enabled = true };
    private readonly SampleCustomer customer = new()
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
        ],
    };

    private JsonSerializerSettings newtonsoftSettings = default!;
    private JsonSerializerOptions systemTextOptions = default!;

    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        newtonsoftSettings = new JsonSerializerSettings();
        new NewtonsoftJsonMaskingConfigurator(maskingService).Configure(newtonsoftSettings);

        systemTextOptions = new JsonSerializerOptions();
        new SystemTextJsonMaskingConfigurator(maskingService).Configure(systemTextOptions);
    }

    [Benchmark]
    public string NewtonsoftSerialization() =>
        ExecuteWithMasking(() => JsonConvert.SerializeObject(customer, Formatting.None, newtonsoftSettings));

    [Benchmark]
    public string SystemTextJsonSerialization() =>
        ExecuteWithMasking(() => JsonSerializer.Serialize(customer, systemTextOptions));

    private string ExecuteWithMasking(Func<string> serializer)
    {
        MaskingContextAccessor.Set(maskingContext);

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
