using Json.Masker.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Json.Masker.AspNet.Newtonsoft;

public static class JsonMaskingMiddlewareExtensions
{
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