using System.Data;
record YeuCau(DateTime Nhan,DateTime Tra,int Lon,int Nho,string Loai);
sealed class DieuKhienDatPhong(CoSoDuLieu db,NhanVien nv)
{
    public DataTable Khach()=>db.Doc("SELECT id,CONCAT(full_name,' · ',COALESCE(identity_card,'')) AS ten FROM Guest ORDER BY full_name");
    public DataTable Loai()=>db.Doc("SELECT name FROM RoomType ORDER BY name");
    public DataTable LichSu()=>db.Doc("SELECT b.id AS 'Mã đặt',g.full_name AS 'Khách',GROUP_CONCAT(r.room_number ORDER BY r.room_number) AS 'Phòng',b.check_in_date AS 'Ngày nhận',b.check_out_date AS 'Ngày trả',CASE b.status WHEN 'confirmed' THEN 'Đã xác nhận' ELSE b.status END AS 'Trạng thái' FROM Booking b JOIN Guest g ON g.id=b.guest_id JOIN BookingDetail d ON d.booking_id=b.id JOIN Room r ON r.id=d.room_id GROUP BY b.id ORDER BY b.id DESC");
    public DataTable Tim(YeuCau y)
    {
        if(y.Nhan<DateTime.Today || y.Tra<=y.Nhan || y.Lon<1 || y.Nho<0) throw new Exception("Kiểm tra ngày lưu trú và số khách.");
        return db.Doc(@"SELECT r.id,r.room_number AS 'Phòng',t.name AS 'Loại',t.capacity AS 'Sức chứa',t.base_price AS 'Đơn giá',t.base_price*@p0 AS 'Tạm tính'
          FROM Room r JOIN RoomType t ON t.id=r.room_type_id WHERE r.is_deleted=0 AND r.status!='maintenance' AND (@p1='' OR t.name=@p1)
          AND NOT EXISTS(SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id WHERE d.room_id=r.id AND b.status!='cancelled' AND b.check_in_date<@p2 AND @p3<b.check_out_date) ORDER BY r.room_number",(y.Tra-y.Nhan).Days,y.Loai,y.Tra,y.Nhan);
    }
    public long Tao(YeuCau y,int[] ids,int guest,string[] profile,int coc,string nguon,string note)=>db.Luu(()=>{
        var rooms=ids.Distinct().Order().ToArray();
        if(rooms.Length==0) throw new Exception("Chọn ít nhất một phòng.");
        foreach(int id in rooms) db.Doc("SELECT id FROM Room WHERE id=@p0 FOR UPDATE",id);
        var found=Tim(y).AsEnumerable().Where(r=>rooms.Contains((int)r["id"])).ToArray();
        if(found.Length!=rooms.Length) throw new Exception("Phòng đã được đặt. Vui lòng chọn phòng khác.");
        if(found.Sum(r=>(int)r["Sức chứa"])<y.Lon+y.Nho) throw new Exception("Tổng sức chứa chưa đủ.");
        if(coc<Convert.ToInt32(db.Doc("SELECT value FROM Setting WHERE setting_key='min_deposit'").Rows[0][0])) throw new Exception("Tiền đặt cọc chưa đạt mức tối thiểu.");
        if(guest==0){
            if(profile.Length!=9 || string.IsNullOrWhiteSpace(profile[0]) || string.IsNullOrWhiteSpace(profile[1])) throw new Exception("Nhập tên và giấy tờ của khách mới.");
            if(profile[4]!="" && (!DateTime.TryParse(profile[4],out var birth)||birth>DateTime.Today)) throw new Exception("Ngày sinh không hợp lệ.");
            try {guest=(int)db.Ghi("INSERT INTO Guest(full_name,identity_card,phone_number,gender,date_of_birth,email,address,nationality,guest_type) VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)",profile);}
            catch(MySql.Data.MySqlClient.MySqlException ex) when(ex.Number==1062){throw new Exception("Khách hàng đã tồn tại.");}
        }
        var key=db.Ghi("INSERT INTO Booking(guest_id,employee_id,check_in_date,check_out_date,num_adults,num_children,source,deposit,status,note) VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,'confirmed',@p8)",guest,nv.Id,y.Nhan,y.Tra,y.Lon,y.Nho,nguon,coc,note);
        foreach(var room in found){
            db.Ghi("INSERT INTO BookingDetail(booking_id,room_id,price_per_night) VALUES(@p0,@p1,@p2)",key,room["id"],room["Đơn giá"]);
            db.Ghi("UPDATE Room SET status='reserved' WHERE id=@p0 AND status='available'",room["id"]);
        }
        return key;
    });
}
