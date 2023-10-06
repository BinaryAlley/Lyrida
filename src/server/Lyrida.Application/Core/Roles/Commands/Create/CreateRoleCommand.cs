#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Domain.Common.Entities.Authorization;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Create;

/// <summary>
/// Create role command
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public record CreateRoleCommand(string? RoleName, List<int> Permissions) : IRequest<ErrorOr<RoleEntity>>;