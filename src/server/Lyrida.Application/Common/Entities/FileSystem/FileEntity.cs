#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
#endregion

namespace Lyrida.Application.Common.Entities.FileSystem;

/// <summary>
/// File data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name} (File, Size: {Size} bytes)")]
public record FileEntity(string Path, string Name, DateTime DateCreated, DateTime DateModified, long Size) : FileSystemItemEntity(Path, Name, DateCreated, DateModified);