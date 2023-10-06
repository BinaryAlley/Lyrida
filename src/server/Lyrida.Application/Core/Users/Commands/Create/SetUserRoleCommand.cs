#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Create;

/// <summary>
/// Set a user role command
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public record SetUserRoleCommand(int UserId, int? RoleId) : IRequest<ErrorOr<bool>>;