namespace Json.Masker.Abstract;

public static class MaskingContextAccessor
{
    private static readonly AsyncLocal<MaskingContext?> _ctx = new();
    public static MaskingContext Current => _ctx.Value ?? new MaskingContext { Enabled = false };

    public static void Set(MaskingContext ctx) => _ctx.Value = ctx;
}