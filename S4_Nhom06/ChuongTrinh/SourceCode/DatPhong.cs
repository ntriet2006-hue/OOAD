using System.Data;

record YeuCau(DateTime Nhan, DateTime Tra, int Lon, int Nho, string Loai);

sealed class DieuKhienDatPhong(CoSoDuLieu db, NhanVien nv)
{
    public DataTable Khach() => db.Doc("SELECT id,CONCAT(full_name,' · ',COALESCE(identity_card,'')) AS ten FROM Guest ORDER BY full_name");
    public DataTable Loai() => db.Doc("SELECT name FROM RoomType ORDER BY name");
    public DataTable LichSu() => db.Doc("""
        SELECT b.id AS 'Mã đặt',g.full_name AS 'Khách',
        GROUP_CONCAT(r.room_number ORDER BY r.room_number) AS 'Phòng',
        b.check_in_date AS 'Ngày nhận',b.check_out_date AS 'Ngày trả',
        CASE b.status WHEN 'confirmed' THEN 'Đã xác nhận'
          WHEN 'checked_in' THEN 'Đang lưu trú' WHEN 'checked_out' THEN 'Đã trả phòng'
          WHEN 'cancelled' THEN 'Đã hủy' ELSE 'Chờ xác nhận' END AS 'Trạng thái'
        FROM Booking b JOIN Guest g ON g.id=b.guest_id
        JOIN BookingDetail d ON d.booking_id=b.id JOIN Room r ON r.id=d.room_id
        GROUP BY b.id ORDER BY b.id DESC
        """);

    public DataTable Tim(YeuCau y)
    {
        KiemTraYeuCau(y);
        return db.Doc("""
            SELECT r.id,r.room_number AS 'Phòng',t.name AS 'Loại',t.capacity AS 'Sức chứa',
            t.base_price AS 'Đơn giá',t.base_price*@p0 AS 'Tạm tính'
            FROM Room r JOIN RoomType t ON t.id=r.room_type_id
            WHERE r.is_deleted=0 AND r.status!='maintenance' AND (@p1='' OR t.name=@p1)
            AND NOT EXISTS(SELECT 1 FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id
              WHERE d.room_id=r.id AND b.status!='cancelled'
                AND b.check_in_date<@p2 AND @p3<b.check_out_date)
            ORDER BY r.room_number
            """, (y.Tra-y.Nhan).Days, y.Loai, y.Tra, y.Nhan);
    }

    internal static void KiemTraYeuCau(YeuCau y)
    {
        if (y.Nhan != y.Nhan.Date || y.Tra != y.Tra.Date ||
            y.Nhan < DateTime.Today || y.Tra <= y.Nhan)
            throw new Exception("Khoảng thời gian lưu trú không hợp lệ");
        if(y.Lon < 1 || y.Nho < 0)
            throw new Exception("Số khách không hợp lệ.");
    }

    public long Tao(YeuCau y, int[] ids, int guest, string[] profile, decimal coc, string nguon, string note)
    {
        if (nv.VaiTro is not ("Quản lý" or "Lễ tân"))
            throw new Exception("Không có quyền tạo đặt phòng.");
        KiemTraYeuCau(y);
        DuLieuNhap.Tien(coc);
        if (nguon is not ("walk_in" or "phone") || note.Length > 255)
            throw new Exception("Nguồn đặt phòng hoặc ghi chú không hợp lệ.");

        return db.Luu(() =>
        {
            var rooms = ids.Distinct().Order().Select(id => Room.DocVaKhoa(db, id)).ToList();
            if (rooms.Count == 0) throw new Exception("Chọn ít nhất một phòng.");
            var minimum = decimal.Parse(db.Doc(
                "SELECT setting_value FROM Setting WHERE setting_key='min_deposit'").Rows[0][0].ToString()!);
            if (coc < minimum) throw new Exception("Tiền đặt cọc chưa đạt mức tối thiểu.");
            foreach (var room in rooms)
                if (!room.kiemTraPhongTrong(y.Nhan, y.Tra))
                    throw new Exception("Phòng đã được đặt. Vui lòng chọn phòng khác.");
            if (rooms.Sum(r => (long)r.Capacity) < (long)y.Lon+y.Nho)
                throw new Exception("Tổng sức chứa chưa đủ.");

            // Hồ sơ mới cùng giao dịch: tạo đặt phòng thất bại thì không lưu khách lẻ.
            var khach = guest == 0 ? Guest.Tao(db, profile) : Guest.Doc(db, guest);
            var booking = new Booking(db, khach, nv, y, rooms, coc, nguon, note);
            if (!booking.taoDatPhong()) throw new Exception("Không thể tạo đặt phòng.");
            return booking.Id;
        });
    }
}

sealed partial class Booking
{
    readonly CoSoDuLieu db;
    readonly int detail;
    readonly Guest? guest;
    readonly NhanVien? employee;
    readonly IReadOnlyList<Room> rooms = [];

    public int Id { get; private set; }
    public DateTime BookingDate { get; private set; }
    public DateTime CheckInDate { get; private set; }
    public DateTime CheckOutDate { get; private set; }
    public int NumAdults { get; private set; }
    public int NumChildren { get; private set; }
    public string Source { get; private set; } = "";
    public double Deposit { get; private set; }
    public string Status { get; private set; } = "";
    public string? CancelReason { get; private set; }
    public double? CancelFee { get; private set; }
    public string Note { get; private set; } = "";

    public Booking(CoSoDuLieu db, int detail)
    {
        this.db = db;
        this.detail = detail;
    }

    public Booking(CoSoDuLieu db, Guest guest, NhanVien employee, YeuCau y,
        IReadOnlyList<Room> rooms, decimal deposit, string source, string note)
    {
        this.db=db; this.guest=guest; this.employee=employee; this.rooms=rooms;
        CheckInDate=y.Nhan; CheckOutDate=y.Tra; NumAdults=y.Lon; NumChildren=y.Nho;
        Deposit=(double)deposit; Source=source; Note=note; Status="confirmed";
    }

    // CD Booking.taoDatPhong(): boolean. Control đã mở giao dịch và khóa phòng.
    public bool taoDatPhong()
    {
        if (guest is null || employee is null || rooms.Count == 0 || Id != 0) return false;
        foreach (var room in rooms)
            if (!room.kiemTraPhongTrong(CheckInDate, CheckOutDate))
                throw new Exception("Phòng đã được đặt. Vui lòng chọn phòng khác.");

        BookingDate=DateTime.Now;
        Id=checked((int)db.Ghi("""
            INSERT INTO Booking(guest_id,employee_id,booking_date,check_in_date,check_out_date,
              num_adults,num_children,source,deposit,status,note)
            VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)
            """, guest.Id,employee.Id,BookingDate,CheckInDate,CheckOutDate,
            NumAdults,NumChildren,Source,(decimal)Deposit,Status,Note));
        foreach (var room in rooms)
        {
            db.Ghi("INSERT INTO BookingDetail(booking_id,room_id,price_per_night) VALUES(@p0,@p1,@p2)",
                Id,room.Id,room.BasePrice);
            room.capNhatTrangThai("reserved");
        }
        return Id > 0;
    }
}
