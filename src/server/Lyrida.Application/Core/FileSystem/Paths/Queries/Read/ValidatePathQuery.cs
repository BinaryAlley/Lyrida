#region ========================================================================= USING =====================================================================================
using MediatR;
#endregion

namespace Lyrida.Application.Core.FileSystem.Paths.Queries.Read;

/// <summary>
/// Query for checking the validity of a path
/// </summary>
/// <remarks>
/// Creation Date: 02nd of October, 2023
/// </remarks>
public record ValidatePathQuery(string Path) : IRequest<bool>;