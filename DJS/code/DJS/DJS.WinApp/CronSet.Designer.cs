namespace DJS.WinApp
{
    partial class CronSet
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
            this.wbCronShow = new System.Windows.Forms.WebBrowser();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wbCronShow
            // 
            this.wbCronShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.wbCronShow.Location = new System.Drawing.Point(0, 0);
            this.wbCronShow.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbCronShow.Name = "wbCronShow";
            this.wbCronShow.Size = new System.Drawing.Size(665, 398);
            this.wbCronShow.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(172, 404);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(396, 404);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "取消";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // CronSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 436);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.wbCronShow);
            this.Name = "CronSet";
            this.Text = "CronSet";
            this.Load += new System.EventHandler(this.CronSet_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbCronShow;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnNo;
    }
}