using System.Data;
using MySql.Data.MySqlClient;

sealed class CoSoDuLieu : IDisposable
{
    readonly MySqlConnection ketNoi;
    MySqlTransaction? giaoDich;
    public CoSoDuLieu(string chuoi) { ketNoi = new(chuoi); ketNoi.Open(); }
    MySqlCommand Lenh(string sql, object?[] args)
    {
        var cmd = new MySqlCommand(sql, ketNoi, giaoDich);
        for (int i=0;i<args.Length;i++) cmd.Parameters.AddWithValue("@p"+i,args[i] ?? DBNull.Value);
        return cmd;
    }
    public DataTable Doc(string sql, params object?[] args)
    {
        using var cmd=Lenh(sql,args);
        using var reader=cmd.ExecuteReader();
        var table=new DataTable(); table.Load(reader); return table;
    }
    public long Ghi(string sql, params object?[] args)
    {
        using var cmd=Lenh(sql,args); cmd.ExecuteNonQuery(); return cmd.LastInsertedId;
    }
    public T Luu<T>(Func<T> fn)
    {
        using var tx=ketNoi.BeginTransaction(IsolationLevel.ReadCommitted);
        giaoDich=tx;
        try { var kq=fn(); tx.Commit(); return kq; }
        catch { tx.Rollback(); throw; }
        finally { giaoDich=null; }
    }
    public void Dispose()=>ketNoi.Dispose();
}
record NhanVien(int Id,string VaiTro);
