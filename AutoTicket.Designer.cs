namespace CatCode_Selenium
{
    partial class AutoTicket
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoTicket));
            this.button1 = new System.Windows.Forms.Button();
            this.richPayloadQuery = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbAllawTop = new System.Windows.Forms.CheckBox();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Id = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AreaPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssignedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkItemType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(724, 392);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "RUN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richPayloadQuery
            // 
            this.richPayloadQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richPayloadQuery.Location = new System.Drawing.Point(12, 394);
            this.richPayloadQuery.Name = "richPayloadQuery";
            this.richPayloadQuery.ReadOnly = true;
            this.richPayloadQuery.Size = new System.Drawing.Size(647, 44);
            this.richPayloadQuery.TabIndex = 1;
            this.richPayloadQuery.Text = resources.GetString("richPayloadQuery.Text");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 378);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Payload Query";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Title,
            this.State,
            this.AreaPath,
            this.AssignedTo,
            this.WorkItemType});
            this.dataGridView1.Location = new System.Drawing.Point(12, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(706, 346);
            this.dataGridView1.TabIndex = 3;
            // 
            // txtInterval
            // 
            this.txtInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInterval.Location = new System.Drawing.Point(668, 413);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(39, 20);
            this.txtInterval.TabIndex = 4;
            this.txtInterval.Text = "60";
            this.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(660, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Interval(s)";
            // 
            // ckbAllawTop
            // 
            this.ckbAllawTop.AutoSize = true;
            this.ckbAllawTop.Location = new System.Drawing.Point(12, 6);
            this.ckbAllawTop.Name = "ckbAllawTop";
            this.ckbAllawTop.Size = new System.Drawing.Size(92, 17);
            this.ckbAllawTop.TabIndex = 6;
            this.ckbAllawTop.Text = "Always on top";
            this.ckbAllawTop.UseVisualStyleBackColor = true;
            this.ckbAllawTop.CheckedChanged += new System.EventHandler(this.ckbAllawTop_CheckedChanged);
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(174, 6);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(302, 20);
            this.txtToken.TabIndex = 7;
            this.txtToken.Text = "csjhrusin4ofmiplw4okjo5igp7maan3ocs4waumlb6rrfhecjqa";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Token:";
            // 
            // Id
            // 
            this.Id.DataPropertyName = "System.Id";
            this.Id.Frozen = true;
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 50;
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title.DataPropertyName = "System.Title";
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // State
            // 
            this.State.DataPropertyName = "System.State";
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.Visible = false;
            this.State.Width = 50;
            // 
            // AreaPath
            // 
            this.AreaPath.DataPropertyName = "Custom.DefectIdentifiedInVersion";
            this.AreaPath.HeaderText = "Version";
            this.AreaPath.Name = "AreaPath";
            this.AreaPath.Width = 50;
            // 
            // AssignedTo
            // 
            this.AssignedTo.DataPropertyName = "System.AssignedTo";
            this.AssignedTo.HeaderText = "Assigned";
            this.AssignedTo.Name = "AssignedTo";
            this.AssignedTo.ReadOnly = true;
            this.AssignedTo.Visible = false;
            this.AssignedTo.Width = 80;
            // 
            // WorkItemType
            // 
            this.WorkItemType.DataPropertyName = "System.WorkItemType";
            this.WorkItemType.HeaderText = "Type";
            this.WorkItemType.Name = "WorkItemType";
            this.WorkItemType.ReadOnly = true;
            this.WorkItemType.Visible = false;
            this.WorkItemType.Width = 50;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "6.1",
            "6.1.1",
            "6.2",
            "6.2.1",
            "7.0"});
            this.checkedListBox1.Location = new System.Drawing.Point(724, 29);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(64, 79);
            this.checkedListBox1.TabIndex = 9;
            // 
            // AutoTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.ckbAllawTop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richPayloadQuery);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoTicket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoTicket";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richPayloadQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckbAllawTop;
        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewLinkColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn AreaPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn AssignedTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkItemType;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
    }
}