#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using MapsterMapper;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Lyrida.Infrastructure.Common.Enums;
using Lyrida.Infrastructure.Localization;
using Lyrida.Application.Common.DTO.Environments;
using Lyrida.Application.Core.Environments.Queries.Read;
using Lyrida.Application.Core.Environments.Commands.Create;
using Lyrida.Application.Core.Environments.Commands.Delete;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for managing environments actions
/// </summary>
/// <remarks>
/// Creation Date: 22nd of September, 2023
/// </remarks>
[Route("[controller]")]
public class EnvironmentsController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IMapper mapper;
    private readonly ISender mediator;
    private readonly ITranslationService translationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="mediator">Injected service for mediating commands and queries.</param>
    /// <param name="translationService">Injected service for localization.</param>
    /// <param name="mapper">Injected service for mapping objects.</param>
    public EnvironmentsController(ISender mediator, ITranslationService translationService, IMapper mapper) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.translationService = translationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates or updates a file system data source.
    /// </summary>
    /// <param name="data">The DTO containing the data needed to create or update the file system data source.</param>
    [HttpPost()]
    public async Task<IActionResult> AddEnvironment([FromBody] FileSystemDataSourceDto data)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<FileSystemDataSourceDto> result = await mediator.Send(new AddFileSystemDataSourceCommand(userId, data));
        return result.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Gets the collection of user file system data sources of the current user.
    /// </summary>
    [HttpGet()]
    public async Task<IActionResult> GetEnvironments()
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<IEnumerable<FileSystemDataSourceDto>> getResult = await mediator.Send(new GetFileSystemDataSourcesQuery(userId));
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Deletes a user's file system data source identified by <paramref name="guid"/>.
    /// </summary>
    [HttpDelete("{guid}")]
    public async Task<IActionResult> Delete(string guid)
    {
        if (!TryGetUserId(out int userId))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: translationService.Translate(Terms.InvalidUserId));
        ErrorOr<bool> result = await mediator.Send(new DeleteFileSystemDataSourceCommand(userId, guid));
        return result.Match(result => NoContent(), errors => Problem(errors));
    }

    /// <summary>
    /// Gets the id of the user currently making requests.
    /// </summary>
    /// <param name="userId">The id of the user currently making requests.</param>
    /// <returns>True if the id of the user currently making requests could be parsed, False otherwise.</returns>
    private bool TryGetUserId(out int userId)
    {
        var userIdClaim = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out userId);
    }
    #endregion
}