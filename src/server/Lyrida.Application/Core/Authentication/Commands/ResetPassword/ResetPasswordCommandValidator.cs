#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.ResetPassword;

/// <summary>
/// Validates the needed validation rules for the password reset command
/// </summary>
/// <remarks>
/// Creation Date: 02nd of August, 2023
/// </remarks>
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage(Terms.EmailCannotBeEmpty.ToString())
            .EmailAddress().WithMessage(Terms.InvalidEmailAddress.ToString());
        RuleFor(x => x.Password).NotEmpty().WithMessage(Terms.PasswordCannotBeEmpty.ToString())
            .Matches("^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$").WithMessage(Terms.InvalidPassword.ToString());
        RuleFor(x => x.PasswordConfirm).NotEmpty().WithMessage(Terms.EmptyPasswordConfirm.ToString());
        RuleFor(x => x.Password).Equal(x => x.PasswordConfirm).WithMessage(Terms.PasswordsNotMatch.ToString());
    }
    #endregion
}