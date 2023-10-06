namespace Lyrida.Domain.Common.Models;

/// <summary>
/// Base class for all domain Aggregate Roots
/// </summary>
/// <remarks>
/// Creation Date: 20th of July, 2023
/// </remarks>
/// <typeparam name="TId">The type representing the unique identifier for the Aggregate Root. It should be a non-nullable type.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="id">The id of the entity</param>
    protected AggregateRoot(TId id) : base(id)
    {
        
    }
    #endregion
}