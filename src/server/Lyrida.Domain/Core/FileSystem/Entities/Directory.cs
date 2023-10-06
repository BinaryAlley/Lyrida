#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Entities;

/// <summary>
/// Entity representing a directory
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
[DebuggerDisplay("{Id.Path}")]
public class Directory : FileSystemItem
{
    #region ==================================================================== PROPERTIES =================================================================================
    public List<FileSystemItem> Items { get; private set; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="id">The unique identifier of the directory in the file system path.</param>
    /// <param name="name">The name of the directory.</param>
    /// <param name="dateCreated">The date and time the directory was created. Can be <see langword="null"/> if unknown.</param>
    /// <param name="dateModified">The date and time the directory was last modified. Can be <see langword="null"/> if unknown.</param>
    public Directory(FileSystemPathId id, string name, DateTime? dateCreated, DateTime? dateModified) : base(id, name, dateCreated, dateModified)
    {
        Items = new List<FileSystemItem>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Adds a file system item to the directory.
    /// </summary>
    /// <param name="item">The file system item to add.</param>
    public void AddItem(FileSystemItem item)
    {
        Items.Add(item);
    }

    /// <summary>
    /// Removes a file system item from the directory.
    /// </summary>
    /// <param name="item">The file system item to remove.</param>
    public void RemoveItem(FileSystemItem item)
    {
        Items.Remove(item);
    }
    #endregion
}