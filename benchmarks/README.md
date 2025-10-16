# Benchmarks

The `benchmarks` folder hosts two independent BenchmarkDotNet runners that make
it easy to analyse Json.Masker's performance characteristics.

## Projects

| Project | Description |
| --- | --- |
| `MaskingServiceBenchmarks` | Micro-benchmarks that exercise `DefaultMaskingService` strategies directly to determine the intrinsic cost of each masking operation. |
| `SerializationBenchmarks` | End-to-end serialization benchmarks that compare Json.Masker with other masking libraries such as JsonDataMasking, JsonMasking, and Byndyusoft.MaskedSerialization. |

## Running

Each project is a standalone executable targeting .NET 8.0. From the repository
root you can execute them with:

```bash
# Run the micro-benchmarks
dotnet run --project benchmarks/MaskingServiceBenchmarks/MaskingServiceBenchmarks.csproj -c Release

# Run the serialization comparison suite
# (requires the JsonDataMasking.dll included next to the benchmarks)
dotnet run --project benchmarks/SerializationBenchmarks/SerializationBenchmarks.csproj -c Release
```

The serialization suite documents the GitHub repositories for each competing
library in the benchmark source files and explains important behavioural
differences—such as Byndyusoft.MaskedSerialization skipping collections and not
respecting existing converters—so results can be interpreted correctly.
