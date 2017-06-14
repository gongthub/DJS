namespace DJS.WinApp.Setting
{
    partial class DLLUpdate
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
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUpLoad = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.ofdUpload = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(61, 24);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(192, 21);
            this.txtUrl.TabIndex = 11;
            this.txtUrl.DoubleClick += new System.EventHandler(this.txtUrl_DoubleClick);
            // 
            // lblUpLoad
            // 
            this.lblUpLoad.AutoSize = true;
            this.lblUpLoad.Location = new System.Drawing.Point(17, 27);
            this.lblUpLoad.Name = "lblUpLoad";
            this.lblUpLoad.Size = new System.Drawing.Size(29, 12);
            this.lblUpLoad.TabIndex = 14;
            this.lblUpLoad.Text = "上传";
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(179, 76);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 13;
            this.btnNo.Text = "取消";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(56, 76);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "确认";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ofdUpload
            // 
            this.ofdUpload.FileName = "ofdUpload";
            // 
            // DLLUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 128);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblUpLoad);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOk);
            this.Name = "DLLUpdate";
            this.Text = "DLL升级";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblUpLoad;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.OpenFileDialog ofdUpload;
    }
}