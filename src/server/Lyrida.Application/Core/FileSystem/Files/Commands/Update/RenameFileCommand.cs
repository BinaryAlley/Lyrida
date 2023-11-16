#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Update;

/// <summary>
/// Rename file command
/// </summary>
/// <remarks>
/// Creation Date: 13th of November, 2023
/// </remarks>
public record RenameFileCommand(string Path, string Name) : IRequest<ErrorOr<FileDto?>>;