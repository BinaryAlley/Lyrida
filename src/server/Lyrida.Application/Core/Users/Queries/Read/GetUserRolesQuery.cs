#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.Authorization;
#endregion

namespace Lyrida.Application.Core.Users.Queries.Read;

/// <summary>
/// Query for retrieving the roles of a user
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public record GetUserRolesQuery(int UserId, int CurrentUserId) : IRequest<ErrorOr<IEnumerable<UserRoleDto>>>;