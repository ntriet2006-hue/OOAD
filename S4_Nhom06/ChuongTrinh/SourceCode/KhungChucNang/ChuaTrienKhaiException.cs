/// <summary>Phần khung chưa có xử lý dữ liệu và chưa được nối vào giao diện web.</summary>
sealed class ChuaTrienKhaiException(string chucNang)
    : NotImplementedException($"Chức năng {chucNang} mới có khung, chưa triển khai.")
{
}
