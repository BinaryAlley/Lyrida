#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Domain.Common.Errors;

/// <summary>
/// Domain permission related error types
/// </summary>
/// <remarks>
/// Creation Date: 25th of July, 2023
/// </remarks>
public static partial class Errors
{
    public static class Permission
    {
        #region ==================================================================== PROPERTIES =================================================================================
        public static Error UnauthorizedAccess => Error.Failure(nameof(UnauthorizedAccess));
        #endregion
    }
}