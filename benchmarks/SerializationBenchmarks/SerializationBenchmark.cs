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

namespace SerializationBenchmarks;

/*
Latest:

| Method                     | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| Plain_SystemTextJson       |   570.9 ns |  16.05 ns |  45.27 ns |   556.4 ns |  1.00 |    0.00 | 0.0591 |     752 B |        1.00 |
| JsonMasker_Newtonsoft      | 1,271.5 ns |  65.40 ns | 184.46 ns | 1,275.2 ns |  2.24 |    0.34 | 0.1698 |    2152 B |        2.86 |
| JsonMasker_SystemTextJson  |   860.7 ns |  37.36 ns | 108.97 ns |   813.0 ns |  1.52 |    0.21 | 0.0515 |     648 B |        0.86 |
| JsonDataMasking_Newtonsoft | 8,155.5 ns | 161.22 ns | 363.90 ns | 8,068.0 ns | 14.24 |    1.47 | 0.4883 |    6761 B |        8.99 |
| JsonMasking_PayloadMasking | 6,957.8 ns | 140.71 ns | 382.81 ns | 6,860.4 ns | 12.31 |    1.07 | 0.4272 |    5720 B |        7.61 |
| Byndyusoft_SystemTextJson  |   514.8 ns |  61.66 ns | 169.84 ns |   471.6 ns |  0.90 |    0.29 | 0.0181 |     232 B |        0.31 |

| Method                     | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| Plain_SystemTextJson       |   479.9 ns |   9.48 ns |  10.14 ns |   475.7 ns |  1.00 |    0.00 | 0.0591 |     752 B |        1.00 |
| JsonMasker_Newtonsoft      |   947.5 ns |  17.93 ns |  19.19 ns |   945.0 ns |  1.97 |    0.04 | 0.1698 |    2152 B |        2.86 |
| JsonMasker_SystemTextJson  |   938.0 ns |  32.83 ns |  91.52 ns |   943.7 ns |  1.96 |    0.34 | 0.0515 |     648 B |        0.86 |
| JsonDataMasking_Newtonsoft | 9,122.8 ns | 238.08 ns | 683.09 ns | 8,905.6 ns | 18.67 |    1.34 | 0.4883 |    6761 B |        8.99 |
| JsonMasking_PayloadMasking | 7,136.8 ns | 131.23 ns | 161.16 ns | 7,145.7 ns | 14.84 |    0.46 | 0.4272 |    5720 B |        7.61 |
| Byndyusoft_SystemTextJson  |   366.5 ns |  11.00 ns |  31.91 ns |   366.5 ns |  0.75 |    0.06 | 0.0181 |     232 B |        0.31 |


 */

/// <summary>
/// Benchmarks Json.Masker against several popular masking libraries to provide
/// guidance on throughput and allocation trade-offs when serializing rich DTOs.
///
/// Compared projects:
///  * JsonDataMasking (https://github.com/IvanJosipovic/JsonDataMasking)
///  * JsonMasking (https://github.com/dnauck/JsonMasking)
///  * Byndyusoft.MaskedSerialization (https://github.com/Byndyusoft/Byndyusoft.MaskedSerialization)
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

        // Json.Masker (Newtonsoft) - https://github.com/AvivaSolutions/Json.Masker
        var newtonsoftConfigurator = new NewtonsoftJsonMaskingConfigurator(maskingService);
        _jsonMaskerNewtonsoftSettings = new JsonSerializerSettings();
        newtonsoftConfigurator.Configure(_jsonMaskerNewtonsoftSettings);

        // Json.Masker (System.Text.Json) - https://github.com/AvivaSolutions/Json.Masker
        var systemTextConfigurator = new SystemTextJsonMaskingConfigurator(maskingService);
        _jsonMaskerSystemTextOptions = new JsonSerializerOptions();
        systemTextConfigurator.Configure(_jsonMaskerSystemTextOptions);

        // Byndyusoft.MaskedSerialization (https://github.com/Byndyusoft/Byndyusoft.MaskedSerialization)
        // The library applies attribute-driven masking but currently skips collections and
        // ignores custom JsonSerializerOptions converters, so we benchmark a payload that
        // highlights those limitations next to Json.Masker's richer support.
        _byndyusoftOptions = new JsonSerializerOptions();
        MaskedSerializationHelper.SetupOptionsForMaskedSerialization(_byndyusoftOptions);

        // JsonMasking (https://github.com/dnauck/JsonMasking)
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
    /// JsonDataMasking library (https://github.com/IvanJosipovic/JsonDataMasking) masks
    /// the object graph before serialization using its own attribute model.
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
    /// JsonMasking (https://github.com/dnauck/JsonMasking) performs string-based masking
    /// and therefore requires pre-computed JSON payloads plus target paths.
    /// </summary>
    [Benchmark]
    public string JsonMasking_PayloadMasking() =>
        _jsonMaskingPayload.MaskFields(_jsonMaskingTargets, JsonMaskingReplacement);

    // --------------------------------------------------
    // BYNDYUSOFT
    // --------------------------------------------------

    /// <summary>
    /// Byndyusoft.MaskedSerialization (https://github.com/Byndyusoft/Byndyusoft.MaskedSerialization)
    /// with System.Text.Json. The library does not mask nested collections and does not
    /// honor pre-registered converters, so complex payloads can leak data.
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
