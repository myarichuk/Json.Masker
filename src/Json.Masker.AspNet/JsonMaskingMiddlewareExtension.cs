using Json.Masker.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable RedundantIfElseBlock
// ReSharper disable UnusedMethodReturnValue.Global
namespace Json.Masker.AspNet;

/// <summary>
/// Extension methods that enable JSON masking for ASP.NET Core applications using <c>System.Text.Json</c>.
/// </summary>
public static class JsonMaskingMiddlewareExtensions
{
    /// <summary>
    /// Registers middleware that toggles masking and ensures the configured JSON serializer uses masking converters.
    /// </summary>
    /// <param name="app">The application builder used to configure the HTTP request pipeline.</param>
    /// <param name="shouldMask">Optional predicate that decides whether masking is enabled for the current request.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance to allow fluent calls.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="IJsonMaskingConfigurator"/> has not been registered.</exception>
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

    /// <summary>
    /// Default predicate used to determine whether masking should be enabled for a request.
    /// </summary>
    /// <param name="ctx">The <see cref="HttpContext"/> associated with the current request.</param>
    /// <returns><see langword="true"/> when the <c>X-Json-Mask</c> header requests masking; otherwise, <see langword="false"/>.</returns>
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
