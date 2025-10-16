using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Json.Masker.Abstract;

namespace DefaultMaskerBenchmark;

/*
| Method                    | Mean      | Error     | StdDev     | StdErr    | Min        | Q1         | Median     | Q3        | Max       | Op/s          | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------- |----------:|----------:|-----------:|----------:|-----------:|-----------:|-----------:|----------:|----------:|--------------:|------:|--------:|-------:|----------:|------------:|
| 'Simple Mask'             |  1.037 ns | 0.2125 ns |  0.6166 ns | 0.0626 ns |  0.0383 ns |  0.5159 ns |  0.8820 ns |  1.438 ns |  3.017 ns | 964,126,783.1 |  1.00 |    0.00 |      - |         - |          NA |
| 'Mask Credit Card Direct' | 42.525 ns | 1.2703 ns |  3.4559 ns | 0.3727 ns | 37.3879 ns | 39.7022 ns | 42.5239 ns | 44.132 ns | 52.856 ns |  23,515,742.6 | 52.07 |   29.91 | 0.0051 |      64 B |          NA |
| 'Mask SSN Direct'         | 56.459 ns | 3.5491 ns | 10.0681 ns | 1.0440 ns | 40.3811 ns | 47.5055 ns | 55.1960 ns | 60.770 ns | 88.173 ns |  17,712,107.0 | 71.19 |   42.73 | 0.0038 |      48 B |          NA |
| 'Mask Email Direct'       | 59.021 ns | 4.3933 ns | 12.6053 ns | 1.2933 ns | 42.3030 ns | 49.4284 ns | 57.3940 ns | 65.364 ns | 93.085 ns |  16,943,132.5 | 92.69 |  125.98 | 0.0044 |      56 B |          NA |
| 'Mask IBAN Direct'        | 61.774 ns | 1.1754 ns |  1.2577 ns | 0.2964 ns | 60.4463 ns | 60.8814 ns | 61.5188 ns | 62.165 ns | 64.882 ns |  16,187,991.5 | 44.36 |   13.88 | 0.0057 |      72 B |          NA |
| 'Custom Pattern Masking'  | 30.366 ns | 0.6361 ns |  0.6532 ns | 0.1584 ns | 29.7280 ns | 29.7870 ns | 30.1674 ns | 30.876 ns | 31.937 ns |  32,931,887.2 | 22.13 |    6.76 | 0.0044 |      56 B |          NA |
 */

[MemoryDiagnoser]
[Config(typeof(Config))]
public class DefaultMaskingServiceBenchmarks
{
    private readonly DefaultMaskingService _service = new();
    private readonly MaskingContext _ctx = new() { Enabled = true };

    [Benchmark(Baseline = true, Description = "Simple Mask")]
    public string Mask_Simple() => 
        _service.Mask("4111111111111111", MaskingStrategy.Default, pattern: null);

    [Benchmark(Description = "Mask Credit Card Direct")]
    public string Mask_CreditCard() => 
        _service.Mask("4111111111111111", MaskingStrategy.Creditcard, null);

    [Benchmark(Description = "Mask SSN Direct")]
    public string Mask_SSN() => 
        _service.Mask("123-45-6789", MaskingStrategy.Ssn, null);

    [Benchmark(Description = "Mask Email Direct")]
    public string Mask_Email() => 
        _service.Mask("john.doe@example.com", MaskingStrategy.Email, null);

    [Benchmark(Description = "Mask IBAN Direct")]
    public string Mask_Iban() => 
        _service.Mask("DE89370400440532013000", MaskingStrategy.Iban, null);

    [Benchmark(Description = "Custom Pattern Masking")]
    public string Mask_CustomPattern() => 
        _service.Mask("ABCDEFG1234567", MaskingStrategy.Default, "###-***-###");

    public class Config : ManualConfig
    {
        public Config()
        {
            AddColumn(BenchmarkDotNet.Columns.StatisticColumn.AllStatistics);
            AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
        }
    }
}
