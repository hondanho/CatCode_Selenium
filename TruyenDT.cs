using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic; 
using System.Data; 
using System.IO;
using System.Linq; 
using System.Threading; 
using System.Windows.Forms; 

namespace CatCode_Selenium
{
    public partial class TruyenDT : UserControl
    {
         ChromeDriver chromeDriver;
        public TruyenDT()
        {
            InitializeComponent();
            chromeDriver = new ChromeDriver();
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            chromeDriver.Quit();
        }

        //session not created: This version of ChromeDriver only supports Chrome version 109
        //Current browser version is 115.0.5790.171 with binary path C:\Program Files\Google\Chrome\Application\chrome.exe
        #region ChuongTruyen

        string idTruyen = "";

        public void WriteLog(string mgs)
        {
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

        public void TruyenDT_10TrangMoiCapNhat_Text()
        {

            var dtForce = Program.ExcecuteDataTable("select * from tblForceGetTruyen where title_url is not null and title_url like '%truyenhdt%' ");
            foreach (DataRow dr in dtForce.Rows)
            {
                var href = (string)dr["title_url"];
                GetDataFromLink(href);
                Program.ExcecuteDataTable("delete tblForceGetTruyen where title_url = @title_url", new Dictionary<string, object>() { { "@title_url", href } });
            }

            UpdateTextControl(lblBaseURL, "");
            UpdateTextControl(lblPercent, "");
            UpdateTextControl(lblProcessMessage, "");
        }
        public void GetTruyenMoiCapNhat_TruyenDT_Text(int page)
        {
            const string baseURL = "https://truyenfull.vn/danh-sach/truyen-moi/trang-{0}/";
            string url = string.Format(baseURL, page);
            UpdateTextControl(lblBaseURL, url);
            try
            {
                HtmlAgilityPack.HtmlDocument doc = Program.LoadDocument(url);
                var lstNode1 = doc.DocumentNode.SelectNodes("//h3[@class='truyen-title']");
                if (lstNode1 == null || !lstNode1.Any())
                {
                    return;
                }
                int step = 0;
                SetupProgessBar(lstNode1.Count);
                foreach (HtmlAgilityPack.HtmlNode node in lstNode1)
                {
                    step++;
                    var lstNode2 = node.SelectNodes(".//a[@href]").ToList();
                    foreach (HtmlAgilityPack.HtmlNode node2 in lstNode2)
                    {
                        try
                        {
                            var href = node2.GetAttributeValue("href", "");
                            UpdateTextControl(lblProcessMessage, href);
                            if (string.IsNullOrEmpty(href)) { continue; }
                            ChangeProgessBar(step, href);
                            GetDataFromLink(href);
                        }
                        catch (Exception ex)
                        {
                            this.WriteLog("[" + String.Format("Trang : {0}", page) + "]" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("[" + String.Format("Trang:{0}", page) + "]" + ex.Message);
            }
        }

        public void GetDataFromLink(string href)
        {
            href = "https://truyenhdt.com/truyen/quy-y-sat/";
            href = "https://truyenhdt.com/truyen/nam-chinh-y-dien-roi/";
            try
            {
                chromeDriver.Navigate().GoToUrl(href);
                GetThongTinTruyen(chromeDriver, out string title,
                                            out string tenTacGia,
                                            out string gioiThieu,
                                            out double diemDanhGia,
                                            out int soLuotDanhGia,
                                            out string trangThaiTruyen,
                                            out string urlHinhAnh,
                                            out string dsTheLoai,
                                            out string description,
                                            out List<string> lstLink
                                            );
                if (string.IsNullOrEmpty(title)) { return; }

                var dictParam = new Dictionary<string, object>
                            {
                                { "@nguon_url", href },
                                { "@title",title.Length > 255 ? title.Substring(0,254) : title },
                                { "@tenTacGia", tenTacGia },
                                { "@dsTheLoai", dsTheLoai },
                                { "@gioiThieu",gioiThieu.Length > 3999 ? gioiThieu.Substring(0,3999) :gioiThieu },
                                { "@diemDanhGia", Convert.ToInt32(diemDanhGia) },
                                { "@soLuotDanhGia", soLuotDanhGia },
                                { "@trangThaiTruyen", trangThaiTruyen },
                                { "@urlHinhAnh", urlHinhAnh },
                                { "@description", description.Length > 3999 ? description.Substring(0,3999): description },
                                { "@idLoaiTruyen", 1 },
                                { "@moiCapNhat", true }
                            };

                var dt = Program.ExcecuteDataTable("sp_CreateTruyen", dictParam);
                if (dt != null && dt.Rows.Count != 0)
                {
                    this.GetChuongTruyen_Text(dt.Rows[0], chromeDriver, lstLink);
          
                    Program.ExcecuteDataTable("update tblTruyen set done = 1 where ID = " + dt.Rows[0]["ID"]);
                }



            }
            catch (Exception ex)
            {
            }

        }
         

        private void GetThongTinTruyen(ChromeDriver driver,
            out string title,
            out string tenTacGia,
            out string gioiThieu,
            out double dDiemDanhGia,
            out int iSoLuotDanhGia,
            out string trangThaiTruyen,
            out string urlHinhAnh,
            out string dsTheLoai,
            out string description,
            out List<string> lstLink
            )
        {

            description = driver.FindElement(By.XPath("//meta[@property='og:description']")).GetAttribute("content");
            urlHinhAnh = driver.FindElement(By.XPath("//meta[@property='og:image']")).GetAttribute("content");
            title = driver.FindElement(By.XPath("//*[@id=\"html\"]/body/div[5]/div[1]/div[1]/div[1]/h1"))?.Text;
            trangThaiTruyen = "";
            tenTacGia = "";
            foreach (var tr in driver.FindElements(By.XPath("//*[@id=\"thong_tin\"]/table/tbody/tr")))
            {
                var text = tr.Text;
                if (text.StartsWith("Tác Giả:"))
                {
                    tenTacGia = text.Replace("Tác Giả:", "").Trim();

                }
                else if (text.StartsWith("Tình Trạng:"))
                {
                    trangThaiTruyen = text.Replace("Tình Trạng:", "").Trim();
                }
            }
            trangThaiTruyen = trangThaiTruyen.Replace("Hoàn Thành", "Full")
                .Replace("Đầy đủ", "Full")
                .Replace("Đang Cập Nhật", "Đang ra")
                .Replace("Tạm Ngưng", "");
            string str_diemDanhGia = driver.FindElement(By.XPath("//*[@id=\"rate\"]/div[3]/strong[1]"))?.Text;
            string str_soLuotDanhGia = driver.FindElement(By.XPath("//*[@id=\"rate\"]/div[3]/strong[2]"))?.Text;
            driver.FindElement(By.XPath("//*[@id=\"truyen_tabs\"]/ul/li[2]/a"))?.Click();

            gioiThieu = driver.FindElement(By.XPath("//*[@id=\"gioi_thieu\"]/div"))?.Text;
            lstLink = new List<string>();
            bool stopALl = false;
            while (!stopALl)
            {
                try
                {
                    for (int i = 1; i <= 50; i++)
                    {
                        try
                        {
                            if (driver.FindElements(By.XPath("//*[@id=\"dsc\"]/ul[1]/li[" + i + "]/div/div[2]")).Count > 0)
                            {
                                stopALl = true;
                                break;
                            }
                            var item = driver.FindElement(By.XPath("//*[@id=\"dsc\"]/ul[1]/li[" + i + "]/div/div/a"));
                            var link = item.GetAttribute("href");
                            if (!lstLink.Contains(link))
                                lstLink.Add(link);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (stopALl)
                    {
                        break;
                    }
                    bool nextPage = false;
                    foreach (var li in driver.FindElements(By.XPath("//*[@id=\"dsc\"]/ul[2]/li")))
                    {
                        var text = li.Text;
                        if (text == "»")
                        {
                            li.Click();
                            Thread.Sleep(3000);
                            nextPage = true;
                            break;
                        }
                    }
                    if (!nextPage) break;
                }
                catch (Exception)
                {
                }
            }

            if (!string.IsNullOrEmpty(gioiThieu))
            {
                const string newURL = "https://truyenfree.net";
                gioiThieu = gioiThieu
                    .Replace("https://truyenhdt.com", newURL)
                    .Replace("https://truyenhdt.vn", newURL)
                    ;
            }
            double.TryParse(str_diemDanhGia, out dDiemDanhGia);
            int.TryParse(str_soLuotDanhGia, out iSoLuotDanhGia);

            //dsTheLoai = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[2]/div[2]/div[2]")?.InnerText;
            //if (string.IsNullOrEmpty(dsTheLoai)) dsTheLoai = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]")?.InnerText;

            var lstTheLoai = new List<string>();

            foreach (var li in driver.FindElements(By.XPath("//*[@id=\"html\"]/body/div[5]/div[1]/div[1]/div[1]/div[2]/div[5]/span")))
            {
                var text = li.Text;
                if (string.IsNullOrEmpty(text) || lstTheLoai.Contains(text)) continue;
                lstTheLoai.Add(text);
            }
            dsTheLoai = string.Join(", ", lstTheLoai);
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
                List<IWebElement> xemChuong;
                do
                {
                    xemChuong = driver.FindElements(By.XPath("//*[@id=\"user_pass_chap\"]/span")).ToList();
                    if(xemChuong.Any())
                    {
                        xemChuong.First().Click();
                        Thread.Sleep(500);
                    }
                } while (xemChuong.Any());
                var contents = driver.FindElements(By.ClassName("reading"));
                if(!contents.Any())
                {
                    continue;
                }
                var textChuong = contents.First().Text;
                if (string.IsNullOrEmpty(textChuong))
                {
                    continue;
                }
                dictParam["@chuong"] = chuong;

                var urlRun = url;
                if (!urlRun.EndsWith("/")) urlRun += "/";
                var uriChuong = urlRun.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last();
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
                    HtmlAgilityPack.HtmlDocument doc = Program.LoadDocument(urlRun);
                    var html_NoiDung = doc.GetElementbyId("chapter-c")?.InnerHtml;
                    if (string.IsNullOrEmpty(html_NoiDung))
                    {
                        if (urlRun.EndsWith("/")) urlRun = urlRun.Substring(0, urlRun.Length - 1);
                        urlRun += "-";
                        if (!urlRun.EndsWith("/")) urlRun += "/";
                        doc = Program.LoadDocument(urlRun, 5);
                        html_NoiDung = doc.GetElementbyId("chapter-c")?.InnerHtml;
                    }
                    html_NoiDung = html_NoiDung ?? "";
                    string startDiv = "<div class=\"ads\">";
                    string endDiv = "</div>";
                    while (html_NoiDung.Contains(startDiv))
                    {
                        int indexStart = html_NoiDung.IndexOf(startDiv);
                        int indexEnd = html_NoiDung.IndexOf(endDiv);
                        int lenthSub = indexEnd + endDiv.Length - indexStart;
                        try
                        {
                            var htmlQC = html_NoiDung.Substring(indexStart, lenthSub);
                            html_NoiDung = html_NoiDung.Replace(htmlQC, "<br>");
                        }
                        catch
                        {
                            break;
                        }
                    }
                    var tenChuong = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div/div/h2/a/text()")?.InnerText;
                    if (string.IsNullOrEmpty(tenChuong))
                    {
                        tenChuong = doc.DocumentNode.SelectSingleNode("//*[@id=\"chapter-big-container\"]/div/div/h2/a/text()")?.InnerText;
                    }
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
