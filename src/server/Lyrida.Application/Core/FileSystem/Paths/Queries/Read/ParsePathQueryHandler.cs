#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Mapster;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Core.FileSystem.ValueObjects;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
using Lyrida.Application.Common.Entities.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Paths.Queries.Read;

/// <summary>
/// Parse path query handler
/// </summary>
/// <remarks>
/// Creation Date: 04th of October, 2023
/// </remarks>
public class ParsePathQueryHandler : IRequestHandler<ParsePathQuery, ErrorOr<IEnumerable<PathSegmentEntity>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPathService pathService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="pathService">Injected domain service for path related functionality</param>
    public ParsePathQueryHandler(IPathService pathService)
    {
        this.pathService = pathService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Parses a path into its constituent seegments
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing the path segments, or an error.</returns>
    public Task<ErrorOr<IEnumerable<PathSegmentEntity>>> Handle(ParsePathQuery request, CancellationToken cancellationToken)
    {
        ErrorOr<IEnumerable<PathSegment>> result = pathService.ParsePath(request.Path);
        return Task.FromResult(result.Match(values => ErrorOrFactory.From(values.Adapt<IEnumerable<PathSegmentEntity>>()), errors => errors));
    }
    #endregion
}