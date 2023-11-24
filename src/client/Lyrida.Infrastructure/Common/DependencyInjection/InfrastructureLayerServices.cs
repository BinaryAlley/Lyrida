#region ========================================================================= USING =====================================================================================
using Autofac;
using Lyrida.Infrastructure.Localization;
using Lyrida.Infrastructure.Common.Logging;
using Lyrida.Infrastructure.Common.Security;
#endregion

namespace Lyrida.Infrastructure.Common.DependencyInjection;

/// <summary>
/// Class containing all services of the Infrastructure Layer
/// </summary>
/// <remarks>
/// Creation Date: 04th of July, 2023
/// </remarks>
public class InfrastructureLayerServices : Module
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the services of the Infrastructure Layer into the Dependency Injection container
    /// </summary>
    /// <param name="builder">The Dependency Injection container where the services are registered</param>
    protected override void Load(ContainerBuilder builder)
    {        
        builder.RegisterType<ProxyInterceptor>().InstancePerLifetimeScope();
        builder.RegisterType<AsyncProxyInterceptor>().InstancePerLifetimeScope();
        builder.RegisterType<NLogLogger>().As<ILoggerManager>().InstancePerLifetimeScope();
        builder.RegisterType<Cryptography>().As<ICryptography>().InstancePerLifetimeScope();
        builder.RegisterType<TranslationService>().As<ITranslationService>().InstancePerLifetimeScope();
    }
    #endregion
}