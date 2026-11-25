# Quản lý khách sạn · S4_Nhom06

Web ASP.NET Core C# (.NET 10), MySQL 8.4. Hai phân hệ chạy nghiệp vụ thật: Quản lý đặt phòng và Quản lý dịch vụ. Các phân hệ còn lại là giao diện minh họa và lớp khung theo báo cáo.

## Chạy chương trình

1. Cài .NET 10 SDK và MySQL 8.4.
2. Trên máy mới, nhập `S4_Nhom06/ChuongTrinh/Database/S4_Nhom06_DB.sql` bằng MySQL Workbench. File này tạo database mới, không chạy lại trên dữ liệu đang sử dụng.
3. Cấp tài khoản MySQL quyền SELECT, INSERT, UPDATE, DELETE trên `ooad_hotel.*`.
4. Chạy `ChayDemo.cmd`, giữ cửa sổ máy chủ mở và truy cập http://127.0.0.1:5086/.
5. Đăng nhập `quanly` hoặc `letan`, mật khẩu demo `Demo123!`.

Máy mới sẽ hỏi tài khoản MySQL và mặc định cổng 3306. Máy phát triển hiện tại dùng instance riêng tại cổng 33316, thông tin kết nối cục bộ nằm trong `mysql-local/` và không đưa lên GitHub. Có thể cấu hình `OOAD_HOST`, `OOAD_PORT`, `OOAD_USER`, `OOAD_PASSWORD`, `OOAD_DATABASE` bằng biến môi trường. Tài khoản MySQL khác tài khoản đăng nhập ứng dụng.

Cổng khách hàng: http://127.0.0.1:5086/datphong.html, không cần đăng nhập nhân viên.

## Cấu trúc mã nguồn

Các đường dẫn dưới đây tính từ `S4_Nhom06/ChuongTrinh/`:

| File / thư mục | Trách nhiệm |
|---|---|
| SourceCode/KhachSan.csproj | Project ASP.NET Core |
| SourceCode/KhoiDong.cs | API, phiên đăng nhập, CSRF và gọi Control |
| SourceCode/DatPhong.cs | DieuKhienDatPhong và Booking tạo đặt phòng |
| SourceCode/KhungChucNang/DieuKhien/DatPhongBoSung.cs | Control sửa, hủy, tìm kiếm và đặt trực tuyến |
| SourceCode/KhungChucNang/ThucThe/DatPhongBoSung.cs | Booking sửa, hủy, tìm kiếm và email xác nhận |
| SourceCode/PhongVaKhach.cs | Room, Guest và kiểm tra dữ liệu |
| SourceCode/DichVu.cs | DieuKhienDichVu, Service, ServiceUsage và Booking ghi nhận dịch vụ |
| SourceCode/CoSoDuLieu.cs | Kết nối MySQL, truy vấn tham số hóa và transaction |
| SourceCode/wwwroot | HTML, CSS và JavaScript giao diện |
| Database/S4_Nhom06_DB.sql | 20 bảng, 21 khóa ngoại và dữ liệu mẫu |
| Database/ThemPhongLuuTruMau.sql | Bổ sung phòng 103, 104, 204 đang lưu trú; tránh tạo trùng khi chạy lại |
| KiemThu/KiemThuDatPhong.py | Kiểm thử tích hợp UC4 và UC6 trên database riêng |
| HuongDanBaoVe.md | Giải thích class, method và luồng để trình bày demo |

Không xóa KhungChucNang: đây là các lớp và chức năng được bổ sung theo yêu cầu báo cáo. Booking và Guest dùng partial, mỗi tên vẫn là một class duy nhất khi biên dịch.

## Đối chiếu SD và CD

| Chức năng | Luồng hiện thực |
|---|---|
| UC4.1 · Tìm phòng | Control kiểm tra thời gian, gọi Room.DanhSachPhongTrong; mỗi Room gọi kiemTraPhongTrong(DateTime, DateTime), trả sức chứa, giá và tạm tính. |
| UC4.2 · Tạo đặt phòng | Control mở transaction, khóa phòng theo ID, đọc/tạo khách và gọi Booking.taoDatPhong(). Booking kiểm tra min_deposit, kiểm tra phòng và sức chứa, lưu confirmed và BookingDetail, cập nhật Room. |
| UC4.3 · Sửa | Booking.suaDatPhong() chỉ cho pending/confirmed; gọi đúng Room.kiemTraPhongTrong hai tham số. Ngữ cảnh loại trừ chính lượt đặt nằm trên bản sao Room riêng, không ghi thêm cột SQL; cập nhật ngày/phòng/giá và đồng bộ trạng thái phòng trong transaction. |
| UC4.4 · Hủy | Booking.huyDatPhong() kiểm tra trạng thái, lưu lý do và phí hủy, giữ lịch sử chi tiết/cọc; Room đồng bộ tình trạng, bảo toàn các lượt đang lưu trú và lịch tương lai khác. |
| UC4.5 · Tìm đặt phòng | Control gọi Booking.TimKiem theo mã, khách, phòng, khoảng lưu trú giao với bộ lọc ngày và trạng thái; có nhánh không có kết quả. |
| UC4.6 · Online | Chọn loại và số phòng, nhập liên hệ; khóa và kiểm tra lại phòng trong transaction. Booking.taoDatPhong() lưu pending, online, employee_id NULL, cọc 0. Sau commit tạo email cục bộ. |
| UC6.1 · Thêm dịch vụ | Control kiểm tra quyền/dữ liệu; Service.Them kiểm tra trùng tên và lưu. |
| UC6.2 · Sửa dịch vụ | Tải chi tiết qua Control và Service.Doc; kiểm tra dữ liệu, gọi Service.capNhatThongTin(). |
| UC6.3 · Xóa dịch vụ | Xác nhận xóa; Service.Xoa kiểm tra ServiceUsage, chỉ xóa khi chưa sử dụng. |
| UC6.4 · Ghi nhận | Control gọi Booking.KiemTraDangO, Service.kiemTraConCungCap, Service.tinhThanhTien(int), rồi Booking.GhiNhan và ServiceUsage.ghiNhanSuDung(int); trả tổng tiền Booking. |

C# dùng bool tương ứng boolean, DateTime tương ứng Date. Thuộc tính tiền dùng double theo CD; API dùng decimal để kiểm tra số nguyên đồng và giới hạn DECIMAL(12,0); SQL lưu DECIMAL. ServiceUsage giữ đơn giá lịch sử khi danh mục đổi giá. ServiceUsage.Id dùng int theo CD/SQL.

SD Hình 3.1.45 đặt bước tính tiền ở lifeline Dịch vụ, CD giao method cho ServiceUsage. Theo yêu cầu ưu tiên SD, code bổ sung Service.tinhThanhTien(int) cho luồng chính; ServiceUsage.tinhThanhTien() vẫn được giữ để tính theo đơn giá đã lưu. Báo cáo chưa được sửa theo method bổ sung này.

## Phạm vi và lựa chọn demo

- DanhMucChucNang.cs đối chiếu 49 use case; 13 use case được triển khai hoặc dùng trong luồng hỗ trợ. UC3.1 thêm khách nằm trong luồng tạo đặt phòng.
- 36 use case còn lại có giao diện ghi rõ minh họa, dùng Xem trước/Nhập lại và bảng mẫu. Không lưu vào MySQL. Các Control chưa triển khai ném ChuaTrienKhaiException.
- Booking.nhanPhong() và traPhong() là khung UC5, không thuộc phạm vi hoàn thiện UC4. Account.dangNhap trong khung không nối API; đăng nhập thật nằm ở KhoiDong.cs.
- Chính sách hủy do người dùng chọn: miễn phí, toàn bộ deposit là số tiền cần hoàn; không thực hiện chuyển tiền. Dùng cancel_reason, cancel_fee và deposit có sẵn, không thêm thuộc tính ngoài từ điển dữ liệu.
- Email chỉ lưu `.eml` cục bộ, không gửi Internet. ChayDemo.cmd đặt thư tại `mysql-local/ThuXacNhan/DPxxxx/`; OOAD_EMAIL_DIR cho phép đổi nơi lưu. Nếu chạy DLL trực tiếp không đặt biến này, thư nằm trong ThuXacNhan cạnh DLL. Lỗi ghi thư sau commit được báo cùng mã đặt phòng đã lưu, tránh tạo lại ngoài ý muốn.

## Kiểm thử

```powershell
dotnet build S4_Nhom06/ChuongTrinh/SourceCode/KhachSan.csproj
python S4_Nhom06/ChuongTrinh/KiemThu/KiemThuDatPhong.py --dll S4_Nhom06/ChuongTrinh/SourceCode/bin/Debug/net10.0/KhachSan.dll
```

Script hiện dùng MySQL local cổng 33316 và tài khoản root với mật khẩu đọc từ `mysql-local/demo-password`; cần instance này đang chạy. Script tự tạo database tên ngẫu nhiên, khởi động server trên cổng riêng, chạy 44 kiểm tra rồi xóa tài nguyên của lần thử. Không ghi vào ooad_hotel. Các ca gồm rollback, cọc cấu hình, sửa trùng, sửa chính phòng cũ, cấm sửa/hủy lưu trú, hủy lặp, quyền, tranh chấp online, email, tính tiền dịch vụ, đơn giá lịch sử, tách phòng và xóa có ràng buộc.

bin, obj, database local, mật khẩu, log, email, cache và cấu hình cá nhân được loại khỏi Git. Không cần sao chép các thư mục này lên GitHub hoặc vào gói source nộp bài.
