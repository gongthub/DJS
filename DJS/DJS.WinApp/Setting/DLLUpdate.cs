using DJS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp.Setting
{
    public partial class DLLUpdate : Form
    {
        public Guid JOBIDS = Guid.Empty;
        private FileInfo FILE = null;
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string PATH = ConfigHelp.AssemblySrcPath;
        private static string DLLMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("DLLMgr_K");

        public DLLUpdate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLLUpdate_Load(object sender, EventArgs e)
        {
            if (JOBIDS != Guid.Empty)
            {
                Model.Jobs jobModel = BLL.Jobs.GetModelById(JOBIDS);
                if (jobModel != null && jobModel.DLLID != null)
                {
                    Model.DllMgr dllModel = BLL.DllMgr.GetModelById(jobModel.DLLID);
                    if (dllModel != null)
                    {
                        nudNo.Value = dllModel.No;
                        txtName.Text = dllModel.Name;
                        txtNameSpace.Text = dllModel.NameSpace;
                        nudNo.Enabled = false;
                        txtName.Enabled = false;
                        txtNameSpace.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// 上传双击事件
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
                if (JOBIDS != Guid.Empty)
                {
                    Model.Jobs jobModel = BLL.Jobs.GetModelById(JOBIDS);
                    if (jobModel != null && jobModel.DLLID != null)
                    {

                        Model.DllMgr dllModel = BLL.DllMgr.GetModelById(jobModel.DLLID);
                        if (dllModel != null)
                        { //文件存在时先删除
                            if (FileHelp.FileExists(FileHelp.GetFullPath(dllModel.Url)))
                            {
                                BLL.Jobs.DelByIdForQuartz(JOBIDS);
                                FileHelp.DeleteFiles(FileHelp.GetFullPath(dllModel.Url));
                            }
                            FILE.CopyTo(FileHelp.GetFullPath(dllModel.Url));
                        }
                        BLL.Jobs.UpdateByIdForQuartz(JOBIDS);
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
