// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class BookingDetail
{
    /// <summary>Mã chi tiết đặt phòng.</summary>
    public int Id { get; set; }

    /// <summary>Giá phòng mỗi đêm tại thời điểm đặt.</summary>
    public double PricePerNight { get; set; }

    /// <summary>Giờ nhận phòng thực tế.</summary>
    public DateTime? ActualCheckIn { get; set; }

    /// <summary>Giờ trả phòng thực tế.</summary>
    public DateTime? ActualCheckOut { get; set; }

    /// <summary>Tính chi phí lưu trú của từng phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public double tinhTienPhong()
    {
        throw new ChuaTrienKhaiException("BookingDetail.tinhTienPhong");
    }

    /// <summary>Thực hiện đổi phòng khi còn phòng phù hợp. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool doiPhong(Room phongMoi)
    {
        throw new ChuaTrienKhaiException("BookingDetail.doiPhong");
    }

    /// <summary>Gia hạn thời gian lưu trú của chi tiết đặt phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool giaHanLuuTru(DateTime ngayTraMoi)
    {
        throw new ChuaTrienKhaiException("BookingDetail.giaHanLuuTru");
    }
}
