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
    public partial class YoutubeKey : Form
    {
        public YoutubeKey()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txturiChuong.Text.Trim()))
            {
                return;
            }
            try
            {
               var dt = Program.ExcecuteDataTable("sp_UpdateAPI_dsChuong"
                    , new Dictionary<string, object> { 
                        { "@refId",txtrefID.Text.Trim()},
                        { "@uriChuong",txturiChuong.Text.Trim()},
                        { "@yKey",txtYKey.Text.Trim()},
                    });
                if(dt != null && dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0][0].ToString());

                }
                else
                {
                    MessageBox.Show("Cập nhật thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
