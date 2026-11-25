// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class RolePermission
{
    /// <summary>Mã phân quyền.</summary>
    public int Id { get; set; }

    /// <summary>Mã chức năng.</summary>
    public string FunctionCode { get; set; } = "";

    /// <summary>Quyền xem.</summary>
    public bool CanView { get; set; }

    /// <summary>Quyền thêm.</summary>
    public bool CanAdd { get; set; }

    /// <summary>Quyền sửa.</summary>
    public bool CanEdit { get; set; }

    /// <summary>Quyền xóa.</summary>
    public bool CanDelete { get; set; }

    /// <summary>Cập nhật quyền thao tác trên chức năng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatQuyen(bool xem, bool them, bool sua, bool xoa)
    {
        throw new ChuaTrienKhaiException("RolePermission.capNhatQuyen");
    }

    /// <summary>Kiểm tra quyền xem, thêm, sửa hoặc xóa. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool coQuyen(string maThaoTac)
    {
        throw new ChuaTrienKhaiException("RolePermission.coQuyen");
    }
}
