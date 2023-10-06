#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Delete;

/// <summary>
/// Delete user command
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public record DeleteUserCommand(int Id) : IRequest<ErrorOr<bool>>;