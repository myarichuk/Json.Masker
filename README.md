# Json.Masker

Json.Masker is a simple library mask sensitive values when serializing them to JSON. Mark a property with `[Sensitive]`, flip the ambient masking context on, and the library does the rest—no custom DTOs, no brittle string hacks.

The repository contains the core abstractions plus concrete integrations for both Newtonsoft.Json and `System.Text.Json` so you can keep your favorite serializer and still get automatic masking.

## Packages

| Package | Description |
| --- | --- |
| `Json.Masker.Abstract` | Attribute, masking strategies, context accessors, and the default masking service. This transient package isn't published to NuGet; it just keeps the shared bits tidy for the two adapters. |
| `Json.Masker.Newtonsoft` | Plug-and-play `ContractResolver` that wraps sensitive members before Newtonsoft writes them out. |
| `Json.Masker.SystemTextJson` | `JsonTypeInfo` modifier that swaps in masking converters when the built-in source generator runs. |

All packages version together and ship to NuGet whenever `main` is updated.

## Quick start

Install the package that matches your serializer:

```bash
dotnet add package Json.Masker.Newtonsoft
# or
dotnet add package Json.Masker.SystemTextJson
```

### Wire it up

Pick the style that fits your app:

* **Use the plumbing extension** if you're already on `Microsoft.Extensions.DependencyInjection`. Call `services.AddJsonMasking()` from the matching package and it will register:
  * `IMaskingService` as a singleton, using either the built-in `DefaultMaskingService` or the instance you provide through `MaskingOptions`.
  * `IJsonMaskingConfigurator` as a singleton, wired to the serializer-specific implementation (`NewtonsoftJsonMaskingConfigurator` or `SystemTextJsonMaskingConfigurator`).

  Once that extension is in place you can inject `IJsonMaskingConfigurator` wherever you configure the serializer (for example in MVC setup) and let it bolt on the masking bits for you.
* **Wire it manually** if you're configuring the serializer yourself or aren't using DI. Just new up `DefaultMaskingService` (or your own implementation) and pass it into the resolver/modifier shown in the manual samples below.

Either way, masking kicks in once you mark your models and flip the context switch.

### Mask the model

1. Annotate the bits of your model that shouldn't leak.
   ```csharp
   public sealed class Customer
   {
       public string Name { get; set; } = string.Empty;

       [Sensitive(MaskingStrategy.Creditcard)]
       public string CreditCard { get; set; } = string.Empty;

       [Sensitive(MaskingStrategy.Ssn)]
       public string SSN { get; set; } = string.Empty;

       [Sensitive]
       public int Age { get; set; }

       [Sensitive(MaskingStrategy.Redacted)]
       public List<string> Hobbies { get; set; } = [];
   }
   ```
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
  "Age": "****",
  "Hobbies": ["<redacted>", "<redacted>"]
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

### Masking strategies

The built-in `DefaultMaskingService` supports a few common strategies:

| Strategy | Result |
| --- | --- |
| `MaskingStrategy.Default` | `****` |
| `MaskingStrategy.Creditcard` | `****-****-****-1234` (keeps the last four digits) |
| `MaskingStrategy.Ssn` | `***-**-6789` |
| `MaskingStrategy.Redacted` | `<redacted>` |

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
