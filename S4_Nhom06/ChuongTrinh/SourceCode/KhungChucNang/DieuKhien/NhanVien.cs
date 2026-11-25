// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienNhanVien
{
    /// <summary>UC8.1 – Thêm nhân viên. Chưa triển khai.</summary>
    public void ThemNhanVien(Employee nhanVien)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn thêm nhân viên
        // TODO: 2. Hệ thống hiển thị biểu mẫu nhập thông tin nhân viên
        // TODO: 3. Quản lý nhập thông tin: họ tên, giới tính, ngày sinh, số CCCD, số điện thoại, email, địa chỉ, phòng ban, chức vụ, ngày vào làm, lương cơ bản, số tài khoản ngân hàng
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu và kiểm tra trùng số CCCD
        // TODO: 5. Hệ thống lưu thông tin nhân viên vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Lưu thành công” và cập nhật danh sách nhân viên
        throw new ChuaTrienKhaiException("UC8.1 – Thêm nhân viên");
    }

    /// <summary>UC8.2 – Sửa thông tin nhân viên. Chưa triển khai.</summary>
    public void SuaNhanVien(Employee nhanVien)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn nhân viên cần sửa
        // TODO: 2. Hệ thống hiển thị thông tin chi tiết của nhân viên
        // TODO: 3. Quản lý cập nhật thông tin nhân viên
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu
        // TODO: 5. Hệ thống lưu thông tin đã chỉnh sửa vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Cập nhật thành công”
        throw new ChuaTrienKhaiException("UC8.2 – Sửa thông tin nhân viên");
    }

    /// <summary>UC8.3 – Xóa nhân viên. Chưa triển khai.</summary>
    public void XoaNhanVien(int maNhanVien)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn nhân viên muốn xóa
        // TODO: 2. Quản lý chọn “Xóa”
        // TODO: 3. Hệ thống kiểm tra ràng buộc dữ liệu liên quan đến nhân viên
        // TODO: 4. Hệ thống xóa nhân viên khỏi CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Xóa thành công” và cập nhật danh sách
        throw new ChuaTrienKhaiException("UC8.3 – Xóa nhân viên");
    }

    /// <summary>UC8.4 – Xem chi tiết nhân viên. Chưa triển khai.</summary>
    public void XemChiTietNhanVien(int maNhanVien)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn nhân viên muốn xem thông tin chi tiết
        // TODO: 2. Hệ thống truy xuất thông tin nhân viên từ CSDL
        // TODO: 3. Hệ thống hiển thị thông tin chi tiết của nhân viên (kèm chức vụ, phòng ban và lịch làm việc)
        throw new ChuaTrienKhaiException("UC8.4 – Xem chi tiết nhân viên");
    }

    /// <summary>UC8.5 – Tìm kiếm nhân viên. Chưa triển khai.</summary>
    public void TimKiemNhanVien(string tuKhoa)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý nhập tiêu chí tìm kiếm: mã nhân viên, họ tên, chức vụ, phòng ban
        // TODO: 2. Hệ thống truy vấn CSDL theo các tiêu chí đã nhập
        // TODO: 3. Hệ thống hiển thị danh sách nhân viên phù hợp
        throw new ChuaTrienKhaiException("UC8.5 – Tìm kiếm nhân viên");
    }

    /// <summary>UC8.6 – Phân ca làm việc. Chưa triển khai.</summary>
    public void PhanCaLamViec(Employee nhanVien, DateTime ngay, string ca)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn ngày và ca làm việc cần phân
        // TODO: 2. Quản lý chọn nhân viên từ danh sách nhân viên đang làm việc
        // TODO: 3. Hệ thống kiểm tra trùng ca và số ca tối đa trong tuần của nhân viên
        // TODO: 4. Hệ thống lưu ca làm việc vào CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Phân ca thành công” và cập nhật lịch làm việc
        throw new ChuaTrienKhaiException("UC8.6 – Phân ca làm việc");
    }

}
