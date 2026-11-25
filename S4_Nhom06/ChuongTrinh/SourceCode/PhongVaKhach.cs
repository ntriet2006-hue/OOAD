using System.Data;
using System.Net.Mail;

/// <summary>
/// Các kiểm tra dữ liệu dùng chung cho hai phân hệ; không đại diện một bảng hoặc Entity trong CD.
/// </summary>
static class DuLieuNhap
{
    /// <summary>
    /// Kiểm tra số tiền là số nguyên đồng, không âm và không vượt giới hạn DECIMAL(12,0) của MySQL.
    /// </summary>
    public static void Tien(decimal value)
    {
        if (value < 0 || value > 999999999999m || decimal.Truncate(value) != value)
            throw new Exception("Số tiền phải là số nguyên từ 0 đến 999.999.999.999 đồng.");
    }
}

/// <summary>
/// Entity phòng khách sạn; record không có từ khóa struct là kiểu tham chiếu, hỗ trợ tạo bản sao bằng with.
/// Chứa các method CD kiểm tra lịch trống và cập nhật trạng thái phòng.
/// </summary>
sealed partial record Room(CoSoDuLieu Db, int Id, string RoomNumber, int Floor,
    string Status, string Description, bool IsDeleted, int Capacity, decimal BasePrice)
{
    /// <summary>
    /// Đọc Room cùng sức chứa/giá loại phòng và khóa bản ghi FOR UPDATE trong giao dịch để chống đặt trùng đồng thời.
    /// </summary>
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

    // Ngu canh chi dung tren ban sao Room cua lan sua, khong luu vao SQL.
    internal int MaDatPhongLoaiTru { get; init; }

    /// <summary>
    /// Method CD: trả false khi phòng đã xóa, bảo trì hoặc bị trùng lịch đặt/lưu trú trong khoảng yêu cầu.
    /// Khi sửa, ngữ cảnh trên bản sao Room loại trừ chính Booking đang sửa, không thay đổi chữ ký hai tham số trong CD.
    /// </summary>
    public bool kiemTraPhongTrong(DateTime tuNgay, DateTime denNgay)
    {
        if(IsDeleted || Status=="maintenance") return false;
        return Db.Doc("""
            SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
            WHERE d.room_id=@p0 AND b.id!=@p3 AND b.status!='cancelled'
              AND b.check_in_date<@p1 AND @p2<b.check_out_date LIMIT 1
            """,Id,denNgay,tuNgay,MaDatPhongLoaiTru).Rows.Count==0;
    }

    // SD UC4.1: Room cung cap ung vien, tung Room doi chieu lich dat qua method CD.
    /// <summary>
    /// Thao tác hỗ trợ SD UC4.1: lấy phòng ứng viên theo loại và gọi kiemTraPhongTrong() cho từng phòng.
    /// Trả mã/số phòng, loại, sức chứa, đơn giá và tạm tính bằng giá mỗi đêm nhân số đêm.
    /// </summary>
    internal static DataTable DanhSachPhongTrong(CoSoDuLieu db,YeuCau y,int maDatPhongLoaiTru=0)
    {
        var candidates=db.Doc("""
            SELECT r.id,r.room_number,r.floor,r.status,r.description,r.is_deleted,
              t.name,t.capacity,t.base_price
            FROM Room r JOIN RoomType t ON t.id=r.room_type_id
            WHERE r.is_deleted=0 AND r.status!='maintenance' AND (@p0='' OR t.name=@p0)
            ORDER BY r.room_number
            """,y.Loai);
        var result=new DataTable();
        result.Columns.Add("id",typeof(int));result.Columns.Add("Phòng",typeof(string));
        result.Columns.Add("Loại",typeof(string));result.Columns.Add("Sức chứa",typeof(int));
        result.Columns.Add("Đơn giá",typeof(decimal));result.Columns.Add("Tạm tính",typeof(decimal));
        foreach(DataRow row in candidates.Rows)
        {
            var room=new Room(db,(int)row["id"],(string)row["room_number"],(int)row["floor"],
                (string)row["status"],row["description"].ToString()!,Convert.ToBoolean(row["is_deleted"]),
                (int)row["capacity"],(decimal)row["base_price"]){MaDatPhongLoaiTru=maDatPhongLoaiTru};
            if(room.kiemTraPhongTrong(y.Nhan,y.Tra))
                result.Rows.Add(room.Id,room.RoomNumber,row["name"],room.Capacity,room.BasePrice,
                    room.BasePrice*(y.Tra-y.Nhan).Days);
        }
        return result;
    }

    /// <summary>
    /// Method CD: kiểm tra tên trạng thái và cập nhật Room; nếu khách vẫn chưa trả thì giữ occupied khi đặt cho tương lai.
    /// </summary>
    public bool capNhatTrangThai(string trangThai)
    {
        if(trangThai is not ("available" or "reserved" or "occupied" or "cleaning" or "maintenance"))
            throw new Exception("Trạng thái phòng không hợp lệ.");
        // Lịch đặt lưu trong BookingDetail. Khách chưa trả phòng vẫn đang sử dụng
        // phòng, kể cả khi có đặt phòng cho khoảng ngày tiếp theo.
        Db.Ghi("""
            UPDATE Room SET status=CASE
              WHEN @p0='reserved' AND EXISTS(
                SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
                WHERE d.room_id=Room.id AND b.status='checked_in'
                  AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL)
              THEN 'occupied' ELSE @p0 END
            WHERE id=@p1
            """,trangThai,Id);
        return true;
    }
}

/// <summary>
/// Entity hồ sơ khách hàng, dùng để đọc/tạo khách khi đặt phòng và kiểm tra trùng giấy tờ.
/// </summary>
sealed partial record Guest(CoSoDuLieu Db, int Id)
{
    /// <summary>
    /// Đọc đủ thông tin khách đã tồn tại; báo lỗi nếu mã khách không có trong database.
    /// </summary>
    public static Guest Doc(CoSoDuLieu db,int id)
    {
        var rows=db.Doc("SELECT * FROM Guest WHERE id=@p0",id).Rows;
        if(rows.Count==0) throw new Exception("Không tìm thấy khách hàng.");
        var row=rows[0];
        return new(db,id)
        {
            FullName=(string)row["full_name"],
            Gender=row["gender"].ToString()!,
            DateOfBirth=row["date_of_birth"] is DateTime birth ? birth : null,
            PhoneNumber=row["phone_number"].ToString()!,
            Email=row["email"].ToString()!,
            Address=row["address"].ToString()!,
            IdentityCard=row["identity_card"].ToString()!,
            Nationality=row["nationality"].ToString()!,
            GuestType=(string)row["guest_type"],
            CreatedAt=(DateTime)row["created_at"]
        };
    }

    /// <summary>
    /// Method CD: kiểm tra CCCD/hộ chiếu đã có trong hồ sơ để tránh tạo trùng khách theo giấy tờ.
    /// </summary>
    public bool kiemTraTrungGiayTo(string soGiayTo) =>
        Db.Doc("SELECT id FROM Guest WHERE identity_card=@p0",soGiayTo).Rows.Count>0;

    /// <summary>
    /// UC3.1 trong luồng đặt phòng: kiểm tra tên, giấy tờ, ngày sinh, email và độ dài rồi lưu hồ sơ mới.
    /// Dùng cùng transaction với Booking để không lưu khách lẻ nếu tạo đặt phòng thất bại.
    /// </summary>
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
            return Doc(db,checked((int)id));
        }
        catch(MySql.Data.MySqlClient.MySqlException e) when(e.Number==1062)
        {
            throw new Exception("Khách hàng đã tồn tại.");
        }
    }
}

/// <summary>
/// Phần bổ sung của cùng Entity Room, đồng bộ tình trạng sau khi sửa/hủy đặt phòng.
/// </summary>
sealed partial record Room
{
    /// <summary>
    /// Sau sửa/hủy, xác định lại occupied nếu còn khách ở, reserved nếu còn lịch pending/confirmed tương lai, ngược lại available.
    /// Giữ nguyên trạng thái bảo trì hoặc đang dọn phòng.
    /// </summary>
    internal void DongBoSauDatPhong()
    {
        if(Status is "maintenance" or "cleaning")return;
        var occupied=Db.Doc("SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id WHERE d.room_id=@p0 AND b.status='checked_in' AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL LIMIT 1",Id).Rows.Count>0;
        var reserved=Db.Doc("SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id WHERE d.room_id=@p0 AND b.status IN ('pending','confirmed') AND b.check_out_date>CURDATE() LIMIT 1",Id).Rows.Count>0;
        capNhatTrangThai(occupied?"occupied":reserved?"reserved":"available");
    }
}

/// <summary>
/// Phần bổ sung của Entity Guest để tạo thông tin liên hệ khách đặt trực tuyến.
/// </summary>
sealed partial record Guest
{
    /// <summary>
    /// Tạo hồ sơ bằng họ tên, điện thoại và email cho UC4.6; không bắt khách online nhập CCCD như luồng hồ sơ đầy đủ.
    /// </summary>
    public static Guest TaoLienHeTrucTuyen(CoSoDuLieu db,string name,string phone,string email)
    {
        var id=db.Ghi("INSERT INTO Guest(full_name,phone_number,email,guest_type,created_at) VALUES(@p0,@p1,@p2,'normal',NOW())",name,phone,email);
        return Doc(db,checked((int)id));
    }
}
