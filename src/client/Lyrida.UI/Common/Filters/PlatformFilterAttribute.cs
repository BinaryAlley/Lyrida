#region ========================================================================= USING =====================================================================================
using Microsoft.AspNetCore.Mvc.Filters;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.UI.Common.Filters;

/// <summary>
/// A filter attribute that determines and sets the platform metadata for an action.
/// </summary>
/// <remarks>
/// Creation Date: 22nd of November, 2023
/// </remarks>
public class PlatformFilterAttribute : ActionFilterAttribute
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Overrides the action execution process to inject platform metadata.
    /// </summary>
    /// <param name="context">The action execution context.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("X-Platform-Type", out var platformTypeValue))
        {
            // default value
            PlatformType platform = PlatformType.Unix;
            if (platformTypeValue == "Windows")
                platform = PlatformType.Windows;
            // store the determined value in the HttpContext.Items for access during the request's lifecycle
            context.HttpContext.Items["Platform"] = platform;
        }
        // call the base method to continue the action execution pipeline
        base.OnActionExecuting(context);
    }
    #endregion
}