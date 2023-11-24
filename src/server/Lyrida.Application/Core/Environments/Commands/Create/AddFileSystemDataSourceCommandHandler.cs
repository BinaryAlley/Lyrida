#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Mapster;
using System.Threading;
using Lyrida.DataAccess.UoW;
using System.Threading.Tasks;
using Lyrida.Domain.Common.Errors;
using Lyrida.Application.Common.DTO.Pages;
using Lyrida.Application.Common.DTO.Environments;
using Lyrida.DataAccess.Repositories.UserEnvironments;
using System.Linq;
using Lyrida.Infrastructure.Common.Security;
using System;
#endregion

namespace Lyrida.Application.Core.Environments.Commands.Create;

/// <summary>
/// Add file system data source command handler
/// </summary>
/// <remarks>
/// Creation Date: 22nd of November, 2023
/// </remarks>
public class AddFileSystemDataSourceCommandHandler : IRequestHandler<AddFileSystemDataSourceCommand, ErrorOr<FileSystemDataSourceDto>>
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IHash hashService;
    private readonly IUserEnvironmentRepository userEnvironmentRepository;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor.
    /// </summary>
    /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories.</param>
    public AddFileSystemDataSourceCommandHandler(IUnitOfWork unitOfWork, IHash hashService)
    {
        userEnvironmentRepository = unitOfWork.GetRepository<IUserEnvironmentRepository>();
        this.hashService = hashService;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Adds or updates a file system data source in the storage repository.
    /// </summary>
    /// <param name="command">The file system data source to be added or updated.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>An <see cref="ErrorOr{T}"/> containing either the file system data source, or an error.</returns>
    public async Task<ErrorOr<FileSystemDataSourceDto>> Handle(AddFileSystemDataSourceCommand command, CancellationToken cancellationToken)
    {
        // assign the id of current user
        command.DataSource.UserId = command.UserId;
        // hash passwords before sending them to repository!
        if (!string.IsNullOrEmpty(command.DataSource.Password))
            command.DataSource.Password = Uri.EscapeDataString(hashService.HashString(command.DataSource.Password));
        // check if an environment with the specified id already exists
        var resultGetUserEnvironment = await userEnvironmentRepository.GetByIdAsync(command.DataSource.EnvironmentId.ToString());
        if (resultGetUserEnvironment.Error is null)
        {
            if (resultGetUserEnvironment.Data is null)
            {
                // environment doesn't exist - insert a new one
                var resultInsertUserEnvironment = await userEnvironmentRepository.InsertAsync(command.DataSource.ToStorageDto());
                if (resultInsertUserEnvironment.Error is not null)
                    return Errors.DataAccess.InsertUserEnvironmentError;
            }
            else
            {
                // environment exists, update it
                var resultUpdateUserEnvironment = await userEnvironmentRepository.UpdateAsync(command.DataSource.ToStorageDto());
                if (resultUpdateUserEnvironment.Error is not null)
                    return Errors.DataAccess.UpdateUserEnvironmentError;
            }
            // get the environment again and return it
            resultGetUserEnvironment = await userEnvironmentRepository.GetByIdAsync(command.DataSource.EnvironmentId.ToString());
            if (resultGetUserEnvironment.Error is null && resultGetUserEnvironment.Data is not null)
            {
                resultGetUserEnvironment.Data[0].Password = string.Empty; // do not include the passwords in the returned result!
                return resultGetUserEnvironment.Data[0].Adapt<FileSystemDataSourceDto>();
            }
            else
                return Errors.DataAccess.GetUserEnvironmentsError;
        }
        else
            return Errors.DataAccess.GetUserEnvironmentsError;
    }
    #endregion
}