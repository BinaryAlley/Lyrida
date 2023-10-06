﻿/// Written by: Yulia Danilova
/// Creation Date: 04th of November, 2021
/// Purpose: Interface of the service for file system files
#region ========================================================================= USING =====================================================================================
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
#endregion

namespace Lyrida.Application.Core.FileSystem;

public interface IFileService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
    /// <param name="path">A string specifying the path on for which to get the list of files</param>
    /// <param name="searchPattern">The search string to match against the names of files.
    /// This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories.
    /// The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.FileInfo" /> object is invalid.</exception>
    /// <exception cref="T:System.Security.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    Task<IEnumerable<FileInfo>> GetFilesAsync(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly);
    #endregion
}