using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace CatCode_Selenium
{
    public class Rootobject
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class Parsedresult
    {
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
    public partial class frmManager : Form
    {
        public frmManager()
        {
            InitializeComponent();
            this.FormClosed += FrmManager_FormClosed;
            #region Visible Menu
            this.mnChonHinhAnh.Visible = false;
            this.speechtoTextToolStripMenuItem.Visible = false;
            this.removeImageSpamToolStripMenuItem.Visible = false;
            this.webToolStripMenuItem.Visible = false;
            this.mn_Zip.Visible = false;
            this.gGDriveToolStripMenuItem.Visible = false;
            this.mn_MenuFullTruyenTranh.Visible = false;
            this.txtNumberRun.Visible = false;
            this.menuFullTruyen.Visible = false;
            this.fullTruyenToolStripMenuItem.Visible = false;
            #endregion
            this.Shown += FrmManager_Shown;
        }

        private void FrmManager_Shown(object sender, EventArgs e)
        {
            mn_FullTruyen_Text_Click(null,null);
        }

        private void mnChonHinhAnh_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.Controls.Add(new UChonHinhAnh() { Dock = DockStyle.Fill });
        }
        private void FrmManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (lstFullTruyen != null)
            {
                foreach (var item in lstFullTruyen)
                {
                    item.Dispose();
                }
            }
        }

        List<UTruyenFull> lstFullTruyen = null;
        #region FULL TRUYEN

        private void fullTruyen_ThongTinThieu_Click(object sender, EventArgs e)
        {
            this.Text = "[GET] thông tin thiếu full truyện";
            int run = Program.configuration.TASK_RUN;
            if (run <= 0)
            {
                MessageBox.Show("Nhập số lượng task");
                return;
            }
            lstFullTruyen = new List<UTruyenFull>();
            this.flowLayoutPanel1.Controls.Clear();
            int wControl = this.flowLayoutPanel1.Width / 4 - 12;
            for (int i = 1; i <= run; i++)
            {
                UTruyenFull uFull = new UTruyenFull()
                {
                    Name = "fullTruyen_ChuongTruyen_" + i
                    ,
                    Width = wControl
                };
                lstFullTruyen.Add(uFull);
                this.flowLayoutPanel1.Controls.Add(uFull);
                uFull.GET_THONG_TIN_THIEU();
            }
        }
        private void fullTruyen_GetHinhAnhTruyen_Click(object sender, EventArgs e)
        {
            this.Text = "[GET] ds hình ảnh";
            int run = Program.configuration.TASK_RUN;

            if (run <= 0)
            {
                MessageBox.Show("Nhập số lượng task");
                return;
            }
            lstFullTruyen = new List<UTruyenFull>();
            this.flowLayoutPanel1.Controls.Clear();
            for (int i = 1; i <= run; i++)
            {
                UTruyenFull uFull = new UTruyenFull()
                {
                    Name = "fullTruyen_HinhAnh_" + i
                };
                lstFullTruyen.Add(uFull);
                this.flowLayoutPanel1.Controls.Add(uFull);
                uFull.START_GET_HINH_ANH_TRUYEN();

            }
        }
        #region [GET] ds truyện mới cập nhật

        #endregion

        #region Action

        private void fullTruyen_StopAll_Click(object sender, EventArgs e)
        {
            if (lstFullTruyen == null)
            {
                return;
            }
            foreach (var uFull in lstFullTruyen)
            {
                uFull.ckbStop.Checked = true;

            }
        }

        private void fullTruyen_RunAll_Click(object sender, EventArgs e)
        {
            if (lstFullTruyen == null)
            {
                return;
            }
            foreach (var uFull in lstFullTruyen)
            {
                uFull.ckbStop.Checked = false;

            }

        }

        private void fullTruyen_mnCloseAll_Click(object sender, EventArgs e)
        {
            if (lstFullTruyen != null)
            {
                foreach (var item in lstFullTruyen)
                {
                    item.Dispose();
                }
                this.flowLayoutPanel1.Controls.Clear();
            }
        }

        #endregion

        #endregion

        #region UZipFile

        List<UZipFile> lstUZipFile = null;
        private void mn_Zip_Click(object sender, EventArgs e)
        {
            this.Text = "ZipFile";
            int run = Program.configuration.TASK_RUN;
            if (run <= 0)
            {
                MessageBox.Show("Nhập số lượng task");
                return;
            }
            lstUZipFile = new List<UZipFile>();
            this.flowLayoutPanel1.Controls.Clear();
            int wControl = this.flowLayoutPanel1.Width / 4 - 12;
            for (int i = 1; i <= run; i++)
            {
                UZipFile uFull = new UZipFile()
                {
                    Name = "UZipFile_" + i
                    ,
                    Width = wControl
                };
                lstUZipFile.Add(uFull);
                this.flowLayoutPanel1.Controls.Add(uFull);
                uFull.START_ZIP_FILE();
            }
        }
        #endregion

        #region ggDrive
        private void ggDrive_Tong_Click(object sender, EventArgs e)
        {
            int wControl = this.flowLayoutPanel1.Width / 4 - 6;

            #region GG Truyện
            UGGDrive uDriveTruyen = new UGGDrive()
            {
                Name = "UGGDrive_Control"
                ,
                Width = wControl
            };
            this.flowLayoutPanel1.Controls.Add(uDriveTruyen);
            Task.Factory.StartNew(new Action(async () =>
            {
                while (true)
                {
                    try
                    {
                        this.Invoke(new Action(() =>
                        {
                            uDriveTruyen.lblBaseURL.Text = "GG Drive Truyện";
                        }));
                        await uDriveTruyen.START_UGGDrive_Truyen();
                        this.Invoke(new Action(() =>
                        {
                            uDriveTruyen.lblBaseURL.Text = "GG Drive Chương";
                        }));
                        DataTable dtTruyen = Program.ExcecuteDataTable("select ID,ggd_id,title from tblTruyen where ggd_id != '' and daXuLy_ggd_id is null");
                        foreach (DataRow dr in dtTruyen.Rows)
                        {
                            await uDriveTruyen.START_UGGDrive_Chuong(dr);
                        }
                        uDriveTruyen.WriteLog("DONE Sleep 30'");
                        Thread.Sleep(30 * 60 * 1000);
                    }
                    catch
                    {
                    }
                }
            }));
            #endregion
        }
        #endregion


        #region Truyện + Chương truyện
        private void mn_FullTruyen_Text_Click(object sender, EventArgs e)
        {
            this.mn_MenuFullTruyenTranh.Enabled = false;
            this.menuFullTruyen.Enabled = false;
            this.Text = "[GET] ds truyện mới cập nhật";
            int countRun = Program.configuration.TASK_RUN;
            for (int iThread = 1; iThread <= countRun; iThread++)
            {
                if (iThread > Program.configuration.MAX_TASK_RUNNING) continue;

                UTruyenFull uFull = new UTruyenFull()
                {
                    Name = "fullTruyen_TruyenMoiCapNhat_" + iThread,
                    Width = this.flowLayoutPanel1.Width / 4 - 6,
                };
                this.flowLayoutPanel1.Controls.Add(uFull);
                int pageThread = iThread;
                int iPageRunning = pageThread;
                Task.Factory.StartNew(new Action(() =>
                {
                    while (true)
                    {
                        try
                        {
                            if (iPageRunning > Program.configuration.MAX_TASK_RUNNING)
                            {
                                iPageRunning = pageThread;
                            }
                            uFull.WriteLog("[Start] get FullTruyện mới cập nhật trang: " + iPageRunning);
                            uFull.FullTruyen_TrangMoiCapNhat_Text(iPageRunning);
                            iPageRunning += countRun;
                            uFull.WriteLog("DONE Sleep 15'");
                            Thread.Sleep(15 * 60 * 1000);
                            if (uFull.ckbStop.Checked)
                            {
                                uFull.WriteLog("DONE ckbStop.Checked");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            uFull.WriteLog("Exception: " + ex.Message);
                        }
                    }
                }));
            }
            ggDrive_Tong_Click(null, null);
            mn_FullTruyen_Tranh_Click(null, null);
        }
        #endregion

        #region  [TRUYỆN TRANH] Truyện + Chương truyện 
        private void mn_FullTruyen_Tranh_Click(object sender, EventArgs e)
        {
            UTruyenFull uFull = new UTruyenFull()
            {
                Name = "fullTruyen_TruyenTranhMoiCapNhat",
                Width = this.flowLayoutPanel1.Width / 4 - 6,
            };
            this.flowLayoutPanel1.Controls.Add(uFull);

            Task.Factory.StartNew(new Action(() =>
            {
                while (true)
                {
                    try
                    {
                        uFull.FullTruyen_Tranh();
                        uFull.WriteLog("DONE Sleep 6h");
                        Thread.Sleep(6 * 60 * 60 * 1000);
                        if (uFull.ckbStop.Checked)
                        {
                            uFull.WriteLog("DONE ckbStop.Checked");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        uFull.WriteLog("Exception: " + ex.Message);
                    }
                }
            }));
        }
        #endregion

        private void TextToSpeechToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "[GET] ds truyện mới cập nhật";
            UGSpeechToText uFull = new UGSpeechToText()
            {
                Name = "uUGSpeechToText",
                Width = this.flowLayoutPanel1.Width - 6,
            };
            this.flowLayoutPanel1.Controls.Add(uFull);
            Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    //uFull.SpeechToText("", "", "");
                    uFull.TextToSpeech_Full();
                }
                catch (Exception ex)
                {
                    uFull.WriteLog("Exception: " + ex.Message);
                }
            }));

        }

        private async void removeImageSpamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dtTruyen = Program.ExcecuteDataTable("SELECT * FROM tblTruyen where idLoaiTruyen = 2");

            foreach (DataRow dr in dtTruyen.Rows)
            {
                var ID = (int)dr["ID"];
                string tblDsChuong = Program.GetTableName_dsChuong(ID);
                var dtDsChuong = Program.ExcecuteDataTable("SELECT * FROM " + tblDsChuong + " where refId = " + ID);
                foreach (DataRow drChuong in dtDsChuong.Rows)
                {
                    var IDChuong = (int)drChuong["ID"];
                    var arr_ggd_id = (string)drChuong["arr_ggd_id"];
                    if (string.IsNullOrEmpty(arr_ggd_id)) continue;

                    List<string> lstURL = JsonConvert.DeserializeObject<List<string>>(arr_ggd_id);
                    List<string> new_arr_ggd_id = new List<string>();
                    foreach (var url in lstURL)
                    {
                        bool isSpam = await CheckValid_SpamImage(url);
                        if (isSpam)
                        {
                            continue;
                        }
                        else
                        {
                            new_arr_ggd_id.Add(url);
                        }
                    }
                }
            }
        }

        private async Task<bool> CheckValid_SpamImage(string url)
        {
            string strContent = "";
            string ImagePath = Application.StartupPath + "\\tmp.jpg";
            try
            {

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), ImagePath);
                }

                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("helloworld"), "apikey"); //Added api key in form data
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");

                //form.Add(new StringContent("vie"), "language");
                //form.Add(new StringContent("3"), "ocrengine");

                form.Add(new StringContent("eng"), "language");
                form.Add(new StringContent("2"), "ocrengine");

                byte[] imageData = System.IO.File.ReadAllBytes(ImagePath);
                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");

                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                strContent = await response.Content.ReadAsStringAsync();
                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);
                var lstSpam_Text = new List<string>() { ".net", ".com", ".vn", "http", ".xyz", ".online", ".click", ".blog", ".ink", ".io", };
                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        foreach (var spam_text in lstSpam_Text)
                        {
                            if (ocrResult.ParsedResults[i].ParsedText.Contains(spam_text) || ocrResult.ParsedResults[i].ParsedText.Contains(spam_text.ToUpper()))
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("ERROR: " + strContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(strContent + "\nException: " + ex);
            }
            return false;
        }
    }
}
