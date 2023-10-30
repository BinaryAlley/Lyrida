#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
#endregion

namespace Lyrida.Application.Common.DTO.FileSystem;

/// <summary>
/// File data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name} (File, Size: {Size} bytes)")]
public record FileDto(string Path, string Name, DateTime DateCreated, DateTime DateModified, long Size) : FileSystemItemDto(Path, Name, DateCreated, DateModified);