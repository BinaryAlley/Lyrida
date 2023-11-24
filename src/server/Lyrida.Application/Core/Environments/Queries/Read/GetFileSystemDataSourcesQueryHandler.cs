#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Mapster;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Common.DTO.Environments;
using Lyrida.DataAccess.Repositories.UserEnvironments;
#endregion

namespace Lyrida.Application.Core.Environments.Queries.Read;

/// <summary>
/// Get user environments query handler
/// </summary>
/// <remarks>
/// Creation Date: 23rd of November, 2023
/// </remarks>
public class GetFileSystemDataSourcesQueryHandler : IRequestHandler<GetFileSystemDataSourcesQuery, ErrorOr<IEnumerable<FileSystemDataSourceDto>>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IUserEnvironmentRepository userEnvironmentRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public GetFileSystemDataSourcesQueryHandler(IUnitOfWork unitOfWork)
    {
        userEnvironmentRepository = unitOfWork.GetRepository<IUserEnvironmentRepository>();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of user environments stored in the repository.
    /// </summary>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either a collection of user environments, or an error.</returns>
    public async Task<ErrorOr<IEnumerable<FileSystemDataSourceDto>>> Handle(GetFileSystemDataSourcesQuery query, CancellationToken cancellationToken)
    {
        var resultSelectUserEnvironments = await userEnvironmentRepository.GetByUserIdAsync(query.UserId.ToString());
        if (resultSelectUserEnvironments.Error is null)
            return ErrorOrFactory.From(resultSelectUserEnvironments.Data.Adapt<IEnumerable<FileSystemDataSourceDto>>());
        else
            return Errors.DataAccess.GetUserEnvironmentsError;
    }
    #endregion
}