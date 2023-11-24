#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Mapster;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Common.DTO.Pages;
using Lyrida.DataAccess.Repositories.UserPages;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Create;

/// <summary>
/// Add user page command handler
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public class AddPageCommandHandler : IRequestHandler<AddPageCommand, ErrorOr<PageDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPageRepository userPageRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public AddPageCommandHandler(IUnitOfWork unitOfWork)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Adds a new user page in the repository.
    /// </summary>
    /// <param name="command">The user page to be added.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the added user page, or an error.</returns>
    public async Task<ErrorOr<PageDto>> Handle(AddPageCommand command, CancellationToken cancellationToken)
    {
        // assign the id of current user
        command.Page.UserId = command.UserId;
        // check if a page with the specified id already exists
        var resultGetUserPage = await userPageRepository.GetByIdAsync(command.Page.PageId.ToString());
        if (resultGetUserPage.Error is null)
        {
            if (resultGetUserPage.Data is null)
            {
                // page doesn't exist - insert a new one
                var resultInsertUserPage = await userPageRepository.InsertAsync(command.Page.ToStorageDto());
                if (resultInsertUserPage.Error is not null)
                    return Errors.DataAccess.InsertUserPageError;
            }
            else
            {
                // page exists, update it
                var resultUpdateUserPage = await userPageRepository.UpdateAsync(command.Page.ToStorageDto());
                if (resultUpdateUserPage.Error is not null)
                    return Errors.DataAccess.UpdateUserPageError;
            }
            // get the page again and return it
            resultGetUserPage = await userPageRepository.GetByIdAsync(command.Page.PageId.ToString());
            if (resultGetUserPage.Error is null && resultGetUserPage.Data is not null)
                return resultGetUserPage.Data[0].Adapt<PageDto>();
            else
                return Errors.DataAccess.GetUserEnvironmentsError;            
        }
        else
            return Errors.DataAccess.GetUserEnvironmentsError;
    }
    #endregion
}