namespace DJS.WinApp
{
    partial class AddJob
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTriggerName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.txtCron = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.cbJobGroup = new System.Windows.Forms.ComboBox();
            this.cbTriggerGroup = new System.Windows.Forms.ComboBox();
            this.cbNameSpace = new System.Windows.Forms.ComboBox();
            this.cbClassName = new System.Windows.Forms.ComboBox();
            this.txtConfigName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFiles = new System.Windows.Forms.TextBox();
            this.btnNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(101, 398);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(117, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 25;
            this.label6.Text = "类名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "job名称：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(93, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "名称空间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "job组名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "trigger名称：";
            // 
            // txtTriggerName
            // 
            this.txtTriggerName.Location = new System.Drawing.Point(190, 115);
            this.txtTriggerName.Name = "txtTriggerName";
            this.txtTriggerName.Size = new System.Drawing.Size(200, 21);
            this.txtTriggerName.TabIndex = 21;
            this.txtTriggerName.Text = "trigger1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "trigger组名称：";
            // 
            // txtJobName
            // 
            this.txtJobName.Location = new System.Drawing.Point(190, 88);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(200, 21);
            this.txtJobName.TabIndex = 23;
            this.txtJobName.Text = "job1";
            // 
            // txtCron
            // 
            this.txtCron.Location = new System.Drawing.Point(190, 57);
            this.txtCron.Name = "txtCron";
            this.txtCron.ReadOnly = true;
            this.txtCron.Size = new System.Drawing.Size(200, 21);
            this.txtCron.TabIndex = 30;
            this.txtCron.DoubleClick += new System.EventHandler(this.txtCron_DoubleClick);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(117, 61);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(41, 12);
            this.lblTime.TabIndex = 16;
            this.lblTime.Text = "策略：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(117, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "类型：";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(190, 28);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(200, 20);
            this.cbType.TabIndex = 32;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(190, 57);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(200, 21);
            this.dtpTime.TabIndex = 33;
            this.dtpTime.Visible = false;
            // 
            // cbJobGroup
            // 
            this.cbJobGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbJobGroup.FormattingEnabled = true;
            this.cbJobGroup.Location = new System.Drawing.Point(190, 169);
            this.cbJobGroup.Name = "cbJobGroup";
            this.cbJobGroup.Size = new System.Drawing.Size(200, 20);
            this.cbJobGroup.TabIndex = 34;
            // 
            // cbTriggerGroup
            // 
            this.cbTriggerGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTriggerGroup.FormattingEnabled = true;
            this.cbTriggerGroup.Location = new System.Drawing.Point(190, 203);
            this.cbTriggerGroup.Name = "cbTriggerGroup";
            this.cbTriggerGroup.Size = new System.Drawing.Size(200, 20);
            this.cbTriggerGroup.TabIndex = 35;
            // 
            // cbNameSpace
            // 
            this.cbNameSpace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNameSpace.FormattingEnabled = true;
            this.cbNameSpace.Location = new System.Drawing.Point(190, 234);
            this.cbNameSpace.Name = "cbNameSpace";
            this.cbNameSpace.Size = new System.Drawing.Size(200, 20);
            this.cbNameSpace.TabIndex = 35;
            this.cbNameSpace.SelectedIndexChanged += new System.EventHandler(this.cbNameSpace_SelectedIndexChanged);
            // 
            // cbClassName
            // 
            this.cbClassName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClassName.FormattingEnabled = true;
            this.cbClassName.Location = new System.Drawing.Point(190, 265);
            this.cbClassName.Name = "cbClassName";
            this.cbClassName.Size = new System.Drawing.Size(200, 20);
            this.cbClassName.TabIndex = 36;
            // 
            // txtConfigName
            // 
            this.txtConfigName.Location = new System.Drawing.Point(190, 142);
            this.txtConfigName.Name = "txtConfigName";
            this.txtConfigName.Size = new System.Drawing.Size(200, 21);
            this.txtConfigName.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(93, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "配置名称：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 294);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 37;
            this.label8.Text = "附件：";
            // 
            // txtFiles
            // 
            this.txtFiles.Location = new System.Drawing.Point(190, 291);
            this.txtFiles.Multiline = true;
            this.txtFiles.Name = "txtFiles";
            this.txtFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFiles.Size = new System.Drawing.Size(200, 80);
            this.txtFiles.TabIndex = 38;
            this.txtFiles.DoubleClick += new System.EventHandler(this.txtFiles_DoubleClick);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(237, 398);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 39;
            this.btnNo.Text = "取消";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // AddJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 462);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtFiles);
            this.Controls.Add(this.cbClassName);
            this.Controls.Add(this.cbNameSpace);
            this.Controls.Add(this.cbTriggerGroup);
            this.Controls.Add(this.cbJobGroup);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCron);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtConfigName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTriggerName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtJobName);
            this.Name = "AddJob";
            this.Text = "添加任务";
            this.Load += new System.EventHandler(this.AddJob_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTriggerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label lblTime;
        public System.Windows.Forms.TextBox txtCron;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.ComboBox cbJobGroup;
        private System.Windows.Forms.ComboBox cbTriggerGroup;
        private System.Windows.Forms.ComboBox cbNameSpace;
        private System.Windows.Forms.ComboBox cbClassName;
        private System.Windows.Forms.TextBox txtConfigName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFiles;
        private System.Windows.Forms.Button btnNo;
    }
}