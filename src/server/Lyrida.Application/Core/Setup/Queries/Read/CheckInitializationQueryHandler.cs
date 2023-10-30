#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.DataAccess.Repositories.Users;
#endregion

namespace Lyrida.Application.Core.Setup.Queries.Read;

/// <summary>
/// Check application initialization query handler
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public class CheckInitializationQueryHandler : IRequestHandler<CheckInitializationQuery, ErrorOr<bool>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserRepository userRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
    public CheckInitializationQueryHandler(IUnitOfWork unitOfWork)
    {
        userRepository = unitOfWork.GetRepository<IUserRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if the application has been initialized or not
    /// </summary>
    /// <returns><see langword="true"/> if the application has been initialized, <see langword="false"/> otherwise</returns>
    public async Task<ErrorOr<bool>> Handle(CheckInitializationQuery request, CancellationToken cancellationToken)
    {
        // if the repository reports an error, or there are no users, the application has not been initialized
        var resultSelectUser = await userRepository.GetAllAsync();
        if (resultSelectUser.Error is null)
            return resultSelectUser.Data is not null;
        else
        {
            if (resultSelectUser.Error.Contains("Table") && resultSelectUser.Error.Contains("doesn't exist"))
                return Errors.DataAccess.UninitializedDatabaseError;
            else
                return Errors.DataAccess.GetUsersError;
        }
    }
    #endregion
}