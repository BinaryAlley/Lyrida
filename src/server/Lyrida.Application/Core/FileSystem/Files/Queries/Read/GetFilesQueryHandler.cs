#region ========================================================================= USING =====================================================================================
using Mapster;
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.Entities;
using Lyrida.Application.Common.DTO.FileSystem;
using Lyrida.Domain.Core.FileSystem.Services.Files;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Queries.Read;

/// <summary>
/// Get folders query handler
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, ErrorOr<IEnumerable<FileDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IFileService fileService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="fileService">Injected service for file related functionality.</param>
    public GetFilesQueryHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of files at the specified path.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of files, or an error.</returns>
    public async Task<ErrorOr<IEnumerable<FileDto>>> Handle(GetFilesQuery query, CancellationToken cancellationToken)
    {
        ErrorOr<IEnumerable<File>> result = await fileService.GetFilesAsync(query.Path);
        return result.Match(values => ErrorOrFactory.From(values.Adapt<IEnumerable<FileDto>>()), errors => errors);
    }
    #endregion
}