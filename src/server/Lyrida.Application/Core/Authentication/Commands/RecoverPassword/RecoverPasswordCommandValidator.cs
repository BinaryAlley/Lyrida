#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.RecoverPassword;

/// <summary>
/// Validates the needed validation rules for the password recovery command
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class RecoverPasswordCommandValidator : AbstractValidator<RecoverPasswordCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public RecoverPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage(Terms.EmailCannotBeEmpty.ToString())
            .EmailAddress().WithMessage(Terms.InvalidEmailAddress.ToString());
        RuleFor(x => x.TotpCode).NotEmpty().WithMessage(Terms.TotpCodeCannotBeEmpty.ToString());
    }
    #endregion
}