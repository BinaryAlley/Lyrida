#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
using System.Collections.Generic;
#endregion

namespace Lyrida.Application.Common.DTO.FileSystem;

/// <summary>
/// Directory data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name} (Directory)")]
public record DirectoryDto(string Path, string Name, DateTime DateCreated, DateTime DateModified, List<FileSystemItemDto> Items) : FileSystemItemDto(Path, Name, DateCreated, DateModified);