using System.Data;
using MySql.Data.MySqlClient;

sealed record Service(CoSoDuLieu Db, int Id, string Name,string Category,string Unit,double Price,string Description,string Status)
{
    public static DataTable DanhSach(CoSoDuLieu db,bool dangCungCap=false) =>
        db.Doc("SELECT id,name,category,unit,price,description,status FROM Service "+(dangCungCap?"WHERE status='active' ":"")+"ORDER BY name");
    public static Service Doc(CoSoDuLieu db,int id,bool khoa=false)
    {
        var rows=db.Doc("SELECT * FROM Service WHERE id=@p0"+(khoa?" FOR UPDATE":""),id).Rows;
        if(rows.Count==0) throw new Exception("Không tìm thấy dịch vụ.");
        var r=rows[0];
        return new(db,id,(string)r["name"],r["category"].ToString()!,r["unit"].ToString()!,Convert.ToDouble(r["price"]),r["description"].ToString()!,(string)r["status"]);
    }
    // SD thêm dịch vụ: kiểm tra trùng tên trước khi lưu.
    public void Them()
    {
        if(Db.Doc("SELECT id FROM Service WHERE name=@p0",Name).Rows.Count>0)
            throw new Exception("Dịch vụ đã tồn tại.");
        Db.Ghi("INSERT INTO Service(name,category,unit,price,description,status) VALUES(@p0,@p1,@p2,@p3,@p4,@p5)",Name,Category,Unit,(decimal)Price,Description,Status);
    }
    public void capNhatThongTin()=>Db.Ghi("UPDATE Service SET name=@p0,category=@p1,unit=@p2,price=@p3,description=@p4,status=@p5 WHERE id=@p6",Name,Category,Unit,(decimal)Price,Description,Status,Id);
    public bool kiemTraConCungCap()=>Status=="active";
    public void Xoa()
    {
        if(Db.Doc("SELECT id FROM ServiceUsage WHERE service_id=@p0 LIMIT 1",Id).Rows.Count>0)
            throw new Exception("Không thể xóa dịch vụ này. Có thể chuyển sang Ngừng cung cấp.");
        Db.Ghi("DELETE FROM Service WHERE id=@p0",Id);
    }
}
sealed class ServiceUsage(CoSoDuLieu db,int detail,Service service,int employee,int quantity)
{
    public long Id {get; private set;}
    public int Quantity {get; private set;} = quantity > 0 ? quantity : throw new Exception("Số lượng phải lớn hơn 0.");
    public double UnitPrice {get;}=service.Price;
    public DateTime UsedAt {get;}=DateTime.Now;
    public bool ghiNhanSuDung(int soLuong)
    {
        if(soLuong<=0) throw new Exception("Số lượng phải lớn hơn 0.");
        Quantity=soLuong;
        Id=db.Ghi("INSERT INTO ServiceUsage(booking_detail_id,service_id,employee_id,quantity,unit_price,used_at) VALUES(@p0,@p1,@p2,@p3,@p4,@p5)",detail,service.Id,employee,Quantity,(decimal)UnitPrice,UsedAt);
        return Id>0;
    }
    public double tinhThanhTien()=>Quantity*UnitPrice;
}
sealed class DieuKhienDichVu(CoSoDuLieu db,NhanVien nv)
{
    void QuyenQuanLy(){if(nv.VaiTro!="Quản lý") throw new Exception("Chỉ Quản lý được thay đổi danh mục.");}
    public Service ChiTiet(int id)=>Service.Doc(db,id);
    public DataTable DanhSach(bool active=false)=>Service.DanhSach(db,active);
    public void Luu(int id,string name,string category,string unit,decimal price,string description,string status)
    {
        QuyenQuanLy();
        name=name.Trim();
        category=category.Trim(); unit=unit.Trim(); description=description.Trim();
        DuLieuNhap.Tien(price);
        if(name.Length is 0 or >50 || category.Length>30 || unit.Length>20 || price<0 || description.Length>255 || status is not ("active" or "inactive"))
            throw new Exception("Thông tin dịch vụ không hợp lệ.");
        var service=new Service(db,id,name,category,unit,(double)price,description,status);
        try { db.Luu(()=>{if(id==0) service.Them(); else {Service.Doc(db,id,true);service.capNhatThongTin();} return true;}); }
        catch(MySqlException e) when(e.Number==1062){throw new Exception("Dịch vụ đã tồn tại.");}
    }
    public void Xoa(int id){QuyenQuanLy(); db.Luu(()=>{Service.Doc(db,id,true).Xoa();return true;});}
    public DataTable PhongDangO()=>db.Doc("SELECT d.id,CONCAT(r.room_number,' · ',g.full_name) AS ten FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id JOIN Room r ON r.id=d.room_id JOIN Guest g ON g.id=b.guest_id WHERE b.status='checked_in' AND r.status='occupied' AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL");
    public DataTable LichSu(int detail)=>db.Doc("SELECT s.name AS 'Dịch vụ',u.quantity AS 'Số lượng',u.unit_price AS 'Đơn giá',u.quantity*u.unit_price AS 'Thành tiền' FROM ServiceUsage u JOIN Service s ON s.id=u.service_id WHERE booking_detail_id=@p0 ORDER BY u.id DESC",detail);
    public (double Dong,decimal Tong) GhiNhan(int detail,int key,int qty)=>db.Luu(()=>{
        if(nv.VaiTro is not ("Quản lý" or "Lễ tân")) throw new Exception("Không có quyền ghi nhận dịch vụ.");
        var booking=new Booking(db,detail);
        booking.KiemTraDangO();
        var service=Service.Doc(db,key,true);
        if(!service.kiemTraConCungCap()) throw new Exception("Dịch vụ hiện không được cung cấp.");
        var usage=new ServiceUsage(db,detail,service,nv.Id,qty);
        // SD mô tả bước tính tiền trong lifeline Dịch vụ.
        // CD giao method tinhThanhTien cho ServiceUsage, nên gọi thực thể này.
        // Tính trước khi ghi nhận vào đặt phòng theo thứ tự SD.
        var thanhTien=usage.tinhThanhTien();
        var tong=booking.GhiNhan(usage,qty);
        return (thanhTien,tong);
    });
}
sealed partial class Booking
{
    public void KiemTraDangO()
    {
        if(db.Doc("SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id JOIN Room r ON r.id=d.room_id WHERE d.id=@p0 AND b.status='checked_in' AND r.status='occupied' AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL FOR UPDATE",detail).Rows.Count==0)
            throw new Exception("Chỉ ghi nhận dịch vụ cho phòng đang lưu trú.");
    }
    public decimal GhiNhan(ServiceUsage usage,int qty)
    {
        usage.ghiNhanSuDung(qty);
        return Convert.ToDecimal(db.Doc("SELECT SUM(u.quantity*u.unit_price) FROM ServiceUsage u JOIN BookingDetail d ON d.id=u.booking_detail_id WHERE d.booking_id=(SELECT booking_id FROM BookingDetail WHERE id=@p0)",detail).Rows[0][0]);
    }
}
