namespace Json.Masker.Abstract;

public interface IMaskingService
{
    string Mask(object? value, MaskingStrategy strategy, MaskingContext ctx);
}