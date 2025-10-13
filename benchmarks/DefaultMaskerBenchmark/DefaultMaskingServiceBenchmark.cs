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
public class DefaultMaskingServiceBenchmark
{
    private static readonly MaskingContext DisabledMaskingContext = new();

    private readonly MaskingContext _maskingContext = new() { Enabled = true };
    private readonly SampleCustomer _customer = SampleDataFactory.CreateSampleCustomer();
    private readonly JsonDataMaskingCustomer _jsonDataMaskingCustomer = SampleDataFactory.CreateJsonDataMaskingCustomer();

    private JsonSerializerSettings _newtonsoftSettings = default!;
    private JsonSerializerOptions _systemTextOptions = default!;

    /// <summary>
    /// Configures the serializer options required for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        var newtonsoftConfigurator = new NewtonsoftJsonMaskingConfigurator(maskingService);
        _newtonsoftSettings = new JsonSerializerSettings();
        newtonsoftConfigurator.Configure(_newtonsoftSettings);

        var systemTextConfigurator = new SystemTextJsonMaskingConfigurator(maskingService);
        _systemTextOptions = new JsonSerializerOptions();
        systemTextConfigurator.Configure(_systemTextOptions);
    }

    /// <summary>
    /// Serializes the sample customer using Newtonsoft.Json while the default masking service is enabled.
    /// </summary>
    [Benchmark]
    public string NewtonsoftSerialization() =>
        SerializeWithMaskingContext(() => JsonConvert.SerializeObject(_customer, Formatting.None, _newtonsoftSettings));

    /// <summary>
    /// Serializes the sample customer using System.Text.Json while the default masking service is enabled.
    /// </summary>
    [Benchmark]
    public string SystemTextJsonSerialization() =>
        SerializeWithMaskingContext(() => JsonSerializer.Serialize(_customer, _systemTextOptions));

    /// <summary>
    /// Masks the sample customer with the JsonDataMasking library and serializes it using Newtonsoft.Json.
    /// </summary>
    [Benchmark]
    public string JsonDataMaskingSerialization()
    {
        var maskedCustomer = JsonMask.MaskSensitiveData(_jsonDataMaskingCustomer);
        return JsonConvert.SerializeObject(maskedCustomer, Formatting.None);
    }

    /// <summary>
    /// Runs a serialization delegate within an enabled masking context to capture masked output.
    /// </summary>
    private string SerializeWithMaskingContext(Func<string> serializer)
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
