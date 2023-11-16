#region ========================================================================= USING =====================================================================================
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Lyrida.Domain.Core.FileSystem.Services.Paths;
#endregion

namespace Lyrida.Application.Core.FileSystem.Paths.Queries.Read;

/// <summary>
/// Validate path query handler
/// </summary>
/// <remarks>
/// Creation Date: 02nd of October, 2023
/// </remarks>
public class ValidatePathQueryHandler : IRequestHandler<ValidatePathQuery, bool>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPathService pathService;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="pathService">Injected domain service for path related functionality</param>
    public ValidatePathQueryHandler(IPathService pathService)
    {
        this.pathService = pathService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Validates the validity of a path.
    /// </summary>
    /// <returns><see langword="true"/> if the provided path is valid, <see langword="false"/> otherwise.</returns>
    public Task<bool> Handle(ValidatePathQuery query, CancellationToken cancellationToken)
    {
         return Task.FromResult(pathService.IsValidPath(query.Path));
    }
    #endregion
}