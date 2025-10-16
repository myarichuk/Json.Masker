namespace Json.Masker.Cli.IO;

internal sealed class InputResolver
{
    public async Task<string?> ResolvePayloadAsync(FileInfo? input, CancellationToken cancellationToken)
    {
        if (input is not null)
        {
            return await File.ReadAllTextAsync(input.FullName, cancellationToken).ConfigureAwait(false);
        }

        if (!Console.IsInputRedirected)
        {
            return null;
        }

        return await Console.In.ReadToEndAsync().WaitAsync(cancellationToken).ConfigureAwait(false);
    }
}
