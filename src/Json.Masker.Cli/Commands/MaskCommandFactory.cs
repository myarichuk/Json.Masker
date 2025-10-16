using System.CommandLine;
using System.CommandLine.Invocation;
using Json.Masker.Cli.Handlers;
using Json.Masker.Cli.IO;

namespace Json.Masker.Cli.Commands;

internal static class MaskCommandFactory
{
    public static RootCommand Create()
    {
        var configOption = new Option<FileInfo>(
            name: "--config",
            description: "Path to a JSON configuration file describing JsonPath masking rules.")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true,
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

        var handler = new MaskCommandHandler(
            new InputResolver(),
            new OutputEmitter(),
            new UsagePrinter());

        root.SetHandler(
            (InvocationContext context, FileInfo config, FileInfo? input, FileInfo? output) => handler.HandleAsync(config, input, output, context),
            configOption,
            inputOption,
            outputOption);

        return root;
    }
}
