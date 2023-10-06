/// Written by: Yulia Danilova
/// Creation Date: 04th of November, 2021
/// Purpose: Facade for the services dealing with file systems

namespace Lyrida.Application.Core.FileSystem;

public class FileSystemService : IFileSystemService
{
    #region ==================================================================== PROPERTIES =================================================================================
    public IFileService FileService { get; private set; }
    public IDirectoryService DirectoryService { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="directoryService">Injected file system directory service</param>
    /// <param name="fileService">Injected file system file service</param>
    public FileSystemService(IDirectoryService directoryService, IFileService fileService)
    {
        DirectoryService = directoryService;
        FileService = fileService;
    }
    #endregion
}