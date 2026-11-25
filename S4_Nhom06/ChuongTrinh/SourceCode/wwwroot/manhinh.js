// Cac man hinh mo phong chi thao tac tren trinh duyet, khong goi API ghi du lieu.
(() => {
const modules={
 rooms:{title:'Phòng & loại phòng',sub:'Theo dõi danh mục, loại phòng và tình trạng phòng.',group:2,cols:['Phòng','Loại phòng','Tầng','Trạng thái'],rows:[['103','Tiêu chuẩn','1','Đang lưu trú'],['104','Tiêu chuẩn','1','Đang lưu trú'],['204','Gia đình','2','Đang lưu trú']]},
 guests:{title:'Khách hàng',sub:'Quản lý hồ sơ và thông tin liên hệ của khách lưu trú.',group:3,cols:['Khách hàng','CCCD / hộ chiếu','Điện thoại','Loại khách'],rows:[['Nguyễn An','DEMO001','0900000001','Thường'],['Trần Bình','DEMO002','0900000002','VIP']]},
 stays:{title:'Nhận & trả phòng',sub:'Tiếp nhận khách, xử lý trả phòng, đổi phòng và gia hạn lưu trú.',group:5,cols:['Mã đặt phòng','Phòng','Khách hàng','Trạng thái'],rows:[['DP0011','103','Khách mẫu phòng 103','Đang lưu trú'],['DP0013','204','Khách mẫu phòng 204','Đang lưu trú']]},
 invoices:{title:'Hóa đơn & thanh toán',sub:'Tổng hợp chi phí lưu trú, dịch vụ và thông tin thanh toán.',group:7,cols:['Mã hóa đơn','Đặt phòng','Tổng tiền','Trạng thái'],rows:[['HD-MAU01','DP-MAU01','1.030.000 đ','Chưa thanh toán'],['HD-MAU02','DP-MAU02','1.605.000 đ','Chưa thanh toán']]},
 employees:{title:'Nhân viên & ca làm',sub:'Quản lý hồ sơ nhân viên và phân công lịch làm việc.',group:8,cols:['Mã nhân viên','Họ tên','Bộ phận','Ca làm'],rows:[['NV-MAU01','Nhân viên mẫu','Lễ tân','Ca sáng'],['NV-MAU02','Quản lý mẫu','Quản lý','Ca hành chính']]},
 reports:{title:'Báo cáo thống kê',sub:'Theo dõi doanh thu, công suất phòng và hoạt động đặt phòng.',group:9,cols:['Báo cáo','Kỳ báo cáo','Người lập','Trạng thái'],rows:[['Doanh thu tháng — mẫu','11/2026','Quản lý','Bản minh họa'],['Công suất phòng — mẫu','11/2026','Quản lý','Bản minh họa']]},
 settings:{title:'Cài đặt hệ thống',sub:'Thiết lập tài khoản, phân quyền, thông tin khách sạn và dữ liệu.',group:10,cols:['Hạng mục','Giá trị minh họa','Trạng thái'],rows:[['Giờ nhận phòng','14:00','Mẫu giao diện'],['Giờ trả phòng','12:00','Mẫu giao diện'],['Tiền cọc tối thiểu','100.000 đ','Mẫu giao diện']]},
 account:{title:'Tài khoản cá nhân',sub:'Thay đổi thông tin bảo mật của tài khoản.',group:1,cols:[],rows:[]}
};
// Moi use case co bo truong rieng de trinh bay giao dien theo chuc nang.
const schemas={
 'UC1.3':['Mật khẩu hiện tại|password','Mật khẩu mới|password','Nhập lại mật khẩu mới|password'],
 'UC2.1':['Số phòng','Tầng|number','Loại phòng|select:Tiêu chuẩn,Gia đình','Trạng thái|select:Sẵn sàng,Bảo trì','Mô tả|textarea'],
 'UC2.2':['Phòng|select:103,104,204','Số phòng','Tầng|number','Loại phòng|select:Tiêu chuẩn,Gia đình','Mô tả|textarea'],
 'UC2.3':['Phòng cần xóa|select:103,104,204','Lý do xóa|textarea'],
 'UC2.4':['Phòng|select:103,104,204'],
 'UC2.5':['Số phòng / từ khóa','Loại phòng|select:Tất cả,Tiêu chuẩn,Gia đình','Trạng thái|select:Tất cả,Sẵn sàng,Đang lưu trú,Bảo trì'],
 'UC2.6':['Tên loại phòng','Sức chứa|number','Giá cơ bản mỗi đêm (đ)|number','Mô tả|textarea'],
 'UC2.7':['Loại phòng|select:Tiêu chuẩn,Gia đình','Tên loại phòng','Sức chứa|number','Giá cơ bản mỗi đêm (đ)|number'],
 'UC2.8':['Phòng|select:103,104,204','Trạng thái mới|select:Sẵn sàng,Đang lưu trú,Bảo trì','Ghi chú|textarea'],
 'UC3.2':['Khách hàng|select:Nguyễn An,Trần Bình','Họ tên','Điện thoại|tel','Email|email','Địa chỉ','Quốc tịch','Loại khách|select:Thường,VIP'],
 'UC3.3':['Khách hàng|select:Nguyễn An,Trần Bình'],
 'UC3.4':['Họ tên / CCCD / điện thoại','Loại khách|select:Tất cả,Thường,VIP'],
 'UC5.1':['Mã đặt phòng','Khách hàng','CCCD / hộ chiếu','Phòng|select:103,104,204','Thời gian nhận thực tế|datetime-local','Ghi chú|textarea'],
 'UC5.2':['Phòng đang lưu trú|select:103,104,204','Thời gian trả thực tế|datetime-local','Tình trạng phòng|select:Bình thường,Cần kiểm tra','Phụ thu (đ)|number','Ghi chú|textarea'],
 'UC5.3':['Phòng hiện tại|select:103,104,204','Phòng chuyển đến','Thời gian đổi|datetime-local','Lý do đổi phòng|textarea'],
 'UC5.4':['Phòng đang lưu trú|select:103,104,204','Ngày trả mới|date','Ghi chú|textarea'],
 'UC7.1':['Mã đặt phòng','Ngày lập|date','Tiền phòng (đ)|number','Tiền dịch vụ (đ)|number','Phụ thu (đ)|number','Tiền đã cọc (đ)|number'],
 'UC7.2':['Mã hóa đơn','Số tiền thanh toán (đ)|number','Phương thức|select:Tiền mặt,Chuyển khoản,Thẻ','Mã giao dịch','Ghi chú|textarea'],
 'UC7.3':['Mã hóa đơn','Khổ giấy|select:A4,A5','Bản hiển thị|select:Chi tiết,Tóm tắt'],
 'UC7.4':['Mã hóa đơn','Mã khuyến mãi'],
 'UC8.1':['Họ tên','Ngày sinh|date','Giới tính|select:Nam,Nữ','CCCD / hộ chiếu','Điện thoại|tel','Email|email','Bộ phận','Chức vụ','Ngày vào làm|date'],
 'UC8.2':['Mã nhân viên','Họ tên','Điện thoại|tel','Email|email','Bộ phận','Chức vụ','Trạng thái|select:Đang làm,Ngừng làm'],
 'UC8.3':['Mã nhân viên','Lý do xóa|textarea'],
 'UC8.4':['Mã nhân viên'],
 'UC8.5':['Họ tên / mã nhân viên','Bộ phận','Trạng thái|select:Tất cả,Đang làm,Ngừng làm'],
 'UC8.6':['Mã nhân viên','Ngày làm|date','Ca làm|select:Ca sáng,Ca chiều,Ca đêm','Ghi chú|textarea'],
 'UC9.1':['Từ ngày|date','Đến ngày|date','Nhóm theo|select:Ngày,Tháng,Năm'],
 'UC9.2':['Từ ngày|date','Đến ngày|date','Loại phòng|select:Tất cả,Tiêu chuẩn,Gia đình'],
 'UC9.3':['Từ ngày|date','Đến ngày|date','Nhóm khách|select:Tất cả,Thường,VIP'],
 'UC9.4':['Tên báo cáo','Loại báo cáo|select:Tất cả,Doanh thu,Công suất phòng,Khách hàng và đặt phòng','Từ ngày|date','Đến ngày|date'],
 'UC9.5':['Báo cáo|select:Doanh thu,Công suất phòng,Khách hàng và đặt phòng','Định dạng|select:PDF,Excel','Từ ngày|date','Đến ngày|date'],
 'UC10.1':['Tên đăng nhập','Mã nhân viên','Mật khẩu ban đầu|password','Trạng thái|select:Hoạt động,Khóa'],
 'UC10.2':['Tài khoản','Vai trò|select:Quản lý,Lễ tân','Quyền truy cập|select:Xem,Thêm và sửa,Toàn quyền'],
 'UC10.3':['Tên khách sạn','Địa chỉ','Điện thoại|tel','Email|email','Giờ nhận phòng|time','Giờ trả phòng|time','Tiền cọc tối thiểu (đ)|number'],
 'UC10.4':['Mã khuyến mãi','Tên chương trình','Ngày bắt đầu|date','Ngày kết thúc|date','Mức giảm (%)|number','Trạng thái|select:Hoạt động,Ngừng áp dụng'],
 'UC10.5':['Thao tác|select:Sao lưu dữ liệu,Phục hồi dữ liệu','Tên bản sao lưu','Tệp phục hồi (.sql)|file','Ghi chú|textarea']
};
let catalog=[],current=null,operation=null;
const esc=s=>String(s??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const host=document.querySelector('#prototype');
function sampleTable(m,query=''){
 const rows=m.rows.filter(r=>r.join(' ').toLocaleLowerCase('vi').includes(query.toLocaleLowerCase('vi')));
 return rows.length?'<table><thead><tr>'+m.cols.map(c=>'<th>'+esc(c)+'</th>').join('')+'</tr></thead><tbody>'+rows.map(r=>'<tr>'+r.map(c=>'<td>'+esc(c)+'</td>').join('')+'</tr>').join('')+'</tbody></table>':'<div class="empty">Không tìm thấy dữ liệu minh họa phù hợp.</div>';
}
function fields(code){return schemas[code].map((spec,i)=>{const [label,type='text']=spec.split('|'),id='preview-'+i;let input;
 if(type.startsWith('select:'))input='<select id="'+id+'" name="'+esc(label)+'">'+type.slice(7).split(',').map(v=>'<option>'+esc(v)+'</option>').join('')+'</select>';
 else if(type==='textarea')input='<textarea id="'+id+'" name="'+esc(label)+'" placeholder="Nhập '+esc(label.toLowerCase())+'"></textarea>';
 else input='<input id="'+id+'" name="'+esc(label)+'" type="'+type+'" '+(type==='number'?'min="0" step="1"':type==='file'?'accept=".sql"':'')+' '+(['text','tel','email'].includes(type)?'placeholder="Nhập '+esc(label.toLowerCase())+'"':'')+'>';
 return '<label class="'+(type==='textarea'?'full':'')+'" for="'+id+'">'+esc(label)+input+'</label>';
}).join('')}
function selectOperation(code){operation=catalog.find(c=>c.code===code);if(!operation)return;
 host.querySelectorAll('[data-operation]').forEach(b=>b.classList.toggle('active',b.dataset.operation===code));
 document.querySelector('#operationPanel').innerHTML='<form id="previewForm"><div class="heading"><h2>'+esc(operation.name)+'</h2><span class="screen-label">'+esc(code)+'</span></div><p class="card-description">Điền thông tin để xem trước biểu mẫu. Chưa kết nối xử lý nghiệp vụ.</p><div class="form-grid">'+fields(code)+'</div><div class="form-footer"><button type="reset" class="secondary">Nhập lại</button><button type="submit">Xem trước</button></div><div id="previewResult" class="preview-result" role="status" hidden></div></form>';
 const form=document.querySelector('#previewForm');
 form.onreset=()=>{document.querySelector('#previewResult').hidden=true};
 form.onsubmit=e=>{e.preventDefault();const result=document.querySelector('#previewResult');let pairs=[];
 for(const input of form.querySelectorAll('input,select,textarea')){const v=input.type==='password'?(input.value?'••••••••':'Chưa nhập'):input.type==='file'?(input.files[0]?.name||'Chưa chọn tệp'):input.value||'Chưa nhập';pairs.push('<dt>'+esc(input.name)+'</dt><dd>'+esc(v)+'</dd>')}
 result.innerHTML='<strong>Bản xem trước · '+esc(operation.name)+'</strong><dl>'+pairs.join('')+'</dl><span>Chỉ hiển thị thông tin vừa nhập. Không lưu, xóa, thanh toán, xuất tệp hoặc thay đổi dữ liệu.</span>';result.hidden=false;
 };
}
window.moManHinh=async key=>{
 const m=modules[key];if(!m)return;current=key;
 host.innerHTML='<div class="eyebrow">QUẢN LÝ KHÁCH SẠN</div><h1>'+esc(m.title)+'</h1><p>'+esc(m.sub)+'</p>'+'<div class="preview-note"><strong>Giao diện minh họa</strong><span>Các chức năng trong mục này chưa kết nối nghiệp vụ. Dữ liệu bên dưới là mẫu trình bày và không được lưu vào hệ thống.</span></div><div id="prototypeContent"><div class="empty">Đang tải giao diện…</div></div>';
 try{if(!catalog.length){const response=await fetch('/chucnang.json');if(!response.ok)throw Error();catalog=await response.json()}if(current!==key)return;
 const ops=catalog.filter(c=>Number(c.code.match(/^UC(\d+)/)[1])===m.group);
 document.querySelector('#prototypeContent').innerHTML=(key==='guests'?'<p class="card-description">Thêm khách hàng thật được thực hiện trong mục Tạo đặt phòng → Nhập khách mới.</p>':'')+('<div class="operation-tabs" aria-label="Chọn chức năng">'+ops.map(c=>'<button type="button" data-operation="'+c.code+'">'+esc(c.name)+'</button>').join('')+'</div>')+'<div class="card" id="operationPanel"></div>'+(m.rows.length?'<div class="card"><div class="heading"><h2>Dữ liệu minh họa</h2><span class="caption">Không phải dữ liệu đang chạy</span></div><div class="toolbar"><input id="sampleSearch" aria-label="Tìm trong dữ liệu minh họa" placeholder="Tìm trong danh sách mẫu…" type="search"></div><div id="sampleList" class="scroll">'+sampleTable(m)+'</div></div>':'');
 host.querySelectorAll('[data-operation]').forEach(b=>b.onclick=()=>selectOperation(b.dataset.operation));selectOperation(ops[0].code);
 const search=document.querySelector('#sampleSearch');if(search)search.oninput=()=>{document.querySelector('#sampleList').innerHTML=sampleTable(m,search.value)};
 }catch{document.querySelector('#prototypeContent').innerHTML='<div class="empty">Chưa tải được danh mục chức năng. Vui lòng mở lại mục này.</div>'}
};
})();
