namespace Lyrida.Domain.Core.FileSystem.Services.Directories.DirectoryProviderStrategies;

/// <summary>
/// Interface defining an abstract factory for creating directory provider strategies
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public interface IDirectoryProviderStrategyFactory
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate directory provider strategy.
    /// </summary>
    /// <typeparam name="TDirectoryProviderStrategy">The type of directory provider strategy to create</typeparam>
    /// <returns>The directory provider strategy.</returns>
    TDirectoryProviderStrategy CreateStrategy<TDirectoryProviderStrategy>() where TDirectoryProviderStrategy : IDirectoryProviderStrategy;
    #endregion
}