using DJS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        /// <summary>
        /// 获取程序集文件所在文件夹名称
        /// </summary>
        private static string PATH = ConfigHelp.AssemblySrcPath;

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
            //iConfigMgr.SetConfig("TestConfig", "test");
            //iConfigMgr.SetConfig("TestConfig123", "123");
            //iConfigMgr.GetConfig("TestConfig");
            //bool b = iConfigMgr.IsExist("TestConfig"); 
            //if (ofdUpLoad.ShowDialog() == DialogResult.OK)
            //{
            //    string[] names = ofdUpLoad.FileNames;
            //    for (int i = 0; i < names.Length; i++)
            //    {
            //        Upload(names[i],"test");
            //    }
            //}
        }


        public void Upload(string filename,string name)
        {
            FileInfo FILE = new FileInfo(filename);
            if (FILE.Exists)
            {
                string paths = PATH + @"\" + name + @"\";
                if (!DJS.Common.FileHelp.DirectoryIsExists(paths))
                {
                    DJS.Common.FileHelp.CreateDirectory(paths);
                }
                string fileNamePaths = PATH + @"\" + name + @"\" + FILE.Name;
                //文件存在时先删除
                if (FileHelp.FileExists(fileNamePaths))
                {
                    FileHelp.DeleteFiles(fileNamePaths);
                }
                FILE.CopyTo(PATH + @"\" + name + @"\" + FILE.Name);
                 
            }
        }


    }
}
