#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
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
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public DeletePageCommandHandler(IUnitOfWork unitOfWork)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a user page in the repository.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean being <see langword="true"/> if the deletion was successful, or an error.</returns>
    public async Task<ErrorOr<bool>> Handle(DeletePageCommand query, CancellationToken cancellationToken)
    {
        // first, make sure the user page belongs to the user
        var resultSelectUserPage = await userPageRepository.GetByIdAsync(query.PageId);
        if (resultSelectUserPage.Error is null && resultSelectUserPage.Data is not null)
        {
            if (resultSelectUserPage.Data[0].UserId == query.UserId)
            {
                // delete the user page
                var resultDeleteUserPage = await userPageRepository.DeleteByIdAsync(query.PageId);
                if (resultDeleteUserPage.Error is null && resultDeleteUserPage.Count > 0)
                    return true;
                else
                    return Errors.DataAccess.DeleteUserPageError;
            }
            else
                return Errors.Authorization.InvalidPermissionError;
        }
        else
            return Errors.DataAccess.GetUserPagesError;
    }
    #endregion
}