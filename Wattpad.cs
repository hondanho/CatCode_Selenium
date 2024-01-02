using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace CatCode_Selenium
{
    public partial class Wattpad : UserControl
    {
         ChromeDriver chromeDriver;
        public Wattpad()
        {
            InitializeComponent();
            Application.ApplicationExit += Application_ApplicationExit;
        }



        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (chromeDriver != null)
                chromeDriver.Quit();
        }

        //session not created: This version of ChromeDriver only supports Chrome version 109
        //Current browser version is 115.0.5790.171 with binary path C:\Program Files\Google\Chrome\Application\chrome.exe
        #region ChuongTruyen

        string idTruyen = "";

        public void WriteLog(string mgs)
        {
            return;// not log
            try
            {
                this.Invoke(new Action(() =>
                {
                    this.richLog.Text = DateTime.Now.ToString() + ": " + mgs + "\n" + this.richLog.Text;
                }));
            }
            catch
            {
            }
        }

        private void SaveFileToLocal(string html_NoiDung, string path)
        {
            if (string.IsNullOrEmpty(html_NoiDung)) return;
            try
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch (Exception ex)
                {
                    this.WriteLog("Exception SaveFileToLocal File.Delete: " + ex.Message);
                }
                System.IO.File.WriteAllText(path, html_NoiDung);
            }
            catch (Exception ex)
            {
                this.WriteLog("Exception SaveFileToLocal: " + ex.Message);
            }
        }


        #endregion

        #region Truyen Moi Cap Nhat

        public void Wattpad_10TrangMoiCapNhat_Text()
        {
            var dtForce = Program.ExcecuteDataTable("select * from tblForceGetTruyen where title_url is not null and title_url like '%wattpad%' ");
            if (dtForce == null)
            {
                this.WriteLog("DB NULL");
                return;
            }
            foreach (DataRow dr in dtForce.Rows)
            {
                var href = (string)dr["title_url"];
                GetDataFromLink(href);
                Program.ExcecuteDataTable("delete tblForceGetTruyen where title_url = @title_url", new Dictionary<string, object>() { { "@title_url", href } });
            }
            GetTruyenMoiCapNhat_Wattpad_Text();
            UpdateTextControl(lblBaseURL, "");
            UpdateTextControl(lblPercent, "");
            UpdateTextControl(lblProcessMessage, "");
        }
        public void GetTruyenMoiCapNhat_Wattpad_Text()
        {
            var dtTruyen = Program.ExcecuteDataTable("select ID, title_url, nguon_url from tblTruyen where nguon_url like 'https%wattpad%' order by id desc ");
            if (dtTruyen == null)
            {
                this.WriteLog("DB NULL");
                return;
            }
            foreach (DataRow dr in dtTruyen.Rows)
            {
                string nguon_url = dr["nguon_url"].ToString();
                try
                {
                    InitChromeDriver();
                    chromeDriver.Navigate().GoToUrl(nguon_url);
                    var lstLinks = chromeDriver.FindElements(By.CssSelector(".story-list .story-list__container .story-list__item a.on-story-preview"))
                        .Select(i => i.GetAttribute("href"))
                        .Where(i => !string.IsNullOrEmpty(i)).ToList();
                    foreach (var link in lstLinks)
                    {

                        try
                        {
                            var dtTruyen_link = Program.ExcecuteDataTable("select 1 from tblTruyen where nguon_url = @nguon_url ", new Dictionary<string, object>() { { "@nguon_url", link } });
                            if (dtTruyen_link.Rows.Count > 0) continue;
                            GetDataFromLink(link);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.WriteLog(ex.Message);
                }
            }
            Thread.Sleep(5000);
            GetTruyenMoiCapNhat_Wattpad_Text();
        }

        private void InitChromeDriver(bool quit = false)
        {
            try
            {
                try
                {
                    if (chromeDriver != null)
                    {
                        chromeDriver.Quit();
                        chromeDriver.Dispose();
                    }

                }
                catch (Exception)
                {
                }

                try
                {
                    foreach (var process in Process.GetProcessesByName("chrome"))
                    {
                        process.Kill();
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    foreach (var process in Process.GetProcessesByName("chromedriver"))
                    {
                        process.Kill();
                    }
                }
                catch (Exception)
                {

                }

            }
            catch
            {
            }
            finally
            {
                chromeDriver = null;
            }
            if (!quit)
            {
                chromeDriver = new ChromeDriver();
            }

        }
        public List<string> lstTruyenTiengAnh = new List<string>();
        private int MAX_TRUYEN = 25;
        public void GetDataFromLink(string href)
        {
            if (lstTruyenTiengAnh.Contains(href))
            {
                return;
            }
            try
            {
                InitChromeDriver();
                chromeDriver.Navigate().GoToUrl(href);
                GetThongTinTruyen(chromeDriver, out string title,
                                            out string tenTacGia,
                                            out string gioiThieu,
                                            out double diemDanhGia,
                                            out int soLuotDanhGia,
                                            out int soLuotDanhXem,
                                            out string trangThaiTruyen,
                                            out string urlHinhAnh,
                                            out string dsTheLoai,
                                            out string description,
                                            out List<string> lstLink
                                            );
                if (string.IsNullOrEmpty(title)) { return; }
                if (!KiemTraTruyenTiengViet(title, gioiThieu, description))
                {
                    if (!lstTruyenTiengAnh.Contains(href))
                        lstTruyenTiengAnh.Add(href);
                    return;
                }
                string title_url = title;
                var dictParam = new Dictionary<string, object>
                            {
                                { "@nguon_url", href },
                                { "@title",title.Length > 255 ? title.Substring(0,254) : title },
                                { "@tenTacGia", tenTacGia },
                                { "@dsTheLoai", dsTheLoai },
                                { "@gioiThieu",gioiThieu.Length > 3999 ? gioiThieu.Substring(0,3999) :gioiThieu },
                                { "@diemDanhGia", Convert.ToInt32(diemDanhGia) },
                                { "@soLuotDanhGia", soLuotDanhGia },
                                { "@soLuotXem", soLuotDanhXem },
                                { "@trangThaiTruyen", trangThaiTruyen },
                                { "@urlHinhAnh", urlHinhAnh },
                                { "@description", description.Length > 3999 ? description.Substring(0,3999): description },
                                { "@idLoaiTruyen", 1 },
                                { "@moiCapNhat", true },
                                { "@title_url", title_url }
                            };

                var dt = Program.ExcecuteDataTable("sp_CreateTruyen", dictParam);
                if (dt != null && dt.Rows.Count != 0)
                {
                    this.GetChuongTruyen_Text(dt.Rows[0], chromeDriver, lstLink);
                    Program.ExcecuteDataTable("update tblTruyen set done = 1 where ID = " + dt.Rows[0]["ID"]);
                    MAX_TRUYEN--;
                    if (MAX_TRUYEN == 0)
                    {
                        InitChromeDriver(quit: true);
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteLog(ex.Message);
            }
        }

        const string pattem = "ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ";

        private bool KiemTraTruyenTiengViet(string title, string gioiThieu, string description)
        {
            return title.Any(c => pattem.Contains(c));
            return title.Any(c => pattem.Contains(c)) || gioiThieu.Any(c => gioiThieu.Contains(c)) || description.Any(c => description.Contains(c));
        }

        private void GetThongTinTruyen(ChromeDriver driver,
            out string title,
            out string tenTacGia,
            out string gioiThieu,
            out double dDiemDanhGia,
            out int iSoLuotDanhGia,
            out int iSoLuotXem,
            out string trangThaiTruyen,
            out string urlHinhAnh,
            out string dsTheLoai,
            out string description,
            out List<string> lstLink
            )
        {

            iSoLuotDanhGia = 0;
            iSoLuotXem = 0;
            dDiemDanhGia = 8.5;
            trangThaiTruyen = "";
            tenTacGia = "";

            description = driver.FindElement(By.XPath("//meta[@name='description']")).GetAttribute("content");
            urlHinhAnh = driver.FindElement(By.XPath("//*[@class=\"component-wrapper\"]/div/div[1]/div[1]/img"))?.GetAttribute("src");
            title = driver.FindElement(By.XPath("//*[@class=\"component-wrapper\"]/div/div[1]/div[2]/span"))?.Text;
            gioiThieu = driver.FindElement(By.XPath("//*[@class=\"component-wrapper\"]/div/div[2]/div[1]/div[2]/div/pre"))?.Text;

            string str_soLuotDanhGia = driver.FindElement(By.XPath("//*[@class=\"component-wrapper\"]/div/div[1]/div[2]/ul/li[2]/div[2]/div[1]/span[2]"))?.Text?.ToLower() ?? "";
            if (str_soLuotDanhGia.Contains("k"))
            {
                str_soLuotDanhGia = str_soLuotDanhGia.Replace("k", "");
                if (double.TryParse(str_soLuotDanhGia, out double dSoLuongDanhGia))
                {
                    dSoLuongDanhGia *= 1000;
                    try
                    {
                        iSoLuotDanhGia = Convert.ToInt32(dSoLuongDanhGia);

                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                int.TryParse(str_soLuotDanhGia, out iSoLuotDanhGia);
            }

            string str_soLuotXem = driver.FindElement(By.XPath("//*[@class=\"component-wrapper\"]/div/div[1]/div[2]/ul/li[1]/div[2]/div[1]/span[2]"))?.Text?.ToLower() ?? "";
            if (str_soLuotXem.Contains("k"))
            {
                str_soLuotXem = str_soLuotXem.Replace("k", "");
                if (double.TryParse(str_soLuotXem, out double dSoLuongXem))
                {
                    dSoLuongXem *= 1000;
                    try
                    {
                        iSoLuotXem = Convert.ToInt32(dSoLuongXem);

                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                int.TryParse(str_soLuotXem, out iSoLuotXem);
            }

            lstLink = new List<string>();

            var tocs = driver.FindElements(By.XPath("//*[@class=\"story-parts\"]/ul/li"));
            foreach (var li in tocs)
            {
                var link = li.FindElements(By.TagName("a")).FirstOrDefault()?.GetAttribute("href");
                if (!lstLink.Contains(link)) lstLink.Add(link);
            }

            if (!string.IsNullOrEmpty(gioiThieu))
            {
                const string newURL = "https://truyenfree.net";
                gioiThieu = gioiThieu
                    .Replace("https://www.wattpad.com", newURL)
                    .Replace("https://www.wattpad.com", newURL)
                    ;
            }
            var arrGioiThieu = gioiThieu.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


            dsTheLoai = arrGioiThieu.FirstOrDefault(i => i.ToLower().StartsWith("Thể Loại".ToLower()));
            if (!string.IsNullOrEmpty(dsTheLoai))
            {
                dsTheLoai = dsTheLoai.Replace("Thể Loại:", "").Replace("Thể Loại", "").Trim();
            }

            tenTacGia = arrGioiThieu.FirstOrDefault(i => i.ToLower().StartsWith("Tác Giả".ToLower()));
            if (!string.IsNullOrEmpty(tenTacGia))
            {
                tenTacGia = tenTacGia.Replace("Tác Giả:", "").Replace("Tác Giả", "").Trim();
            }

        }


        public void GetChuongTruyen_Text(DataRow dr, ChromeDriver driver, List<string> listURLChuong)
        {

            string baseURL = dr["nguon_url"].ToString();
            idTruyen = dr["ID"].ToString();
            string tenTruyen = dr["title"].ToString();
            string title = dr["title"].ToString();
            this.UpdateTextControl(this.lblProcessMessage, "[" + idTruyen + "]" + baseURL);
            this.UpdateTextControl(this.lblPercent, "#NA");
            if (!baseURL.EndsWith("/")) baseURL += "/";

            Program.ExcecuteNoneQuery("UPDATE tblTruyen set soLuongChuong = " + listURLChuong.Count + ",done = 0  where ID = " + idTruyen);

            if (!listURLChuong.Any()) return;

            SetupProgessBar(listURLChuong.Count);
            var direc = string.Format("C:/Working/CatCode_Selenium/CatCode_Selenium/bin/Release/DataTruyen/My Drive/DataTruyen/{0}/", idTruyen);
            if (!Directory.Exists(direc)) Directory.CreateDirectory(direc);

            var dictParam = new Dictionary<string, object>();
            dictParam["@refID"] = idTruyen;
            var lstChuongDaCo = Program.ExcecuteDataTable("sp_dsChuongDaCo", dictParam).AsEnumerable().Select(d => new ChuongTruyen()
            {
                ID = Convert.ToInt32(d["ID"]),
                Chuong = Convert.ToInt32(d["chuong"]),
                RefID = Convert.ToInt32(d["refID"]),
                title = Convert.ToString(d["title"]),
                nguon_url = Convert.ToString(d["nguon_url"]),
                uriChuong = Convert.ToString(d["uriChuong"]),
            }).ToList();

            for (int chuong = 1; chuong <= listURLChuong.Count; chuong++)
            {
                var url = listURLChuong[chuong - 1];
                driver.Navigate().GoToUrl(url);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, 999999);");
                Thread.Sleep(200);
                var contents = driver.FindElements(By.CssSelector(".container .panel.panel-reading pre"));
                if (!contents.Any()) continue;
                var textChuong = "";
                foreach (var item in contents)
                {
                    textChuong += "<pre>" + item.Text + "</pre>";
                }
                if (string.IsNullOrEmpty(textChuong)) continue;
                dictParam["@chuong"] = chuong;

                var urlRun = url;
                if (!urlRun.EndsWith("/")) urlRun += "/";
                var uriChuong = "chuong-" + chuong;
                var local_path = string.Format("C:/Working/CatCode_Selenium/CatCode_Selenium/bin/Release/DataTruyen/My Drive/DataTruyen/{0}/{1}.txt", idTruyen, uriChuong);
                dictParam["@nguon_url"] = urlRun;
                dictParam["@uriChuong"] = uriChuong;
                if (File.Exists(local_path))
                {
                    //checked sever
                    if (lstChuongDaCo.Any(i => i.nguon_url == urlRun && i.uriChuong == uriChuong))
                    {
                        continue;
                    }
                }
                try
                {
                    string html_NoiDung = textChuong;
                    var tenChuong = driver.FindElement(By.CssSelector("#story-reading header.panel-reading h1"))?.Text;
                    tenChuong = string.IsNullOrEmpty(tenChuong) ? chuong.ToString() : tenChuong;
                    tenChuong = tenChuong.Replace("Chương " + chuong + ".", "");
                    tenChuong = tenChuong.Replace("Chương " + chuong + ":", "");
                    tenChuong = tenChuong.Replace("Chương " + chuong, "");
                    tenChuong = tenChuong.Trim();
                    tenChuong = string.IsNullOrEmpty(tenChuong) ? chuong.ToString() : tenChuong;

                    ChangeProgessBar(chuong, "[" + idTruyen + "] " + tenTruyen + "\nChương: " + tenChuong + "\n" + url);
                    SaveFileToLocal(html_NoiDung, local_path);
                    dictParam["@title"] = tenChuong;
                    dictParam["@done"] = File.Exists(local_path);
                    Program.ExcecuteNoneQuery("sp_CreateChuongTruyen", dictParam);
                }
                catch (Exception ex)
                {
                    this.WriteLog(String.Format("ID Truyen:{0} Chuong:{1} URL:{2} \nException:{3}", idTruyen, chuong, urlRun, ex.Message));
                }
            }
        }


        #endregion

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
        public void UpdateTextControl(Control ctrl, string txt)
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
    }
}
