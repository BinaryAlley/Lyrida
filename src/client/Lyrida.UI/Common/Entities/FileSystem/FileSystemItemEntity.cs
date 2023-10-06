#region ========================================================================= USING =====================================================================================
using System;
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.FileSystem;

/// <summary>
/// Generic filesystem data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
[DebuggerDisplay("{Name}")]
public record FileSystemItemEntity(string Path, string Name, string? Type, DateTime DateModified, DateTime DateCreated);