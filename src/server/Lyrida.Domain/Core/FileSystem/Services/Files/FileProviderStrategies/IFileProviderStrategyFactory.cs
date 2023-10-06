namespace Lyrida.Domain.Core.FileSystem.Services.Files.FileProviderStrategies;

/// <summary>
/// Interface defining an abstract factory for creating file provider strategies
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public interface IFileProviderStrategyFactory
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate file provider strategy.
    /// </summary>
    /// <typeparam name="TFileProviderStrategy">The type of file provider strategy to create</typeparam>
    /// <returns>The file provider strategy.</returns>
    TFileProviderStrategy CreateStrategy<TFileProviderStrategy>() where TFileProviderStrategy : IFileProviderStrategy;
    #endregion
}