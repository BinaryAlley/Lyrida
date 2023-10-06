#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Roles.Queries.Read;

/// <summary>
/// Query for retrieving the list of roles
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public record GetAllRolesQuery() : IRequest<ErrorOr<IEnumerable<RoleEntity>>>;