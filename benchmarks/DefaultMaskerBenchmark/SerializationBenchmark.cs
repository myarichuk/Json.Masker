// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    private const string JsonMaskingReplacement = "****";
    private static readonly MaskingContext DisabledMaskingContext = new();

    private readonly MaskingContext maskingContext = new() { Enabled = true };
    private BenchmarkCustomer customer = null!;
    private BenchmarkCustomer byndyusoftCustomer = null!;

    private JsonSerializerSettings jsonMaskerNewtonsoftSettings = null!;
    private JsonSerializerOptions jsonMaskerSystemTextOptions = null!;
    private JsonSerializerOptions byndyusoftOptions = null!;
    private string jsonMaskingPayload = string.Empty;
    private string[] jsonMaskingTargets = [];

    /// <summary>
    /// Initializes shared state for each benchmark run.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        var maskingService = new DefaultMaskingService();

        this.customer = SampleDataFactory.CreateCustomer();
        this.byndyusoftCustomer = SampleDataFactory.CreateCustomer();

        // Json.Masker (Newtonsoft)
        var newtonsoftConfigurator = new NewtonsoftJsonMaskingConfigurator(maskingService);
        this.jsonMaskerNewtonsoftSettings = new JsonSerializerSettings();
        newtonsoftConfigurator.Configure(this.jsonMaskerNewtonsoftSettings);

        // Json.Masker (System.Text.Json)
        var systemTextConfigurator = new SystemTextJsonMaskingConfigurator(maskingService);
        this.jsonMaskerSystemTextOptions = new JsonSerializerOptions();
        systemTextConfigurator.Configure(this.jsonMaskerSystemTextOptions);

        // Byndyusoft.MaskedSerialization
        this.byndyusoftOptions = new JsonSerializerOptions();
        MaskedSerializationHelper.SetupOptionsForMaskedSerialization(this.byndyusoftOptions);

        // JsonMasking
        this.jsonMaskingPayload = SampleDataFactory.CreateJsonMaskingPayload();
        this.jsonMaskingTargets = SampleDataFactory.CreateJsonMaskingTargets();

        MaskingContextAccessor.Set(DisabledMaskingContext);
    }

    // --------------------------------------------------
    // BASELINE
    // --------------------------------------------------

    /// <summary>
    /// Baseline: plain System.Text.Json serialization without any masking.
    /// </summary>
    /// <returns>The serialized JSON payload.</returns>
    [Benchmark(Baseline = true)]
    public string Plain_SystemTextJson() => JsonSerializer.Serialize(this.customer);

    // --------------------------------------------------
    // JSON.MASKER
    // --------------------------------------------------

    /// <summary>
    /// Json.Masker using Newtonsoft.Json.
    /// </summary>
    /// <returns>The masked JSON payload.</returns>
    [Benchmark]
    public string JsonMasker_Newtonsoft() =>
        this.SerializeWithMaskingContext(() => JsonConvert.SerializeObject(this.customer, Formatting.None, this.jsonMaskerNewtonsoftSettings));

    /// <summary>
    /// Json.Masker using System.Text.Json.
    /// </summary>
    /// <returns>The masked JSON payload.</returns>
    [Benchmark]
    public string JsonMasker_SystemTextJson() =>
        this.SerializeWithMaskingContext(() => JsonSerializer.Serialize(this.customer, this.jsonMaskerSystemTextOptions));

    // --------------------------------------------------
    // JSONDATAMASKING
    // --------------------------------------------------

    /// <summary>
    /// JsonDataMasking library (masks before serialization).
    /// </summary>
    /// <returns>The masked JSON payload.</returns>
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
    /// <returns>The masked JSON payload.</returns>
    [Benchmark]
    public string JsonMasking_PayloadMasking() =>
        this.jsonMaskingPayload.MaskFields(this.jsonMaskingTargets, JsonMaskingReplacement);

    // --------------------------------------------------
    // BYNDYUSOFT
    // --------------------------------------------------

    /// <summary>
    /// Byndyusoft.MaskedSerialization with System.Text.Json.
    /// </summary>
    /// <returns>The masked JSON payload.</returns>
    [Benchmark]
    public string Byndyusoft_SystemTextJson() =>
        JsonSerializer.Serialize(this.byndyusoftCustomer, this.byndyusoftOptions);

    // --------------------------------------------------
    // Helper
    // --------------------------------------------------
    private string SerializeWithMaskingContext(Func<string> serializer)
    {
        MaskingContextAccessor.Set(this.maskingContext);
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
