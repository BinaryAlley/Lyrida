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
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.Pages;
using Lyrida.DataAccess.Repositories.UserPages;
#endregion

namespace Lyrida.Application.Core.Pages.Queries.Read;

/// <summary>
/// Get thumbnail query handler
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, ErrorOr<IEnumerable<PageDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPageRepository userPageRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    public GetPagesQueryHandler(IUnitOfWork unitOfWork)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of permissions stored in the repository
    /// </summary>
    /// <returns>A list of permissions</returns>
    public async Task<ErrorOr<IEnumerable<PageDto>>> Handle(GetPagesQuery query, CancellationToken cancellationToken)
    {
        var resultSelectUserPages = await userPageRepository.GetByIdAsync(query.UserId.ToString());
        if (resultSelectUserPages.Error is null)
            return ErrorOrFactory.From(resultSelectUserPages.Data.Adapt<IEnumerable<PageDto>>());
        else
            return Errors.DataAccess.GetUserPagesError;
    }
    #endregion
}