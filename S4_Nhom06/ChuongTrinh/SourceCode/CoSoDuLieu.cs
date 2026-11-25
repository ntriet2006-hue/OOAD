using System.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Lớp hạ tầng chung: mở kết nối MySQL, tạo truy vấn tham số hóa và quản lý transaction; không phải Entity nghiệp vụ trong CD.
/// </summary>
sealed class CoSoDuLieu : IDisposable
{
    readonly MySqlConnection ketNoi;
    MySqlTransaction? giaoDich;
    /// <summary>
    /// Khởi tạo và mở kết nối bằng chuỗi cấu hình được truyền vào.
    /// </summary>
    public CoSoDuLieu(string chuoi) { ketNoi = new(chuoi); ketNoi.Open(); }
    /// <summary>
    /// Tạo lệnh gắn transaction hiện tại và tham số @p0, @p1...; giá trị null được đổi thành DBNull.
    /// </summary>
    MySqlCommand Lenh(string sql, object?[] args)
    {
        var cmd = new MySqlCommand(sql, ketNoi, giaoDich);
        for (int i=0;i<args.Length;i++) cmd.Parameters.AddWithValue("@p"+i,args[i] ?? DBNull.Value);
        return cmd;
    }
    /// <summary>
    /// Thực hiện truy vấn đọc, trả kết quả dạng DataTable.
    /// </summary>
    public DataTable Doc(string sql, params object?[] args)
    {
        using var cmd=Lenh(sql,args);
        using var reader=cmd.ExecuteReader();
        var table=new DataTable(); table.Load(reader); return table;
    }
    /// <summary>
    /// Thực hiện INSERT/UPDATE/DELETE; trả LastInsertedId khi thêm, không phải số dòng bị tác động.
    /// </summary>
    public long Ghi(string sql, params object?[] args)
    {
        using var cmd=Lenh(sql,args); cmd.ExecuteNonQuery(); return cmd.LastInsertedId;
    }
    /// <summary>
    /// Chạy một nhóm thao tác trong transaction ReadCommitted: thành công commit và trả kết quả, có ngoại lệ thì rollback.
    /// </summary>
    public T Luu<T>(Func<T> fn)
    {
        using var tx=ketNoi.BeginTransaction(IsolationLevel.ReadCommitted);
        giaoDich=tx;
        try { var kq=fn(); tx.Commit(); return kq; }
        catch { tx.Rollback(); throw; }
        finally { giaoDich=null; }
    }
    /// <summary>
    /// Đóng và giải phóng kết nối MySQL khi kết thúc sử dụng.
    /// </summary>
    public void Dispose()=>ketNoi.Dispose();
}
/// <summary>
/// Thông tin nhân viên của phiên đăng nhập để phân quyền/gắn người thao tác; khác Entity Employee đầy đủ trong khung CD.
/// </summary>
record NhanVien(int Id,string VaiTro);
