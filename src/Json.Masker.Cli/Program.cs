using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace Json.Masker.Cli;

/// <summary>
/// Entry point for the Json.Masker CLI.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Runs the command-line application.
    /// </summary>
    /// <param name="args">Raw arguments passed to the process.</param>
    /// <returns>The exit code returned to the operating system.</returns>
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = CreateRootCommand();
        var builder = new CommandLineBuilder(rootCommand)
            .UseDefaults();

        return await builder.Build().InvokeAsync(args).ConfigureAwait(false);
    }

    /// <summary>
    /// Builds the root command for the CLI.
    /// </summary>
    /// <returns>The configured root command.</returns>
    internal static RootCommand CreateRootCommand()
    {
        var configOption = new Option<FileInfo?>(
            name: "--config",
            description: "Path to a JSON configuration file describing JsonPath masking rules.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
        configOption.AddAlias("-c");

        var inputOption = new Option<FileInfo?>(
            name: "--input",
            description: "Optional path to a JSON payload. When omitted, the CLI reads from standard input so it works in Unix pipelines.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
        inputOption.AddAlias("-i");

        var outputOption = new Option<FileInfo?>(
            name: "--output",
            description: "Optional path for the masked output. When omitted, the CLI writes to standard output.")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
        outputOption.AddAlias("-o");

        var root = new RootCommand("Json.Masker CLI (work in progress). Accepts JSON input, applies JsonPath-based masking rules, and writes the result to standard output.")
        {
            configOption,
            inputOption,
            outputOption,
        };

        root.SetHandler(
            async (FileInfo? config, FileInfo? input, FileInfo? output, InvocationContext context) =>
            {
                var cancellationToken = context.GetCancellationToken();

                if (config is not null && !config.Exists)
                {
                    await context.Console.Error.WriteLineAsync($"Configuration file '{config.FullName}' does not exist.").ConfigureAwait(false);
                    context.ExitCode = 2;
                    return;
                }

                if (input is not null && !input.Exists)
                {
                    await context.Console.Error.WriteLineAsync($"Input file '{input.FullName}' does not exist.").ConfigureAwait(false);
                    context.ExitCode = 2;
                    return;
                }

                string? configurationPreview = null;
                if (config is not null)
                {
                    configurationPreview = await ReadFileAsync(config, cancellationToken).ConfigureAwait(false);
                    await context.Console.Error.WriteLineAsync($"Loaded configuration from '{config.FullName}'. JsonPath rules are not applied yet.").ConfigureAwait(false);
                }
                else
                {
                    await context.Console.Error.WriteLineAsync("No configuration supplied. Future releases will expect JsonPath masking rules via --config or other inputs.").ConfigureAwait(false);
                }

                var payload = await ResolvePayloadAsync(input, cancellationToken).ConfigureAwait(false);
                if (payload is null)
                {
                    await context.Console.Error.WriteLineAsync("No input detected. Provide --input <file> or pipe JSON into the process.").ConfigureAwait(false);
                    context.ExitCode = 3;
                    return;
                }

                await context.Console.Error.WriteLineAsync("Json.Masker CLI is under active development. The current build only echoes the input payload without masking.").ConfigureAwait(false);

                await WriteOutputAsync(payload, output, cancellationToken).ConfigureAwait(false);

                if (configurationPreview is not null)
                {
                    await context.Console.Error.WriteLineAsync($"Configuration preview (first 120 chars): {TrimPreview(configurationPreview, 120)}").ConfigureAwait(false);
                }

                context.ExitCode = 0;
            },
            configOption,
            inputOption,
            outputOption);

        return root;
    }

    /// <summary>
    /// Resolves the payload source.
    /// </summary>
    /// <param name="input">An optional file path containing JSON to mask.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>The payload text or <see langword="null"/> when no input is available.</returns>
    private static async Task<string?> ResolvePayloadAsync(FileInfo? input, CancellationToken cancellationToken)
    {
        if (input is not null)
        {
            return await ReadFileAsync(input, cancellationToken).ConfigureAwait(false);
        }

        if (!Console.IsInputRedirected)
        {
            return null;
        }

        var reader = Console.In;
        return await reader.ReadToEndAsync().WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Writes the final payload either to standard output or to the provided file.
    /// </summary>
    /// <param name="payload">The JSON payload to write.</param>
    /// <param name="output">Optional output file.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    private static async Task WriteOutputAsync(string payload, FileInfo? output, CancellationToken cancellationToken)
    {
        if (output is null)
        {
            await Console.Out.WriteAsync(payload).ConfigureAwait(false);
            if (!payload.EndsWith(Environment.NewLine, StringComparison.Ordinal))
            {
                await Console.Out.WriteLineAsync().ConfigureAwait(false);
            }

            return;
        }

        output.Directory?.Create();
        await File.WriteAllTextAsync(output.FullName, payload, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads all text from a file using asynchronous IO.
    /// </summary>
    /// <param name="file">The file to read.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>The file contents.</returns>
    private static async Task<string> ReadFileAsync(FileInfo file, CancellationToken cancellationToken)
    {
        return await File.ReadAllTextAsync(file.FullName, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a shortened preview of configuration text for diagnostic output.
    /// </summary>
    /// <param name="text">The text to shorten.</param>
    /// <param name="maxLength">The maximum allowed length of the preview.</param>
    /// <returns>The preview string.</returns>
    private static string TrimPreview(string text, int maxLength)
    {
        if (text.Length <= maxLength)
        {
            return text;
        }

        return string.Concat(text.AsSpan(0, maxLength), "…");
    }
}
