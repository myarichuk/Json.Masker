namespace Json.Masker.Abstract;

public static class MaskingContextAccessor
{
    private static readonly AsyncLocal<MaskingContext?> Ctx = new();

    public static MaskingContext Current => 
        Ctx.Value ?? new MaskingContext { Enabled = false };

    public static void Set(MaskingContext ctx) => 
        Ctx.Value = ctx;
}