// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Setting
{
    /// <summary>Mã cấu hình.</summary>
    public int Id { get; set; }

    /// <summary>Khóa cấu hình.</summary>
    public string SettingKey { get; set; } = "";

    /// <summary>Giá trị cấu hình.</summary>
    public string SettingValue { get; set; } = "";

    /// <summary>Mô tả cấu hình.</summary>
    public string? Description { get; set; }

    /// <summary>Kiểm tra giá trị cấu hình hợp lệ. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool kiemTraGiaTri(string giaTriMoi)
    {
        throw new ChuaTrienKhaiException("Setting.kiemTraGiaTri");
    }

    /// <summary>Lưu tham số vận hành khách sạn. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool capNhatCauHinh(string giaTriMoi)
    {
        throw new ChuaTrienKhaiException("Setting.capNhatCauHinh");
    }
}
