#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Directories.Queries.Read;

/// <summary>
/// Query for retrieving the list of folders at a path
/// </summary>
/// <remarks>
/// Creation Date: 25th of September, 2023
/// </remarks>
public record GetDirectoriesQuery(string Path) : IRequest<ErrorOr<IEnumerable<DirectoryDto>>>;