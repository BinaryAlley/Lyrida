#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
using System.Collections.Generic;
#endregion

namespace Lyrida.UI.Common.Entities.FileSystem;

/// <summary>
/// Directory data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name} (Directory)")]
public record DirectoryEntity(string Path, string Name, string? Type, DateTime DateCreated, DateTime DateModified, List<FileSystemItemEntity> Items) : FileSystemItemEntity(Path, Name, Type, DateCreated, DateModified);