#region ========================================================================= USING =====================================================================================
using FluentValidation;
using Lyrida.Infrastructure.Common.Enums;
#endregion

namespace Lyrida.Application.Core.Roles.Commands.Update;

/// <summary>
/// Validates the needed validation rules for the update role command
/// </summary>
/// <remarks>
/// Creation Date: 18th of August, 2023
/// </remarks>
public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.RoleName).NotEmpty().WithMessage(Terms.RoleNameCannotBeEmpty.ToString());
        RuleFor(x => x.Permissions).NotEmpty().WithMessage(Terms.RoleMustHavePermissions.ToString());
    }
    #endregion
}