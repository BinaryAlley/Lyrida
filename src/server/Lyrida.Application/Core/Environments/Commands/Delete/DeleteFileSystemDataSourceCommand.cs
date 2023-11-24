#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.Environments.Commands.Delete;

/// <summary>
/// Delete user page command
/// </summary>
/// <remarks>
/// Creation Date: 23rd of November, 2023
/// </remarks>
public record DeleteFileSystemDataSourceCommand(int UserId, string EnvironmentId) : IRequest<ErrorOr<bool>>;