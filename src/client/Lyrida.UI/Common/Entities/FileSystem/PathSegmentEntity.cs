#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.FileSystem;

/// <summary>
/// Path segment data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 04th of October, 2023
/// </remarks>
[DebuggerDisplay("{Name} (IsDirectory: {IsDirectory}, IsDrive: {IsDrive})")]
public record PathSegmentEntity(string Name, bool IsDirectory, bool IsDrive);