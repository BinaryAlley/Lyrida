#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.Environments;
#endregion

namespace Lyrida.Application.Core.Environments.Queries.Read;

/// <summary>
/// Query for retrieving the list of environments of a user
/// </summary>
/// <remarks>
/// Creation Date: 23rd of November, 2023
/// </remarks>
public record GetFileSystemDataSourcesQuery(int UserId) : IRequest<ErrorOr<IEnumerable<FileSystemDataSourceDto>>>;