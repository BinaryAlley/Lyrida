#region ========================================================================= USING =====================================================================================
using System.Diagnostics;
#endregion

namespace Lyrida.UI.Common.Entities.Authentication;

/// <summary>
/// Account login data transfer object
/// </summary>
/// <remarks>
/// Creation Date: 13th of June, 2023
/// </remarks>
[DebuggerDisplay("{Email}")]
public record LoginEntity(string? Email, string? Password);