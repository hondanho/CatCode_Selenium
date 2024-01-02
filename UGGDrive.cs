using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;

namespace CatCode_Selenium
{
    public partial class UGGDrive : UserControl
    {
        public UGGDrive()
        {
            InitializeComponent();
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

        #region UGGDrive
        string publicFolderId { get { return Program.configuration.publicFolderId; } }
        string googleDriveApiKey { get { return Program.configuration.googleDriveApiKey; } }

        public async Task START_UGGDrive_Truyen()
        {

            try
            {
                var dtTruyen = Program.ExcecuteDataTable("select ID from tblTruyen");
                var lstID = dtTruyen.AsEnumerable().Select(dr => (int)dr["ID"]).ToList();
                SetupProgessBar(lstID.Count);
                int valProcess = 0;
                var httpClient = new HttpClient();
                var nextPageToken = "";
                int page = 0;
                do
                {
                    var folderContentsUri = $"https://www.googleapis.com/drive/v3/files?q='{publicFolderId}'+in+parents&key={googleDriveApiKey}";
                    if (!string.IsNullOrEmpty(nextPageToken)) folderContentsUri += $"&pageToken={nextPageToken}";
                    var contentsJson = await httpClient.GetStringAsync(folderContentsUri);
                    if (string.IsNullOrEmpty(contentsJson)) continue;
                    try
                    {
                        var ggDriveFolder = Newtonsoft.Json.JsonConvert.DeserializeObject<GGDriveFolder>(contentsJson);
                        if (ggDriveFolder == null) continue;
                        nextPageToken = ggDriveFolder.nextPageToken;
                        if (!ggDriveFolder.files.Any()) continue;
                        page++;
                        foreach (var f in ggDriveFolder.files)
                        {
                            int idTruyen;
                            if (!f.mimeType.Contains("folder") || f.name.Contains(".") || !int.TryParse(f.name, out idTruyen) || idTruyen == 0) continue;
                            ChangeProgessBar(valProcess++, String.Format("Page:{0} ID:{1} fid:{2}", page, f.name, f.id));
                            if (!lstID.Contains(idTruyen))
                            {
                                this.DeleteDirectory(idTruyen);
                            }
                            else
                            {
                                Program.ExcecuteNoneQuery("update tblTruyen set ggd_id = @ggd_id, visible = 1, daXuLy_ggd_id = null where ID = @ID and isnull(ggd_id,'') !=  @ggd_id",
                                                      new Dictionary<string, object>() { { "@ggd_id", f.id }, { "@ID", idTruyen } });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.WriteLog(ex.Message);
                    }

                } while (!String.IsNullOrEmpty(nextPageToken));
            }
            catch (Exception)
            {

            }
        }

        private void DeleteDirectory(int idTruyen)
        {
            try
            {
                var direc = string.Format("C:/Working/CatCode_Selenium/CatCode_Selenium/bin/Release/DataTruyen/My Drive/DataTruyen/{0}/", idTruyen);
                if (Directory.Exists(direc))
                {
                    Directory.Delete(direc, true);
                }
            }
            catch (Exception ex)
            {
                this.WriteLog(ex.Message);
            }
        }

        public async Task START_UGGDrive_Chuong(DataRow drTruyen)
        {
            try
            {
                var httpClient = new HttpClient();
                int valProcess = 0;
                var ggd_idTruyen = drTruyen["ggd_id"].ToString();
                var tenTruyen = drTruyen["title"].ToString();
                var idTruyen = (int)drTruyen["ID"];
                var idLoaiTruyen = (int)drTruyen["idLoaiTruyen"];
                var nextPageToken = "";
                string tableName_dsChuong = Program.GetTableName_dsChuong(idTruyen);

                var dtChuong = Program.ExcecuteDataTable("select * from " + tableName_dsChuong + " where  uriChuong != '' and refID = " + idTruyen);
                var lstChuongDaCo = dtChuong.AsEnumerable().Where(dr => dr["ggd_id"] != DBNull.Value).Select(dr => (string)dr["uriChuong"]).ToList();
                SetupProgessBar(dtChuong.Rows.Count);
                do
                {
                    var folderContentsUri = $"https://www.googleapis.com/drive/v3/files?q='{ggd_idTruyen}'+in+parents&key={googleDriveApiKey}";
                    if (!string.IsNullOrEmpty(nextPageToken)) folderContentsUri += $"&pageToken={nextPageToken}";
                    var contentsJson = await httpClient.GetStringAsync(folderContentsUri);
                    if (string.IsNullOrEmpty(contentsJson)) continue;
                    try
                    {
                        var ggDriveFolder = Newtonsoft.Json.JsonConvert.DeserializeObject<GGDriveFolder>(contentsJson);
                        if (ggDriveFolder == null) continue;
                        nextPageToken = ggDriveFolder.nextPageToken;
                        if (!ggDriveFolder.files.Any()) continue;
                        foreach (var f in ggDriveFolder.files)
                        {
                            ChangeProgessBar(valProcess++, String.Format("[" + idTruyen + "]" + tenTruyen + " - {0} - {1}", f.name, f.id));
                            if (string.IsNullOrEmpty(f.name) || !f.name.Contains(".")) continue;
                            if (idLoaiTruyen == 1)//truyen chữ
                            {
                                var uriChuongGGDrive = f.name.Replace(".txt", "");
                                if (lstChuongDaCo.Contains(uriChuongGGDrive)) continue;
                                lstChuongDaCo.Add(uriChuongGGDrive);
                                Program.ExcecuteNoneQuery("UPDATE " + tableName_dsChuong + " SET ggd_id = @ggd_id WHERE uriChuong = @uriChuong AND refID = @idTruyen",
                                    new Dictionary<string, object>() {
                                      { "@ggd_id", f.id }
                                    , { "@uriChuong", uriChuongGGDrive }
                                    , { "@idTruyen", idTruyen }
                                    }
                                );
                            }
                            else if (idLoaiTruyen == 2)//truyện tranh
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.WriteLog(ex.Message);
                    }
                } while (!String.IsNullOrEmpty(nextPageToken));

                if (dtChuong.Rows.Count == lstChuongDaCo.Count)
                {
                    Program.ExcecuteDataTable("update tblTruyen set daXuLy_ggd_id = 1 where ID = " + idTruyen);
                }
            }
            catch (Exception)
            {
                 
            }
        }


        #endregion

    }
}
