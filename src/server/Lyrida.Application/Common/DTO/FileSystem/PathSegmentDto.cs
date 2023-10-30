#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.Application.Common.DTO.FileSystem;

/// <summary>
/// Path segment data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 04th of October, 2023
/// </remarks>
[DebuggerDisplay("{Name} (IsDirectory: {IsDirectory}, IsDrive: {IsDrive})")]
public record PathSegmentDto(string Name, bool IsDirectory, bool IsDrive);