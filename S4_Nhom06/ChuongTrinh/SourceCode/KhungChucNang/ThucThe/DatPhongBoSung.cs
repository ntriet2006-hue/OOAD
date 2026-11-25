using System.Data;
using System.Net.Mail;
using System.Text;

// Nhan/tra phong thuoc UC5, chua trien khai. UC4 duoc xu ly trong KhungChucNang/ThucThe/DatPhongBoSung.cs.
sealed partial class Booking
{
    /// <summary>Thực hiện thủ tục nhận phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool nhanPhong()
    {
        throw new ChuaTrienKhaiException("Booking.nhanPhong");
    }

    /// <summary>Thực hiện thủ tục trả phòng. Khung theo CD, chưa triển khai nghiệp vụ.</summary>
    public bool traPhong()
    {
        throw new ChuaTrienKhaiException("Booking.traPhong");
    }
}

sealed partial class Booking
{
    int[] phongMoi=[];
    public static Booking DocDatPhong(CoSoDuLieu db,int id,bool khoa=false)
    {
        var rows=db.Doc("SELECT * FROM Booking WHERE id=@p0"+(khoa?" FOR UPDATE":""),id).Rows;
        if(rows.Count==0)throw new Exception("Không tìm thấy đặt phòng.");var r=rows[0];
        return new Booking(db,0){Id=id,BookingDate=(DateTime)r["booking_date"],CheckInDate=(DateTime)r["check_in_date"],CheckOutDate=(DateTime)r["check_out_date"],
          NumAdults=(int)r["num_adults"],NumChildren=Convert.ToInt32(r["num_children"]),Source=(string)r["source"],Deposit=Convert.ToDouble(r["deposit"]),
          Status=(string)r["status"],CancelReason=r["cancel_reason"].ToString(),CancelFee=r["cancel_fee"]==DBNull.Value?null:Convert.ToDouble(r["cancel_fee"]),Note=r["note"].ToString()!};
    }
    public void KiemTraChoSua(){if(Status is not ("pending" or "confirmed"))throw new Exception("Đặt phòng không còn ở trạng thái cho phép chỉnh sửa");}
    public void KiemTraChoHuy()
    {
        if(Status=="checked_in")throw new Exception("Không thể hủy đặt phòng đang lưu trú. Vui lòng thực hiện trả phòng");
        if(Status is not ("pending" or "confirmed"))throw new Exception("Đặt phòng không còn ở trạng thái cho phép hủy.");
    }
    internal void ChuanBiSua(YeuCau y,int[] ids,string note){CheckInDate=y.Nhan;CheckOutDate=y.Tra;NumAdults=y.Lon;NumChildren=y.Nho;phongMoi=ids.Distinct().Order().ToArray();Note=note.Trim();}
    int[] PhongCu()=>db.Doc("SELECT room_id FROM BookingDetail WHERE booking_id=@p0 ORDER BY room_id",Id).AsEnumerable().Select(r=>(int)r[0]).ToArray();
    // CD giu nguyen chu ky suaDatPhong(): boolean.
    public bool suaDatPhong()
    {
        KiemTraChoSua();if(phongMoi.Length==0)throw new Exception("Chọn ít nhất một phòng.");
        var old=PhongCu();var locked=old.Concat(phongMoi).Distinct().Order().Select(id=>Room.DocVaKhoa(db,id)).ToList();
        var selected=locked.Where(r=>phongMoi.Contains(r.Id)).ToList();
        foreach(var room in selected)
        {
            // Ban sao rieng loai tru lich cua chinh Booking, van goi dung method CD hai tham so.
            var phongDangKiemTra=room with { MaDatPhongLoaiTru=Id };
            if(!phongDangKiemTra.kiemTraPhongTrong(CheckInDate,CheckOutDate))
                throw new Exception("Phòng không còn trống trong thời gian đã chọn");
        }
        if(selected.Sum(r=>(long)r.Capacity)<(long)NumAdults+NumChildren)throw new Exception("Tổng sức chứa chưa đủ.");
        var total=selected.Sum(r=>r.BasePrice)*(CheckOutDate-CheckInDate).Days;DuLieuNhap.Tien(total);
        db.Ghi("UPDATE Booking SET check_in_date=@p0,check_out_date=@p1,num_adults=@p2,num_children=@p3,note=@p4 WHERE id=@p5",CheckInDate,CheckOutDate,NumAdults,NumChildren,Note,Id);
        // Giu id chi tiet phong cu, khong cat dut cac lien ket du lieu.
        foreach(var id in old.Except(phongMoi))db.Ghi("DELETE FROM BookingDetail WHERE booking_id=@p0 AND room_id=@p1",Id,id);
        foreach(var r in selected){if(old.Contains(r.Id))db.Ghi("UPDATE BookingDetail SET price_per_night=@p0 WHERE booking_id=@p1 AND room_id=@p2",r.BasePrice,Id,r.Id);
            else db.Ghi("INSERT INTO BookingDetail(booking_id,room_id,price_per_night) VALUES(@p0,@p1,@p2)",Id,r.Id,r.BasePrice);}
        foreach(var r in locked)r.DongBoSauDatPhong();return true;
    }
    internal void ChuanBiHuy(string lyDo){CancelReason=lyDo;CancelFee=0;}
    // Chinh sach demo do nguoi dung chon: mien phi huy, hoan toan bo coc.
    public bool huyDatPhong()
    {
        KiemTraChoHuy();var rooms=PhongCu().Select(id=>Room.DocVaKhoa(db,id)).ToList();CancelFee=0;
        db.Ghi("UPDATE Booking SET status='cancelled',cancel_reason=@p0,cancel_fee=0 WHERE id=@p1",CancelReason,Id);Status="cancelled";
        foreach(var r in rooms)r.DongBoSauDatPhong();return true;
    }
    public object KetQuaHuy()=>new{id=Id,fee=0m,refund=(decimal)Deposit,reason=CancelReason,policy="Miễn phí hủy, hoàn toàn bộ tiền cọc. Số tiền hiển thị là khoản cần hoàn, chưa thực hiện chuyển tiền."};
    public object ThongTin()
    {
        var d=db.Doc("SELECT d.room_id AS id,r.room_number AS number,d.price_per_night AS price,t.capacity,t.name AS type FROM BookingDetail d JOIN Room r ON r.id=d.room_id JOIN RoomType t ON t.id=r.room_type_id WHERE d.booking_id=@p0 ORDER BY r.id",Id);
        var g=db.Doc("SELECT g.full_name FROM Booking b JOIN Guest g ON g.id=b.guest_id WHERE b.id=@p0",Id);
        return new{id=Id,guest=g.Rows[0][0],checkIn=CheckInDate.ToString("yyyy-MM-dd"),checkOut=CheckOutDate.ToString("yyyy-MM-dd"),adults=NumAdults,children=NumChildren,source=Source,deposit=Deposit,status=Status,note=Note,cancelReason=CancelReason,cancelFee=CancelFee,
            total=d.AsEnumerable().Sum(r=>(decimal)r["price"])*(CheckOutDate-CheckInDate).Days,
            rooms=d.AsEnumerable().Select(r=>new{id=(int)r["id"],number=(string)r["number"],price=(decimal)r["price"],capacity=(int)r["capacity"],type=(string)r["type"]}).ToArray()};
    }
    public static DataTable TimKiem(CoSoDuLieu db,string word,DateTime? from,DateTime? to,string status)=>db.Doc("""
        SELECT b.id AS 'Mã đặt',g.full_name AS 'Khách',GROUP_CONCAT(r.room_number ORDER BY r.room_number) AS 'Phòng',
          b.check_in_date AS 'Ngày nhận',b.check_out_date AS 'Ngày trả',
          CASE b.status WHEN 'pending' THEN 'Chờ xác nhận' WHEN 'confirmed' THEN 'Đã xác nhận' WHEN 'checked_in' THEN 'Đang lưu trú' WHEN 'checked_out' THEN 'Đã trả phòng' ELSE 'Đã hủy' END AS 'Trạng thái'
        FROM Booking b JOIN Guest g ON g.id=b.guest_id JOIN BookingDetail d ON d.booking_id=b.id JOIN Room r ON r.id=d.room_id
        WHERE (@p0='' OR CAST(b.id AS CHAR)=@p0 OR CONCAT('DP',LPAD(b.id,4,'0'))=@p0 OR LOCATE(@p0,g.full_name)>0
          OR EXISTS(SELECT 1 FROM BookingDetail x JOIN Room xr ON xr.id=x.room_id WHERE x.booking_id=b.id AND LOCATE(@p0,xr.room_number)>0))
          AND (@p1 IS NULL OR b.check_out_date>=@p1) AND (@p2 IS NULL OR b.check_in_date<=@p2) AND (@p3='' OR b.status=@p3)
        GROUP BY b.id ORDER BY b.id DESC
        """,word,from,to,status);
    public string LuuEmailXacNhan(string email,string name)
    {
        var folder=Environment.GetEnvironmentVariable("OOAD_EMAIL_DIR")??Path.Combine(AppContext.BaseDirectory,"ThuXacNhan");folder=Path.Combine(folder,$"DP{Id:D4}");Directory.CreateDirectory(folder);
        var before=Directory.GetFiles(folder,"*.eml").ToHashSet();
        using var message=new MailMessage("datphong@khachsan.example",email){Subject=$"Xác nhận yêu cầu đặt phòng DP{Id:D4}",SubjectEncoding=Encoding.UTF8,BodyEncoding=Encoding.UTF8,
          Body=$"Xin chào {name},\nYêu cầu đặt phòng DP{Id:D4} đã được lưu, đang chờ xác nhận của lễ tân.\nNgày nhận: {CheckInDate:dd/MM/yyyy}\nNgày trả: {CheckOutDate:dd/MM/yyyy}\nSố phòng: {rooms.Count}\nĐây là email lưu cục bộ phục vụ demo, chưa gửi qua Internet."};
        using var smtp=new SmtpClient{DeliveryMethod=SmtpDeliveryMethod.SpecifiedPickupDirectory,PickupDirectoryLocation=Path.GetFullPath(folder)};smtp.Send(message);
        return Path.GetFileName(Directory.GetFiles(folder,"*.eml").Except(before).FirstOrDefault()??"");
    }
}