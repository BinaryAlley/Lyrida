#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Users.Queries.Read;

/// <summary>
/// Query for retrieving the permissions of a user
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public record GetUserPermissionsQuery(int UserId, int CurrentUserId) : IRequest<ErrorOr<IEnumerable<UserPermissionEntity>>>;