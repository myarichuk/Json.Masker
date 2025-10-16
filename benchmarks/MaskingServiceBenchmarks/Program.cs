using BenchmarkDotNet.Running;

namespace MaskingServiceBenchmarks;

/// <summary>
/// Entry point for running the masking service micro-benchmarks.
/// </summary>
public static class Program
{
    /// <summary>
    /// Executes the benchmarks using BenchmarkDotNet.
    /// </summary>
    /// <param name="args">Command-line arguments forwarded to BenchmarkDotNet.</param>
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
