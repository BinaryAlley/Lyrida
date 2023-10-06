#region ========================================================================= USING =====================================================================================
using Lyrida.Infrastructure.Common.Configuration;
using Lyrida.Infrastructure.Common.Security;
using Lyrida.Infrastructure.Common.Logging;
#endregion

namespace Lyrida.Infrastructure.Common;

/// <summary>
/// Facade interface for the services offered by the Infrastructure Layer
/// </summary>
/// <remarks>
/// Creation Date: 11th of July, 2023
/// </remarks>
public interface IInfrastructure
{
    #region ==================================================================== PROPERTIES =================================================================================
    ISecurity Security { get; }
    ILoggerManager LoggerService { get; }
    IAppConfig ConfigurationService { get; }
    #endregion
}