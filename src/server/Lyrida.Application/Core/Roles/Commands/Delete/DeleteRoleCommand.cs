#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Delete;

/// <summary>
/// Delete role command
/// </summary>
/// <remarks>
/// Creation Date: 17th of August, 2023
/// </remarks>
public record DeleteRoleCommand(int Id) : IRequest<ErrorOr<bool>>;