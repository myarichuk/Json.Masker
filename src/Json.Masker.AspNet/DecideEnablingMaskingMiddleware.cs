using Json.Masker.Abstract;
using Microsoft.AspNetCore.Http;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Json.Masker.AspNet;

/// <summary>
/// A middleware 
/// </summary>
/// <param name="next">a delegate to execute next in chain of the delegate</param>
/// <param name="shouldMask">A delegate to decide whether to mask any serialization of the current context or not</param>
// ReSharper disable once ClassNeverInstantiated.Global
public class DecideEnablingMaskingMiddleware(RequestDelegate next, Func<HttpContext, bool>? shouldMask)
{
    private readonly Func<HttpContext, bool> _shouldMask = shouldMask ?? (_ => false);

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">HttpContext of the current, ongoing call</param>
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