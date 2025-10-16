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

/*
Latest:

| Method                     | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Plain_SystemTextJson       |   501.6 ns |  10.10 ns |  21.30 ns |  1.00 |    0.00 | 0.0591 |     752 B |        1.00 |
| JsonMasker_Newtonsoft      |   990.0 ns |  14.70 ns |  29.01 ns |  1.97 |    0.09 | 0.1698 |    2152 B |        2.86 |
| JsonMasker_SystemTextJson  |   801.0 ns |  15.44 ns |  21.14 ns |  1.62 |    0.07 | 0.0515 |     648 B |        0.86 |
| JsonDataMasking_Newtonsoft | 8,085.0 ns | 161.61 ns | 311.37 ns | 16.08 |    0.98 | 0.5188 |    6761 B |        8.99 |
| JsonMasking_PayloadMasking | 7,013.2 ns |  50.64 ns |  47.37 ns | 13.90 |    0.37 | 0.4272 |    5720 B |        7.61 |
| Byndyusoft_SystemTextJson  |   324.4 ns |   5.13 ns |   4.80 ns |  0.64 |    0.02 | 0.0181 |     232 B |        0.31 |


 */

/// <summary>
/// Benchmarks various JSON masking and serialization implementations
/// (Json.Masker, JsonDataMasking, JsonMasking, Byndyusoft.MaskedSerialization)
/// against a plain System.Text.Json baseline.
/// </summary>
[MemoryDiagnoser]
public class SerializationBenchmark
{
    private static readonly MaskingContext DisabledMaskingContext = new();
    private const string JsonMaskingReplacement = "****";

    private readonly MaskingContext _maskingContext = new() { Enabled = true };
    private BenchmarkCustomer _customer = null!;
    private BenchmarkCustomer _byndyusoftCustomer = null!;

    private JsonSerializerSettings _jsonMaskerNewtonsoftSettings = null!;
    private JsonSerializerOptions _jsonMaskerSystemTextOptions = null!;
    private JsonSerializerOptions _byndyusoftOptions = null!;
    private string _jsonMaskingPayload = string.Empty;
    private string[] _jsonMaskingTargets = [];

    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        _customer = SampleDataFactory.CreateCustomer();
        _byndyusoftCustomer = SampleDataFactory.CreateCustomer();

        // Json.Masker (Newtonsoft)
        var newtonsoftConfigurator = new NewtonsoftJsonMaskingConfigurator(maskingService);
        _jsonMaskerNewtonsoftSettings = new JsonSerializerSettings();
        newtonsoftConfigurator.Configure(_jsonMaskerNewtonsoftSettings);

        // Json.Masker (System.Text.Json)
        var systemTextConfigurator = new SystemTextJsonMaskingConfigurator(maskingService);
        _jsonMaskerSystemTextOptions = new JsonSerializerOptions();
        systemTextConfigurator.Configure(_jsonMaskerSystemTextOptions);

        // Byndyusoft.MaskedSerialization
        _byndyusoftOptions = new JsonSerializerOptions();
        MaskedSerializationHelper.SetupOptionsForMaskedSerialization(_byndyusoftOptions);

        // JsonMasking
        _jsonMaskingPayload = SampleDataFactory.CreateJsonMaskingPayload();
        _jsonMaskingTargets = SampleDataFactory.CreateJsonMaskingTargets();

        MaskingContextAccessor.Set(DisabledMaskingContext);
    }

    // --------------------------------------------------
    // BASELINE
    // --------------------------------------------------

    /// <summary>
    /// Baseline: plain System.Text.Json serialization without any masking.
    /// </summary>
    [Benchmark(Baseline = true)]
    public string Plain_SystemTextJson() => JsonSerializer.Serialize(_customer);

    // --------------------------------------------------
    // JSON.MASKER
    // --------------------------------------------------

    /// <summary>
    /// Json.Masker using Newtonsoft.Json.
    /// </summary>
    [Benchmark]
    public string JsonMasker_Newtonsoft() =>
        SerializeWithMaskingContext(() => JsonConvert.SerializeObject(_customer, Formatting.None, _jsonMaskerNewtonsoftSettings));

    /// <summary>
    /// Json.Masker using System.Text.Json.
    /// </summary>
    [Benchmark]
    public string JsonMasker_SystemTextJson() =>
        SerializeWithMaskingContext(() => JsonSerializer.Serialize(_customer, _jsonMaskerSystemTextOptions));

    // --------------------------------------------------
    // JSONDATAMASKING
    // --------------------------------------------------

    /// <summary>
    /// JsonDataMasking library (masks before serialization).
    /// </summary>
    [Benchmark]
    public string JsonDataMasking_Newtonsoft()
    {
        var maskedCustomer = JsonMask.MaskSensitiveData(SampleDataFactory.CreateCustomer());
        return JsonConvert.SerializeObject(maskedCustomer, Formatting.None);
    }

    // --------------------------------------------------
    // JSONMASKING
    // --------------------------------------------------

    /// <summary>
    /// JsonMasking library (string-based masking).
    /// </summary>
    [Benchmark]
    public string JsonMasking_PayloadMasking() =>
        _jsonMaskingPayload.MaskFields(_jsonMaskingTargets, JsonMaskingReplacement);

    // --------------------------------------------------
    // BYNDYUSOFT
    // --------------------------------------------------

    /// <summary>
    /// Byndyusoft.MaskedSerialization with System.Text.Json.
    /// </summary>
    [Benchmark]
    public string Byndyusoft_SystemTextJson() =>
        JsonSerializer.Serialize(_byndyusoftCustomer, _byndyusoftOptions);

    // --------------------------------------------------
    // Helper
    // --------------------------------------------------

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
