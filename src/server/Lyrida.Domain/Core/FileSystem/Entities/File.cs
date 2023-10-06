#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Entities;

/// <summary>
/// Entity representing a file
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
public class File : FileSystemItem
{
    #region ==================================================================== PROPERTIES =================================================================================
    public long Size { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="id">The unique identifier of the file in the file system path.</param>
    /// <param name="name">The name of the file.</param>
    /// <param name="dateCreated">The date and time the file was created. Can be <see langword="null"/> if unknown.</param>
    /// <param name="dateModified">The date and time the file was last modified. Can be <see langword="null"/> if unknown.</param>
    /// <param name="size">The size of the file in bytes.</param>
    public File(FileSystemPathId id, string name, DateTime? dateCreated, DateTime? dateModified, long size) : base(id, name, dateCreated, dateModified)
    {
        Size = size;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates the size of the file.
    /// </summary>
    /// <param name="newSize">The new size in bytes.</param>
    public void UpdateSize(long newSize)
    {
        Size = newSize;
    }
    #endregion
}