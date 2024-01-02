using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CatCode_Selenium
{
    public class GGDriveFolder
    {
        public string kind { set;get;}
        public string incompleteSearch { set;get; }
        public string nextPageToken { set;get; }
        
        public List<GGDriveFile> files { set;get; }
    }
    public class GGDriveFile
    {
        public string kind { set; get; }
        public string mimeType { set; get; }
        public string id { set; get; }
        public string name { set; get; }
    }
    public partial class Form1 : Form
    {

        const int sleepTime = 200;
        const int loopTime = 150;
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += Form1_FormClosed;
            this.Shown += Form1_Shown;

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(chromeDrivers != null)
            {
                foreach (var chromeItem in chromeDrivers)
                {
                    try
                    {
                        chromeItem.Quit();
                        chromeItem.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            chromeDrivers = null;
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
           
        }
        private void RUN_BatTu_GetData( ChromeDriver webDriver, string email,string token)
        {
            try
            {
                int length = 3650;
                while (length > 0)
                {
                    length--;

                    var chartId = this.GetNext_ChartId_GetData(email);
                    if (chartId < 0)
                    {
                        if (webDriver != null)
                            webDriver.Quit();
                        return;
                    }

                    webDriver.Url = "C:/Working/CatCode_Selenium/CatCode_Selenium/Self_Plus.html?chartId=" + chartId
                        + "&token=" + token 
                        ;
                    webDriver.Navigate();
                    IWebElement input_jsonBatTu = null;
                    string jsonBatTu = string.Empty;
                    int count_wait = 0;
                    while ((input_jsonBatTu == null || string.IsNullOrEmpty(jsonBatTu)) && count_wait < loopTime)
                    {
                        input_jsonBatTu = webDriver.FindElement(By.Id("jsonBatTu"));
                        jsonBatTu = input_jsonBatTu.Text;
                        Thread.Sleep(sleepTime);
                        count_wait++;
                    }
                    if (string.IsNullOrEmpty(jsonBatTu) || count_wait > loopTime)
                    {
                        break;
                    }
                    UpdateJsonBatTu(chartId, jsonBatTu);
                }
                RUN_BatTu_GetData(webDriver, email, token );
            }
            catch (Exception ex)
            {
                LogText("[" + DateTime.Now.ToString() + "] Exception: " + ex.Message);
                RUN_BatTu_GetData(webDriver, email, token );
                        return;
            }
            finally
            {
            }
        }
        private void RUN_BatTu_Create_GetData(ChromeDriver webDriver, string email, string token, int chartId)
        {
            try
            {
                int length = 3650;
                while (length > 0)
                {
                    length--;

                    int idGioiTinh; DateTime dtRun;
                    this.GetNextDay(out idGioiTinh, out dtRun);
                    if (idGioiTinh < 0) return;

                    webDriver.Url = "C:/Working/CatCode_Selenium/CatCode_Selenium/Self_Plus.html?chartId=" + chartId
                        + "&token=" + token
                        + "&idGioiTinh=" + idGioiTinh
                        + "&year=" + dtRun.Year
                        + "&month=" + dtRun.Month
                        + "&day=" + dtRun.Day
                        ;
                    webDriver.Navigate();
                    IWebElement input_jsonBatTu = null;
                    string jsonBatTu = string.Empty;
                    int count_wait = 0;
                    while ((input_jsonBatTu == null || string.IsNullOrEmpty(jsonBatTu)) && count_wait < loopTime)
                    {
                        input_jsonBatTu = webDriver.FindElement(By.Id("jsonBatTu"));
                        jsonBatTu = input_jsonBatTu.Text;
                        Thread.Sleep(sleepTime);
                        count_wait++;
                    }
                    if (string.IsNullOrEmpty(jsonBatTu) || count_wait > loopTime)
                    {
                        break;
                    }
                    if(jsonBatTu.Length < 100)
                    {
                        if (webDriver != null)
                            webDriver.Quit();
                        return;
                    }
                    InsertFullDB("",dtRun, -1, idGioiTinh,email, jsonBatTu);
                }
                RUN_BatTu_Create_GetData(webDriver, email, token, chartId);
            }
            catch (Exception ex)
            {
                LogText("[" + DateTime.Now.ToString() + "] Exception: " + ex.Message);
                RUN_BatTu_Create_GetData(webDriver, email, token, chartId);
                return;
            }
            finally
            {
            }
        }
        private void RUN_BatTu_CreateChartId(ChromeDriver webDriver, string email )
        {
            try
            {
                int count_wait = 0;
                int length = 3650;
                while (length > 0)
                {
                    length--;
                    int idGioiTinh;DateTime dtRun;
                    this.GetNextDay(out idGioiTinh, out dtRun);
                    if(idGioiTinh <0 ) return;
                    LogText("[" + DateTime.Now.ToString() + "] Length=" + length + "; DateRun: " + dtRun.ToString("dd-MM-yyyy"));
                    webDriver.Url = "https://bz.selfplus.vn/Home/LoginV2";

                    IWebElement btn_Login = null;
                    count_wait = 0;
                    while (btn_Login == null)
                    {
                        if (count_wait++ >= loopTime) break;
                        try
                        {
                            btn_Login = webDriver.FindElement(By.XPath("/html/body/div[2]/div/span"));
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(sleepTime);
                            btn_Login = null;
                        }
                        if(webDriver.Url.Contains("Home/BaziDashboard"))
                        {
                            break;
                        }
                    }
                    if (!webDriver.Url.Contains("Home/BaziDashboard"))
                    {
                        if (count_wait++ >= loopTime)
                        {
                            RUN_BatTu_CreateChartId(webDriver, email );
                            return;
                        }

                        var input_email = webDriver.FindElement(By.Name("email"));
                        input_email.SendKeys(email);
                        var input_pw = webDriver.FindElement(By.Name("password"));
                        input_pw.SendKeys(email);

                        btn_Login.Click();
                    }

                    count_wait = 0;
                    while (!webDriver.Url.Contains("Home/BaziDashboard"))
                    {
                        if (count_wait++ >= loopTime) break;
                        Thread.Sleep(sleepTime);
                    }
                    if (count_wait++ >= loopTime)
                    {
                        RUN_BatTu_CreateChartId(webDriver, email );
                        return;
                    }
                    webDriver.Url = "https://bz.selfplus.vn/Home/PersonalBazi";

                    IWebElement btn_LapBang = null;
                    count_wait = 0;
                    while (btn_LapBang == null)
                    {
                        if (count_wait++ >= loopTime) break;
                        try
                        {
                            btn_LapBang = webDriver.FindElement(By.XPath("/html/body/div[2]/form/div[6]/div/button"));
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(sleepTime);
                            btn_LapBang = null;
                        }
                    }
                    if (count_wait++ >= loopTime)
                    {
                        RUN_BatTu_CreateChartId(webDriver, email );
                        return;
                    }
                    string hoVaTen = (idGioiTinh == 1  ? "Nam" : "Nữ")+" " + dtRun.ToString("dd-MM-yyyy");
                    var input_name = webDriver.FindElement(By.Id("name"));
                    input_name.SendKeys(hoVaTen);

                    var cbb_gender = new SelectElement(webDriver.FindElement(By.Id("gender")));
                    cbb_gender.SelectByValue(idGioiTinh == 2 ? "0":  idGioiTinh.ToString());

                    var cbb_year = new SelectElement(webDriver.FindElement(By.Id("year")));
                    cbb_year.SelectByValue(dtRun.Year.ToString());


                    var cbb_month = new SelectElement(webDriver.FindElement(By.Id("month")));
                    cbb_month.SelectByValue(dtRun.Month.ToString());


                    var cbb_day = new SelectElement(webDriver.FindElement(By.Id("day")));
                    cbb_day.SelectByValue(dtRun.Day.ToString());

                    var ckb_unknownHour = webDriver.FindElement(By.Id("unknownHour"));
                    ckb_unknownHour.Click();

                    btn_LapBang.Click();

                    string id = "";
                    count_wait = 0;
                    while (string.IsNullOrEmpty(id))
                    {
                        if (count_wait++ >= loopTime) break;
                        try
                        {
                            if (webDriver.Url.Contains("id=")) id = webDriver.Url.Split('=')[1];
                            else Thread.Sleep(sleepTime);
                        }
                        catch
                        {
                        }
                    }
                    if (count_wait++ >= loopTime)
                    {
                        RUN_BatTu_CreateChartId(webDriver, email);
                        return;
                    }
                    InsertDB(hoVaTen, dtRun, int.Parse(id), idGioiTinh,email);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot locate option with value:"))
                {
                    webDriver.Quit();
                    return;
                }
                LogText("[" + DateTime.Now.ToString() + "] Exception: " + ex.Message);
                RUN_BatTu_CreateChartId(webDriver, email );
                return;
            }
            finally
            {
            }
        }

        private void LogText(string mgs)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate {
                    // Running on the UI thread
                    this.richTextBox1.Text = mgs;
                });
            }
            catch 
            {
            }
        }

        public string connetionString { get { return Program.configuration.SQL_CONNECTION;} }

        private void InsertDB(string name, DateTime dt, int idBatTu, int idGioiTinh, string email)
        {
            string queryString = string.Format(
                   "INSERT INTO tblDataBatTu(title,statusData,visible,uidCreate, createDate, ngay,idGioiTinh,idBatTu, emailCreate)" +
                   "select N'{0}',3,1,1,N'{1}',N'{1}',N'{2}',N'{3}',N'{4}' where not exists(select 1 from tblDataBatTu where ngay = N'{1}' and idGioiTinh = {2})"
                   , name, dt.ToString("yyyyMMdd"), idGioiTinh, idBatTu, email);

            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private void InsertFullDB(string name, DateTime dt, int idBatTu, int idGioiTinh, string email,string batTuJson)
        {
            string queryString = string.Format(
                   "INSERT INTO tblDataBatTu(title,statusData,visible,uidCreate, createDate, ngay,idGioiTinh,idBatTu, emailCreate,batTuJson )" +
                   "select N'{0}',3,1,1,N'{1}',N'{1}',N'{2}',N'{3}',N'{4}',@batTuJson where not exists(select 1 from tblDataBatTu where ngay = N'{1}' and idGioiTinh = {2})"
                   , name, dt.ToString("yyyyMMdd"), idGioiTinh, idBatTu, email);

            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.Parameters.AddWithValue("@batTuJson", batTuJson);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private void UpdateJsonBatTu(int idBatTu, string jsonBatTu)
        {
            string queryString =   "update tblDataBatTu set batTuJson = @jsonBatTu where idBatTu = @idBatTu";

            using (SqlConnection connection = new SqlConnection(connetionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.Parameters.AddWithValue("@jsonBatTu", jsonBatTu);
                command.Parameters.AddWithValue("@idBatTu", idBatTu);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private void GetNextDay(out int idGioiTinh,out DateTime dtRun)
        {
            try
            {
                string queryString = "delete TOP(1) from tblNgayGetDatabatTu OUTPUT deleted.ngay,deleted.idGioiTinh" ;
                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    connection.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(queryString, connection))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        idGioiTinh = Convert.ToInt32(dt.Rows[0]["idGioiTinh"]);
                        dtRun = Convert.ToDateTime(dt.Rows[0]["ngay"]);
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                dtRun = new DateTime(1980, 1, 1);
                idGioiTinh = -1;
            }
           
        } 
        private int GetNext_ChartId_GetData(string email)
        {
            var ChartId = 1;
            try
            {
                string queryString = "select MIN(idBatTu) as ngay from tblDataBatTu where batTuJson is null and emailCreate = N'" + email + "'";

                using (SqlConnection connection = new SqlConnection(connetionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    ChartId = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception)
            {
               return -1;
            }
            finally
            {
            }
            return ChartId;
        }
            //XTVCE1
        List<Dictionary<string, string>> lst_account = new List<Dictionary<string, string>>() {

            //                                  new Dictionary<string, string>(){
            //             {"email","a1980@gmail.com"},
            //             {"pw","a1980@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1980"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODY4MCwidXNlcl9pZCI6InNJcTFzUTFBZnBUdWg3TzZmS1d4cG9FTFUzaDEiLCJzdWIiOiJzSXExc1ExQWZwVHVoN082ZktXeHBvRUxVM2gxIiwiaWF0IjoxNjc2NTk4NjgwLCJleHAiOjE2NzY2MDIyODAsImVtYWlsIjoiYTE5ODBAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImExOTgwQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.J0kZTRAuhMcn54V3wpu2il6IPWIe-s49utK5njRtoQ41bcfsM-o1p9LVu0vZ1-3at4nZth1g6keG9SGKm6xKWAMe5y_NKjVeKPQI2jSZmTbePdypGL-8SiwOulivFIvCCzZTDaTWlIzlPmsQyaXzb2b_zKb6f5yLt_hv6UBR5QpFvQz4nlmZGNPW4QIlGpmG8xHuizVlxJA82S0AzSxehhw74Pw-eijHqOblDaFb962PfPIU-CLe0yA8rd-qUOjTx_w4-ghv17pLepMplTINOxgwP9LV4SlYhlpSA8BrLAEr-E1MBSpuMidICgR7GkR08x3Bc4g7O4EX7I2cVjXC8Q"},

            //         },
            //       new Dictionary<string, string>(){
            //             {"email","a1983@gmail.com"},
            //             {"pw","a1983@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1983"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODcxNiwidXNlcl9pZCI6Ik9Ud2pQTXhONjVaZzBzUEZPbTQ5WlZNcW5zcDIiLCJzdWIiOiJPVHdqUE14TjY1Wmcwc1BGT200OVpWTXFuc3AyIiwiaWF0IjoxNjc2NTk4NzE2LCJleHAiOjE2NzY2MDIzMTYsImVtYWlsIjoiYTE5ODNAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImExOTgzQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.2xEuJ-GdJtqmNLdCwyFMl_69RnCy1X1ymjc45VXd8ScIWv5vXZBNl8gI3BDqkS1ohIduGGoMVGAY9j9hdvIiaplAD6udfrFPJG7fgzc1ZmuJHTjRv-yFGptOfC-bwHrtxFPQQE8K1LJcKhd2Es4KKt1egwkbi1_mFAw5HgEGJO2kTCj-SXOdVUJZdvKchrTRkEwU31Y0aVJCjML2n6Ndb6OlATvgfLwRM3TwNWUtKKvDBswhjBuzHXuisfHXnLF4BT54rmYlMkaqAXmOvYgIQW1i63Qt3Mw-bVonXCZrz4nUzJXsEaJL8tHG4FvuKLPcwzNpPV5qq-j5Ko-wA1Ex4Q"},

            //         },new Dictionary<string, string>(){
            //             {"email","a1983-2@gmail.com"},
            //             {"pw","a1983-2@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1983"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODc0NCwidXNlcl9pZCI6IkIzNDlmaFhIbmxUZENITjBHNXpQVzVPYVhxMDMiLCJzdWIiOiJCMzQ5ZmhYSG5sVGRDSE4wRzV6UFc1T2FYcTAzIiwiaWF0IjoxNjc2NTk4NzQ0LCJleHAiOjE2NzY2MDIzNDQsImVtYWlsIjoiYTE5ODMtMkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5ODMtMkBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.CrWB0D0MMrdtQyRrwKIcBuOIdRuSfORwf6dP_J1MEN3IkEK2UEWyry7ubHtmCiNCSGrEwhxKjkajUbnUuBX-tttPyO0H96SonziFVb3pIlKmPR0W9On1FwSMLqxdh3MQO3QPiq8TYmThWQVnfKn8H4gUxtz49hE9pcRxfGpQMkoy6VM0qlWgCA4e4bBSDsJChX74WsOm2D_PYstOW319HYmgfY3F4AJe4_XeIzydRynAxPva4r6Uo_QvwFufIL3oeIw62TwEIWjXO46KNPzL7IQUF8zzbPpDaXRo-SG74Dv3NQ4cgLPGUJqRV0EifO4bTJjWM5HvXSSKJOKdW9IBVQ"},
            //         },

            //       new Dictionary<string, string>(){
            //             {"email","a1986-1@gmail.com"},
            //             {"pw","a1986-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1986"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODc3MCwidXNlcl9pZCI6IkpZaFM5b1dub2lWbnBWd1h1dGtmbGRpeUZjbTIiLCJzdWIiOiJKWWhTOW9Xbm9pVm5wVndYdXRrZmxkaXlGY20yIiwiaWF0IjoxNjc2NTk4NzcwLCJleHAiOjE2NzY2MDIzNzAsImVtYWlsIjoiYTE5ODYtMUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5ODYtMUBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.jX4mcTnqHuVZFY95oQrIRZjTN-JDKqc1p30a0atyJckcH7jY7oSXw6G_r4DfCRFBY2neHNRqU0q-MBqMf5UTAem14-RwOjZFo6T12K8xKuGvdHY542UHnIoeugkI1B0cwq0o3ropCSzfSUpAX7G2xZ_lrmNfNlRADB1lQzwebK2uXgAZM0DLViW6ewDQBlXFkKHiY8jF82uwb1tt54N-9ZOIg6P5feIWV2W4r667aL2d2flbb3Rk0EwM4aXuDoLFVau2pkjUx4ccxT2GMWZ8r0p4DBDh1ZuHPn9UTwCFwY2rDUZpVPScXxWtwoEh0AOt34kSu__1r7KPmoCAApFKGA"},

            //      },

            //         new Dictionary<string, string>(){
            //             {"email","a1986-2@gmail.com"},
            //             {"pw","a1986-2@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1986"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODgzMywidXNlcl9pZCI6IkxDdDJSNkdSSnJRWGRuY1JpZHQzbkc0WHBGNzIiLCJzdWIiOiJMQ3QyUjZHUkpyUVhkbmNSaWR0M25HNFhwRjcyIiwiaWF0IjoxNjc2NTk4ODMzLCJleHAiOjE2NzY2MDI0MzMsImVtYWlsIjoiYTE5ODYtMkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5ODYtMkBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.6xvlidQV0eeWKvyvvumtEgLp0A0LSArtVdY25et1PQLu289aOqATl5vwO7V85YFi89J4cJcjI-G3uQVKVVUbdR8nPFEhhzZCPAkWgSLjZ71hSCi70epBymIuj4xmK5uGhMkbshvtxY99VLmbGqvl4Ku1z7kVKHnehgQW4nOnQTa3U-FzU3pg3U76crNOzkYmvrCb1AIHLiW5I_Q1e0brHEvN2QZ57fC2t6c4nrN5kD-92_GQasW6O9UKs0m5eTNgl2nxsxFgubeRrCz2HC9Ji8ctjC4Q5Venihpu-Ypx3lGUBDiS5v-iJ9RPpM133UNS5fQCVDk9CRQmiso3ZvLBzw"},

            //         },

            //       new Dictionary<string, string>(){
            //             {"email","a1989-1@gmail.com"},
            //             {"pw","a1989-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1989"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODc5NSwidXNlcl9pZCI6IklnWklYOHFMa3hhNGdXdFVkWUJGdkV3ZExqcDEiLCJzdWIiOiJJZ1pJWDhxTGt4YTRnV3RVZFlCRnZFd2RManAxIiwiaWF0IjoxNjc2NTk4Nzk1LCJleHAiOjE2NzY2MDIzOTUsImVtYWlsIjoiYTE5ODktMUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5ODktMUBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.nBvfYPZm1pFVJMu4YFc2txMzoY3DiNX7Vz8PezNVoRyN9pFd-8Z4Fe1XJoM2Sih2d3tXHqKWqLi9LuTDaRU2Zlg2OS4L_qyxNeo4qDXc3n4RhLlUc0sKiW-8LUhcpzfFzzj2MIqrDty-SNfcvzNent2UaLur6l5JagAfPjX6y3sg5XaoevUObvLXfzsEr7uwW6rGVtF7rSksL6luP1u66ETqsXAAWLhpNMoyE4DNYgzFfjPGVfqH1VnH98PZBIz9tGszMjq5SPWs8FNOpn9PDiVz2G3fiPWxbM5eJAmXvu7x7BLfKA7OqSBBl6N-dRYxsQJntzibZicQcJcnQ8-FuA"},
            //         }
            //         ,new Dictionary<string, string>(){
            //             {"email","a1989-2@gmail.com"},
            //             {"pw","a1989-2@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1989"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODg1OCwidXNlcl9pZCI6IjdHUXdvMVVtU1VWRndxNVluNXNGeXZreWtRRjIiLCJzdWIiOiI3R1F3bzFVbVNVVkZ3cTVZbjVzRnl2a3lrUUYyIiwiaWF0IjoxNjc2NTk4ODU4LCJleHAiOjE2NzY2MDI0NTgsImVtYWlsIjoiYTE5ODktMkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5ODktMkBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.3tYUrvOROS13HYrfJzny3hMBpEqVL6WFV6X4Bx1GEqIOvIkXiAHMAK4C756BppgIs1T1N2bMFSCGZ1eTL1PLZ6nQwkzYGrMj7HOvuShjq6xgZiPardvPALwzm2qO8ntFhmp8U0yNtlUpnaX1jM2gwHAwq3gBgAGCJfgGEUV1TTMNBFJmrVxOUlLrtU7bIO3iuUUZbZ1sHh3SKcxJ1AGfc49gdHgWPW6xF72mhMEjJN-YVIK6KS3UImmji4urNpFkigUJvb-YynonpLhqUWuMcUDUrTdgxOSfsk22kfsAOpMfl0b8ns2bw0-Wjjj7pr36Uf5Ym3VhSavV0TrNpRFW3w"},

            //         }

            //            ,new Dictionary<string, string>(){
            //             {"email","a1995-1@gmail.com"},
            //             {"pw","a1995-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1995"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODk2OCwidXNlcl9pZCI6Ikxnd0RnSVg3NUNRZUM2aGprbGlZb2FoSzZKNzMiLCJzdWIiOiJMZ3dEZ0lYNzVDUWVDNmhqa2xpWW9haEs2SjczIiwiaWF0IjoxNjc2NTk4OTY4LCJleHAiOjE2NzY2MDI1NjgsImVtYWlsIjoiYTE5OTUtMUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5OTUtMUBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.eaG8KsQMdDLlFhLUcUl8-MkebNFrdYKcMlTU8o2J_fEWAVJg-1Vzou0ylQfv3h8vNO72nYqlr8pxRPbyn_8PTuLjjrnpnpy16Yzkd1GGeunsffLOfJK8PzHMh97f-VGbLzSJhfJm7b-7pVCA_dZj5c1AW6BCmmagtaVeREwSsSuxtUDjISS6DVqEPGgzVQJlnYLxak-nlgKcDEPdDfBr1bDy31Fo7drT6mxezCcPPosMO-jjbkQMXwUWIQdTA-D8Bq-U-k7qplskqmL6WK6izY5VkAdp191kAF7xBGnsjohGwXryyAg4tz63t6jydvd4jdyM_JdETkugNdkYf3PayQ"},

            //         }
            //         ,new Dictionary<string, string>(){
            //             {"email","a1995-2@gmail.com"},
            //             {"pw","a1995-2@gmail.com"},
            //             {"idGioiTinh","2"},
            //             {"namBatDau","1995"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTAyMywidXNlcl9pZCI6ImF0ZXlDREZEWGRZQTRmOUxwTm4yN3RwRDVPdzIiLCJzdWIiOiJhdGV5Q0RGRFhkWUE0ZjlMcE5uMjd0cEQ1T3cyIiwiaWF0IjoxNjc2NTk5MDIzLCJleHAiOjE2NzY2MDI2MjMsImVtYWlsIjoiYTE5OTUtMkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5OTUtMkBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.L9jU2tZlY1xJZOQcgsj3UUFyQJeLmU39bhYsWT_oKZJDj_P0pldARTk_qyMh8088FucQoxyAPonjMH6ZVRDQwMOVe3NQFMry8GPIor1kwePUOBFyNd-HnFn28f7GymSqHBmHk3SYriGyqaxjDW9hBIgi8wgsLSUHWMuYTCkY9EoccOAPuBDeXDOcyrNII3Vcj3maidhQhugrA4nz66n52E6KptHkNw8fBnKVpMVA3A0gxsdwPJX3ZeCuHSUco0RVGU9hQ_PzPIuWabrk-supF7smOFsJr53I8ISKWwPz_tzqQ12k7heieYgijHsf28SWLvzzHpIzVxoKOXIkaETs9g"},

            //         },

            //               new Dictionary<string, string>(){
            //             {"email","a1998-1@gmail.com"},
            //             {"pw","a1998-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1998"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5ODk5NSwidXNlcl9pZCI6InRYYWZoM0RaRlNmZXhiSDlnbWMweVRKVWNmUjIiLCJzdWIiOiJ0WGFmaDNEWkZTZmV4Ykg5Z21jMHlUSlVjZlIyIiwiaWF0IjoxNjc2NTk4OTk1LCJleHAiOjE2NzY2MDI1OTUsImVtYWlsIjoiYTE5OTgtMUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5OTgtMUBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.56IiSEskN_yWZO7kSq5w7fIpfEwyV0wxD_zsO_ace-Rb89tDzJU8es6ZqlU-QA2wmwBAmm3z4o3aO3vV0l_Fa4x-0aCGOndzbPBUUVGaXhH1_DxiUvNqZhkbsNzcq8zo5V6WMPsOp0tf1UXNfEXx4tOqO4yyHBddkDUE65RpW3Hag-HzGly1rgOcHD3clR2-qvzWPYAQHRd69uPCcjfjmOBg6ulwkM5huuTRBF-6vw9AlWs9CFP1i9tiQZaRLHs91P6doXlXFaIVqRfcsQTqVfOEBfdu-imz55WdOpQdWFfT08B3gfoAaMtxecMXWxSU7p18kNj73q26wqoSOTUOJQ"},

            //         }
            //         ,new Dictionary<string, string>(){
            //             {"email","a1998-2@gmail.com"},
            //             {"pw","a1998-2@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1998"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTA1MiwidXNlcl9pZCI6InJ4eUdNc3BaOFhQUUFQVU5BcWpLSHByWTd2ODMiLCJzdWIiOiJyeHlHTXNwWjhYUFFBUFVOQXFqS0hwclk3djgzIiwiaWF0IjoxNjc2NTk5MDUyLCJleHAiOjE2NzY2MDI2NTIsImVtYWlsIjoiYTE5OTgtMkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiYTE5OTgtMkBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.s08G3jXAjuU3TxiqrfiBjvAxIMvQlU7J-17v0AurWRK6-LmBSx34MUFVKCRkr6pkDGQsYkNlWOzX0YhztOcMqyEOJykmDUt502LoNI2fM7Xmco0R_xf8BbxDh2fHYjVQkiM93L08VugokjlhFCr4p23UdlMblDNHCGMuij4DeKOgocC8gbMEAFjUOmBSVAj68dUZbebe75GCBpFumC06gz78tJZY4G0_ps1pyuDE5so8n6DdFjDutQIOTBJaKQgCLAdAdT8MJ8RCwo8K6RteaDeoBirgOTxjg1mMjuuHYPkfmPZq-X93ZjzACPZ8jvsYYZNOTrsK_n7kMg-H8PFVSg"},
            //         },


            // new Dictionary<string, string>(){
            //             {"email","z1980-1@gmail.com"},
            //             {"pw","z1980-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1980"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTA3NywidXNlcl9pZCI6InpLdmtLMGE4a2liZXZ2Y1JMcGtvekhtQVZ1NzIiLCJzdWIiOiJ6S3ZrSzBhOGtpYmV2dmNSTHBrb3pIbUFWdTcyIiwiaWF0IjoxNjc2NTk5MDc3LCJleHAiOjE2NzY2MDI2NzcsImVtYWlsIjoiejE5ODAtMUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiejE5ODAtMUBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.KRYiTyyksgh4Aor_1KZZ9zjckEaN99CjvTb8WFr5782Lsiwll5ZtZrL7dYraFTpLIVjODZFvpmIBZ_XqEzHw0OSo11Zb38GCFB19zGjH73IrGbG-rfArmpbpJZUVv6UKv6ff4F1t-jF5Ff80yg_IEoIvYA9QbuBxwR14914KtiODWiqKWJR21VFFsbESE9xfvmXjkjWStjXsr4NDzo33hzRw1_-_ikt9IPVqxjaPYS75Vd59LrTvmlG86f9waHBqoHm4oSr0nKkgE_XWSpVjSqu4A2RDe7gr4k0zN9Lu8hD0PwvJSm1WcMwC9W1hGim7t1e40-4FiUfuaI-lZim4YQ"},

            //         },
            //new Dictionary<string, string>(){
            //             {"email","z1980-2@gmail.com"},
            //             {"pw","z1980-2@gmail.com"},
            //             {"idGioiTinh","2"},
            //             {"namBatDau","1980"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTEzMCwidXNlcl9pZCI6IjdYZzZtSzBQeGxNcU90aHVxbUd5MTVxc0ZwcDEiLCJzdWIiOiI3WGc2bUswUHhsTXFPdGh1cW1HeTE1cXNGcHAxIiwiaWF0IjoxNjc2NTk5MTMwLCJleHAiOjE2NzY2MDI3MzAsImVtYWlsIjoiejE5ODAtMkBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiejE5ODAtMkBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.v51v-YmZ0WH8skK7GLP6FDLVclVoAN7Md_2Yl82A1zuKgqUk6yiPsvebC4xSDY0wiJL2MM_amkAihGVI-XyEpcWOqfPDb75RAQIfh_daThderSgkPY4y2AjHHZrpVrBQXOqCGZ3KjsHJzGaQWYwTKcltHgH241Y79wYo52M4ckH_Ix3Dcaot6R1yIax_HMPbh5sqYKyZU1y-ZSY7JDsQRL5TGpWNGIrmA_TKWl-TnADR4ChscrgnyWk2Ntcm233mfzOfjGfuAcbV8fH0vVs4OhUIlKuiJcb5QhGusuvUfcPx1rw3yo5-P5oHfsSmSZWH6FF2NePHJXw7k7yvWR4g3w"},
            //         },


            //   new Dictionary<string, string>(){
            //             {"email","n-1980-1@gmail.com"},
            //             {"pw","n-1980-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","1990"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTE2MywidXNlcl9pZCI6InFRQlY5R0txMlZXVktXSUU3amI0Q3JvaGk3ZDIiLCJzdWIiOiJxUUJWOUdLcTJWV1ZLV0lFN2piNENyb2hpN2QyIiwiaWF0IjoxNjc2NTk5MTYzLCJleHAiOjE2NzY2MDI3NjMsImVtYWlsIjoibi0xOTgwLTFAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbIm4tMTk4MC0xQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.nyDB7BxZ7uBlQFat923x5fSmfWUwmPWJ1ZiLdeKyKsxh6PfI_ZX_uot5H5TzXpCk6tDIzFtcbV134Mpx0m9then6yF_HalODM52rl3XIRW2wdCV38hd8ei2za-RSGuR1ezIJWHzstcazIiU7ZW7e1sVhJ80oI1Ui9Dc2QjAuIkGRBAAf0JWjQmNHgvCVzDN2D45eDm5RPEaOm3kDrAH7QbiqPIxepmim7JwI6z6_Jq-t4kfkcgjwF4tYxwvfMeHulUt39sfGlrkrch3RADapImx8y9ULFeURAhGI54IfNUHcDq4HUiqeCI4ZyUCqd_q_o8bix4XIn79e2uGgAuoLeA"},

            //         },
            //new Dictionary<string, string>(){
            //             {"email","n-1990-2@gmail.com"},
            //             {"pw","n-1990-2@gmail.com"},
            //             {"idGioiTinh","2"},
            //             {"namBatDau","1990"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRja14@gmail.comZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTE5MSwidXNlcl9pZCI6InhOdzBGcURRZHZldUFCSHoxYWRrZ0FjMTF4cjIiLCJzdWIiOiJ4TncwRnFEUWR2ZXVBQkh6MWFka2dBYzExeHIyIiwiaWF0IjoxNjc2NTk5MTkxLCJleHAiOjE2NzY2MDI3OTEsImVtYWlsIjoibi0xOTkwLTJAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbIm4tMTk5MC0yQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.D3dOlrb0xk2GwVMDcaHf0kiIRjl_brqcFgJhWizAGfJ5gW0deF-jhctbvzntKQlvOm-6MvGoXMN3EwZRUf5kP2A4MjvUjLofdh3VjzOyl-f4BgN1Y31Kz2ooe9eBquNYKY5i2RrcXHlCJOKKpr7bAmyXIQ3kgPiSawH4qQhkDSbDJKCQODNViGATiC9cPqxvbYo8TtjQgN-jsor3jCdBmqyAI-QQmUwd7HctI67tn-MEWhgfPtxSKo26_cAwK2OgbqQsZkdruOcw5nwDITxVWDsduVG19KaeNKFxLZ6qQHJn_oRLBp3qSHkTUcvQWHBRS8t5ZgN3vRbc1tTf0_Uh6w"},

            //         },


            //  new Dictionary<string, string>(){
            //             {"email","n-2000-1@gmail.com"},
            //             {"pw","n-2000-1@gmail.com"},
            //             {"idGioiTinh","1"},
            //             {"namBatDau","2000"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTIxOSwidXNlcl9pZCI6InZFSk5tU0k1b1BUc2VlVnp2Y1NqMTE4SWFEcDIiLCJzdWIiOiJ2RUpObVNJNW9QVHNlZVZ6dmNTajExOElhRHAyIiwiaWF0IjoxNjc2NTk5MjE5LCJleHAiOjE2NzY2MDI4MTksImVtYWlsIjoibi0yMDAwLTFAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbIm4tMjAwMC0xQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.6lR0I-lnPIGUZJEn2QQWnnQoQ8DlVdQUqUnX_2AmqFDAjlBoi5ecwU2dfLoD87ukUKN3xBxFl2-kYq_sLaMYKKlO_u-ufKI-6vPCHki4LX0gQjBSuKP7KdZowdi5p0B1t5tVYfTZxbFkytrU02qLMrzkFRjWoKyBpJ_hB2nIUIa7_1wOl2-O6C-wl_gj_BhZ38TFZ18jeg8lMeNmY41jo5vppTavEvqkdu1PaN21RwRL00NqDjjsrK71kdnozZtKHusNna1A7iOYkSbCRsK2xdBa3zc5b_nkSmv-LOKDfchQPWwzxm1sOPLwPL7DcKswiBYmqtNu0JIIi1wojCZfYw"},

            //         },
            //new Dictionary<string, string>(){
            //             {"email","n-2000-2@gmail.com"},
            //             {"pw","n-2000-2@gmail.com"},
            //             {"idGioiTinh","2"},
            //             {"namBatDau","2000"},
            //             {"token","eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjU5OTI0NSwidXNlcl9pZCI6InBlZWsweXNtNTliUEpqUmNmS2JLYlBCUEF4RzIiLCJzdWIiOiJwZWVrMHlzbTU5YlBKalJjZktiS2JQQlBBeEcyIiwiaWF0IjoxNjc2NTk5MjQ1LCJleHAiOjE2NzY2MDI4NDUsImVtYWlsIjoibi0yMDAwLTJAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbIm4tMjAwMC0yQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.10u7_TIZKipXGNc1oausl_BKbsApCf_xRuZiXMJ6EvJbR-fT2X2qYiU76aAe5QhILa1qHOhgSOyJaxi4RIpifzkubU__wjIJslbO5ZQyxUx6423LujSJfCYVP87VQxiinSkuofcrfQe1Eeg50i9D_l2WJaA9-hk_gPR_oaAU4lR_dV0TwF7xWZ7-BF2s-83phVwTLm0r5H-zBMjA3ElDcR0jOJ76S5RGKRdYu4kxk7VfSHL4-u3LDkL42_IiTnaGRj-KkUyFCu6lgiyToRAXFGqmrkvzq48PCWPqcbs0dwh8fstVjsPyAcXZgQiJMOi9SCcg_PfxA2JyteDAbuD0_w"},

            //         },



            new Dictionary<string, string>(){ { "email", "a1@gmail.com" }, { "chartId", "39976" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcyMDEyNywidXNlcl9pZCI6IkxSOGFpaGJYNndhTGFFOVhUbjNYWVBRTHpqSDIiLCJzdWIiOiJMUjhhaWhiWDZ3YUxhRTlYVG4zWFlQUUx6akgyIiwiaWF0IjoxNjc2NzIwMTI3LCJleHAiOjE2NzY3MjM3MjcsImVtYWlsIjoiYTFAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImExQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.LN9CjTvLWIjAEizMc4h60bqOsd3lKXt15PgGn_cKW6vfFPwnAOwNk0qzvrYVrUOWNic6LW1Ozyt0VY2L2GWZ51toidhSli_IGseIweIOpN2Uw27qhzXtbevbUrJyZB3eexex-fCWRl3nLhVMv9hKbMS82Y6AoodtoSejFP6bpcJeK4oEuYLcNYpZ848Pw7SpEyOW_3FWqN32F8WA0rcWCwvehw6zmBM5ZtLX5CWc5fNBTiZLsNR9bYjGnnKVduKKQXWsm9hZF7mOnpafd30Ut1Pf2KVI5TfxVY06vAXgP1H5z1WBXBycFfIl_cHet6eM9BUEsEm_-7udep3egyuSSA" },  },
            //new Dictionary<string, string>(){ { "email", "a2@gmail.com" }, { "chartId", "39920" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcyMTYxMiwidXNlcl9pZCI6IlhiTEYyTmVQU1pUVkdqOFlpdERqdGNRUGxCYjIiLCJzdWIiOiJYYkxGMk5lUFNaVFZHajhZaXREanRjUVBsQmIyIiwiaWF0IjoxNjc2NzIxNjEyLCJleHAiOjE2NzY3MjUyMTIsImVtYWlsIjoiYTJAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImEyQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.fujEU8CeT_coRnLGQnYIiSwfEkNsAOSoi6Xhs0pz2g99YGoO_HooBfFZep5PmXx05pJyPv-GUVUrgtOabKPPzbnDB4g-9ZqHFeslc0VIahqASqDQ6ZGu5TGYnJ092SRNI-qyiwWAfYXnOJ4olR317nPt0kxFHaPT71dRVmpVhY7Bvq-Nl9iOUc4JMYm8TRdFmgyHQS4W78FUNNslfUgOxwcCa3PqwV2Vd_p_NAyUmzzSt9D9gJ_IFXd_atdum_te7j232N5DHEuCBJ8Q5TpzZthPTCJ8k_vGXT2AfNMB_uGq1zc0pH_PG5MDmvFSEmEw6u8tYTs6nHX_jwZj5o7wyA" },  },
            //new Dictionary<string, string>(){ { "email", "a3@gmail.com" }, { "chartId", "39980" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcyMTY1MSwidXNlcl9pZCI6InNrTzRxeFVmcVZiZVU1bGMxZTFqN2pMbzRaMDMiLCJzdWIiOiJza080cXhVZnFWYmVVNWxjMWUxajdqTG80WjAzIiwiaWF0IjoxNjc2NzIxNjUxLCJleHAiOjE2NzY3MjUyNTEsImVtYWlsIjoiYTNAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImEzQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.HnNNj5CVelAo9zvBmA0S6_OwDhkB9IFJPiqo7o9lKfiiXtV-ryH-lA3vHa7i2QCwN-k3uguaxiCWxlo-L_YIS7TmqS-b-5dUTGAxLoGJso4V7wLX7Eoyg6AUlAvJdzOp8GymBUCIO674wrRDYmptIaV4o4a0MXyIW0Qyi8mmYhG78sVbxLgkZ1dtVKX5G56az10XnjoFjGgYhcdXQcapzbGRGnW-wW4WRl29zCL0pgzwO0h5QvWsd2VGSScTLvqsbXV0xF8hwx3NfCJE-R7yWUq6G1ld11hFyTiERrLmp3jOJA-mNmnJOEMp4-kdY8tiEgmLxZAlOhaE6g992nvmBg" },  },
            //new Dictionary<string, string>(){ { "email", "a4@gmail.com" }, { "chartId", "39913" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcyMTY5NiwidXNlcl9pZCI6ImphZHFsU2xpZnFoU2FSWVpPQWZnb3ZyQ1dMYzIiLCJzdWIiOiJqYWRxbFNsaWZxaFNhUllaT0FmZ292ckNXTGMyIiwiaWF0IjoxNjc2NzIxNjk2LCJleHAiOjE2NzY3MjUyOTYsImVtYWlsIjoiYTRAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImE0QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.Ipaua-wu1JTFb0IvEckFYB3m1sopWg-YvDSmD2-cN28t4Fn-EwWNTaJTscxWLuDCltqKFFTlHDP4ag4PlYa9kMQfD6FKNhDmhdqd36uq4z9NCm4rj1tsmPVe3EzlljSi0r5LY1a2eG6jdT-jujDe8gCkoetKuv_QioodxvSMyujDMXAG9idl9lMRV5HyY01RYuxucl01UAdEB2AroP0_xXfee6bszy6ifjJyAcxLl60FF3RQvq6kBODOQcA6B0EmlLQywxsFv4Tln5_V7dJLdNveP9w20cCnsxvWhdVPCsxKpy3V_9AA1l7uve5kbr1yt45V1KteVqsg9WNdQwSTeQ" },  },
            //new Dictionary<string, string>(){ { "email", "a5@gmail.com" }, { "chartId", "39984" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcyMTcyMCwidXNlcl9pZCI6Imp5elBza3FOMElTSGdQTGM1a0ZkYjJUcmRyMDIiLCJzdWIiOiJqeXpQc2txTjBJU0hnUExjNWtGZGIyVHJkcjAyIiwiaWF0IjoxNjc2NzIxNzIwLCJleHAiOjE2NzY3MjUzMjAsImVtYWlsIjoiYTVAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImE1QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.neCBQbfbJ72D1nxg88vjiKGTjE_OhQyxK5G8buaPo7hkATDMlRtCBMFMQfYB7ML4Beuy5mcJM49zmizgG_dniX_6B7SRTrDYrG3Yz5upqdJri4j6FQNgESWPKLDCrJMObHoRq1NobndjDxc7wEx-rx40UKLmrTTiiqL7VFkjdHMxn0XHzdkVNsdfXvdVaMNwY1R4Y29JDgtV20NgD9GfL_zRRNA2PAUl9oIQq4d6BxkzBgZlf96uGznYecDjd9I0yyxCWyqIG255QKZJIm1OUDNX3QYimfUUD_Mxo2Ab09edikm4fUM5R5DE4gVZCErsXKBXnuNsnv3XKcdAY9zbsQ" },  },
           
            
            
            //new Dictionary<string, string>(){ { "email", "a6@gmail.com" }, { "chartId", "44809" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzQ1OCwidXNlcl9pZCI6ImM0NUxLa2g0VGlUMWRHaVk0VVZBU015dmJycDEiLCJzdWIiOiJjNDVMS2toNFRpVDFkR2lZNFVWQVNNeXZicnAxIiwiaWF0IjoxNjc2NzEzNDU4LCJleHAiOjE2NzY3MTcwNTgsImVtYWlsIjoiYTZAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImE2QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.PokPnFkKAvBhoFROmyYTAsjl0Oq9lQmnO6UInTsGOgeQcbVlb99JUAYklUhyFVg6g8ywBWKJCrObAMM5gzO3lCPvnzNN1GostbyYltrS6ESVj-WTrBWcWgFX0L2IyrbbPNRVNzdFGPsyYjZlwCjdpDC8Pzzs_wIbUVOwgxUqM_yavW1_H7qKC09-7zG9rO-opBtSLX9MgNuAociUCkjtW1MgyVYVoQ8bYlSDURdhnz14i_EwPoFSaUMLXXZfnvYbvx1tefOsV999nRhCXBJCaPbapwUg87v2he6J1imuH2LF93VsApdbvfhOr9q4ZX-KxXY23YRFU0gKJqxHZZLzLw" },  },
            //new Dictionary<string, string>(){ { "email", "a7@gmail.com" }, { "chartId", "44922" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzQyOCwidXNlcl9pZCI6IkVsT0xTTVpGTDVjVG5qNWNaREgxMEcxZXN5SDIiLCJzdWIiOiJFbE9MU01aRkw1Y1RuajVjWkRIMTBHMWVzeUgyIiwiaWF0IjoxNjc2NzEzNDI4LCJleHAiOjE2NzY3MTcwMjgsImVtYWlsIjoiYTdAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImE3QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.Qd3BrX7YoAFt7xfZytZ_0NA8etBAKi51K8rS5-Pb7au7JsQ0nJ1RzczMspgXw5Ao56fw4JDzn57yrxXYSXF9gQSY07i48lVGCxoL1m2LYi6dNA4O7TGuxZ019X635W-K5n65-uQBZkG1coPpneyOzu-2Rcxz-NVR49EGOgcMaifB7qsBW4juF06cde2xz19nIKL-z03bYrpqEPZ9XU2qPeraoaZHmt7mWuaA1lt-b09W-dkAsLGlgUGxYx-eITn8x0vSVs9eDQ0Ekq7uG_UGfuqKMXqPEO9fp7OwLfYU0SPzVDgsJthj65TQi1EtwVnFiifEyRbQkRi2Xj26ls4ozA" },  },
            //new Dictionary<string, string>(){ { "email", "a8@gmail.com" }, { "chartId", "44869" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzM3NSwidXNlcl9pZCI6IjhFR3hZU3hBbmNOUG1aMVVTZHdZSmNlVjhrcDIiLCJzdWIiOiI4RUd4WVN4QW5jTlBtWjFVU2R3WUpjZVY4a3AyIiwiaWF0IjoxNjc2NzEzMzc1LCJleHAiOjE2NzY3MTY5NzUsImVtYWlsIjoiYThAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImE4QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.Cdcq0GEnM2kFsdR38iQbIVW-yAJR39ZQdqyV45487esDRmPYIGmquilHa84pxjwmtEId1MfIR7JsxVY_rs5zlqtdOBU40767gJR9ZdVxPL_pypkpUbp1Nm3Q76f4CW9oSC5Ucd-pYMapB5c0XZwul-gge91kF2zZebpUkXP-M5QTGWLq0ClSRlUar13jD3B25MQpiIUMWzs98-XXeIhBkdjJBf_jBg8NnT0eFfnD8z26JCzNGT7EaAvLf6fkna_2oMifduldMVf74KVb_4OZsga3OJTOijipwqt4cPYbPJB9Cx8F8CqoX9NubKuorzJZzwrnYjxXE0SjvAdnNEmttA" },  },
            //new Dictionary<string, string>(){ { "email", "a9@gmail.com" }, { "chartId", "44920" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzMzOCwidXNlcl9pZCI6Ilp0Ump2dmpJZEdYOG5MMmpNcnVkbWNaaTBKQjIiLCJzdWIiOiJadFJqdnZqSWRHWDhuTDJqTXJ1ZG1jWmkwSkIyIiwiaWF0IjoxNjc2NzEzMzM4LCJleHAiOjE2NzY3MTY5MzgsImVtYWlsIjoiYTlAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJmaXJlYmFzZSI6eyJpZGVudGl0aWVzIjp7ImVtYWlsIjpbImE5QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.L_NiP_mAq6uUJyAeUUNztzwMRxWooUSLB6b4aUJpllXBX9At_UQJom_h8FzmAvg2Celvcn4pwB_VkC629u-yVKdK2KgmRXHwxCKprBtsmSWqzfMAsnSyp_y9wOgIBC_odx2vQ-7oV2j5zMy6hzQwHniljAgzXHJYbtMcImZX9HI_YoFD_UeKUQ74rayb38v0Z9nlJW3aKOYEakNavJJT3N4Drq470MtZR8l-COo8_HzCB9oQOzxOEvp3xeeLfl-syKHOo9F5AjZkdabtcxkRX4mOoXN226klqxxwIYYxpTqmVWpK66gIrsxHpjXqEQ-vnhNA-THgmfVca7fVuM73hA" },  },
            //new Dictionary<string, string>(){ { "email", "a10@gmail.com" }, { "chartId", "44748" },{ "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzQwMiwidXNlcl9pZCI6InA5Tnp6TXNZQ0RZMVdPMnAxVDQxaFM2ejIyYTIiLCJzdWIiOiJwOU56ek1zWUNEWTFXTzJwMVQ0MWhTNnoyMmEyIiwiaWF0IjoxNjc2NzEzNDAyLCJleHAiOjE2NzY3MTcwMDIsImVtYWlsIjoiYTEwQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTBAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.JB2Fuzzz-jiRiw0GH6Cp7ADPAFirYzQ67epLAbVFpY6GNhhCfKpA8k6bwH8o46o0z0-iMweX395eYvjLu7TmY5_WLQQeiv0KHDhGDIgfw1Ef4OEMZzdamKZp_DdoPuXXsU_mFk1AcxgXaKc8A7SBBjET0NhvSx4ih5kNzumYmNjaSDmHj_YaT1SaiIkoW1lu1SYbuTMLF58eqdzsthcvXnOBmHCJ_uflh49Sj2KVwimQuTfzptEbojwTkmc3hOkQwAf_Bl3fUh1oj6YBjuOLwCFWqBTJxBmYBmqcIR0z0w64K4mttOPqw_bP9hQafzSHAVygnCntwZur2K5kb7xPlA" },  },


            //new Dictionary<string, string>(){ {"email", "a11@gmail.com" }, { "chartId", "49932" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzEwNCwidXNlcl9pZCI6IldNRlVQOTdpOEhlVmJkVVlQWkg2REVYNDBvaTEiLCJzdWIiOiJXTUZVUDk3aThIZVZiZFVZUFpINkRFWDQwb2kxIiwiaWF0IjoxNjc2NzEzMTA0LCJleHAiOjE2NzY3MTY3MDQsImVtYWlsIjoiYTExQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTFAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.xGWCJuP25yd2ofNuxOPV-e-VljhPr1iq4Bl59_P0iUn1JFdzy__Dw-nIqdacYQPGIAZMbAz5_XXtiSinYZaSyvz2wXx7NE3WQQTEBEireBTY2EP5pI4cUCKSSEDYsHriZJjqr74uWtt9sSAYf6-4UOH-BwYQi48PVKT_2kM4HQQnIrNMD0nQJbTkR2MHUL9HwJVMXGbGAfoM9dFyD6_Zgdz-IYUtZlZpURFfVulPsSh7vdeFQivJmhvu-GFgL8_eHOD1YNF1YvVFhZxUU73XYoA5CD4UCg3AaIxQcc61_mxP3sWtkzQBFy64zd-5_H3nj8a8Ycq9fgNWj1iqxbH4fw" },  },
            //new Dictionary<string, string>(){ {"email", "a12@gmail.com" }, { "chartId", "49901" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzA4NiwidXNlcl9pZCI6ImdIMFdWS3pUek1jSWFXWGJYbnJTajVDWTlRSTIiLCJzdWIiOiJnSDBXVkt6VHpNY0lhV1hiWG5yU2o1Q1k5UUkyIiwiaWF0IjoxNjc2NzEzMDg2LCJleHAiOjE2NzY3MTY2ODYsImVtYWlsIjoiYTEyQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTJAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.wby39Gzu1M2nq1ddS0P7BBkLJhzcWrKlJMgQJxWFoEXYXZjTmQSgylqvgqrQD7o6W5bJAKMLNec2eJBwlhrHpfxoo6F_WRIRX-hzH_g1sY9iut6drRAuOvK15N8zZwm5smCpZECjp76HMIryDDr4lIFvQpB-PSnxdju8XCgVwT6mDaoDaYfQVcOYqr8XD637AnJa8JBkqdvQCgaNAHbLLNEu3nX1S7MwkjYtQMWdZAO5AiRwppdabQO9sVsK8fyEO-uESlorWdr5-H86fqNv4gWiG_nOyfuHSjNV5d0wDVYgfamjSCVYt3PKlxQUN24jx4tNc5YvYO9lrWwC3hrrfQ" }, },
            //new Dictionary<string, string>(){ {"email", "a13@gmail.com" }, { "chartId", "49865" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzA2MSwidXNlcl9pZCI6InlCVEpBRGF4a2VSUW5JWGVqQVdidG1jVEhaQjIiLCJzdWIiOiJ5QlRKQURheGtlUlFuSVhlakFXYnRtY1RIWkIyIiwiaWF0IjoxNjc2NzEzMDYxLCJleHAiOjE2NzY3MTY2NjEsImVtYWlsIjoiYTEzQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTNAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.j3IlsltZn5ok_w8PUGp_oslbdbLupRGxwb85jgf1ajj2OWx5T4UOt9q-QMKKCI4LoUgQ-DN-vuvXxuVGNkA_xm2gsNHO7BuoPkueSxFOalUcJu_AABLpH6nh38wqPSz4RO0ObW4yGhG1XwT13wKQAYOASbt8DJH93nZGMTxXONzuh9-7rGXzmj05zX5k44UP0d_2DlGdaZxnJl-R31KE_oYuh_MMtp_DFGYEz8ITju2xnzo3ncPd3E4m-MZwfwVYv3nS5zuOAmbnTtiJsfpqGAIqvkT2xSb-VABdQ7Kb77Lxz8reSjC3Afs4ZBz2w9QxgFTBF4kIRDY_DilR7KR5Fg" }, },
            //new Dictionary<string, string>(){ {"email", "a14@gmail.com" }, { "chartId", "49897" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzAzNywidXNlcl9pZCI6IkhKcm1QVjhhUTROTHAwZzZsRUJYbHpJY3dxcDEiLCJzdWIiOiJISnJtUFY4YVE0TkxwMGc2bEVCWGx6SWN3cXAxIiwiaWF0IjoxNjc2NzEzMDM3LCJleHAiOjE2NzY3MTY2MzcsImVtYWlsIjoiYTE0QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTRAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.VHZCRi5hrgzoVIO1Gv3BH3E-p14SnQ405QbRymaIvrESY59q2oSyNxSb7bqQqLhNmH_vu7QNqPdISiAicQlJGZz9j0yDzT587DZh8jvYYrxY1Qf9sXnDwy1VVec3cHGudn_DHwfRbqcDinN_k8Y88gdwXmSBCL9geXP_sFJtkYL6r8tZU4zflI04XvyrZ_nHDzPOYl8zs5UXU4oR_tf0Ii56nqtfuVXNA8uaxQkhDJVyRsqGpO8Dtz0ca-aE5FuDQc_DW5b9XyUjhCiazJA8cSVb2roqZzpGU_46okngXIaqEJsEgZ6HXBav4Qjuji52YvHLny9KazCwhhXepj-Yeg" }, },
            //new Dictionary<string, string>(){ {"email", "a15@gmail.com" }, { "chartId", "49841" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMzAxNiwidXNlcl9pZCI6IjBnWVRjRG5IQUxlVXNzTVRXVkVOTXduN3FDcDEiLCJzdWIiOiIwZ1lUY0RuSEFMZVVzc01UV1ZFTk13bjdxQ3AxIiwiaWF0IjoxNjc2NzEzMDE2LCJleHAiOjE2NzY3MTY2MTYsImVtYWlsIjoiYTE1QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTVAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.mteXEYnR6cetQjYDSwMMzlIUS0tggm7jjRxUPACSXg12ccb3wbwOdld002tvBZmbo3tByNTrMlfplEEkAJCvHQyBuWhSp5CICPcMsWvTCKEeb-fkVkc4bw8-2tA5cbZcAeTBwmByrhhJDu2ML4eMPhOQhWtyyUhiTRLt5CqiD7m9AsS1qtD_5Yjzx5BwjIbBRs6cPC9cTVMwDEpdiekeNLz_uyu07_MEIb6l4c_1d4tyk9yBB3p_U-3HN8ARykkoepHZgeKnErDw0lH1TXN5qWCQ42BJUglaj8zSPZSjx9FTWXwIXhiJ700i8aHrRDJfL_29u4neZKe5HYtWrFsdhw" }, },
                                                                            
            //new Dictionary<string, string>(){ {"email", "a16@gmail.com" }, { "chartId", "54871" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMjk5NiwidXNlcl9pZCI6IjN0SmU3aDV1QTJjRzZ2Y2EwTE9OeWtLRVV5VzIiLCJzdWIiOiIzdEplN2g1dUEyY0c2dmNhMExPTnlrS0VVeVcyIiwiaWF0IjoxNjc2NzEyOTk2LCJleHAiOjE2NzY3MTY1OTYsImVtYWlsIjoiYTE2QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTZAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.IWszdTdLJTY0Ihr3Fs0f4hwacPkd9eALRzIskKk_7CHQrfbbkVg-2knaRV1ljzGaF7QA7dZ3D9H_h_ZY3cXYNbm2b9TcCpw34Pw0qE1PU1yW7DILpXWB4EcysAs8tswbJWo-3XgVUEDckW6TX7LUYldSI3FpIaz0gbgGqNix0fIJQ38yCbcrdHmVs90UHaz7BDm4jLigmRZrm-ApRoc9hP2890RQXgw1uRRur-9s-GRVs-jcakL1QD5ZcV4qLuiY5gQV-gpPf6r3bMKyWN5C0ox8z1umgDB0tAOaRQJ5rAfWESZvvAlorMp7MiMISRnH_wcc3L1psqbS73ZUwadYbw" }, },
            //new Dictionary<string, string>(){ {"email", "a17@gmail.com" }, { "chartId", "54921" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMjk3NCwidXNlcl9pZCI6Ijk5eXV3SnZkNzBOVW4yY2JnVjk2ak93NldIczEiLCJzdWIiOiI5OXl1d0p2ZDcwTlVuMmNiZ1Y5NmpPdzZXSHMxIiwiaWF0IjoxNjc2NzEyOTc0LCJleHAiOjE2NzY3MTY1NzQsImVtYWlsIjoiYTE3QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTdAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.Okm15iVDua5idjMpqWGd3ppfIiTKhD2R79oo7tombMgQ-bNVZM3_Kc3ImxKh86ofGVUAE95lg28ThG913NdzNuU7S7MjNX_iUPySJd7csmERIEOTom6b1SV5k3-3rvVcC-pBs4cOHQms5_AA1z6vNpCfDREKG4Wk2gMy3B27ZIgd190X7kJ0vsCWLJwmr_A2wgwstmOXVmzbBwO0QpAiSUnAuEzfS2TV1Sg2fNS2nbzfGTG4GPjluh8KX12aCPTVj1D45RUylStUd5fylzbCC7w337U163NU6wTGq2Wr79pty7Nqsz3y80k9gTIH9-ocAfI-F8btcf0NcmqFx7S4GQ" }, },
            //new Dictionary<string, string>(){ {"email", "a18@gmail.com" }, { "chartId", "54941" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMjk1MywidXNlcl9pZCI6IkxHRGZrUEY2Y2RhdlZzR1lDbEFwYldiTzBaRzIiLCJzdWIiOiJMR0Rma1BGNmNkYXZWc0dZQ2xBcGJXYk8wWkcyIiwiaWF0IjoxNjc2NzEyOTUzLCJleHAiOjE2NzY3MTY1NTMsImVtYWlsIjoiYTE4QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMThAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.MlXYCMly5Chy2LJ3DipG5AgY3V2tS3A5zBxvTqN4gLle-cNZRhzMu6By0qUGJJDXqJHp9UWBOGAywCEHfrFG0PafpTGLprdUcRxHSmmH7yw14nuUyTWBoF5cq0hmJMWMHT29FXML4C4kW_Mz8py3ZuG_97pRLVptq7V5S1JYTds52A_HT7NBsO1XdkCifIC2uXJwP1C10Sp0AbiIT5edGTpt-KEOiEWOxjVG1jasQwMQIRbMfI3z3k4LEnq0eX6ufM4e4zni_PKYxwc017I8SZRh40iJHtaQn3oYE_s81aG8yQdDte4iVRUvJO9hv4C6sZwZJ9xkOzSrpZPZHSVUBg" }, },
            //new Dictionary<string, string>(){ {"email", "a19@gmail.com" }, { "chartId", "54924" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcxMjkyNiwidXNlcl9pZCI6IjQ0WWhuWE44dmtYUm5SZUpFTWlWcnRhcGJWRTMiLCJzdWIiOiI0NFloblhOOHZrWFJuUmVKRU1pVnJ0YXBiVkUzIiwiaWF0IjoxNjc2NzEyOTI2LCJleHAiOjE2NzY3MTY1MjYsImVtYWlsIjoiYTE5QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMTlAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.vCfo0lRc97iUZCPIi0dw0fyGuJG9zCS9VHAUj7EHLJznknTwh8yVZ7HUy9dp1uU8wkNw4UAGJFH27uU5jxpOhJ6k-6lVw0vC799N3hF1Y0YnZlq0KyfgeOHmqF-Wu_HjtW4E1n-ga2-nuNbId_IWpYTcSr9l_arLQi_9d5Nyh5AbH6buC0K6NzpmwDbDzPxv3qCkQdK63F61e2fjuYpx32tsXSi8oIqX10lDlzay5V12QRaU3fPF_1sZppiXVSVDCvl6YhJy7ZVKgyxX9AgMzqftwmzFi03uUAj5GsvBLk_rTWk8MKc_OeORNz7YmFtKbehhqmERQzQAOXUuXNb_QQ" }, },
            //new Dictionary<string, string>(){ {"email", "a20@gmail.com" }, { "chartId", "54822" }, { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjcwNzgxNiwidXNlcl9pZCI6IkJvcFNwNVdobXdUcDBnME5IZk5aVjV5YVpkcDIiLCJzdWIiOiJCb3BTcDVXaG13VHAwZzBOSGZOWlY1eWFaZHAyIiwiaWF0IjoxNjc2NzEyODczLCJleHAiOjE2NzY3MTY0NzMsImVtYWlsIjoiYTIwQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMjBAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.5t7MUmLtbuU-HxxwheNjPPii5U3oOsV6iz8u3UbXm_LqDUdX2ZqiRbLwI1WDIrbbJBuUKnwpE55mpdrGxeAby7rOoFM-kmFKApWuc9ZjFHjG6-v0HzLatP4-HyPf0uAC2igO39sC_QfFHAtNSO-4ACgNB7q1Ql_ebm8jW50POwZk_gXLNFEvzbUL-a-hVOPTC0QGbv1_mQliYgOYJmgAhqDToDk9JVDk_BboFw0c-BjjpjADAAYvF1ydtUvkykasq1Lr5_P9sRJGJUUlZ42OFgC5d8I5pYDBeiLUd9VQZs-MOsZDbPm_c6g7D4M3LnLX7ABDRMzRy6MLpeAa3JDkUw" }, },

            //new Dictionary<string, string>(){ {"email", "a21@gmail.com" },  { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjY5MDA0NywidXNlcl9pZCI6InZLMmliZ3NOckxWQldyaHVUU1VEZUZyRW9SVzIiLCJzdWIiOiJ2SzJpYmdzTnJMVkJXcmh1VFNVRGVGckVvUlcyIiwiaWF0IjoxNjc2NjkwMDQ3LCJleHAiOjE2NzY2OTM2NDcsImVtYWlsIjoiYTIxQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMjFAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.SDRdkRmKKN7Qbp8vb3saBeZmNyzOOVZVVnNYUVvAdD0cLMarusFLfFR7mIEzWXUzkyD6cKQGqRwSqF30Q5AlQFduaVo2vH7ZzRIu_g8xmp_yACmCIHFmLQPo_wDulTL9JeUPs09XoRwNvxPazo93wHZGlhJg3JsS8U-vULVv24IWM2ztvtwOfNPM-kTkBBemsE0i1jltIDu7aVzwNLTLkDET9yiCNaWC6q4lKVsboDxoMH80aEhD-ON-W0NnoiSwLyajWIub9Wtt3WiRMBkzbVEec3GAqstfRP1QKjCRkpfL3OiZXQHwaXn1b16DEfzWkfdAsxS5tDJk1GWMWlH6WA" }, },
            //new Dictionary<string, string>(){ {"email", "a22@gmail.com" },  { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjY5MDc3NiwidXNlcl9pZCI6Ind0YkJQNnJ4WEFXSXhPZk9iRFl1QnB0V0NuQjMiLCJzdWIiOiJ3dGJCUDZyeFhBV0l4T2ZPYkRZdUJwdFdDbkIzIiwiaWF0IjoxNjc2NjkwNzc2LCJleHAiOjE2NzY2OTQzNzYsImVtYWlsIjoiYTIyQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMjJAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.fEY9xto5oZoD8oS4ddllUtEmK5htTapaTS_5BDMAhEXtBwDA_3q8voNeA3dVyKwyJusv8JTNCIkiapiwjHUTk3kjZhjq8H-djaY455--pO56xKNtXddtU-ID4CVHf2c0-q4dZIAbw8rPym3pqJjttcX8e3tmjAhoi4K_1B3UUmTcZmGYTPc_8BH0ZjJa5iGZzIeEqHxxnKRF8ITAkG-b5rBIZdbC6YMma74Z_QPV9mIrpk9VsRhM249esjeopSKCDIj_Xtr54d7udOoRfgvuEftD41BXCppgcfu3remBy039mClf1qZesinYJW5c_sxIX7tO7wQN4rubasU1kNiFlg" }, },
            //new Dictionary<string, string>(){ {"email", "a23@gmail.com" },  { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjY5MDgxMywidXNlcl9pZCI6InhxT3cyaDZHaWtVaTZONk9EU0w1RE92OHlGQTIiLCJzdWIiOiJ4cU93Mmg2R2lrVWk2TjZPRFNMNURPdjh5RkEyIiwiaWF0IjoxNjc2NjkwODEzLCJleHAiOjE2NzY2OTQ0MTMsImVtYWlsIjoiYTIzQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMjNAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.SE3RKzVltSLzbdWSm48aIE1FqqF7xvZq-YQPxLpY6OYr8BQvKN1a9U-ZlqI7th74Hl4WLdkQJ0MZlj9ek4wssol8Oy6TYdCZHYOXbM8Z-LzdMkmn10pPLarJgb_Ip0e0BMpfzBs0oSdrg358fygFCSjs1s0D1UckBVvwLYsw_oyLaDtLts5X12vd0IF0sZ8jA1JHf1BA2hCLohDnw_oE_e-JqK6GJvQHjVWeuNgCzTNFrUjpNFCzHzlW9E053V5q6r7m_ZkTx9vowYcYg4uKeF1M0zLjhTrbNgeNkWw81VKsZOCHtf4eBGMtzOKuf5ODjhtkIDM7lgfMjFT_wtxRAQ" }, },
            //new Dictionary<string, string>(){ {"email", "a24@gmail.com" },  { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjY5MDg2MiwidXNlcl9pZCI6IlowMzk3UlM3V0lWY3BzWGhIV3lXWlEzbmt4RTIiLCJzdWIiOiJaMDM5N1JTN1dJVmNwc1hoSFd5V1pRM25reEUyIiwiaWF0IjoxNjc2NjkwODYyLCJleHAiOjE2NzY2OTQ0NjIsImVtYWlsIjoiYTI0QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMjRAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.03ugD2U_HWG6r-b6kxlzI2rV8bSl4x8tuyZCM7ZLzIyWWCi-09gbymgcoyXI1u9PrUVj2AQPZJJXHyh3eCxA10Y3T_smxsjJeq-oJVlDtPsf-pO3iMmmrPJmDBF_fUbrn3Z7233Kjh-y13qQDt7XrVeoXfEsRTEQdZpILb_k_Nq8mDasb3QKQJDaDZbeDqGRauXs5mPnuLdHaATJ9JHkcXpgGtN2W5ewoYhxyXHhMg82mlkV6aQ2HySiyzG_xQVz2-6VEaKi39MAkrtbwWCQaINSaKR7OICC_xEiMo1SjBNKqP4pDJY2MpMPlBg4pI-D8Xj7H3xbmrUtSwQwVyKffA" }, },
            //new Dictionary<string, string>(){ {"email", "a25@gmail.com" },  { "token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFlYjMxMjdiMjRjZTg2MDJjODEyNDUxZThmZTczZDU4MjkyMDg4N2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vc2VsZi1wbHVzLXRvbmdzaHUiLCJhdWQiOiJzZWxmLXBsdXMtdG9uZ3NodSIsImF1dGhfdGltZSI6MTY3NjY5MDg5MiwidXNlcl9pZCI6InZSR1I4SG1OSVpXVmZqWWhBZDg0S2loaU85MzMiLCJzdWIiOiJ2UkdSOEhtTklaV1ZmalloQWQ4NEtpaGlPOTMzIiwiaWF0IjoxNjc2NjkwODkyLCJleHAiOjE2NzY2OTQ0OTIsImVtYWlsIjoiYTI1QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJhMjVAZ21haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.wG96kD9Dl8EX0R7BqsreztGjEy3eLgIfF9ineb9THiagxZgQ1vxmzIBSswzhcSRnbz1K9EFJ1KpVLraAa0pgO1IaZEB_CZTD20EfzFe_NR1fisgOOT-KP_hoUoa9Y_LPzgMK-p4NdWqwznxKknAEZjxS0CO9aD3MaqGNbw6f68NgvSkpc4oBxW2a5XIqz7os9N0sXPl8FRaGPMrxP0sebqPNQv5KXvw2_L0SIU4NRs_grqwz_8g0ICQ1_Bww7bgODzXDPLdQQcn-7dlhiy8NphGqUHtfxqGMnU_Q7TKFcLiSBpHUNmWzK57dDwKkWNX3BUyL_Y7TQHEWF2sM_RzWWg" }, },

            //new Dictionary<string, string>(){ {"email", "a26@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a27@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a28@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a29@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a30@gmail.com" },  { "token", ""}, },

            //new Dictionary<string, string>(){ {"email", "a31@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a32@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a33@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a34@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a35@gmail.com" },  { "token", ""}, },

            //new Dictionary<string, string>(){ {"email", "a36@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a37@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a38@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a39@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a40@gmail.com" },  { "token", ""}, },

            //new Dictionary<string, string>(){ {"email", "a41@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a42@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a43@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a44@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a45@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a46@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a47@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a48@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a49@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a50@gmail.com" },  { "token", ""}, },

            //new Dictionary<string, string>(){ {"email", "a51@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a52@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a53@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a54@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a55@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a56@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a57@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a58@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a59@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a60@gmail.com" },  { "token", ""}, },



            //new Dictionary<string, string>(){ {"email", "a61@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a2@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a3@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a4@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a5@gmail.com" },  { "token", ""}, },

            //new Dictionary<string, string>(){ {"email", "a6@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a7@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a8@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a9@gmail.com" },  { "token", ""}, },
            //new Dictionary<string, string>(){ {"email", "a0@gmail.com" },  { "token", ""}, },

        };
        //XTVCE1
        private List<ChromeDriver> chromeDrivers = null;
        private void btnCreateChartId_Click(object sender, EventArgs e)
        {
           
            chromeDrivers = new List<ChromeDriver>();
            foreach (var item in lst_account)
            {
                if (string.IsNullOrEmpty(item["email"])) continue;
                Thread.Sleep(sleepTime);
                Task.Factory.StartNew(new Action(() =>
                {
                    var chrome = new ChromeDriver();
                    chromeDrivers.Add(chrome);
                    //RUN_BatTu_CreateChartId(chrome, item["email"], item["pw"], int.Parse(item["idGioiTinh"]), int.Parse(item["namBatDau"]));
                    RUN_BatTu_CreateChartId(chrome, item["email"] );
                }));
            }
        }
        private void btnGetDATA_CLICK(object sender, EventArgs e)
        {
            chromeDrivers = new List<ChromeDriver>();
            foreach (var item in lst_account)
            {
                if(string.IsNullOrEmpty(item["token"])) continue;
                Thread.Sleep(sleepTime);
                var chrome = new ChromeDriver();
                chromeDrivers.Add(chrome);
                Task.Factory.StartNew(new Action(() => { 
                    RUN_BatTu_GetData(chrome, item["email"], item["token"]);
                }));
            }
        }
        private void btnCreateGetDATA_CLICK(object sender, EventArgs e)
        {
            chromeDrivers = new List<ChromeDriver>();
            foreach (var item in lst_account)
            {
                if (string.IsNullOrEmpty(item["token"])) continue;
                Thread.Sleep(sleepTime);
                var chrome = new ChromeDriver();
                chromeDrivers.Add(chrome);
                Task.Factory.StartNew(new Action(() => {
                    RUN_BatTu_Create_GetData(chrome, item["email"], item["token"], int.Parse(item["chartId"]));
                }));
            }
        }
    }
}
