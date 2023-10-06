#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.Infrastructure.Localization;
using Lyrida.Api.Common.Entities.Authorization;
using Lyrida.Application.Core.Roles.Queries.Read;
using Lyrida.Domain.Common.Entities.Authorization;
using Lyrida.Application.Core.Roles.Commands.Create;
using Lyrida.Application.Core.Roles.Commands.Delete;
using Lyrida.Application.Core.Roles.Commands.Update;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing roles
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
[Route("[controller]")]
public class RolesController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly ISender mediator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating command and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    public RolesController(ISender mediator, ITranslationService translationService) : base(translationService)
    {
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets all roles
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        ErrorOr<IEnumerable<RoleEntity>> result = await mediator.Send(new GetAllRolesQuery());
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Gets all permissions of a role identified by <paramref name="id"/>
    /// </summary>
    /// <param name="id">The id of the role for which to get the list of permissions</param>
    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetPermissionsByRoleId(int id)
    {
        ErrorOr<IEnumerable<PermissionEntity>> result = await mediator.Send(new GetAllRolePermissionsQuery(id));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Deletes a role identified by <paramref name="id"/>
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ErrorOr<bool> result = await mediator.Send(new DeleteRoleCommand(id));
        return result.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Creates a new role with permissions
    /// </summary>
    /// <param name="entity">Entity containing the name of the role to create, and its permissions</param>
    [HttpPost()]
    public async Task<IActionResult> Add([FromBody] AddRoleRequestEntity entity)
    {
        ErrorOr<RoleEntity> result = await mediator.Send(new CreateRoleCommand(entity.RoleName, entity.Permissions));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Updates an existing role and its permissions
    /// </summary>
    /// <param name="id">The id of the role to update</param>
    /// <param name="entity">Entity containing the updated role, and its permissions</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleRequestEntity entity)
    {
        ErrorOr<bool> result = await mediator.Send(new UpdateRoleCommand(id, entity.RoleName, entity.Permissions));
        return result.Match(result => NoContent(), errors => Problem(errors));
    }
    #endregion
}