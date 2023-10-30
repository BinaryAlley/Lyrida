#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.Authorization;
#endregion

namespace Lyrida.Application.Core.Permissions.Queries.Read;

/// <summary>
/// Query for retrieving the list of permissions
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public record GetAllPermissionsQuery() : IRequest<ErrorOr<IEnumerable<PermissionDto>>>;