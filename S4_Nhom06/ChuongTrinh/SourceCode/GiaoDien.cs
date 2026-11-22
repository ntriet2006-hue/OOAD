using System.Data;
static class GiaoDien
{
    internal static FlowLayoutPanel Cot()=>new(){BindingContext=new BindingContext(),Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,WrapContents=false,AutoScroll=true,Padding=new Padding(12)};
    internal static FlowLayoutPanel Hang(Control p){var h=new FlowLayoutPanel{AutoSize=true,WrapContents=false};p.Controls.Add(h);return h;}
    internal static T O<T>(Control p,string ten,T input) where T:Control{
        var box=new FlowLayoutPanel{AutoSize=true,FlowDirection=FlowDirection.TopDown,WrapContents=false};
        p.Controls.Add(box);box.Controls.Add(new Label{Text=ten,AutoSize=true});box.Controls.Add(input);return input;
    }
    internal static ComboBox Chon()=>new(){DropDownStyle=ComboBoxStyle.DropDownList,Width=170};
    internal static NumericUpDown So(int min,int value)=>new(){Minimum=min,Maximum=int.MaxValue,Value=value,ThousandsSeparator=true};
    internal static void Nut(Control p,string name,Action fn){
        var b=new Button{Text=name,AutoSize=true};p.Controls.Add(b);
        b.Click+=(_,_)=>{try{fn();}catch(Exception ex){MessageBox.Show(ex.Message,"Kiểm tra thông tin");}};
    }
    internal static DataGridView Bang(Control p,int height=145){
        var g=new DataGridView{Width=870,Height=height,ReadOnly=true,AllowUserToAddRows=false,RowHeadersVisible=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,SelectionMode=DataGridViewSelectionMode.FullRowSelect,MultiSelect=false};p.Controls.Add(g);return g;
    }
    internal static void Gan(ComboBox c,DataTable t,string text){c.DisplayMember=text;c.ValueMember="id";c.DataSource=t;}
    internal static int Id(ComboBox c)=>c.SelectedValue is int id?id:0;
    internal static int Dong(DataGridView grid)=>grid.CurrentRow?.Cells["id"].Value is int id?id:throw new Exception("Chọn một dịch vụ.");
    internal static void Nhap(string title,string[] labels,string[] values,Action<string[]> save, string[]? choices=null){
        using var f=new Form{Text=title,Width=440,Height=550,StartPosition=FormStartPosition.CenterParent};
        var panel=Cot();f.Controls.Add(panel); var controls=new List<Control>();
        for(int i=0;i<labels.Length;i++){
            Control c=new TextBox{Width=280,Text=values[i]};
            if(i==labels.Length-1 && choices!=null){var list=Chon();list.Items.AddRange(choices);list.SelectedItem=values[i];c=list;}
            controls.Add(O(panel,labels[i],c));
        }
        Nut(panel,"Hoàn tất",()=>{save(controls.Select(x=>x.Text.Trim()).ToArray());f.Close();}); f.ShowDialog();
    }
}

