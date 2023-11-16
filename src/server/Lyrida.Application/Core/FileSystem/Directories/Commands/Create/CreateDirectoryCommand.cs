#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Create;

/// <summary>
/// Create directory command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public record CreateDirectoryCommand(string Path, string Name) : IRequest<ErrorOr<DirectoryDto>>;