#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
using Lyrida.DataAccess.StorageAccess;
using Lyrida.DataAccess.Common.DTO.Common;
#endregion

namespace Lyrida.DataAccess.Repositories.Setup;

/// <summary>
/// Setup repository for the initial database setup
/// </summary>
/// <remarks>
/// Creation Date: 15th of August, 2023
/// </remarks>
internal sealed class SetupRepository : ISetupRepository
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private readonly IDataAccess dataAccess;
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Overload C-tor
    /// </summary>
    /// <param name="dataAccess">Injected data access service</param>
    public SetupRepository(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess ?? throw new ArgumentException("Data access cannot be null!");
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Opens a transaction
    /// </summary>
    public void OpenTransaction()
    {
        dataAccess.OpenTransaction();
    }

    /// <summary>
    /// Closes a transaction, rolls back changes if the transaction was faulty
    /// </summary>
    public void CloseTransaction()
    {
        dataAccess.CloseTransaction();
    }

    /// <summary>
    /// Initializes the database
    /// </summary>
    /// <returns>The result of setting up the database, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
    public async Task<ApiResponse> SetDatabase()
    {
        ApiResponse response = new();
        OpenTransaction();
        response.Error = (await dataAccess.ExecuteAsync(
           // create the tables
           @"CREATE TABLE `Permissions` (
              `id` int(9) UNSIGNED NOT NULL,
              `permission_name` varchar(250) NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `Roles` (
              `id` int(9) UNSIGNED NOT NULL,
              `role_name` varchar(250) NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `Users` (
              `id` int(9) UNSIGNED NOT NULL,
              `email` varchar(255) NOT NULL,
              `password` varchar(255) NOT NULL,
              `first_name` varchar(50) NOT NULL,
              `last_name` varchar(50) NOT NULL,
              `totp_secret` varchar(1024) DEFAULT NULL,
              `verification_token` varchar(250) DEFAULT NULL,
              `verification_token_created` timestamp NULL DEFAULT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `RolePermissions` (
              `id` int(9) UNSIGNED NOT NULL,
              `role_id` int(9) UNSIGNED NOT NULL,
              `permission_id` int(9) UNSIGNED NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `UserPermissions` (
              `id` int(9) UNSIGNED NOT NULL,
              `user_id` int(9) UNSIGNED NOT NULL,
              `permission_id` int(9) UNSIGNED NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `UserRoles` (
              `id` int(9) UNSIGNED NOT NULL,
              `user_id` int(9) UNSIGNED NOT NULL,
              `role_id` int(9) UNSIGNED NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `UserPreferences` (
              `id` int(9) UNSIGNED NOT NULL,
              `user_id` int(9) UNSIGNED NOT NULL,
              `use_2fa` tinyint(1) UNSIGNED NOT NULL,
              `remember_open_tabs` tinyint(1) UNSIGNED NOT NULL,
              `show_image_previews` tinyint(1) UNSIGNED NOT NULL,
              `inspect_file_for_thumbnails` tinyint(1) UNSIGNED NOT NULL,
              `enable_console_debug_messages` tinyint(1) UNSIGNED NOT NULL,
              `image_previews_quality` int(3) UNSIGNED NOT NULL,
              `full_image_quality` int(3) UNSIGNED NOT NULL,
              `scroll_thumbnail_retrieval_timeout` int(5) UNSIGNED NOT NULL,
              `thumbnails_retrieval_batch_size` int(3) UNSIGNED NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

            CREATE TABLE `UserPages` (
              `id` int(9) UNSIGNED NOT NULL,
              `user_id` int(9) UNSIGNED NOT NULL,
              `page_id` varchar(36) NOT NULL,
              `title` varchar(256) NOT NULL,
              `path` varchar(1024) DEFAULT NULL,
              `platform_id` int(2) UNSIGNED NOT NULL,
              `created` timestamp NOT NULL DEFAULT current_timestamp(),
              `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;"))?.Error;
        // create primary keys
        response.Error = (await dataAccess.ExecuteAsync(
           @"ALTER TABLE `Permissions`
              ADD PRIMARY KEY (`id`);

            ALTER TABLE `RolePermissions`
              ADD PRIMARY KEY (`id`);

            ALTER TABLE `Roles`
              ADD PRIMARY KEY (`id`);            

            ALTER TABLE `UserPermissions`
              ADD PRIMARY KEY (`id`);

            ALTER TABLE `UserRoles`
              ADD PRIMARY KEY (`id`);

            ALTER TABLE `Users`
              ADD PRIMARY KEY (`id`),
              ADD UNIQUE KEY `email` (`email`);

            ALTER TABLE `UserPreferences`
              ADD PRIMARY KEY (`id`);

            ALTER TABLE `UserPages`
              ADD PRIMARY KEY (`id`),
              ADD UNIQUE KEY `page_id` (`page_id`);

            ALTER TABLE `Permissions`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

            ALTER TABLE `RolePermissions`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

            ALTER TABLE `Roles`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;            

            ALTER TABLE `UserPermissions`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

            ALTER TABLE `UserRoles`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

            ALTER TABLE `Users`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

            ALTER TABLE `UserPreferences`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

            ALTER TABLE `UserPages`
              MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
            "))?.Error;
        // create foreign keys
        response.Error = (await dataAccess.ExecuteAsync(
           @"ALTER TABLE `RolePermissions`
            ADD CONSTRAINT `fk_rolepermissions_role_id`
                FOREIGN KEY (`role_id`) REFERENCES `Roles`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE,
            ADD CONSTRAINT `fk_rolepermissions_permission_id`
                FOREIGN KEY (`permission_id`) REFERENCES `Permissions`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE;

            ALTER TABLE `UserPermissions`
            ADD CONSTRAINT `fk_userpermissions_user_id`
                FOREIGN KEY (`user_id`) REFERENCES `Users`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE,
            ADD CONSTRAINT `fk_userpermissions_permission_id`
                FOREIGN KEY (`permission_id`) REFERENCES `Permissions`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE;

            ALTER TABLE `UserRoles`
            ADD CONSTRAINT `fk_userroles_user_id`
                FOREIGN KEY (`user_id`) REFERENCES `Users`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE,
            ADD CONSTRAINT `fk_userroles_role_id`
                FOREIGN KEY (`role_id`) REFERENCES `Roles`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE;

            ALTER TABLE `UserPreferences`
            ADD CONSTRAINT `fk_userpreferences_user_id`
                FOREIGN KEY (`user_id`) REFERENCES `Users`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE;

            ALTER TABLE `UserPages`
            ADD CONSTRAINT `fk_userpages_user_id`
                FOREIGN KEY (`user_id`) REFERENCES `Users`(`id`)
                ON DELETE CASCADE ON UPDATE CASCADE;"))?.Error;
        CloseTransaction();
        return response;
    }
    #endregion
}