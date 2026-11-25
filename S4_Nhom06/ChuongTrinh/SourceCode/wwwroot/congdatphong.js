(() => {
const $=s=>document.querySelector(s),esc=s=>String(s??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c])),money=n=>Number(n).toLocaleString('vi-VN')+' đ';
let token='',types=[],criteria=null,available=[];
const ready=fetch('/api/session').then(r=>r.json()).then(s=>token=s.csrf);
async function api(path,body){await ready;const r=await fetch('/api/online/'+path,{method:body===undefined?'GET':'POST',headers:{'Content-Type':'application/json','X-CSRF':token},body:body===undefined?undefined:JSON.stringify(body)});const result=await r.json().catch(()=>({message:'Không kết nối được máy chủ. Vui lòng thử lại.'}));if(!r.ok)throw Error(result.message||'Không xử lý được yêu cầu.');return result}
function action(fn){return async e=>{e.preventDefault();const b=e.submitter;if(b)b.disabled=true;$('#onlineNotice').hidden=true;try{await fn()}catch(err){$('#onlineNotice').textContent=err.message;$('#onlineNotice').hidden=false}finally{if(b)b.disabled=false}}}
const day=d=>new Date(d.getTime()-d.getTimezoneOffset()*60000).toISOString().slice(0,10),f=$('#onlineSearch');f.nhan.value=day(new Date());f.tra.value=day(new Date(Date.now()+86400000));
f.oninput=()=>{criteria=null;$('#onlineType').innerHTML='<option value="">Hãy tìm lại phòng</option>';$('#onlineRooms').innerHTML='<div class="empty">Ngày hoặc số khách đã thay đổi. Vui lòng tìm phòng lại.</div>'};
f.onsubmit=action(async()=>{criteria=null;const q=Object.fromEntries(new FormData(f));types=await api('types');available=await api('rooms?'+new URLSearchParams(q));criteria={nhan:q.nhan,tra:q.tra,lon:+q.lon,nho:+q.nho,loai:''};
 $('#onlineRooms').innerHTML=available.length?available.map(r=>'<div class="room"><strong>'+esc(r.name)+'</strong><span>'+r.capacity+' khách / phòng · Còn '+r.available+' phòng</span><b>'+money(r.price)+' / đêm</b></div>').join(''):'<div class="empty">Không còn phòng trống. Vui lòng chọn ngày khác.</div>';
 $('#onlineType').innerHTML=available.map(r=>'<option value="'+types.find(t=>t.name===r.name).id+'">'+esc(r.name)+' · '+money(r.price)+' / đêm</option>').join('');
});
$('#onlineBook').onsubmit=action(async()=>{if(!criteria)throw Error('Hãy tìm phòng trống trước khi xác nhận.');const r=await api('bookings',{criteria,typeId:+$('#onlineType').value,count:+$('#onlineCount').value,name:$('#onlineName').value,phone:$('#onlinePhone').value,email:$('#onlineEmail').value,note:$('#onlineNote').value});
 $('#onlineResult').innerHTML='<h2>Đặt phòng thành công</h2><p>Mã đặt phòng: <strong>DP'+String(r.id).padStart(4,'0')+'</strong></p><p>Trạng thái: Chờ xác nhận · Nguồn: Trực tuyến</p><p>'+(r.emailSaved?'Đã tạo email xác nhận cục bộ để mở xem khi demo.':'Đặt phòng đã lưu. '+esc(r.warning))+'</p>';$('#onlineResult').hidden=false;$('#onlineResult').scrollIntoView({behavior:'smooth',block:'center'});criteria=null;$('#onlineType').innerHTML='<option value="">Tìm lại phòng cho lượt đặt tiếp theo</option>';
});
})();
