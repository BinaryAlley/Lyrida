#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Commands.Delete;

/// <summary>
/// Delete file command
/// </summary>
/// <remarks>
/// Creation Date: 11th of November, 2023
/// </remarks>
public record DeleteFileCommand(string Path) : IRequest<ErrorOr<bool>>; 