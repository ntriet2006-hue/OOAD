// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Position
{
    /// <summary>Mã chức vụ.</summary>
    public int Id { get; set; }

    /// <summary>Tên chức vụ.</summary>
    public string Name { get; set; } = "";

    /// <summary>Mô tả chức vụ.</summary>
    public string? Description { get; set; }

    /// <summary>Cập nhật thông tin chức vụ của nhân viên. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatThongTin(string ten, string moTa)
    {
        throw new ChuaTrienKhaiException("Position.capNhatThongTin");
    }
}
