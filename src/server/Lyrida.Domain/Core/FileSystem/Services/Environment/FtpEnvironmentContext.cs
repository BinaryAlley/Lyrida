#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;
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
    public IFileTypeStrategy FileTypeService { get; private set; }
    public IFileProviderStrategy FileProvider { get; private set; }
    public IDirectoryProviderStrategy DirectoryProvider { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="ftpDirectoryProviderStrategy">Injected service for creating directory provider strategies for FTP services</param>
    /// <param name="ftpFileProviderStrategy">Injected service for creating file provider strategies for FTP services</param>
    /// <param name="ftpFileTypeStrategy">Injected service for creating file type strategies for FTP services</param>
    public FtpEnvironmentContext(IFtpDirectoryProviderStrategy ftpDirectoryProviderStrategy, IFtpFileProviderStrategy ftpFileProviderStrategy, IFtpFileTypeStrategy ftpFileTypeStrategy)
    {
        FileTypeService = ftpFileTypeStrategy;
        FileProvider = ftpFileProviderStrategy;
        DirectoryProvider = ftpDirectoryProviderStrategy;
    }
    #endregion
}