#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Update;

/// <summary>
/// Update role command
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public record UpdateRoleCommand(int RoleId, string? RoleName, List<int> Permissions) : IRequest<ErrorOr<bool>>;