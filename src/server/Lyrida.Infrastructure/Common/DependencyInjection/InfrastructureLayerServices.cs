﻿#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using System.IO;
using Newtonsoft.Json;
using Autofac.Extras.DynamicProxy;
using Lyrida.Infrastructure.Common.Time;
using Lyrida.Infrastructure.Localization;
using Lyrida.Infrastructure.Common.Logging;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Core.Services.Time;
using Lyrida.Infrastructure.Common.Notification;
using Lyrida.Infrastructure.Core.Authentication;
using Lyrida.Infrastructure.Common.Configuration;
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
        string configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        if (File.Exists(configurationFilePath))
        {
            AppConfig configuration = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(configurationFilePath)) ?? throw new InvalidOperationException("Cannot deserialize appsettings.json!");
            configuration.ConfigurationFilePath = configurationFilePath;
            builder.Register(context => configuration)
                   .OnActivating(e => e.Instance!.ConfigurationFilePath = configurationFilePath)
                   .As<IAppConfig>()
                   .InstancePerLifetimeScope();
            // re-save the configuration (the configuration file might contain properties that were not previously present in the JSON file, those need to be serialized too)
            configuration.UpdateConfiguration();
        }
        else
            throw new FileNotFoundException("Configuration file not found!\nPath: " + configurationFilePath);
        builder.RegisterType<Hash>().As<IHash>().InstancePerLifetimeScope();
        builder.RegisterType<ProxyInterceptor>().InstancePerLifetimeScope();
        builder.RegisterType<TimeService>().As<ITimeService>().SingleInstance();
        builder.RegisterType<AsyncProxyInterceptor>().InstancePerLifetimeScope();
        builder.RegisterType<DatabaseProxyInterceptor>().InstancePerLifetimeScope();
        builder.RegisterType<EmailService>().As<IEmailService>().InstancePerDependency();
        builder.RegisterType<DatabaseAsyncProxyInterceptor>().InstancePerLifetimeScope();
        builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>().SingleInstance();
        builder.RegisterType<NLogLogger>().As<ILoggerManager>().InstancePerLifetimeScope();
        builder.RegisterType<SecurityService>().As<ISecurity>().InstancePerLifetimeScope();
        builder.RegisterType<Cryptography>().As<ICryptography>().InstancePerLifetimeScope();
        builder.RegisterType<TokenGenerator>().As<ITokenGenerator>().InstancePerLifetimeScope();
        builder.RegisterType<InfrastructureService>().As<IInfrastructure>().InstancePerLifetimeScope();
        builder.RegisterType<TranslationService>().As<ITranslationService>().InstancePerLifetimeScope();

        // dynamically apply the proxy interceptor for the domain layer (it has no dependency on infrastructure)
        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            .Where(t => t.Namespace != null && 
                        t.Namespace.StartsWith("Lyrida.Domain.Core") &&
                        (t.Name.EndsWith("Service") ||
                        t.Name.EndsWith("Factory") ||
                        t.Name.EndsWith("Strategy") ||
                        t.Name.EndsWith("Manager")))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(ProxyInterceptor));
    }
    #endregion
}