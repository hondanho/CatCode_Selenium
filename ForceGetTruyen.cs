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
    public partial class ForceGetTruyen : Form
    {
        public ForceGetTruyen()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtURLFullTruyen.Text.Trim()))
            {
                return;
            }
            try
            {
                var dt = Program.ExcecuteDataTable("insert into tblForceGetTruyen(title_url) select @title_url"
                    , new Dictionary<string, object> { 
                        { "@title_url",txtURLFullTruyen.Text.Trim()},
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
            var dt = Program.ExcecuteDataTable("SELECT *  FROM tblForceGetTruyen");
            this.dataGridView1.DataSource = dt;
            this.dataGridView1.Refresh();
            this.BestFitColumn();
        }

        private void BestFitColumn()
        {
            var grd  = dataGridView1;
            grd.ReadOnly = true;
            grd.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grd.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
         
    }
}
