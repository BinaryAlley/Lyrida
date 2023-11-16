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
        public static Error FileCopyError => Error.Failure(nameof(FileCopyError));
        public static Error FileMoveError => Error.Failure(nameof(FileMoveError));
        public static Error InvalidPathError => Error.Failure(nameof(InvalidPathError));
        public static Error FileNotFoundError => Error.Failure(nameof(FileNotFoundError));
        public static Error DirectoryCopyError => Error.Failure(nameof(DirectoryCopyError));
        public static Error DirectoryMoveError => Error.Failure(nameof(DirectoryMoveError));
        public static Error CannotNavigateUpError => Error.Failure(nameof(CannotNavigateUpError));
        public static Error NameCannotBeEmptyError => Error.Failure(nameof(NameCannotBeEmptyError));
        public static Error FileAlreadyExistsError => Error.Failure(nameof(FileAlreadyExistsError));
        public static Error DirectoryNotFoundError => Error.Failure(nameof(DirectoryNotFoundError));
        public static Error DirectoryAlreadyExistsError => Error.Failure(nameof(DirectoryAlreadyExistsError));
        #endregion
    }
}