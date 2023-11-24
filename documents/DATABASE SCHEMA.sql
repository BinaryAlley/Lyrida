-- needs to be placed in the SetupRepository
START TRANSACTION;

CREATE TABLE `users` (
  `id` int(9) UNSIGNED NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `totp_secret` varchar(1024) DEFAULT NULL,
  `verification_token` varchar(250) DEFAULT NULL,
  `verification_token_created` timestamp NULL DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userpreferences` (
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

CREATE TABLE `userenvironments` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `environment_id` varchar(36) NOT NULL,
  `type` varchar(256) NOT NULL,
  `platform_type` varchar(256) NOT NULL,
  `title` varchar(256) NOT NULL,
  `initial_path` varchar(1024) NOT NULL,
  `url` varchar(1024) DEFAULT NULL,
  `port` int(5) UNSIGNED DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `passive_mode` tinyint(1) UNSIGNED DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userpages` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `page_id` varchar(36) NOT NULL,
  `title` varchar(256) NOT NULL,
  `path` varchar(1024) DEFAULT NULL,
  `environment_id` varchar(36) NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

ALTER TABLE `userpreferences`
    ADD PRIMARY KEY (`id`);

ALTER TABLE `userenvironments`
    ADD PRIMARY KEY (`id`),
    ADD UNIQUE KEY `unique_environment_id` (`environment_id`);

ALTER TABLE `userpages`
    ADD PRIMARY KEY (`id`),
    ADD UNIQUE KEY `page_id` (`page_id`);

ALTER TABLE `users`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userpreferences`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userenvironments`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userpages`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

-- Foreign key constraints for the `UserPreferences` table
ALTER TABLE `userpreferences`
ADD CONSTRAINT `fk_userpreferences_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `UserEnvironments` table
ALTER TABLE `userenvironments`
ADD CONSTRAINT `fk_userenvironments_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `UserPages` table
ALTER TABLE `userpages`
ADD CONSTRAINT `fk_userpages_user_id`
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_userpages_environment_id`
    FOREIGN KEY (`environment_id`) REFERENCES `userenvironments`(`environment_id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

COMMIT;