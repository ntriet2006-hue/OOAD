const $=s=>document.querySelector(s), money=n=>Number(n).toLocaleString('vi-VN')+' đ';
let csrf='',data={},criteria=null,profile=[],rooms=[];
const escape=s=>String(s??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
function notice(text,error=false){
 const dialog=document.querySelector('dialog[open]');
 if(dialog && error){
  let feedback=dialog.querySelector('.formError');
  if(!feedback){feedback=document.createElement('p');feedback.className='formError';feedback.setAttribute('role','alert');dialog.querySelector('form').append(feedback)}
  feedback.textContent=text;
 }
$('#notice').textContent=text;$('#notice').className=error?'error':'';$('#notice').hidden=false;clearTimeout(notice.timer);notice.timer=setTimeout(()=>$('#notice').hidden=true,9000)}
async function api(path,body){
 const r=await fetch('/api/'+path,{method:body===undefined?'GET':'POST',headers:{'Content-Type':'application/json','X-CSRF':csrf},body:body===undefined?undefined:JSON.stringify(body)});
 if(!r.ok){if(r.status===401){$('#workspace').hidden=true;$('#login').hidden=false}let e=await r.json().catch(()=>({message:'Phiên làm việc hết hạn. Hãy tải lại trang.'}));throw Error(e.message)}
 return r.headers.get('content-type')?.includes('json')?r.json():null;
}
function action(fn){return async e=>{e?.preventDefault();const b=e?.submitter;if(b)b.disabled=true;try{await fn(e)}catch(err){notice(err.message,true)}finally{if(b)b.disabled=false}}}
function options(id,items,key,text){const previous=$(id).value;$(id).innerHTML=items.map(r=>'<option value="'+escape(r[key])+'">'+escape(r[text])+'</option>').join('');if([...$(id).options].some(o=>o.value===previous))$(id).value=previous}
function table(id,rows,extra=''){if(!rows.length){$(id).innerHTML='<div class="empty">Chưa có dữ liệu để hiển thị.</div>';return}let keys=Object.keys(rows[0]);$(id).innerHTML='<table><thead><tr>'+keys.map(k=>'<th>'+escape(k)+'</th>').join('')+(extra?'<th>Thao tác</th>':'')+'</tr></thead><tbody>'+rows.map((r,i)=>'<tr>'+keys.map(k=>'<td>'+cell(k,r[k])+'</td>').join('')+(extra?'<td>'+extra(i)+'</td>':'')+'</tr>').join('')+'</tbody></table>'}
function cell(k,v){
 if(/Ngày/.test(k)&&typeof v==='string')return escape(v.slice(0,10).split('-').reverse().join('/'));
 if(['Đơn giá','Thành tiền'].includes(k)&&typeof v==='number')return '<span class="money">'+money(v)+'</span>';
 if(k==='Trạng thái')return '<span class="pill '+(/Ngừng|Hủy/.test(v)?'muted':'')+'">'+escape(v)+'</span>';
 return escape(v);
}
function stats(id,items){$(id).innerHTML=items.map(([label,value])=>'<div class="stat"><span>'+escape(label)+'</span><strong>'+escape(value)+'</strong></div>').join('')}
async function load(){
 data=await api('data');$('#role').textContent=data.role;$('#addService').hidden=data.role!=='Quản lý';
 const old=$('#guest').value;options('#guest',[...data.guests,{id:0,ten:'Khách mới'}],'id','ten');if([...$('#guest').options].some(x=>x.value===old))$('#guest').value=old;
 options('#type',[{name:'Tất cả'},...data.types],'name','name');
 options('#stay',data.stays,'id','ten');options('#activeService',data.services.filter(s=>s.status==='active'),'id','name');
 table('#bookings',data.bookings);
 stats('#bookingStats',[['Lượt đặt phòng',data.bookings.length],['Phòng đang lưu trú',data.stays.length],['Khách hàng',data.guests.length]]);
 stats('#serviceStats',[['Dịch vụ trong danh mục',data.services.length],['Đang cung cấp',data.services.filter(s=>s.status==='active').length],['Phòng đang lưu trú',data.stays.length]]);
 table('#catalog',data.services.map(s=>({'Tên dịch vụ':s.name,'Nhóm':s.category,'Đơn vị':s.unit,'Đơn giá':money(s.price),'Trạng thái':s.status==='active'?'Đang cung cấp':'Ngừng cung cấp'})),data.role==='Quản lý'?i=>'<button class="secondary" data-edit="'+i+'">Sửa</button><button class="secondary" data-delete="'+i+'">Xóa</button>':'');
 await usageLog();
}
async function usageLog(){const detail=$('#stay').value;const rows=detail?await api('usage?detail='+detail):[];if($('#stay').value!==detail)return;table('#usageLog',rows);$('#usageTotal').textContent='Tổng dịch vụ của phòng: '+money(rows.reduce((a,r)=>a+Number(r['Thành tiền']),0))}
async function enter(){await load();$('#login').hidden=true;$('#workspace').hidden=false}
$('#loginForm').onsubmit=action(async()=>{const session=await api('session');csrf=session.csrf;let q=Object.fromEntries(new FormData($('#loginForm')));await api('login',q);$('#loginForm').password.value='';await enter()});
$('#logout').onclick=action(async()=>{await api('logout',{});location.reload()});
document.querySelectorAll('[data-tab]').forEach(b=>b.onclick=()=>{document.querySelector('#prototype').innerHTML='';document.querySelector('#bookingPrototype').innerHTML='';document.querySelector('#bookingLive').hidden=false;document.querySelector('#bookingPrototype').hidden=true;document.querySelectorAll('[data-booking-view]').forEach(x=>x.classList.toggle('active',x.dataset.bookingView==='live'));document.querySelectorAll('[data-tab]').forEach(x=>x.classList.toggle('active',x===b));$('#booking').hidden=b.dataset.tab!=='booking';$('#services').hidden=b.dataset.tab!=='services';$('#prototype').hidden=['booking','services'].includes(b.dataset.tab);if(!$('#prototype').hidden)window.moManHinh?.(b.dataset.tab);window.scrollTo({top:0,behavior:'instant'})});
const date=d=>new Date(d.getTime()-d.getTimezoneOffset()*60000).toISOString().slice(0,10);
$('#search').nhan.value=date(new Date());$('#search').tra.value=date(new Date(Date.now()+86400000));
$('#search').oninput=()=>{criteria=null;$('#rooms').innerHTML='';$('#selection').textContent='Điều kiện đã đổi. Hãy tìm phòng lại.'};
$('#search').onsubmit=action(async()=>{
 criteria=null;$('#rooms').innerHTML='';let q=Object.fromEntries(new FormData($('#search')));if(q.loai==='Tất cả')q.loai='';
 rooms=await api('rooms?'+new URLSearchParams(q));criteria={nhan:q.nhan,tra:q.tra,lon:+q.lon,nho:+q.nho,loai:q.loai};
 $('#roomCount').textContent=rooms.length+' phòng';
 $('#rooms').innerHTML=rooms.length?rooms.map(r=>'<label class="room"><div><input type="checkbox" value="'+r.id+'"> <strong>'+escape(r['Phòng'])+'</strong></div><span>'+escape(r['Loại'])+' · '+r['Sức chứa']+' khách</span><b>'+money(r['Đơn giá'])+' / đêm</b><span>Tạm tính '+money(r['Tạm tính'])+'</span></label>').join(''):'<p>Không còn phòng trống trong thời gian này. Hãy đổi ngày hoặc loại phòng.</p>';$('#selection').textContent='Chưa chọn phòng';
});
$('#rooms').onchange=()=>{const selected=[...document.querySelectorAll('#rooms input:checked')].map(x=>+x.value);$('#selection').textContent=selected.length+' phòng đã chọn · '+money(rooms.filter(r=>selected.includes(r.id)).reduce((a,r)=>a+Number(r['Tạm tính']),0))};
$('#book').onsubmit=action(async()=>{
 if(!criteria)throw Error('Hãy tìm phòng trống trước khi xác nhận.');
 let key=await api('bookings',{criteria,rooms:[...document.querySelectorAll('#rooms input:checked')].map(x=>+x.value),guest:+$('#guest').value,profile,coc:+$('#deposit').value,nguon:$('#source').value,note:$('#note').value});
 criteria=null;profile=[];await load();$('#roomCount').textContent='';$('#rooms').innerHTML='';$('#selection').textContent='Hãy tìm lại phòng cho lượt đặt tiếp theo.';notice('Đặt phòng thành công · DP'+String(key.id).padStart(4,'0'));
});
function serviceForm(s={id:0,name:'',category:'',unit:'',price:0,description:'',status:'active'}){for(let [k,v]of Object.entries(s))if($('#serviceForm').elements[k])$('#serviceForm').elements[k].value=v;$('#serviceForm').querySelector('.formError')?.remove();$('#dialogTitle').textContent=s.id?'Sửa dịch vụ':'Thêm dịch vụ';$('#serviceDialog').showModal()}
$('#addService').onclick=()=>serviceForm();
let pendingDeletion=null;
$('#catalog').onclick=action(async e=>{
 const b=e.target.closest('button');
 if(!b)return;
 if(b.dataset.edit!==undefined)serviceForm(await api('services/'+data.services[+b.dataset.edit].id));
 if(b.dataset.delete!==undefined){
  pendingDeletion=data.services[+b.dataset.delete];
  $('#deleteForm').querySelector('.formError')?.remove();
  $('#deleteMessage').textContent='Bạn muốn xóa dịch vụ “'+pendingDeletion.name+'”?';
  $('#deleteDialog').showModal();
 }
});
$('#deleteDialog').onclose=()=>{pendingDeletion=null};
$('#deleteForm').onsubmit=action(async()=>{
 if(!pendingDeletion)throw Error('Vui lòng chọn dịch vụ cần xóa.');
 await api('services/delete',{id:pendingDeletion.id});
 $('#deleteDialog').close();
 await load();
 notice('Xóa thành công');
});
$('#serviceForm').onsubmit=action(async()=>{let q=Object.fromEntries(new FormData($('#serviceForm')));q.id=+q.id;q.price=+q.price;await api('services',q);$('#serviceDialog').close();await load();notice(q.id?'Cập nhật thành công':'Lưu thành công')});
$('#stay').onchange=action(usageLog);
$('#usage').onsubmit=action(async()=>{let r=await api('usage',{detail:+$('#stay').value,service:+$('#activeService').value,quantity:+$('#quantity').value});await usageLog();notice('Ghi nhận dịch vụ thành công\nThành tiền: '+money(r.amount)+'\nTổng dịch vụ của đặt phòng: '+money(r.total))});
const guestNames=['Họ tên *','CCCD/hộ chiếu *','Điện thoại','Giới tính','Ngày sinh','Email','Địa chỉ','Quốc tịch'];
const limits=[50,20,15,6,10,50,100,30];
$('#guestFields').innerHTML=guestNames.map((label,i)=>{
 const input=i===3?'<select name="g3"><option value="">Chưa chọn</option><option value="male">Nam</option><option value="female">Nữ</option></select>':
 '<input name="g'+i+'" '+(i<2?'required':'')+' '+(i===4?'type="date"':i===5?'type="email"':'')+' maxlength="'+limits[i]+'">';
 return '<label>'+label+input+'</label>';
}).join('')+'<label>Loại khách<select name="g8"><option value="normal">Thường</option><option value="vip">VIP</option></select></label>';
$('#newGuest').onclick=()=>$('#guestDialog').showModal();
$('#guestForm').onsubmit=e=>{e.preventDefault();profile=Array.from({length:9},(_,i)=>$('#guestForm').elements['g'+i].value.trim());$('#guest').value='0';$('#guestDialog').close();notice('Đã nhập hồ sơ '+profile[0])};
document.querySelectorAll('.close').forEach(b=>b.onclick=()=>b.closest('dialog').close());
action(async()=>{const s=await api('session');csrf=s.csrf;if(s.role)await enter()})();
