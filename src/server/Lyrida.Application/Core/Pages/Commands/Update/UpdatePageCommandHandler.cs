#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.UserPages;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Update;

/// <summary>
/// Update user page command handler
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserPageRepository userPageRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    public UpdatePageCommandHandler(IUnitOfWork unitOfWork)
    {
        userPageRepository = unitOfWork.GetRepository<IUserPageRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Updates a user page in the respository.
    /// </summary>
    /// <returns>True, if the action was successful, an Error otherwise.</returns>
    public async Task<ErrorOr<bool>> Handle(UpdatePageCommand command, CancellationToken cancellationToken)
    {
        command.Page.UserId = command.UserId;
        var resultUpdateRole = await userPageRepository.UpdateAsync(command.Page.ToStorageDto());
        if (resultUpdateRole.Error is null && resultUpdateRole.Count > 0)
            return true;
        else
            return Errors.DataAccess.InsertRoleError;
    }
    #endregion
}