#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Delete;

/// <summary>
/// Validates the needed validation rules for the delete file command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.Path).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}