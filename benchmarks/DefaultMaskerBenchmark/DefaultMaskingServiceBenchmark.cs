using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Json.Masker.Abstract;

namespace DefaultMaskerBenchmark;

[MemoryDiagnoser]
[Config(typeof(Config))]
public class DefaultMaskingServiceBenchmarks
{
    private readonly DefaultMaskingService _service = new();
    private readonly MaskingContext _ctx = new() { Enabled = true };

    [Params(
        "4111111111111111",
        "123-45-6789",
        "john.doe@example.com",
        "DE89370400440532013000",
        "SensitiveValue"
    )]
    public string Input { get; set; } = string.Empty;

    [Params(
        MaskingStrategy.Creditcard,
        MaskingStrategy.Ssn,
        MaskingStrategy.Email,
        MaskingStrategy.Iban,
        MaskingStrategy.Redacted,
        MaskingStrategy.Default
    )]
    public MaskingStrategy Strategy { get; set; }

    [Benchmark(Baseline = true, Description = "Unified Mask() entrypoint")]
    public string Unified_Mask()
    {
        return _service.Mask(Input, Strategy, pattern: null, _ctx);
    }

    [Benchmark(Description = "Mask Credit Card Direct")]
    public string Mask_CreditCard() => _service.Mask("4111111111111111", MaskingStrategy.Creditcard, null, _ctx);

    [Benchmark(Description = "Mask SSN Direct")]
    public string Mask_SSN() => _service.Mask("123-45-6789", MaskingStrategy.Ssn, null, _ctx);

    [Benchmark(Description = "Mask Email Direct")]
    public string Mask_Email() => _service.Mask("john.doe@example.com", MaskingStrategy.Email, null, _ctx);

    [Benchmark(Description = "Mask IBAN Direct")]
    public string Mask_Iban() => _service.Mask("DE89370400440532013000", MaskingStrategy.Iban, null, _ctx);

    [Benchmark(Description = "Custom Pattern Masking")]
    public string Mask_CustomPattern()
    {
        return _service.Mask("ABCDEFG1234567", MaskingStrategy.Default, "###-***-###", _ctx);
    }

    public class Config : ManualConfig
    {
        public Config()
        {
            AddColumn(BenchmarkDotNet.Columns.StatisticColumn.AllStatistics);
            AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
        }
    }
}
