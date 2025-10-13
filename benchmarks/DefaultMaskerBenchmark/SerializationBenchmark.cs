using System;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Byndyusoft.MaskedSerialization.Helpers;
using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.SystemTextJson;
using JsonDataMasking.Masks;
using JsonMasking;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Benchmarks the default masking service against different JSON serializers and a third-party masking library.
/// </summary>
[MemoryDiagnoser]
public class SerializationBenchmark
{
    private static readonly MaskingContext DisabledMaskingContext = new();
    private const string JsonMaskingReplacement = "****";

    private readonly MaskingContext _maskingContext = new() { Enabled = true };
    private BenchmarkCustomer _customer = null!;
    private BenchmarkCustomer _byndyusoftCustomer = null!;

    private JsonSerializerSettings _newtonsoftSettings = null!;
    private JsonSerializerOptions _systemTextOptions = null!;
    private JsonSerializerOptions _byndyusoftOptions = null!;
    private string _jsonMaskingPayload = string.Empty;
    private string[] _jsonMaskingTargets = Array.Empty<string>();

    /// <summary>
    /// Configures the serializer options required for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        _customer = SampleDataFactory.CreateCustomer();
        _byndyusoftCustomer = SampleDataFactory.CreateCustomer();

        var newtonsoftConfigurator = new NewtonsoftJsonMaskingConfigurator(maskingService);
        _newtonsoftSettings = new JsonSerializerSettings();
        newtonsoftConfigurator.Configure(_newtonsoftSettings);

        var systemTextConfigurator = new SystemTextJsonMaskingConfigurator(maskingService);
        _systemTextOptions = new JsonSerializerOptions();
        systemTextConfigurator.Configure(_systemTextOptions);

        _byndyusoftOptions = new JsonSerializerOptions();
        MaskedSerializationHelper.SetupOptionsForMaskedSerialization(_byndyusoftOptions);

        _jsonMaskingPayload = SampleDataFactory.CreateJsonMaskingPayload();
        _jsonMaskingTargets = SampleDataFactory.CreateJsonMaskingTargets();

        MaskingContextAccessor.Set(DisabledMaskingContext);
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
        var maskedCustomer = JsonMask.MaskSensitiveData(SampleDataFactory.CreateCustomer());
        return JsonConvert.SerializeObject(maskedCustomer, Formatting.None);
    }

    /// <summary>
    /// Masks the sample payload using the JsonMasking library and returns the resulting JSON.
    /// </summary>
    [Benchmark]
    public string JsonMaskingSerialization() => _jsonMaskingPayload.MaskFields(_jsonMaskingTargets, JsonMaskingReplacement);

    /// <summary>
    /// Serializes the Byndyusoft masked customer with System.Text.Json using the library's helper configuration.
    /// </summary>
    [Benchmark]
    public string ByndyusoftSystemTextJsonSerialization() => JsonSerializer.Serialize(_byndyusoftCustomer, _byndyusoftOptions);
    
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
