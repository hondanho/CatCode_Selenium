namespace CatCode_Selenium
{
    partial class frmAddLogo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddLogo));
            this.picResource = new System.Windows.Forms.PictureBox();
            this.pnResize = new System.Windows.Forms.Panel();
            this.picResize = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.picKQ = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picResource)).BeginInit();
            this.pnResize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKQ)).BeginInit();
            this.SuspendLayout();
            // 
            // picResource
            // 
            this.picResource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picResource.Image = ((System.Drawing.Image)(resources.GetObject("picResource.Image")));
            this.picResource.Location = new System.Drawing.Point(0, 0);
            this.picResource.Name = "picResource";
            this.picResource.Size = new System.Drawing.Size(413, 370);
            this.picResource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picResource.TabIndex = 1;
            this.picResource.TabStop = false;
            // 
            // pnResize
            // 
            this.pnResize.BackColor = System.Drawing.Color.Transparent;
            this.pnResize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnResize.Controls.Add(this.picResize);
            this.pnResize.Controls.Add(this.picLogo);
            this.pnResize.Location = new System.Drawing.Point(116, 155);
            this.pnResize.Name = "pnResize";
            this.pnResize.Size = new System.Drawing.Size(186, 42);
            this.pnResize.TabIndex = 6;
            // 
            // picResize
            // 
            this.picResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picResize.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.picResize.Image = ((System.Drawing.Image)(resources.GetObject("picResize.Image")));
            this.picResize.Location = new System.Drawing.Point(168, 25);
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
            this.picLogo.Size = new System.Drawing.Size(184, 40);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 3;
            this.picLogo.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Blue;
            this.btnSave.Location = new System.Drawing.Point(686, 621);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 30);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save (Ctrs+S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(830, 621);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close (ESC)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Olive;
            this.label2.Location = new System.Drawing.Point(0, 594);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Nguồn";
            // 
            // picKQ
            // 
            this.picKQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picKQ.Location = new System.Drawing.Point(774, 12);
            this.picKQ.Name = "picKQ";
            this.picKQ.Size = new System.Drawing.Size(184, 240);
            this.picKQ.TabIndex = 21;
            this.picKQ.TabStop = false;
            // 
            // frmAddLogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 653);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnResize);
            this.Controls.Add(this.picResource);
            this.Controls.Add(this.picKQ);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddLogo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Logo";
            ((System.ComponentModel.ISupportInitialize)(this.picResource)).EndInit();
            this.pnResize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picResize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picKQ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picResource;
        public System.Windows.Forms.Panel pnResize;
        public System.Windows.Forms.PictureBox picLogo;
        public System.Windows.Forms.PictureBox picResize;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picKQ;
    }
}