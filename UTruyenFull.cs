using CloudinaryDotNet.Actions;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
using System.Windows.Interop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CatCode_Selenium
{
    public partial class UTruyenFull : UserControl
    {
         public UTruyenFull()
        {
            InitializeComponent();
        }


        #region ChuongTruyen

        int soLuongDaLayThanhCong = 0;
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

        private List<string> GetListChuong(string baseURL)
        {
            int trang = 0;
            List<string> lstURL = new List<string>();
            int max_miss = 5;
            int max_miss_trang = 20;

            while (trang < 500)
            {
                trang++;
                string url = baseURL;
                if (trang > 1)
                {
                    url = url + "trang-" + trang + "/";
                }
                try
                {
                    this.UpdateTextControl(this.lblProcessMessage, "Danh sách chương trang:" + trang + ", URL: " + url);
                    var doc = Program.LoadDocument(url);
                    if (doc == null || doc.DocumentNode == null)
                    {
                        max_miss_trang--;
                        if (max_miss_trang == 0)
                        {
                            return lstURL;
                        }
                        continue;
                    }
                    var lstNode = doc.DocumentNode.SelectNodes("//*[@id=\"list-chapter\"]");
                    if (lstNode == null || !lstNode.Any())
                    {
                        max_miss_trang--;
                        continue;
                    }
                    max_miss_trang--;
                    if (max_miss_trang == 0)
                    {
                        return lstURL;
                    }
                    var node = lstNode.FirstOrDefault();
                    if (node == null)
                    {
                        continue;
                    }
                    var lstItem = node.SelectNodes(".//a[@href]");
                    if (lstItem == null || !lstItem.Any())
                    {
                        max_miss--;
                        if (max_miss == 0)
                        {
                            return lstURL;
                        }
                        continue;
                    }
                    var newLink = lstItem.Select(link => link.GetAttributeValue("href", string.Empty)).ToList();
                    var newNotLike = newLink?.Where(link => !lstURL.Contains(link)
                         && link != url && link != baseURL
                         && !link.Contains("utm_source")
                         && !link.Contains("#list-chapter")
                         && !link.Contains("#chapter-list")
                         && link.Contains("http")
                    ).Distinct().ToList();
                    if (newNotLike == null || !newNotLike.Any())
                    {
                        max_miss--;
                        if (max_miss == 0)
                        {
                            return lstURL;
                        }
                        if (max_miss >= 3)
                        {
                            continue;
                        }
                        break;
                    }
                    lstURL.AddRange(newNotLike);
                }
                catch (Exception ex)
                {
                    this.WriteLog("[ID] = " + idTruyen + " - Exception GetListChuong: " + ex.Message);
                    if (max_miss >= 3)
                    {
                        continue;
                    }
                    break;
                }
            }
            return lstURL.Distinct().ToList();
        }
        #endregion

        #region Truyen Moi Cap Nhat

        public void FullTruyen_TrangMoiCapNhat_Text(int page)
        {
            DataTable dtForce;
            do
            {
                dtForce = Program.ExcecuteDataTable("select TOP(1) * from tblForceGetTruyen where title_url is not null and title_url like '%fulltruyen%' ");
                foreach (DataRow dr in dtForce.Rows)
                {
                    var href = (string)dr["title_url"];
                    Program.ExcecuteDataTable("delete tblForceGetTruyen where title_url = @title_url", new Dictionary<string, object>() { { "@title_url", href } });
                    GetDataFromLink(href);
                }
            }
            while (dtForce != null && dtForce.Rows.Count != 0);

            GetTruyenMoiCapNhat_FullTruyen_Text(page);
            UpdateTextControl(lblBaseURL, "");
            UpdateTextControl(lblPercent, "");
            UpdateTextControl(lblProcessMessage, "");
        }
        public void GetTruyenMoiCapNhat_FullTruyen_Text(int page)
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
                            this.WriteLog("[" + String.Format("Trang: {0}", page) + "]" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("[" + String.Format("Trang:{0}", page) + "]" + ex.Message);
            }
        }

        private void GetDataFromLink(string href)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument doc_2 = Program.LoadDocument(href);

                GetThongTinTruyen(doc_2, out string title,
                                            out string tenTacGia,
                                            out string gioiThieu,
                                            out double diemDanhGia,
                                            out int soLuotDanhGia,
                                            out string trangThaiTruyen,
                                            out string urlHinhAnh,
                                            out string dsTheLoai,
                                            out string description
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
                    this.GetChuongTruyen_Text(dt.Rows[0]);
                    Program.ExcecuteDataTable("update tblTruyen set done = 1 where ID = " + dt.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void GetThongTinTruyen(HtmlAgilityPack.HtmlDocument doc_2, out string title,
            out string tenTacGia,
            out string gioiThieu,
            out double dDiemDanhGia,
            out int iSoLuotDanhGia,
            out string trangThaiTruyen,
            out string urlHinhAnh,
            out string dsTheLoai,
            out string description
            )
        {

            description = "";
            var list = doc_2.DocumentNode.SelectNodes("//meta");
            if (list != null && list.Any())
            {
                foreach (var node in list)
                {
                    string name = node.GetAttributeValue("name", "");
                    if (name == "description")
                    {
                        description = node.GetAttributeValue("content", "");
                        break;
                    }
                }
            }



            title = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/h3")?.InnerText;
            if (string.IsNullOrEmpty(title)) title = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/h3")?.InnerText;

            tenTacGia = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[2]/div[2]/div[1]/a")?.InnerText;
            if (string.IsNullOrEmpty(tenTacGia)) tenTacGia = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[2]/div[1]/a")?.InnerText;

            gioiThieu = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[3]/div[2]")?.InnerHtml;
            if (string.IsNullOrEmpty(gioiThieu)) gioiThieu = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]")?.InnerHtml;
            if (!string.IsNullOrEmpty(gioiThieu))
            {
                const string newURL = "https://truyenfree.net";
                gioiThieu = gioiThieu
                    .Replace("https://truyenfull.vn", newURL)
                    .Replace("https://truyenfull.com", newURL)
                    .Replace("http://truyenfull.vn", newURL)
                    .Replace("http://truyenfull.com", newURL)
                    .Replace("truyenfull.com/", newURL)
                    .Replace("truyenfull.com/", newURL)
                    ;
            }

            var str_diemDanhGia = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[3]/div[1]/div[2]/em/strong[1]/span")?.InnerText;
            if (string.IsNullOrEmpty(str_diemDanhGia)) str_diemDanhGia = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[3]/div[2]/em/strong[1]/span")?.InnerText;
            double.TryParse(str_diemDanhGia, out dDiemDanhGia);

            var str_soLuotDanhGia = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[3]/div[1]/div[2]/em/strong[2]/span")?.InnerText;
            if (string.IsNullOrEmpty(str_soLuotDanhGia)) str_soLuotDanhGia = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[3]/div[2]/em/strong[2]/span")?.InnerText;
            int.TryParse(str_soLuotDanhGia, out iSoLuotDanhGia);

            trangThaiTruyen = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[2]/div[2]/div[4]/span")?.InnerText;
            if (string.IsNullOrEmpty(trangThaiTruyen)) trangThaiTruyen = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[2]/div[3]/span")?.InnerText;

            urlHinhAnh = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[2]/div[1]/div/img")?.GetAttributeValue("src", "");
            if (string.IsNullOrEmpty(urlHinhAnh)) urlHinhAnh = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[1]/div/img")?.GetAttributeValue("src", "");

            dsTheLoai = doc_2.DocumentNode.SelectSingleNode("/html/body/div[2]/div[4]/div[1]/div[1]/div[2]/div[2]/div[2]")?.InnerText;
            if (string.IsNullOrEmpty(dsTheLoai)) dsTheLoai = doc_2.DocumentNode.SelectSingleNode("/html/body/div[1]/div[2]/div[1]/div[1]/div[2]/div[2]/div[2]")?.InnerText;

            trangThaiTruyen = trangThaiTruyen ?? "";
            dsTheLoai = dsTheLoai ?? "";
            dsTheLoai = dsTheLoai.ToString().Replace("Thể loại:", "");
            tenTacGia = tenTacGia ?? "";
        }


        public void GetChuongTruyen_Text(DataRow dr)
        {
            string baseURL = dr["nguon_url"].ToString();
            idTruyen = dr["ID"].ToString();
            string tenTruyen = dr["title"].ToString();
            string title = dr["title"].ToString();
            this.UpdateTextControl(this.lblProcessMessage, "[" + idTruyen + "]" + baseURL);
            this.UpdateTextControl(this.lblPercent, "#NA");
            if (!baseURL.EndsWith("/")) baseURL += "/";
            if (baseURL.Contains("gap-lai-quan-tim-kiem-tinh-yeu"))
            {
                return;
            }
            var listURLChuong = GetListChuong(baseURL);
            Program.ExcecuteNoneQuery("UPDATE tblTruyen set soLuongChuong = " + listURLChuong.Count + ",done = 0  where ID = " + idTruyen);

            if (!listURLChuong.Any()) return;

            SetupProgessBar(listURLChuong.Count);
            var direc = string.Format(Program.configuration.OUTPUT_DataTruyen + "/{0}/", idTruyen);
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

                dictParam["@chuong"] = chuong;

                var urlRun = url;
                if (!urlRun.EndsWith("/")) urlRun += "/";
                var uriChuong = urlRun.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last();
                var local_path = string.Format(Program.configuration.OUTPUT_DataTruyen + "/{0}/{1}.txt", idTruyen, uriChuong);
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

        #region HINH ANH TRUYEN

        public void START_GET_HINH_ANH_TRUYEN()
        {
            var dtTruyen = Program.ExcecuteDataTable("SELECT TOP(100) ID,urlHinhAnh,title_url FROM tblTruyen where title_url is null");
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
                    GetHinhAnhTruyen(dr);
                }
                UpdateTextControl(lblBaseURL, "Thành công: " + soLuongDaLayThanhCong);
                UpdateTextControl(lblPercent, "");
                UpdateTextControl(lblProcessMessage, "");
            }));
        }

        private void GetHinhAnhTruyen(DataRow dr)
        {
            try
            {

                string ID = Convert.ToString(dr["ID"]);
                string urlHinhAnh = Convert.ToString(dr["urlHinhAnh"]);
                string title_url = Convert.ToString(dr["title_url"]);
                var dictParam = new Dictionary<string, object>();
                dictParam["@ID"] = ID;
                int rowIndex = dr.Table.Rows.IndexOf(dr);
                ChangeProgessBar(rowIndex + 1, " [URL] " + title_url);

                if (string.IsNullOrEmpty(urlHinhAnh)) return;

                var urlHinhAnh_local = title_url;
                if (urlHinhAnh_local.Contains("/")
                    || urlHinhAnh_local.Contains("\\")
                    || urlHinhAnh_local.Contains(".")
                    || urlHinhAnh_local.Contains("--")
                    || urlHinhAnh_local.Contains(" "))

                {
                    urlHinhAnh_local = urlHinhAnh_local
                      .Replace(" ", "-")
                      .Replace("/", "-")
                      .Replace(".", "-")
                      .Replace("--", "-");
                }

                try
                {
                    if (urlHinhAnh_local.EndsWith("-"))
                    {
                        urlHinhAnh_local = urlHinhAnh_local.Substring(0, urlHinhAnh_local.Length - 1);
                    }
                    urlHinhAnh_local = "DataHinhAnh\\" + urlHinhAnh_local + ".png";
                    if (!File.Exists(urlHinhAnh_local))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(urlHinhAnh), urlHinhAnh_local);
                        }
                    }
                    ChangeProgessBar(rowIndex + 1, " [Local Path]" + urlHinhAnh_local);
                    dictParam["@urlHinhAnh_local"] = urlHinhAnh_local;
                    Program.ExcecuteNoneQuery("update tblTruyen set urlHinhAnh_local = @urlHinhAnh_local where ID = @ID", dictParam);
                    soLuongDaLayThanhCong++;
                }
                catch (Exception ex)
                {
                    Program.ExcecuteNoneQuery("update tblTruyen set loiHinhAnh = 1 where ID = @ID", dictParam);
                    if (!ex.Message.Contains("(404)"))
                    {
                        this.WriteLog("[ID: " + ID + "] " + ex.Message + "\n\t - URL Local: " + urlHinhAnh_local + "\n\t - URL Download" + urlHinhAnh);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region GET THONG TIN THIEU
        public void GET_THONG_TIN_THIEU()
        {
            int pid = Process.GetCurrentProcess().Id;
            var dtTruyen = Program.ExcecuteDataTable("sp_dsTruyenThieuThongTin");
            if (dtTruyen.Rows.Count == 0)
            {
                this.Dispose();
            }
            SetupProgessBar(dtTruyen.Rows.Count);
            Task.Factory.StartNew(new Action(() =>
            {
                foreach (DataRow dr in dtTruyen.Rows)
                {
                    GetThongTinThieu(dr);
                }
                if (!this.ckbStop.Checked)
                {
                    GET_THONG_TIN_THIEU();
                }
            }));
        }
        private void GetThongTinThieu(DataRow dr)
        {

            string ID = dr["ID"].ToString();
            string url = dr["nguon_url"].ToString();
            var dictParam = new Dictionary<string, object>();
            dictParam.Add("@ID", ID);

            try
            {
                HtmlAgilityPack.HtmlDocument doc = Program.LoadDocument(url);
                ChangeProgessBar(dr.Table.Rows.IndexOf(dr) + 1, url);
                var description = "";
                var list = doc.DocumentNode.SelectNodes("//meta");
                if (list != null && list.Any())
                {
                    foreach (var node in list)
                    {
                        string name = node.GetAttributeValue("name", "");
                        if (name == "description")
                        {
                            description = node.GetAttributeValue("content", "");
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(description))
                {
                    Program.ExcecuteNoneQuery("UPDATE tblTruyen SET daXuLy_ThongTinThieu = 0 WHERE ID = @ID", dictParam);
                }
                else
                {
                    dictParam.Add("@description", description);
                    Program.ExcecuteNoneQuery("UPDATE tblTruyen SET description = @description, daXuLy_ThongTinThieu = 1 WHERE ID = @ID", dictParam);
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("URL: " + url + "\n Exception: " + ex.Message);
            }
        }

        #endregion


        #region GET_TRUYEN_TRANH_MOI_CAP_NHAT
        public void FullTruyen_Tranh()
        {
            var dtURLTruyenTranhDaChay = Program.ExcecuteDataTable("SELECT nguon_url FROM tblTruyen where idLoaiTruyen = 2 and done is null");
            var lstURL_DaChay = dtURLTruyenTranhDaChay.AsEnumerable().Select(dr => dr["nguon_url"].ToString()).ToList();
            for (int page = 1; page <= 10; page++)
            {
                GetTruyenTranhMoiCapNhat(page, ref lstURL_DaChay);
            }
            UpdateTextControl(lblBaseURL, "");
            UpdateTextControl(lblPercent, "");
            UpdateTextControl(lblProcessMessage, "");
        }
        public void GetTruyenTranhMoiCapNhat(int page, ref List<string> lstURL_DaChay)
        {
            const string baseURL = "https://nettruyenfull.com/tim-truyen?page={0}";
            string url = string.Format(baseURL, page);
            UpdateTextControl(lblBaseURL, url);
            try
            {
                HtmlAgilityPack.HtmlDocument doc = Program.LoadDocument(url);
                var lstNode1 = doc.DocumentNode.SelectNodes("//*[@id=\"ctl00_divCenter\"]/div[2]/div/div/div/div[@class='item']");
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
                            if (string.IsNullOrEmpty(href) || lstURL_DaChay.Contains(href) || href.Contains("chap-")) continue;
                            lstURL_DaChay.Add(href);
                            UpdateTextControl(lblProcessMessage, href);
                            if (string.IsNullOrEmpty(href)) { continue; }
                            ChangeProgessBar(step, href);
                            HtmlAgilityPack.HtmlDocument doc_2 = Program.LoadDocument(href);
                            GetThongTinTruyenTranh(doc_2, out string title,
                                        out string tenTacGia,
                                        out string gioiThieu,
                                        out double diemDanhGia,
                                        out int soLuotDanhGia,
                                        out string trangThaiTruyen,
                                        out string urlHinhAnh,
                                        out string dsTheLoai,
                                        out string description
                                        );
                            if (string.IsNullOrEmpty(title)) { continue; }

                            var dictParam = new Dictionary<string, object>
                            {
                                { "@nguon_url", href },
                                { "@title",title.Length> 255 ? title.Substring(0,254) :title  },
                                { "@tenTacGia", tenTacGia },
                                { "@dsTheLoai", dsTheLoai },
                                { "@gioiThieu",gioiThieu.Length > 3999 ? gioiThieu.Substring(0,3999) :gioiThieu },
                                { "@diemDanhGia", Convert.ToInt32(diemDanhGia) },
                                { "@soLuotDanhGia", soLuotDanhGia },
                                { "@trangThaiTruyen", trangThaiTruyen },
                                { "@urlHinhAnh", urlHinhAnh },
                                { "@description", description.Length > 3999 ?description.Substring(0,3999) : description },
                                { "@idLoaiTruyen", 2 },//2: Truyện Tranh
                                { "@moiCapNhat", true }
                            };

                            var dt = Program.ExcecuteDataTable("sp_CreateTruyen", dictParam);
                            if (dt != null && dt.Rows.Count != 0)
                            {
                                this.GetChuongTruyenTranh(dt.Rows[0]);
                                Program.ExcecuteDataTable("update tblTruyen set done = 1 where ID = " + dt.Rows[0]["ID"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.WriteLog("[" + String.Format("Trang: {0}", page) + "]" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteLog("[" + String.Format("Trang: {0}", page) + "]" + ex.Message);
            }
        }
        private void GetThongTinTruyenTranh(HtmlAgilityPack.HtmlDocument doc_2, out string title,
          out string tenTacGia,
          out string gioiThieu,
          out double dDiemDanhGia,
          out int iSoLuotDanhGia,
          out string trangThaiTruyen,
          out string urlHinhAnh,
          out string dsTheLoai,
          out string description
          )
        {

            description = "";
            var list = doc_2.DocumentNode.SelectNodes("//meta");
            if (list != null && list.Any())
            {
                foreach (var node in list)
                {
                    string name = node.GetAttributeValue("name", "");
                    if (name == "description")
                    {
                        description = node.GetAttributeValue("content", "");
                        break;
                    }
                }
            }



            title = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/article/h1")?.InnerText;
            if (string.IsNullOrEmpty(title)) title = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/ul/li[3]/a/span")?.InnerText;

            tenTacGia = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/article/div[1]/div/div[2]/ul/li[1]/p[2]")?.InnerText;
            if (tenTacGia == "Đang cập nhật") tenTacGia = null;

            gioiThieu = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/article/div[2]/p")?.InnerHtml;
            if (string.IsNullOrEmpty(gioiThieu)) gioiThieu = description;
            if (!string.IsNullOrEmpty(gioiThieu))
            {
                const string newURL = "https://truyenfree.net";
                gioiThieu = gioiThieu
                    .Replace("https://truyenfull.vn", newURL)
                    .Replace("https://truyenfull.com", newURL)
                    .Replace("http://truyenfull.vn", newURL)
                    .Replace("http://truyenfull.com", newURL)
                    .Replace("truyenfull.com/", newURL)
                    .Replace("truyenfull.com/", newURL)

                    .Replace("nettruyenfull.com/", newURL)
                    .Replace("https://nettruyenfull.com/", newURL)
                    .Replace("nettruyenfull.com/truyen-tranh", newURL)
                    .Replace("https://nettruyenfull.com/truyen-tranh", newURL)
                    ;
            }

            iSoLuotDanhGia = 0;
            dDiemDanhGia = 0;

            trangThaiTruyen = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/article/div[1]/div/div[2]/ul/li[2]/p[2]")?.InnerText;

            urlHinhAnh = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/article/div[1]/div/div[1]/img")?.GetAttributeValue("src", "");

            dsTheLoai = doc_2.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div/div[1]/article/div[1]/div/div[2]/ul/li[3]/p[2]")?.InnerText;
            dsTheLoai = dsTheLoai?.Replace("\n", "").Replace("- ", ", ");
            trangThaiTruyen = trangThaiTruyen ?? "";
            dsTheLoai = dsTheLoai ?? "";
            dsTheLoai = dsTheLoai.ToString().Replace("Thể loại:", "");
            tenTacGia = tenTacGia ?? "";
        }

        public void GetChuongTruyenTranh(DataRow dr)
        {
            string baseURL = dr["nguon_url"].ToString();
            idTruyen = dr["ID"].ToString();
            string tenTruyen = dr["title"].ToString();
            this.UpdateTextControl(this.lblProcessMessage, "[" + idTruyen + "]" + baseURL);
            this.UpdateTextControl(this.lblPercent, "#NA");
            if (!baseURL.EndsWith("/")) baseURL += "/";

            var listURLChuong = GetListChuongTruyenTranh(baseURL);
            Program.ExcecuteNoneQuery("UPDATE tblTruyen set soLuongChuong = " + listURLChuong.Count + ", done = 0  where ID = " + idTruyen);

            if (!listURLChuong.Any()) return;

            SetupProgessBar(listURLChuong.Count);

            var dictParam = new Dictionary<string, object>();
            dictParam["@refID"] = idTruyen;
            for (int chuong = 1; chuong <= listURLChuong.Count; chuong++)
            {
                var url = listURLChuong[chuong - 1];
                dictParam["@chuong"] = chuong;
                var urlRun = url;
                if (!urlRun.EndsWith("/")) urlRun += "/";
                var uriChuong = "chuong-" + chuong;
                //var direcChuong = direc + uriChuong;
                //if (!Directory.Exists(direcChuong)) Directory.CreateDirectory(direcChuong);
                try
                {
                    var lstURLHinhAnh = GetHinhAnhTruyenTranh(urlRun);
                    ChangeProgessBar(chuong, "[" + idTruyen + "] " + tenTruyen + "\nChương: " + chuong + "\n" + url);
                    //SaveFileToLocal_TruyenTranh(lstURLHinhAnh, direcChuong);
                    dictParam["@title"] = chuong;
                    dictParam["@uriChuong"] = uriChuong;
                    dictParam["@nguon_url"] = uriChuong;
                    dictParam["@arr_ggd_id"] = Newtonsoft.Json.JsonConvert.SerializeObject(lstURLHinhAnh);
                    Program.ExcecuteNoneQuery("sp_CreateChuongTruyen", dictParam);
                }
                catch (Exception ex)
                {
                    this.WriteLog(String.Format("ID Truyen:{0} Chuong:{1} URL:{2} \nException:{3}", idTruyen, chuong, urlRun, ex.Message));
                }
            }
        }
        private List<string> GetListChuongTruyenTranh(string baseURL)
        {
            List<string> lstURL = new List<string>();

            string url = baseURL;
            try
            {
                var doc = Program.LoadDocument(url);
                if (doc == null || doc.DocumentNode == null)
                {
                    return lstURL;
                }
                for (int chuong = 1; chuong < int.MaxValue; chuong++)
                {
                    var node2 = doc.DocumentNode.SelectNodes("/html/body/main/div[2]/div/div[1]/article/div[3]/nav/ul/li[" + chuong + "]/div[1]/a")?.FirstOrDefault();
                    if (node2 == null) break;
                    var href = node2.GetAttributeValue("href", "");
                    if (string.IsNullOrEmpty(href) || lstURL.Contains(href)) continue;
                    lstURL.Insert(0, href);
                }
            }
            catch (Exception ex)
            {

            }

            return lstURL.Distinct().ToList();
        }
        private List<string> GetHinhAnhTruyenTranh(string baseURL)
        {
            List<string> lstURL = new List<string>();
            string url = baseURL;
            try
            {
                var doc = Program.LoadDocument(url);
                if (doc == null || doc.DocumentNode == null)
                {
                    return lstURL;
                }
                for (int page = 1; page < int.MaxValue; page++)
                {
                    var node2 = doc.DocumentNode.SelectSingleNode("//*[@id=\"page_" + page + "\"]/img");
                    if (node2 == null) break;
                    var href = node2.GetAttributeValue("data-original", "");
                    if (string.IsNullOrEmpty(href) || lstURL.Contains(href)) continue;
                    lstURL.Add(href);
                }
            }
            catch (Exception ex)
            {

            }

            return lstURL.Distinct().ToList();
        }
        #endregion
    }
}
