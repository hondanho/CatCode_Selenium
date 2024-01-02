using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Google.Api;
using Google.Apis.Util;
using NAudio.Gui;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace CatCode_Selenium
{
    public partial class UploadCloudinary : Form
    {
        public UploadCloudinary()
        {
            InitializeComponent();
            if (!System.IO.Directory.Exists(Application.StartupPath + "\\tmp_photo"))
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\tmp_photo");
            }
        }

   
        private void UpdateExeption(Exception ex)
        {
            this.Invoke(new Action(() =>
            {
                richTextBox1.Text = ex.Message;
            }));
        }
        private void UpdateProgressBar(string mgs = "")
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    label1.Text = mgs;
                    progressBar.Value++;
                    progressBar.Update();
                }));
            }
            catch (Exception ex)
            {
                UpdateExeption(ex);
            }

        }
 

        #region Ảnh bìa truyện - urlHinhAnh_image_path
        private void UploadButton_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(async () =>
            {
                while (!checkBox1.Checked)
                {
                    await StartUploadAsync();
                }
            });
        }

        private async Task StartUploadAsync()
        {
            var dtUpload = Program.ExcecuteDataTable("select TOP(1000) ID,title_url, urlHinhAnh_image_path,nguon_url from tblTruyen where urlHinhAnh_image_path is not null and cloudinary_url is null order by luotXem");
            if (dtUpload.Rows.Count == 0)
            {
                MessageBox.Show("Hết Ảnh bìa cần upload");
                Application.Exit();
            }
            this.Invoke(new Action(() =>
            {
                progressBar.Maximum = dtUpload.Rows.Count;
                progressBar.Minimum = 0;
                progressBar.Value = 0;
            }));
            int idCloudinary = 0;
            var cloudinary = Program.GetCloudinary(ref idCloudinary);
            if (cloudinary == null)
            {
                UpdateExeption(new Exception("Find not found \"cloudinary account\""));
                return;
            }
            foreach (DataRow dr in dtUpload.Rows)
            {
                string filePath = string.Empty;
                try
                {
                    int ID = (int)dr["ID"];
                    string urlHinhAnh_image_path = dr["urlHinhAnh_image_path"].ToString();
                    string title_url = dr["title_url"].ToString();
                    string nguon_url = dr["nguon_url"].ToString();
                    string folder = Program.GetTableName_dsChuong(ID, tbl: "f");
                    UpdateProgressBar(urlHinhAnh_image_path);
                    filePath = Application.StartupPath + "\\tmp_photo\\" + title_url + ".png";
                    var rsDownloadImage = Program.DownloadImage(urlHinhAnh_image_path, filePath);
                    if (!rsDownloadImage || !System.IO.File.Exists(filePath) || new System.IO.FileInfo(filePath).Length == 0)
                    {
                        UpdateExeption(new Exception("Find not found \"" + urlHinhAnh_image_path + "\""));
                        Thread.Sleep(1000);
                        rsDownloadImage = Program.DownloadImage(urlHinhAnh_image_path, filePath);
                        if (!rsDownloadImage || !System.IO.File.Exists(filePath) || new System.IO.FileInfo(filePath).Length == 0)
                        {
                            Program.ExcecuteNoneQuery("UPDATE tblTruyen set urlHinhAnh_image_path = null where ID = @ID"
                           , new Dictionary<string, object>() { { "@ID", ID } });
                            continue;
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        try
                        {
                            this.pictureBox1.Image = new Bitmap(filePath);
                        }
                        catch (Exception ex)
                        {
                            UpdateExeption(ex);
                        }
                    }));
                    if (!System.IO.File.Exists(filePath)) continue;
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(filePath),
                        Folder = folder,
                        AssetFolder = folder,
                        Overwrite = true,
                    };
                    ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                    var cloudinary_url = uploadResult?.SecureUrl?.AbsoluteUri;
                    if (!string.IsNullOrEmpty(cloudinary_url))
                    {
                        Program.ExcecuteNoneQuery("UPDATE tblTruyen set cloudinary_url = @cloudinary_url where ID = @ID"
                            , new Dictionary<string, object>() { { "@ID", ID }, { "@cloudinary_url", cloudinary_url } });
                    }
                }
                catch (Exception ex)
                {
                    UpdateExeption(ex);
                }
                finally
                {
                    DeleteImageFile(filePath);
                }
            }
            Program.UpdateCreditsUsage(cloudinary);
        }
        #endregion

        #region Truyện tranh netfullTruyen
        bool run_netfullTruyen = false;
        private void ChuongTruyenTranh_netfullTruyen_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked || run_netfullTruyen)
            {
                MessageBox.Show("if (checkBox1.Checked || run_netfullTruyen)");
                return;
            }
            string condition = this.richTextBox1.Text;
            this.richTextBox1.Text = "";
            this.lblQuery.Text = condition;
            Task.Factory.StartNew(async () =>
            {
                while (!checkBox1.Checked)
                {
                    run_netfullTruyen = true;
                    await StartUploadAsync_ChuongTruyenTranh_netfullTruyen(condition);
                    run_netfullTruyen = false;
                }
            });
        }
        private async Task StartUploadAsync_ChuongTruyenTranh_netfullTruyen(string condition)
        {
            var dtTruyen = Program.ExcecuteDataTable(GetQueryTruyen(condition));
            if (dtTruyen.Rows.Count == 0)
            {
                MessageBox.Show("Hết truyện");
                this.Invoke(new Action(() =>
                {
                    this.checkBox1.Checked = true;
                }));
                return;
            }
            int idTruyen = (int)dtTruyen.Rows[0]["ID"];
            var tableChuong = Program.GetTableName_dsChuong(idTruyen);
            this.Invoke(new Action(() =>
            {
                this.Text = string.Format("[{0}] - {1}", idTruyen, dtTruyen.Rows[0]["title"]);
            }));
            var dtChuong = Program.ExcecuteDataTable("select  * from " + tableChuong + " where refID = " + idTruyen + " and arr_ggd_id is not null and done is null");
            if (dtChuong.Rows.Count == 0)
            {
                Program.ExcecuteDataTable("UPDATE tblTruyen set percentCloudinary = 100 where ID = " + idTruyen);
                return;
            }
            int idCloudinary = 0;
            if (dtTruyen.Rows[0]["idCloudinary"] != DBNull.Value)
            {
                idCloudinary = Convert.ToInt32(dtTruyen.Rows[0]["idCloudinary"]);
            }

            var cloudinary = Program.GetCloudinary(ref idCloudinary);
            if (cloudinary == null)
            {
                UpdateExeption(new Exception("Find not found \"cloudinary account\""));
                return;
            }
            Program.ExcecuteNoneQuery("UPDATE tblTruyen set idCloudinary = @idCloudinary where ID = @ID"
                        , new Dictionary<string, object>() { { "@ID", idTruyen }, { "@idCloudinary", idCloudinary } });
            foreach (DataRow dr in dtChuong.Rows)
            {
                string filePath = string.Empty;
                try
                {
                    List<string> arr_ggd_id = JsonConvert.DeserializeObject<List<string>>((string)dr["arr_ggd_id"]);


                    this.Invoke(new Action(() =>
                    {
                        progressBar.Maximum = arr_ggd_id.Count;
                        progressBar.Minimum = 0;
                        progressBar.Value = 0;
                    }));

                    List<string> new_arr_ggd_id = new List<string>();
                    if (!arr_ggd_id.Any()) continue;
                    string folder = idTruyen + "/" + dr["ID"];
                    foreach (var urlHinhAnh_image_path in arr_ggd_id)
                    {
                        try
                        {
                            string uriChuong = dr["uriChuong"].ToString();
                            UpdateProgressBar(urlHinhAnh_image_path);
                            filePath = Application.StartupPath + "\\tmp_photo\\" + uriChuong + ".png";
                            var rsDownloadImage = Program.DownloadImage(urlHinhAnh_image_path, filePath);
                            if (!rsDownloadImage || !System.IO.File.Exists(filePath))
                            {
                                new_arr_ggd_id.Add(urlHinhAnh_image_path);
                                continue;
                            }
                            if (new System.IO.FileInfo(filePath).Length == 0)
                            {
                                continue;
                            }
                            this.Invoke(new Action(() =>
                            {
                                try
                                {
                                    this.pictureBox1.Image = new Bitmap(filePath);
                                }
                                catch (Exception ex)
                                {
                                    UpdateExeption(ex);
                                }
                            }));
                            if (!System.IO.File.Exists(filePath)) continue;
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(filePath),
                                Folder = folder,
                                AssetFolder = folder,
                                Overwrite = true,

                            };

                            ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                            var cloudinary_url = uploadResult?.SecureUrl?.AbsoluteUri;
                            if (!string.IsNullOrEmpty(cloudinary_url))
                            {
                                new_arr_ggd_id.Add(cloudinary_url);
                            }
                        }
                        catch (Exception ex)
                        {
                            UpdateExeption(ex);
                        }
                        DeleteImageFile(filePath);
                    }

                    Program.ExcecuteNoneQuery("UPDATE " + tableChuong + " set arr_ggd_id = @arr_ggd_id, done = 1 where ID = @ID "
                      , new Dictionary<string, object>() { { "@ID", dr["ID"] }, { "@arr_ggd_id", Newtonsoft.Json.JsonConvert.SerializeObject(new_arr_ggd_id) } });
                    Program.ExcecuteNoneQuery("sp_UpdatePercentCloudinary"
                     , new Dictionary<string, object>() { { "@ID", idTruyen } });
                }
                catch (Exception ex)
                {
                    UpdateExeption(ex);
                }
            }
        }

        private string GetQueryTruyen(string condition)
        {
            string queryGetTruyen = "select TOP(1) * from tblTruyen " +
                "  where idLoaiTruyen = 2 AND isnull(percentCloudinary,0) != 100.0 " +
                " AND  " + condition +
                "  order by luotXem";
            return queryGetTruyen;

        }

        private void DeleteImageFile(string filePath)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    if (this.pictureBox1.Image != null)
                    {
                        this.pictureBox1.Image.Dispose();
                    }
                    this.pictureBox1.Image = null;
                }));
            }
            catch (Exception ex)
            {
                UpdateExeption(ex);
            }
            Program.DeleteFile(filePath);
        }
        #endregion

        #region CreateJsonDsChuong
        private void btnDsChuong_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked || run_netfullTruyen)
            {
                MessageBox.Show("if (checkBox1.Checked || run_netfullTruyen)");
                return;
            }
            string condition = this.richTextBox1.Text;
            this.richTextBox1.Text = "";
            this.lblQuery.Text = condition;
            Task.Factory.StartNew(async () =>
            {
                while (!checkBox1.Checked)
                {
                    run_netfullTruyen = true;
                    await CreateJsonDsChuong(condition);
                    run_netfullTruyen = false;
                }
            });
        }

        private async Task CreateJsonDsChuong(string condition)
        {
            int idCloudinary = 11;
            var cloudinary = Program.GetCloudinary(ref idCloudinary);
            if (cloudinary == null)
            {
                UpdateExeption(new Exception("Find not found \"cloudinary account\""));
                this.Invoke(new Action(() =>
                {
                    this.checkBox1.Checked = true;
                }));
                return;
            }
            var dtTruyen = Program.ExcecuteDataTable("SELECT TOP(1) * FROM tblTruyen where idLoaiTruyen = 1 and coChuong is null and  dsChuong_url is null and " + condition + " order by luotXem");
            if (dtTruyen.Rows.Count == 0)
            {
                MessageBox.Show("hết Truyện");
                this.Invoke(new Action(() =>
                {
                    this.checkBox1.Checked = true;
                }));
                return;
            }
            this.Invoke(new Action(() =>
            {
                progressBar.Maximum = dtTruyen.Rows.Count;
                progressBar.Minimum = 0;
                progressBar.Value = 0;
            }));

            foreach (DataRow dr in dtTruyen.Rows)
            {
                string filePath_image = "";
                try
                {

                    int idTruyen = (int)dr["ID"];

                    filePath_image = Application.StartupPath + "\\tmp_photo\\" + idTruyen + ".png";
                    var rsDownloadImage = Program.DownloadImage(dr["urlHinhAnh_image_path"]?.ToString(), filePath_image);
                    UpdateProgressBar((string)dr["title"]);
                    if (System.IO.File.Exists(filePath_image))
                    {
                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                                this.pictureBox1.Image = new Bitmap(filePath_image);
                            }
                            catch (Exception ex)
                            {
                                UpdateExeption(ex);
                            }
                        }));
                    }

                    string folder = Program.GetTableName_dsChuong(idTruyen, tbl: "f");
                    string tableChuong = Program.GetTableName_dsChuong(idTruyen);
                    var dtChuong = Program.ExcecuteDataTable("SELECT nullif(title,'') title,chuong,ggd_id, nullif(uriChuong,'') uriChuong FROM " + tableChuong + " where refID = " + idTruyen + " and ggd_id is not null");
                    if (dtChuong.Rows.Count == 0)
                    {
                        Program.ExcecuteNoneQuery("UPDATE tblTruyen set coChuong = 0 where ID = " + idTruyen);
                        continue;
                    }
                    if (dr["dsChuong_key"] != DBNull.Value)
                    {
                        cloudinary.DeleteRelatedResourcesByAssetIds(new DeleteRelatedResourcesByAssetIdsParams() { AssetId = (string)dr["dsChuong_key"] });
                    }
                    RawUploadResult uploadResult = null;
                    using (var stream = Program.GenerateStreamFromString(JsonConvert.SerializeObject(dtChuong.ToDictionaryDataTable())))
                    {
                        var uploadParams = new RawUploadParams()
                        {
                            File = new FileDescription(idTruyen + ".json", stream),
                            Folder = folder,
                            AssetFolder = folder,
                            Overwrite = true,
                            UseFilename = true,
                        };
                        uploadResult = await cloudinary.UploadLargeAsync(uploadParams);
                        this.Invoke(new Action(() =>
                        {
                            this.richTextBox1.Text = uploadResult?.JsonObj?.ToString();
                        }));
                    }
                    if (uploadResult == null)
                    {
                        continue;
                    }
                    string dsChuong_url = uploadResult?.SecureUrl?.AbsoluteUri;
                    string dsChuong_key = uploadResult.AssetId;
                    int dsChuongidCloudinary = idCloudinary;
                    if (string.IsNullOrEmpty(dsChuong_url) || string.IsNullOrEmpty(dsChuong_key))
                    {
                        continue;
                    }
                    Program.ExcecuteNoneQuery("UPDATE tblTruyen set coChuong = 1 ,dsChuong_url = @dsChuong_url, dsChuong_key = @dsChuong_key, dsChuongidCloudinary = @dsChuongidCloudinary where ID = @ID", new Dictionary<string, object>
                {
                    { "@ID",idTruyen},
                    { "@dsChuong_url",dsChuong_url},
                    { "@dsChuong_key",dsChuong_key},
                    { "@dsChuongidCloudinary",dsChuongidCloudinary},
                });
                }
                catch (Exception ex)
                {
                    UpdateExeption(ex);
                }
                finally
                {
                    DeleteImageFile(filePath_image);
                }
            }
        }

        #endregion


    }
}
