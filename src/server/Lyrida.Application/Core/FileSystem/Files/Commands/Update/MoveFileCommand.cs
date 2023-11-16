#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Update;

/// <summary>
/// Move file command
/// </summary>
/// <remarks>
/// Creation Date: 14th of November, 2023
/// </remarks>
public record MoveFileCommand(string SourcePath, string DestinationPath, bool? OverrideExisting) : IRequest<ErrorOr<FileDto>>;