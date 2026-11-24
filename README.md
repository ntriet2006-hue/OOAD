# Quản lý khách sạn · S4_Nhom06

Bản web dùng ASP.NET Core C# (.NET 10) và MySQL 8.4. Chạy ChayDemo.cmd, đợi dòng Now listening rồi mở http://127.0.0.1:5086. Giữ cửa sổ máy chủ mở khi dùng. Đăng nhập quanly hoặc letan với mật khẩu Demo123!.

## Cấu trúc

- SourceCode/KhachSan.csproj là project C#.
- SourceCode/wwwroot chứa giao diện, sự kiện trình duyệt và định dạng.
- SourceCode/KhoiDong.cs tiếp nhận HTTP, kiểm tra phiên và gọi Control.
- SourceCode/DatPhong.cs chứa DieuKhienDatPhong và phần tạo Booking.
- SourceCode/PhongVaKhach.cs chứa Room, Guest và kiểm tra dữ liệu nhập.
- SourceCode/DichVu.cs chứa DieuKhienDichVu, Service, ServiceUsage và phần Booking phục vụ ghi nhận dịch vụ.
- Database/S4_Nhom06_DB.sql tạo 20 bảng theo mục 1.6.4, 21 khóa ngoại và dữ liệu mẫu trên máy mới chưa có ooad_hotel.

## Đối chiếu hai phần demo

| Use case và SD | Luồng code | Method CD được gọi |
|---|---|---|
| UC4.1 và UC4.2 | Giao diện tìm phòng, chọn phòng và khách, nhập cọc. Control mở giao dịch, khóa phòng theo thứ tự ID, kiểm tra lại lịch và mức cọc, tạo khách mới nếu cần, gọi Booking tạo dữ liệu và cập nhật Room. | Booking.taoDatPhong(): bool, Room.kiemTraPhongTrong(DateTime, DateTime): bool, Room.capNhatTrangThai(string): bool, Guest.kiemTraTrungGiayTo(string): bool |
| UC6.1 và Hình 3.1.42 | Nhập biểu mẫu, Control kiểm tra dữ liệu và quyền, Service kiểm tra trùng tên rồi thêm, trả thông báo và làm mới danh sách. | Them là thao tác hỗ trợ lưu dữ liệu của thông điệp tạo trong SD. |
| UC6.2 và Hình 3.1.43 | Bấm Sửa tải chi tiết từ MySQL qua Control, nhập thay đổi, kiểm tra dữ liệu rồi cập nhật. | Service.capNhatThongTin(): void |
| UC6.3 và Hình 3.1.44 | Xác nhận xóa, Control kiểm tra quyền, Service kiểm tra ServiceUsage, chỉ xóa khi chưa có lịch sử. | Xoa là thao tác hỗ trợ thực hiện thông điệp xóa trong SD. |
| UC6.4 và Hình 3.1.45 | Chọn phòng đang ở và dịch vụ active, kiểm tra lại lưu trú, kiểm tra cung cấp, tính thành tiền, ghi nhận vào BookingDetail, tính tổng của Booking. | Service.kiemTraConCungCap(): bool; ServiceUsage.tinhThanhTien(): double; ServiceUsage.ghiNhanSuDung(int): bool |

CD dùng boolean được hiện thực bằng bool, Date bằng DateTime. Các thuộc tính tiền của thực thể dùng double theo CD, dữ liệu SQL lưu DECIMAL theo RDM. API nhận decimal để kiểm tra giới hạn DECIMAL(12,0) và không chấp nhận phần lẻ đồng. SD gộp bước tính tiền ở lifeline Dịch vụ, CD giao method cho ServiceUsage, nên code gọi ServiceUsage và giữ thứ tự tính trước khi lưu của SD.

Bản demo triển khai các method cần cho hai chức năng đã chọn. Các nghiệp vụ sửa/hủy đặt phòng, nhận/trả phòng và các phân hệ còn lại chưa được triển khai giao diện. Các thao tác Doc, Them, Xoa, DanhSach là hỗ trợ truy vấn và lưu cho thông điệp SD.

## Cài trên máy khác

Cài .NET 10 SDK, MySQL 8.4 và nhập Database/S4_Nhom06_DB.sql bằng Workbench. Cấp tài khoản chạy ứng dụng quyền SELECT, INSERT, UPDATE, DELETE trên ooad_hotel.*. Chạy SourceCode/ChayDemo.cmd, nhập tài khoản MySQL khi được hỏi. Máy mới mặc định dùng 127.0.0.1:3306. Có thể đặt OOAD_HOST, OOAD_PORT, OOAD_USER, OOAD_PASSWORD và OOAD_DATABASE bằng biến môi trường. Tài khoản MySQL khác với tài khoản đăng nhập ứng dụng.

Máy hiện tại dùng instance MySQL tại cổng 33316. Mật khẩu ứng dụng được lưu theo định dạng PBKDF2-SHA256 trong Account.password. Thời điểm đăng nhập được ghi vào last_login.

## Chuyển dữ liệu và kiểm tra ngày 24 tháng 11 năm 2026

Database đang chạy đã được đồng bộ cấu trúc với file SQL. Giữ nguyên 3 khách, 5 đặt phòng, 5 chi tiết đặt phòng, 2 dịch vụ và 6 lượt sử dụng dịch vụ hiện có. Giới tính nam được chuẩn hóa thành male, ngày sinh dạng dd-MM-yyyy thành DATE. Những trường chưa có dữ liệu ở bản cũ được bổ sung theo quy tắc demo: tên và giấy tờ nhân viên mẫu, phòng ban và chức vụ mẫu, tầng lấy từ số đầu số phòng, created_at của khách dùng thời điểm chuyển đổi. Mã bản ghi và dữ liệu nghiệp vụ cũ được giữ lại.

Sao lưu SQL trước chuyển đổi ở C:/OOAD/SaoLuuTruocDongBo-20261124/ooad_truoc_dong_bo.sql. Các bảng cũ được giữ trong schema ooad_truoc_dongbo_20261124. Không đưa các bản sao lưu này vào gói nộp bài.

Đã kiểm tra SQL nhập mới và cấu trúc live, các nhánh thành công/lỗi, quyền Quản lý/Lễ tân, đơn giá lớn hơn INT32, giữ giá lịch sử, rollback, CASCADE và hai phiên đồng thời đặt cùng phòng. Dữ liệu kiểm thử chạy ở database riêng.

Bản cuối đã qua 60 kiểm tra HTTP/MySQL, build 0 lỗi và 0 cảnh báo. Đã xác nhận 2 phiên cùng đặt một phòng cho kết quả 1 thành công, 1 lỗi trùng và chỉ tăng 1 đặt phòng.
