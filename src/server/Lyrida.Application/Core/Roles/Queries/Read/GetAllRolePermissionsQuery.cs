#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Roles.Queries.Read;

/// <summary>
/// Query for retrieving the list of permissions of a role
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public record GetAllRolePermissionsQuery(int RoleId) : IRequest<ErrorOr<IEnumerable<PermissionEntity>>>;