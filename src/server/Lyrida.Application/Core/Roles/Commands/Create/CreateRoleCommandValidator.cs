#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Create;

/// <summary>
/// Validates the needed validation rules for the create role command
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.RoleName).NotEmpty().WithMessage(Terms.RoleNameCannotBeEmpty.ToString());
        RuleFor(x => x.Permissions).NotEmpty().WithMessage(Terms.RoleMustHavePermissions.ToString());
    }
    #endregion
}