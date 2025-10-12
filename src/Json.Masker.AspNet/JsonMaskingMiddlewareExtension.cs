using Json.Masker.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable RedundantIfElseBlock
// ReSharper disable UnusedMethodReturnValue.Global
namespace Json.Masker.AspNet;

public static class JsonMaskingMiddlewareExtensions
{
    public static IApplicationBuilder UseTextJsonMasking(this IApplicationBuilder app, Func<HttpContext, bool>? shouldMask = null)
    {
        app.UseMiddleware<DecideEnablingMaskingMiddleware>(shouldMask ?? DefaultMaskingPredicate);

        var configurator = app.ApplicationServices.GetService<IJsonMaskingConfigurator>();
        if (configurator == null)
        {
            throw new InvalidOperationException(
                $"Could not resolve {nameof(IJsonMaskingConfigurator)}. Did you call AddJsonMasking() when configuring services?");
        }

        // try MVC and Minimal API
        var mvcOptions = app.ApplicationServices.GetService<IOptions<Microsoft.AspNetCore.Mvc.JsonOptions>>();
        var httpOptions = app.ApplicationServices.GetService<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>();

        if (mvcOptions is not null)
        {
            configurator.Configure(mvcOptions.Value.JsonSerializerOptions);
        }
        else if (httpOptions is not null)
        {
            configurator.Configure(httpOptions.Value.SerializerOptions);
        }
        else
        {
            // maybe log? could be using Newtonsoft or a custom serializer
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
