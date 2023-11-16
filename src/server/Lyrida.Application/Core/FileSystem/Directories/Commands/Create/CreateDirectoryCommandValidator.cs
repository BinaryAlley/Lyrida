#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Create;

/// <summary>
/// Validates the needed validation rules for the create directory command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class CreateDirectoryCommandValidator : AbstractValidator<CreateDirectoryCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public CreateDirectoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(Terms.NameCannotBeEmptyError.ToString());
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}