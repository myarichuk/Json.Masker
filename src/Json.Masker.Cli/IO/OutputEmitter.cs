namespace Json.Masker.Cli.IO;

internal sealed class OutputEmitter
{
    public async Task EmitAsync(string payload, FileInfo? output, CancellationToken cancellationToken)
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
}
