// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienHoaDon
{
    /// <summary>UC7.1 – Lập hóa đơn. Chưa triển khai.</summary>
    public void LapHoaDon(int maDatPhong, string maKhuyenMai)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Lễ tân chọn đặt phòng cần lập hóa đơn
        // TODO: 2. Hệ thống tính tiền phòng bằng số đêm nhân giá phòng
        // TODO: 3. Hệ thống tổng hợp tiền các dịch vụ đã sử dụng
        // TODO: 4. Hệ thống tính phụ phí (nếu có) và thuế VAT theo cấu hình
        // TODO: 5. Lễ tân áp dụng khuyến mãi nếu có (UC7.4)
        // TODO: 6. Hệ thống trừ tiền đặt cọc đã thu
        // TODO: 7. Hệ thống lưu hóa đơn với trạng thái “Chưa thanh toán”
        // TODO: 8. Hệ thống hiển thị thông báo “Lập hóa đơn thành công”
        throw new ChuaTrienKhaiException("UC7.1 – Lập hóa đơn");
    }

    /// <summary>UC7.2 – Thanh toán hóa đơn. Chưa triển khai.</summary>
    public void ThanhToanHoaDon(Payment thanhToan)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Lễ tân chọn hóa đơn cần thanh toán
        // TODO: 2. Hệ thống hiển thị chi tiết hóa đơn và số tiền còn phải thanh toán
        // TODO: 3. Lễ tân chọn hình thức thanh toán (tiền mặt, thẻ, chuyển khoản, ví điện tử) và nhập số tiền khách thanh toán
        // TODO: 4. Hệ thống kiểm tra số tiền thanh toán
        // TODO: 5. Hệ thống lưu giao dịch và cập nhật trạng thái hóa đơn (“Đã thanh toán” hoặc “Thanh toán một phần”)
        // TODO: 6. Hệ thống hiển thị thông báo thanh toán thành công và tiền thừa trả khách (nếu có)
        throw new ChuaTrienKhaiException("UC7.2 – Thanh toán hóa đơn");
    }

    /// <summary>UC7.3 – Xem và in hóa đơn. Chưa triển khai.</summary>
    public void XemVaInHoaDon(int maHoaDon, bool inHoaDon)
    {
        // Tác nhân: Lễ tân, Quản lý
        // TODO: 1. Người dùng nhập tiêu chí tìm kiếm: mã hóa đơn, tên khách hàng, ngày lập, trạng thái
        // TODO: 2. Hệ thống hiển thị danh sách hóa đơn phù hợp
        // TODO: 3. Người dùng chọn hóa đơn cần xem
        // TODO: 4. Hệ thống hiển thị chi tiết hóa đơn (tiền phòng, dịch vụ, thuế, khuyến mãi, các lần thanh toán)
        // TODO: 5. Người dùng chọn in hóa đơn
        // TODO: 6. Hệ thống tạo bản in hoặc tệp PDF của hóa đơn
        throw new ChuaTrienKhaiException("UC7.3 – Xem và in hóa đơn");
    }

    /// <summary>UC7.4 – Áp dụng khuyến mãi. Chưa triển khai.</summary>
    public void ApDungKhuyenMai(int maHoaDon, string maKhuyenMai)
    {
        // Tác nhân: Lễ tân
        // TODO: 1. Trong khi lập hóa đơn, Lễ tân nhập mã khuyến mãi hoặc chọn chương trình khuyến mãi
        // TODO: 2. Hệ thống tìm chương trình khuyến mãi trong CSDL
        // TODO: 3. Hệ thống kiểm tra điều kiện áp dụng (thời hạn, giá trị hóa đơn tối thiểu, trạng thái)
        // TODO: 4. Hệ thống tính số tiền được giảm
        // TODO: 5. Hệ thống cập nhật tổng tiền của hóa đơn
        // TODO: 6. Hệ thống hiển thị thông báo “Áp dụng khuyến mãi thành công”
        throw new ChuaTrienKhaiException("UC7.4 – Áp dụng khuyến mãi");
    }

}
