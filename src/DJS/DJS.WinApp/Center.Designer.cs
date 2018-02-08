namespace DJS.WinApp
{
    partial class Center
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Center));
            this.tsMenu = new System.Windows.Forms.ToolStrip();
            this.tsbtnMain = new System.Windows.Forms.ToolStripButton();
            this.tsbtnJobShow = new System.Windows.Forms.ToolStripButton();
            this.tsbtnTriggerList = new System.Windows.Forms.ToolStripButton();
            this.tsbDataShow = new System.Windows.Forms.ToolStripButton();
            this.tsddbtnSet = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmtJobGroupMgr = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtTriggerGroupMgr = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtDllMgr = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtAddJob = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtDllJob = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmtRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.panMain = new System.Windows.Forms.Panel();
            this.ssShow = new System.Windows.Forms.StatusStrip();
            this.tsslblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.niImg = new System.Windows.Forms.NotifyIcon(this.components);
            this.tsMenu.SuspendLayout();
            this.ssShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMenu
            // 
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnMain,
            this.tsbtnJobShow,
            this.tsbtnTriggerList,
            this.tsbDataShow,
            this.tsddbtnSet});
            this.tsMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(784, 25);
            this.tsMenu.TabIndex = 14;
            // 
            // tsbtnMain
            // 
            this.tsbtnMain.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMain.Image")));
            this.tsbtnMain.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMain.Name = "tsbtnMain";
            this.tsbtnMain.Size = new System.Drawing.Size(52, 22);
            this.tsbtnMain.Text = "主页";
            this.tsbtnMain.Click += new System.EventHandler(this.tsbtnMain_Click);
            // 
            // tsbtnJobShow
            // 
            this.tsbtnJobShow.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnJobShow.Image")));
            this.tsbtnJobShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnJobShow.Name = "tsbtnJobShow";
            this.tsbtnJobShow.Size = new System.Drawing.Size(76, 22);
            this.tsbtnJobShow.Text = "任务列表";
            this.tsbtnJobShow.Click += new System.EventHandler(this.tsbtnJobShow_Click);
            // 
            // tsbtnTriggerList
            // 
            this.tsbtnTriggerList.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTriggerList.Image")));
            this.tsbtnTriggerList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTriggerList.Name = "tsbtnTriggerList";
            this.tsbtnTriggerList.Size = new System.Drawing.Size(88, 22);
            this.tsbtnTriggerList.Text = "触发器列表";
            this.tsbtnTriggerList.Visible = false;
            this.tsbtnTriggerList.Click += new System.EventHandler(this.tsbtnTriggerList_Click);
            // 
            // tsbDataShow
            // 
            this.tsbDataShow.Image = ((System.Drawing.Image)(resources.GetObject("tsbDataShow.Image")));
            this.tsbDataShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDataShow.Name = "tsbDataShow";
            this.tsbDataShow.Size = new System.Drawing.Size(76, 22);
            this.tsbDataShow.Text = "数据查看";
            this.tsbDataShow.Click += new System.EventHandler(this.tsbDataShow_Click);
            // 
            // tsddbtnSet
            // 
            this.tsddbtnSet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmtJobGroupMgr,
            this.tsmtTriggerGroupMgr,
            this.tsmtDllMgr,
            this.tsmtAddJob,
            this.tsmtRestart});
            this.tsddbtnSet.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnSet.Image")));
            this.tsddbtnSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnSet.Name = "tsddbtnSet";
            this.tsddbtnSet.Size = new System.Drawing.Size(61, 22);
            this.tsddbtnSet.Text = "设置";
            // 
            // tsmtJobGroupMgr
            // 
            this.tsmtJobGroupMgr.Name = "tsmtJobGroupMgr";
            this.tsmtJobGroupMgr.Size = new System.Drawing.Size(160, 22);
            this.tsmtJobGroupMgr.Text = "任务组配置";
            this.tsmtJobGroupMgr.Click += new System.EventHandler(this.tsmtJobGroupMgr_Click);
            // 
            // tsmtTriggerGroupMgr
            // 
            this.tsmtTriggerGroupMgr.Name = "tsmtTriggerGroupMgr";
            this.tsmtTriggerGroupMgr.Size = new System.Drawing.Size(160, 22);
            this.tsmtTriggerGroupMgr.Text = "触发器组配置";
            this.tsmtTriggerGroupMgr.Click += new System.EventHandler(this.tsmtTriggerGroupMgr_Click);
            // 
            // tsmtDllMgr
            // 
            this.tsmtDllMgr.Name = "tsmtDllMgr";
            this.tsmtDllMgr.Size = new System.Drawing.Size(160, 22);
            this.tsmtDllMgr.Text = "外部程序集配置";
            this.tsmtDllMgr.Click += new System.EventHandler(this.tsmtDllMgr_Click);
            // 
            // tsmtAddJob
            // 
            this.tsmtAddJob.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmtDllJob});
            this.tsmtAddJob.Name = "tsmtAddJob";
            this.tsmtAddJob.Size = new System.Drawing.Size(160, 22);
            this.tsmtAddJob.Text = "添加任务";
            this.tsmtAddJob.Click += new System.EventHandler(this.tsmtAddJob_Click);
            // 
            // tsmtDllJob
            // 
            this.tsmtDllJob.Name = "tsmtDllJob";
            this.tsmtDllJob.Size = new System.Drawing.Size(121, 22);
            this.tsmtDllJob.Text = "DLL任务";
            this.tsmtDllJob.Click += new System.EventHandler(this.tsmtDllJob_Click);
            // 
            // tsmtRestart
            // 
            this.tsmtRestart.Name = "tsmtRestart";
            this.tsmtRestart.Size = new System.Drawing.Size(160, 22);
            this.tsmtRestart.Text = "重新启动";
            this.tsmtRestart.Click += new System.EventHandler(this.tsmtRestart_Click);
            // 
            // panMain
            // 
            this.panMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panMain.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMain.Location = new System.Drawing.Point(13, 29);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(759, 381);
            this.panMain.TabIndex = 15;
            // 
            // ssShow
            // 
            this.ssShow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblTime});
            this.ssShow.Location = new System.Drawing.Point(0, 400);
            this.ssShow.Name = "ssShow";
            this.ssShow.Size = new System.Drawing.Size(784, 22);
            this.ssShow.TabIndex = 16;
            // 
            // tsslblTime
            // 
            this.tsslblTime.Name = "tsslblTime";
            this.tsslblTime.Size = new System.Drawing.Size(0, 17);
            // 
            // niImg
            // 
            this.niImg.Icon = ((System.Drawing.Icon)(resources.GetObject("niImg.Icon")));
            this.niImg.Text = "icon";
            this.niImg.DoubleClick += new System.EventHandler(this.niImg_DoubleClick);
            // 
            // Center
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 422);
            this.Controls.Add(this.ssShow);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.tsMenu);
            this.Name = "Center";
            this.Text = "Center";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Center_FormClosing);
            this.Load += new System.EventHandler(this.Center_Load);
            this.SizeChanged += new System.EventHandler(this.Center_SizeChanged);
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
            this.ssShow.ResumeLayout(false);
            this.ssShow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripButton tsbtnMain;
        private System.Windows.Forms.ToolStripButton tsbtnJobShow;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.StatusStrip ssShow;
        private System.Windows.Forms.ToolStripStatusLabel tsslblTime;
        private System.Windows.Forms.ToolStripButton tsbtnTriggerList;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnSet;
        private System.Windows.Forms.ToolStripMenuItem tsmtJobGroupMgr;
        private System.Windows.Forms.ToolStripMenuItem tsmtTriggerGroupMgr;
        private System.Windows.Forms.ToolStripMenuItem tsmtDllMgr;
        private System.Windows.Forms.ToolStripMenuItem tsmtAddJob;
        private System.Windows.Forms.ToolStripMenuItem tsmtDllJob;
        private System.Windows.Forms.ToolStripButton tsbDataShow;
        private System.Windows.Forms.ToolStripMenuItem tsmtRestart;
        private System.Windows.Forms.NotifyIcon niImg;
    }
}