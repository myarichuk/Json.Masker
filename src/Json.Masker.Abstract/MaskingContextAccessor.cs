using System.Threading;

namespace Json.Masker.Abstract;

/// <summary>
/// Provides access to the current <see cref="MaskingContext"/> for the executing asynchronous flow.
/// </summary>
public static class MaskingContextAccessor
{
    private static readonly AsyncLocal<MaskingContext?> Ctx = new();

    /// <summary>
    /// Gets the active masking context, defaulting to a disabled context when none has been set.
    /// </summary>
    public static MaskingContext Current =>
        Ctx.Value ?? new MaskingContext { Enabled = false };

    /// <summary>
    /// Sets the masking context for the current asynchronous flow.
    /// </summary>
    /// <param name="ctx">The context to apply.</param>
    public static void Set(MaskingContext ctx) =>
        Ctx.Value = ctx;
}
