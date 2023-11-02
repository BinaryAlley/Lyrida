#region ========================================================================= USING =====================================================================================
using System;
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Common.DTO.Pages;
using Lyrida.DataAccess.Repositories.UserPages;
using Mapster;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Create;

/// <summary>
/// Authentication register command handler
/// </summary>
/// <remarks>
/// Creation Date: 18th of July, 2023
/// </remarks>
public class AddPageCommandHandler : IRequestHandler<AddPageCommand, ErrorOr<PageDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPageRepository userPageRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
   public AddPageCommandHandler(IUnitOfWork unitOfWork)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Registers a new account in the repository
    /// </summary>
    /// <param name="command">The account to be registered</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A DTO containing the register result, or an error</returns>
    public async Task<ErrorOr<PageDto>> Handle(AddPageCommand command, CancellationToken cancellationToken)
    {
        command.Page.UserId = command.UserId;
        var resultInsertUserPage = await userPageRepository.InsertAsync(command.Page.ToStorageDto());
        if (resultInsertUserPage.Error is null && resultInsertUserPage.Data is not null)
            return resultInsertUserPage.Data[0].Adapt<PageDto>();
        else
            return Errors.DataAccess.InsertUserPageError;
    }
    #endregion
}