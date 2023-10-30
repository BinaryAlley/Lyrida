#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Paths.Queries.Read;

/// <summary>
/// Query for navigating up one level from a path and parsing the resulting path into its constituent segments
/// </summary>
/// <remarks>
/// Creation Date: 11th of October, 2023
/// </remarks>
public record NavigateUpOneLevelQuery(string Path) : IRequest<ErrorOr<IEnumerable<PathSegmentDto>>>;