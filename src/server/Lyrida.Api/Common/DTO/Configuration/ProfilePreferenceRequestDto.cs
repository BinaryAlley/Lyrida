namespace Lyrida.Api.Common.DTO.Configuration;

/// <summary>
/// Data transfer object for API profile preferences requests
/// </summary>
/// <remarks>
/// Creation Date: 25th of October, 2023
/// </remarks>
public record ProfilePreferenceRequestDto(bool RememberOpenTabs, bool ShowImagePreviews, bool Use2fa, int ImagePreviewsQuality, int FullImageQuality, bool InspectFileForThumbnails,
    bool EnableConsoleDebugMessages, int ScrollThumbnailRetrievalTimeout, int ThumbnailsRetrievalBatchSize);