#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Update;

/// <summary>
/// Rename directory command
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
public record RenameDirectoryCommand(string Path, string Name) : IRequest<ErrorOr<DirectoryDto?>>;