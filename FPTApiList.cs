using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatCode_Selenium
{
    public partial class FPTApiList : Form
    {
        public FPTApiList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtAPI.Text.Trim()))
            {
                return;
            }
            try
            {
                var dt = Program.ExcecuteDataTable("sp_AddFPT_API"
                    , new Dictionary<string, object> { 
                        { "@api",txtAPI.Text.Trim()},
                        { "@email",txtEmail.Text.Trim()},
                    });
                this.btnRefresh.PerformClick();
                MessageBox.Show("Thêm thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var dt = Program.ExcecuteDataTable("SELECT *  FROM tblFPTAPI");
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Refresh();
            this.BestFitColumn();
        }

        private void BestFitColumn()
        {
            var grd  = dataGridView1;
            grd.ReadOnly = true;
            grd.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grd.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grd.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grd.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grd.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAPI.Text.Trim()))
            {
                return;
            }
            try
            {
                var dt = Program.ExcecuteDataTable("sp_DeleteFPT_API", new Dictionary<string, object> { 
                    { "@api", txtAPI.Text.Trim() },
                  { "@email",txtEmail.Text.Trim()},
                });
                this.btnRefresh.PerformClick();
                MessageBox.Show("Xóa thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtAPI_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
