#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
#endregion

namespace Lyrida.Application.Core.Users.Commands.Delete;

/// <summary>
/// Delete user command handler
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserRepository userRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a user in the repository.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean being <see langword="true"/> if the deletion was successful, or an error.</returns>
    public async Task<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    { 
        // delete the user
        var resultDeleteUser = await userRepository.DeleteByIdAsync(request.Id.ToString());
        if (resultDeleteUser.Error is null && resultDeleteUser.Count > 0)
            return true;
        else
            return Errors.DataAccess.DeleteUserError;
    }
    #endregion
}