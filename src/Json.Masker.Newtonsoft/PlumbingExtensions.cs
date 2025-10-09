using Json.Masker.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Json.Masker.Newtonsoft;

/// <summary>
/// Provides dependency injection helpers for enabling Newtonsoft.Json masking.
/// </summary>
public static class PlumbingExtensions
{
    /// <summary>
    /// Adds JSON masking services configured for Newtonsoft.Json.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configure">An optional delegate to customize masking options.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddJsonMasking(
        this IServiceCollection services,
        Action<MaskingOptions>? configure = null)
    {
        var opts = new MaskingOptions();
        configure?.Invoke(opts);

        services.AddSingleton(opts.MaskingService ?? new DefaultMaskingService());
        services.AddSingleton<IJsonMaskingConfigurator, NewtonsoftJsonMaskingConfigurator>();

        return services;
    }
}
