// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienNhanTraPhong
{
    /// <summary>UC5.1 – Nhận phòng (Check-in). Chưa triển khai.</summary>
    public void NhanPhong(int maDatPhong)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Lễ tân chọn đặt phòng đến hạn nhận phòng (hoặc nhập mã đặt phòng)
        // TODO: 2. Hệ thống hiển thị chi tiết đặt phòng, thông tin khách hàng và phòng
        // TODO: 3. Lễ tân đối chiếu giấy tờ tùy thân và cập nhật thông tin khách hàng (nếu cần)
        // TODO: 4. Lễ tân xác nhận khoản tiền đặt cọc
        // TODO: 5. Hệ thống kiểm tra phòng đã sẵn sàng để nhận khách
        // TODO: 6. Hệ thống ghi nhận giờ nhận phòng thực tế, chuyển đặt phòng sang “Đang lưu trú” và phòng sang “Đang sử dụng”
        // TODO: 7. Hệ thống hiển thị thông báo “Nhận phòng thành công”
        throw new ChuaTrienKhaiException("UC5.1 – Nhận phòng (Check-in)");
    }

    /// <summary>UC5.2 – Trả phòng (Check-out). Chưa triển khai.</summary>
    public void TraPhong(int maDatPhong)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Lễ tân chọn phòng (đặt phòng) khách cần trả
        // TODO: 2. Hệ thống tổng hợp tiền phòng, dịch vụ đã sử dụng, phụ phí và tiền đặt cọc
        // TODO: 3. Lễ tân kiểm tra phòng cùng khách và ghi nhận phát sinh (nếu có)
        // TODO: 4. Hệ thống thực hiện lập hóa đơn (UC7.1)
        // TODO: 5. Lễ tân thực hiện thanh toán cho khách (UC7.2)
        // TODO: 6. Hệ thống chuyển đặt phòng sang “Đã trả phòng” và phòng sang “Đang dọn”
        // TODO: 7. Hệ thống hiển thị thông báo “Trả phòng thành công”
        throw new ChuaTrienKhaiException("UC5.2 – Trả phòng (Check-out)");
    }

    /// <summary>UC5.3 – Đổi phòng. Chưa triển khai.</summary>
    public void DoiPhong(int maChiTietDatPhong, Room phongMoi)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Lễ tân chọn phòng khách đang lưu trú cần đổi
        // TODO: 2. Lễ tân nhập lý do đổi phòng
        // TODO: 3. Hệ thống hiển thị các phòng trống trong khoảng thời gian lưu trú còn lại
        // TODO: 4. Lễ tân chọn phòng mới
        // TODO: 5. Hệ thống kiểm tra phòng mới còn trống
        // TODO: 6. Hệ thống cập nhật chi tiết đặt phòng, chuyển phòng cũ sang “Đang dọn”, phòng mới sang “Đang sử dụng” và ghi nhận chênh lệch giá (nếu có)
        // TODO: 7. Hệ thống hiển thị thông báo “Đổi phòng thành công”
        throw new ChuaTrienKhaiException("UC5.3 – Đổi phòng");
    }

    /// <summary>UC5.4 – Gia hạn lưu trú. Chưa triển khai.</summary>
    public void GiaHanLuuTru(int maChiTietDatPhong, DateTime ngayTraMoi)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Lễ tân chọn phòng khách đang lưu trú
        // TODO: 2. Lễ tân nhập ngày trả phòng mới
        // TODO: 3. Hệ thống kiểm tra phòng có bị khách khác đặt trong khoảng thời gian gia hạn hay không
        // TODO: 4. Hệ thống tính tiền phòng phát sinh do gia hạn
        // TODO: 5. Hệ thống cập nhật ngày trả phòng và tiền phòng tạm tính vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Gia hạn lưu trú thành công”
        throw new ChuaTrienKhaiException("UC5.4 – Gia hạn lưu trú");
    }

}
