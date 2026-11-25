// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Role
{
    /// <summary>Mã vai trò.</summary>
    public int Id { get; set; }

    /// <summary>Tên vai trò.</summary>
    public string Name { get; set; } = "";

    /// <summary>Mô tả vai trò.</summary>
    public string? Description { get; set; }

    /// <summary>Vai trò Admin hệ thống (không được thu hồi quyền).</summary>
    public bool IsSystem { get; set; }

    /// <summary>Cập nhật thông tin vai trò. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatVaiTro(string ten, string moTa)
    {
        throw new ChuaTrienKhaiException("Role.capNhatVaiTro");
    }

    /// <summary>Xác định vai trò hệ thống để bảo vệ quyền Admin. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool kiemTraVaiTroHeThong()
    {
        throw new ChuaTrienKhaiException("Role.kiemTraVaiTroHeThong");
    }
}
