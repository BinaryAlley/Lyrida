#region ========================================================================= USING =====================================================================================
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.Application.Core.Authorization;
#endregion

namespace Lyrida.Api.Common.Middleware;

/// <summary>
/// Middleware for setting the authorization service 
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public class AuthorizationMiddleware
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
    public AuthorizationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Middleware pipeline method to check and set the authorization of operations
    /// </summary>
    /// <param name="context">The current HttpContext for the middleware pipeline</param>
    /// <param name="authorizationService">The service used to authorize operations</param>
    /// <param name="translationService">Service used for translations</param>
    /// <returns>A Task representing the completion of the middleware operation</returns>
    public async Task InvokeAsync(HttpContext context, IAuthorizationService authorizationService, ITranslationService translationService)
    {
        // make sure the user is authenticated before authorizing stuff
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            // get the id of the authenticated user
            int userId = int.Parse(context.User?.FindFirst(ClaimTypes.NameIdentifier)!.Value!);
            // get the list of roles and permissions of the authenticated user
            var getResult = await authorizationService.GetUserPermissionsAsync(userId);
            if (getResult.IsError || getResult.Value == false)
            {
                // if we get here, there is something seriously wrong - authorization service was unable to get the user permissions
                // create a ProblemDetails object
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = translationService.Translate(Terms.InternalServerError),
                    Instance = context.Request.Path
                };
                // serialize the ProblemDetails object to JSON
                var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
                // write the JSON response
                context.Response.StatusCode = problemDetails.Status.Value;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(problemDetailsJson);
                // signal that the response is complete
                await context.Response.CompleteAsync();
                // return immediately to short-circuit request processing
                return;
            }
        }
        await next(context);
    }
    #endregion
}