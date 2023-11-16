#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Commands.Create;

/// <summary>
/// Copy directory command
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public record CopyDirectoryCommand(string SourcePath, string DestinationPath, bool? OverrideExisting) : IRequest<ErrorOr<DirectoryDto>>;