/// Written by: Ciprian Horeanu
/// Creation Date: 28th of September, 2023
/// Purpose: Query for retrieving the thumbnail of a file at a path
#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Domain.Common.DTO;
#endregion

namespace Lyrida.Application.Core.FileSystem.Thumbnails.Queries.Read;

public record GetThumbnailQuery(string Path) : IRequest<ErrorOr<ThumbnailDto>>;