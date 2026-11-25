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
