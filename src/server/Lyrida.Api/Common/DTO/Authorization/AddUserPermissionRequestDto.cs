#region ========================================================================= USING =====================================================================================
using System.Collections.Generic;
#endregion

namespace Lyrida.Api.Common.DTO.Authorization;

/// <summary>
/// Data transfer object for API setting user permissions requests
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public record AddUserPermissionRequestDto(int UserId, List<int> Permissions);