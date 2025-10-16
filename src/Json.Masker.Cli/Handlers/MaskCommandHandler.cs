using System.CommandLine.Invocation;
using Json.Masker.Cli.IO;

namespace Json.Masker.Cli.Handlers;

internal sealed class MaskCommandHandler
{
    private readonly InputResolver _inputResolver;
    private readonly OutputEmitter _outputEmitter;
    private readonly UsagePrinter _usagePrinter;

    public MaskCommandHandler(InputResolver inputResolver, OutputEmitter outputEmitter, UsagePrinter usagePrinter)
    {
        _inputResolver = inputResolver;
        _outputEmitter = outputEmitter;
        _usagePrinter = usagePrinter;
    }

    public async Task HandleAsync(FileInfo config, FileInfo? input, FileInfo? output, InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();
        var console = context.Console;

        await _usagePrinter.PrintUsageAsync(console).ConfigureAwait(false);

        if (!config.Exists)
        {
            await console.Error.WriteLineAsync($"Configuration file '{config.FullName}' does not exist.").ConfigureAwait(false);
            context.ExitCode = 2;
            return;
        }

        if (input is not null && !input.Exists)
        {
            await console.Error.WriteLineAsync($"Input file '{input.FullName}' does not exist.").ConfigureAwait(false);
            context.ExitCode = 2;
            return;
        }

        var configurationText = await File.ReadAllTextAsync(config.FullName, cancellationToken).ConfigureAwait(false);
        await console.Error.WriteLineAsync("Loaded JsonPath masking rules. Masking is not applied yet in this preview build.").ConfigureAwait(false);

        var payload = await _inputResolver.ResolvePayloadAsync(input, cancellationToken).ConfigureAwait(false);
        if (payload is null)
        {
            await console.Error.WriteLineAsync("No input detected. Provide --input <file> or pipe JSON into the process.").ConfigureAwait(false);
            context.ExitCode = 3;
            return;
        }

        await console.Error.WriteLineAsync("Json.Masker CLI is under active development. The current build only echoes the input payload without masking.").ConfigureAwait(false);

        await _outputEmitter.EmitAsync(payload, output, cancellationToken).ConfigureAwait(false);

        await console.Error.WriteLineAsync($"Configuration preview (first 120 chars): {_usagePrinter.CreatePreview(configurationText, 120)}").ConfigureAwait(false);
        context.ExitCode = 0;
    }
}
