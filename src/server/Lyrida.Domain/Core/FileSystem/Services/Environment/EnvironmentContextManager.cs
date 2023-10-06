#region ========================================================================= USING =====================================================================================
using System;
using Lyrida.Domain.Common.Enums;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Service for managing the environment context
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
internal class EnvironmentContextManager : IEnvironmentContextManager
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private IEnvironmentContext? currentEnvironmentContext;
    private readonly IEnvironmentContextFactory environmentContextFactory;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="environmentContextFactory">Injected abstract factory for creating environment contexts</param>
    public EnvironmentContextManager(IEnvironmentContextFactory environmentContextFactory)
    {
        this.environmentContextFactory = environmentContextFactory;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the current environment context
    /// </summary>
    /// <returns>The current environment context</returns>
    public IEnvironmentContext GetCurrentContext()
    {
        // set a default context if none is set
        if (currentEnvironmentContext is null)
            SetCurrentEnvironment(EnvironmentType.LocalFileSystem);
        return currentEnvironmentContext!;
    }

    /// <summary>
    /// Sets the current environment context
    /// </summary>
    /// <param name="environmentType">The environment to set</param>
    /// <exception cref="ArgumentException">Thrown when an unsupported environment type is provided</exception>
    public void SetCurrentEnvironment(EnvironmentType environmentType)
    {
        // determine the correct context based on environmentType
        currentEnvironmentContext = environmentType switch
        {
            EnvironmentType.Ftp => environmentContextFactory.CreateStrategy<IFtpEnvironmentContext>(),
            EnvironmentType.LocalFileSystem => environmentContextFactory.CreateStrategy<ILocalSystemEnvironmentContext>(),
            _ => throw new ArgumentException($"Unsupported environment type: {environmentType}"),
        };
    }
    #endregion
}
