#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Update;

/// <summary>
/// Set user permissions command
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public record SetUserPermissionsCommand(int UserId, List<int> Permissions) : IRequest<ErrorOr<bool>>;