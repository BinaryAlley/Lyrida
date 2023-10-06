#region ========================================================================= USING =====================================================================================
using ErrorOr;
using MediatR;
using System.Collections.Generic;
using Lyrida.Application.Common.Entities.FileSystem;
#endregion

namespace Lyrida.Application.Core.FileSystem.Paths.Queries.Read;

/// <summary>
/// Query for parsing a path into its constituent segments
/// </summary>
/// <remarks>
/// Creation Date: 04th of October, 2023
/// </remarks>
public record ParsePathQuery(string Path) : IRequest<ErrorOr<IEnumerable<PathSegmentEntity>>>;