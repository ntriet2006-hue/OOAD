// Khung Control theo đặc tả use case. Chưa nối route hoặc giao diện web.
sealed class DieuKhienPhong
{
    /// <summary>UC2.1 – Thêm phòng. Chưa triển khai.</summary>
    public void ThemPhong(Room phong)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn thêm phòng
        // TODO: 2. Hệ thống hiển thị biểu mẫu nhập thông tin phòng
        // TODO: 3. Quản lý nhập thông tin: số phòng, tầng, loại phòng, mô tả, tình trạng ban đầu
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu và kiểm tra trùng số phòng
        // TODO: 5. Hệ thống lưu thông tin phòng vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Lưu thành công” và cập nhật danh sách phòng
        throw new ChuaTrienKhaiException("UC2.1 – Thêm phòng");
    }

    /// <summary>UC2.2 – Sửa thông tin phòng. Chưa triển khai.</summary>
    public void SuaPhong(Room phong)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn phòng cần sửa
        // TODO: 2. Hệ thống hiển thị thông tin chi tiết của phòng
        // TODO: 3. Quản lý cập nhật thông tin phòng
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu
        // TODO: 5. Hệ thống lưu thông tin đã chỉnh sửa vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Cập nhật thành công”
        throw new ChuaTrienKhaiException("UC2.2 – Sửa thông tin phòng");
    }

    /// <summary>UC2.3 – Xóa phòng. Chưa triển khai.</summary>
    public void XoaPhong(int maPhong)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn phòng muốn xóa
        // TODO: 2. Quản lý chọn “Xóa”
        // TODO: 3. Hệ thống kiểm tra ràng buộc dữ liệu liên quan đến phòng
        // TODO: 4. Hệ thống xóa phòng khỏi CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Xóa thành công” và cập nhật danh sách
        throw new ChuaTrienKhaiException("UC2.3 – Xóa phòng");
    }

    /// <summary>UC2.4 – Xem chi tiết phòng. Chưa triển khai.</summary>
    public void XemChiTietPhong(int maPhong)
    {
        // Tác nhân: Quản lý, Lễ tân
        // TODO: 1. Người dùng chọn phòng muốn xem thông tin chi tiết
        // TODO: 2. Hệ thống truy xuất thông tin phòng từ CSDL
        // TODO: 3. Hệ thống hiển thị thông tin chi tiết của phòng (kèm loại phòng, giá phòng và tình trạng hiện tại)
        throw new ChuaTrienKhaiException("UC2.4 – Xem chi tiết phòng");
    }

    /// <summary>UC2.5 – Tìm kiếm và lọc phòng. Chưa triển khai.</summary>
    public void TimKiemVaLocPhong(string tuKhoa, string trangThai, int? maLoaiPhong)
    {
        // Tác nhân: Quản lý, Lễ tân
        // TODO: 1. Người dùng nhập tiêu chí tìm kiếm: số phòng, loại phòng, tầng, tình trạng, khoảng giá
        // TODO: 2. Hệ thống truy vấn CSDL theo các tiêu chí đã nhập
        // TODO: 3. Hệ thống hiển thị danh sách phòng phù hợp
        throw new ChuaTrienKhaiException("UC2.5 – Tìm kiếm và lọc phòng");
    }

    /// <summary>UC2.6 – Thêm loại phòng. Chưa triển khai.</summary>
    public void ThemLoaiPhong(RoomType loaiPhong)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn thêm loại phòng
        // TODO: 2. Hệ thống hiển thị biểu mẫu nhập thông tin loại phòng
        // TODO: 3. Quản lý nhập thông tin: tên loại phòng, sức chứa, loại giường, diện tích, giá cơ bản mỗi đêm, mô tả
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu và kiểm tra trùng tên loại phòng
        // TODO: 5. Hệ thống lưu thông tin loại phòng vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Lưu thành công” và cập nhật danh sách loại phòng
        throw new ChuaTrienKhaiException("UC2.6 – Thêm loại phòng");
    }

    /// <summary>UC2.7 – Cập nhật loại phòng và giá. Chưa triển khai.</summary>
    public void CapNhatLoaiPhongVaGia(RoomType loaiPhong)
    {
        // Tác nhân: Quản lý
        // TODO: 1. Quản lý chọn loại phòng cần sửa
        // TODO: 2. Hệ thống hiển thị thông tin chi tiết của loại phòng
        // TODO: 3. Quản lý cập nhật thông tin loại phòng
        // TODO: 4. Hệ thống kiểm tra tính hợp lệ của dữ liệu
        // TODO: 5. Hệ thống lưu thông tin đã chỉnh sửa vào CSDL
        // TODO: 6. Hệ thống hiển thị thông báo “Cập nhật thành công”
        throw new ChuaTrienKhaiException("UC2.7 – Cập nhật loại phòng và giá");
    }

    /// <summary>UC2.8 – Cập nhật trạng thái phòng. Chưa triển khai.</summary>
    public void CapNhatTrangThaiPhong(int maPhong, string trangThai)
    {
        // Tác nhân: Lễ tân, Quản lý
        // TODO: 1. Người dùng chọn phòng cần cập nhật trạng thái
        // TODO: 2. Người dùng chọn trạng thái mới cho phòng
        // TODO: 3. Hệ thống kiểm tra tính hợp lệ của việc chuyển trạng thái
        // TODO: 4. Hệ thống lưu trạng thái mới vào CSDL
        // TODO: 5. Hệ thống hiển thị thông báo “Cập nhật thành công” và làm mới sơ đồ phòng
        throw new ChuaTrienKhaiException("UC2.8 – Cập nhật trạng thái phòng");
    }

}
