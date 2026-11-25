// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienCaiDat
{
    /// <summary>UC10.1 – Quản lý tài khoản. Chưa triển khai.</summary>
    public void QuanLyTaiKhoan(Account taiKhoan, string thaoTac)
    {
        // Tác nhân: Admin
        // TODO: 1. Admin chọn thao tác: thêm tài khoản, khóa/mở khóa hoặc đặt lại mật khẩu
        // TODO: 2. Admin nhập thông tin cần thiết (tên đăng nhập, nhân viên liên kết, vai trò, mật khẩu tạm thời)
        // TODO: 3. Hệ thống kiểm tra tính hợp lệ của dữ liệu
        // TODO: 4. Hệ thống lưu thay đổi vào CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Cập nhật tài khoản thành công”
        throw new ChuaTrienKhaiException("UC10.1 – Quản lý tài khoản");
    }

    /// <summary>UC10.2 – Phân quyền người dùng. Chưa triển khai.</summary>
    public void PhanQuyenNguoiDung(RolePermission quyen)
    {
        // Tác nhân: Admin
        // TODO: 1. Admin chọn vai trò cần phân quyền
        // TODO: 2. Hệ thống hiển thị ma trận quyền (chức năng và các quyền xem, thêm, sửa, xóa)
        // TODO: 3. Admin điều chỉnh các quyền của vai trò
        // TODO: 4. Hệ thống cập nhật phân quyền vào CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Phân quyền thành công”; quyền mới có hiệu lực từ lần đăng nhập kế tiếp
        throw new ChuaTrienKhaiException("UC10.2 – Phân quyền người dùng");
    }

    /// <summary>UC10.3 – Cấu hình khách sạn. Chưa triển khai.</summary>
    public void CauHinhKhachSan(Setting cauHinh, string giaTriMoi)
    {
        // Tác nhân: Admin
        // TODO: 1. Admin chọn cấu hình khách sạn
        // TODO: 2. Hệ thống hiển thị các tham số: giờ nhận phòng chuẩn, giờ trả phòng chuẩn, thuế VAT, phí phục vụ, phụ phí trả muộn, mức đặt cọc tối thiểu, chính sách hủy phòng
        // TODO: 3. Admin chỉnh sửa các tham số
        // TODO: 4. Hệ thống kiểm tra giá trị tham số
        // TODO: 5. Hệ thống lưu cấu hình vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Lưu cấu hình thành công”
        throw new ChuaTrienKhaiException("UC10.3 – Cấu hình khách sạn");
    }

    /// <summary>UC10.4 – Quản lý chương trình khuyến mãi. Chưa triển khai.</summary>
    public void QuanLyKhuyenMai(Promotion khuyenMai, string thaoTac)
    {
        // Tác nhân: Admin, Quản lý
        // TODO: 1. Người dùng chọn thao tác: thêm, sửa hoặc ngừng áp dụng chương trình khuyến mãi
        // TODO: 2. Người dùng nhập thông tin: mã, tên chương trình, loại giảm giá (phần trăm hoặc số tiền), giá trị giảm, thời gian áp dụng, giá trị hóa đơn tối thiểu
        // TODO: 3. Hệ thống kiểm tra tính hợp lệ và trùng mã khuyến mãi
        // TODO: 4. Hệ thống lưu thay đổi vào CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Lưu thành công”
        throw new ChuaTrienKhaiException("UC10.4 – Quản lý chương trình khuyến mãi");
    }

    /// <summary>UC10.5 – Sao lưu và phục hồi dữ liệu. Chưa triển khai.</summary>
    public void SaoLuuVaPhucHoiDuLieu(BackupFile banSaoLuu, string thaoTac)
    {
        // Tác nhân: Admin
        // TODO: 1. Admin chọn sao lưu ngay hoặc chọn bản sao lưu cần phục hồi
        // TODO: 2. Hệ thống yêu cầu xác thực bằng mật khẩu của Admin
        // TODO: 3. Admin xác thực mật khẩu
        // TODO: 4. Hệ thống thực hiện sao lưu hoặc phục hồi dữ liệu
        // TODO: 5. Hệ thống hiển thị kết quả thực hiện
        throw new ChuaTrienKhaiException("UC10.5 – Sao lưu và phục hồi dữ liệu");
    }

}
