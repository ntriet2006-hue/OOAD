using System.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Entity danh mục dịch vụ: tên, nhóm, đơn vị, giá hiện tại, mô tả và trạng thái. Record không có struct là kiểu tham chiếu.
/// </summary>
sealed record Service(CoSoDuLieu Db, int Id, string Name,string Category,string Unit,double Price,string Description,string Status)
{
    /// <summary>
    /// Đọc danh mục; dangCungCap=true chỉ lấy dịch vụ active để chọn khi ghi nhận sử dụng.
    /// </summary>
    public static DataTable DanhSach(CoSoDuLieu db,bool dangCungCap=false) =>
        db.Doc("SELECT id,name,category,unit,price,description,status FROM Service "+(dangCungCap?"WHERE status='active' ":"")+"ORDER BY name");
    /// <summary>
    /// Đọc một dịch vụ; khoa=true khóa bản ghi trong transaction trước khi sửa, xóa hoặc ghi nhận giá sử dụng.
    /// </summary>
    public static Service Doc(CoSoDuLieu db,int id,bool khoa=false)
    {
        var rows=db.Doc("SELECT * FROM Service WHERE id=@p0"+(khoa?" FOR UPDATE":""),id).Rows;
        if(rows.Count==0) throw new Exception("Không tìm thấy dịch vụ.");
        var r=rows[0];
        return new(db,id,(string)r["name"],r["category"].ToString()!,r["unit"].ToString()!,Convert.ToDouble(r["price"]),r["description"].ToString()!,(string)r["status"]);
    }
    // SD thêm dịch vụ: kiểm tra trùng tên trước khi lưu.
    /// <summary>
    /// UC6.1: kiểm tra trùng tên rồi thêm dịch vụ; Control đã kiểm tra quyền và dữ liệu nhập.
    /// </summary>
    public void Them()
    {
        if(Db.Doc("SELECT id FROM Service WHERE name=@p0",Name).Rows.Count>0)
            throw new Exception("Dịch vụ đã tồn tại.");
        Db.Ghi("INSERT INTO Service(name,category,unit,price,description,status) VALUES(@p0,@p1,@p2,@p3,@p4,@p5)",Name,Category,Unit,(decimal)Price,Description,Status);
    }
    /// <summary>
    /// Method CD cho UC6.2: cập nhật tên, nhóm, đơn vị, giá, mô tả và trạng thái của dịch vụ.
    /// </summary>
    public void capNhatThongTin()=>Db.Ghi("UPDATE Service SET name=@p0,category=@p1,unit=@p2,price=@p3,description=@p4,status=@p5 WHERE id=@p6",Name,Category,Unit,(decimal)Price,Description,Status,Id);
    /// <summary>
    /// Method CD: trả true khi dịch vụ active; Control chặn ghi nhận nếu dịch vụ đã ngừng cung cấp.
    /// </summary>
    public bool kiemTraConCungCap()=>Status=="active";
    // SD 3.1.45: lifeline Dich vu tinh thanh tien truoc khi ghi vao Dat phong.
    /// <summary>
    /// Bước tính tiền tại lifeline Service trong SD UC6.4: số lượng dương nhân đơn giá hiện tại.
    /// Đây là method bổ sung theo yêu cầu ưu tiên SD; CD hiện liệt kê method tính tiền không tham số ở ServiceUsage.
    /// </summary>
    public double tinhThanhTien(int soLuong)
    {
        if(soLuong<=0)throw new Exception("Số lượng phải lớn hơn 0.");
        return soLuong*Price;
    }
    /// <summary>
    /// UC6.3: chặn xóa nếu đã có ServiceUsage để giữ lịch sử; chưa dùng thì xóa khỏi danh mục.
    /// </summary>
    public void Xoa()
    {
        if(Db.Doc("SELECT id FROM ServiceUsage WHERE service_id=@p0 LIMIT 1",Id).Rows.Count>0)
            throw new Exception("Không thể xóa dịch vụ này. Có thể chuyển sang Ngừng cung cấp.");
        Db.Ghi("DELETE FROM Service WHERE id=@p0",Id);
    }
}
/// <summary>
/// Entity một lần dùng dịch vụ của một BookingDetail; giữ số lượng, đơn giá tại thời điểm dùng và thời gian sử dụng.
/// Giá được chụp khi tạo đối tượng nên sửa giá danh mục không đổi tiền lịch sử đã ghi.
/// </summary>
sealed class ServiceUsage(CoSoDuLieu db,int detail,Service service,int employee,int quantity)
{
    public int Id {get; private set;}
    public int Quantity {get; private set;} = quantity > 0 ? quantity : throw new Exception("Số lượng phải lớn hơn 0.");
    public double UnitPrice {get;}=service.Price;
    public DateTime UsedAt {get;}=DateTime.Now;
    /// <summary>
    /// Method CD: kiểm tra số lượng dương, lưu lần dùng gắn BookingDetail/Service/nhân viên và trả true khi có mã mới.
    /// </summary>
    public bool ghiNhanSuDung(int soLuong)
    {
        if(soLuong<=0) throw new Exception("Số lượng phải lớn hơn 0.");
        Quantity=soLuong;
        Id=checked((int)db.Ghi("INSERT INTO ServiceUsage(booking_detail_id,service_id,employee_id,quantity,unit_price,used_at) VALUES(@p0,@p1,@p2,@p3,@p4,@p5)",detail,service.Id,employee,Quantity,(decimal)UnitPrice,UsedAt));
        return Id>0;
    }
    /// <summary>
    /// Method CD: tính số lượng nhân đơn giá đã lưu; luồng ghi nhận chính hiện tính qua Service theo SD.
    /// </summary>
    public double tinhThanhTien()=>Quantity*UnitPrice;
}
/// <summary>
/// Control UC6: kiểm tra quyền/dữ liệu và điều phối Service, Booking, ServiceUsage trong transaction.
/// </summary>
sealed class DieuKhienDichVu(CoSoDuLieu db,NhanVien nv)
{
    /// <summary>
    /// Chỉ Quản lý được thêm/sửa/xóa danh mục; Lễ tân được ghi nhận dịch vụ sử dụng.
    /// </summary>
    void QuyenQuanLy(){if(nv.VaiTro!="Quản lý") throw new Exception("Chỉ Quản lý được thay đổi danh mục.");}
    /// <summary>
    /// Gọi Service.Doc để tải dữ liệu cho giao diện trước khi sửa.
    /// </summary>
    public Service ChiTiet(int id)=>Service.Doc(db,id);
    /// <summary>
    /// Yêu cầu Entity Service cung cấp danh mục hoặc danh sách dịch vụ đang hoạt động.
    /// </summary>
    public DataTable DanhSach(bool active=false)=>Service.DanhSach(db,active);
    /// <summary>
    /// Kiểm tra quyền, chuẩn hóa/kiểm tra dữ liệu và mở transaction; id=0 gọi Them, id khác 0 gọi capNhatThongTin.
    /// Chuyển lỗi trùng tên từ ràng buộc duy nhất MySQL thành thông báo nghiệp vụ.
    /// </summary>
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
    /// <summary>
    /// Kiểm tra quyền, mở transaction, đọc/khóa Service rồi giao Entity kiểm tra ràng buộc và xóa.
    /// </summary>
    public void Xoa(int id){QuyenQuanLy(); db.Luu(()=>{Service.Doc(db,id,true).Xoa();return true;});}
    /// <summary>
    /// Lấy mã BookingDetail và nhãn phòng/khách của lượt đã nhận thực tế, chưa trả và Room đang occupied.
    /// </summary>
    public DataTable PhongDangO()=>db.Doc("SELECT d.id,CONCAT(r.room_number,' · ',g.full_name) AS ten FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id JOIN Room r ON r.id=d.room_id JOIN Guest g ON g.id=b.guest_id WHERE b.status='checked_in' AND r.status='occupied' AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL");
    /// <summary>
    /// Lấy dịch vụ đã dùng của đúng mã BookingDetail, gồm số lượng, giá lịch sử và thành tiền; không trộn lịch sử phòng khác.
    /// </summary>
    public DataTable LichSu(int detail)=>db.Doc("SELECT s.name AS 'Dịch vụ',u.quantity AS 'Số lượng',u.unit_price AS 'Đơn giá',u.quantity*u.unit_price AS 'Thành tiền' FROM ServiceUsage u JOIN Service s ON s.id=u.service_id WHERE booking_detail_id=@p0 ORDER BY u.id DESC",detail);
    /// <summary>
    /// UC6.4 theo SD: kiểm tra quyền, Booking kiểm tra lưu trú, Service kiểm tra cung cấp và tính tiền, rồi Booking ghi nhận.
    /// Mở transaction cho toàn bộ thao tác; trả tiền dòng vừa thêm và tổng dịch vụ của Booking.
    /// </summary>
    public (double Dong,decimal Tong) GhiNhan(int detail,int key,int qty)=>db.Luu(()=>{
        if(nv.VaiTro is not ("Quản lý" or "Lễ tân")) throw new Exception("Không có quyền ghi nhận dịch vụ.");
        var booking=new Booking(db,detail);
        booking.KiemTraDangO();
        var service=Service.Doc(db,key,true);
        if(!service.kiemTraConCungCap()) throw new Exception("Dịch vụ hiện không được cung cấp.");
        var thanhTien=service.tinhThanhTien(qty);
        var usage=new ServiceUsage(db,detail,service,nv.Id,qty);
        // Booking ghi nhan va tong hop tien, sau buoc tinh tien cua Service theo SD.
        var tong=booking.GhiNhan(usage,qty);
        return (thanhTien,tong);
    });
}
/// <summary>
/// Phần của cùng Entity Booking dùng cho UC6.4: xác nhận khách đang lưu trú và ghi nhận dịch vụ vào lượt đặt.
/// </summary>
sealed partial class Booking
{
    /// <summary>
    /// Kiểm tra và khóa chi tiết lưu trú: Booking checked_in, Room occupied, đã nhận thực tế và chưa trả thực tế.
    /// </summary>
    public void KiemTraDangO()
    {
        if(db.Doc("SELECT d.id FROM BookingDetail d JOIN Booking b ON b.id=d.booking_id JOIN Room r ON r.id=d.room_id WHERE d.id=@p0 AND b.status='checked_in' AND r.status='occupied' AND d.actual_check_in IS NOT NULL AND d.actual_check_out IS NULL FOR UPDATE",detail).Rows.Count==0)
            throw new Exception("Chỉ ghi nhận dịch vụ cho phòng đang lưu trú.");
    }
    /// <summary>
    /// Gọi ServiceUsage lưu lần dùng, rồi cộng tiền dịch vụ của các BookingDetail thuộc cùng Booking.
    /// Tổng trả về là tổng lượt đặt; lịch sử trên giao diện lọc riêng theo chi tiết phòng đang chọn.
    /// </summary>
    public decimal GhiNhan(ServiceUsage usage,int qty)
    {
        usage.ghiNhanSuDung(qty);
        return Convert.ToDecimal(db.Doc("SELECT SUM(u.quantity*u.unit_price) FROM ServiceUsage u JOIN BookingDetail d ON d.id=u.booking_detail_id WHERE d.booking_id=(SELECT booking_id FROM BookingDetail WHERE id=@p0)",detail).Rows[0][0]);
    }
}
