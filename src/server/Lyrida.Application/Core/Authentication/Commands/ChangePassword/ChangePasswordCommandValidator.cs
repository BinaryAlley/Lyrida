#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.ChangePassword;

/// <summary>
/// Validates the needed validation rules for the change password command
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage(Terms.UsernameCannotBeEmpty.ToString());
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(Terms.EmptyCurrentPassword.ToString())
            .Matches("^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$").WithMessage(Terms.InvalidPassword.ToString());
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage(Terms.EmptyNewPassword.ToString())
            .Matches("^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$").WithMessage(Terms.InvalidPassword.ToString());
        RuleFor(x => x.NewPasswordConfirm).NotEmpty().WithMessage(Terms.EmptyNewPasswordConfirm.ToString());
        RuleFor(x => x.NewPassword).Equal(x => x.NewPasswordConfirm).WithMessage(Terms.PasswordsNotMatch.ToString());
    }
    #endregion
}