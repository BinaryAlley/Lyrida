namespace Lyrida.Domain.Core.FileSystem.Services.Platform;

/// <summary>
/// Interface defining an abstract factory for creating platform contexts
/// </summary>
/// <remarks>
/// Creation Date: 04th of September, 2023
/// </remarks>
public interface IPlatformContextFactory
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Creates and returns the appropriate platform context.
    /// </summary>
    /// <typeparam name="TIPlatformContext">The type of platform context to create</typeparam>
    /// <returns>The platform context.</returns>
    TIPlatformContext CreateStrategy<TIPlatformContext>() where TIPlatformContext : IPlatformContext;
    #endregion
}