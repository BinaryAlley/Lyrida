#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Setup.Commands.Create;

/// <summary>
/// Application setup command
/// </summary>
/// <remarks>
/// Creation Date: 15th of August, 2023
/// </remarks>
public record SetupCommand(string FirstName, string LastName, string Email, string Password, string PasswordConfirm, bool Use2fa) : IRequest<ErrorOr<RegistrationResultDto>>;