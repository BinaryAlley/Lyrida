#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
#endregion

namespace Lyrida.Application.Core.Setup.Queries.Read;

/// <summary>
/// Query for checking if the application has been initialized
/// </summary>
/// <remarks>
/// Creation Date: 14th of August, 2023
/// </remarks>
public record CheckInitializationQuery() : IRequest<ErrorOr<bool>>;