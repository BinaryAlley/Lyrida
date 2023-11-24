#region ========================================================================= USING =====================================================================================
using System;
#endregion

namespace Lyrida.Api.Common.DTO.Pages;

/// <summary>
/// Data transfer object for API pages requests
/// </summary>
/// <remarks>
/// Creation Date: 02nd of November, 2023
/// </remarks>
public record PageRequestDto(Guid PageId, string? Title, string? Path, Guid EnvironmentId);
