#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Core.Authorization;
using Lyrida.DataAccess.Repositories.UserPages;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Delete;

/// <summary>
/// Delete user page command handler
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPageRepository userPageRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    /// <param name="authorizationService">Injected service for permissions</param>
    public DeletePageCommandHandler(IUnitOfWork unitOfWork, IAuthorizationService authorizationService)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a user page in the repository
    /// </summary>
    /// <returns>True, if the deletion was successful, an Error otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(DeletePageCommand query, CancellationToken cancellationToken)
    {
        // delete the user page
        var resultDeleteUserPage = await userPageRepository.DeleteByIdAsync(query.UserId.ToString(), query.PageId);
        if (resultDeleteUserPage.Error is null && resultDeleteUserPage.Count > 0)
            return true;
        else
            return Errors.DataAccess.DeleteUserPageError;
    }
    #endregion
}