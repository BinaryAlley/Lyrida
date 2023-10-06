namespace Lyrida.Domain.Core.FileSystem.Services.Environment;

/// <summary>
/// Interface defining an abstract factory for creating environment contexts
/// </summary>
/// <remarks>
/// Creation Date: 29th of September, 2023
/// </remarks>
public interface IEnvironmentContextFactory
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate environment context.
    /// </summary>
    /// <typeparam name="TIEnvironmentContext">The type of environment context to create</typeparam>
    /// <returns>The environment context.</returns>
    TIEnvironmentContext CreateStrategy<TIEnvironmentContext>() where TIEnvironmentContext : IEnvironmentContext;
    #endregion
}