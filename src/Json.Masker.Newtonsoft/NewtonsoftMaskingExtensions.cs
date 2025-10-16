using Json.Masker.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
namespace Json.Masker.Newtonsoft;

/// <summary>
/// Provides explicit registration helpers for enabling JSON masking with Newtonsoft.Json.
/// </summary>
public static class NewtonsoftMaskingExtensions
{
    /// <summary>
    /// Adds JSON masking services configured for the <see cref="JsonSerializer"/> serializer.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which JSON masking services will be added.
    /// </param>
    /// <param name="configure">
    /// An optional delegate for customizing <see cref="MaskingOptions"/> during registration.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance, enabling fluent configuration.
    /// </returns>
    /// <remarks>
    /// This method explicitly registers the Newtonsoft.Json implementation of JSON masking,
    /// serving as a clearer alternative to <c>AddJsonMasking()</c> to avoid namespace ambiguity
    /// when multiple serializer backends are referenced in the same project.
    /// </remarks>
    public static IServiceCollection AddNewtonsoftJsonMasking(
        this IServiceCollection services,
        Action<MaskingOptions>? configure = null)
        => services.AddJsonMasking(configure);
}
