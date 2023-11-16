#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.Application.Common.DTO.FileSystem;

/// <summary>
/// File system item paste data transfer object.
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
[DebuggerDisplay("{SourcePath} {DestinationPath}")]
public record PasteFileSystemItemDto(string SourcePath, string DestinationPath, bool IsFile, bool? OverrideExisting);