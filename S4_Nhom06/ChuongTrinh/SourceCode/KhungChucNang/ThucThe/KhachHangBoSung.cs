// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
/// <summary>
/// Phần thuộc tính hồ sơ đầy đủ của cùng Entity Guest theo CD, được nạp bởi Guest.Doc khi đặt phòng.
/// capNhatHoSo vẫn là khung UC3.2, chưa hoàn thiện nghiệp vụ trong demo.
/// </summary>
sealed partial record Guest
{
    /// <summary>Họ tên khách hàng.</summary>
    public string FullName { get; set; } = "";

    /// <summary>Giới tính.</summary>
    public string Gender { get; set; } = "";

    /// <summary>Ngày sinh.</summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>Số điện thoại.</summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Email.</summary>
    public string Email { get; set; } = "";

    /// <summary>Địa chỉ.</summary>
    public string? Address { get; set; }

    /// <summary>Số CCCD/hộ chiếu.</summary>
    public string IdentityCard { get; set; } = "";

    /// <summary>Quốc tịch.</summary>
    public string? Nationality { get; set; }

    /// <summary>Loại khách hàng.</summary>
    public string GuestType { get; set; } = "";

    /// <summary>Thời điểm tạo hồ sơ.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Cập nhật thông tin khách hàng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool capNhatHoSo()
    {
        throw new ChuaTrienKhaiException("Guest.capNhatHoSo");
    }
}
