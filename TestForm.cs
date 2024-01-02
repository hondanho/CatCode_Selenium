using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CatCode_Selenium
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
            this.Load("https://truyenhdt.com/truyen/trong-bung-deu-tran-day-tinh-hoa-cua-dam-cong/chap/9430570-chuong-1/");
        }
        public void Load(string url)
        {
            this.webBrowser1.Url = new Uri(url);
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var htmlText = this.webBrowser1.Document.Body.OuterHtml;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlText);
            var tenTacGia = doc.DocumentNode.SelectSingleNode("//div[@class='reading']");

            //var start = htmlText.IndexOf("<div class=\"reading\">") ;
            //var end = htmlText.IndexOf("<div class=\"row text-center\">") ;
            //var content = htmlText.Substring(start, end);

        }

    }
}
