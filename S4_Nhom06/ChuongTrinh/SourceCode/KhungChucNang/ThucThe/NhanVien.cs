// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Employee
{
    /// <summary>Mã nhân viên.</summary>
    public int Id { get; set; }

    /// <summary>Họ tên nhân viên.</summary>
    public string FullName { get; set; } = "";

    /// <summary>Giới tính.</summary>
    public string Gender { get; set; } = "";

    /// <summary>Ngày sinh.</summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>Số điện thoại.</summary>
    public string PhoneNumber { get; set; } = "";

    /// <summary>Email.</summary>
    public string Email { get; set; } = "";

    /// <summary>Địa chỉ.</summary>
    public string? Address { get; set; }

    /// <summary>Số căn cước công dân.</summary>
    public string IdentityCard { get; set; } = "";

    /// <summary>Ngày vào làm.</summary>
    public DateTime? HireDate { get; set; }

    /// <summary>Lương cơ bản.</summary>
    public double BaseSalary { get; set; }

    /// <summary>Số tài khoản ngân hàng.</summary>
    public string? BankAccount { get; set; }

    /// <summary>Tình trạng làm việc.</summary>
    public string Status { get; set; } = "";

    /// <summary>Nhân viên đã bị xóa (xóa mềm).</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Cập nhật hồ sơ nhân viên theo dữ liệu hợp lệ. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool capNhatHoSo()
    {
        throw new ChuaTrienKhaiException("Employee.capNhatHoSo");
    }

    /// <summary>Kiểm tra trùng giấy tờ tùy thân khi quản lý nhân viên. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool kiemTraTrungGiayTo(string soGiayTo)
    {
        throw new ChuaTrienKhaiException("Employee.kiemTraTrungGiayTo");
    }
}
