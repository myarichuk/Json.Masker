using System;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.SystemTextJson;
using JsonDataMasking.Masks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Benchmarks the default masking service against different JSON serializers and a third-party masking library.
/// </summary>
[MemoryDiagnoser]
[ThreadingDiagnoser]
public class DefaultMaskingServiceSerializationBenchmark
{
    private static readonly MaskingContext DisabledMaskingContext = new();

    private readonly MaskingContext maskingContext = new() { Enabled = true };
    private readonly SampleCustomer customer = SampleDataFactory.CreateSampleCustomer();
    private readonly JsonDataMaskingCustomer jsonDataMaskingCustomer = SampleDataFactory.CreateJsonDataMaskingCustomer();

    private JsonSerializerSettings newtonsoftSettings = default!;
    private JsonSerializerOptions systemTextOptions = default!;

    /// <summary>
    /// Configures the serializer options required for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        var newtonsoftConfigurator = new NewtonsoftJsonMaskingConfigurator(maskingService);
        newtonsoftSettings = new JsonSerializerSettings();
        newtonsoftConfigurator.Configure(newtonsoftSettings);

        var systemTextConfigurator = new SystemTextJsonMaskingConfigurator(maskingService);
        systemTextOptions = new JsonSerializerOptions();
        systemTextConfigurator.Configure(systemTextOptions);
    }

    /// <summary>
    /// Serializes the sample customer using Newtonsoft.Json while the default masking service is enabled.
    /// </summary>
    [Benchmark]
    public string NewtonsoftSerialization() =>
        SerializeWithMaskingContext(() => JsonConvert.SerializeObject(customer, Formatting.None, newtonsoftSettings));

    /// <summary>
    /// Serializes the sample customer using System.Text.Json while the default masking service is enabled.
    /// </summary>
    [Benchmark]
    public string SystemTextJsonSerialization() =>
        SerializeWithMaskingContext(() => JsonSerializer.Serialize(customer, systemTextOptions));

    /// <summary>
    /// Masks the sample customer with the JsonDataMasking library and serializes it using Newtonsoft.Json.
    /// </summary>
    [Benchmark]
    public string JsonDataMaskingSerialization()
    {
        var maskedCustomer = JsonMask.MaskSensitiveData(jsonDataMaskingCustomer);
        return JsonConvert.SerializeObject(maskedCustomer, Formatting.None);
    }

    /// <summary>
    /// Runs a serialization delegate within an enabled masking context to capture masked output.
    /// </summary>
    private string SerializeWithMaskingContext(Func<string> serializer)
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
