-- needs to be placed in the SetupRepository
START TRANSACTION;
CREATE TABLE `permissions` (
  `id` int(9) UNSIGNED NOT NULL,
  `permission_name` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `roles` (
  `id` int(9) UNSIGNED NOT NULL,
  `role_name` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `users` (
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

CREATE TABLE `rolepermissions` (
  `id` int(9) UNSIGNED NOT NULL,
  `role_id` int(9) UNSIGNED NOT NULL,
  `permission_id` int(9) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userpermissions` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `permission_id` int(9) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userroles` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `role_id` int(9) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userpreferences` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `use_2fa` tinyint(1) UNSIGNED NOT NULL,
  `remember_open_tabs` tinyint(1) UNSIGNED NOT NULL,
  `show_image_previews` tinyint(1) UNSIGNED NOT NULL,
  `image_previews_quality` int(3) UNSIGNED NOT NULL,
  `full_image_quality` int(3) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userpages` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `page_id` varchar(36) NOT NULL,
  `title` varchar(256) NOT NULL,
  `path` varchar(1024) DEFAULT NULL,
  `platform_id` int(2) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

ALTER TABLE `permissions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `rolepermissions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `roles`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `userpermissions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `userroles`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

ALTER TABLE `userpreferences`
    ADD PRIMARY KEY (`id`);

ALTER TABLE `userpages`
    ADD PRIMARY KEY (`id`),
    ADD UNIQUE KEY `page_id` (`page_id`);

ALTER TABLE `permissions`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `rolepermissions`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `roles`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userpermissions`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userroles`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `users`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userpreferences`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userpages`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

-- Foreign key constraints for the `rolepermissions` table
ALTER TABLE `rolepermissions`
ADD CONSTRAINT `fk_rolepermissions_role_id`
    FOREIGN KEY (`role_id`) REFERENCES `roles`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_rolepermissions_permission_id`
    FOREIGN KEY (`permission_id`) REFERENCES `permissions`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `userpermissions` table
ALTER TABLE `userpermissions`
ADD CONSTRAINT `fk_userpermissions_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_userpermissions_permission_id`
    FOREIGN KEY (`permission_id`) REFERENCES `permissions`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `userroles` table
ALTER TABLE `userroles`
ADD CONSTRAINT `fk_userroles_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_userroles_role_id`
    FOREIGN KEY (`role_id`) REFERENCES `roles`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `userpreferences` table
ALTER TABLE `userpreferences`
ADD CONSTRAINT `fk_userpreferences_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `userpages` table
ALTER TABLE `userpages`
ADD CONSTRAINT `fk_userpages_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;