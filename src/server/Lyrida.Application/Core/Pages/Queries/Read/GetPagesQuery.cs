#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using System.Collections.Generic;
using Lyrida.Application.Common.DTO.Pages;
#endregion

namespace Lyrida.Application.Core.Pages.Queries.Read;

/// <summary>
/// Query for retrieving the list of open pages of a user
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public record GetPagesQuery(int UserId) : IRequest<ErrorOr<IEnumerable<PageDto>>>;