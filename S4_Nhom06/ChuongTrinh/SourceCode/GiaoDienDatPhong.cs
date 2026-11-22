using static GiaoDien;
sealed class GiaoDienDatPhong
{
    public GiaoDienDatPhong(Control page,DieuKhienDatPhong c)
    {
        var panel=Cot();page.Controls.Add(panel);var h=Hang(panel);
        var nhan=O(h,"Ngày nhận",new DateTimePicker{Format=DateTimePickerFormat.Short,Width=120});
        var tra=O(h,"Ngày trả",new DateTimePicker{Format=DateTimePickerFormat.Short,Width=120,Value=DateTime.Today.AddDays(1)});
        var lon=O(h,"Người lớn",So(1,1));var nho=O(h,"Trẻ em",So(0,0));var loai=O(h,"Loại phòng",Chon());
        loai.Items.Add("Tất cả");foreach(System.Data.DataRow r in c.Loai().Rows) loai.Items.Add(r[0]);loai.SelectedIndex=0;
        YeuCau Criteria()=>new(nhan.Value.Date,tra.Value.Date,(int)lon.Value,(int)nho.Value,loai.SelectedIndex==0?"":loai.Text);
        YeuCau? last=null;
        var actions=Hang(panel);var rooms=Bang(panel,125);rooms.MultiSelect=true;
        void Search(){last=null;var y=Criteria();rooms.DataSource=c.Tim(y);rooms.Columns["id"]!.Visible=false;last=y;}
        Nut(actions,"Tìm phòng trống",()=>{Search();if(rooms.Rows.Count==0)MessageBox.Show("Không còn phòng trống trong thời gian này.");});
        actions.Controls.Add(new Label{AutoSize=true,Text="Giữ Ctrl để chọn nhiều phòng.",Padding=new Padding(5)});
        h=Hang(panel);var guest=O(h,"Khách hàng",Chon());string[] profile=[];
        void Guests(){var t=c.Khach();t.Rows.Add(0,"Khách mới");Gan(guest,t,"ten");}Guests();
        Nut(h,"Nhập khách mới",()=>Nhap("Hồ sơ khách hàng",["Họ tên","CCCD/hộ chiếu","Điện thoại","Giới tính","Ngày sinh","Email","Địa chỉ","Quốc tịch","Loại khách"],["","","","","","","","","Thường"],v=>{
            if(v[0]=="" || v[1]=="") throw new Exception("Nhập tên và giấy tờ.");
            v[8]=v[8]=="VIP"?"vip":"normal";profile=v;guest.SelectedValue=0;
        },["Thường","VIP"]));
        var source=O(h,"Nguồn đặt",Chon());source.Items.AddRange(["Trực tiếp","Điện thoại"]);source.SelectedIndex=0;
        h=Hang(panel);var coc=O(h,"Tiền cọc (đ)",So(0,100000));var note=O(h,"Ghi chú",new TextBox{Width=250});
        var save=Hang(panel);var history=Bang(panel,130);
        Nut(save,"Xác nhận đặt phòng",()=>{
            if(last!=Criteria())throw new Exception("Hãy tìm phòng lại theo điều kiện mới.");
            var ids=rooms.SelectedRows.Cast<DataGridViewRow>().Select(r=>Convert.ToInt32(r.Cells["id"].Value)).ToArray();
            long key=c.Tao(last,ids,Id(guest),profile,(int)coc.Value,source.SelectedIndex==0?"walk_in":"phone",note.Text);
            Guests();profile=[];Search();history.DataSource=c.LichSu();MessageBox.Show($"Đặt phòng thành công · DP{key:0000}");
        });
        Search();history.DataSource=c.LichSu();
    }
}

