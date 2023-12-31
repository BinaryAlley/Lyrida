﻿#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Directories;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Queries.Read;

/// <summary>
/// Get folders query handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public class GetDirectoriesQueryHandler : IRequestHandler<GetDirectoriesQuery, ErrorOr<IEnumerable<DirectoryDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDirectoryService directoryService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="directoryService">Injected service for handling directories.</param>
    public GetDirectoriesQueryHandler(IDirectoryService directoryService)
    {
        this.directoryService = directoryService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of directories at the specified path.
    /// </summary>
    /// <returns>A list of directories.</returns>
    public async Task<ErrorOr<IEnumerable<DirectoryDto>>> Handle(GetDirectoriesQuery request, CancellationToken cancellationToken)
    {
        ErrorOr<IEnumerable<Directory>> result = await directoryService.GetSubdirectoriesAsync(request.Path);            
        return result.Match(values => ErrorOrFactory.From(values.Adapt<IEnumerable<DirectoryDto>>()), errors => errors);
    }
    #endregion
}