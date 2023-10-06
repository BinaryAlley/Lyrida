#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.Entities.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.ResetPassword;

/// <summary>
/// Authentication password reset command
/// </summary>
/// <remarks>
/// Creation Date: 02nd of August, 2023
/// </remarks>
public record ResetPasswordCommand(string Email, string Password, string PasswordConfirm) : IRequest<ErrorOr<RegistrationResultEntity>>;