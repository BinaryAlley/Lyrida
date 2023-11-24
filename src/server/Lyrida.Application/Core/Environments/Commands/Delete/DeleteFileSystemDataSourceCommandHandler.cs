#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.UserEnvironments;
#endregion

namespace Lyrida.Application.Core.Environments.Commands.Delete;

/// <summary>
/// Delete user file system data source command handler
/// </summary>
/// <remarks>
/// Creation Date: 23rd of November, 2023
/// </remarks>
public class DeleteFileSystemDataSourceCommandHandler : IRequestHandler<DeleteFileSystemDataSourceCommand, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserEnvironmentRepository userEnvironmentRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public DeleteFileSystemDataSourceCommandHandler(IUnitOfWork unitOfWork)
    {
        userEnvironmentRepository = unitOfWork.GetRepository<IUserEnvironmentRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Deletes a user file system data source in the repository.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a boolean being <see langword="true"/> if the deletion was successful, or an error.</returns>
    public async Task<ErrorOr<bool>> Handle(DeleteFileSystemDataSourceCommand query, CancellationToken cancellationToken)
    {
        // first, make sure the user environment belongs to the user
        var resultSelectUserPage = await userEnvironmentRepository.GetByIdAsync(query.EnvironmentId);
        if (resultSelectUserPage.Error is null && resultSelectUserPage.Data is not null)
        {
            if (resultSelectUserPage.Data[0].UserId == query.UserId)
            {
                // delete the user environment
                var resultDeleteUserPage = await userEnvironmentRepository.DeleteByIdAsync(query.EnvironmentId);
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