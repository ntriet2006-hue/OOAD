using static GiaoDien;
sealed class GiaoDienDichVu
{
    public GiaoDienDichVu(Control page,DieuKhienDichVu c,NhanVien nv)
    {
        var panel=Cot(); page.Controls.Add(panel);
        var catalog=Bang(panel); var actions=Hang(panel);
        var fields=new[]{"Tên dịch vụ","Nhóm","Đơn vị","Đơn giá (đ)","Mô tả","Trạng thái"};
        var statuses=new[]{"Đang cung cấp","Ngừng cung cấp"};
        var row=Hang(panel);
        var stay=O(row,"Phòng đang lưu trú",Chon()); var service=O(row,"Dịch vụ đang cung cấp",Chon()); var qty=O(row,"Số lượng",So(1,1));
        var record=Hang(panel); var history=Bang(panel);
        void Refresh(){
            catalog.DataSource=c.DanhSach(); catalog.Columns["id"]!.Visible=false;
            for(int i=0;i<fields.Length;i++) catalog.Columns[i+1].HeaderText=fields[i];
            Gan(service,c.DanhSach(true),"name");
            history.DataSource=c.LichSu(Id(stay));
        }
        void Form(int key){
            string[] values=["","","","0","",statuses[0]];
            if(key!=0){var s=c.ChiTiet(key);values=[s.Name,s.Category,s.Unit,s.Price.ToString(),s.Description,s.kiemTraConCungCap()?statuses[0]:statuses[1]];}
            Nhap(key==0?"Thêm dịch vụ":"Sửa dịch vụ",fields,values,v=>{
                if(!int.TryParse(v[3],out int price)) throw new Exception("Đơn giá phải là số nguyên.");
                c.Luu(key,v[0],v[1],v[2],price,v[4],v[5]==statuses[0]?"active":"inactive");
                Refresh();MessageBox.Show(key==0?"Lưu thành công":"Cập nhật thành công");
            },statuses);
        }
        if(nv.VaiTro=="Quản lý"){
            Nut(actions,"Thêm dịch vụ",()=>Form(0));Nut(actions,"Sửa dịch vụ",()=>Form(Dong(catalog)));
            Nut(actions,"Xóa dịch vụ",()=>{int key=Dong(catalog);if(MessageBox.Show("Xóa dịch vụ đang chọn?","Xác nhận",MessageBoxButtons.YesNo)==DialogResult.Yes){c.Xoa(key);Refresh();MessageBox.Show("Xóa thành công");}});
        }
        Nut(record,"Ghi nhận dịch vụ",()=>{var kq=c.GhiNhan(Id(stay),Id(service),(int)qty.Value);Refresh();MessageBox.Show($"Ghi nhận dịch vụ thành công\nThành tiền: {kq.Dong:N0} đ\nTổng dịch vụ của đặt phòng: {kq.Tong:N0} đ");});
        Gan(stay,c.PhongDangO(),"ten");stay.SelectedIndexChanged+=(_,_)=>history.DataSource=c.LichSu(Id(stay));Refresh();
    }
}
