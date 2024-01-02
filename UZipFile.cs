using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace CatCode_Selenium
{
    public partial class UZipFile : UserControl
    {
        string FOLDER_PROCESS_ID = "";

        public UZipFile()
        {
            InitializeComponent();
            FOLDER_PROCESS_ID = "tmp_" + Guid.NewGuid();
            if (!Directory.Exists(FOLDER_PROCESS_ID)) Directory.CreateDirectory(FOLDER_PROCESS_ID);

        }

        private void SetupProgessBar(int max)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    this.progressBar1.Maximum = max;
                    this.progressBar1.Minimum = 0;
                    this.progressBar1.Value = 0;
                    this.progressBar1.Update();
                }));
            }
            catch
            {
            }


        }
        private void UpdateTextControl(Control ctrl, string txt)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    ctrl.Text = txt;
                }));
            }
            catch
            {

            }

        }
        private void ChangeProgessBar(int val, string process_mgs)
        {
            try
            {
                if (this.progressBar1.Maximum < val) return;
                this.Invoke(new Action(() =>
                {
                    this.progressBar1.Value = val;
                    this.progressBar1.Update();
                    this.lblProcessMessage.Text = process_mgs;
                    lblPercent.Text = Math.Round((this.progressBar1.Value * 100.0 / (double)progressBar1.Maximum), 2).ToString() + "%";
                }));
            }
            catch
            {

            }



        }

        private void WriteLog(string mgs)
        {
            this.Invoke(new Action(() =>
            {
                this.richLog.Text = DateTime.Now.ToString() + ": " + mgs + "\n" + this.richLog.Text;
            }));
        }

        #region ZIP FILE

        public void START_ZIP_FILE()
        {
            var dtTruyen = Program.ExcecuteDataTable("sp_ZipHinhAnh");
            if (dtTruyen.Rows.Count == 0)
            {
                this.Dispose();

                return;
            }

            SetupProgessBar(dtTruyen.Rows.Count);
            Task.Factory.StartNew(new Action(() =>
            {
                foreach (DataRow dr in dtTruyen.Rows)
                {
                    ZipHinhAnh_TungChuong(dr);
                }
                if (!this.ckbStop.Checked)
                {
                    START_ZIP_FILE();
                }
            }));
        }
        private void ZipHinhAnh_TungChuong(DataRow dr)
        {
            string ID = Convert.ToString(dr["ID"]);
            string title = Convert.ToString(dr["title"]);
            string pathZip = string.Empty;
            string fileNameInNewFolder = string.Empty;
            try
            {
                int rowIndex = dr.Table.Rows.IndexOf(dr) + 1;
                ChangeProgessBar(rowIndex, "ID: " + ID + " - " + title);
                string pathSource = "DataTruyen/" + ID;
                if (!Directory.Exists(pathSource)) return;
                var lstFilesChuong = Directory.GetFiles(pathSource);
                string dirZip = "DataTruyen_ZipTungChuong/" + ID;
                if (!Directory.Exists(dirZip)) Directory.CreateDirectory(dirZip);
                foreach (var fileChuong in lstFilesChuong)
                {
                    var fileName = Path.GetFileName(fileChuong);
                    pathZip = dirZip + "/" + fileName.Replace(".txt", "").Replace(".html", "") + ".zip";
                    if (!File.Exists(pathZip))
                    {
                        fileNameInNewFolder = FOLDER_PROCESS_ID + "/" + fileName;
                        DeleteFile(fileNameInNewFolder);
                        File.Copy(fileChuong, fileNameInNewFolder);
                        ZipFile.CreateFromDirectory(FOLDER_PROCESS_ID, pathZip);
                        DeleteFile(fileNameInNewFolder);
                    }
                }
                Program.ExcecuteNoneQuery("UPDATE tblTruyen set daXuLy_Zip = 1 where ID = " + ID);
            }
            catch (Exception ex)
            {
                DeleteFile(pathZip);
                DeleteFile(fileNameInNewFolder);

                this.WriteLog(String.Format("ID Truyen:{0} - {1}\nException:{2}", ID, title, ex.Message));
            }
        }

        private void DeleteFile(string filePath)
        {
            try
            {

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return;
                }
                File.Delete(filePath);
            }
            catch
            {
            }
        }

        private void ZipHinhAnh(DataRow dr)
        {
            string ID = Convert.ToString(dr["ID"]);
            string title = Convert.ToString(dr["title"]);
            try
            {
                int rowIndex = dr.Table.Rows.IndexOf(dr) + 1;
                ChangeProgessBar(rowIndex, "ID: " + ID + " - " + title);
                string pathSource = "DataTruyen/" + ID;
                string pathZip = "DataTruyen_Zip/" + ID + ".zip";
                if (!Directory.Exists(pathSource)) return;
                if (!File.Exists(pathZip))
                {
                    ZipFile.CreateFromDirectory(pathSource, pathZip);
                }
                Program.ExcecuteNoneQuery("UPDATE tblTruyen set daXuLy_Zip = 1 where ID = " + ID);
            }
            catch (Exception ex)
            {
                this.WriteLog(String.Format("ID Truyen:{0} - {1}\nException:{2}", ID, title, ex.Message));
            }
        }

        #endregion

    }
}
