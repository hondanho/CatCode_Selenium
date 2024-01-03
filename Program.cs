using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CatCode_Selenium
{
    public class AppConfig
    {
        public string SQL_CONNECTION { get; set; }
        public string CREDENTIALS_GG_Drive { get; set; }
        public string OUTPUT_DataTruyen { get; set; }
        public int RUN_INDEX { get; set; }
        public int TASK_RUN { get; set; }
        public int MAX_TASK_RUNNING { get; set; }
        
        public string publicFolderId { get; set; }
        public string googleDriveApiKey { get; set; }
        
    }

    internal static class Program
    {
        private static string CONNECTION_STRING { get { return configuration.SQL_CONNECTION; } }
        public static AppConfig configuration = new AppConfig();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form frm = new Form();
            configuration = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText("appconfig.json"));
            int index = configuration.RUN_INDEX;
            switch (index)
            {
                case 0:
                    frm = new Form();
                    frm.Shown += (s, e) =>
                    {
                        var watt = new Wattpad() { Dock = DockStyle.Fill, Width = 1000, Height = 1000 };
                        frm.Controls.Add(watt);
                        watt.Wattpad_10TrangMoiCapNhat_Text();
                    };
                    break;
                case 1:
                    frm = new UploadCloudinary_8xLand()
                    {

                        Text = "UploadCloudinary_8xLand"
                    };
                    break;
                case 2:
                    frm = new AutoTicket(); break;
                case 3:
                    frm = new frmManager(); break;
                case 4:
                    RunScriptLineByLine();
                    break;
                case 5: break;
                case 6: break;
            }


            Application.Run(frm);
        }

        private static void RunScriptLineByLine()
        {
            string line;
            System.IO.StreamReader file =
               new System.IO.StreamReader("C:\\Working\\wikinovel-data.sql");
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(line) || line.Contains("IDENTITY_INSERT")) continue;
                    ExcecuteNoneQuery(line);
                    Console.WriteLine(line);
                    Console.Clear();
                }
                catch (Exception)
                {

                }
            }

            file.Close();
        }

        private static void SycImg2()
        {
            string path = @"C:\Users\linhb\Downloads\img2\img2";
            var lstPathImages = Program.ExcecuteDataTable("select urlHinhAnh_image_path from tbltruyen where urlHinhAnh_image_path is not null")
                .AsEnumerable().Select(dr => dr["urlHinhAnh_image_path"].ToString()).ToList();
            var direc = new DirectoryInfo(path);
            var img2s = direc.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in img2s)
            {
                var fullPath = lstPathImages.Where(f => f.EndsWith(file.Name)).FirstOrDefault();
                if (string.IsNullOrEmpty(fullPath))
                {
                    File.Delete(file.FullName);
                }
                else
                {
                    if (!fullPath.Contains("truyenfree.net"))
                    {
                        Program.ExcecuteNoneQuery("update tbltruyen set urlHinhAnh_image_path = @newPath  where urlHinhAnh_image_path  = @oldPath"
                            , new Dictionary<string, object>() { { "@newPath", "https://truyenfree.net/img2/" + file.Name }, { "@oldPath", fullPath } });
                    }
                }
            }
        }

        static public HtmlAgilityPack.HtmlDocument LoadDocument(string urlRun, int maxloop = 15)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                web.UsingCache = false;
                web.BrowserTimeout = TimeSpan.FromMinutes(3);
                web.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36";
                web.CaptureRedirect = true;
                var doc = web.Load(urlRun);
                try
                {
                    if (doc.ParsedText.Contains("<title>301 Moved Permanently</title>"))
                    {
                        var lstItem = doc.DocumentNode.SelectNodes(".//a[@href]");
                        if (lstItem != null && lstItem.Any())
                        {
                            var moveURL = lstItem[0].GetAttributeValue("href", string.Empty);
                            return LoadDocument(moveURL, maxloop);
                        }
                    }
                }
                catch
                {

                }

                return doc;
            }
            catch (Exception ex)
            {
                maxloop--;
                if (maxloop <= 0)
                {
                    throw ex;
                }
                return LoadDocument(urlRun, maxloop);
            }
        }

        static public DataTable ExcecuteDataTable(string queryString, Dictionary<string, object> dictParam = null)
        {
            DataTable dt = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandTimeout = 240;
                    if (!queryString.Contains(" ")) command.CommandType = CommandType.StoredProcedure;
                    if (dictParam != null)
                    {
                        foreach (var item in dictParam)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        dt = new DataTable();
                        da.Fill(dt);
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("[ERROR] BaseModel.ExcecuteDataTable Exception: {0 }", ex.Message));
                throw;
            }
            return dt;
        }

        static public void ExcecuteNoneQuery(string queryString, Dictionary<string, object> dictParam = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandTimeout = 240;
                    if (!queryString.Contains(" ")) command.CommandType = CommandType.StoredProcedure;
                    if (dictParam != null)
                    {
                        foreach (var item in dictParam)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("[ERROR] BaseModel.ExcecuteDataTable Exception: {0 }", ex.Message));
                throw;
            }
        }

        static public void MergeMp3File(string outputFile, List<string> sourceFiles)
        {
            try
            {
                using (var fs = File.Create(outputFile))
                {
                    foreach (var file in sourceFiles)
                    {
                        byte[] buffer = File.ReadAllBytes(file);
                        fs.Write(buffer, 0, buffer.Length);
                    }
                    fs.Flush();
                }

            }
            finally
            {
            }

        }

        static public string GetTableName_dsChuong(int idTruyen, string tbl = "tblTruyen_dsChuong_")
        {
            int tableIndex = idTruyen / 1000;
            return string.Format(tbl + "{0}", tableIndex);
        }

        static public bool DownloadImage(string imageUrl, string downloadPath)
        {
            DeleteFile(downloadPath);
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(imageUrl, downloadPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);
            }
            catch
            {
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static List<Dictionary<string, object>> ToDictionaryDataTable(this DataTable dtIn, bool removeDBNull = true, int parentID = -1, string childField = "")
        {
            if (dtIn == null || dtIn.Rows.Count == 0)
            {
                return new List<Dictionary<string, object>>();
            }

            DataTable dt = dtIn;
            if (removeDBNull)
            {
                if (parentID > 0)
                {
                    childField = (string.IsNullOrEmpty(childField) ? "refID" : childField);
                    if (!dt.Columns.Contains(childField))
                    {
                        return null;
                    }

                    return (from dr in dt.AsEnumerable()
                            where dr[childField] != DBNull.Value && int.Parse(dr[childField].ToString()) == parentID
                            select (from DataColumn c in dt.Columns
                                    select c.ColumnName into cName
                                    where dr[cName] != DBNull.Value && !string.IsNullOrEmpty(dr[cName].ToString())
                                    select cName).ToDictionary((string cName) => cName, (string cName) => dr[cName])).ToList();
                }

                return (from dr in dt.AsEnumerable()
                        select (from DataColumn c in dt.Columns
                                select c.ColumnName into cName
                                where dr[cName] != DBNull.Value
                                select cName).ToDictionary((string cName) => cName, (string cName) => dr[cName])).ToList();
            }

            return (from dr in dt.AsEnumerable()
                    select (from DataColumn c in dt.Columns
                            select c.ColumnName).ToDictionary((string cName) => cName, (string cName) => dr[cName])).ToList();
        }

        public static Cloudinary GetCloudinary(ref int idCloudinary, int creditsLimit = 2, bool forceLimit = false)
        {
            try
            {
                var dtCloudinary = Program.ExcecuteDataTable("select  * from tblCloudinaryAccount");
                foreach (DataRow dr in dtCloudinary.Rows)
                {
                    if (idCloudinary > 0 && idCloudinary != (int)dr["ID"])
                    {
                        continue;
                    }
                    UpdateCreditsUsage(new Cloudinary(new Account(
                        (string)dr["cloud"],
                        (string)dr["apiKey"],
                        (string)dr["apiSecret"]
                    )));
                }

                DataTable dtAccount;
                if (idCloudinary > 0)
                {
                    if (forceLimit)
                    {
                        dtAccount = Program.ExcecuteDataTable("select TOP(1) * from tblCloudinaryAccount where id = @id AND isnull(creditsUsage,0) < @creditsLimit", new Dictionary<string, object> { { "@id", idCloudinary }, { "@creditsLimit", creditsLimit } });
                    }
                    else
                    {
                        dtAccount = Program.ExcecuteDataTable("select TOP(1) * from tblCloudinaryAccount where id = @id ", new Dictionary<string, object> { { "@id", idCloudinary } });
                    }
                }
                else
                {
                    dtAccount = Program.ExcecuteDataTable("select TOP(1) * from tblCloudinaryAccount where statusData = 3 and isnull(creditsUsage,0) < @creditsLimit order by isnull(modifyDate,getdate()) ", new Dictionary<string, object> { { "@creditsLimit", creditsLimit } });
                }

                var rowAccount = dtAccount.Rows[0];
                idCloudinary = Convert.ToInt32(dtAccount.Rows[0]["ID"]);

                Program.ExcecuteNoneQuery("update tblCloudinaryAccount set modifyDate = GETDATE() where ID = " + idCloudinary);

                var cloudinary = new Cloudinary(new Account(
                  (string)rowAccount["cloud"],
                 (string)rowAccount["apiKey"],
                  (string)rowAccount["apiSecret"]
                  ));
                var CreditsUsage = Convert.ToDouble(rowAccount["creditsUsage"] != DBNull.Value ? rowAccount["creditsUsage"] : 0);
                return cloudinary;
            }
            catch
            {
                return null;
            }

        }

        public static void UpdateCreditsUsage(Cloudinary cloudinary)
        {
            try
            {
                UsageResult usage = cloudinary.GetUsage();
                // Available upload size is the remaining bandwidth in bytes 
                Program.ExcecuteNoneQuery("UPDATE tblCloudinaryAccount" +
                    " set creditsUsage = @creditsUsage" +
                    " where cloud = @cloud"
                             , new Dictionary<string, object>() {
                              { "@cloud", cloudinary.Api.Account.Cloud },
                             { "@creditsUsage", Math.Round(usage.Storage.CreditsUsage , 2)},
                             });
            }
            catch (Exception ex)
            {

            }

        }
    }
}
