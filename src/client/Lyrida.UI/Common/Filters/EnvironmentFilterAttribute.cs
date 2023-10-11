#region ========================================================================= USING =====================================================================================
using Microsoft.AspNetCore.Mvc.Filters;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.UI.Common.Filters;

/// <summary>
/// A filter attribute that determines and sets the environment and platform metadata for an action.
/// </summary>
/// <remarks>
/// Creation Date: 11th of October, 2023
/// </remarks>
public class EnvironmentFilterAttribute : ActionFilterAttribute
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Overrides the action execution process to inject environment and platform metadata.
    /// </summary>
    /// <param name="context">The action execution context.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {      
        // try to get the value of environmentId and ensure it can be cast to an integer
        //if (context.ActionArguments.TryGetValue("environmentId", out var environmentIdObj) && environmentIdObj is int environmentId)
        if (context.HttpContext.Request.Headers.TryGetValue("X-Environment-Type", out var environmentIdValue) && int.TryParse(environmentIdValue, out int environmentId))
        {
            // default values for the environment and platform
            EnvironmentType environment = EnvironmentType.LocalFileSystem;
            PlatformType platform = PlatformType.Unix;
            // determine the appropriate platform and environment based on the provided environmentId
            if (environmentId == 2)
                platform = PlatformType.Windows;
            else if (environmentId == 3)
                environment = EnvironmentType.Ftp;
            else if (environmentId == 4)
                environment = EnvironmentType.GoogleDrive;
            // store the determined values in the HttpContext.Items for access during the request's lifecycle
            context.HttpContext.Items["Environment"] = environment;
            context.HttpContext.Items["Platform"] = platform;
        }
        // call the base method to continue the action execution pipeline
        base.OnActionExecuting(context);
    }
    #endregion
}