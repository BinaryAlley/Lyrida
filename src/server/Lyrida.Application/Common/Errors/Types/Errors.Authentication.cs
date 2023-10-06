#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Application.Common.Errors.Types;

/// <summary>
/// Authentication error types
/// </summary>
/// <remarks>
/// Creation Date: 15th of July, 2023
/// </remarks>
public static partial class Errors
{
    public static class Authentication
    {
        #region ==================================================================== PROPERTIES =================================================================================
        public static Error TokenExpired => Error.Failure(nameof(TokenExpired));
        public static Error InvalidUsername => Error.Validation(nameof(InvalidUsername));
        public static Error UnverifiedAccount => Error.Failure(nameof(UnverifiedAccount));
        public static Error TokenAlreadyIssued => Error.Failure(nameof(TokenAlreadyIssued));
        public static Error DuplicateEmailError => Error.Conflict(nameof(DuplicateEmailError));
        public static Error AccountAlreadyVerified => Error.Failure(nameof(AccountAlreadyVerified));
        #endregion
    }
}