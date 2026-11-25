// Thuộc tính và chữ ký method theo mục 1.5.4 của báo cáo.
sealed partial class Account
{
    /// <summary>Mã tài khoản.</summary>
    public int Id { get; set; }

    /// <summary>Tên đăng nhập.</summary>
    public string Username { get; set; } = "";

    /// <summary>Mật khẩu (đã mã hóa).</summary>
    public string Password { get; set; } = "";

    /// <summary>Trạng thái tài khoản.</summary>
    public string Status { get; set; } = "";

    /// <summary>Thời điểm đăng nhập gần nhất.</summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>Xác thực thông tin đăng nhập của tài khoản. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool dangNhap(string username, string matKhau)
    {
        throw new ChuaTrienKhaiException("Account.dangNhap");
    }

    /// <summary>Đổi mật khẩu khi dữ liệu nhập hợp lệ. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool doiMatKhau(string matKhauCu, string matKhauMoi)
    {
        throw new ChuaTrienKhaiException("Account.doiMatKhau");
    }

    /// <summary>Khóa hoặc mở khóa tài khoản theo quyền quản trị. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public void capNhatTrangThai(string trangThai)
    {
        throw new ChuaTrienKhaiException("Account.capNhatTrangThai");
    }
}
