#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Lyrida.Api.Common.Entities.Authorization;

/// <summary>
/// Data transfer object for API creating roles requests
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public record AddRoleRequestEntity(string? RoleName, List<int> Permissions);