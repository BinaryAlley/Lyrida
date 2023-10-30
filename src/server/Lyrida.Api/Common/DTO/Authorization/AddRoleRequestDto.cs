#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Lyrida.Api.Common.DTO.Authorization;

/// <summary>
/// Data transfer object for API creating roles requests
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public record AddRoleRequestDto(string? RoleName, List<int> Permissions);