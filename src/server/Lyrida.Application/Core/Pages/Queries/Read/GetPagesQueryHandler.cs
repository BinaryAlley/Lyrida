#region ========================================================================= USING =====================================================================================
using MediatR;
using Mapster;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Common.DTO.Pages;
using Lyrida.DataAccess.Repositories.UserPages;
#endregion

namespace Lyrida.Application.Core.Pages.Queries.Read;

/// <summary>
/// Get open user pages query handler
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, ErrorOr<IEnumerable<PageDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPageRepository userPageRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public GetPagesQueryHandler(IUnitOfWork unitOfWork)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of user pages stored in the repository.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of user pages, or an error.</returns>
    public async Task<ErrorOr<IEnumerable<PageDto>>> Handle(GetPagesQuery query, CancellationToken cancellationToken)
    {
        var resultSelectUserPages = await userPageRepository.GetByUserIdAsync(query.UserId.ToString());
        if (resultSelectUserPages.Error is null)
            return ErrorOrFactory.From(resultSelectUserPages.Data.Adapt<IEnumerable<PageDto>>());
        else
            return Errors.DataAccess.GetUserPagesError;
    }
    #endregion
}