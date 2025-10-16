using Json.Masker.Abstract;
using Microsoft.AspNetCore.Http;

namespace Json.Masker.AspNet;

/// <summary>
/// Middleware that toggles the ambient <see cref="MaskingContext"/> for a request based on a predicate.
/// </summary>
/// <param name="next">The next middleware delegate in the ASP.NET Core pipeline.</param>
/// <param name="shouldMask">Predicate that determines whether masking should be enabled for the current request.</param>
// ReSharper disable once ClassNeverInstantiated.Global
public class DecideEnablingMaskingMiddleware(RequestDelegate next, Func<HttpContext, bool>? shouldMask)
{
    private readonly Func<HttpContext, bool> _shouldMask = shouldMask ?? (_ => false);

    /// <summary>
    /// Executes the middleware pipeline while applying the masking context if required.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var currentEnabled = MaskingContextAccessor.Current.Enabled;
        if (_shouldMask(context))
        {
            MaskingContextAccessor.Set(new MaskingContext { Enabled = true });
        }

        try
        {
            await next(context);
        }
        finally
        {
            MaskingContextAccessor.Set(new MaskingContext { Enabled = currentEnabled });
        }
    }
}
