﻿#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace Lyrida.Domain.Common.Models;

/// <summary>
/// Base class for all domain Value Objects
/// </summary>
/// <remarks>
/// Creation Date: 20th of July, 2023
/// </remarks>
/// <typeparam name="TId">The type representing the unique identifier for the Entity. It should be a non-nullable type.</typeparam>
public abstract class ValueObject : IEquatable<ValueObject>
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Gets the list of items that define equality of the object
    /// </summary>
    /// <returns>A list of items defining the equality</returns>
    public abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type
    /// </summary>
    /// <param name="other">An object to compare with this object</param>
    /// <returns>True if the current object is equal to the other parameter, False otherwise</returns>
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    /// <param name="obj">The object to compare with the current object</param>
    /// <returns>True if the specified object is equal to the current object, False otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;
        var valueObject = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    /// <summary>
    /// Custom implementation of the equality operator
    /// </summary>
    /// <param name="left">The left operand of equality</param>
    /// <param name="right">The right operand of equality</param>
    /// <returns>True if <paramref name="left"/> is equal to <paramref name="right"/>, False otherwise</returns>
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Custom implementation of the inequality operator
    /// </summary>
    /// <param name="left">The left operand of equality</param>
    /// <param name="right">The right operand of equality</param>
    /// <returns>True if <paramref name="left"/> is not equal to <paramref name="right"/>, False otherwise</returns>
    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Override of the default hash function
    /// </summary>
    /// <returns>A hash code for the current object</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0)
                                      .Aggregate((x, y) => x ^ y);
    }
    #endregion
}