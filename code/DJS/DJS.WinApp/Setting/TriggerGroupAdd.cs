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
    public partial class TriggerGroupAdd : Form
    {
        private static string TRIGGERGROUP_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("TriggerGroup_K");
        public TriggerGroupAdd()
        {
            InitializeComponent();
        }

        #region 确认按钮点击事件 -void btnOk_Click(object sender, EventArgs e)
        /// <summary>
        /// 确认按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            decimal nos = nudNo.Value;
            string names = txtName.Text; 
            //判断名称是否存在
            if (BLL.TriggerGroup.IsExist(names))
            {
                MessageBox.Show("组名称已经存在，请重新输入！");
            }
            else
            {
                List<Model.TriggerGroup> models = new List<Model.TriggerGroup>();
                models = Common.RedisHelp.redisHelp.Get<List<Model.TriggerGroup>>(TRIGGERGROUP_KEY);
                if (models == null)
                {
                    models = new List<Model.TriggerGroup>();
                }
                Model.TriggerGroup group = new Model.TriggerGroup();
                group.ID = Guid.NewGuid();
                group.No = nos;
                group.Name = names;
                models.Add(group);
                bool ret = Common.RedisHelp.redisHelp.Set<List<Model.TriggerGroup>>(TRIGGERGROUP_KEY, models);
                if (ret)
                {
                    MessageBox.Show("添加成功");
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
            }
        }
        #endregion

        #region 取消按钮点击事件 -void btnNo_Click(object sender, EventArgs e)
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
