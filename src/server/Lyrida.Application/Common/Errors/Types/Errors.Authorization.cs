#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Application.Common.Errors.Types;

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
        public static Error CannotDeleteAdminAccount => Error.Failure(nameof(CannotDeleteAdminAccount));
        public static Error CannotDeleteAdminRole => Error.Failure(nameof(CannotDeleteAdminRole));
        public static Error CannotSetAdminRole => Error.Failure(nameof(CannotSetAdminRole));
        public static Error CannotUpdateAdminRole => Error.Failure(nameof(CannotUpdateAdminRole));
        public static Error RoleAlreadyExists => Error.Conflict(nameof(RoleAlreadyExists));
        public static Error RoleDoesNotExist => Error.Conflict(nameof(RoleDoesNotExist));
        public static Error InvalidPermission => Error.Failure(nameof(InvalidPermission));
        #endregion
    }
}