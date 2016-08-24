using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static DJS.SDK.ILog iLog = null;
        private static DJS.SDK.IConfigMgr iConfigMgr = null;

        #endregion

        public Form1()
        {
            InitializeComponent();
            iLog = DJS.SDK.DataAccess.CreateILog(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string names = iLog.GetConfigNameByJobName("job1");
            iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr(names);
            iConfigMgr.SetConfig("TestConfig", "test");
            //iConfigMgr.SetConfig("TestConfig123", "123");
            //iConfigMgr.GetConfig("TestConfig");
            //bool b = iConfigMgr.IsExist("TestConfig");
        }
    }
}
