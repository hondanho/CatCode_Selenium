namespace CatCode_Selenium
{
    partial class frmManager
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.webToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFullTruyen = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyen_GetChuongTruyen = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyen_GetHinhAnhTruyen = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyen_GetTruyenMoiCaiNhat = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyen_ThongTinThieu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.fullTruyen_StopAll = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyen_RunAll = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyen_mnCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.sSTruyenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dTruyenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topTruyenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wattpadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mn_Zip = new System.Windows.Forms.ToolStripMenuItem();
            this.txtNumberRun = new System.Windows.Forms.ToolStripTextBox();
            this.mnChonHinhAnh = new System.Windows.Forms.ToolStripMenuItem();
            this.gGDriveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullTruyenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mn_MenuFullTruyenTranh = new System.Windows.Forms.ToolStripMenuItem();
            this.speechtoTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeImageSpamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SSTruyen_HinhAnh = new System.Windows.Forms.ToolStripMenuItem();
            this.dTruyen_HinhAnh = new System.Windows.Forms.ToolStripMenuItem();
            this.topTruyen_HinhAnh = new System.Windows.Forms.ToolStripMenuItem();
            this.wattpad_HinhAnh = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.webToolStripMenuItem,
            this.mn_Zip,
            this.txtNumberRun,
            this.mnChonHinhAnh,
            this.gGDriveToolStripMenuItem,
            this.fullTruyenToolStripMenuItem,
            this.mn_MenuFullTruyenTranh,
            this.speechtoTextToolStripMenuItem,
            this.removeImageSpamToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1908, 37);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // webToolStripMenuItem
            // 
            this.webToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFullTruyen,
            this.sSTruyenToolStripMenuItem,
            this.dTruyenToolStripMenuItem,
            this.topTruyenToolStripMenuItem,
            this.wattpadToolStripMenuItem});
            this.webToolStripMenuItem.Name = "webToolStripMenuItem";
            this.webToolStripMenuItem.Size = new System.Drawing.Size(64, 31);
            this.webToolStripMenuItem.Text = "Web";
            // 
            // menuFullTruyen
            // 
            this.menuFullTruyen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullTruyen_GetChuongTruyen,
            this.fullTruyen_GetHinhAnhTruyen,
            this.fullTruyen_GetTruyenMoiCaiNhat,
            this.fullTruyen_ThongTinThieu,
            this.toolStripSeparator1,
            this.fullTruyen_StopAll,
            this.fullTruyen_RunAll,
            this.fullTruyen_mnCloseAll});
            this.menuFullTruyen.Name = "menuFullTruyen";
            this.menuFullTruyen.Size = new System.Drawing.Size(270, 34);
            this.menuFullTruyen.Text = "FullTruyen";
            // 
            // fullTruyen_GetChuongTruyen
            // 
            this.fullTruyen_GetChuongTruyen.Name = "fullTruyen_GetChuongTruyen";
            this.fullTruyen_GetChuongTruyen.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_GetChuongTruyen.Text = "1. Chương truyện";
            // 
            // fullTruyen_GetHinhAnhTruyen
            // 
            this.fullTruyen_GetHinhAnhTruyen.Name = "fullTruyen_GetHinhAnhTruyen";
            this.fullTruyen_GetHinhAnhTruyen.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_GetHinhAnhTruyen.Text = "2. Hình ảnh truyện";
            this.fullTruyen_GetHinhAnhTruyen.Click += new System.EventHandler(this.fullTruyen_GetHinhAnhTruyen_Click);
            // 
            // fullTruyen_GetTruyenMoiCaiNhat
            // 
            this.fullTruyen_GetTruyenMoiCaiNhat.Name = "fullTruyen_GetTruyenMoiCaiNhat";
            this.fullTruyen_GetTruyenMoiCaiNhat.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_GetTruyenMoiCaiNhat.Text = "3. Truyện mới cập nhật";
            // 
            // fullTruyen_ThongTinThieu
            // 
            this.fullTruyen_ThongTinThieu.Name = "fullTruyen_ThongTinThieu";
            this.fullTruyen_ThongTinThieu.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_ThongTinThieu.Text = "4. Thông tin thiếu";
            this.fullTruyen_ThongTinThieu.Click += new System.EventHandler(this.fullTruyen_ThongTinThieu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(290, 6);
            // 
            // fullTruyen_StopAll
            // 
            this.fullTruyen_StopAll.Name = "fullTruyen_StopAll";
            this.fullTruyen_StopAll.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_StopAll.Text = "Stop All";
            this.fullTruyen_StopAll.Click += new System.EventHandler(this.fullTruyen_StopAll_Click);
            // 
            // fullTruyen_RunAll
            // 
            this.fullTruyen_RunAll.Name = "fullTruyen_RunAll";
            this.fullTruyen_RunAll.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_RunAll.Text = "Continue All";
            this.fullTruyen_RunAll.Click += new System.EventHandler(this.fullTruyen_RunAll_Click);
            // 
            // fullTruyen_mnCloseAll
            // 
            this.fullTruyen_mnCloseAll.Name = "fullTruyen_mnCloseAll";
            this.fullTruyen_mnCloseAll.Size = new System.Drawing.Size(293, 34);
            this.fullTruyen_mnCloseAll.Text = "Closed All";
            this.fullTruyen_mnCloseAll.Click += new System.EventHandler(this.fullTruyen_mnCloseAll_Click);
            // 
            // sSTruyenToolStripMenuItem
            // 
            this.sSTruyenToolStripMenuItem.Name = "sSTruyenToolStripMenuItem";
            this.sSTruyenToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            // 
            // dTruyenToolStripMenuItem
            // 
            this.dTruyenToolStripMenuItem.Name = "dTruyenToolStripMenuItem";
            this.dTruyenToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            // 
            // topTruyenToolStripMenuItem
            // 
            this.topTruyenToolStripMenuItem.Name = "topTruyenToolStripMenuItem";
            this.topTruyenToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            // 
            // wattpadToolStripMenuItem
            // 
            this.wattpadToolStripMenuItem.Name = "wattpadToolStripMenuItem";
            this.wattpadToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            // 
            // mn_Zip
            // 
            this.mn_Zip.Name = "mn_Zip";
            this.mn_Zip.Size = new System.Drawing.Size(53, 31);
            this.mn_Zip.Text = "Zip";
            this.mn_Zip.Click += new System.EventHandler(this.mn_Zip_Click);
            // 
            // txtNumberRun
            // 
            this.txtNumberRun.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNumberRun.Name = "txtNumberRun";
            this.txtNumberRun.Size = new System.Drawing.Size(148, 31);
            this.txtNumberRun.ToolTipText = "Số lượng task";
            // 
            // mnChonHinhAnh
            // 
            this.mnChonHinhAnh.Name = "mnChonHinhAnh";
            this.mnChonHinhAnh.Size = new System.Drawing.Size(143, 31);
            this.mnChonHinhAnh.Text = "Chọn hình ảnh";
            this.mnChonHinhAnh.Click += new System.EventHandler(this.mnChonHinhAnh_Click);
            // 
            // gGDriveToolStripMenuItem
            // 
            this.gGDriveToolStripMenuItem.Name = "gGDriveToolStripMenuItem";
            this.gGDriveToolStripMenuItem.Size = new System.Drawing.Size(98, 31);
            this.gGDriveToolStripMenuItem.Text = "GG Drive";
            this.gGDriveToolStripMenuItem.Click += new System.EventHandler(this.ggDrive_Tong_Click);
            // 
            // fullTruyenToolStripMenuItem
            // 
            this.fullTruyenToolStripMenuItem.Name = "fullTruyenToolStripMenuItem";
            this.fullTruyenToolStripMenuItem.Size = new System.Drawing.Size(111, 31);
            this.fullTruyenToolStripMenuItem.Text = "Full Truyện";
            this.fullTruyenToolStripMenuItem.Click += new System.EventHandler(this.mn_FullTruyen_Text_Click);
            // 
            // mn_MenuFullTruyenTranh
            // 
            this.mn_MenuFullTruyenTranh.Name = "mn_MenuFullTruyenTranh";
            this.mn_MenuFullTruyenTranh.Size = new System.Drawing.Size(152, 31);
            this.mn_MenuFullTruyenTranh.Text = "FullTruyện tranh";
            this.mn_MenuFullTruyenTranh.Click += new System.EventHandler(this.mn_FullTruyen_Tranh_Click);
            // 
            // speechtoTextToolStripMenuItem
            // 
            this.speechtoTextToolStripMenuItem.Name = "speechtoTextToolStripMenuItem";
            this.speechtoTextToolStripMenuItem.Size = new System.Drawing.Size(147, 31);
            this.speechtoTextToolStripMenuItem.Text = "Text-To-Speech";
            this.speechtoTextToolStripMenuItem.Click += new System.EventHandler(this.TextToSpeechToolStripMenuItem_Click);
            // 
            // removeImageSpamToolStripMenuItem
            // 
            this.removeImageSpamToolStripMenuItem.Name = "removeImageSpamToolStripMenuItem";
            this.removeImageSpamToolStripMenuItem.Size = new System.Drawing.Size(198, 31);
            this.removeImageSpamToolStripMenuItem.Text = "Remove Image Spam";
            this.removeImageSpamToolStripMenuItem.Click += new System.EventHandler(this.removeImageSpamToolStripMenuItem_Click);
            // 
            // SSTruyen_HinhAnh
            // 
            this.SSTruyen_HinhAnh.Name = "SSTruyen_HinhAnh";
            this.SSTruyen_HinhAnh.Size = new System.Drawing.Size(32, 19);
            // 
            // dTruyen_HinhAnh
            // 
            this.dTruyen_HinhAnh.Name = "dTruyen_HinhAnh";
            this.dTruyen_HinhAnh.Size = new System.Drawing.Size(32, 19);
            // 
            // topTruyen_HinhAnh
            // 
            this.topTruyen_HinhAnh.Name = "topTruyen_HinhAnh";
            this.topTruyen_HinhAnh.Size = new System.Drawing.Size(32, 19);
            // 
            // wattpad_HinhAnh
            // 
            this.wattpad_HinhAnh.Name = "wattpad_HinhAnh";
            this.wattpad_HinhAnh.Size = new System.Drawing.Size(32, 19);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 37);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1908, 954);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // frmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1908, 991);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CatCode - Big Data";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem webToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuFullTruyen;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_GetChuongTruyen;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_GetHinhAnhTruyen;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_GetTruyenMoiCaiNhat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_StopAll;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_RunAll;
        private System.Windows.Forms.ToolStripTextBox txtNumberRun;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_mnCloseAll;
        private System.Windows.Forms.ToolStripMenuItem sSTruyenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SSTruyen_HinhAnh;
        private System.Windows.Forms.ToolStripMenuItem dTruyenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dTruyen_HinhAnh;
        private System.Windows.Forms.ToolStripMenuItem topTruyenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topTruyen_HinhAnh;
        private System.Windows.Forms.ToolStripMenuItem fullTruyen_ThongTinThieu;
        private System.Windows.Forms.ToolStripMenuItem mnChonHinhAnh;
        private System.Windows.Forms.ToolStripMenuItem wattpadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wattpad_HinhAnh;
        private System.Windows.Forms.ToolStripMenuItem mn_Zip;
        private System.Windows.Forms.ToolStripMenuItem gGDriveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullTruyenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mn_MenuFullTruyenTranh;
        private System.Windows.Forms.ToolStripMenuItem speechtoTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeImageSpamToolStripMenuItem;
    }
}