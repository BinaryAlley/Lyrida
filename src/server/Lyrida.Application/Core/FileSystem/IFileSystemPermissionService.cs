/// Written by: Yulia Danilova, Sameh Salem
/// Creation Date: 03rd of November, 2021
/// Purpose: Interface for the service for file system permissions

namespace Lyrida.Application.Core.FileSystem;

public interface IFileSystemPermissionsService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> can be accessed
    /// </summary>
    /// <param name="path">The path to be accessed</param>
    /// <returns><see langword="true"/>, if <paramref name="path"/> can be accessed, <see langword="false"/> otherwise</returns>
    bool CanAccessPath(string path);
    #endregion
}
