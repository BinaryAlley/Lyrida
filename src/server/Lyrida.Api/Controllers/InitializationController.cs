#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using MapsterMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lyrida.Infrastructure.Localization;
using Microsoft.AspNetCore.Authorization;
using Lyrida.Application.Core.Setup.Queries.Read;
using Lyrida.Application.Core.Setup.Commands.Create;
using Lyrida.Api.Common.DTO.Authentication;
using Lyrida.Application.Common.DTO.Authentication;
#endregion

namespace Lyrida.Api.Controllers;

/// <summary>
/// Controller for checking and performing the initial setup of the application
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
[Route("[controller]")]
public class InitializationController : ApiController
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IMapper mapper;
    private readonly ISender mediator;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="mediator">Injected service for mediating command and queries</param>
    /// <param name="translationService">Injected service for localization</param>
    /// <param name="mapper">Injected service for mapping objects</param>
    public InitializationController(ISender mediator, ITranslationService translationService, IMapper mapper) : base(translationService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if the web application has been initialized
    /// </summary>
    [HttpGet()]
    [AllowAnonymous]
    public async Task<IActionResult> CheckInitialization()
    {
        ErrorOr<bool> getResult = await mediator.Send(new CheckInitializationQuery());
        return getResult.Match(result => Ok(result), errors => Problem(errors));
    }

    /// <summary>
    /// Initializes the web application 
    /// </summary>
    /// <param name="data">The admin account registered upon initial setup</param>
    [HttpPost()]
    [AllowAnonymous]
    public async Task<IActionResult> Setup(RegisterRequestDto data)
    {
        ErrorOr<RegistrationResultDto> registerResult = await mediator.Send(mapper.Map<SetupCommand>(data));
        return registerResult.Match(result => Ok(mapper.Map<AuthenticationResponseDto>(result)), errors => Problem(errors));
    }
    #endregion
}