#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Environment context for local file systems
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
internal class LocalSystemEnvironmentContext : ILocalSystemEnvironmentContext
{
    #region ==================================================================== PROPERTIES =================================================================================
    public IPathStrategy PathStrategy { get; private set; }
    public IFileTypeStrategy FileTypeStrategy { get; private set; }
    public IFileProviderStrategy FileProviderStrategy { get; private set; }
    public IDirectoryProviderStrategy DirectoryProviderStrategy { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="localSystemDirectoryProviderStrategy">Injected service for creating directory provider strategies for local file systems</param>
    /// <param name="localSystemFileProviderStrategy">Injected service for creating file provider strategies for local file systems</param>
    /// <param name="localSystemFileTypeStrategy">Injected service for creating file type strategies for local file systems</param>
    /// <param name="pathStrategy">Injected service for creating file type strategies for path services</param>
    public LocalSystemEnvironmentContext(ILocalSystemDirectoryProviderStrategy localSystemDirectoryProviderStrategy, ILocalSystemFileProviderStrategy localSystemFileProviderStrategy, 
        ILocalSystemFileTypeStrategy localSystemFileTypeStrategy, IPathStrategy pathStrategy)
    {
        PathStrategy = pathStrategy;
        FileTypeStrategy = localSystemFileTypeStrategy;
        FileProviderStrategy = localSystemFileProviderStrategy;
        DirectoryProviderStrategy = localSystemDirectoryProviderStrategy;
    }
    #endregion
}