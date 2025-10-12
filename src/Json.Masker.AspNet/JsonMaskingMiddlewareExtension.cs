using Json.Masker.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
namespace Json.Masker.AspNet;

public static class JsonMaskingMiddlewareExtensions
{
    public static IApplicationBuilder UseTextJsonMasking(this IApplicationBuilder app, Func<HttpContext, bool>? shouldMask = null)
    {
        app.UseMiddleware<DecideEnablingMaskingMiddleware>(shouldMask ?? DefaultMaskingPredicate);
        var options = (IOptions<JsonOptions>?)app.ApplicationServices
            .GetService(typeof(IOptions<JsonOptions>));

        if (options != null)
        {
            var configurator = (IJsonMaskingConfigurator?)app.ApplicationServices
                .GetService(typeof(IJsonMaskingConfigurator));

            if (configurator != null)
            {
                // note: if we use Newtonsoft this would be NOP so no harm done
                configurator.Configure(options.Value.JsonSerializerOptions);
            }
            else
            {
                throw new InvalidOperationException($"Could not resolve the {nameof(IJsonMaskingConfigurator)} type. Did you call for {nameof(IServiceCollection)}::AddJsonMasking() when configuring the DI?");
            }
        }
        
        return app;
    }

    public static bool DefaultMaskingPredicate(HttpContext ctx)
    {
        if (!ctx.Request.Headers.TryGetValue("X-Json-Mask", out var value))
        {
            return false;
        }

        if (value.Count == 0)
        {
            return false;
        }
        
        var str = value[0] ?? string.Empty;
        return str.Equals("true", StringComparison.OrdinalIgnoreCase)
               || str.Equals("1")
               || str.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }
}