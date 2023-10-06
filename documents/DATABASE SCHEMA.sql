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

CREATE TABLE `statuses` (
  `id` int(9) UNSIGNED NOT NULL,
  `name` varchar(200) NOT NULL,
  `description` varchar(1000) DEFAULT NULL,
  `color` varchar(7) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `equipments` (
  `id` int(9) UNSIGNED NOT NULL,
  `name` varchar(200) NOT NULL,
  `description` varchar(1000) DEFAULT NULL,
  `number_of_clients` int(9) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `areas` (
  `id` int(9) UNSIGNED NOT NULL,
  `name` varchar(200) NOT NULL,
  `description` varchar(1000) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `cities` (
  `id` int(9) UNSIGNED NOT NULL,
  `name` varchar(200) NOT NULL,
  `description` varchar(1000) DEFAULT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `workloads` (
  `id` int(9) UNSIGNED NOT NULL,
  `name` varchar(200) NOT NULL,
  `description` varchar(1000) DEFAULT NULL,
  `customer_name` varchar(200) DEFAULT NULL,
  `customer_title` varchar(200) DEFAULT NULL,
  `customer_phone` varchar(200) DEFAULT NULL,
  `customer_email` varchar(200) DEFAULT NULL,
  `is_completed` tinyint(1) NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `assignments` (
  `id` int(9) UNSIGNED NOT NULL,
  `edited_by_user_id` int(9) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `users` (
  `id` int(9) UNSIGNED NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `last_name` varchar(50) NOT NULL,
  `verification_token` varchar(250) DEFAULT NULL,
  `verification_token_created` timestamp NULL DEFAULT NULL,
  `is_verified` tinyint(1) NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `rolepermissions` (
  `id` int(9) UNSIGNED NOT NULL,
  `role_id` int(9) UNSIGNED NOT NULL,
  `permission_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userpermissions` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `permission_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `userroles` (
  `id` int(9) UNSIGNED NOT NULL,
  `user_id` int(9) UNSIGNED NOT NULL,
  `role_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `assignmenthistory` (
  `id` int(9) UNSIGNED NOT NULL,
  `assignment_id` int(9) UNSIGNED NOT NULL,
  `workload_id` int(9) UNSIGNED NOT NULL,
  `equipment_id` int(9) UNSIGNED NOT NULL,
  `engineer_id` int(9) UNSIGNED NOT NULL,
  `status_id` int(9) UNSIGNED NOT NULL,
  `edited_by_user_id` int(9) UNSIGNED NOT NULL,
  `created` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `cityareas` (
  `id` int(9) UNSIGNED NOT NULL,
  `city_id` int(9) UNSIGNED NOT NULL,
  `area_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `areaequipments` (
  `id` int(9) UNSIGNED NOT NULL,
  `area_id` int(9) UNSIGNED NOT NULL,
  `equipment_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `workloadequipments` (
  `id` int(9) UNSIGNED NOT NULL,
  `workload_id` int(9) UNSIGNED NOT NULL,
  `equipment_id` int(9) UNSIGNED NOT NULL,
  `status_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `assignmentengineers` (
  `id` int(9) UNSIGNED NOT NULL,
  `assignment_id` int(9) UNSIGNED NOT NULL,
  `engineer_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `assignmentworkloads` (
  `id` int(9) UNSIGNED NOT NULL,
  `assignment_id` int(9) UNSIGNED NOT NULL,
  `workloadequipment_id` int(9) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

ALTER TABLE `permissions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `rolepermissions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `roles`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `statuses`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `equipments`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `areas`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `cities`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `workloads`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `assignments`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `assignmenthistory`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `cityareas`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `areaequipments`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `workloadequipments`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `assignmentengineers`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `assignmentworkloads`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `userpermissions`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `userroles`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

ALTER TABLE `permissions`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `rolepermissions`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `roles`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `statuses`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
  
ALTER TABLE `equipments`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
  
ALTER TABLE `areas`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
  
ALTER TABLE `cities`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
  
ALTER TABLE `workloads`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
  
ALTER TABLE `assignments`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
    
ALTER TABLE `assignmenthistory`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
    
ALTER TABLE `cityareas`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
    
ALTER TABLE `areaequipments`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
   
ALTER TABLE `workloadequipments`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
   
ALTER TABLE `assignmentengineers`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;
   
ALTER TABLE `assignmentworkloads`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userpermissions`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `userroles`
  MODIFY `id` int(9) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `users`
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
  
-- Foreign key constraints for the `assignmenthistory` table
ALTER TABLE `assignmenthistory`
ADD CONSTRAINT `fk_assignmenthistory_assignment_id`
    FOREIGN KEY (`assignment_id`) REFERENCES `assignments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmenthistory_workload_id`
    FOREIGN KEY (`workload_id`) REFERENCES `workloads`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmenthistory_equipment_id`
    FOREIGN KEY (`equipment_id`) REFERENCES `equipments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmenthistory_engineer_id`
    FOREIGN KEY (`engineer_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmenthistory_status_id`
    FOREIGN KEY (`status_id`) REFERENCES `statuses`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmenthistory_edited_by_user_id`
    FOREIGN KEY (`edited_by_user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;
    
-- Foreign key constraints for the `cityareas` table
ALTER TABLE `cityareas`
ADD CONSTRAINT `fk_cityareas_city_id`
    FOREIGN KEY (`city_id`) REFERENCES `cities`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_cityareas_area_id`
    FOREIGN KEY (`area_id`) REFERENCES `areas`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `areaequipments` table
ALTER TABLE `areaequipments`
ADD CONSTRAINT `fk_areaequipments_area_id`
    FOREIGN KEY (`area_id`) REFERENCES `areas`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_areaequipments_equipment_id`
    FOREIGN KEY (`equipment_id`) REFERENCES `equipments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `workloadequipments` table
ALTER TABLE `workloadequipments`
ADD CONSTRAINT `fk_workloadequipments_workload_id`
    FOREIGN KEY (`workload_id`) REFERENCES `workloads`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_workloadequipments_equipment_id`
    FOREIGN KEY (`equipment_id`) REFERENCES `equipments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_workloadequipments_status_id`
    FOREIGN KEY (`status_id`) REFERENCES `statuses`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `assignmentengineers` table
ALTER TABLE `assignmentengineers`
ADD CONSTRAINT `fk_assignmentengineers_assignment_id`
    FOREIGN KEY (`assignment_id`) REFERENCES `assignments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmentengineers_engineer_id`
    FOREIGN KEY (`engineer_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `assignmentworkloads` table
ALTER TABLE `assignmentworkloads`
ADD CONSTRAINT `fk_assignmentworkloads_assignment_id`
    FOREIGN KEY (`assignment_id`) REFERENCES `assignments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT `fk_assignmentworkloads_workloadequipment_id`
    FOREIGN KEY (`workloadequipment_id`) REFERENCES `workloadequipments`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Foreign key constraints for the `assignments` table
ALTER TABLE `assignments`
ADD CONSTRAINT `fk_assignments_edited_by_user_id`
    FOREIGN KEY (`edited_by_user_id`) REFERENCES `users`(`id`)
    ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;