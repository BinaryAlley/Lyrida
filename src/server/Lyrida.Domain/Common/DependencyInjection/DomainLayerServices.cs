#region ========================================================================= USING =====================================================================================
using Autofac;
using System.IO.Abstractions;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
using Lyrida.Domain.Core.FileSystem.Services.Files;
using Lyrida.Domain.Core.FileSystem.Services.Platform;
using Lyrida.Domain.Core.FileSystem.Services.Thumbnails;
using Lyrida.Domain.Core.FileSystem.Services.Directories;
using Lyrida.Domain.Core.FileSystem.Services.Environment;
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;
#endregion

namespace Lyrida.Domain.Common.DependencyInjection;

/// <summary>
/// Contains all services of the Domain Layer
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public class DomainLayerServices : Module
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers the services of the Application Layer into the Dependency Injection container
    /// </summary>
    /// <param name="builder">The Dependency Injection container where the services are registered</param>
    protected override void Load(ContainerBuilder builder)
    {
        // the proxy interceptors for these services are added in the infrastructure layer services registration module!
        builder.RegisterType<LocalSystemDirectoryProviderStrategy>()
               .As<IDirectoryProviderStrategy>()
               .InstancePerLifetimeScope();

        builder.RegisterType<DirectoryService>()
               .As<IDirectoryService>()
               .InstancePerLifetimeScope();

        builder.RegisterType<LocalSystemFileProviderStrategy>()
                .As<IFileProviderStrategy>()
                .InstancePerLifetimeScope();

        builder.RegisterType<FileService>()
               .As<IFileService>()
               .InstancePerLifetimeScope();

        builder.RegisterType<ThumbnailService>()
             .As<IThumbnailService>()
             .InstancePerLifetimeScope();

        builder.RegisterType<PathService>()
             .As<IPathService>()
             .InstancePerLifetimeScope();

        builder.RegisterType<LocalSystemFileTypeStrategy>()
             .As<ILocalSystemFileTypeStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterType<FtpFileTypeStrategy>()
             .As<IFtpFileTypeStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterType<LocalSystemFileProviderStrategy>()
             .As<ILocalSystemFileProviderStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterType<FtpFileProviderStrategy>()
             .As<IFtpFileProviderStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterType<FileProviderStrategyFactory>()
             .As<IFileProviderStrategyFactory>()
             .InstancePerLifetimeScope();


        builder.RegisterType<LocalSystemDirectoryProviderStrategy>()
             .As<ILocalSystemDirectoryProviderStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterType<FtpDirectoryProviderStrategy>()
             .As<IFtpDirectoryProviderStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterType<DirectoryProviderStrategyFactory>()
             .As<IDirectoryProviderStrategyFactory>()
             .InstancePerLifetimeScope();

        builder.RegisterType<LocalSystemEnvironmentContext>()
             .As<ILocalSystemEnvironmentContext>()
             .InstancePerLifetimeScope();

        builder.RegisterType<FtpEnvironmentContext>()
             .As<IFtpEnvironmentContext>()
             .InstancePerLifetimeScope();

        builder.RegisterType<EnvironmentContextFactory>()
             .As<IEnvironmentContextFactory>()
             .InstancePerLifetimeScope();

        builder.RegisterType<EnvironmentContextManager>()
               .As<IEnvironmentContextManager>()
               .InstancePerLifetimeScope();


        builder.RegisterType<PlatformContextFactory>()
             .As<IPlatformContextFactory>()
             .InstancePerLifetimeScope();

        builder.RegisterType<UnixPlatformContext>()
             .As<IUnixPlatformContext>()
             .InstancePerLifetimeScope();

        builder.RegisterType<WindowsPlatformContext>()
             .As<IWindowsPlatformContext>()
             .InstancePerLifetimeScope();

        builder.RegisterType<PlatformContextManager>()
               .As<IPlatformContextManager>()
               .InstancePerLifetimeScope();

        builder.RegisterType<UnixPathStrategy>()
            .As<IUnixPathStrategy>()
            .InstancePerLifetimeScope();

        builder.RegisterType<WindowsPathStrategy>()
             .As<IWindowsPathStrategy>()
             .InstancePerLifetimeScope();

        builder.RegisterInstance(new FileSystem())
               .As<IFileSystem>()
               .SingleInstance();
    }
    #endregion
}