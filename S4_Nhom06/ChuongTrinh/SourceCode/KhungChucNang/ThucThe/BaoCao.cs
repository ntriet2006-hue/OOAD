// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Report
{
    /// <summary>Mã báo cáo.</summary>
    public int Id { get; set; }

    /// <summary>Tên báo cáo.</summary>
    public string Name { get; set; } = "";

    /// <summary>Loại báo cáo.</summary>
    public string ReportType { get; set; } = "";

    /// <summary>Thời điểm tạo.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Từ ngày.</summary>
    public DateTime FromDate { get; set; }

    /// <summary>Đến ngày.</summary>
    public DateTime ToDate { get; set; }

    /// <summary>Nội dung báo cáo.</summary>
    public string? Content { get; set; }

    /// <summary>Tạo báo cáo doanh thu theo khoảng thời gian. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void taoBaoCaoDoanhThu(DateTime tuNgay, DateTime denNgay)
    {
        throw new ChuaTrienKhaiException("Report.taoBaoCaoDoanhThu");
    }

    /// <summary>Tạo báo cáo công suất phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void taoBaoCaoCongSuatPhong(DateTime tuNgay, DateTime denNgay)
    {
        throw new ChuaTrienKhaiException("Report.taoBaoCaoCongSuatPhong");
    }

    /// <summary>Tạo báo cáo khách hàng và đặt phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void taoBaoCaoKhachHang(DateTime tuNgay, DateTime denNgay)
    {
        throw new ChuaTrienKhaiException("Report.taoBaoCaoKhachHang");
    }

    /// <summary>Xuất hoặc in báo cáo đã tạo. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool xuatBaoCao(string dinhDang)
    {
        throw new ChuaTrienKhaiException("Report.xuatBaoCao");
    }
}
