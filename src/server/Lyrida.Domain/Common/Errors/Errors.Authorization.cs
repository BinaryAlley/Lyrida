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
        public static Error DeleteAdminRoleError => Error.Failure(nameof(DeleteAdminRoleError));
        public static Error SetAdminRoleError => Error.Failure(nameof(SetAdminRoleError));
        public static Error UpdateAdminRoleError => Error.Failure(nameof(UpdateAdminRoleError));
        public static Error DuplicateRoleError => Error.Failure(nameof(DuplicateRoleError));
        public static Error RoleDoesNotExistError => Error.Failure(nameof(RoleDoesNotExistError));
        public static Error InvalidPermissionError => Error.Failure(nameof(InvalidPermissionError));
        #endregion
    }
}