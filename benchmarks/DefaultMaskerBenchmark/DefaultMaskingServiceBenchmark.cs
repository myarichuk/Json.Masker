using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Json.Masker.Abstract;

namespace DefaultMaskerBenchmark;

/*
| Method                      | Mean       | Error      | StdDev     | StdErr    | Median     | Min        | Q1         | Q3         | Max        | Op/s          | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |-----------:|-----------:|-----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|--------------:|-------:|--------:|-------:|----------:|------------:|
| 'Unified Mask() entrypoint' |   3.623 ns |  0.1368 ns |  0.2170 ns | 0.0378 ns |   3.616 ns |   3.319 ns |   3.442 ns |   3.725 ns |   4.142 ns | 276,009,828.1 |   1.00 |    0.00 |      - |         - |          NA |
| 'Mask Credit Card Direct'   |  78.907 ns |  2.7154 ns |  7.7473 ns | 0.7991 ns |  76.002 ns |  70.067 ns |  72.747 ns |  82.497 ns | 101.836 ns |  12,673,180.2 |  21.62 |    2.65 | 0.0178 |     224 B |          NA |
| 'Mask SSN Direct'           |  54.777 ns |  1.3130 ns |  1.9246 ns | 0.3574 ns |  54.483 ns |  51.243 ns |  53.748 ns |  56.373 ns |  58.504 ns |  18,255,984.3 |  15.20 |    1.00 | 0.0172 |     216 B |          NA |
| 'Mask Email Direct'         | 474.751 ns |  9.5920 ns | 16.2879 ns | 2.6777 ns | 470.142 ns | 450.857 ns | 462.501 ns | 489.740 ns | 514.121 ns |   2,106,365.7 | 131.40 |    9.53 | 0.0877 |    1104 B |          NA |
| 'Mask IBAN Direct'          | 267.718 ns | 25.6933 ns | 71.6226 ns | 7.5497 ns | 253.033 ns | 164.895 ns | 222.823 ns | 319.830 ns | 484.639 ns |   3,735,277.5 |  60.20 |   18.40 | 0.0107 |     136 B |          NA |
| 'Custom Pattern Masking'    | 117.838 ns |  5.1850 ns | 14.7930 ns | 1.5258 ns | 115.836 ns |  96.830 ns | 106.287 ns | 125.156 ns | 156.919 ns |   8,486,231.5 |  32.38 |    3.84 | 0.0210 |     264 B |          NA |
 */

[MemoryDiagnoser]
[Config(typeof(Config))]
public class DefaultMaskingServiceBenchmarks
{
    private readonly DefaultMaskingService _service = new();
    private readonly MaskingContext _ctx = new() { Enabled = true };

    [Benchmark(Baseline = true, Description = "Simple Mask")]
    public string Mask_Simple() => 
        _service.Mask("4111111111111111", MaskingStrategy.Default, pattern: null, _ctx);

    [Benchmark(Description = "Mask Credit Card Direct")]
    public string Mask_CreditCard() => 
        _service.Mask("4111111111111111", MaskingStrategy.Creditcard, null, _ctx);

    [Benchmark(Description = "Mask SSN Direct")]
    public string Mask_SSN() => 
        _service.Mask("123-45-6789", MaskingStrategy.Ssn, null, _ctx);

    [Benchmark(Description = "Mask Email Direct")]
    public string Mask_Email() => 
        _service.Mask("john.doe@example.com", MaskingStrategy.Email, null, _ctx);

    [Benchmark(Description = "Mask IBAN Direct")]
    public string Mask_Iban() => 
        _service.Mask("DE89370400440532013000", MaskingStrategy.Iban, null, _ctx);

    [Benchmark(Description = "Custom Pattern Masking")]
    public string Mask_CustomPattern() => 
        _service.Mask("ABCDEFG1234567", MaskingStrategy.Default, "###-***-###", _ctx);

    public class Config : ManualConfig
    {
        public Config()
        {
            AddColumn(BenchmarkDotNet.Columns.StatisticColumn.AllStatistics);
            AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
        }
    }
}
