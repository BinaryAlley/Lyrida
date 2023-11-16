#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Update;

/// <summary>
/// Move directory command
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public record MoveDirectoryCommand(string SourcePath, string DestinationPath, bool? OverrideExisting) : IRequest<ErrorOr<DirectoryDto>>;