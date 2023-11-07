#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Configuration.Commands.Update;

/// <summary>
/// Validates the needed validation rules for the update profile preference command
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public class UpdateProfilePreferenceCommandValidator : AbstractValidator<UpdateProfilePreferenceCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public UpdateProfilePreferenceCommandValidator()
    {
        RuleFor(x => x.ProfilePreferences).NotNull().WithMessage(Terms.ProfilePreferencesCannotBeEmpty.ToString());
        RuleFor(x => x.ProfilePreferences.ImagePreviewsQuality).InclusiveBetween(1, 100).When(x => x.ProfilePreferences.ShowImagePreviews)
            .WithMessage(Terms.ValueBetweenZeroAndOneHundred.ToString()); // only apply the rule when ShowImagePreviews is true
        RuleFor(x => x.ProfilePreferences.FullImageQuality).InclusiveBetween(1, 100).WithMessage(Terms.ValueBetweenZeroAndOneHundred.ToString());
        RuleFor(x => x.ProfilePreferences.ThumbnailsRetrievalBatchSize).GreaterThan(0).WithMessage(Terms.ValueGreaterThanZero.ToString());
        RuleFor(x => x.ProfilePreferences.ScrollThumbnailRetrievalTimeout).GreaterThan(0).WithMessage(Terms.ValueGreaterThanZero.ToString());
    }
    #endregion
}