#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lyrida.Infrastructure.Localization;
using Lyrida.Domain.Common.Entities.Authorization;
using Lyrida.Application.Core.Users.Commands.Delete;
using Lyrida.Application.Core.Permissions.Queries.Read;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing permissions
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
[Route("[controller]")]
public class PermissionsController : ApiController
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
    public PermissionsController(ISender mediator, ITranslationService translationService) : base(translationService)
    {
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets all permissions
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        ErrorOr<IEnumerable<PermissionEntity>> getResult = await mediator.Send(new GetAllPermissionsQuery());
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Deletes an user identified by <paramref name="id"/>
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        ErrorOr<bool> deleteResult = await mediator.Send(new DeleteUserCommand(id));
        return deleteResult.Match(result => NoContent(), errors => Problem(errors));
    }
    #endregion
}