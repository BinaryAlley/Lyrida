#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.VerifyRegistration;

/// <summary>
/// Validates the needed validation rules for the registration token verification query
/// </summary>
/// <remarks>
/// Creation Date: 27th of July, 2023
/// </remarks>
public class VerifyRegistrationTokenQueryValidator : AbstractValidator<VerifyRegistrationTokenQuery>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public VerifyRegistrationTokenQueryValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage(Terms.TokenCannotBeEmpty.ToString());
    }
    #endregion
}