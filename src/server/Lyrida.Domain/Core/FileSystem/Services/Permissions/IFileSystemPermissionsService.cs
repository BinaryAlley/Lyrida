#region ========================================================================= USING =====================================================================================
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Permissions;

/// <summary>
/// Interface for file system permission service
/// </summary>
/// <remarks>
/// Creation Date: 10th of October, 2023
/// </remarks>
internal interface IFileSystemPermissionsService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> can be accessed.
    /// </summary>
    /// <param name="path">The path to be accessed.</param>
    /// <param name="accessMode">The mode in which to access the path.</param>
    /// <param name="isFile">Indicates whether the path represents a file or directory.</param>
    /// <returns><see langword="true"/>, if <paramref name="path"/> can be accessed, <see langword="false"/> otherwise.</returns>
    bool CanAccessPath(FileSystemPathId path, FileAccessMode accessMode, bool isFile = true);
    #endregion
}