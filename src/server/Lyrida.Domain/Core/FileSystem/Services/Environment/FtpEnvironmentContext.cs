#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Environment context for FTP services
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public class FtpEnvironmentContext : IFtpEnvironmentContext
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
    /// <param name="ftpDirectoryProviderStrategy">Injected service for creating directory provider strategies for FTP services</param>
    /// <param name="ftpFileProviderStrategy">Injected service for creating file provider strategies for FTP services</param>
    /// <param name="ftpFileTypeStrategy">Injected service for creating file type strategies for FTP services</param>
    /// <param name="pathStrategy">Injected service for creating file type strategies for path services</param>
    public FtpEnvironmentContext(IFtpDirectoryProviderStrategy ftpDirectoryProviderStrategy, IFtpFileProviderStrategy ftpFileProviderStrategy, IFtpFileTypeStrategy ftpFileTypeStrategy, 
        IPathStrategy pathStrategy)
    {
        PathStrategy = pathStrategy;
        FileTypeStrategy = ftpFileTypeStrategy;
        FileProviderStrategy = ftpFileProviderStrategy;
        DirectoryProviderStrategy = ftpDirectoryProviderStrategy;
    }
    #endregion
}