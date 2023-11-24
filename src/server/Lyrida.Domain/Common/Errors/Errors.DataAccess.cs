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
        public static Error GetUserPreferencesError => Error.Failure(nameof(GetUserPreferencesError));
        public static Error GetUserPagesError => Error.Failure(nameof(GetUserPagesError));
        public static Error GetUserEnvironmentsError => Error.Failure(nameof(GetUserEnvironmentsError));
        public static Error GetUsersError => Error.Failure(nameof(GetUsersError));
        public static Error InsertUserError => Error.Failure(nameof(InsertUserError));
        public static Error InsertUserPreferencesError => Error.Failure(nameof(InsertUserPreferencesError));
        public static Error InsertUserPageError => Error.Failure(nameof(InsertUserPageError));
        public static Error InsertUserEnvironmentError => Error.Failure(nameof(InsertUserEnvironmentError));
        public static Error UpdateUserError => Error.Failure(nameof(UpdateUserError));
        public static Error UpdateUserEnvironmentError => Error.Failure(nameof(UpdateUserEnvironmentError));
        public static Error UpdateUserPreferencesError => Error.Failure(nameof(UpdateUserPreferencesError));
        public static Error UpdateUserPageError => Error.Failure(nameof(UpdateUserPageError));
        public static Error DeleteUserError => Error.Failure(nameof(DeleteUserError));
        public static Error DeleteUserPageError => Error.Failure(nameof(DeleteUserPageError));
        public static Error DatabaseSetupError => Error.Failure(nameof(DatabaseSetupError));
        public static Error ValidateAccountError => Error.Validation(nameof(ValidateAccountError));
        public static Error UninitializedDatabaseError => Error.Failure(nameof(UninitializedDatabaseError));
        #endregion
    }
}