/// Written by: Yulia Danilova
/// Creation Date: 04th of November, 2021
/// Purpose: Facade interface for the services dealing with file systems

namespace Lyrida.Application.Core.FileSystem;

public interface IFileSystemService
{
    #region ==================================================================== PROPERTIES =================================================================================
    IDirectoryService DirectoryService { get; }
    IFileService FileService { get; }
    #endregion
}