#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.Login;

/// <summary>
/// Validates the needed validation rules for the login query
/// </summary>
/// <remarks>
/// Creation Date: 19th of July, 2023
/// </remarks>
public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage(Terms.EmailCannotBeEmpty.ToString());
        RuleFor(x => x.Password).NotEmpty().WithMessage(Terms.PasswordCannotBeEmpty.ToString());
    }
    #endregion
}