using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Cloud.TextToSpeech.V1;
using HtmlAgilityPack;
using NAudio.Wave;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using File = System.IO.File;

namespace CatCode_Selenium
{
    public class NoiDungTruyen
    {
        public string noiDungTruyen { set; get; }
        public string type { set; get; }
    }
    public class FPT_TextToSpeech
    {
        public string async { set; get; }
        public string error { set; get; }
        public string message { set; get; }
        public string request_id { set; get; }
    }
    public class utterance_obj
    {
        public string confidence { set; get; }
        public string utterance { set; get; }
    }
    public class FPT_SpeechToText
    {
        public string status { set; get; }
        public string id { set; get; }
        public string message { set; get; }
        public List<utterance_obj> hypotheses { set; get; }

    }


    public partial class UGSpeechToText : UserControl
    {
        Dictionary<string, string> dictChange = new Dictionary<string, string>() {
            {"vợ","con vợ" },
            {" lo "," phờ-lo "},
            {"cút","chim cút" },
            {"biến", "tốc biến" },
            {"té","chim té" },
            {"bật ngữa","bật ngữa, ngỡ ngàng và ngơ ngác" },
        };
        public UGSpeechToText()
        {
            InitializeComponent();
            var lst = dictChange.Keys.ToList();
            foreach (var k in lst)
            {
                dictChange[Capitalze(k)] = Capitalze(dictChange[k]);
            }
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
                    if (!string.IsNullOrEmpty(process_mgs)) this.lblProcessMessage.Text = process_mgs;
                    lblPercent.Text = Math.Round((this.progressBar1.Value * 100.0 / (double)progressBar1.Maximum), 2).ToString() + "%";
                }));
            }
            catch
            {

            }

        }

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



        #region Text-To-Speech 
        internal async void TextToSpeech_Full()
        {
            string output_dir = System.Windows.Forms.Application.StartupPath + @"\output_TextToSpeech";
            if (!Directory.Exists(output_dir)) Directory.CreateDirectory(output_dir);
            //string title_url = "xuyen-thanh-bach-nguyet-quang-yeu-menh-ta-cung-vai-ac-he";
            //string title_url = "overgeared-tho-ren-huyen-thoai";
            string title_url = "du-tinh-kha-dai";
            var dtTruyen = Program.ExcecuteDataTable("SELECT * FROM tblTruyen where title_url = N'" + title_url + "'");
            if (dtTruyen.Rows[0]["doneAudio"] != DBNull.Value)
            {
                MessageBox.Show("doneAudio is not null");
                return;
            }
            var idTruyen = (int)(dtTruyen.Rows[0]["ID"]);
            var title_truyen = (string)(dtTruyen.Rows[0]["title"]);
            var dtChuong = Program.ExcecuteDataTable("sp_dsChuong", new Dictionary<string, object> { { "@idTruyen", idTruyen }, { "@getAll", true } });
            string tableName_dsChuong = Program.GetTableName_dsChuong(idTruyen);

            string dir_title_url = output_dir + "\\" + idTruyen + "-" + title_url;
            if (!Directory.Exists(dir_title_url)) Directory.CreateDirectory(dir_title_url);
            var dtRowChuong_Text = dtChuong.AsEnumerable().Where(dr => dr["doneAudio"] == DBNull.Value).OrderBy(dr => (int)dr["chuong"]);
            if (!dtRowChuong_Text.Any())
            {
                MessageBox.Show("Danh sách chương cần lấy đã có đủ audio(doneAudio != DBNull.Value)");
            }
            this.SetupProgessBar(dtRowChuong_Text.Count());
            foreach (DataRow drChuong in dtRowChuong_Text)
            {
                this.ChangeProgessBar(this.progressBar1.Value + 1, string.Empty);
                string uriChuong = (string)drChuong["uriChuong"];
                string output_full_dir = dir_title_url + "\\output";
                if (!Directory.Exists(output_full_dir)) Directory.CreateDirectory(output_full_dir);
                string output_full_chuong = output_full_dir + "\\" + uriChuong + ".mp3";
                if (File.Exists(output_full_chuong))
                {
                    continue;
                }

                var lstText = await GetListText_Chuong(title_url, chuong: uriChuong);
                if (!lstText.Any()) continue;

                string titleChuong = (string)drChuong["title"];
                int chuong = (int)drChuong["chuong"];
                string dir_chuong = dir_title_url + "\\" + uriChuong;
                if (!Directory.Exists(dir_chuong)) Directory.CreateDirectory(dir_chuong);

                titleChuong = XuLyTitleChuong(titleChuong, chuong);
                lstText.Insert(0, titleChuong);
                lstText.Add("\nHết chương " + chuong + ".");
                this.UpdateTextControl(lblPercent, idTruyen.ToString());
                this.UpdateTextControl(lblBaseURL, String.Format("[{0}] {1} - {2}", idTruyen, title_truyen, uriChuong));
                var lstOutput = new List<string>();
                for (int i = 0; i < lstText.Count; i++)
                {
                    string output_fileName = i + ".mp3";
                    string output_fullPath = dir_chuong + "\\" + output_fileName;
                    lstOutput.Add(output_fullPath);
                    var text = lstText[i];
                    if (File.Exists(output_fullPath)) continue;
                    var valid = TextToSpeech(text, output_fullPath);
                    if (!valid || !File.Exists(output_fullPath))
                    {
                        MessageBox.Show("File Not found - hết API");
                        return;
                    }
                }
                Program.MergeMp3File(output_full_chuong, lstOutput);
                int idChuong = (int)drChuong["ID"];
                Program.ExcecuteDataTable("update [" + tableName_dsChuong + "] set doneAudio = 1 where ID = " + idChuong);
            }
            Program.ExcecuteDataTable("update tblTruyen set doneAudio = 1 where ID = " + idTruyen);
        }

        private static string XuLyTitleChuong(string titleChuong, int chuong)
        {
            if(!string.IsNullOrEmpty(titleChuong) && int.TryParse(titleChuong.Trim(), out int i))
            {
                return "Chương " + chuong  +".\n\n\n";
            }
            else
            {
                return "Chương " + chuong + ": " + titleChuong + ".\n\n\n";
            }
        }

        private string GetFptAPI()
        {
            try
            {
                var dtAPI = Program.ExcecuteDataTable("SELECT * FROM tblFPTAPI where coTheSuDung = 1 order by ngayTao");
                return (string)dtAPI.Rows[0]["api"];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private async Task<List<string>> GetListText_Chuong(string title_url, string chuong)
        {
            var lstResult = new List<string>();
            var data = await GetNoiDungTruyen(truyen: title_url, chuong: chuong);
            if (data == null) return lstResult;
            data.noiDungTruyen = data.noiDungTruyen
                .Replace("<br>", "\r\n")
                .Replace("<br/>", "\r\n")
                .Replace("..", ".")
                .Replace(". .", ". ")
                .Replace("  ", " ")
                ;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(data.noiDungTruyen);
            var textOnly = htmlDoc.DocumentNode.InnerText;
            textOnly = VuiVe(textOnly);
            textOnly = textOnly.Replace("Chương này có nội dung ảnh, nếu bạn không thấy nội dung chương, vui lòng bật chế độ hiện hình ảnh của trình duyệt để đọc", "");
            textOnly = textOnly.Replace("*.", "");
            var arrText = textOnly.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(i=> !string.IsNullOrEmpty(i.Trim()))
                .Select(i=> i.Trim())
                .ToList();
            try
            {
                string text = string.Empty;
                int max_length = 4900;
                foreach (var para in arrText)
                {
                    string newText = para.Trim();
                    if (!newText.EndsWith(".")) newText += ".";

                    if ((text + newText).Length > max_length)
                    {
                        lstResult.Add(text);
                        text = newText;
                    }
                    else
                    {
                        text = text + "\n" + newText;
                    }
                }
                lstResult.Add(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return lstResult;
        }

        public bool TextToSpeech(String text, string output)
        {
            string api;
            try
            {
                do
                {
                    api = this.GetFptAPI();
                    var valid = TextToSpeech(text, output, api);
                    if (valid && File.Exists(output))
                    {
                        Program.ExcecuteNoneQuery("update tblFPTAPI set  suDungLanCuoi = GETDATE(),soKyTuDaTao = soKyTuDaTao + " + text.Length + " where api = '" + api + "'");
                        return true;
                    }
                    else
                    {
                        Program.ExcecuteNoneQuery("update tblFPTAPI set coTheSuDung = 0 , suDungLanCuoi = GETDATE()  where api = '" + api + "'");
                    }
                } while (!string.IsNullOrEmpty(api));
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string VuiVe(string text)
        {
            foreach (var item in dictChange)
            {
                text = text.Replace(item.Key, item.Value);
            }
            return text;
        }
        string Capitalze(string str)
        {
            if (str.Length == 0)
                return str;
            else if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
        }
        private bool TextToSpeech(string text, string output, string api)
        {
            try
            {
                this.UpdateTextControl(lblProcessMessage, "TextToSpeech: " + api);
                String result = Task.Run(async () =>
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("api-key", api);
                    client.DefaultRequestHeaders.Add("speed", "0.5");
                    client.DefaultRequestHeaders.Add("voice", "banmai");
                    var response = await client.PostAsync("https://api.fpt.ai/hmi/tts/v5", new StringContent(text));
                    return await response.Content.ReadAsStringAsync();
                }).GetAwaiter().GetResult();

                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FPT_TextToSpeech>(result);
                this.UpdateTextControl(lblProcessMessage, "Error: " + obj.error + " Message: " + obj.message);
                if (obj.error == "0")
                {
                    int count_wait = 0;
                    while (!File.Exists(output))
                    {
                        Thread.Sleep(10000);
                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(new Uri(obj.async), output);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.UpdateTextControl(lblProcessMessage, "Error: " + obj.error + " Message: " + obj.message + " Thread.Sleep:" + (++count_wait));
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<NoiDungTruyen> GetNoiDungTruyen(string truyen, string chuong)
        {
            try
            {
                string urlRequest = "https://truyenfree.net/Truyen/Contents";
                urlRequest = string.Format(urlRequest, truyen, chuong);
                HttpClient client = new HttpClient();
                var content = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("truyen", truyen),
                     new KeyValuePair<string, string>("chuong", chuong),
                });
                var responce = await client.PostAsync(urlRequest, content);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<NoiDungTruyen>(await responce.Content.ReadAsStringAsync());
                return obj;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        #endregion

        #region Speech to Text

        public bool SpeechToText(string audioPath, string output, string api)
        {
            api = "BsoKyw88SrHAcj9etXs553fzULF5YX2c";
            audioPath = "C:\\Users\\linh.nguyenb\\Downloads\\Thức Tỉnh Kĩ Năng Tử Vong x1 5p.mp3";
            try
            {
                this.UpdateTextControl(lblProcessMessage, "TextToSpeech: " + api);

                String result = Task.Run(async () =>
                {
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromDays(1);
                    var payload = File.ReadAllBytes(audioPath);
                    client.DefaultRequestHeaders.Add("api-key", api);
                   var response = await client.PostAsync("https://api.fpt.ai/hmi/asr/general", new ByteArrayContent(payload));
                    var rs = await response.Content.ReadAsStringAsync();
                    return rs;
                }).GetAwaiter().GetResult();
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FPT_SpeechToText>(result);
                MessageBox.Show(obj.hypotheses[0].utterance);
                this.UpdateTextControl(lblProcessMessage, "Error: " + obj.message + " Message: " + obj.message);
                if (obj.status == "0")
                {
                    return true;
                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }

}
