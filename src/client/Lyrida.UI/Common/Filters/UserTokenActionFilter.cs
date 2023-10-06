#region ========================================================================= USING =====================================================================================
using Microsoft.AspNetCore.Mvc.Filters;
#endregion

namespace Lyrida.UI.Common.Filters;

/// <summary>
/// Fetches user-related settings like the UserToken and LanguagePreference and places them in the HttpContext.Items collection
/// </summary>
/// <remarks>
/// Creation Date: 07th of September, 2023
/// </remarks>
public class UserTokenActionFilter : IActionFilter
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// OnActionExecuting is called before the action method is invoked. It's used here to fetch the user token and language preference and place them in HttpContext.Items
    /// </summary>
    /// <param name="context">Provides the context for the action filter</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["UserToken"] = context.HttpContext.User?.FindFirst("Token")?.Value;
    }

    /// <summary>
    /// OnActionExecuted is called after the action method is invoked. It's not implemented here since there's no post-action behavior needed for this filter in this context
    /// </summary>
    /// <param name="context">Provides the context for the action filter</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // no implementation needed here
    }
    #endregion
}