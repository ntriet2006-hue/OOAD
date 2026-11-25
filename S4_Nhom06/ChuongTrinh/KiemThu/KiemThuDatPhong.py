"""Kiem thu tich hop UC4 va UC6 tren database rieng. Khong ghi vao ooad_hotel.
Chay: python KiemThuDatPhong.py --dll C:/OOAD/KiemTraKhung/Debug/net10.0/KhachSan.dll
Can MySQL local va tep mysql-local/demo-password; cac tai nguyen tu tao duoc don trong finally.
"""
from email import policy
from email.parser import BytesParser
import argparse,concurrent.futures,datetime,http.cookiejar,json,os,pathlib,shutil,socket,subprocess,tempfile,time,urllib.request,urllib.error,uuid
root=pathlib.Path(__file__).resolve().parents[3]
parser=argparse.ArgumentParser();parser.add_argument('--dll',required=True);args=parser.parse_args()
mysql=shutil.which('mysql') or r'C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe'
env=os.environ.copy();env['MYSQL_PWD']=(root/'mysql-local/demo-password').read_text().strip()
name='ooad_test_uc4_'+uuid.uuid4().hex[:12];assert name.startswith('ooad_test_uc4_')
checks=0;proc=None;tmp=pathlib.Path(tempfile.mkdtemp(prefix='ooad-uc4-'))
with socket.socket() as sock:sock.bind(('127.0.0.1',0));port=sock.getsockname()[1]
base=f'http://127.0.0.1:{port}'
def sql(query):
 r=subprocess.run([mysql,'--no-defaults','--host=127.0.0.1','--port=33316','--user=root','--default-character-set=utf8mb4','--batch','--skip-column-names'],input=query.encode(),env=env,capture_output=True)
 if r.returncode:raise RuntimeError(r.stderr.decode())
 return r.stdout.decode().strip()
def check(ok,description):
 global checks
 assert ok,description
 checks+=1;print('PASS',description,flush=True)
class Client:
 def __init__(self):self.http=urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()));self.token=''
 def call(self,path,body=None,status=200):
  data=None if body is None else json.dumps(body).encode()
  req=urllib.request.Request(base+'/api/'+path,data=data,headers={'Content-Type':'application/json','X-CSRF':self.token})
  try:
   with self.http.open(req,timeout=15) as r:code=r.status;raw=r.read()
  except urllib.error.HTTPError as e:code=e.code;raw=e.read()
  result=json.loads(raw) if raw else None
  assert code==status,(path,code,result)
  return result
 def session(self):self.token=self.call('session')['csrf'];return self
 def login(self):self.session();self.call('login',{'user':'letan','password':'Demo123!'});return self
now=datetime.date.today()
def y(offset=10,days=2,adults=1):return {'nhan':str(now+datetime.timedelta(days=offset)),'tra':str(now+datetime.timedelta(days=offset+days)),'lon':adults,'nho':0,'loai':''}
def query(criteria):return urllib.parse.urlencode({k:v for k,v in criteria.items() if k!='loai'})
def staff_booking(client,room,criteria):return client.call('bookings',{'criteria':criteria,'rooms':[room],'guest':1,'profile':[],'coc':100000,'nguon':'walk_in','note':'Kiem thu UC4'})['id']
def online(client,criteria,type_id=1):return client.call('online/bookings',{'criteria':criteria,'typeId':type_id,'count':1,'name':'Khách kiểm thử UC4','phone':'0900000000','email':'uc4@example.test','note':'Kiểm thử độc lập'})
try:
 seed=(root/'S4_Nhom06/ChuongTrinh/Database/S4_Nhom06_DB.sql').read_text(encoding='utf-8-sig').replace('ooad_hotel',name)
 sql(seed)
 runtime=env.copy();runtime.update(OOAD_USER='root',OOAD_PASSWORD=env['MYSQL_PWD'],OOAD_DATABASE=name,OOAD_EMAIL_DIR=str(tmp/'mail'),ASPNETCORE_URLS=base,ASPNETCORE_ENVIRONMENT='Development')
 log=open(tmp/'server.log','w',encoding='utf-8');proc=subprocess.Popen(['dotnet',str(pathlib.Path(args.dll).resolve())],cwd=root/'S4_Nhom06/ChuongTrinh/SourceCode',env=runtime,stdout=log,stderr=log)
 for _ in range(100):
  try:anon=Client().session();break
  except (OSError,urllib.error.URLError):time.sleep(.1)
 else:raise RuntimeError('Server khong khoi dong: '+(tmp/'server.log').read_text())
 anon.call('bookings/search',status=401);check(True,'khach chua dang nhap khong doc du lieu noi bo')
 types=anon.call('online/types');check(len(types)==2,'cong online truy cap khong can dang nhap')
 online_id=online(anon,y())['id'];row=sql(f'USE {name}; SELECT status,source,employee_id,deposit FROM Booking WHERE id={online_id}');check(row=='pending\tonline\tNULL\t0','online luu pending, online, nhan vien NULL, khong coc')
 mail=list((tmp/'mail').rglob('*.eml'));check(len(mail)==1 and f'DP{online_id:04d}' in str(BytesParser(policy=policy.default).parsebytes(mail[0].read_bytes())['Subject']),'tao tep email xac nhan co ma dat phong')
 staff=Client().login()
 available=staff.call('rooms?'+query(y())+'&loai='+urllib.parse.quote('Tiêu chuẩn'))
 booked_room=int(sql(f'USE {name}; SELECT room_id FROM BookingDetail WHERE booking_id={online_id}'))
 check(all(r['id']!=booked_room and r['Loại']=='Tiêu chuẩn' and r['Tạm tính']==1000000 for r in available) and len(available)>0,'Room tim phong: loc loai, loai lich trung va tinh tien')
 counts=sql(f'USE {name}; SELECT (SELECT COUNT(*) FROM Guest),(SELECT COUNT(*) FROM Booking),(SELECT COUNT(*) FROM BookingDetail)')
 poor={'criteria':y(60),'rooms':[1],'guest':0,'profile':['Khách thử thiếu cọc','TESTCOC','0900000000','','','coc@example.test','','','normal'],'coc':99999,'nguon':'walk_in','note':'Không được lưu'}
 error=staff.call('bookings',poor,status=400)
 check('cọc chưa đạt' in error['message'],'Booking chan coc duoi muc toi thieu')
 check(sql(f'USE {name}; SELECT (SELECT COUNT(*) FROM Guest),(SELECT COUNT(*) FROM Booking),(SELECT COUNT(*) FROM BookingDetail)')==counts,'thieu coc rollback ca khach moi va chi tiet')
 sql(f"USE {name}; UPDATE Setting SET setting_value='150000' WHERE setting_key='min_deposit'")
 poor.update(guest=1,profile=[],coc=100000)
 staff.call('bookings',poor,status=400);check(True,'Booking doc muc coc hien tai tu Setting')
 poor['coc']=150000
 staff.call('bookings',poor);check(True,'chap nhan coc dung muc toi thieu moi')
 sql(f"USE {name}; UPDATE Setting SET setting_value='100000' WHERE setting_key='min_deposit'")
 results=staff.call('bookings/search?word='+str(online_id)+'&status=pending');check(len(results)==1,'tim kiem ma va trang thai')
 check(staff.call('bookings/search?word=khong-ton-tai')==[],'tim kiem khong ket qua')
 staff.call('bookings/search?from=2027-02-02&to=2027-01-01',status=400);check(True,'chan khoang ngay tim kiem sai')
 b=staff.call(f'bookings/{online_id}?edit=true');room=b['rooms'][0]['id']
 edit_rooms=staff.call(f'bookings/{online_id}/rooms?'+query(y()))
 check(any(r['id']==room for r in edit_rooms),'kiemTraPhongTrong trong luong sua bo qua chinh Booking')
 normal_rooms=staff.call('rooms?'+query(y()))
 check(all(r['id']!=room for r in normal_rooms),'ngu canh loai tru khong ro ri sang luot tim phong khac')
 duplicate={'criteria':y(),'rooms':[room],'guest':1,'profile':[],'coc':100000,'nguon':'phone','note':''}
 staff.call('bookings',duplicate,status=400);check(True,'tao dat phong van chan trung sau khi chuyen kiem tra vao Booking')
 changed=staff.call('bookings/edit',{'id':online_id,'criteria':y(),'rooms':[room],'note':'Giữ nguyên phòng'});check(changed['rooms'][0]['id']==room,'sua khong tu bao trung phong cua chinh minh')
 changed=staff.call('bookings/edit',{'id':online_id,'criteria':y(12,3),'rooms':[3],'note':'Đổi sang 201'});check(changed['total']==2250000 and changed['rooms'][0]['number']=='201','doi phong va ngay, tinh lai tien')
 other=staff_booking(staff,2,y(12,3))
 before=sql(f'USE {name}; SELECT CONCAT(check_in_date,":",check_out_date,":",note) FROM Booking WHERE id={online_id}')
 err=staff.call('bookings/edit',{'id':online_id,'criteria':y(12,3),'rooms':[2],'note':'Khong duoc luu'},status=400)
 check('không còn trống' in err['message'],'chan sua trung phong')
 check(sql(f'USE {name}; SELECT CONCAT(check_in_date,":",check_out_date,":",note) FROM Booking WHERE id={online_id}')==before,'sua that bai rollback thong tin')
 check(sql(f'USE {name}; SELECT room_id FROM BookingDetail WHERE booking_id={online_id}')=='3','sua that bai giu phong cu')
 staff.call('bookings/edit',{'id':online_id,'criteria':y(12,3,100),'rooms':[3],'note':''},status=400);check(True,'chan suc chua khong du')
 staff.call('bookings/1?edit=true',status=400);staff.call('bookings/edit',{'id':1,'criteria':y(),'rooms':[1],'note':''},status=400);check(True,'chan sua dat phong da nhan o ca GET va POST')
 err=staff.call('bookings/cancel',{'id':1,'reason':'Thử chặn'},status=400);check('đang lưu trú' in err['message'],'chan huy phong dang luu tru')
 staff.call('bookings/cancel',{'id':other,'reason':' '},status=400);check(True,'bat buoc ly do huy')
 fee=staff.call(f'bookings/{other}/cancellation');check(fee['fee']==0 and fee['refund']==100000,'xem phi huy va coc can hoan dung chinh sach')
 result=staff.call('bookings/cancel',{'id':other,'reason':'Khách yêu cầu hủy'});check(result['refund']==100000,'huy thanh cong hoan toan bo coc')
 row=sql(f'USE {name}; SELECT status,cancel_fee,cancel_reason,deposit FROM Booking WHERE id={other}');check(row=='cancelled\t0\tKhách yêu cầu hủy\t100000','luu trang thai ly do phi huy, bao toan lich su coc')
 staff.call('bookings/cancel',{'id':other,'reason':'Lặp lại'},status=400);check(True,'chan huy hai lan')
 staff.call('bookings/edit',{'id':other,'criteria':y(),'rooms':[2],'note':''},status=400);check(True,'chan sua dat phong da huy')
 check(sql(f'USE {name}; SELECT status FROM Room WHERE id=2')=='available','giai phong phong khi huy')
 active_future=staff_booking(staff,4,y(20));staff.call('bookings/cancel',{'id':active_future,'reason':'Thử giữ lưu trú'});check(sql(f'USE {name}; SELECT status FROM Room WHERE id=4')=='occupied','huy lich tuong lai khong doi phong dang o thanh trong')
 count_before=int(sql(f'USE {name}; SELECT COUNT(*) FROM Booking'))
 body={'criteria':y(30),'typeId':1,'count':1,'name':'Khách','phone':'0900000000','email':'sai-email','note':''};anon.call('online/bookings',body,status=400)
 check(int(sql(f'USE {name}; SELECT COUNT(*) FROM Booking'))==count_before,'email sai khong luu dat phong')
 sql(f"USE {name}; UPDATE Room SET status='maintenance' WHERE room_type_id=1 AND id!=1")
 def race(_):
  c=Client().session()
  try:return online(c,y(40))
  except AssertionError as e:return str(e)
 with concurrent.futures.ThreadPoolExecutor(max_workers=2) as pool:out=list(pool.map(race,range(2)))
 check(sum(isinstance(x,dict) for x in out)==1,'hai khach cung dat phong cuoi: chi mot thanh cong')
 check(sql(f"USE {name}; SELECT COUNT(*) FROM Booking WHERE check_in_date='{y(40)['nhan']}'")=='1','khong luu them dat phong trung khi gui dong thoi')
 check(len(list((tmp/'mail').rglob('*.eml')))==2,'khong tao email cho dat phong that bai')

 # Kiem thu UC6 sau UC4, dung cung database tam va tu don dep.
 manager=Client().session();manager.call('login',{'user':'quanly','password':'Demo123!'})
 svc={'id':0,'name':'Dịch vụ thử SD','category':'Kiểm thử','unit':'lần','price':25000,'description':'Kiểm thử riêng','status':'active'}
 manager.call('services',svc)
 sid=next(s['id'] for s in manager.call('data')['services'] if s['name']==svc['name'])
 check(manager.call('services/'+str(sid))['price']==25000,'UC6 them dich vu')
 svc.update(id=sid,price=30000);manager.call('services',svc)
 check(manager.call('services/'+str(sid))['price']==30000,'UC6 sua dich vu')
 d103=int(sql(f"USE {name}; SELECT d.id FROM BookingDetail d JOIN Room r ON r.id=d.room_id WHERE r.room_number='103' LIMIT 1"))
 d104=int(sql(f"USE {name}; SELECT d.id FROM BookingDetail d JOIN Room r ON r.id=d.room_id WHERE r.room_number='104' LIMIT 1"))
 sql(f"USE {name}; UPDATE Room SET status='occupied' WHERE room_number IN ('103','104')")
 a=staff.call('usage',{'detail':d103,'service':sid,'quantity':2})
 b=staff.call('usage',{'detail':d104,'service':sid,'quantity':3})
 check(a['amount']==60000 and a['total']==60000 and b['amount']==90000 and b['total']==90000,'SD Service tinh tien, Booking ghi nhan, tien rieng tung phong')
 svc['price']=40000;manager.call('services',svc)
 check(staff.call('usage?detail='+str(d103))[0]['Đơn giá']==30000,'doi gia khong sua gia lich su')
 a=staff.call('usage',{'detail':d103,'service':sid,'quantity':1})
 check(a['amount']==40000 and a['total']==100000,'Service tinh gia moi, Booking tra tong cong don')
 staff.call('usage',{'detail':d103,'service':sid,'quantity':0},status=400)
 check(len(staff.call('usage?detail='+str(d103)))==2,'so luong 0 khong ghi them du lieu')
 svc['status']='inactive';manager.call('services',svc)
 err=staff.call('usage',{'detail':d103,'service':sid,'quantity':1},status=400)
 check('không được cung cấp' in err['message'],'chan dich vu ngung cung cap')
 err=manager.call('services/delete',{'id':sid},status=400)
 check('Không thể xóa' in err['message'],'chan xoa dich vu da su dung')
 unused=dict(svc,id=0,name='Dịch vụ chưa dùng',status='active');manager.call('services',unused)
 uid=next(s['id'] for s in manager.call('data')['services'] if s['name']==unused['name'])
 manager.call('services/delete',{'id':uid})
 check(all(s['id']!=uid for s in manager.call('data')['services']),'xoa dich vu chua su dung')

 print(f'HOAN TAT: {checks} kiem tra thanh cong; database that khong bi thay doi.',flush=True)
finally:
 if proc:proc.terminate();proc.wait(timeout=15);log.close()
 sql(f'DROP DATABASE IF EXISTS `{name}`')
 assert tmp.resolve().parent==pathlib.Path(tempfile.gettempdir()).resolve() and tmp.name.startswith('ooad-uc4-')
 shutil.rmtree(tmp)
