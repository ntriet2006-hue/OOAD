// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class RoomType
{
    /// <summary>Mã loại phòng.</summary>
    public int Id { get; set; }

    /// <summary>Tên loại phòng.</summary>
    public string Name { get; set; } = "";

    /// <summary>Mô tả loại phòng.</summary>
    public string? Description { get; set; }

    /// <summary>Sức chứa tối đa (số người).</summary>
    public int Capacity { get; set; }

    /// <summary>Loại giường.</summary>
    public string? BedType { get; set; }

    /// <summary>Diện tích (m²).</summary>
    public double Area { get; set; }

    /// <summary>Giá cơ bản mỗi đêm.</summary>
    public double BasePrice { get; set; }

    /// <summary>Cập nhật giá cơ bản của loại phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool capNhatGia(double giaMoi)
    {
        throw new ChuaTrienKhaiException("RoomType.capNhatGia");
    }

    /// <summary>Cập nhật thông tin loại phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatLoaiPhong()
    {
        throw new ChuaTrienKhaiException("RoomType.capNhatLoaiPhong");
    }
}
