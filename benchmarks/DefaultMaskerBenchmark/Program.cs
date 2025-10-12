using BenchmarkDotNet.Running;

namespace DefaultMaskerBenchmark;

/// <summary>
/// Entry point for running the benchmark suite.
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
