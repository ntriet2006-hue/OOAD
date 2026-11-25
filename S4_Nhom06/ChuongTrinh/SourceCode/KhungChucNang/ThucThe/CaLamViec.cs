// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class WorkShift
{
    /// <summary>Mã ca làm việc.</summary>
    public int Id { get; set; }

    /// <summary>Ngày làm việc.</summary>
    public DateTime ShiftDate { get; set; }

    /// <summary>Loại ca (sáng, chiều, đêm).</summary>
    public string ShiftType { get; set; } = "";

    /// <summary>Ghi chú.</summary>
    public string? Note { get; set; }

    /// <summary>Phân công ca làm việc cho nhân viên. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool phanCa(Employee nhanVien, DateTime ngay, string ca)
    {
        throw new ChuaTrienKhaiException("WorkShift.phanCa");
    }

    /// <summary>Kiểm tra ca đã được phân công hay chưa. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool kiemTraTrungCa(Employee nhanVien, DateTime ngay, string ca)
    {
        throw new ChuaTrienKhaiException("WorkShift.kiemTraTrungCa");
    }
}
