using Json.Masker.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Json.Masker.Newtonsoft;

public static class PlumbingExtensions
{
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