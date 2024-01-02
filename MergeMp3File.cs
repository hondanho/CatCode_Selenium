using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatCode_Selenium
{
    public partial class MergeMp3File : Form
    {
        public MergeMp3File()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            if (string.IsNullOrEmpty(this.txtInput.Text) || string.IsNullOrEmpty(this.txtOutput.Text) || string.IsNullOrEmpty(this.txtBatchFiles.Text))
            {
                richTextBox1.Text += "Some config is empty\n";
                return;
            }
            try
            {
                button1.Enabled = false;
                DirectoryInfo d = new DirectoryInfo(txtInput.Text);

                var Files = d.GetFiles("*.mp3").OrderBy(f => f.LastWriteTime).Take(5).ToList();
                if(Files.Count < 5)
                {
                    richTextBox1.Text += "Files.Count < 5\n";
                    return;
                }
                string newfileName = String.Format("{0}-to-{1}.mp3", Files[0].Name.Replace(".mp3","") , Files[4].Name.Replace(".mp3", ""));
                richTextBox1.Text += "Output file Name: " + newfileName + "\n";
                Program.MergeMp3File(txtOutput.Text +"\\"+ newfileName,new List<string>()
                {
                    Files[0].FullName,
                    "C:\\Users\\linhb\\Downloads\\TuLieu\\silent-2s.mp3",

                    "C:\\Users\\linhb\\Downloads\\TuLieu\\intro.mp3",

                    Files[1].FullName,
                    "C:\\Users\\linhb\\Downloads\\TuLieu\\silent-2s.mp3",

                    Files[2].FullName,
                    "C:\\Users\\linhb\\Downloads\\TuLieu\\silent-2s.mp3",

                    "C:\\Users\\linhb\\Downloads\\TuLieu\\splip.mp3",
                    Files[3].FullName,
                    "C:\\Users\\linhb\\Downloads\\TuLieu\\silent-2s.mp3",

                    Files[4].FullName,
                    "C:\\Users\\linhb\\Downloads\\TuLieu\\silent-2s.mp3",

                    "C:\\Users\\linhb\\Downloads\\TuLieu\\outtro.mp3",
                });
                foreach (var item in Files)
                {
                    File.Delete(item.FullName);
                }
                richTextBox1.Text += "DONE\n";
                button1.Enabled = true;

            }
            catch (Exception ex)
            {

                richTextBox1.Text += "Exception: " + ex.Message + "\n";
            }


        }
    }
}
