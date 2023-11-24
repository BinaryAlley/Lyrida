#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Common.DTO;
using Lyrida.Domain.Core.FileSystem.Services.Thumbnails;
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
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="thumbnailsService">Injected service for handling thumbnails.</param>
    public GetThumbnailQueryHandler(IThumbnailService thumbnailsService)
    {
        this.thumbnailsService = thumbnailsService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the thumbnail for a file located at the specified path, with the specified quality.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a thumbnail, or an error.</returns>
    public async Task<ErrorOr<ThumbnailDto>> Handle(GetThumbnailQuery request, CancellationToken cancellationToken)
    {
        return await thumbnailsService.GetThumbnailAsync(request.Path, request.Quality);   
    }
    #endregion
}