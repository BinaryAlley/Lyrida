#region ========================================================================= USING =====================================================================================
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Lyrida.UI.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
#endregion

namespace Lyrida.UI.Common.Filters;

/// <summary>
/// Filters API exceptions and provides centralized handling for API-related errors
/// </summary>
/// <remarks>
/// Creation Date: 07th of September, 2023
/// </remarks>
public class ApiExceptionFilter : IExceptionFilter
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Called after an action has thrown an Exception
    /// </summary>
    /// <param name="context">The <see cref="ExceptionContext"/></param>
    public void OnException(ExceptionContext context)
    {
        // check if the exception is an ApiException
        if (context.Exception is ApiException apiException)
        {
            // handle unauthorized errors by prompting the user to re-login
            if (apiException.Error?.Status == 401)
            {
                SignOutSynchronously(context.HttpContext);
                var currentUrl = context.HttpContext.Request.GetDisplayUrl();
                context.HttpContext.Response.Redirect(currentUrl); // force an entire refresh of the current location, so that header and footer are re-rendered too
            }
            else
            {
                // return a JSON result with the error details
                context.Result = new JsonResult(new
                {
                    success = false,
                    errorMessage = apiException.Message + " " + apiException.Error?.Errors?.First()
                });
            }
            // mark the exception as handled to prevent propagation
            context.ExceptionHandled = true;
        }
    }

    /// <summary>
    /// Signs out the user synchronously by deleting the authentication cookie and token.
    /// </summary>
    /// <param name="httpContext">The HttpContext for the current request.</param>
    private static void SignOutSynchronously(HttpContext httpContext)
    {
        var task = httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        task.Wait();  // run the task synchronously
        httpContext.Response.Cookies.Delete("Token");
    }
    #endregion
}