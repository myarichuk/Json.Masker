using Json.Masker.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Json.Masker.AspNet.Newtonsoft;

/// <summary>
/// Extension methods that enable JSON masking for ASP.NET Core applications that use Newtonsoft.Json.
/// </summary>
public static class JsonMaskingMiddlewareExtensions
{
    /// <summary>
    /// Registers middleware that controls masking and updates MVC's <see cref="MvcNewtonsoftJsonOptions"/> to use masked output.
    /// </summary>
    /// <param name="app">The application builder used to configure the HTTP request pipeline.</param>
    /// <param name="shouldMask">Optional predicate that decides whether masking is enabled for the current request.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance to allow fluent calls.</returns>
    public static IApplicationBuilder UseNewtonsoftJsonMasking(
        this IApplicationBuilder app,
        Func<HttpContext, bool>? shouldMask = null)
    {
        app.UseMiddleware<DecideEnablingMaskingMiddleware>(shouldMask ?? Json.Masker.AspNet.JsonMaskingMiddlewareExtensions.DefaultMaskingPredicate);
        var options = (IOptions<MvcNewtonsoftJsonOptions>?)app.ApplicationServices
            .GetService(typeof(IOptions<MvcNewtonsoftJsonOptions>));

        if (options != null)
        {
            var configurator = (IJsonMaskingConfigurator?)app.ApplicationServices
                .GetService(typeof(IJsonMaskingConfigurator));

            configurator?.Configure(options.Value);
        }

        return app;
    }
}
