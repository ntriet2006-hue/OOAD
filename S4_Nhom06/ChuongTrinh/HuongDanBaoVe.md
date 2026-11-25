# Trình bày các class khi bảo vệ

## Cách giải thích ngắn

Ứng dụng tổ chức theo Boundary – Control – Entity trong báo cáo. Giao diện HTML/JavaScript nhận thao tác; API chuyển yêu cầu đến Control; Control điều phối và mở giao dịch; Entity thực hiện nghiệp vụ và lưu qua CoSoDuLieu. Không phải mỗi chức năng chỉ có một class.

## Những class cần chỉ được trong source

| Class | Vai trò | Method cần chỉ |
|---|---|---|
| DieuKhienDatPhong | Điều phối UC4 | Tim, Tao, SuaDatPhong, HuyDatPhong, TimKiemDatPhong, DatPhongTrucTuyen |
| Booking | Thực thể đặt phòng, trạng thái, cọc, lịch lưu trú | taoDatPhong(): bool, suaDatPhong(): bool, huyDatPhong(): bool |
| Room | Thực thể phòng, kiểm tra lịch và tình trạng | kiemTraPhongTrong(DateTime, DateTime): bool, capNhatTrangThai(string): bool |
| Guest | Hồ sơ khách | Doc, Tao, kiemTraTrungGiayTo(string): bool |
| DieuKhienDichVu | Điều phối UC6 và kiểm tra quyền | ChiTiet, DanhSach, Luu, Xoa, GhiNhan |
| Service | Thông tin dịch vụ và giá hiện tại | capNhatThongTin(): void, kiemTraConCungCap(): bool, tinhThanhTien(int): double |
| ServiceUsage | Lần sử dụng dịch vụ, số lượng, giá lịch sử, thời điểm | ghiNhanSuDung(int): bool, tinhThanhTien(): double |
| CoSoDuLieu | Hạ tầng kết nối/truy vấn/giao dịch | Doc, Ghi, Luu<T>, Dispose |

## Ví dụ thầy hỏi luồng

**Tạo đặt phòng:** giao diện gửi API -> DieuKhienDatPhong.Tao -> mở giao dịch, khóa Room theo ID -> đọc/tạo Guest -> Booking.taoDatPhong kiểm tra cọc, gọi Room.kiemTraPhongTrong -> lưu Booking/BookingDetail -> Room.capNhatTrangThai -> commit và trả mã đặt. Nếu lỗi thì rollback, không lưu sót hồ sơ khách mới.

**Sửa:** Control đọc Booking, kiểm tra trạng thái và chuẩn bị dữ liệu -> Booking.suaDatPhong -> Room.kiemTraPhongTrong với ngữ cảnh loại trừ chính lượt đặt -> tính lại tiền theo số đêm và giá -> cập nhật chi tiết và trạng thái trong giao dịch.

**Ghi nhận dịch vụ:** DieuKhienDichVu.GhiNhan -> Booking.KiemTraDangO -> Service.kiemTraConCungCap -> Service.tinhThanhTien -> Booking.GhiNhan -> ServiceUsage.ghiNhanSuDung -> tổng tiền Booking. Theo lựa chọn ưu tiên SD, bước tính tiền chính nằm ở Service; CD hiện chỉ liệt kê tinhThanhTien trong ServiceUsage, method này vẫn còn để tính theo giá lịch sử. Nêu rõ điểm khác nhau của tài liệu, không nói hai sơ đồ đã thống nhất hoàn toàn.

**Chặn xóa:** Service.Xoa kiểm tra ServiceUsage. Nếu đã dùng thì báo lỗi và gợi ý ngừng cung cấp; nếu chưa dùng thì xóa. Lễ tân không được sửa danh mục, chỉ Quản lý được thêm/sửa/xóa.

## Các câu hỏi thường gặp

- **Vì sao Booking ở nhiều file?** Đây là partial class: compiler gộp các phần thành một class Booking duy nhất, không tạo nhiều loại Booking. Guest cũng tương tự.
- **record có phải class không?** Room, Guest, Service khai báo record không có từ khóa struct nên là kiểu tham chiếu dạng record class, có thêm so sánh theo giá trị và hỗ trợ with. Không nói chúng là struct.
- **Tại sao dùng bản sao Room khi sửa?** Để truyền ngữ cảnh loại trừ lịch của chính Booking mà vẫn giữ chữ ký hai tham số của method CD; bản sao không ghi thêm thuộc tính SQL và không ảnh hưởng yêu cầu khác.
- **Class hỗ trợ có trong CD không?** CoSoDuLieu, DTO yêu cầu và Control là phần hiện thực/hạ tầng. CD trong báo cáo tập trung vào 20 Entity, không nên khẳng định mọi class trong source đều phải xuất hiện trên CD Entity.
- **Vì sao có Them, Xoa, Doc ngoài method CD?** Đây là các thao tác hỗ trợ thông điệp tạo/xóa/truy vấn của SD, bổ sung để hiện thực chương trình.
- **Có hoàn thiện tất cả class không?** Chưa. Hai phân hệ UC4/UC6 đã có nghiệp vụ thật; các chức năng khác chỉ có khung và giao diện minh họa. nhanPhong/traPhong thuộc UC5 vẫn ném chưa triển khai.
- **Vì sao giữ đơn giá trong ServiceUsage?** Để đổi giá danh mục không làm thay đổi chi phí khách đã dùng trước đó.
- **Email và hoàn cọc đã thực hiện thật chưa?** Email lưu cục bộ phục vụ demo; hoàn cọc chỉ ghi nhận khoản cần hoàn, không chuyển tiền. Đây là lựa chọn demo đã xác nhận.
