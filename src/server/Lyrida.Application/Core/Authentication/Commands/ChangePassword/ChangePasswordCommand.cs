#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.ChangePassword;

/// <summary>
/// Change password command
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record ChangePasswordCommand(string Username, string CurrentPassword, string NewPassword, string NewPasswordConfirm) : IRequest<ErrorOr<RegistrationResultDto>>;