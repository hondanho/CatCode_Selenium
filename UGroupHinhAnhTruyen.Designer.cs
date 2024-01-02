namespace CatCode_Selenium
{
    partial class UGroupHinhAnhTruyen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UGroupHinhAnhTruyen));
            this.group = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnResize = new System.Windows.Forms.Panel();
            this.picResize = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.pic = new System.Windows.Forms.PictureBox();
            this.group.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnResize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // group
            // 
            this.group.BackColor = System.Drawing.Color.Red;
            this.group.Controls.Add(this.panel2);
            this.group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.group.ForeColor = System.Drawing.Color.White;
            this.group.Location = new System.Drawing.Point(0, 0);
            this.group.Name = "group";
            this.group.Padding = new System.Windows.Forms.Padding(10);
            this.group.Size = new System.Drawing.Size(795, 860);
            this.group.TabIndex = 4;
            this.group.TabStop = false;
            this.group.Text = "Truyện Free";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.pnResize);
            this.panel2.Controls.Add(this.pic);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(10, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(775, 821);
            this.panel2.TabIndex = 6;
            // 
            // pnResize
            // 
            this.pnResize.BackColor = System.Drawing.Color.Transparent;
            this.pnResize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnResize.Controls.Add(this.picResize);
            this.pnResize.Controls.Add(this.picLogo);
            this.pnResize.Location = new System.Drawing.Point(17, 19);
            this.pnResize.Name = "pnResize";
            this.pnResize.Size = new System.Drawing.Size(281, 86);
            this.pnResize.TabIndex = 5;
            // 
            // picResize
            // 
            this.picResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picResize.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.picResize.Image = ((System.Drawing.Image)(resources.GetObject("picResize.Image")));
            this.picResize.Location = new System.Drawing.Point(263, 69);
            this.picResize.Name = "picResize";
            this.picResize.Size = new System.Drawing.Size(16, 15);
            this.picResize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picResize.TabIndex = 4;
            this.picResize.TabStop = false;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(279, 84);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 3;
            this.picLogo.TabStop = false;
            // 
            // pic
            // 
            this.pic.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic.Image = ((System.Drawing.Image)(resources.GetObject("pic.Image")));
            this.pic.Location = new System.Drawing.Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(775, 821);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pic.TabIndex = 2;
            this.pic.TabStop = false;
            // 
            // UGroupHinhAnhTruyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.group);
            this.Name = "UGroupHinhAnhTruyen";
            this.Size = new System.Drawing.Size(795, 860);
            this.group.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnResize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picResize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox group;
        public System.Windows.Forms.PictureBox pic;
        public System.Windows.Forms.PictureBox picLogo;
        public System.Windows.Forms.PictureBox picResize;
        public System.Windows.Forms.Panel pnResize;
        public System.Windows.Forms.Panel panel2;
    }
}
