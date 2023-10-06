/// Written by: Ciprian Horeanu
/// Creation Date: 28th of September, 2023
/// Purpose: Get thumbnail query handler
#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Core.Authorization;
using System.Threading.Tasks;
using System.Threading;
using Mapster;
using Lyrida.Domain.Core.FileSystem.Services.Thumbnails;
using Lyrida.Domain.Common.DTO;
//using Lyrida.Domain.Core.FileSystem.Services.Thumbnails;
#endregion

namespace Lyrida.Application.Core.FileSystem.Thumbnails.Queries.Read;

public class GetThumbnailQueryHandler : IRequestHandler<GetThumbnailQuery, ErrorOr<ThumbnailDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IThumbnailService thumbnailsService;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetThumbnailQueryHandler(IAuthorizationService authorizationService, IThumbnailService thumbnailsService)
    {
        this.authorizationService = authorizationService;
        this.thumbnailsService = thumbnailsService;
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
            return await thumbnailsService.GetThumbnailAsync(request.Path);
            //return new ThumbnailDto( Domain.Common.Enums.ImageType.JPEG, System.IO.File.ReadAllBytes("A:\\Tigernails DDD\\Code\\TigerNails.Views\\Resources\\Images\\logo GN.jpg"));
            //return System.IO.File.ReadAllBytes(request.Path);
            //return result.Match(values => ErrorOrFactory.From(values.Adapt<byte[]>()), errors => errors);
            //return directories;
            //return ErrorOrFactory.From(result.Adapt<IEnumerable<DirectoryEntity>>());
            //return ErrorOr<IEnumerable<DirectoryEntity>>.From(result.Adapt<DirectoryEntity[]>());
            //return Array.Empty<Directory>();
            //// get the list of permissions from the repository
            //var resultSelectPermissions = await permissionRepository.GetAllAsync();
            //if (resultSelectPermissions.Error is null)
            //{
            //    //if (resultSelectPermissions.Data is not null)
            //    //    return resultSelectPermissions.Data.Adapt<PermissionEntity[]>();
            //    //else
            //        return Array.Empty<FileSystemItem>();
            //}
            //else
            //    return Errors.DataAccess.GetUserPermissionsError;
        }
        //else
        //return Errors.Authorization.InvalidPermission;
    }
    #endregion
}