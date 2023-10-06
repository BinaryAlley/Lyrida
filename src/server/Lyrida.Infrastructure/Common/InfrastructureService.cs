﻿#region ========================================================================= USING =====================================================================================
using Lyrida.Infrastructure.Common.Logging;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Common.Configuration;
#endregion

namespace Lyrida.Infrastructure.Common;

/// <summary>
/// Services offered by the Infrastructure Layer
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
public class InfrastructureService : IInfrastructure
{
    #region ==================================================================== PROPERTIES =================================================================================
    public ISecurity Security { get; }
    public ILoggerManager LoggerService { get; }
    public IAppConfig ConfigurationService { get; }
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="security">Injected security infrastructure layer service</param>
    /// <param name="loggerManager">Injected logging infrastructure layer service</param>
    /// <param name="config">Injected configuration infrastructure layer service</param>
    // <param name="translation">Injected translation infrastructure layer service</param>
    public InfrastructureService(ISecurity security, ILoggerManager loggerManager, IAppConfig config)
    {
        Security = security;
        LoggerService = loggerManager;
        ConfigurationService = config;
    }
    #endregion
}