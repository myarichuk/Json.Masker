# Json.Masker

**Json.Masker** is a lightweight library for masking sensitive values during JSON serialization.  
Mark properties with `[Sensitive]`, enable the masking context, and the rest happens automatically—no custom DTOs or fragile string hacks.

Supports both **Newtonsoft.Json** and **System.Text.Json**.

---

## Packages

| Package | Description |
| --- | --- |
| **Json.Masker.Abstract** | Shared abstractions: `[Sensitive]` attribute, masking strategies, context accessors, and `DefaultMaskingService`. Not published to NuGet. |
| **Json.Masker.Newtonsoft** | `ContractResolver` that wraps sensitive members before serialization. |
| **Json.Masker.SystemTextJson** | `JsonTypeInfo` modifier that injects masking converters for `System.Text.Json`. |
| **Json.Masker.AspNet** | Middleware and helpers for toggling masking per request (System.Text.Json). |
| **Json.Masker.AspNet.Newtonsoft** | ASP.NET Core middleware integration for Newtonsoft.Json. |

---

## Quick Start

Install the package for your serializer:

```bash
dotnet add package Json.Masker.Newtonsoft
# or
dotnet add package Json.Masker.SystemTextJson

# optional ASP.NET helpers
dotnet add package Json.Masker.AspNet
dotnet add package Json.Masker.AspNet.Newtonsoft
```

The ASP.NET helpers let you toggle masking per request with zero boilerplate.

---

## Setup

### Option 1: Dependency Injection

```csharp
builder.Services.AddJsonMasking(options =>
{
    // Replace with a custom IMaskingService if needed
    options.MaskingService = new DefaultMaskingService();
});

builder.Services
    .AddOptions<MvcNewtonsoftJsonOptions>()
    .Configure<IJsonMaskingConfigurator>((opts, configurator) =>
        configurator.Configure(opts.SerializerSettings));

app.UseNewtonsoftJsonMasking();
```

Swap `Newtonsoft` for `System.Text.Json` as needed:

```csharp
builder.Services.AddControllers();
builder.Services.AddJsonMasking();

builder.Services
    .AddOptions<JsonOptions>()
    .Configure<IJsonMaskingConfigurator>((opts, configurator) =>
        configurator.Configure(opts.SerializerOptions));

app.UseTextJsonMasking();
```

DI automatically registers:

* `IMaskingService` (default or custom)
* `IJsonMaskingConfigurator` (serializer-specific configurator)

---

### Option 2: Manual Setup

**Newtonsoft.Json**

```csharp
var masking = new DefaultMaskingService();
var settings = new JsonSerializerSettings
{
    ContractResolver = new MaskingContractResolver(masking)
};

var json = JsonConvert.SerializeObject(customer, Formatting.Indented, settings);
```

**System.Text.Json**

```csharp
var masking = new DefaultMaskingService();
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { new MaskingTypeInfoModifier(masking).Modify }
    }
};

var json = JsonSerializer.Serialize(customer, options);
```

---

## Masking Example

```csharp
public class Customer
{
    public string Name { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Creditcard)]
    public string CreditCard { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Ssn)]
    public string SSN { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Email)]
    public string Email { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Iban)]
    public string BankAccount { get; set; } = string.Empty;

    [Sensitive] public int Age { get; set; }

    [Sensitive(MaskingStrategy.Redacted)]
    public List<string> Hobbies { get; set; } = [];

    [Sensitive("##-****-####")]
    public string LoyaltyNumber { get; set; } = string.Empty;
}
```

Enable masking for the current scope:

```csharp
MaskingContextAccessor.Set(new MaskingContext { Enabled = true });
```

**Result:**

```json
{
  "Name": "Alice",
  "CreditCard": "****-****-****-1234",
  "SSN": "***-**-6789",
  "Email": "a*****@g****.com",
  "BankAccount": "GB** **** **** **** 1234",
  "Age": "****",
  "Hobbies": ["<redacted>", "<redacted>"],
  "LoyaltyNumber": "12-****-3456"
}
```

---

## Masking Strategies

| Strategy | Example Output |
| --- | --- |
| `Default` | `****` |
| `Creditcard` | `****-****-****-1234` |
| `Ssn` | `***-**-6789` |
| `Redacted` | `<redacted>` |
| `Email` | `a*****@d****.com` |
| `Iban` | `GB** **** **** **** 1234` |

Custom masking logic? Implement `IMaskingService` and register it via `MaskingOptions`.

---

## Contributing

1. `dotnet restore`, `dotnet build`, `dotnet test`
2. Run `./install-dependencies.sh` to sync the expected .NET SDK and git hooks.
3. Run `./run-pre-commit.sh` to check analyzers and formatters.
4. Open a PR with conventional commits; merging to `main` triggers NuGet release.

### Extending Masking

To add a new masking rule:

1. Implement `IMaskingService` or subclass `DefaultMaskingService`.
2. Add tests under `tests/Json.Masker.Tests`.
3. (Optional) Contribute back via a new `MaskingStrategy` value.

---

## Adding Serializer Support

To integrate another serializer:

1. Create a project under `src/` referencing `Json.Masker.Abstract`.
2. Implement `IJsonMaskingConfigurator`.
3. Expose an `AddJsonMasking` extension.
4. Add integration tests to ensure sensitive data is masked.

---

## Performance

| Method | Mean | Allocated | Ratio |
| --- | ---: | ---: | ---: |
| Plain_SystemTextJson | 501 ns | 752 B | 1.00 |
| JsonMasker_Newtonsoft | 990 ns | 2,152 B | 1.97 |
| JsonMasker_SystemTextJson | 801 ns | 648 B | 1.62 |
| JsonDataMasking_Newtonsoft | 8,085 ns | 6,761 B | 16.08 |
| JsonMasking_PayloadMasking | 7,013 ns | 5,720 B | 13.90 |
| Byndyusoft_SystemTextJson | 324 ns | 232 B | 0.64 |

---

## Build & Test

```bash
dotnet restore
dotnet build
dotnet test
```

Run `./install-dependencies.sh` to install matching SDKs and pre-commit hooks.

---

## Samples

Check out `/samples` for minimal demos:

* **System.Text.Json:** `Json.Masker.Sample.SystemTextJson`
* **Newtonsoft.Json:** `Json.Masker.Sample.Newtonsoft`

Run with:

```bash
dotnet run --project <sample-project>
```

View masked and unmasked outputs side-by-side in your favorite REST client. **Note: in order to see masked output make sure to include 'X-Json-Mask: true' in the request headers.**
