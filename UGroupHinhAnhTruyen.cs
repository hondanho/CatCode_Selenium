using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatCode_Selenium
{
    public partial class UGroupHinhAnhTruyen : UserControl
    {
        public UGroupHinhAnhTruyen()
        {
            InitializeComponent();
            this.group.Text = "Truyện Free";

            ResizeEvent();
            MoveControlEvent();
            this.pnResize.Visible = false;

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


        public UGroupHinhAnhTruyen(DataRow dr, string urlHinhAnhSelected) : this()
        {
            this.group.Text = dr["title"].ToString();
            this.pic.Image = null;
            UrlHinhAnh = string.Empty;
            try
            {
                UrlHinhAnh = dr["urlHinhAnh"].ToString();
                var request = WebRequest.Create(UrlHinhAnh);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    this.pic.Image = Bitmap.FromStream(stream);
                }
            }
            catch
            {
            }

            //Selected = UrlHinhAnh == urlHinhAnhSelected;
            Selected = false;
        }
        public string UrlHinhAnh { get; set; }
        public bool Selected
        {
            get
            {
                return this.group.BackColor == System.Drawing.Color.Red;
            }
            set
            {
                if (value)
                {
                    this.group.BackColor = System.Drawing.Color.Red;
                    this.group.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    this.group.BackColor = System.Drawing.Color.WhiteSmoke;
                    this.group.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

    }
}
