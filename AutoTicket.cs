using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Linq;
using System.Data.Linq;

namespace CatCode_Selenium
{
    public partial class AutoTicket : Form
    {
        //csjhrusin4ofmiplw4okjo5igp7maan3ocs4waumlb6rrfhecjqa
        public AutoTicket()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            //this.dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            this.txtToken.Text = Properties.Settings.Default.PAT;
            //this.richPayloadQuery.Text = Properties.Settings.Default.PayloadQuery;
            Nofiycation(null);

        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell is not in the header row and contains non-empty data
            var dtSource = this.dataGridView1.DataSource as DataTable;
            if (dtSource == null || dtSource.Rows.Count == 0 || e.RowIndex < 0 || e.RowIndex >= dtSource.Rows.Count) return;
            var dr = dtSource.Rows[e.RowIndex];
            if (dr["System.WorkItemType"].ToString() == "Bug" &&
                dr["System.State"].ToString() == "Queue" &&
                dr["System.AssignedTo"].ToString().StartsWith("Tommy")
                )
            {
                // Set the background color of the cell to your desired color
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow; // Replace with your desired color
                return;
            }
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the desired column (e.g., column index 1)
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0) // Replace 1 with the actual column index
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Check if the cell's content is a hyperlink
                if (cell is DataGridViewLinkCell linkCell)
                {
                    // Extract the hyperlink text
                    string hyperlinkValue = linkCell.Value?.ToString();
                    string hyperlinkText = string.Format("https://dev.azure.com/DMCHosting-Auvenir-Org/AuvenirApp/_workitems/edit/{0}", hyperlinkValue);
                    // You can now handle the hyperlink, for example, by opening it in a browser
                    if (!string.IsNullOrEmpty(hyperlinkText))
                    {
                        System.Diagnostics.Process.Start(hyperlinkText);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.button1.Text == "RUN")
            {
                this.button1.Text = "STOP";
            }
            else
            {
                this.button1.Text = "RUN";
            }
            GetTicketAsync();
        }

        private void GetTicketAsync()
        {
            string payloayQuyery = this.richPayloadQuery.Text;
            string pat = this.txtToken.Text;

            Properties.Settings.Default.PAT = pat;
            Properties.Settings.Default.PayloadQuery = payloayQuyery;
            Properties.Settings.Default.Save();

            Task.Factory.StartNew(async () =>
            {
                while (this.button1.Text != "RUN")
                {
                    await GetTicket(payloayQuyery, pat);
                    await Task.Delay(int.Parse(this.txtInterval.Text) * 1000);
                }
            });
        }

        private async Task GetTicket(string paloayQuery, string pat)
        {
            // Define your Azure DevOps API URL
            string apiUrl = "https://dev.azure.com/DMCHosting-Auvenir-Org/_apis/Contribution/dataProviders/query";

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Define your request payload as an anonymous type (or use a custom class)

                // Serialize the request data to JSON
                string jsonPayload = paloayQuery;

                // Create a StringContent with JSON media type
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Set the required headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=5.1-preview.1;excludeUrls=true;enumsAsNumbers=true;msDateFormat=true;noArrayWrap=true");
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + pat); // Replace with your Azure DevOps PAT
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{pat}")));

                // Make the POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Check if the request was successful (status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    // Your JSON string
                    string jsonString = responseContent;// Paste your JSON string here

                    // Deserialize the JSON string into the RootObject
                    Root rootObject = JsonConvert.DeserializeObject<Root>(jsonString);

                    // Access the desired data
                    var rows = rootObject.data.msvssworkwebworkitemquerydataprovider.data.payload.rows;
                    var cols = rootObject.data.msvssworkwebworkitemquerydataprovider.data.payload.columns;
                    this.Invoke(new Action(() =>
                    {
                        this.dataGridView1.DataSource = GetDataSource(cols, rows);
                        this.dataGridView1.Refresh();
                    }));

                    // Now, you have the rows data in the 'rows' variable

                    // Process the response content as needed
                    Console.WriteLine(responseContent);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
        }

        private DataTable GetDataSource(List<string> cols, List<List<object>> rows)
        {
            DataTable table = new DataTable();
            foreach (string c in cols)
            {
                table.Columns.Add(c, typeof(string));
            }
            foreach (var r in rows)
            {
                var new_row = table.NewRow();
                foreach (string c in cols)
                {
                    new_row[c] = r[cols.IndexOf(c)];
                }
                table.Rows.Add(new_row);
            }
            table.AcceptChanges();
           var selectedItemsVersion =  this.checkedListBox1.CheckedItems.Cast<string>().ToList();
            table = table.AsEnumerable()
                .Where(dr=> selectedItemsVersion.Contains((string)dr["Custom.DefectIdentifiedInVersion"]))
                .OrderBy(dr=> (string)dr["Custom.DefectIdentifiedInVersion"]).CopyToDataTable();

            Nofiycation(table);
            return table;
        }

        private void Nofiycation(DataTable table)
        {
            //new ToastContentBuilder()
            //.AddArgument("action", "viewConversation")
            //.AddArgument("conversationId", 9813)
            //.AddText("Andrew sent you a picture")
            //.AddText("Check this out, The Enchantments in Washington!")
            //.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater

        }

        private void ckbAllawTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ckbAllawTop.Checked;
        }
    }

}
