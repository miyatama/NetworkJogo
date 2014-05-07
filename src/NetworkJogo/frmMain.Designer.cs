namespace NetworkJogo
{
    partial class frmMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.colIPADDR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHOSTNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhigicalAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveIPAddrListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreIPAddrListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redeRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pesquisaDaLocalRedeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RPPoisoningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packetCaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自端末情報表示IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbInterface = new System.Windows.Forms.ComboBox();
            this.lblInterfaceDesc = new System.Windows.Forms.Label();
            this.gbxArpPoisoningInfo = new System.Windows.Forms.GroupBox();
            this.lblArppTargetMac = new System.Windows.Forms.TextBox();
            this.lblArppTargetIP = new System.Windows.Forms.TextBox();
            this.txtTargetDestIP = new System.Windows.Forms.TextBox();
            this.btnArppExec = new System.Windows.Forms.Button();
            this.lblArppTargetDestMac = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.gbxArpPoisoningInfo.SuspendLayout();
            this.SuspendLayout();
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
            this.colIPADDR,
            this.colHOSTNAME,
            this.colPhigicalAddress});
            this.dgvList.Location = new System.Drawing.Point(12, 54);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.RowTemplate.Height = 21;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(826, 324);
            this.dgvList.TabIndex = 4;
            this.dgvList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_RowEnter);
            // 
            // colIPADDR
            // 
            this.colIPADDR.DataPropertyName = "IPADDR_STR";
            this.colIPADDR.HeaderText = "IPADDR_STR";
            this.colIPADDR.Name = "colIPADDR";
            this.colIPADDR.ReadOnly = true;
            this.colIPADDR.Width = 200;
            // 
            // colHOSTNAME
            // 
            this.colHOSTNAME.DataPropertyName = "HOSTNAME";
            this.colHOSTNAME.HeaderText = "HOSTNAME";
            this.colHOSTNAME.Name = "colHOSTNAME";
            this.colHOSTNAME.ReadOnly = true;
            this.colHOSTNAME.Width = 300;
            // 
            // colPhigicalAddress
            // 
            this.colPhigicalAddress.DataPropertyName = "MAC_ADDR";
            this.colPhigicalAddress.HeaderText = "MAC_ADDR";
            this.colPhigicalAddress.Name = "colPhigicalAddress";
            this.colPhigicalAddress.Width = 200;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.redeRToolStripMenuItem,
            this.ヘルプHToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(850, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveIPAddrListToolStripMenuItem,
            this.restoreIPAddrListToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // saveIPAddrListToolStripMenuItem
            // 
            this.saveIPAddrListToolStripMenuItem.Name = "saveIPAddrListToolStripMenuItem";
            this.saveIPAddrListToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveIPAddrListToolStripMenuItem.Text = "ファイルへ保存";
            this.saveIPAddrListToolStripMenuItem.Click += new System.EventHandler(this.saveIPAddrListToolStripMenuItem_Click);
            // 
            // restoreIPAddrListToolStripMenuItem
            // 
            this.restoreIPAddrListToolStripMenuItem.Name = "restoreIPAddrListToolStripMenuItem";
            this.restoreIPAddrListToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.restoreIPAddrListToolStripMenuItem.Text = "ファイルからリロード";
            this.restoreIPAddrListToolStripMenuItem.Click += new System.EventHandler(this.restoreIPAddrListToolStripMenuItem_Click);
            // 
            // redeRToolStripMenuItem
            // 
            this.redeRToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pesquisaDaLocalRedeToolStripMenuItem,
            this.RPPoisoningToolStripMenuItem,
            this.packetCaptureToolStripMenuItem});
            this.redeRToolStripMenuItem.Name = "redeRToolStripMenuItem";
            this.redeRToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.redeRToolStripMenuItem.Text = "ネットワーク(&R)";
            // 
            // pesquisaDaLocalRedeToolStripMenuItem
            // 
            this.pesquisaDaLocalRedeToolStripMenuItem.Name = "pesquisaDaLocalRedeToolStripMenuItem";
            this.pesquisaDaLocalRedeToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.pesquisaDaLocalRedeToolStripMenuItem.Text = "同一セグメント端末走査";
            this.pesquisaDaLocalRedeToolStripMenuItem.Click += new System.EventHandler(this.pesquisaDaLocalRedeToolStripMenuItem_Click);
            // 
            // RPPoisoningToolStripMenuItem
            // 
            this.RPPoisoningToolStripMenuItem.Name = "RPPoisoningToolStripMenuItem";
            this.RPPoisoningToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.RPPoisoningToolStripMenuItem.Text = "ARP Poisoning";
            this.RPPoisoningToolStripMenuItem.Click += new System.EventHandler(this.RPPoisoningToolStripMenuItem_Click);
            // 
            // packetCaptureToolStripMenuItem
            // 
            this.packetCaptureToolStripMenuItem.Name = "packetCaptureToolStripMenuItem";
            this.packetCaptureToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.packetCaptureToolStripMenuItem.Text = "Packet Capture";
            this.packetCaptureToolStripMenuItem.Click += new System.EventHandler(this.packetCaptureToolStripMenuItem_Click);
            // 
            // ヘルプHToolStripMenuItem
            // 
            this.ヘルプHToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.自端末情報表示IToolStripMenuItem});
            this.ヘルプHToolStripMenuItem.Name = "ヘルプHToolStripMenuItem";
            this.ヘルプHToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.ヘルプHToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // 自端末情報表示IToolStripMenuItem
            // 
            this.自端末情報表示IToolStripMenuItem.Name = "自端末情報表示IToolStripMenuItem";
            this.自端末情報表示IToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.自端末情報表示IToolStripMenuItem.Text = "自端末情報(&I)";
            this.自端末情報表示IToolStripMenuItem.Click += new System.EventHandler(this.自端末情報表示IToolStripMenuItem_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Location = new System.Drawing.Point(0, 427);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(850, 20);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "なにこれ";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Interface : ";
            // 
            // cmbInterface
            // 
            this.cmbInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInterface.FormattingEnabled = true;
            this.cmbInterface.Location = new System.Drawing.Point(80, 28);
            this.cmbInterface.Name = "cmbInterface";
            this.cmbInterface.Size = new System.Drawing.Size(183, 20);
            this.cmbInterface.TabIndex = 2;
            this.cmbInterface.SelectedIndexChanged += new System.EventHandler(this.cmbInterface_SelectedIndexChanged);
            // 
            // lblInterfaceDesc
            // 
            this.lblInterfaceDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInterfaceDesc.Location = new System.Drawing.Point(269, 30);
            this.lblInterfaceDesc.Name = "lblInterfaceDesc";
            this.lblInterfaceDesc.Size = new System.Drawing.Size(574, 17);
            this.lblInterfaceDesc.TabIndex = 3;
            this.lblInterfaceDesc.Text = "Jogo";
            // 
            // gbxArpPoisoningInfo
            // 
            this.gbxArpPoisoningInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxArpPoisoningInfo.Controls.Add(this.lblArppTargetMac);
            this.gbxArpPoisoningInfo.Controls.Add(this.lblArppTargetIP);
            this.gbxArpPoisoningInfo.Controls.Add(this.txtTargetDestIP);
            this.gbxArpPoisoningInfo.Controls.Add(this.btnArppExec);
            this.gbxArpPoisoningInfo.Controls.Add(this.lblArppTargetDestMac);
            this.gbxArpPoisoningInfo.Controls.Add(this.label3);
            this.gbxArpPoisoningInfo.Controls.Add(this.label2);
            this.gbxArpPoisoningInfo.Location = new System.Drawing.Point(12, 376);
            this.gbxArpPoisoningInfo.Name = "gbxArpPoisoningInfo";
            this.gbxArpPoisoningInfo.Size = new System.Drawing.Size(831, 48);
            this.gbxArpPoisoningInfo.TabIndex = 5;
            this.gbxArpPoisoningInfo.TabStop = false;
            this.gbxArpPoisoningInfo.Text = "ARP poisoning info";
            // 
            // lblArppTargetMac
            // 
            this.lblArppTargetMac.Location = new System.Drawing.Point(182, 17);
            this.lblArppTargetMac.Name = "lblArppTargetMac";
            this.lblArppTargetMac.Size = new System.Drawing.Size(133, 19);
            this.lblArppTargetMac.TabIndex = 8;
            this.lblArppTargetMac.Text = "192.168.001.084";
            this.lblArppTargetMac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArppTargetIP
            // 
            this.lblArppTargetIP.Location = new System.Drawing.Point(76, 18);
            this.lblArppTargetIP.Name = "lblArppTargetIP";
            this.lblArppTargetIP.Size = new System.Drawing.Size(100, 19);
            this.lblArppTargetIP.TabIndex = 7;
            this.lblArppTargetIP.Text = "192.168.001.084";
            this.lblArppTargetIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTargetDestIP
            // 
            this.txtTargetDestIP.Location = new System.Drawing.Point(449, 16);
            this.txtTargetDestIP.Name = "txtTargetDestIP";
            this.txtTargetDestIP.Size = new System.Drawing.Size(100, 19);
            this.txtTargetDestIP.TabIndex = 4;
            this.txtTargetDestIP.Text = "192.168.001.084";
            this.txtTargetDestIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetDestIP.Validating += new System.ComponentModel.CancelEventHandler(this.txtTargetDestIP_Validating);
            // 
            // btnArppExec
            // 
            this.btnArppExec.Location = new System.Drawing.Point(736, 16);
            this.btnArppExec.Name = "btnArppExec";
            this.btnArppExec.Size = new System.Drawing.Size(78, 20);
            this.btnArppExec.TabIndex = 6;
            this.btnArppExec.Text = "Start";
            this.btnArppExec.UseVisualStyleBackColor = true;
            this.btnArppExec.Click += new System.EventHandler(this.btnArppExec_Click);
            // 
            // lblArppTargetDestMac
            // 
            this.lblArppTargetDestMac.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblArppTargetDestMac.Location = new System.Drawing.Point(555, 17);
            this.lblArppTargetDestMac.Name = "lblArppTargetDestMac";
            this.lblArppTargetDestMac.Size = new System.Drawing.Size(139, 18);
            this.lblArppTargetDestMac.TabIndex = 5;
            this.lblArppTargetDestMac.Text = "00:00:00:00:00:00:00:00";
            this.lblArppTargetDestMac.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(321, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Target destination IP : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Target IP : ";
            // 
            // tmrMessage
            // 
            this.tmrMessage.Interval = 1000;
            this.tmrMessage.Tick += new System.EventHandler(this.tmrMessage_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 447);
            this.Controls.Add(this.gbxArpPoisoningInfo);
            this.Controls.Add(this.lblInterfaceDesc);
            this.Controls.Add(this.cmbInterface);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "NetworkJogo Main";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbxArpPoisoningInfo.ResumeLayout(false);
            this.gbxArpPoisoningInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ヘルプHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自端末情報表示IToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redeRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pesquisaDaLocalRedeToolStripMenuItem;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ToolStripMenuItem saveIPAddrListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreIPAddrListToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbInterface;
        private System.Windows.Forms.Label lblInterfaceDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIPADDR;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHOSTNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhigicalAddress;
        private System.Windows.Forms.ToolStripMenuItem RPPoisoningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packetCaptureToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbxArpPoisoningInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblArppTargetDestMac;
        private System.Windows.Forms.Button btnArppExec;
        private System.Windows.Forms.TextBox txtTargetDestIP;
        private System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.TextBox lblArppTargetMac;
        private System.Windows.Forms.TextBox lblArppTargetIP;
    }
}

