using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using static GiaoDien;
static class KhoiDong
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        try {
            var cs=new MySqlConnectionStringBuilder{Server=Environment.GetEnvironmentVariable("OOAD_HOST")??"127.0.0.1",
                Port=uint.Parse(Environment.GetEnvironmentVariable("OOAD_PORT")??"33316"),Database="ooad_hotel",
                UserID=Environment.GetEnvironmentVariable("OOAD_USER")??"ooad_demo",Password=Environment.GetEnvironmentVariable("OOAD_PASSWORD")??"",CharacterSet="utf8mb4"};
            using var db=new CoSoDuLieu(cs.ConnectionString);
            using var form=new Form{Text="Quản lý khách sạn · Nhóm 06 · C#",Width=980,Height=700,StartPosition=FormStartPosition.CenterScreen,AutoScaleMode=AutoScaleMode.Dpi};
            void Login(){
                form.Controls.Clear();var panel=Cot();form.Controls.Add(panel);
                var user=O(panel,"Tên đăng nhập",new TextBox{Text="quanly",Width=230});
                var pass=O(panel,"Mật khẩu",new TextBox{UseSystemPasswordChar=true,Width=230});
                Nut(panel,"Đăng nhập",()=>{
                    var rows=db.Doc("SELECT employee_id,role,salt,digest FROM Account JOIN Employee ON Employee.id=Account.employee_id WHERE username=@p0",user.Text.Trim()).Rows;
                    if(rows.Count==0)throw new Exception("Sai tài khoản hoặc mật khẩu.");
                    var r=rows[0];var hash=Rfc2898DeriveBytes.Pbkdf2(pass.Text,Convert.FromHexString((string)r["salt"]),210000,HashAlgorithmName.SHA256,32);
                    if(!CryptographicOperations.FixedTimeEquals(hash,Convert.FromHexString((string)r["digest"])))throw new Exception("Sai tài khoản hoặc mật khẩu.");
                    var nv=new NhanVien((int)r["employee_id"],(string)r["role"]);var tabs=new TabControl{Dock=DockStyle.Fill};
                    var booking=new TabPage("Tạo đặt phòng");var service=new TabPage("Quản lý dịch vụ");
                    tabs.TabPages.AddRange([booking,service]);new GiaoDienDatPhong(booking,new(db,nv));new GiaoDienDichVu(service,new(db,nv),nv);
                    form.Controls.Clear();form.Controls.Add(tabs);var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=38};form.Controls.Add(bar);
                    bar.Controls.Add(new Label{AutoSize=true,Text=nv.VaiTro,Padding=new Padding(5)});Nut(bar,"Đăng xuất",Login);
                });
            }
            Login();Application.Run(form);
        } catch(Exception ex){MessageBox.Show(ex.Message,"Không mở được chương trình");}
    }
}
