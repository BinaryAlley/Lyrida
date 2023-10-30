#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Domain.Common.DTO;
#endregion

namespace Lyrida.Application.Core.FileSystem.Thumbnails.Queries.Read;

/// <summary>
/// Query for retrieving the thumbnail of a file at a path
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
public record GetThumbnailQuery(string Path, int Quality, int UserId) : IRequest<ErrorOr<ThumbnailDto>>;