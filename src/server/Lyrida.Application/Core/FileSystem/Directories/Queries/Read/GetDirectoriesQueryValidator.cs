#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Queries.Read;

/// <summary>
/// Validates the needed validation rules for the get directories command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class GetDirectoriesQueryValidator : AbstractValidator<GetDirectoriesQuery>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public GetDirectoriesQueryValidator()
    {
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}