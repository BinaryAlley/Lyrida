#region ========================================================================= USING =====================================================================================
using Autofac;
using Lyrida.UI.Common.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
#endregion

namespace Lyrida.UI.Common.DependencyInjection;

/// <summary>
/// Web app composition root
/// </summary>
/// <remarks>
/// Creation Date: 25th of July, 2023
/// </remarks>
public class PresentationLayerServices : Module
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the services of the UI Layer into the Dependency Injection container
    /// </summary>
    /// <param name="builder">The Dependency Injection container where the services are registered</param>
    protected override void Load(ContainerBuilder builder)
    {
        // register the filters needed for cross cutting concerns such as error handling or user token, needed in the interaction with the API
        builder.RegisterType<ApiExceptionFilter>().As<IFilterMetadata>().InstancePerLifetimeScope();
        builder.RegisterType<UserTokenActionFilter>().As<IFilterMetadata>().InstancePerLifetimeScope();
    }
    #endregion
}