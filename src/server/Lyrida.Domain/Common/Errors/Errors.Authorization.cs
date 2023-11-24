#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Domain.Common.Errors;

/// <summary>
/// Authorization error types
/// </summary>
/// <remarks>
/// Creation Date: 09th of August, 2023
/// </remarks>
public static partial class Errors
{
    public static class Authorization
    {
        #region ==================================================================== PROPERTIES =================================================================================
        public static Error DeleteAdminAccountError => Error.Failure(nameof(DeleteAdminAccountError));
        public static Error InvalidPermissionError => Error.Failure(nameof(InvalidPermissionError));
        #endregion
    }
}