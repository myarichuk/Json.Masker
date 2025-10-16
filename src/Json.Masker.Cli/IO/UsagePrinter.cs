using System.CommandLine;

namespace Json.Masker.Cli.IO;

internal sealed class UsagePrinter
{
    private const string UsageMessage = "Usage: json-masker --config <rules.json> [--input <payload.json>] [--output <masked.json>]";

    public async Task PrintUsageAsync(IConsole console)
    {
        await console.Error.WriteLineAsync(UsageMessage).ConfigureAwait(false);
        await console.Error.WriteLineAsync("  --config <rules.json> (required) JsonPath-based masking rules to apply.").ConfigureAwait(false);
        await console.Error.WriteLineAsync("  --input <payload.json>  Optional JSON payload. Omit to read from STDIN.").ConfigureAwait(false);
        await console.Error.WriteLineAsync("  --output <masked.json> Optional destination file. Omit to write to STDOUT.").ConfigureAwait(false);
    }

    public string CreatePreview(string text, int maxLength)
    {
        if (text.Length <= maxLength)
        {
            return text;
        }

        return string.Concat(text.AsSpan(0, maxLength), "…");
    }
}
