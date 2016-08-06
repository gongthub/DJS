using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class DLLAdd : Form
    {
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string PATH = ConfigurationManager.AppSettings["AssemblySrc"].ToString();
        private static string DLLMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("DLLMgr_K");

        private FileInfo FILE = null;

        public DLLAdd()
        {
            InitializeComponent();
        }
         

        #region url文本框双击事件 -void txtUrl_DoubleClick(object sender, EventArgs e)
        /// <summary>
        /// url文本框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUrl_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ofdUpLoad.ShowDialog() == DialogResult.OK)
                {
                    FILE = new FileInfo(ofdUpLoad.FileName);
                    if (FILE.Exists)
                    {
                        txtUrl.Text = FILE.FullName;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        #endregion

        #region 确定按钮点击事件 -void btnOk_Click(object sender, EventArgs e)
        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                decimal nos = nudNo.Value;
                string names = txtName.Text;

                //判断名称是否存在
                if (BLL.DllMgr.IsExist(names))
                {
                    MessageBox.Show("命名空间已经存在，请重新输入！");
                }
                else
                {
                    List<Model.DllMgr> models = new List<Model.DllMgr>();
                    models = Common.RedisHelp.redisHelp.Get<List<Model.DllMgr>>(DLLMGR_KEY);
                    if (models == null)
                    {
                        models = new List<Model.DllMgr>();
                    }

                    Model.DllMgr model = new Model.DllMgr();
                    model.ID = Guid.NewGuid();
                    model.No = nos;
                    model.NameSpace = names;

                    if (FILE.Exists)
                    {
                        string paths = PATH + @"\" + names + @"\";
                        if (!Common.FileHelp.DirectoryIsExists(paths))
                        {
                            Common.FileHelp.CreateDirectory(paths);
                        }

                        FILE.CopyTo(PATH + @"\" + names + @"\" + ofdUpLoad.SafeFileName);

                        model.Url = PATH + @"\" + names + @"\" + ofdUpLoad.FileName;
                    }
                    models.Add(model);
                    bool ret = Common.RedisHelp.redisHelp.Set<List<Model.DllMgr>>(DLLMGR_KEY, models);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
