using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Http;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Update_Manager
{
    public partial class UpdateManager : Form
    {
        public UpdateManager()
        {
            InitializeComponent();
        }

        private void UpdateManager_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            // When the user clicks the "Install" button, mark the corresponding file as "installing"
            if (e.ColumnIndex == dataGridView1.Columns["InstallColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell cell = dataGridView1.Rows[e.RowIndex].Cells["InstallColumn"] as DataGridViewCheckBoxCell;
                if ( cell != null)
                {
                    if (cell.Value == cell.TrueValue)
                    {
                        cell.Value = cell.FalseValue;
                    }
                    else
                    {
                        cell.Value = cell.TrueValue;
                    }
                }
                else
                {
                    return;
                }
              
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {      
                // Send a request to the URL of your website and retrieve the HTML content of the page
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://www.pythonanywhere.com/user/codefilter/files/home/codefilter/Inventory/Update_Manager_APP.msi");
                string content = await response.Content.ReadAsStringAsync();

                // Use HtmlAgilityPack to extract the list of files available for download
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);

                List<InventoryFile> fileList = new List<InventoryFile>();
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    string href = link.Attributes["href"].Value;
                    if (href.EndsWith(".msi"))
                    {
                        fileList.Add(new InventoryFile() { FileName = href, Install = false });
                    }
                }

                // Populate the DataGridView control with the list of files
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = fileList;

            }
            catch( Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }
        private void btn_install_Click(object sender, EventArgs e)
        {

        }
    }
    public class InventoryFile
    {
        public string FileName { get; set; }
        public bool Install { get; set; }
    }
}
