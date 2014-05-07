namespace NetworkJogo
{
    partial class fmPacketCapture
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tmrCaptureBuffer = new System.Windows.Forms.Timer(this.components);
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.colCaptureDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colERRORMSG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrCaptureBuffer
            // 
            this.tmrCaptureBuffer.Interval = 1000;
            this.tmrCaptureBuffer.Tick += new System.EventHandler(this.tmrCaptureBuffer_Tick);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCaptureDate,
            this.colLength,
            this.colData,
            this.colERRORMSG});
            this.dgvList.Location = new System.Drawing.Point(14, 41);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.RowTemplate.Height = 21;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(961, 404);
            this.dgvList.TabIndex = 4;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(895, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(65, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(810, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(65, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(78, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(717, 20);
            this.label2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Interface : ";
            // 
            // colCaptureDate
            // 
            this.colCaptureDate.DataPropertyName = "CAPTURE_DATE";
            dataGridViewCellStyle1.Format = "hh:mm:ss";
            dataGridViewCellStyle1.NullValue = null;
            this.colCaptureDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCaptureDate.HeaderText = "DateTime";
            this.colCaptureDate.Name = "colCaptureDate";
            this.colCaptureDate.ReadOnly = true;
            // 
            // colLength
            // 
            this.colLength.DataPropertyName = "LINKLAYERTYPE";
            this.colLength.HeaderText = "LINK LAYER";
            this.colLength.Name = "colLength";
            this.colLength.ReadOnly = true;
            // 
            // colData
            // 
            this.colData.DataPropertyName = "DATA";
            this.colData.HeaderText = "DATA";
            this.colData.Name = "colData";
            this.colData.ReadOnly = true;
            this.colData.Width = 400;
            // 
            // colERRORMSG
            // 
            this.colERRORMSG.DataPropertyName = "ANALYSIS_ERROR_MSG";
            this.colERRORMSG.HeaderText = "ERROR MESSAGE";
            this.colERRORMSG.Name = "colERRORMSG";
            this.colERRORMSG.ReadOnly = true;
            this.colERRORMSG.Width = 300;
            // 
            // fmPacketCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 457);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "fmPacketCapture";
            this.Text = "Packet Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmPacketCapture_FormClosing);
            this.Load += new System.EventHandler(this.fmPacketCapture_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Timer tmrCaptureBuffer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCaptureDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colERRORMSG;
    }
}