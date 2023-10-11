#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Domain.Common.Errors;

/// <summary>
/// Domain file system related error types
/// </summary>
/// <remarks>
/// Creation Date: 28th of September, 2023
/// </remarks>
public static partial class Errors
{
    public static class FileSystem
    {
        #region ==================================================================== PROPERTIES =================================================================================
        public static Error InvalidPath => Error.Failure(nameof(InvalidPath));
        public static Error CannotNavigateUp => Error.Failure(nameof(CannotNavigateUp));
        #endregion
    }
}