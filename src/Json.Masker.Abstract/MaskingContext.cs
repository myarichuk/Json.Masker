namespace Json.Masker.Abstract;

// ambient request-scoped context (should be set in middleware)
public sealed class MaskingContext
{
    public bool Enabled { get; init; }
    public string? Role { get; init; }
}