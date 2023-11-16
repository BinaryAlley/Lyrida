#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Update;

/// <summary>
/// Validates the needed validation rules for the rename file command
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
public class RenameFileCommandValidator : AbstractValidator<RenameFileCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public RenameFileCommandValidator()
    {
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
        RuleFor(x => x.Name).NotEmpty().WithMessage(Terms.NameCannotBeEmptyError.ToString());
    }
    #endregion
}