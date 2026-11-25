// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Payment
{
    /// <summary>Mã thanh toán.</summary>
    public int Id { get; set; }

    /// <summary>Số tiền thanh toán.</summary>
    public double Amount { get; set; }

    /// <summary>Hình thức thanh toán.</summary>
    public string Method { get; set; } = "";

    /// <summary>Thời điểm thanh toán.</summary>
    public DateTime PaidAt { get; set; }

    /// <summary>Mã giao dịch.</summary>
    public string? TransactionCode { get; set; }

    /// <summary>Kiểm tra số tiền thanh toán hợp lệ. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool kiemTraSoTien()
    {
        throw new ChuaTrienKhaiException("Payment.kiemTraSoTien");
    }

    /// <summary>Lưu giao dịch thanh toán của hóa đơn. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool ghiNhanThanhToan()
    {
        throw new ChuaTrienKhaiException("Payment.ghiNhanThanhToan");
    }
}
