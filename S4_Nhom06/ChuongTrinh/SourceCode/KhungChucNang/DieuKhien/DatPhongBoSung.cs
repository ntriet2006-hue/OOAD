using System.Data;
using System.Net.Mail;

// UC4.3–UC4.6: Boundary -> Control -> Booking -> Room theo SD 3.1.34–37.
sealed partial class DieuKhienDatPhong
{
    void QuyenDatPhong()
    {
        if(nv.VaiTro is not ("Quản lý" or "Lễ tân"))throw new Exception("Không có quyền quản lý đặt phòng.");
    }
    public object ChiTietDatPhong(int id,bool deSua=false)
    {
        QuyenDatPhong();var b=Booking.DocDatPhong(db,id);
        if(deSua)b.KiemTraChoSua();return b.ThongTin();
    }
    public DataTable TimKiemDatPhong(string tuKhoa,DateTime? tuNgay,DateTime? denNgay,string trangThai)
    {
        QuyenDatPhong();
        if(tuKhoa.Length>100 || (tuNgay.HasValue&&denNgay.HasValue&&tuNgay>denNgay) ||
            trangThai is not ("" or "pending" or "confirmed" or "checked_in" or "checked_out" or "cancelled"))throw new Exception("Tiêu chí tìm kiếm không hợp lệ.");
        return Booking.TimKiem(db,tuKhoa.Trim(),tuNgay,denNgay,trangThai);
    }
    public object SuaDatPhong(int maDatPhong,YeuCau yeuCau,int[] maPhong,string ghiChu)
    {
        QuyenDatPhong();KiemTraYeuCau(yeuCau);
        if(ghiChu.Length>255)throw new Exception("Ghi chú tối đa 255 ký tự.");
        return db.Luu(()=>{
            var b=Booking.DocDatPhong(db,maDatPhong,true);b.KiemTraChoSua();
            b.ChuanBiSua(yeuCau,maPhong,ghiChu);
            if(!b.suaDatPhong())throw new Exception("Không thể cập nhật đặt phòng.");return b.ThongTin();
        });
    }
    public DataTable PhongDeSua(int id,YeuCau y){QuyenDatPhong();Booking.DocDatPhong(db,id).KiemTraChoSua();return Tim(y,id);}
    public object XemPhiHuy(int id)
    {
        QuyenDatPhong();var b=Booking.DocDatPhong(db,id);b.KiemTraChoHuy();return b.KetQuaHuy();
    }
    public object HuyDatPhong(int maDatPhong,string lyDo)
    {
        QuyenDatPhong();lyDo=lyDo.Trim();if(lyDo.Length is 0 or >255)throw new Exception("Nhập lý do hủy từ 1 đến 255 ký tự.");
        return db.Luu(()=>{var b=Booking.DocDatPhong(db,maDatPhong,true);b.KiemTraChoHuy();b.ChuanBiHuy(lyDo);
            if(!b.huyDatPhong())throw new Exception("Không thể hủy đặt phòng.");return b.KetQuaHuy();});
    }
    public object LoaiPhongTrucTuyen(YeuCau y)
    {
        var available=Tim(y);
        return available.AsEnumerable().GroupBy(r=>r["Loại"].ToString()).Select(g=>new{
            name=g.Key,available=g.Count(),price=Convert.ToDecimal(g.First()["Đơn giá"]),capacity=Convert.ToInt32(g.First()["Sức chứa"])
        }).ToArray();
    }
    public object DatPhongTrucTuyen(YeuCau yeuCau,int maLoaiPhong,int soPhong,string hoTen,string dienThoai,string email,string yeuCauDacBiet)
    {
        KiemTraYeuCau(yeuCau);hoTen=hoTen.Trim();dienThoai=dienThoai.Trim();email=email.Trim();yeuCauDacBiet=yeuCauDacBiet.Trim();
        if(soPhong is <1 or >50 || hoTen.Length is 0 or >50 || dienThoai.Length is 0 or >15 ||
           !dienThoai.All(c=>char.IsDigit(c)||c=='+'||c==' '||c=='-') || email.Length>50 ||
           !MailAddress.TryCreate(email,out var address)||address.Address!=email||yeuCauDacBiet.Length>255)
            throw new Exception("Thông tin liên hệ, số phòng hoặc yêu cầu đặc biệt không hợp lệ.");
        // Khoa cung loai theo thu tu id truoc khi kiem tra de hai khach khong cung lay mot phong.
        var booking=db.Luu(()=>{
            var ids=db.Doc("SELECT id FROM Room WHERE room_type_id=@p0 AND is_deleted=0 ORDER BY id",maLoaiPhong).AsEnumerable().Select(r=>(int)r["id"]).ToArray();
            var selected=ids.Select(id=>Room.DocVaKhoa(db,id)).Where(r=>r.kiemTraPhongTrong(yeuCau.Nhan,yeuCau.Tra)).Take(soPhong).ToList();
            if(selected.Count!=soPhong)throw new Exception("Phòng đã hết. Vui lòng chọn ngày hoặc loại phòng khác.");
            if(selected.Sum(r=>(long)r.Capacity)<(long)yeuCau.Lon+yeuCau.Nho)throw new Exception("Tổng sức chứa chưa đủ.");
            var guest=Guest.TaoLienHeTrucTuyen(db,hoTen,dienThoai,email);
            var b=new Booking(db,guest,null,yeuCau,selected,0,"online",yeuCauDacBiet);
            if(!b.taoDatPhong())throw new Exception("Không thể tạo đặt phòng.");return b;
        });
        // Chi tao thu sau khi giao dich da commit; loi tao thu khong duoc bao dat phong that bai.
        try{return new{id=booking.Id,status="pending",emailSaved=true,emailFile=booking.LuuEmailXacNhan(email,hoTen)};}
        catch(Exception){return new{id=booking.Id,status="pending",emailSaved=false,emailFile="",warning="Đặt phòng đã lưu nhưng chưa tạo được tệp email xác nhận. Vui lòng liên hệ lễ tân với mã đặt phòng."};}
    }
}
