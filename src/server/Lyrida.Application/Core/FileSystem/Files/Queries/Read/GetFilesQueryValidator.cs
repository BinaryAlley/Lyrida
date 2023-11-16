#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Queries.Read;

/// <summary>
/// Validates the needed validation rules for the get files query
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class GetFilesQueryValidator : AbstractValidator<GetFilesQuery>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public GetFilesQueryValidator()
    {
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}