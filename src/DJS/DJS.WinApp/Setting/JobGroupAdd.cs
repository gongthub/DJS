using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class JobGroupAdd : Form
    {

        private  DJS.WinApp.JobGroupMgr.SetdataHandler M_BINDLIST;
        public JobGroupAdd(DJS.WinApp.JobGroupMgr.SetdataHandler setdata)
        {
            InitializeComponent();
            this.M_BINDLIST = setdata;
        }

        #region 窗体加载事件 +void JobGroupAdd_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobGroupAdd_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region 确认按钮点击事件 +void btnOk_Click(object sender, EventArgs e)
        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {

            int nos = 0;
            int.TryParse(nudNo.Value.ToString(), out nos);
            string names = txtName.Text;

            //判断名称是否存在
            if (BLL.JobGroup.IsExist(names))
            {
                MessageBox.Show("组名称已经存在，请重新输入！");
            }
            else
            {
                Model.JobGroup group = new Model.JobGroup();
                group.ID = Guid.NewGuid().ToString();
                group.SortCode = nos;
                group.Name = names;
                bool ret = DJS.BLL.JobGroup.AddForm(group);
                if (ret)
                {
                    MessageBox.Show("添加成功"); 
                    this.Dispose();
                    if (this.M_BINDLIST != null)
                    {
                        this.M_BINDLIST();
                    }
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
            }
        }
        #endregion

        #region 取消按钮点击事件 +void btnNo_Click(object sender, EventArgs e)
        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
