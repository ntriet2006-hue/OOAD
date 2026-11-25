// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Invoice
{
    /// <summary>Mã hóa đơn.</summary>
    public int Id { get; set; }

    /// <summary>Thời điểm lập hóa đơn.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Tiền phòng.</summary>
    public double RoomCharge { get; set; }

    /// <summary>Tiền dịch vụ.</summary>
    public double ServiceCharge { get; set; }

    /// <summary>Phụ phí.</summary>
    public double Surcharge { get; set; }

    /// <summary>Số tiền giảm giá.</summary>
    public double DiscountAmount { get; set; }

    /// <summary>Tiền thuế VAT.</summary>
    public double TaxAmount { get; set; }

    /// <summary>Tổng tiền phải thanh toán.</summary>
    public double TotalAmount { get; set; }

    /// <summary>Trạng thái hóa đơn (hóa đơn chỉ được hủy, không bị xóa).</summary>
    public string Status { get; set; } = "";

    /// <summary>Tổng hợp tiền phòng, dịch vụ, phụ phí, thuế và giảm giá. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public double tinhTongTien()
    {
        throw new ChuaTrienKhaiException("Invoice.tinhTongTien");
    }

    /// <summary>Áp dụng chương trình khuyến mãi hợp lệ. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool apDungKhuyenMai(string maKhuyenMai)
    {
        throw new ChuaTrienKhaiException("Invoice.apDungKhuyenMai");
    }

    /// <summary>Cập nhật trạng thái theo các giao dịch đã ghi nhận. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatTrangThaiThanhToan()
    {
        throw new ChuaTrienKhaiException("Invoice.capNhatTrangThaiThanhToan");
    }
}
