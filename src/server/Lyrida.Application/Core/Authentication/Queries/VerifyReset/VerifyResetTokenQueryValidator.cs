#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Authentication.Queries.VerifyReset;

/// <summary>
/// Validates the needed validation rules for the password reset token verification query
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public class VerifyResetTokenQueryValidator : AbstractValidator<VerifyResetTokenQuery>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public VerifyResetTokenQueryValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage(Terms.TokenCannotBeEmpty.ToString());
    }
    #endregion
}