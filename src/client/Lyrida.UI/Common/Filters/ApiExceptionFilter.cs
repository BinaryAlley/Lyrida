#region ========================================================================= USING =====================================================================================
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Lyrida.UI.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="translationService">Injected service for translations</param>
    public ApiExceptionFilter(ITranslationService translationService)
    {
        this.translationService = translationService;
    }
    #endregion

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
                context.Result = new PartialViewResult
                {
                    ViewName = "_LoginPartial",
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "loginError", translationService.Translate(Terms.SessionExpired) },
                        { "ReturnUrl", context.HttpContext.Request.Path }
                    }
                };
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
    #endregion
}