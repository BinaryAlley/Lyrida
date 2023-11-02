#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Pages;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Create;

/// <summary>
/// Add page command
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public record AddPageCommand(int UserId, PageDto Page) : IRequest<ErrorOr<PageDto>>;