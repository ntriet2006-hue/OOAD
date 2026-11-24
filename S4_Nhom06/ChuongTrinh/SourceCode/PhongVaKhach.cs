using System.Net.Mail;

static class DuLieuNhap
{
    public static void Tien(decimal value)
    {
        if (value < 0 || value > 999999999999m || decimal.Truncate(value) != value)
            throw new Exception("Số tiền phải là số nguyên từ 0 đến 999.999.999.999 đồng.");
    }
}

sealed record Room(CoSoDuLieu Db, int Id, string RoomNumber, int Floor,
    string Status, string Description, bool IsDeleted, int Capacity, decimal BasePrice)
{
    public static Room DocVaKhoa(CoSoDuLieu db, int id)
    {
        var rows=db.Doc("""
            SELECT r.*,t.capacity,t.base_price FROM Room r
            JOIN RoomType t ON t.id=r.room_type_id WHERE r.id=@p0 FOR UPDATE
            """,id).Rows;
        if(rows.Count==0) throw new Exception("Không tìm thấy phòng.");
        var r=rows[0];
        return new(db,id,(string)r["room_number"],(int)r["floor"],(string)r["status"],
            r["description"].ToString()!,Convert.ToBoolean(r["is_deleted"]),
            (int)r["capacity"],(decimal)r["base_price"]);
    }

    public bool kiemTraPhongTrong(DateTime tuNgay, DateTime denNgay)
    {
        if(IsDeleted || Status=="maintenance") return false;
        return Db.Doc("""
            SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
            WHERE d.room_id=@p0 AND b.status!='cancelled'
              AND b.check_in_date<@p1 AND @p2<b.check_out_date LIMIT 1
            """,Id,denNgay,tuNgay).Rows.Count==0;
    }

    public bool capNhatTrangThai(string trangThai)
    {
        if(trangThai is not ("available" or "reserved" or "occupied" or "cleaning" or "maintenance"))
            throw new Exception("Trạng thái phòng không hợp lệ.");
        // Theo UC4.2, phòng được chọn chuyển sang Đã đặt.
        Db.Ghi("UPDATE Room SET status=@p0 WHERE id=@p1",trangThai,Id);
        return true;
    }
}

sealed record Guest(CoSoDuLieu Db, int Id)
{
    public static Guest Doc(CoSoDuLieu db,int id)
    {
        if(db.Doc("SELECT id FROM Guest WHERE id=@p0",id).Rows.Count==0)
            throw new Exception("Không tìm thấy khách hàng.");
        return new(db,id);
    }

    public bool kiemTraTrungGiayTo(string soGiayTo) =>
        Db.Doc("SELECT id FROM Guest WHERE identity_card=@p0",soGiayTo).Rows.Count>0;

    public static Guest Tao(CoSoDuLieu db,string[] profile)
    {
        if(profile.Length!=9) throw new Exception("Nhập đầy đủ hồ sơ khách mới.");
        var p=profile.Select(s=>s.Trim()).ToArray();
        int[] limits=[50,20,15,6,10,50,100,30,6];
        if(p.Where((s,i)=>s.Length>limits[i]).Any() || p[0]=="" || p[1]=="")
            throw new Exception("Hồ sơ khách thiếu tên, giấy tờ hoặc vượt độ dài cho phép.");
        if(p[3] is not ("" or "male" or "female") || p[8] is not ("normal" or "vip"))
            throw new Exception("Giới tính hoặc loại khách không hợp lệ.");
        DateTime? birth=null;
        if(p[4]!="")
        {
            if(!DateTime.TryParseExact(p[4],"yyyy-MM-dd",null,
                System.Globalization.DateTimeStyles.None,out var value) || value>DateTime.Today)
                throw new Exception("Ngày sinh không hợp lệ.");
            birth=value;
        }
        if(p[5]!="" && (!MailAddress.TryCreate(p[5],out var email) || email.Address!=p[5]))
            throw new Exception("Email không hợp lệ.");
        if(new Guest(db,0).kiemTraTrungGiayTo(p[1])) throw new Exception("Khách hàng đã tồn tại.");
        try
        {
            var id=db.Ghi("""
                INSERT INTO Guest(full_name,identity_card,phone_number,gender,date_of_birth,
                  email,address,nationality,guest_type,created_at)
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)
                """,p[0],p[1],p[2],p[3]==""?null:p[3],birth,p[5],p[6],p[7],p[8],DateTime.Now);
            return new(db,checked((int)id));
        }
        catch(MySql.Data.MySqlClient.MySqlException e) when(e.Number==1062)
        {
            throw new Exception("Khách hàng đã tồn tại.");
        }
    }
}
