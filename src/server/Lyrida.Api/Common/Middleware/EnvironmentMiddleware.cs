#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.Api.Common.Http;
using Microsoft.AspNetCore.Http;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.Services.Environment;
#endregion

namespace Lyrida.Api.Common.Middleware;

/// <summary>
/// Middleware for setting the current file system environment
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public class EnvironmentMiddleware
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly RequestDelegate next;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Constructor that takes the next delegate in the middleware pipeline.
    /// </summary>
    /// <param name="next">The next delegate in the middleware pipeline</param>
    public EnvironmentMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Middleware pipeline method to check and set the filesystem environment
    /// </summary>
    /// <param name="context">The current HttpContext for the middleware pipeline</param>
    /// <param name="environmentContextManager">The environment context manager used to set the current filesystem environment</param>
    /// <returns>A Task representing the completion of the middleware operation</returns>
    public async Task InvokeAsync(HttpContext context, IEnvironmentContextManager environmentContextManager)
    {
        var environment = context.Request.Headers["X-Environment-Type"].ToString();
        if (!string.IsNullOrEmpty(environment))
        {
            context.Items[HttpContextItemKeys.ENVIRONMENT] = environment;
            environmentContextManager.SetCurrentEnvironment((EnvironmentType)Enum.Parse(typeof(EnvironmentType), environment));
        }
        else
        {
            context.Items[HttpContextItemKeys.ENVIRONMENT] = "LocalFileSystem";
            environmentContextManager.SetCurrentEnvironment(EnvironmentType.LocalFileSystem);
        }
        await Console.Out.WriteLineAsync("Environment set to: " + environment);
        await next(context);
    }
    #endregion
}