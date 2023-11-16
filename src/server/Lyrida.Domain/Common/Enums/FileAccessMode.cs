namespace Lyrida.Domain.Common.Enums;

/// <summary>
/// Enumeration for file access mode types
/// </summary>
/// <remarks>
/// Creation Date: 30th of September, 2023
/// </remarks>
public enum FileAccessMode
{
    ListDirectory, 
    ReadProperties,
    ReadContents,
    Write,
    Execute,
    Delete,
}