#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Environments;
#endregion

namespace Lyrida.Application.Core.Environments.Commands.Create;

/// <summary>
/// Add file system data source command
/// </summary>
/// <remarks>
/// Creation Date: 22nd of November, 2023
/// </remarks>
public record AddFileSystemDataSourceCommand(int UserId, FileSystemDataSourceDto DataSource) : IRequest<ErrorOr<FileSystemDataSourceDto>>;