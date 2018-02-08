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
        public string IDS = "";
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string PATH = ConfigHelp.AssemblySrcPath;
        private static string DLLMGR_KEY = Common.RedisConfigHelp.redisConfigHelp.GetRedisKeyByName("DLLMgr_K");

        private FileInfo FILE = null;
        public DLLUpdate()
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
                if (ofdUpload.ShowDialog() == DialogResult.OK)
                {
                    FILE = new FileInfo(ofdUpload.FileName);
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
                Guid Id = Guid.Empty;
                if (Guid.TryParse(IDS, out Id))
                {
                    Model.Jobs jobModel = new Model.Jobs();
                    jobModel = BLL.Jobs.GetModelById(IDS);
                    if (jobModel != null)
                    {
                        Model.DllMgr model = new Model.DllMgr();
                        model = BLL.DllMgr.GetModelById(jobModel.DLLID);

                        if (FILE.Exists)
                        {
                            string fileNamePaths = model.Url;
                            //文件存在时先删除
                            if (FileHelp.FileExists(fileNamePaths))
                            {
                                FileHelp.DeleteFiles(fileNamePaths);
                            }
                            FILE.CopyTo(fileNamePaths);
                            MessageBox.Show("升级成功！");
                            ReAdd(IDS);
                            this.Close();
                        }

                    }
                }
                else
                {
                    MessageBox.Show("升级失败，请重试！");
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

        #region 重新添加
        /// <summary>
        /// 重新添加
        /// </summary>
        /// <param name="Id"></param>
        private void ReAdd(string Id)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要重新添加吗?", "重新添加", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                //任务已经存在时
                if (BLL.Jobs.IsExistJobkeyById(Id))
                {
                    MessageBoxButtons messButtonKey = MessageBoxButtons.OKCancel;
                    DialogResult drKey = MessageBox.Show("任务已经存在，是否继续重新添加?", "重新添加", messButtonKey);
                    if (drKey == DialogResult.OK)//如果点击“确定”按钮
                    {
                        if (!BLL.Jobs.DelByIdForQuartz(Id))
                        {
                            MessageBox.Show("删除Quartz中任务失败！");
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                Model.Jobs model = BLL.Jobs.GetModelById(Id);

                if (BLL.Jobs.ReAddJobs(model))
                {
                    MessageBox.Show("重新添加成功");
                }
                else
                {
                    MessageBox.Show("重新添加失败");
                }
            }
            else
            {
                MessageBox.Show("重新添加失败");
            }
        }
        
        #endregion
    }
}
