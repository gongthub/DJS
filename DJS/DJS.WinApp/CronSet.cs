using DJS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]//com+可见
    public partial class CronSet : Form
    {
        private static string CRONURL = "";
        public CronSet()
        {
            InitializeComponent();
            //Cron 路径
            CRONURL = ConfigHelp.CronUrlPath;
            CRONURL = Common.FileHelp.GetFullPath(CRONURL);
        }

        private void CronSet_Load(object sender, EventArgs e)
        {
            wbCronShow.ScriptErrorsSuppressed = true;
            wbCronShow.ObjectForScripting = this;//具体公开的对象,这里可以公开自定义对象
            wbCronShow.Navigate(CRONURL);
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            wbCronShow.Document.InvokeScript("GetCron");
        }

        /// <summary>
        /// 获取cron
        /// </summary>
        /// <param name="message"></param>
        public void GetCronText(string message)
        {
            AddJob addJob = (AddJob)this.Owner;
            addJob.txtCron.Text = message;
            this.Close();
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
