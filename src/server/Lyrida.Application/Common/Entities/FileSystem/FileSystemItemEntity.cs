#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
#endregion

namespace Lyrida.Application.Common.Entities.FileSystem;

/// <summary>
/// Generic filesystem data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name}")]
public record FileSystemItemEntity(string Path, string Name, DateTime DateCreated, DateTime DateModified);