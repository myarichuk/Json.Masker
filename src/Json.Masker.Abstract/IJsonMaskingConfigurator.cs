namespace Json.Masker.Abstract;

/// <summary>
/// Configures JSON serializer settings with masking behavior.
/// </summary>
public interface IJsonMaskingConfigurator
{
    /// <summary>
    /// Applies masking configuration to the provided serializer settings object.
    /// </summary>
    /// <param name="settings">The serializer settings instance to configure.</param>
    void Configure(object settings);
}
