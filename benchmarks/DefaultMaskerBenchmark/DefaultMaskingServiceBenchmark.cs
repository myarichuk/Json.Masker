using System.Globalization;
using System.Linq;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Byndyusoft.MaskedSerialization;
using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.SystemTextJson;
using JsonDataMasking.Masks;
using JsonMasking;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection;
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
    private static readonly DataClassificationSet CreditCardClassification = new(new DataClassification("Customer", "CreditCard"));
    private static readonly DataClassificationSet SsnClassification = new(new DataClassification("Customer", "SocialSecurityNumber"));
    private static readonly DataClassificationSet AgeClassification = new(new DataClassification("Customer", "Age"));
    private static readonly DataClassificationSet HobbiesClassification = new(new DataClassification("Customer", "Hobbies"));
    private const string JsonMaskingReplacement = "***";

    private readonly MaskingContext _maskingContext = new() { Enabled = true };
    private readonly SampleCustomer _customer = SampleDataFactory.CreateSampleCustomer();
    private readonly JsonDataMaskingCustomer _jsonDataMaskingCustomer = SampleDataFactory.CreateJsonDataMaskingCustomer();
    private ByndyusoftCustomer _byndyusoftCustomer = default!;

    private JsonSerializerSettings _newtonsoftSettings = default!;
    private JsonSerializerOptions _systemTextOptions = default!;
    private JsonSerializerOptions _byndyusoftOptions = default!;
    private string _jsonMaskingPayload = string.Empty;
    private string[] _jsonMaskingTargets = Array.Empty<string>();
    private ServiceProvider? _complianceProvider;
    private Redactor _creditCardRedactor = default!;
    private Redactor _ssnRedactor = default!;
    private Redactor _ageRedactor = default!;
    private Redactor _hobbiesRedactor = default!;

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

        _byndyusoftCustomer = SampleDataFactory.CreateByndyusoftCustomer();
        _byndyusoftOptions = new JsonSerializerOptions();
        MaskedSerializationHelper.SetupSettingsForMaskedSerialization(_byndyusoftOptions);

        _jsonMaskingPayload = SampleDataFactory.CreateJsonMaskingPayload();
        _jsonMaskingTargets = ["CreditCard", "SSN", "Age", "Hobbies", "Hobbies.*"];

        var services = new ServiceCollection();
        services.AddRedaction(builder =>
        {
            builder.SetRedactor<FixedAsteriskRedactor>(CreditCardClassification, SsnClassification, AgeClassification, HobbiesClassification);
            builder.SetFallbackRedactor<FixedAsteriskRedactor>();
        });

        _complianceProvider = services.BuildServiceProvider();
        var redactorProvider = _complianceProvider.GetRequiredService<IRedactorProvider>();
        _creditCardRedactor = redactorProvider.GetRedactor(CreditCardClassification);
        _ssnRedactor = redactorProvider.GetRedactor(SsnClassification);
        _ageRedactor = redactorProvider.GetRedactor(AgeClassification);
        _hobbiesRedactor = redactorProvider.GetRedactor(HobbiesClassification);
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
    /// Redacts the sample customer using Microsoft.Extensions.Compliance.Redaction before serializing with System.Text.Json.
    /// </summary>
    [Benchmark]
    public string MicrosoftComplianceRedactionSerialization()
    {
        var redactedCustomer = new
        {
            _customer.Name,
            CreditCard = _creditCardRedactor.Redact(_customer.CreditCard),
            SSN = _ssnRedactor.Redact(_customer.SSN),
            Age = _ageRedactor.Redact(_customer.Age.ToString(CultureInfo.InvariantCulture)),
            Hobbies = _customer.Hobbies.Select(hobby => _hobbiesRedactor.Redact(hobby)).ToArray(),
        };

        return JsonSerializer.Serialize(redactedCustomer);
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

    /// <summary>
    /// Disposes resources created for the Microsoft.Extensions.Compliance.Redaction benchmark.
    /// </summary>
    [GlobalCleanup]
    public void Cleanup() => _complianceProvider?.Dispose();
}
