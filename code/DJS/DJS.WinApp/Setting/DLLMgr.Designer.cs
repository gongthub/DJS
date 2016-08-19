namespace DJS.WinApp
{
    partial class DLLMgr
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtDLLName = new System.Windows.Forms.TextBox();
            this.lblJobName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDLLLists = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDLLLists)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(414, 18);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(517, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "开始刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(311, 18);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtDLLName
            // 
            this.txtDLLName.Location = new System.Drawing.Point(87, 19);
            this.txtDLLName.Name = "txtDLLName";
            this.txtDLLName.Size = new System.Drawing.Size(150, 21);
            this.txtDLLName.TabIndex = 1;
            // 
            // lblJobName
            // 
            this.lblJobName.AutoSize = true;
            this.lblJobName.Location = new System.Drawing.Point(18, 23);
            this.lblJobName.Name = "lblJobName";
            this.lblJobName.Size = new System.Drawing.Size(29, 12);
            this.lblJobName.TabIndex = 0;
            this.lblJobName.Text = "名称";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Controls.Add(this.btnQuery);
            this.groupBox2.Controls.Add(this.txtDLLName);
            this.groupBox2.Controls.Add(this.lblJobName);
            this.groupBox2.Location = new System.Drawing.Point(12, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 54);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // dgvDLLLists
            // 
            this.dgvDLLLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDLLLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDLLLists.Location = new System.Drawing.Point(-12, 1);
            this.dgvDLLLists.Name = "dgvDLLLists";
            this.dgvDLLLists.RowTemplate.Height = 23;
            this.dgvDLLLists.Size = new System.Drawing.Size(784, 295);
            this.dgvDLLLists.TabIndex = 1;
            this.dgvDLLLists.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDLLLists_DataBindingComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgvDLLLists);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 297);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // DLLMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DLLMgr";
            this.Text = "DLL配置";
            this.Load += new System.EventHandler(this.DLLMgr_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDLLLists)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtDLLName;
        private System.Windows.Forms.Label lblJobName;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.DataGridView dgvDLLLists;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}