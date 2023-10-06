#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.Api.Common.Http;
using Microsoft.AspNetCore.Http;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.Services.Platform;
#endregion

namespace Lyrida.Api.Common.Middleware;

/// <summary>
/// Middleware for setting the current platform
/// </summary>
/// <remarks>
/// Creation Date: 04th of October, 2023
/// </remarks>
public class PlatformMiddleware
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly RequestDelegate next;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Constructor that takes the next delegate in the middleware pipeline.
    /// </summary>
    /// <param name="next">The next delegate in the middleware pipeline</param>
    public PlatformMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Middleware pipeline method to check and set the platform
    /// </summary>
    /// <param name="context">The current HttpContext for the middleware pipeline</param>
    /// <param name="platformContextManager">The platform context manager used to set the current filesystem platform</param>
    /// <returns>A Task representing the completion of the middleware operation</returns>
    public async Task InvokeAsync(HttpContext context, IPlatformContextManager platformContextManager)
    {
        var platform = context.Request.Headers["X-Platform-Type"].ToString();
        if (!string.IsNullOrEmpty(platform))
        {
            context.Items[HttpContextItemKeys.PLATFORM] = platform;
            platformContextManager.SetCurrentPlatform((PlatformType)Enum.Parse(typeof(PlatformType), platform));
        }
        else
        {
            context.Items[HttpContextItemKeys.PLATFORM] = "Unix";
            platformContextManager.SetCurrentPlatform(PlatformType.Unix);
        }
        await Console.Out.WriteLineAsync("Platform set to: " + platform);
        await next(context);
    }
    #endregion
}