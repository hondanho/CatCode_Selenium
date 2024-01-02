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
    public partial class UploadCloudinary_8xLand : Form
    {
        public UploadCloudinary_8xLand()
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
            var dtSetting = Program.ExcecuteDataTable("select " +
                "TABLE_NAME, COLUMN_NAME " +
                "from INFORMATION_SCHEMA.COLUMNS  " +
                "where  DATA_TYPE in ('varbinary') ");
            if (dtSetting.Rows.Count == 0)
            {
                MessageBox.Show("Hết Ảnh bìa cần upload");
                Application.Exit();
            }

            var cloudinary = new Cloudinary(new Account(
                  "dguyovu8n",
                 "233623837685912",
                  "51lcOVGlmOgivL2aiBJYSREEeto"
                  ));
            foreach (DataRow dr in dtSetting.Rows)
            {
                string TABLE_NAME = dr["TABLE_NAME"].ToString();
                string COLUMN_NAME_ImagePath = dr["COLUMN_NAME"].ToString() + "_image_path";
                string COLUMN_NAME_SmallImagePath = dr["COLUMN_NAME"].ToString() + "_small_path";
                try
                {
                    var dtData = Program.ExcecuteDataTable("select * from " + TABLE_NAME + " where isnull(" + COLUMN_NAME_ImagePath + ",'') != '' and " + COLUMN_NAME_ImagePath + " not like '%cloudinary%'");
                    this.Invoke(new Action(() =>
                    {
                        progressBar.Maximum = dtData.Rows.Count;
                        progressBar.Minimum = 0;
                        progressBar.Value = 0;
                    }));
                    foreach (DataRow drUpload in dtData.Rows)
                    {
                        try
                        {
                            UpdateProgressBar(TABLE_NAME + "." + COLUMN_NAME_ImagePath);
                            string filePath_big = Application.StartupPath + "/" + drUpload[COLUMN_NAME_ImagePath].ToString();
                            string filePath_small = Application.StartupPath + "/" + drUpload[COLUMN_NAME_SmallImagePath].ToString();

                            string cloudinary_url_big = await UploadImages(cloudinary, filePath_big, drUpload[COLUMN_NAME_ImagePath].ToString());
                            this.DeleteImageFile(filePath_big);
                            string cloudinary_url_small = await UploadImages(cloudinary, filePath_small, drUpload[COLUMN_NAME_SmallImagePath].ToString());
                            this.DeleteImageFile(filePath_small);

                            Program.ExcecuteNoneQuery("UPDATE " + TABLE_NAME + " set " + COLUMN_NAME_ImagePath + " = @big," + COLUMN_NAME_SmallImagePath + " = @small where ID = @ID"
                               , new Dictionary<string, object>() { { "@ID", drUpload["ID"] }, { "@big", cloudinary_url_big }, { "@small", cloudinary_url_small } });

                            UpdateExeption(new Exception(DateTime.Now.ToString() + " - Upload success: " + drUpload["ID"]));
                        }
                        catch (Exception ex)
                        {
                            UpdateExeption(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateExeption(ex);
                }
                finally
                {
                }
            }
        }

        private async Task<string> UploadImages(Cloudinary cloudinary, string filePath, string defaultVal)
        {
            try
            {

                bool isImage = filePath.EndsWith(".png") || filePath.EndsWith(".jpg") || filePath.EndsWith(".jepg");
                if (!isImage)
                {
                    return defaultVal;
                }
                if (!System.IO.File.Exists(filePath) || new System.IO.FileInfo(filePath).Length == 0)
                {
                    return defaultVal;
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
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    Overwrite = true,
                };
                ImageUploadResult uploadResult = await cloudinary.UploadAsync(uploadParams);
                var cloudinary_url = uploadResult?.SecureUrl?.AbsoluteUri;
                return cloudinary_url;
            }
            catch
            {

                return defaultVal;
            }
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
    }
}
