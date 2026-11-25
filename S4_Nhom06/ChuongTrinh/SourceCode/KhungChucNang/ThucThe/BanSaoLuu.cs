// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class BackupFile
{
    /// <summary>Mã bản sao lưu.</summary>
    public int Id { get; set; }

    /// <summary>Tên tệp sao lưu.</summary>
    public string FileName { get; set; } = "";

    /// <summary>Thời điểm sao lưu.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Dung lượng tệp (byte).</summary>
    public long FileSize { get; set; }

    /// <summary>Tạo bản sao lưu dữ liệu khách sạn. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool saoLuuDuLieu()
    {
        throw new ChuaTrienKhaiException("BackupFile.saoLuuDuLieu");
    }

    /// <summary>Phục hồi dữ liệu từ bản sao lưu. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool phucHoiDuLieu()
    {
        throw new ChuaTrienKhaiException("BackupFile.phucHoiDuLieu");
    }
}
