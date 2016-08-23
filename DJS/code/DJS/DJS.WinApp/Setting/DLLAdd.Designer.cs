namespace DJS.WinApp
{
    partial class DLLAdd
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
            this.nudNo = new System.Windows.Forms.NumericUpDown();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblGroupNo = new System.Windows.Forms.Label();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblUpLoad = new System.Windows.Forms.Label();
            this.ofdUpLoad = new System.Windows.Forms.OpenFileDialog();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudNo)).BeginInit();
            this.SuspendLayout();
            // 
            // nudNo
            // 
            this.nudNo.Location = new System.Drawing.Point(114, 34);
            this.nudNo.Name = "nudNo";
            this.nudNo.Size = new System.Drawing.Size(192, 21);
            this.nudNo.TabIndex = 9;
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(114, 97);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(192, 21);
            this.txtNameSpace.TabIndex = 8;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(45, 97);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(53, 12);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "命名空间";
            // 
            // lblGroupNo
            // 
            this.lblGroupNo.AutoSize = true;
            this.lblGroupNo.Location = new System.Drawing.Point(69, 34);
            this.lblGroupNo.Name = "lblGroupNo";
            this.lblGroupNo.Size = new System.Drawing.Size(29, 12);
            this.lblGroupNo.TabIndex = 7;
            this.lblGroupNo.Text = "序号";
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(231, 186);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "取消";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(108, 186);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "确认";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblUpLoad
            // 
            this.lblUpLoad.AutoSize = true;
            this.lblUpLoad.Location = new System.Drawing.Point(69, 137);
            this.lblUpLoad.Name = "lblUpLoad";
            this.lblUpLoad.Size = new System.Drawing.Size(29, 12);
            this.lblUpLoad.TabIndex = 10;
            this.lblUpLoad.Text = "上传";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(113, 134);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(192, 21);
            this.txtUrl.TabIndex = 11;
            this.txtUrl.DoubleClick += new System.EventHandler(this.txtUrl_DoubleClick);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(113, 67);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(192, 21);
            this.txtName.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "名称";
            // 
            // DLLAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 262);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblUpLoad);
            this.Controls.Add(this.nudNo);
            this.Controls.Add(this.txtNameSpace);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblGroupNo);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOk);
            this.Name = "DLLAdd";
            this.Text = "DLL操作";
            ((System.ComponentModel.ISupportInitialize)(this.nudNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudNo;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblGroupNo;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblUpLoad;
        private System.Windows.Forms.OpenFileDialog ofdUpLoad;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
    }
}