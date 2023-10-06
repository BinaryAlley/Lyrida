/// Written by: Yulia Danilova
/// Creation Date: 03rd of November, 2021
/// Purpose: Interface of the service for file system directories
#region ========================================================================= USING =====================================================================================
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
#endregion

namespace Lyrida.Application.Core.FileSystem;

public interface IDirectoryService
{
    #region ==================================================================== PROPERTIES =================================================================================
    string? CurrentLocation { get; set; }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
    /// <param name="path">A string specifying the path on for which to get the list of directories</param>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    IEnumerable<DirectoryInfo> GetDirectories(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly);

    /// <summary>Gets the parent directory of a specified subdirectory.</summary>
    /// <returns>The parent directory, or <see langword="null" /> if the path is null or if the file path denotes a root (such as /, \, C:\, or \\server\share).</returns>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    DirectoryInfo? GetParentDirectory(string path);
    #endregion
}