﻿namespace Lyrida.Api.Common.DTO.Authorization;

/// <summary>
/// Data transfer object for API setting user roles requests
/// </summary>
/// <remarks>
/// Creation Date: 11th of August, 2023
/// </remarks>
public record AddUserRoleRequestDto(int UserId, string? RoleId);