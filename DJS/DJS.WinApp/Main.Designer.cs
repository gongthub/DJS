namespace DJS.WinApp
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbNows = new System.Windows.Forms.GroupBox();
            this.txtJobsNow = new System.Windows.Forms.TextBox();
            this.gbLogs = new System.Windows.Forms.GroupBox();
            this.txtLogsShow = new System.Windows.Forms.TextBox();
            this.gbNows.SuspendLayout();
            this.gbLogs.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbNows
            // 
            this.gbNows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbNows.AutoSize = true;
            this.gbNows.Controls.Add(this.txtJobsNow);
            this.gbNows.Location = new System.Drawing.Point(12, 287);
            this.gbNows.Name = "gbNows";
            this.gbNows.Size = new System.Drawing.Size(860, 155);
            this.gbNows.TabIndex = 3;
            this.gbNows.TabStop = false;
            this.gbNows.Text = "执行中任务";
            // 
            // txtJobsNow
            // 
            this.txtJobsNow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJobsNow.Location = new System.Drawing.Point(3, 17);
            this.txtJobsNow.Multiline = true;
            this.txtJobsNow.Name = "txtJobsNow";
            this.txtJobsNow.Size = new System.Drawing.Size(854, 135);
            this.txtJobsNow.TabIndex = 0;
            // 
            // gbLogs
            // 
            this.gbLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLogs.AutoSize = true;
            this.gbLogs.Controls.Add(this.txtLogsShow);
            this.gbLogs.Location = new System.Drawing.Point(12, 20);
            this.gbLogs.Name = "gbLogs";
            this.gbLogs.Size = new System.Drawing.Size(860, 261);
            this.gbLogs.TabIndex = 2;
            this.gbLogs.TabStop = false;
            this.gbLogs.Text = "任务日志";
            // 
            // txtLogsShow
            // 
            this.txtLogsShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogsShow.Location = new System.Drawing.Point(3, 17);
            this.txtLogsShow.Multiline = true;
            this.txtLogsShow.Name = "txtLogsShow";
            this.txtLogsShow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogsShow.Size = new System.Drawing.Size(854, 241);
            this.txtLogsShow.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 462);
            this.Controls.Add(this.gbNows);
            this.Controls.Add(this.gbLogs);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.gbNows.ResumeLayout(false);
            this.gbNows.PerformLayout();
            this.gbLogs.ResumeLayout(false);
            this.gbLogs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbNows;
        private System.Windows.Forms.GroupBox gbLogs;
        private System.Windows.Forms.TextBox txtLogsShow;
        private System.Windows.Forms.TextBox txtJobsNow;

    }
}

