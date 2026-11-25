// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Department
{
    /// <summary>Mã phòng ban.</summary>
    public int Id { get; set; }

    /// <summary>Tên phòng ban.</summary>
    public string Name { get; set; } = "";

    /// <summary>Mô tả phòng ban.</summary>
    public string? Description { get; set; }

    /// <summary>Cập nhật thông tin phòng ban của nhân viên. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatThongTin(string ten, string moTa)
    {
        throw new ChuaTrienKhaiException("Department.capNhatThongTin");
    }
}
