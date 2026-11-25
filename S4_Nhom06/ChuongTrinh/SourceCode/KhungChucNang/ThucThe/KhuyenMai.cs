// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Promotion
{
    /// <summary>Mã khuyến mãi.</summary>
    public int Id { get; set; }

    /// <summary>Mã khuyến mãi.</summary>
    public string Code { get; set; } = "";

    /// <summary>Tên chương trình.</summary>
    public string Name { get; set; } = "";

    /// <summary>Loại giảm giá.</summary>
    public string DiscountType { get; set; } = "";

    /// <summary>Giá trị giảm.</summary>
    public double DiscountValue { get; set; }

    /// <summary>Giá trị hóa đơn tối thiểu.</summary>
    public double MinAmount { get; set; }

    /// <summary>Ngày bắt đầu.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>Ngày kết thúc.</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Trạng thái áp dụng.</summary>
    public string Status { get; set; } = "";

    /// <summary>Kiểm tra điều kiện áp dụng chương trình khuyến mãi. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool kiemTraDieuKien(Invoice hoaDon)
    {
        throw new ChuaTrienKhaiException("Promotion.kiemTraDieuKien");
    }

    /// <summary>Tính giá trị khuyến mãi theo quy tắc áp dụng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public double tinhTienGiam(double tamTinh)
    {
        throw new ChuaTrienKhaiException("Promotion.tinhTienGiam");
    }
}
