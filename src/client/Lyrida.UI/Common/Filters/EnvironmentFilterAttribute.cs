#region ========================================================================= USING =====================================================================================
using Microsoft.AspNetCore.Mvc.Filters;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.UI.Common.Filters;

/// <summary>
/// A filter attribute that determines and sets the environment metadata for an action.
/// </summary>
/// <remarks>
/// Creation Date: 11th of October, 2023
/// </remarks>
public class EnvironmentFilterAttribute : ActionFilterAttribute
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Overrides the action execution process to inject environment metadata.
    /// </summary>
    /// <param name="context">The action execution context.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {      
        if (context.HttpContext.Request.Headers.TryGetValue("X-Environment-Type", out var environmentTypeValue))
        {
            // default value for the environment
            EnvironmentType environment = EnvironmentType.LocalFileSystem;
            // determine the appropriate environment based on the provided environmentId
            if (environmentTypeValue == "local")
                environment = EnvironmentType.LocalFileSystem;
            else if (environmentTypeValue == "ftp")
                environment = EnvironmentType.Ftp;
            else if (environmentTypeValue == "gdrive")
                environment = EnvironmentType.GoogleDrive;
            // store the determined value in the HttpContext.Items for access during the request's lifecycle
            context.HttpContext.Items["Environment"] = environment;
        }
        // call the base method to continue the action execution pipeline
        base.OnActionExecuting(context);
    }
    #endregion
}