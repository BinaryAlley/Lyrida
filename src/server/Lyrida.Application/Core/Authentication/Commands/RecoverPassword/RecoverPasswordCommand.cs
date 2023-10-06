﻿#region ========================================================================= USING =====================================================================================
using MediatR;
using ErrorOr;
using Lyrida.Application.Common.Entities.Common;
#endregion

namespace Lyrida.Application.Core.Authentication.Commands.RecoverPassword;

/// <summary>
/// Recover password command
/// </summary>
/// <remarks>
/// Creation Date: 01st of August, 2023
/// </remarks>
public record RecoverPasswordCommand(string Email) : IRequest<ErrorOr<CommandResultEntity>>;