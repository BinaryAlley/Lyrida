#region ========================================================================= USING =====================================================================================
using System;
using Mono.Unix;
using System.IO;
using Mono.Unix.Native;
using System.Security.Principal;
using Lyrida.Domain.Common.Enums;
using System.Security.AccessControl;
using Lyrida.Domain.Core.FileSystem.Services.Platform;
#endregion

namespace Lyrida.Domain.Core.FileSystem.Services.Permissions;

/// <summary>
/// Service for file system permissions
/// </summary>
/// <remarks>
/// Creation Date: 03rd of November, 2021
/// </remarks>
internal class FileSystemPermissionsService : IFileSystemPermissionsService
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IPlatformContextManager platformContextManager;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="platformContextManager">Injected facade service for platform contextual services</param>
    public FileSystemPermissionsService(IPlatformContextManager platformContextManager)
    {
         this.platformContextManager = platformContextManager;
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> can be accessed.
    /// </summary>
    /// <param name="path">The path to be accessed.</param>
    /// <param name="accessMode">The mode in which to access the path.</param>
    /// <param name="isFile">Indicates whether the path represents a file or directory.</param>
    /// <returns><see langword="true"/>, if <paramref name="path"/> can be accessed, <see langword="false"/> otherwise.</returns>
    public bool CanAccessPath(string path, FileAccessMode accessMode, bool isFile = true)
    {
        PlatformType platformType = platformContextManager.GetCurrentContext().Platform;
        if (platformType == PlatformType.Unix)
            return CanAccessPathLinux(path, accessMode);
        else if (platformType == PlatformType.Windows)
            return CanAccessPathWindows(path, accessMode, isFile);
        else
            throw new PlatformNotSupportedException("Support for this platform is not compiled into this assembly.");
    }

    /// <summary>
    /// Checks if a the current user has access permissions on Linux for <paramref name="path"/>, with <paramref name="accessMode"/>.
    /// </summary>
    /// <param name="path">The path for which to check the access.</param>
    /// <param name="accessMode">The access mode in which to check that path access.</param>
    /// <returns><see langword="true"/> if the current user has rights for the specified path and acccess mode, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when trying to check access for a mode that is not supported</exception>
    private static bool CanAccessPathLinux(string path, FileAccessMode accessMode)
    {
        AccessModes modes;
        UnixFileSystemInfo fileInfo = new UnixFileInfo(path);
        switch (accessMode)
        {
            case FileAccessMode.ReadProperties:
                // we would typically need execute permissions on a directory, to list its contents or view file properties
                if (fileInfo.FileType == FileTypes.Directory)
                    return fileInfo.CanAccess(AccessModes.X_OK);
                // for files, just verifying existence might be sufficient for reading properties
                return fileInfo.Exists;
            case FileAccessMode.ReadContents:
                return fileInfo.CanAccess(AccessModes.R_OK);
            case FileAccessMode.Write:
                modes = AccessModes.W_OK;
                break;
            case FileAccessMode.Execute:
                modes = AccessModes.X_OK;
                break;
            case FileAccessMode.ListDirectory:
                modes = AccessModes.F_OK;  // check existence, not an exact match but the closest
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(accessMode), "Unknown FileAccessMode");
        }
        return new UnixDirectoryInfo(path).CanAccess(modes);
    }

    /// <summary>
    /// Checks if a the current user has access permissions on Windows for <paramref name="path"/>, with <paramref name="accessMode"/>.
    /// </summary>
    /// <param name="path">The path for which to check the access.</param>
    /// <param name="accessMode">The access mode in which to check that path access.</param>
    /// <param name="isFile">Indicates whether the path represents a file or directory.</param>
    /// <returns><see langword="true"/> if the current user has rights for the specified path and acccess mode, <see langword="false"/> otherwise.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when trying to check access for a mode that is not supported</exception>
    private static bool CanAccessPathWindows(string path, FileAccessMode accessMode, bool isFile = true)
    {
        // translate to filesystem access modes (multiple can match per case)
        FileSystemRights rights = accessMode switch
        {
            FileAccessMode.ReadProperties => FileSystemRights.ReadAttributes,
            FileAccessMode.ReadContents => FileSystemRights.ReadData | FileSystemRights.ReadExtendedAttributes,
            FileAccessMode.Write => FileSystemRights.Write,
            FileAccessMode.Execute => FileSystemRights.ExecuteFile,
            FileAccessMode.ListDirectory => FileSystemRights.ListDirectory,
            _ => throw new ArgumentOutOfRangeException(nameof(accessMode), "Unknown FileAccessMode"),
        };
        if (HasAccess(rights, path, isFile))
        {
            try
            {
                switch (accessMode)
                {
                    case FileAccessMode.ReadProperties:
                        // just accessing file info for properties, without opening it
                        FileSystemInfo fileSystemInfo = isFile ? new FileInfo(path) : new DirectoryInfo(path);
                        _ = fileSystemInfo.CreationTime;  // trigger potential access denial
                        break;
                    case FileAccessMode.ReadContents:
                        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            stream.Close();
                        break;
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the current user or group has the access <paramref name="rights"/> for <paramref name="path"/>, on Windows platforms
    /// </summary>
    /// <param name="rights">The rights for which to check access</param>
    /// <param name="path">The path for which to check the access <paramref name="rights"/></param>
    /// <param name="isFile">Indicates whether the path represents a file or directory.</param>
    /// <returns><see langword="true"/>, if the current user or group has <paramref name="rights"/> for <paramref name="path"/>, 
    /// <see langword="false"/> otherwise</returns>
    private static bool HasAccess(FileSystemRights rights, string path, bool isFile = true)
    {
        bool allowAccess = false;
        AuthorizationRuleCollection acl;
        //FileSystemInfo fileSystemInfo = isFile ? new FileInfo(path) : new DirectoryInfo(path);
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        // get the collection of authorization rules that apply to the specified directory
        try
        {
            // some paths (ex: C:\Windows\System32\config) throw "unauthorized" exceptions even when trying to check if one is authorized to access them - how dumb is that??...
            if (isFile)
            {
                FileInfo fileInfo = new(path);
                acl = fileInfo.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
            }
            else
            {
                DirectoryInfo directoryInfo = new(path);
                acl = directoryInfo.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
            }
            foreach (FileSystemAccessRule accessRule in acl)
            {
                // Check if the current rule applies to the current user or the groups they belong to
                if (identity?.User?.Equals(accessRule.IdentityReference) == true || principal.IsInRole((SecurityIdentifier)accessRule.IdentityReference))
                {
                    if (accessRule.AccessControlType.Equals(AccessControlType.Deny) && (accessRule.FileSystemRights & rights) == rights)
                        return false; // if there's a deny rule that matches the specified rights, return false immediately
                    else if (accessRule.AccessControlType.Equals(AccessControlType.Allow) && (accessRule.FileSystemRights & rights) == rights)
                        allowAccess = true;
                }
            }

        }
        catch 
        {
            return false;
        }
        return allowAccess;
    }
    #endregion
}