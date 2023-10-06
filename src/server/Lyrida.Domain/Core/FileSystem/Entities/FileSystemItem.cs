#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.Domain.Common.Enums;
using Lyrida.Domain.Common.Models;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Entities;

/// <summary>
/// Entity representing a generic file system element
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
public abstract class FileSystemItem : AggregateRoot<FileSystemPathId>
{
    #region ==================================================================== PROPERTIES =================================================================================
    public string Name { get; protected set; }
    public DateTime? DateCreated { get; protected set; }
    public DateTime? DateModified { get; protected set; }
    public FileSystemItemStatus Status { get; protected set; } = FileSystemItemStatus.Accessible;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="id">The unique identifier for the file system path.</param>
    /// <param name="name">The name of the file system item.</param>
    /// <param name="dateModified">The date and time the file system item was last modified. Can be <see langword="null"/> if unknown.</param>
    /// <param name="dateCreated">The date and time the file system item was created. Can be <see langword="null"/> if unknown.</param>
    protected FileSystemItem(FileSystemPathId id, string name, DateTime? dateModified, DateTime? dateCreated) : base(id)
    {
        Name = name;
        DateCreated = dateCreated;
        DateModified = dateModified;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Renames the file system item to the specified new name.
    /// </summary>
    /// <param name="newName">The new name for the file system item.</param>
    /// <exception cref="ArgumentException">Thrown if the new name is null or whitespace.</exception>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New name cannot be empty", nameof(newName));
        Name = newName;
    }

    /// <summary>
    /// Updates the last modified date of the file system item.
    /// </summary>
    /// <param name="date">The new date and time of last modification.</param>
    public void UpdateLastModified(DateTime date)
    {
        DateModified = date;
    }

    /// <summary>
    /// Sets the status of the filesystem item
    /// </summary>
    /// <param name="status">The status to set</param>
    public void SetStatus(FileSystemItemStatus status)
    {
        Status = status;
    }
    #endregion
}
