#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Paths.Queries.Read;

/// <summary>
/// Navigate up one level query handler
/// </summary>
/// <remarks>
/// Creation Date: 11th of October, 2023
/// </remarks>
public class NavigateUpOneLevelQueryHandler : IRequestHandler<NavigateUpOneLevelQuery, ErrorOr<IEnumerable<PathSegmentDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPathService pathService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="pathService">Injected domain service for path related functionality</param>
    public NavigateUpOneLevelQueryHandler(IPathService pathService)
    {
        this.pathService = pathService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Navigates up one level from a path and parses the resulting path into its constituent segments
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public Task<ErrorOr<IEnumerable<PathSegmentDto>>> Handle(NavigateUpOneLevelQuery request, CancellationToken cancellationToken)
    {
        ErrorOr<IEnumerable<PathSegment>> result = pathService.GoUpOneLevel(request.Path);
        return Task.FromResult(result.Match(values => ErrorOrFactory.From(values.Adapt<IEnumerable<PathSegmentDto>>()), errors => errors));
    }
    #endregion
}