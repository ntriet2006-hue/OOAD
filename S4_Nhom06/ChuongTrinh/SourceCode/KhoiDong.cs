using System.Data;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o=>{o.Cookie.HttpOnly=true;o.Cookie.SameSite=SameSiteMode.Strict;o.IdleTimeout=TimeSpan.FromHours(2);});
builder.Services.AddScoped(_=>new CoSoDuLieu(new MySqlConnectionStringBuilder{
 Server=Environment.GetEnvironmentVariable("OOAD_HOST")??"127.0.0.1",Port=uint.Parse(Environment.GetEnvironmentVariable("OOAD_PORT")??"33316"),
 Database=Environment.GetEnvironmentVariable("OOAD_DATABASE")??"ooad_hotel",UserID=Environment.GetEnvironmentVariable("OOAD_USER")??"ooad_demo",Password=Environment.GetEnvironmentVariable("OOAD_PASSWORD")??"",CharacterSet="utf8mb4"}.ConnectionString));
builder.Services.AddScoped(sp=>{var h=sp.GetRequiredService<IHttpContextAccessor>().HttpContext!;return new NhanVien(h.Session.GetInt32("id")??0,h.Session.GetString("role")??"");});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<DieuKhienDichVu>();builder.Services.AddScoped<DieuKhienDatPhong>();
var app=builder.Build();
app.UseSession();
app.Use(async(ctx,next)=>{
 try{
 if(ctx.Request.Path.StartsWithSegments("/api")){
 if(ctx.Request.Method!="GET" && (ctx.Session.GetString("csrf")==null || ctx.Request.Headers["X-CSRF"]!=ctx.Session.GetString("csrf"))){ctx.Response.StatusCode=403;return;}
 if(ctx.Request.Path!="/api/login" && ctx.Request.Path!="/api/session" && ctx.Session.GetInt32("id")==null){ctx.Response.StatusCode=401;return;}
 }
 await next();
 }catch(MySqlException ex){app.Logger.LogError(ex,"Database operation failed");ctx.Response.StatusCode=400;await ctx.Response.WriteAsJsonAsync(new{message="Không xử lý được dữ liệu. Vui lòng tải lại và kiểm tra thông tin."});}
 catch(Exception ex){ctx.Response.StatusCode=400;await ctx.Response.WriteAsJsonAsync(new{message=ex.Message});}
});
app.UseDefaultFiles();app.UseStaticFiles();
app.MapGet("/api/session",(HttpContext h)=>{
 var token=h.Session.GetString("csrf")??Convert.ToHexString(RandomNumberGenerator.GetBytes(24));h.Session.SetString("csrf",token);
 return new{csrf=token,role=h.Session.GetString("role")};});
app.MapPost("/api/login",(DangNhap q,CoSoDuLieu db,HttpContext h)=>{
 var rows=db.Doc("""
     SELECT a.id,a.employee_id,a.password,r.name AS role FROM Account a
     JOIN Employee e ON e.id=a.employee_id JOIN Role r ON r.id=a.role_id
     WHERE a.username=@p0 AND a.status='active' AND e.status='working' AND e.is_deleted=0
     """,q.User).Rows;
 if(rows.Count==0)throw new Exception("Sai tài khoản hoặc mật khẩu.");
 var r=rows[0];
 var parts=((string)r["password"]).Split('$');
 if(parts.Length!=4 || parts[0]!="PBKDF2-SHA256")
     throw new Exception("Định dạng mật khẩu không hợp lệ.");
 var hash=Rfc2898DeriveBytes.Pbkdf2(q.Password,Convert.FromHexString(parts[2]),
     int.Parse(parts[1]),HashAlgorithmName.SHA256,32);
 if(!CryptographicOperations.FixedTimeEquals(hash,Convert.FromHexString(parts[3])))
     throw new Exception("Sai tài khoản hoặc mật khẩu.");
 db.Ghi("UPDATE Account SET last_login=NOW() WHERE id=@p0",r["id"]);
 h.Session.SetInt32("id",(int)r["employee_id"]);
 h.Session.SetString("role",(string)r["role"]);
 return Results.Ok();
});
app.MapPost("/api/logout",(HttpContext h)=>{h.Session.Clear();return Results.Ok();});
app.MapGet("/api/data",(DieuKhienDichVu s,DieuKhienDatPhong b,NhanVien nv)=>new{role=nv.VaiTro,services=Rows(s.DanhSach()),stays=Rows(s.PhongDangO()),guests=Rows(b.Khach()),types=Rows(b.Loai()),bookings=Rows(b.LichSu())});
app.MapGet("/api/rooms",(DateTime nhan,DateTime tra,int lon,int nho,string? loai,DieuKhienDatPhong b)=>Rows(b.Tim(new(nhan.Date,tra.Date,lon,nho,loai??""))));
app.MapPost("/api/bookings",(DatPhong q,DieuKhienDatPhong b)=>{
 if(q.Nguon is not ("walk_in" or "phone"))throw new Exception("Nguồn đặt phòng không hợp lệ.");
 return new{id=b.Tao(q.Criteria,q.Rooms,q.Guest,q.Profile,q.Coc,q.Nguon,q.Note)};});
// SD sửa dịch vụ: giao diện yêu cầu chi tiết qua Control rồi Entity.
app.MapGet("/api/services/{id:int}",(int id,DieuKhienDichVu s)=>{
 var v=s.ChiTiet(id);
 return new{id=v.Id,name=v.Name,category=v.Category,unit=v.Unit,price=v.Price,description=v.Description,status=v.Status};
});
app.MapPost("/api/services",(DichVu q,DieuKhienDichVu s)=>{s.Luu(q.Id,q.Name,q.Category,q.Unit,q.Price,q.Description,q.Status);return Results.Ok();});
app.MapPost("/api/services/delete",(Ma q,DieuKhienDichVu s)=>{s.Xoa(q.Id);return Results.Ok();});
app.MapGet("/api/usage",(int detail,DieuKhienDichVu s)=>Rows(s.LichSu(detail)));
app.MapPost("/api/usage",(SuDung q,DieuKhienDichVu s)=>{var r=s.GhiNhan(q.Detail,q.Service,q.Quantity);return new{amount=r.Dong,total=r.Tong};});
app.Run();
static object[] Rows(DataTable t)=>t.AsEnumerable().Select(r=>(object)t.Columns.Cast<DataColumn>().ToDictionary(c=>c.ColumnName,c=>r[c]==DBNull.Value?null:r[c])).ToArray();
record DangNhap(string User,string Password);
record DatPhong(YeuCau Criteria,int[] Rooms,int Guest,string[] Profile,decimal Coc,string Nguon,string Note);
record DichVu(int Id,string Name,string Category,string Unit,decimal Price,string Description,string Status);
record Ma(int Id);
record SuDung(int Detail,int Service,int Quantity);
