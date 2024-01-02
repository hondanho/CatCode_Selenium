using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CatCode_Selenium
{
    public partial class UChonHinhAnh : UserControl
    {
        public UChonHinhAnh()
        {
            InitializeComponent();
            this.txtID.TextChanged += TxtID_TextChanged;
            txtID.Text = "";
        }


        private void TxtID_TextChanged(object sender, EventArgs e)
        {
            LoadHinhAnh();
        }

        private void LoadHinhAnh()
        {
            int hControl = Screen.PrimaryScreen.Bounds.Height - txtID.Height - 80;
            int wControl = (Screen.PrimaryScreen.Bounds.Width - richLog.Width) / 4 - 20;
            string ID = txtID.Text;
            this.flowLayoutPanel1.Controls.Clear();
            if (!int.TryParse(ID, out int iID))
            {
                ID = String.Empty;
            }
            var dt = string.IsNullOrEmpty(ID)
                ? Program.ExcecuteDataTable("SELECT TOP(1) * FROM tblTruyen where daXuLy_HinhAnh is null  order by luotXem ")
                : Program.ExcecuteDataTable("SELECT TOP(1) * FROM tblTruyen where ID = " + ID)
                ;
            if(dt.Rows.Count == 0)
            {
                MessageBox.Show("hết ảnh");
                return;
            }
            ID = dt.Rows[0]["ID"].ToString();
            this.lblTenTruyen.Text = dt.Rows[0]["title"].ToString();
            this.txtID.TextChanged -= TxtID_TextChanged;
            this.txtID.Text = ID;
            this.txtID.TextChanged += TxtID_TextChanged;

            var urlHinhAnhSelected = dt.Rows[0]["urlHinhAnh"].ToString();
            var dtMoRong = Program.ExcecuteDataTable("SELECT  * FROM tblTruyen_dsThongTinMoRong where refID  = " + ID + " and urlHinhAnh like 'http%'");
            foreach (DataRow drMoRong in dtMoRong.Rows)
            {
                var uGroup = new UGroupHinhAnhTruyen(drMoRong, urlHinhAnhSelected)
                {
                    Name = drMoRong["title"].ToString()
                    ,
                    Width = wControl
                    ,
                    Height = hControl
                    ,
                };
                uGroup.Tag = drMoRong;
                uGroup.pic.Tag = uGroup;
                this.flowLayoutPanel1.Controls.Add(uGroup);
                uGroup.pic.Click += Pic_Click;

            }
        }

         

        private void Pic_Click(object sender, EventArgs e)
        {
            try
            {
                var pic = sender as PictureBox;
                var group = pic.Tag as UGroupHinhAnhTruyen;
                var dr = group.Tag as DataRow;
                var frmAdd = new frmAddLogo(dr);
                frmAdd.ShowDialog();
                if (frmAdd.DialogResult != DialogResult.OK) return;

                Program.ExcecuteNoneQuery("UPDATE tblTruyen" +
                    " SET " +
                    " urlHinhAnh_DaXuLy = @urlHinhAnh" +
                    ",daXuLy_HinhAnh = 1" +
                    ",x = @x" +
                    ",y = @y" +
                    ",w = @w" +
                    ",h = @h" +
                    ",urlHinh_hinhAnhXuLy = @urlHinh_hinhAnhXuLy" +
                    "  where ID = @ID ", new Dictionary<string, object>() {
                        {"@ID", dr["refID"]} ,
                        {"@urlHinhAnh", frmAdd.OutputPath },

                        {"@x", frmAdd.x },
                        {"@y", frmAdd.y },
                        {"@w", frmAdd.w },
                        {"@h", frmAdd.h },
                        {"@urlHinh_hinhAnhXuLy", frmAdd.UrlHinhAnh},
                });
                txtID.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("- Click: để chọn nháp\n- Double-Click: xác nhận chọn ảnh", "hướng dẫn");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ID = this.txtID.Text;
            if (!int.TryParse(ID, out int iID))
            {
                return;
            }
            Program.ExcecuteNoneQuery("UPDATE tblTruyen SET daXuLy_HinhAnh = 0  where ID = @ID ", new Dictionary<string, object>() {
                {"@ID", ID} ,
            });
            txtID.Text = "";//trigger -> reload new record
        }
    }
}
