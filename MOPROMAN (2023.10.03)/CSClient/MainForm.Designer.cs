namespace nsAspur
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TextError = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tmrRead = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbChC = new System.Windows.Forms.Label();
            this.lbErrors = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lbBytesRead = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbPrecHodnotaRaw = new System.Windows.Forms.Label();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbxLogs = new System.Windows.Forms.ListBox();
            this.btnCitaj = new System.Windows.Forms.Button();
            this.lbPrecHodnota = new System.Windows.Forms.Label();
            this.lbPrecHodnotaText = new System.Windows.Forms.Label();
            this.c = new System.Windows.Forms.Label();
            this.tmrErr = new System.Windows.Forms.Timer(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // TextError
            // 
            this.TextError.BackColor = System.Drawing.Color.White;
            this.TextError.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TextError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextError.ForeColor = System.Drawing.Color.Black;
            this.TextError.Location = new System.Drawing.Point(0, 1130);
            this.TextError.Margin = new System.Windows.Forms.Padding(4);
            this.TextError.Name = "TextError";
            this.TextError.ReadOnly = true;
            this.TextError.Size = new System.Drawing.Size(1280, 23);
            this.TextError.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(867, 302);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 28);
            this.button1.TabIndex = 52;
            this.button1.Text = "Write Fields";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "white.png");
            this.imageList1.Images.SetKeyName(1, "img_sip_dole.png");
            this.imageList1.Images.SetKeyName(2, "img_sip_hore.png");
            this.imageList1.Images.SetKeyName(3, "PLC ICO.png");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbChC);
            this.panel2.Controls.Add(this.lbErrors);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.lbBytesRead);
            this.panel2.Controls.Add(this.btnInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 1054);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1280, 76);
            this.panel2.TabIndex = 61;
            // 
            // lbChC
            // 
            this.lbChC.AutoSize = true;
            this.lbChC.Location = new System.Drawing.Point(111, 25);
            this.lbChC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbChC.Name = "lbChC";
            this.lbChC.Size = new System.Drawing.Size(90, 15);
            this.lbChC.TabIndex = 82;
            this.lbChC.Text = "Reading Errors";
            this.lbChC.Visible = false;
            // 
            // lbErrors
            // 
            this.lbErrors.AutoSize = true;
            this.lbErrors.Location = new System.Drawing.Point(144, 48);
            this.lbErrors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbErrors.Name = "lbErrors";
            this.lbErrors.Size = new System.Drawing.Size(14, 15);
            this.lbErrors.TabIndex = 81;
            this.lbErrors.Text = "0";
            this.lbErrors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbErrors.Visible = false;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(13, 25);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(69, 15);
            this.label25.TabIndex = 79;
            this.label25.Text = "Bytes Read";
            // 
            // lbBytesRead
            // 
            this.lbBytesRead.AutoSize = true;
            this.lbBytesRead.Location = new System.Drawing.Point(40, 47);
            this.lbBytesRead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBytesRead.Name = "lbBytesRead";
            this.lbBytesRead.Size = new System.Drawing.Size(14, 15);
            this.lbBytesRead.TabIndex = 78;
            this.lbBytesRead.Text = "0";
            this.lbBytesRead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.lbPrecHodnotaRaw);
            this.panel1.Controls.Add(this.dgvRecords);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnCitaj);
            this.panel1.Controls.Add(this.lbPrecHodnota);
            this.panel1.Controls.Add(this.lbPrecHodnotaText);
            this.panel1.Controls.Add(this.c);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 1054);
            this.panel1.TabIndex = 62;
            // 
            // lbPrecHodnotaRaw
            // 
            this.lbPrecHodnotaRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbPrecHodnotaRaw.AutoSize = true;
            this.lbPrecHodnotaRaw.Location = new System.Drawing.Point(173, 942);
            this.lbPrecHodnotaRaw.Name = "lbPrecHodnotaRaw";
            this.lbPrecHodnotaRaw.Size = new System.Drawing.Size(118, 15);
            this.lbPrecHodnotaRaw.TabIndex = 22;
            this.lbPrecHodnotaRaw.Text = "0x00000000000000";
            // 
            // dgvRecords
            // 
            this.dgvRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Location = new System.Drawing.Point(12, 12);
            this.dgvRecords.Name = "dgvRecords";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecords.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecords.RowHeadersVisible = false;
            this.dgvRecords.RowHeadersWidth = 51;
            this.dgvRecords.RowTemplate.Height = 24;
            this.dgvRecords.Size = new System.Drawing.Size(1256, 385);
            this.dgvRecords.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.Location = new System.Drawing.Point(176, 987);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(143, 41);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "PAUSE";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.lbxLogs);
            this.groupBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(6, 400);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1268, 539);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logs";
            // 
            // lbxLogs
            // 
            this.lbxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbxLogs.ItemHeight = 18;
            this.lbxLogs.Location = new System.Drawing.Point(3, 22);
            this.lbxLogs.Margin = new System.Windows.Forms.Padding(20);
            this.lbxLogs.Name = "lbxLogs";
            this.lbxLogs.Size = new System.Drawing.Size(1262, 514);
            this.lbxLogs.TabIndex = 0;
            this.lbxLogs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbxLogs_DrawItem);
            // 
            // btnCitaj
            // 
            this.btnCitaj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCitaj.Location = new System.Drawing.Point(15, 987);
            this.btnCitaj.Name = "btnCitaj";
            this.btnCitaj.Size = new System.Drawing.Size(125, 41);
            this.btnCitaj.TabIndex = 16;
            this.btnCitaj.Text = "Čítaj";
            this.btnCitaj.UseVisualStyleBackColor = true;
            this.btnCitaj.Click += new System.EventHandler(this.btnCitaj_Click);
            // 
            // lbPrecHodnota
            // 
            this.lbPrecHodnota.AutoSize = true;
            this.lbPrecHodnota.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbPrecHodnota.Location = new System.Drawing.Point(156, 786);
            this.lbPrecHodnota.Name = "lbPrecHodnota";
            this.lbPrecHodnota.Size = new System.Drawing.Size(86, 16);
            this.lbPrecHodnota.TabIndex = 15;
            this.lbPrecHodnota.Text = "0x00000000";
            this.lbPrecHodnota.Visible = false;
            // 
            // lbPrecHodnotaText
            // 
            this.lbPrecHodnotaText.AutoSize = true;
            this.lbPrecHodnotaText.Location = new System.Drawing.Point(22, 786);
            this.lbPrecHodnotaText.Name = "lbPrecHodnotaText";
            this.lbPrecHodnotaText.Size = new System.Drawing.Size(110, 15);
            this.lbPrecHodnotaText.TabIndex = 13;
            this.lbPrecHodnotaText.Text = "Precitana hodnota:";
            this.lbPrecHodnotaText.Visible = false;
            // 
            // c
            // 
            this.c.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.c.AutoSize = true;
            this.c.Location = new System.Drawing.Point(12, 942);
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(144, 15);
            this.c.TabIndex = 12;
            this.c.Text = "Precitana hodnota (PLC):";
            // 
            // tmrErr
            // 
            this.tmrErr.Interval = 1000;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackgroundImage = global::nsAspur.Properties.Resources.logo1;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(847, 11);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(352, 57);
            this.pictureBox2.TabIndex = 80;
            this.pictureBox2.TabStop = false;
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfo.BackgroundImage = global::nsAspur.Properties.Resources.FLAME;
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInfo.Location = new System.Drawing.Point(1207, 5);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(4);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(67, 67);
            this.btnInfo.TabIndex = 77;
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 1153);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TextError);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "MOPROMAN (DUSAN NECEK ©2023)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.TextBox TextError;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer tmrRead;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lbBytesRead;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbChC;
        private System.Windows.Forms.Label lbErrors;
        private System.Windows.Forms.Timer tmrErr;
        private System.Windows.Forms.Label lbPrecHodnota;
        private System.Windows.Forms.Label lbPrecHodnotaText;
        private System.Windows.Forms.Label c;
        private System.Windows.Forms.Button btnCitaj;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbxLogs;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.Label lbPrecHodnotaRaw;
    }
}

