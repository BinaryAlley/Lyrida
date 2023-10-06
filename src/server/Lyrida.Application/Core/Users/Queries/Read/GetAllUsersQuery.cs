#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Users.Queries.Read;

/// <summary>
/// Query for retrieving the list of users
/// </summary>
/// <remarks>
/// Creation Date: 04th of August, 2023
/// </remarks>
public record GetAllUsersQuery() : IRequest<ErrorOr<IEnumerable<UserEntity>>>;