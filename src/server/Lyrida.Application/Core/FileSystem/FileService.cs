/// Written by: Yulia Danilova
/// Creation Date: 04th of November, 2021
/// Purpose: Service for file system files
#region ========================================================================= USING =====================================================================================
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
#endregion

namespace Lyrida.Application.Core.FileSystem;

public class FileService : IFileService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileSystemPermissionsService permissionsService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="permissionsService">Injected file system permission service</param>
    public FileService(IFileSystemPermissionsService permissionsService)
    {
        this.permissionsService = permissionsService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
    /// <param name="path">A string specifying the path on for which to get the list of directories</param>
    /// <param name="searchPattern">The search string to match against the names of directories.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref name="searchOption" />.</returns>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.Security.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    public async Task<IEnumerable<FileInfo>> GetFilesAsync(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return await Task.Run(() =>
        {
            if (permissionsService.CanAccessPath(path))
                return new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption);
            else
                throw new UnauthorizedAccessException("Access to path is denied!");
        });
    }
    #endregion
}