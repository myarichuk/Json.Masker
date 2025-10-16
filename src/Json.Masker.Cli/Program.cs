using System.CommandLine.Builder;
using Json.Masker.Cli.Commands;

namespace Json.Masker.Cli;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = MaskCommandFactory.Create();
        var builder = new CommandLineBuilder(rootCommand)
            .UseDefaults();

        return await builder.Build().InvokeAsync(args).ConfigureAwait(false);
    }
}
