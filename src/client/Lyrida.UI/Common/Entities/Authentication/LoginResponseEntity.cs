#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.Authentication;

/// <summary>
/// Account login responses data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public record LoginResponseEntity(int Id, string? FirstName, string? LastName, string? Email, string? Token);