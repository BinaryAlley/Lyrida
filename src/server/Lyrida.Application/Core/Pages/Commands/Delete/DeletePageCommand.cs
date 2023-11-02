#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.Pages.Commands.Delete;

/// <summary>
/// Delete user page command
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public record DeletePageCommand(int UserId, string PageId) : IRequest<ErrorOr<bool>>;