using Json.Masker.Abstract;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
namespace Json.Masker.SystemTextJson;

/// <summary>
/// Provides explicit registration for System.Text.Json masking backend.
/// </summary>
public static class SystemTextJsonMaskingExtensions
{
    /// <summary>
    /// Adds JSON masking services configured for System.Text.Json.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configure">Optional delegate to customize masking options.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddSystemTextJsonMasking(
        this IServiceCollection services,
        Action<MaskingOptions>? configure = null)
        => services.AddJsonMasking(configure);
}