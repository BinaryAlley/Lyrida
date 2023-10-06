#region ========================================================================= USING =====================================================================================
using Mapster;
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Application.Core.Authorization;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Domain.Core.FileSystem.Services.Files;
using Lyrida.Application.Common.Entities.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Queries.Read;

/// <summary>
/// Get folders query handler
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, ErrorOr<IEnumerable<FileEntity>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileService fileService;
    private readonly IAuthorizationService authorizationService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public GetFilesQueryHandler(IAuthorizationService authorizationService, IFileService fileService)
    {
        this.fileService = fileService;
        this.authorizationService = authorizationService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of files at the specified path
    /// </summary>
    /// <returns>A list of files</returns>
    public async Task<ErrorOr<IEnumerable<FileEntity>>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
    {
        ErrorOr<IEnumerable<File>> result = await fileService.GetFilesAsync(request.Path);
        return result.Match(values => ErrorOrFactory.From(values.Adapt<IEnumerable<FileEntity>>()), errors => errors);
    }
    #endregion
}