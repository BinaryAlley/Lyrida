#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Domain.Common.Errors;

/// <summary>
/// Data Access Layer error types
/// </summary>
/// <remarks>
/// Creation Date: 15th of July, 2023
/// </remarks>
public static partial class Errors
{
    public static class DataAccess
    {
        #region ==================================================================== PROPERTIES =================================================================================
        public static Error GetUserError => Error.Failure(nameof(GetUserError));
        public static Error GetUserPermissionsError => Error.Failure(nameof(GetUserPermissionsError));
        public static Error GetUserPreferencesError => Error.Failure(nameof(GetUserPreferencesError));
        public static Error GetPermissionsError => Error.Failure(nameof(GetPermissionsError));
        public static Error GetUsersError => Error.Failure(nameof(GetUsersError));
        public static Error GetUserRolesError => Error.Failure(nameof(GetUserRolesError));
        public static Error GetRolesError => Error.Failure(nameof(GetRolesError));
        public static Error GetRolePermissionsError => Error.Failure(nameof(GetRolePermissionsError));
        public static Error GetRoleError => Error.Failure(nameof(GetRoleError));
        public static Error InsertUserError => Error.Failure(nameof(InsertUserError));
        public static Error InsertRoleError => Error.Failure(nameof(InsertRoleError));
        public static Error InsertUserPreferencesError => Error.Failure(nameof(InsertUserPreferencesError));
        public static Error InsertPermissionError => Error.Failure(nameof(InsertPermissionError));
        public static Error UpdateUserError => Error.Failure(nameof(UpdateUserError));
        public static Error UpdateUserRoleError => Error.Failure(nameof(UpdateUserRoleError));
        public static Error UpdateUserPermissionsError => Error.Failure(nameof(UpdateUserPermissionsError));
        public static Error UpdateUserPreferencesError => Error.Failure(nameof(UpdateUserPreferencesError));
        public static Error DeleteUserError => Error.Failure(nameof(DeleteUserError));
        public static Error DeleteRoleError => Error.Failure(nameof(DeleteRoleError));
        public static Error DatabaseSetupError => Error.Failure(nameof(DatabaseSetupError));
        public static Error ValidateAccountError => Error.Validation(nameof(ValidateAccountError));
        public static Error UninitializedDatabaseError => Error.Failure(nameof(UninitializedDatabaseError));
        #endregion
    }
}