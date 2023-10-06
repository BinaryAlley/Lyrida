#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Lyrida.Api.Common.Entities.Authorization;

/// <summary>
/// Data transfer object for API updating roles requests
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public record UpdateRoleRequestEntity(int RoleId, string? RoleName, List<int> Permissions);