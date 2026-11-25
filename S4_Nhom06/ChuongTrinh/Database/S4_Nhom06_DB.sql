-- Khoi tao lan dau tren MySQL 8.4. Khong chay lai tren database dang su dung.
CREATE DATABASE ooad_hotel CHARACTER SET utf8mb4;
USE ooad_hotel;
SET NAMES utf8mb4;

CREATE TABLE `Account` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `employee_id` int NOT NULL,
  `role_id` int NOT NULL,
  `status` enum('active','locked') NOT NULL,
  `last_login` datetime,
  UNIQUE (`employee_id`),
  UNIQUE (`username`)
) ENGINE=InnoDB;

CREATE TABLE `Role` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `name` varchar(50) NOT NULL,
  `description` varchar(255),
  `is_system` tinyint(1) NOT NULL,
  UNIQUE (`name`)
) ENGINE=InnoDB;

CREATE TABLE `RolePermission` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `role_id` int NOT NULL,
  `function_code` varchar(30) NOT NULL,
  `can_view` tinyint(1) NOT NULL,
  `can_add` tinyint(1) NOT NULL,
  `can_edit` tinyint(1) NOT NULL,
  `can_delete` tinyint(1) NOT NULL,
  UNIQUE (`role_id`,`function_code`)
) ENGINE=InnoDB;

CREATE TABLE `Department` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `name` varchar(50) NOT NULL,
  `description` varchar(255),
  UNIQUE (`name`)
) ENGINE=InnoDB;

CREATE TABLE `Position` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `name` varchar(50) NOT NULL,
  `description` varchar(255),
  UNIQUE (`name`)
) ENGINE=InnoDB;

CREATE TABLE `Employee` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `full_name` varchar(50) NOT NULL,
  `gender` enum('male','female'),
  `date_of_birth` date,
  `phone_number` char(10),
  `email` varchar(50),
  `address` varchar(100),
  `identity_card` varchar(12) NOT NULL,
  `department_id` int NOT NULL,
  `position_id` int NOT NULL,
  `hire_date` date,
  `base_salary` decimal(12,0),
  CHECK (base_salary >= 0),
  `bank_account` varchar(30),
  `status` enum('working','off') NOT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  UNIQUE (`identity_card`)
) ENGINE=InnoDB;

CREATE TABLE `WorkShift` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `employee_id` int NOT NULL,
  `shift_date` date NOT NULL,
  `shift_type` enum('morning','afternoon','night') NOT NULL,
  `note` varchar(255),
  UNIQUE (`employee_id`,`shift_date`,`shift_type`)
) ENGINE=InnoDB;

CREATE TABLE `RoomType` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `name` varchar(50) NOT NULL,
  `description` varchar(255),
  `capacity` int NOT NULL,
  CHECK (capacity > 0),
  `bed_type` varchar(30),
  `area` decimal(5,1),
  CHECK (area > 0),
  `base_price` decimal(12,0) NOT NULL,
  CHECK (base_price > 0),
  UNIQUE (`name`)
) ENGINE=InnoDB;

CREATE TABLE `Room` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `room_number` varchar(10) NOT NULL,
  `floor` int NOT NULL,
  CHECK (floor >= 0),
  `room_type_id` int NOT NULL,
  `status` enum('available','reserved','occupied','cleaning','maintenance') NOT NULL,
  `description` varchar(255),
  `is_deleted` tinyint(1) NOT NULL,
  UNIQUE (`room_number`)
) ENGINE=InnoDB;

CREATE TABLE `Guest` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `full_name` varchar(50) NOT NULL,
  `gender` enum('male','female'),
  `date_of_birth` date,
  `phone_number` varchar(15),
  `email` varchar(50),
  `address` varchar(100),
  `identity_card` varchar(20),
  `nationality` varchar(30),
  `guest_type` enum('normal','vip') NOT NULL,
  `created_at` datetime NOT NULL,
  UNIQUE (`identity_card`)
) ENGINE=InnoDB;

CREATE TABLE `Booking` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `guest_id` int NOT NULL,
  `employee_id` int,
  `booking_date` datetime NOT NULL,
  `check_in_date` date NOT NULL,
  `check_out_date` date NOT NULL,
  CHECK (check_out_date > check_in_date),
  `num_adults` int NOT NULL,
  CHECK (num_adults >= 1),
  `num_children` int,
  CHECK (num_children >= 0),
  `source` enum('walk_in','phone','online') NOT NULL,
  `deposit` decimal(12,0),
  CHECK (deposit >= 0),
  `status` enum('pending','confirmed','checked_in','checked_out','cancelled') NOT NULL,
  `cancel_reason` varchar(255),
  `cancel_fee` decimal(12,0),
  CHECK (cancel_fee >= 0),
  `note` varchar(255)
) ENGINE=InnoDB;

CREATE TABLE `BookingDetail` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `booking_id` int NOT NULL,
  `room_id` int NOT NULL,
  `price_per_night` decimal(12,0) NOT NULL,
  CHECK (price_per_night > 0),
  `actual_check_in` datetime,
  `actual_check_out` datetime,
  CHECK (actual_check_out > actual_check_in)
) ENGINE=InnoDB;

CREATE TABLE `Service` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `name` varchar(50) NOT NULL,
  `category` varchar(30),
  `unit` varchar(20),
  `price` decimal(12,0) NOT NULL,
  CHECK (price >= 0),
  `description` varchar(255),
  `status` enum('active','inactive') NOT NULL,
  UNIQUE (`name`)
) ENGINE=InnoDB;

CREATE TABLE `ServiceUsage` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `booking_detail_id` int NOT NULL,
  `service_id` int NOT NULL,
  `employee_id` int NOT NULL,
  `quantity` int NOT NULL,
  CHECK (quantity > 0),
  `unit_price` decimal(12,0) NOT NULL,
  CHECK (unit_price >= 0),
  `used_at` datetime NOT NULL
) ENGINE=InnoDB;

CREATE TABLE `Promotion` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `code` varchar(20) NOT NULL,
  `name` varchar(100) NOT NULL,
  `discount_type` enum('percent','amount') NOT NULL,
  `discount_value` decimal(12,2) NOT NULL,
  CHECK (discount_value > 0),
  `min_amount` decimal(12,0),
  CHECK (min_amount >= 0),
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  CHECK (end_date >= start_date),
  `status` enum('active','inactive') NOT NULL,
  UNIQUE (`code`)
) ENGINE=InnoDB;

CREATE TABLE `Invoice` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `booking_id` int NOT NULL,
  `employee_id` int NOT NULL,
  `promotion_id` int,
  `created_at` datetime NOT NULL,
  `room_charge` decimal(12,0) NOT NULL,
  CHECK (room_charge >= 0),
  `service_charge` decimal(12,0) NOT NULL,
  CHECK (service_charge >= 0),
  `surcharge` decimal(12,0),
  CHECK (surcharge >= 0),
  `discount_amount` decimal(12,0),
  CHECK (discount_amount >= 0),
  `tax_amount` decimal(12,0) NOT NULL,
  CHECK (tax_amount >= 0),
  `total_amount` decimal(12,0) NOT NULL,
  CHECK (total_amount >= 0),
  `status` enum('unpaid','partial','paid','cancelled') NOT NULL,
  UNIQUE (`booking_id`)
) ENGINE=InnoDB;

CREATE TABLE `Payment` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `invoice_id` int NOT NULL,
  `employee_id` int NOT NULL,
  `amount` decimal(12,0) NOT NULL,
  CHECK (amount > 0),
  `method` enum('cash','card','bank_transfer','e_wallet') NOT NULL,
  `paid_at` datetime NOT NULL,
  `transaction_code` varchar(50)
) ENGINE=InnoDB;

CREATE TABLE `Report` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `name` varchar(100) NOT NULL,
  `report_type` enum('revenue','occupancy','booking') NOT NULL,
  `employee_id` int NOT NULL,
  `created_at` datetime NOT NULL,
  `from_date` date NOT NULL,
  `to_date` date NOT NULL,
  CHECK (to_date >= from_date),
  `content` text
) ENGINE=InnoDB;

CREATE TABLE `Setting` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `setting_key` varchar(50) NOT NULL,
  `setting_value` varchar(255) NOT NULL,
  `description` varchar(255),
  UNIQUE (`setting_key`)
) ENGINE=InnoDB;

CREATE TABLE `BackupFile` (
  `id` int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  `file_name` varchar(100) NOT NULL,
  `created_at` datetime NOT NULL,
  `file_size` bigint,
  CHECK (file_size >= 0),
  `employee_id` int NOT NULL
) ENGINE=InnoDB;

ALTER TABLE `Account` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Account` ADD FOREIGN KEY (`role_id`) REFERENCES `Role`(`id`) ON DELETE RESTRICT;

ALTER TABLE `RolePermission` ADD FOREIGN KEY (`role_id`) REFERENCES `Role`(`id`) ON DELETE CASCADE;

ALTER TABLE `Employee` ADD FOREIGN KEY (`department_id`) REFERENCES `Department`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Employee` ADD FOREIGN KEY (`position_id`) REFERENCES `Position`(`id`) ON DELETE RESTRICT;

ALTER TABLE `WorkShift` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE CASCADE;

ALTER TABLE `Room` ADD FOREIGN KEY (`room_type_id`) REFERENCES `RoomType`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Booking` ADD FOREIGN KEY (`guest_id`) REFERENCES `Guest`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Booking` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE SET NULL;

ALTER TABLE `BookingDetail` ADD FOREIGN KEY (`booking_id`) REFERENCES `Booking`(`id`) ON DELETE CASCADE;

ALTER TABLE `BookingDetail` ADD FOREIGN KEY (`room_id`) REFERENCES `Room`(`id`) ON DELETE RESTRICT;

ALTER TABLE `ServiceUsage` ADD FOREIGN KEY (`booking_detail_id`) REFERENCES `BookingDetail`(`id`) ON DELETE CASCADE;

ALTER TABLE `ServiceUsage` ADD FOREIGN KEY (`service_id`) REFERENCES `Service`(`id`) ON DELETE RESTRICT;

ALTER TABLE `ServiceUsage` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Invoice` ADD FOREIGN KEY (`booking_id`) REFERENCES `Booking`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Invoice` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Invoice` ADD FOREIGN KEY (`promotion_id`) REFERENCES `Promotion`(`id`) ON DELETE SET NULL;

ALTER TABLE `Payment` ADD FOREIGN KEY (`invoice_id`) REFERENCES `Invoice`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Payment` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE RESTRICT;

ALTER TABLE `Report` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE RESTRICT;

ALTER TABLE `BackupFile` ADD FOREIGN KEY (`employee_id`) REFERENCES `Employee`(`id`) ON DELETE RESTRICT;
START TRANSACTION;
INSERT INTO Role(id,name,description,is_system) VALUES
(1,'Quản lý','Quản lý vận hành khách sạn',0),(2,'Lễ tân','Nhân viên tiếp nhận khách',0);
INSERT INTO Department(id,name) VALUES(1,'Lễ tân');
INSERT INTO `Position`(id,name) VALUES(1,'Nhân viên');
INSERT INTO Employee(id,full_name,identity_card,department_id,position_id,status,is_deleted)
VALUES(1,'Nhân viên demo 1','000000000001',1,1,'working',0),
(2,'Nhân viên demo 2','000000000002',1,1,'working',0);
INSERT INTO RolePermission(role_id,function_code,can_view,can_add,can_edit,can_delete) VALUES
(1,'SERVICE',1,1,1,1),(1,'SERVICE_USAGE',1,1,0,0),(1,'BOOKING',1,1,0,0),
(2,'SERVICE',1,0,0,0),(2,'SERVICE_USAGE',1,1,0,0),(2,'BOOKING',1,1,0,0);
INSERT INTO RoomType(id,name,capacity,base_price) VALUES(1,'Tiêu chuẩn',2,500000),(2,'Gia đình',4,750000);
INSERT INTO Room(id,room_number,floor,room_type_id,status,is_deleted) VALUES
(1,'101',1,1,'available',0),(2,'102',1,1,'available',0),(3,'201',2,2,'available',0),
(4,'202',2,2,'occupied',0),(5,'203',2,2,'maintenance',0);
INSERT INTO Guest(id,full_name,identity_card,guest_type,created_at)
VALUES(1,'Nguyễn An','DEMO001','normal',NOW()),(2,'Trần Bình','DEMO002','normal',NOW());
INSERT INTO Service(id,name,category,unit,price,status) VALUES
(1,'Giặt ủi','Giặt ủi','kg',35000,'active'),(2,'Nước suối','Đồ uống','chai',15000,'active');
INSERT INTO Setting(setting_key,setting_value,description)
VALUES('min_deposit','100000','Tiền cọc tối thiểu (đ)');
INSERT INTO Booking(id,guest_id,employee_id,booking_date,check_in_date,check_out_date,num_adults,num_children,source,deposit,status)
VALUES(1,1,2,NOW(),CURDATE(),DATE_ADD(CURDATE(),INTERVAL 2 DAY),1,0,'walk_in',0,'checked_in');
INSERT INTO BookingDetail(booking_id,room_id,price_per_night,actual_check_in)
VALUES(1,4,750000,NOW());
INSERT INTO Account(username,password,employee_id,role_id,status) VALUES('quanly','PBKDF2-SHA256$210000$232796ab44ab5ed164dad7ac72c069e9$1fc4b88ca5c93b329a50f87b20bdb19aa297d21f30e53f3c0e84c591c3075d72',1,1,'active');
INSERT INTO Account(username,password,employee_id,role_id,status) VALUES('letan','PBKDF2-SHA256$210000$c85bff5cf4b18420bfc80ae89be3f6d1$3263fc9c70e910d1cce04f85bc0a3bb327e40f3015e853ae95709d5e5baa8722',2,2,'active');
COMMIT;

-- BEGIN THEM PHONG LUU TRU MAU
-- Du lieu mau bo sung cho thu nghiem ghi nhan dich vu.
-- Co the chay rieng tren database dang dung; khong tao trung khi chay lai.
USE ooad_hotel;
SET NAMES utf8mb4;
START TRANSACTION;


-- Phong 103: da nhan phong, chua tra phong.
INSERT INTO Room(room_number,floor,room_type_id,status,is_deleted,description)
SELECT '103',1,1,'available',0,'Phòng mẫu thử ghi nhận dịch vụ'
WHERE NOT EXISTS(SELECT 1 FROM Room WHERE room_number='103');
SELECT id FROM Room WHERE room_number='103' FOR UPDATE;

INSERT INTO Guest(full_name,identity_card,guest_type,created_at)
SELECT 'Khách mẫu phòng 103','DEMO003','normal',NOW()
WHERE NOT EXISTS(SELECT 1 FROM Guest WHERE identity_card='DEMO003');

INSERT INTO Booking(guest_id,employee_id,booking_date,check_in_date,check_out_date,
    num_adults,num_children,source,deposit,status,note)
SELECT g.id,a.employee_id,NOW(),CURDATE(),DATE_ADD(CURDATE(),INTERVAL 3 DAY),
    1,0,'walk_in',100000,'checked_in','MauLuuTru_103'
FROM Guest g JOIN Account a ON a.username='letan'
WHERE g.identity_card='DEMO003'
  AND NOT EXISTS(SELECT 1 FROM Booking WHERE note='MauLuuTru_103')
  AND NOT EXISTS(
    SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
    JOIN Room r ON r.id=d.room_id
    WHERE r.room_number='103' AND b.status!='cancelled'
      AND b.check_in_date<DATE_ADD(CURDATE(),INTERVAL 3 DAY)
      AND CURDATE()<b.check_out_date);

INSERT INTO BookingDetail(booking_id,room_id,price_per_night,actual_check_in)
SELECT b.id,r.id,t.base_price,NOW()
FROM Booking b JOIN Room r ON r.room_number='103'
JOIN RoomType t ON t.id=r.room_type_id
WHERE b.note='MauLuuTru_103' AND b.status='checked_in'
  AND NOT EXISTS(SELECT 1 FROM BookingDetail WHERE booking_id=b.id);

UPDATE Room r SET r.status='occupied'
WHERE r.room_number='103' AND EXISTS(
    SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
    WHERE d.room_id=r.id AND b.note='MauLuuTru_103' AND b.status='checked_in'
      AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL);


-- Phong 104: da nhan phong, chua tra phong.
INSERT INTO Room(room_number,floor,room_type_id,status,is_deleted,description)
SELECT '104',1,1,'available',0,'Phòng mẫu thử ghi nhận dịch vụ'
WHERE NOT EXISTS(SELECT 1 FROM Room WHERE room_number='104');
SELECT id FROM Room WHERE room_number='104' FOR UPDATE;

INSERT INTO Guest(full_name,identity_card,guest_type,created_at)
SELECT 'Khách mẫu phòng 104','DEMO004','normal',NOW()
WHERE NOT EXISTS(SELECT 1 FROM Guest WHERE identity_card='DEMO004');

INSERT INTO Booking(guest_id,employee_id,booking_date,check_in_date,check_out_date,
    num_adults,num_children,source,deposit,status,note)
SELECT g.id,a.employee_id,NOW(),CURDATE(),DATE_ADD(CURDATE(),INTERVAL 3 DAY),
    2,0,'walk_in',100000,'checked_in','MauLuuTru_104'
FROM Guest g JOIN Account a ON a.username='letan'
WHERE g.identity_card='DEMO004'
  AND NOT EXISTS(SELECT 1 FROM Booking WHERE note='MauLuuTru_104')
  AND NOT EXISTS(
    SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
    JOIN Room r ON r.id=d.room_id
    WHERE r.room_number='104' AND b.status!='cancelled'
      AND b.check_in_date<DATE_ADD(CURDATE(),INTERVAL 3 DAY)
      AND CURDATE()<b.check_out_date);

INSERT INTO BookingDetail(booking_id,room_id,price_per_night,actual_check_in)
SELECT b.id,r.id,t.base_price,NOW()
FROM Booking b JOIN Room r ON r.room_number='104'
JOIN RoomType t ON t.id=r.room_type_id
WHERE b.note='MauLuuTru_104' AND b.status='checked_in'
  AND NOT EXISTS(SELECT 1 FROM BookingDetail WHERE booking_id=b.id);

UPDATE Room r SET r.status='occupied'
WHERE r.room_number='104' AND EXISTS(
    SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
    WHERE d.room_id=r.id AND b.note='MauLuuTru_104' AND b.status='checked_in'
      AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL);


-- Phong 204: da nhan phong, chua tra phong.
INSERT INTO Room(room_number,floor,room_type_id,status,is_deleted,description)
SELECT '204',2,2,'available',0,'Phòng mẫu thử ghi nhận dịch vụ'
WHERE NOT EXISTS(SELECT 1 FROM Room WHERE room_number='204');
SELECT id FROM Room WHERE room_number='204' FOR UPDATE;

INSERT INTO Guest(full_name,identity_card,guest_type,created_at)
SELECT 'Khách mẫu phòng 204','DEMO005','normal',NOW()
WHERE NOT EXISTS(SELECT 1 FROM Guest WHERE identity_card='DEMO005');

INSERT INTO Booking(guest_id,employee_id,booking_date,check_in_date,check_out_date,
    num_adults,num_children,source,deposit,status,note)
SELECT g.id,a.employee_id,NOW(),CURDATE(),DATE_ADD(CURDATE(),INTERVAL 3 DAY),
    3,0,'walk_in',100000,'checked_in','MauLuuTru_204'
FROM Guest g JOIN Account a ON a.username='letan'
WHERE g.identity_card='DEMO005'
  AND NOT EXISTS(SELECT 1 FROM Booking WHERE note='MauLuuTru_204')
  AND NOT EXISTS(
    SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
    JOIN Room r ON r.id=d.room_id
    WHERE r.room_number='204' AND b.status!='cancelled'
      AND b.check_in_date<DATE_ADD(CURDATE(),INTERVAL 3 DAY)
      AND CURDATE()<b.check_out_date);

INSERT INTO BookingDetail(booking_id,room_id,price_per_night,actual_check_in)
SELECT b.id,r.id,t.base_price,NOW()
FROM Booking b JOIN Room r ON r.room_number='204'
JOIN RoomType t ON t.id=r.room_type_id
WHERE b.note='MauLuuTru_204' AND b.status='checked_in'
  AND NOT EXISTS(SELECT 1 FROM BookingDetail WHERE booking_id=b.id);

UPDATE Room r SET r.status='occupied'
WHERE r.room_number='204' AND EXISTS(
    SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
    WHERE d.room_id=r.id AND b.note='MauLuuTru_204' AND b.status='checked_in'
      AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL);

COMMIT;
