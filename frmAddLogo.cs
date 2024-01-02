using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatCode_Selenium
{
    public partial class frmAddLogo : Form
    {
        public frmAddLogo()
        {
            InitializeComponent();
            ResizeEvent();
            MoveControlEvent();
            this.btnClose.Focus();
            this.DialogResult = DialogResult.None;

        }
        public string UrlHinhAnh { set;get;}
        public int x { set;get;}
        public int y { set;get;}
        public int w { set;get;}
        public int h { set; get; }

        public string OutputPath { set;get;}
        
        public frmAddLogo(DataRow dr):this()
        {
            this.picResource.Image = null;
            try
            {
                 UrlHinhAnh = dr["urlHinhAnh"].ToString();
                var request = WebRequest.Create(UrlHinhAnh);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    var rsPic = Bitmap.FromStream(stream);
                    this.picResource.Width = rsPic.Width;
                    this.picResource.Height = rsPic.Height;

                    this.picResource.Image = rsPic;
                    this.picKQ.Image = rsPic;


                    this.Width = rsPic.Width * 2 + 30;
                    this.Height = rsPic.Height + 100;

                    this.picKQ.Width = rsPic.Width ;
                    this.picKQ.Height = rsPic.Height ;
                    this.picKQ.Location = new Point(this.picKQ.Width + 10, 0 );

                }
            }
            catch 
            {
            }
        }
        #region Move Control
        private Point MouseDownLocation;
        private void MoveControlEvent()
        {
            this.picLogo.MouseMove += PicLogo_MouseMove;
            this.picLogo.MouseUp += PicLogo_MouseUp;
            this.picLogo.MouseDown += PicLogo_MouseDown;
        }

        private void PicLogo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
                this.Cursor = Cursors.NoMove2D;

            }
        }

        private void PicLogo_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            UpdateKetQua();

        }

        private void PicLogo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.pnResize.Left = e.X + this.pnResize.Left - MouseDownLocation.X;
                this.pnResize.Top = e.Y + this.pnResize.Top - MouseDownLocation.Y;
            }
        }


        #endregion

        #region Resize
        private void ResizeEvent()
        {
            this.picResize.MouseMove += picResize_MouseMove;
            this.picResize.MouseUp += picResize_MouseUp;
            this.picResize.MouseDown += picResize_MouseDown;

        }
        bool mouseClicked = false;
        private void picResize_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClicked = true;
        }

        private void picResize_MouseUp(object sender, MouseEventArgs e)
        { 
            mouseClicked = false;
            UpdateKetQua(); 
        }

        private void picResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseClicked)
            {
                this.pnResize.Height = picResize.Top + e.Y;
                this.pnResize.Width = picResize.Left + e.X;

            }

        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public Stream ToStream( Image image )
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;
            return stream;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = "img2";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                string fileName = System.IO.Path.GetFileName(this.UrlHinhAnh);
                OutputPath = folder + "/" + fileName;
                picKQ.Image.Save(OutputPath);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateKetQua()
        {
            try
            {
                System.Drawing.Image logo = picLogo.Image;

                x = pnResize.Location.X;
                y = pnResize.Location.Y;

                w = picLogo.Width;
                h = picLogo.Height;


                var image = Image.FromStream(ToStream(picResource.Image));

                using (Graphics grfx = Graphics.FromImage(image))
                {
                    grfx.DrawImage(logo, x, y, w, h);
                }
                picKQ.Image = Image.FromStream(ToStream(image));
                image.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.S):
                this.btnSave.PerformClick();
                    return true;
                case (Keys.Escape):
                this.btnClose.PerformClick();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
