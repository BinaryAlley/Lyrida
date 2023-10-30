#region ========================================================================= USING =====================================================================================
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Common.DTO;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Core.Authorization;
using Lyrida.DataAccess.Repositories.Configuration;
using Lyrida.Domain.Core.FileSystem.Services.Thumbnails;
using Lyrida.DataAccess.UoW;
using Lyrida.Application.Common.DTO.Configuration;
#endregion

namespace Lyrida.Application.Core.FileSystem.Thumbnails.Queries.Read;

/// <summary>
/// Get thumbnail query handler
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
public class GetThumbnailQueryHandler : IRequestHandler<GetThumbnailQuery, ErrorOr<ThumbnailDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IThumbnailService thumbnailsService;
    private readonly IAuthorizationService authorizationService;
    private readonly IUserPreferenceRepository userPreferenceRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetThumbnailQueryHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService, IThumbnailService thumbnailsService)
    {
        this.thumbnailsService = thumbnailsService;
        this.authorizationService = authorizationService;
        userPreferenceRepository = unitOfWork.GetRepository<IUserPreferenceRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of permissions stored in the repository
    /// </summary>
    /// <returns>A list of permissions</returns>
    public async Task<ErrorOr<ThumbnailDto>> Handle(GetThumbnailQuery request, CancellationToken cancellationToken)
    {
        // check if the user has the permission to perform the action
        //if (authorizationService.UserPermissions.CanViewPermissions)
        {
            
            return await thumbnailsService.GetThumbnailAsync(request.Path, request.Quality);   
        }
        //else
        //return Errors.Authorization.InvalidPermission;
    }
    #endregion
}