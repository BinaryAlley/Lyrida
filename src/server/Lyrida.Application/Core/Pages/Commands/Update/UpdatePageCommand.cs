#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.DTO.Pages;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Update;

/// <summary>
/// Update user page command
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public record UpdatePageCommand(int UserId, PageDto Page) : IRequest<ErrorOr<bool>>;