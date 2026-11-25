// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienKhachHang
{
    /// <summary>UC3.2 – Sửa thông tin khách hàng. Chưa triển khai.</summary>
    public void SuaKhachHang(Guest khachHang)
    {
        // Tác nhân: Lễ tân, Quản lý
        // TODO: 1. Người dùng chọn khách hàng cần sửa
        // TODO: 2. Hệ thống hiển thị thông tin chi tiết của khách hàng
        // TODO: 3. Người dùng cập nhật thông tin khách hàng
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu
        // TODO: 5. Hệ thống lưu thông tin đã chỉnh sửa vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Cập nhật thành công”
        throw new ChuaTrienKhaiException("UC3.2 – Sửa thông tin khách hàng");
    }

    /// <summary>UC3.3 – Xem chi tiết khách hàng. Chưa triển khai.</summary>
    public void XemChiTietKhachHang(int maKhachHang)
    {
        // Tác nhân: Lễ tân, Quản lý
        // TODO: 1. Người dùng chọn khách hàng muốn xem thông tin chi tiết
        // TODO: 2. Hệ thống truy xuất thông tin khách hàng từ CSDL
        // TODO: 3. Hệ thống hiển thị thông tin chi tiết của khách hàng cùng lịch sử lưu trú và các hóa đơn đã thanh toán
        throw new ChuaTrienKhaiException("UC3.3 – Xem chi tiết khách hàng");
    }

    /// <summary>UC3.4 – Tìm kiếm khách hàng. Chưa triển khai.</summary>
    public void TimKiemKhachHang(string tuKhoa)
    {
        // Tác nhân: Lễ tân, Quản lý
        // TODO: 1. Người dùng nhập tiêu chí tìm kiếm: họ tên, số điện thoại, số CCCD/hộ chiếu, loại khách
        // TODO: 2. Hệ thống truy vấn CSDL theo các tiêu chí đã nhập
        // TODO: 3. Hệ thống hiển thị danh sách khách hàng phù hợp
        throw new ChuaTrienKhaiException("UC3.4 – Tìm kiếm khách hàng");
    }

}
