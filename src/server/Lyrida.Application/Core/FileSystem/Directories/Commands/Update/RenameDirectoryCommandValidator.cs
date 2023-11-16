#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Update;

/// <summary>
/// Validates the needed validation rules for the rename directory command
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
public class RenameDirectoryCommandValidator : AbstractValidator<RenameDirectoryCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public RenameDirectoryCommandValidator()
    {
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
        RuleFor(x => x.Name).NotEmpty().WithMessage(Terms.NameCannotBeEmptyError.ToString());
    }
    #endregion
}