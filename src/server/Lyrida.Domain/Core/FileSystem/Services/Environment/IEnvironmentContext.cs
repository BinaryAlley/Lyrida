#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Core.FileSystem.Services.Paths.PathStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileTypeStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;
using Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Interface for defining the contract of an environment-specific context, encapsulating all platform-specific services and strategies.
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public interface IEnvironmentContext
{
    #region ==================================================================== PROPERTIES =================================================================================
    //IPathStrategy PathStrategy { get; }
    IFileTypeStrategy FileTypeStrategy { get; }
    IFileProviderStrategy FileProviderStrategy { get; }
    IDirectoryProviderStrategy DirectoryProviderStrategy { get; }
    #endregion
}