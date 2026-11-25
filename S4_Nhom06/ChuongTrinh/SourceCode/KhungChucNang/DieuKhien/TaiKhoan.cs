// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienTaiKhoan
{
    /// <summary>UC1.3 – Đổi mật khẩu. Chưa triển khai.</summary>
    public void DoiMatKhau(Account taiKhoan, string matKhauCu, string matKhauMoi)
    {
        // Tác nhân: Admin, Quản lý, Lễ tân
        // TODO: 1. Người dùng chọn đổi mật khẩu trong hồ sơ cá nhân
        // TODO: 2. Người dùng nhập mật khẩu hiện tại và mật khẩu mới
        // TODO: 3. Hệ thống kiểm tra mật khẩu hiện tại và độ mạnh của mật khẩu mới
        // TODO: 4. Hệ thống cập nhật mật khẩu (đã mã hóa) vào CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Đổi mật khẩu thành công”
        throw new ChuaTrienKhaiException("UC1.3 – Đổi mật khẩu");
    }

}
