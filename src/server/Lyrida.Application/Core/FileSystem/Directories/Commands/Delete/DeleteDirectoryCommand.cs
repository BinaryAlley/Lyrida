#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Delete;

/// <summary>
/// Delete directory command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public record DeleteDirectoryCommand(string Path) : IRequest<ErrorOr<bool>>;