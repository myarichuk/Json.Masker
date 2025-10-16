# Json.Masker

Json.Masker is a simple library mask sensitive values when serializing them to JSON. Mark a property with `[Sensitive]`, flip the ambient masking context on, and the library does the rest—no custom DTOs, no brittle string hacks.

The repository contains the core abstractions plus concrete integrations for both Newtonsoft.Json and `System.Text.Json` so you can keep your favorite serializer and still get automatic masking.

## Packages

| Package | Description |
| --- | --- |
| `Json.Masker.Abstract` | Attribute, masking strategies, context accessors, and the default masking service. This transient package isn't published to NuGet; it just keeps the shared bits tidy for the two adapters. |
| `Json.Masker.Newtonsoft` | Plug-and-play `ContractResolver` that wraps sensitive members before Newtonsoft writes them out. |
| `Json.Masker.SystemTextJson` | `JsonTypeInfo` modifier that swaps in masking converters when the built-in source generator runs. |
| `Json.Masker.AspNet` | Middleware and helpers that toggle masking per request and hook the System.Text.Json stack into ASP.NET Core. |
| `Json.Masker.AspNet.Newtonsoft` | Middleware and helpers that wire the Newtonsoft.Json integration into ASP.NET Core's MVC pipeline. |

All packages version together and ship to NuGet whenever `main` is updated.

## Quick start

Install the package that matches your serializer:

```bash
dotnet add package Json.Masker.Newtonsoft
# or
dotnet add package Json.Masker.SystemTextJson
# optional web helpers
dotnet add package Json.Masker.AspNet
dotnet add package Json.Masker.AspNet.Newtonsoft
```

The ASP.NET Core helpers are optional but recommended whenever you want to flip masking on or off per request without writing
boilerplate middleware.

### Wire it up

Pick the style that fits your app:

* **Use the plumbing extension** if you're already on `Microsoft.Extensions.DependencyInjection`. Call `services.AddJsonMasking()` from the matching package and it will register:
  * `IMaskingService` as a singleton, using either the built-in `DefaultMaskingService` or the instance you provide through `MaskingOptions`.
  * `IJsonMaskingConfigurator` as a singleton, wired to the serializer-specific implementation (`NewtonsoftJsonMaskingConfigurator` or `SystemTextJsonMaskingConfigurator`).

  Once that extension is in place you can inject `IJsonMaskingConfigurator` wherever you configure the serializer (for example in MVC setup) and let it bolt on the masking bits for you.
  Pair it with `app.UseNewtonsoftJsonMasking()` or `app.UseTextJsonMasking()` from the ASP.NET helper packages to flip masking on and off per request.
* **Wire it manually** if you're configuring the serializer yourself or aren't using DI. Just new up `DefaultMaskingService` (or your own implementation) and pass it into the resolver/modifier shown in the manual samples below.

  The options expose a writeable `MaskingService` property, so you can swap in your own masking logic:

  ```csharp
  builder.Services.AddJsonMasking(options =>
  {
      options.MaskingService = new MyCustomMaskingService();
  });
  ```

Either way, masking kicks in once you mark your models and flip the context switch.

### Mask the model

1. Annotate the bits of your model that shouldn't leak.
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

        [Sensitive]
        public int Age { get; set; }

        [Sensitive(MaskingStrategy.Redacted)]
        public List<string> Hobbies { get; set; } = [];

        [Sensitive("##-****-####")]
        public string LoyaltyNumber { get; set; } = string.Empty;
   }
   ```
   The optional string parameter on <code>[Sensitive]</code> uses <code>#</code> to copy a character from the source value and
   <code>*</code> to mask it, allowing simple custom formats without a bespoke masking service.
2. Turn masking on for the current request or operation by setting the ambient context (middleware is a great place for this):
   ```csharp
   MaskingContextAccessor.Set(new MaskingContext { Enabled = true });
   ```
3. Serialize like normal.

### Plumbing extension in action

```csharp
var builder = WebApplication.CreateBuilder(args);

// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Options;
builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddJsonMasking(options =>
{
    // Replace with your own IMaskingService if you need custom behavior.
    options.MaskingService = new DefaultMaskingService();
});

builder.Services
    .AddOptions<MvcNewtonsoftJsonOptions>()
    .Configure<IJsonMaskingConfigurator>((opts, configurator) =>
        configurator.Configure(opts.SerializerSettings));

var app = builder.Build();

app.UseNewtonsoftJsonMasking();

app.MapControllers();
app.Run();
```

Swap out `AddNewtonsoftJson()` for the default `System.Text.Json` stack and tweak the options wiring:

```csharp
builder.Services.AddControllers();

// using Microsoft.AspNetCore.Http.Json;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Options;
builder.Services.AddJsonMasking();

builder.Services
    .AddOptions<JsonOptions>()
    .Configure<IJsonMaskingConfigurator>((opts, configurator) =>
        configurator.Configure(opts.SerializerOptions));

var app = builder.Build();

app.UseTextJsonMasking();

app.MapControllers();
app.Run();
```

In both cases the extension ensures DI knows about:

* An `IMaskingService` singleton (default or custom).
* An `IJsonMaskingConfigurator` singleton that knows how to modify the serializer's settings/options.

After that the configurator will plug itself into MVC's JSON settings during startup. You can apply the same pattern to worker services, minimal APIs, or any custom bootstrapping code that uses the `Options` system.

### Manual samples

#### Newtonsoft.Json

```csharp
var maskingService = new DefaultMaskingService();
var settings = new JsonSerializerSettings
{
    ContractResolver = new MaskingContractResolver(maskingService)
};

var json = JsonConvert.SerializeObject(customer, Formatting.Indented, settings);
```

Masked output ends up looking like:

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

#### System.Text.Json

```csharp
var maskingService = new DefaultMaskingService();
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { new MaskingTypeInfoModifier(maskingService).Modify }
    }
};

var json = JsonSerializer.Serialize(customer, options);
```

Behind the scenes the modifier swaps in custom converters that call the masking service for both scalars and collections, so you get the same result.

## Contribution guide

### Local workflow tips

* Run `dotnet restore`, `dotnet build`, and `dotnet test` from the repository root to validate changes. The solution and test project share the same SDK version via `global.json`, so you will get consistent compiler behavior.
* Use `./install-dependencies.sh` (or the PowerShell equivalent) to install the expected .NET SDK and the repository's `pre-commit` hooks.
* Execute `./run-pre-commit.sh` locally before opening a PR. It runs analyzers and formatters so CI does not fail on style issues.

### Extending masking behavior

The built-in `DefaultMaskingService` is intentionally straightforward so you can replace it when needed:

1. Implement `IMaskingService` in a new class with whatever masking rules your domain requires.
2. Register your implementation via `MaskingOptions` (as shown earlier) or by adding it to the DI container.
3. Add tests under `tests/Json.Masker.Tests` to describe the new behavior. The existing tests include helpers for exercising both serializers.

If the new behavior is reusable, consider contributing it back by adding a new value to `MaskingStrategy` and wiring it into `DefaultMaskingService`.

### Overriding the built-in service

When you only need to tweak a specific masking rule, inherit from `DefaultMaskingService` and override the protected masking helpers (for example `MaskCreditCard` or `MaskEmail`). The top-level `Mask` method delegates to those helpers, so overriding one method updates every serializer integration automatically. Remember to register your derived service via DI so the adapters pick it up.

## Implementation guide

### Adding another serializer integration

Both existing adapters wrap `IMaskingService` behind an `IJsonMaskingConfigurator`. To support a new serializer:

1. Create a project under `src/` that references `Json.Masker.Abstract`.
2. Implement `IJsonMaskingConfigurator` to hook the masking service into the serializer's configuration model.
3. Expose a `AddJsonMasking` extension (mirroring the current adapters) so consumers get a consistent experience.
4. Cover the integration with tests that exercise the new serializer and ensure sensitive members are masked.

Following this pattern keeps the developer ergonomics identical regardless of serializer choice.

### Masking strategies

The built-in `DefaultMaskingService` supports a few common strategies:

| Strategy | Result |
| --- | --- |
| `MaskingStrategy.Default` | `****` |
| `MaskingStrategy.Creditcard` | `****-****-****-1234` (keeps the last four digits) |
| `MaskingStrategy.Ssn` | `***-**-6789` |
| `MaskingStrategy.Redacted` | `<redacted>` |
| `MaskingStrategy.Email` | `a*****@d****.com` (keeps the first character and domain suffix) |
| `MaskingStrategy.Iban` | `GB** **** **** **** 1234` (keeps the country code and last four characters) |

You can roll your own `IMaskingService` and plug it in through `MaskingOptions` if you want custom behavior (different patterns, role-based rules, etc.).

Masking is also opt-in per request—leave `MaskingContext.Enabled` set to `false` and the library will just write the original values. That’s handy for internal tooling, auditing flows, or when a user has the right to view their own data.

## Building locally

```bash
dotnet restore
dotnet build
dotnet test
```

Tip: run `./install-dependencies.sh` (or the PowerShell equivalent on Windows) to install consistent tooling like the matching .NET SDK and the `pre-commit` hooks.

## Releasing

Releases are automated through GitHub Actions:

1. Open a pull request with conventional commits.
2. Create a PR into `main`
3. Once it is merged, a Github Action workflow would publish to NuGet and create downloadable artifacts.

## Performance

The current performance benchmarks look like this:  

| Method                                  | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-----------------------------------------|-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| Plain_SystemTextJson (no masking!)      |   501.6 ns |  10.10 ns |  21.30 ns |  1.00 |    0.00 | 0.0591 |     752 B |        1.00 |
| JsonMasker_Newtonsoft (Json.Masker)     |   990.0 ns |  14.70 ns |  29.01 ns |  1.97 |    0.09 | 0.1698 |    2152 B |        2.86 |
| JsonMasker_SystemTextJson (Json.Masker) |   801.0 ns |  15.44 ns |  21.14 ns |  1.62 |    0.07 | 0.0515 |     648 B |        0.86 |
| JsonDataMasking_Newtonsoft              | 8,085.0 ns | 161.61 ns | 311.37 ns | 16.08 |    0.98 | 0.5188 |    6761 B |        8.99 |
| JsonMasking_PayloadMasking              | 7,013.2 ns |  50.64 ns |  47.37 ns | 13.90 |    0.37 | 0.4272 |    5720 B |        7.61 |
| Byndyusoft_SystemTextJson               |   324.4 ns |   5.13 ns |   4.80 ns |  0.64 |    0.02 | 0.0181 |     232 B |        0.31 |
