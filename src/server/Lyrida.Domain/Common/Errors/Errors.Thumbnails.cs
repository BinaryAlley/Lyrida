#region ========================================================================= USING =====================================================================================
using ErrorOr;
#endregion

namespace Lyrida.Domain.Common.Errors;

/// <summary>
/// Domain thumbnails related error types
/// </summary>
/// <remarks>
/// Creation Date: 30th of September, 2023
/// </remarks>
public static partial class Errors
{
    public static class Thumbnails
    {
        #region ==================================================================== PROPERTIES =================================================================================
        public static Error NoThumbnail => Error.Failure(nameof(NoThumbnail));
        #endregion
    }
}