#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Delete;

/// <summary>
/// Validates the needed validation rules for the delete directory command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class DeleteDirectoryCommandValidator : AbstractValidator<DeleteDirectoryCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public DeleteDirectoryCommandValidator()
    {
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}