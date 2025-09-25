namespace Json.Masker.Abstract;

public interface IMaskingService
{
    string Mask(object? value, string strategy, MaskingContext ctx);
}