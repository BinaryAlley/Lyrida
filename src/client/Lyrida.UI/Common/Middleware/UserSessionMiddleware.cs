#region ========================================================================= USING =====================================================================================
using Autofac;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Autofac.Extensions.DependencyInjection;
#endregion

namespace Lyrida.UI.Common.Middleware;

/// <summary>
/// Middleware for managing user sessions
/// </summary>
/// <remarks>
/// Creation Date: 15th of June, 2023
/// </remarks>
internal class UserSessionMiddleware
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    /// <summary>
    /// The next delegate in the middleware pipeline
    /// </summary>
    private readonly RequestDelegate next;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Constructor that takes the next delegate in the middleware pipeline.
    /// </summary>
    /// <param name="next">The next delegate in the middleware pipeline</param>
    public UserSessionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Middleware pipeline method to manage user sessions.
    /// Starts a new lifetime scope for the user session, then passes the control to the next middleware in the pipeline.
    /// </summary>
    /// <param name="context">The current HttpContext for the middleware pipeline</param>
    /// <param name="lifetimeScope">The lifetime scope associated with the current context</param>
    /// <returns>A Task representing the completion of the middleware operation</returns>
    public async Task InvokeAsync(HttpContext context, ILifetimeScope lifetimeScope)
    {
        // Begin a new lifetime scope for the user session
        using (var scope = lifetimeScope.BeginLifetimeScope("userSession"))
        {
            // Assign the new service provider to the HttpContext's RequestServices to handle service requests within the user session's lifetime scope
            context.RequestServices = new AutofacServiceProvider(scope);
            // Pass control to the next middleware in the pipeline
            await next(context);
        }
    }
    #endregion
}