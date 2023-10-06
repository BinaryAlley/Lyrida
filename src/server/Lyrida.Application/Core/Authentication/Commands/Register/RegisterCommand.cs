#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.Register;

/// <summary>
/// Authentication register command
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public record RegisterCommand(string FirstName, string LastName, string Email, string Password, string PasswordConfirm) : IRequest<ErrorOr<RegistrationResultEntity>>;