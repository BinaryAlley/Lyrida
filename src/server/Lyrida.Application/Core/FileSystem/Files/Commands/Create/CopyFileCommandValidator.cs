#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Create;

/// Validates the needed validation rules for the copy file command
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public class CopyFileCommandValidator : AbstractValidator<CopyFileCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public CopyFileCommandValidator()
    {
        RuleFor(x => x.SourcePath).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
        RuleFor(x => x.DestinationPath).NotEmpty().WithMessage(Terms.PathCannotBeEmpty.ToString());
    }
    #endregion
}