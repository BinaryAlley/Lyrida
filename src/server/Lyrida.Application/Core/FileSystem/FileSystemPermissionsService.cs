/// Written by: Yulia Danilova, Sameh Salem
/// Creation Date: 03rd of November, 2021
/// Purpose: Service for file system permissions
#region ========================================================================= USING =====================================================================================
using System.IO;
using Mono.Unix;
using Mono.Unix.Native;
using System.Security.Principal;
using System.Security.AccessControl;
#endregion

namespace Lyrida.Application.Core.FileSystem;

public class FileSystemPermissionsService : IFileSystemPermissionsService
{
    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Checks if <paramref name="path"/> can be accessed
    /// </summary>
    /// <param name="path">The path to be accessed</param>
    /// <returns><see langword="true"/>, if <paramref name="path"/> can be accessed, <see langword="false"/> otherwise</returns>
    public bool CanAccessPath(string path)
    {
#if LINUX
        return new UnixDirectoryInfo(path).CanAccess(AccessModes.R_OK);
#elif WINDOWS
        return HasAccess(FileSystemRights.ListDirectory, path);
#elif OSX
        throw new NotImplementedException();
#endif
    }

#if WINDOWS
    /// <summary>
    /// Checks if the current user or group has the access <paramref name="rights"/> for <paramref name="path"/>, on Windows platforms
    /// </summary>
    /// <param name="rights">The rights for which to check access</param>
    /// <param name="path">The path for which to check the access <paramref name="rights"/></param>
    /// <returns><see langword="true"/>, if the current user or group has <paramref name="rights"/> for <paramref name="path"/>, 
    /// <see langword="false"/> otherwise</returns>
    private static bool HasAccess(FileSystemRights rights, string path)
    {
        bool allowAccess = false;
        DirectoryInfo directoryInfo = new(path);
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        // get the collection of authorization rules that apply to the specified directory
        AuthorizationRuleCollection acl = directoryInfo.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
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
        return allowAccess;
    }
#endif
    #endregion
}