namespace CatCode_Selenium
{
    partial class UZipFile
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProcessMessage = new System.Windows.Forms.Label();
            this.richLog = new System.Windows.Forms.RichTextBox();
            this.ckbStop = new System.Windows.Forms.CheckBox();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblBaseURL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(15, 163);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(384, 10);
            this.progressBar1.TabIndex = 3;
            // 
            // lblProcessMessage
            // 
            this.lblProcessMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProcessMessage.Location = new System.Drawing.Point(12, 125);
            this.lblProcessMessage.Name = "lblProcessMessage";
            this.lblProcessMessage.Size = new System.Drawing.Size(332, 35);
            this.lblProcessMessage.TabIndex = 4;
            this.lblProcessMessage.Text = "Process message";
            this.lblProcessMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // richLog
            // 
            this.richLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richLog.Location = new System.Drawing.Point(12, 27);
            this.richLog.Name = "richLog";
            this.richLog.Size = new System.Drawing.Size(387, 95);
            this.richLog.TabIndex = 5;
            this.richLog.Text = "";
            // 
            // ckbStop
            // 
            this.ckbStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbStop.AutoSize = true;
            this.ckbStop.Location = new System.Drawing.Point(339, 30);
            this.ckbStop.Name = "ckbStop";
            this.ckbStop.Size = new System.Drawing.Size(55, 17);
            this.ckbStop.TabIndex = 6;
            this.ckbStop.Text = "STOP";
            this.ckbStop.UseVisualStyleBackColor = true;
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercent.Location = new System.Drawing.Point(350, 125);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(49, 37);
            this.lblPercent.TabIndex = 8;
            this.lblPercent.Text = "#NA";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBaseURL
            // 
            this.lblBaseURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBaseURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaseURL.Location = new System.Drawing.Point(15, 5);
            this.lblBaseURL.Name = "lblBaseURL";
            this.lblBaseURL.Size = new System.Drawing.Size(383, 17);
            this.lblBaseURL.TabIndex = 9;
            this.lblBaseURL.Text = "Base URL";
            this.lblBaseURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UTruyenFull
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblBaseURL);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.ckbStop);
            this.Controls.Add(this.richLog);
            this.Controls.Add(this.lblProcessMessage);
            this.Controls.Add(this.progressBar1);
            this.Name = "UTruyenFull";
            this.Size = new System.Drawing.Size(408, 185);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProcessMessage;
        private System.Windows.Forms.RichTextBox richLog;
        public System.Windows.Forms.CheckBox ckbStop;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblBaseURL;
    }
}