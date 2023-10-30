#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Files.Queries.Read;

/// <summary>
/// Query for retrieving the list of files at a path
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public record GetFilesQuery(string Path) : IRequest<ErrorOr<IEnumerable<FileDto>>>;