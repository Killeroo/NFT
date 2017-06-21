namespace NFT_Master
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.txtRange = new System.Windows.Forms.TextBox();
            this.listBoxSlaves = new System.Windows.Forms.CheckedListBox();
            this.slaveGroupBox = new System.Windows.Forms.GroupBox();
            this.dirsTreeView = new System.Windows.Forms.TreeView();
            this.dirsImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblStatus = new System.Windows.Forms.ToolStripLabel();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.synchronizeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retransferAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blacklistRulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slaveGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(229, 233);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(168, 89);
            this.txtLog.TabIndex = 0;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(328, 334);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txtRange
            // 
            this.txtRange.Location = new System.Drawing.Point(223, 336);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(99, 20);
            this.txtRange.TabIndex = 4;
            // 
            // listBoxSlaves
            // 
            this.listBoxSlaves.CheckOnClick = true;
            this.listBoxSlaves.FormattingEnabled = true;
            this.listBoxSlaves.Location = new System.Drawing.Point(6, 19);
            this.listBoxSlaves.Name = "listBoxSlaves";
            this.listBoxSlaves.Size = new System.Drawing.Size(168, 199);
            this.listBoxSlaves.TabIndex = 6;
            // 
            // slaveGroupBox
            // 
            this.slaveGroupBox.Controls.Add(this.listBoxSlaves);
            this.slaveGroupBox.Location = new System.Drawing.Point(223, 3);
            this.slaveGroupBox.Name = "slaveGroupBox";
            this.slaveGroupBox.Size = new System.Drawing.Size(180, 224);
            this.slaveGroupBox.TabIndex = 7;
            this.slaveGroupBox.TabStop = false;
            this.slaveGroupBox.Text = "Connected Slaves: 0";
            // 
            // dirsTreeView
            // 
            this.dirsTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dirsTreeView.ImageIndex = 0;
            this.dirsTreeView.ImageList = this.dirsImageList;
            this.dirsTreeView.Location = new System.Drawing.Point(6, 42);
            this.dirsTreeView.Name = "dirsTreeView";
            this.dirsTreeView.SelectedImageIndex = 0;
            this.dirsTreeView.Size = new System.Drawing.Size(184, 277);
            this.dirsTreeView.TabIndex = 8;
            this.dirsTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.dirsTreeView_BeforeExpand);
            // 
            // dirsImageList
            // 
            this.dirsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("dirsImageList.ImageStream")));
            this.dirsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.dirsImageList.Images.SetKeyName(0, "1346238561_folder_classic.png");
            this.dirsImageList.Images.SetKeyName(1, "1346238604_folder_classic_opened.png");
            this.dirsImageList.Images.SetKeyName(2, "1346228331_drive.png");
            this.dirsImageList.Images.SetKeyName(3, "1346228337_drive_cd.png");
            this.dirsImageList.Images.SetKeyName(4, "1346228356_drive_cd_empty.png");
            this.dirsImageList.Images.SetKeyName(5, "1346228364_drive_disk.png");
            this.dirsImageList.Images.SetKeyName(6, "1346228591_drive_network.png");
            this.dirsImageList.Images.SetKeyName(7, "1346228618_drive_link.png");
            this.dirsImageList.Images.SetKeyName(8, "1346228623_drive_error.png");
            this.dirsImageList.Images.SetKeyName(9, "1346228633_drive_go.png");
            this.dirsImageList.Images.SetKeyName(10, "1346228636_drive_delete.png");
            this.dirsImageList.Images.SetKeyName(11, "1346228639_drive_burn.png");
            this.dirsImageList.Images.SetKeyName(12, "1346238642_folder_classic_locked.png");
            this.dirsImageList.Images.SetKeyName(13, "file.png");
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Location = new System.Drawing.Point(166, 16);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(24, 20);
            this.btnFolderBrowse.TabIndex = 10;
            this.btnFolderBrowse.Text = "...";
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // txtWorkingDirectory
            // 
            this.txtWorkingDirectory.Location = new System.Drawing.Point(6, 16);
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.Size = new System.Drawing.Size(154, 20);
            this.txtWorkingDirectory.TabIndex = 9;
            this.txtWorkingDirectory.Text = "C:\\Projects";
            this.txtWorkingDirectory.TextChanged += new System.EventHandler(this.txtWorkingDirectory_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dirsTreeView);
            this.groupBox2.Controls.Add(this.txtWorkingDirectory);
            this.groupBox2.Controls.Add(this.btnFolderBrowse);
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(196, 325);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Working Directory";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.groupBox2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.slaveGroupBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.txtRange);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnSend);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.txtMessage);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnScan);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.txtLog);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(415, 365);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(415, 414);
            this.toolStripContainer1.TabIndex = 13;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.toolStripSeparator1,
            this.lblStatus});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(190, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // progressBar
            // 
            this.progressBar.MarqueeAnimationSpeed = 50;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 22);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 22);
            this.lblStatus.Text = "Ready";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(127, 334);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(22, 336);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(99, 20);
            this.txtMessage.TabIndex = 2;
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.connectionToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(415, 24);
            this.menuStrip.TabIndex = 12;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.synchronizeAllToolStripMenuItem,
            this.retransferAllToolStripMenuItem});
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.connectionToolStripMenuItem.Text = "Actions";
            // 
            // synchronizeAllToolStripMenuItem
            // 
            this.synchronizeAllToolStripMenuItem.Name = "synchronizeAllToolStripMenuItem";
            this.synchronizeAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.synchronizeAllToolStripMenuItem.Text = "Synchronize All";
            // 
            // retransferAllToolStripMenuItem
            // 
            this.retransferAllToolStripMenuItem.Name = "retransferAllToolStripMenuItem";
            this.retransferAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.retransferAllToolStripMenuItem.Text = "Retransfer All";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blacklistRulesToolStripMenuItem,
            this.preferencesToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // blacklistRulesToolStripMenuItem
            // 
            this.blacklistRulesToolStripMenuItem.Name = "blacklistRulesToolStripMenuItem";
            this.blacklistRulesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.blacklistRulesToolStripMenuItem.Text = "Blacklist Rules";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 414);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "MainForm";
            this.Text = "Network File Transfer - Master";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.slaveGroupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox txtRange;
        private System.Windows.Forms.CheckedListBox listBoxSlaves;
        private System.Windows.Forms.GroupBox slaveGroupBox;
        private System.Windows.Forms.TreeView dirsTreeView;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.ImageList dirsImageList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem synchronizeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retransferAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blacklistRulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblStatus;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
    }
}