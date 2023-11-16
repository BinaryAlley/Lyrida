#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Update;

/// Validates the needed validation rules for the move file command
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public class MoveFileCommandValidator : AbstractValidator<MoveFileCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public MoveFileCommandValidator()
    {
        RuleFor(x => x.SourcePath).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
        RuleFor(x => x.DestinationPath).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}