using DJS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public partial class DataList : Form
    {
        //注册改变事件
        private BLL.DatasListen datasListen = new BLL.DatasListen();
        public DataList()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 获取数据文件夹
        /// </summary>
        private static string PATH = ConfigHelp.DataPathPath;

        #region 窗体加载事件 -void DataList_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataList_Load(object sender, EventArgs e)
        { 
            BindDataS();
        }
        #endregion

        #region 显示按钮 -void btnShow_Click(object sender, EventArgs e)
        /// <summary>
        /// 显示按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            string names = cbDatas.Text;
            AddTab(names);
            if (tabShow.TabPages != null && tabShow.TabPages.Count == 1)
            {
                tabShow_SelectedIndexChanged(sender,e);
            }
        }
        #endregion

        #region 关闭按钮 -void btnClose_Click(object sender, EventArgs e)
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (tabShow.SelectedTab != null)
            {
                TabPage page = tabShow.SelectedTab;
                tabShow.TabPages.Remove(page);
            }
        }
        #endregion

        #region 文件改变事件 -void OnChange_ListenDatas(object sender, EventArgs e)
        /// <summary>
        /// 文件改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChange_ListenDatas(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (tabShow.SelectedTab != null)
            {
                string names = tabShow.SelectedTab.Text;
                ShowTxt(names);
            }
        }
        #endregion

        #region 绑定下拉框 -void BindDataS()
        /// <summary>
        /// 绑定下拉框
        /// </summary>
        private void BindDataS()
        {
            ArrayList arry = Common.FileHelp.GetDirectoryList(PATH);
            cbDatas.DataSource = arry;
            cbDatas.DisplayMember = "Name";
        }
        #endregion

        #region 添加tab +void AddTab(string names)
        /// <summary>
        /// 添加tab
        /// </summary>
        private void AddTab(string names)
        {

            if (!IsExistTab(names))
            {
                TabPage page = new TabPage(names); //建立新的tabpage 
                page.Text = names;
                page.Name = names;
                TextBox txtshow = new TextBox();
                txtshow.Name = "txtshow";
                txtshow.Multiline = true;
                txtshow.Dock = DockStyle.Fill;
                page.Controls.Add(txtshow);
                tabShow.TabPages.Add(page);
                tabShow.SelectedTab = page;
            }
        }
        #endregion

        #region 判断选项卡是否存在 +bool IsExistTab(string names)
        /// <summary>
        /// 判断选项卡是否存在
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        private bool IsExistTab(string names)
        {
            bool have = false;
            if (tabShow.TabPages != null && tabShow.TabPages.Count > 0)
            {
                foreach (TabPage tabpage in tabShow.TabPages)
                {
                    if (tabpage.Name == names)
                    {
                        have = true;
                        this.tabShow.SelectedTab = tabpage;
                    }
                }
            }
            return have;
        }
        #endregion

        #region 显示文本 +void ShowTxt(string names)
        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="names"></param>
        private void ShowTxt(string names)
        {
            try
            {
                if (tabShow.SelectedTab != null)
                {
                    string paths = PATH + @"\" + names;
                    ArrayList files = Common.FileHelp.GetFileslist(paths);
                    files.Sort();
                    string file = files[files.Count - 1].ToString();
                    paths += @"\" + file;
                    TabPage page = tabShow.SelectedTab;
                    TextBox txtShow = (TextBox)page.Controls.Find("txtShow", false).FirstOrDefault();
                    string strs = Common.FileHelp.ReadTxtFileNumE(paths, 20);
                    txtShow.Text = strs; 
                    BLL.DatasListen.LastMD5 = Common.SecurityHelp.securityHelp.GetMD5HashFromFile(paths);
                }
            }
            catch (Exception ex)
            {
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }

        }
        #endregion

        #region 选项切换事件 -void tabShow_SelectedIndexChanged(object sender, EventArgs e)
        /// <summary>
        /// 选项切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabShow.SelectedTab != null)
            {
                string names = tabShow.SelectedTab.Text;
                BindListen(names);
            }
        }
        #endregion

        #region 绑定监听事件 -void BindListen(string names)
        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="names"></param>
        private void BindListen(string names)
        {
            try
            {
                datasListen.DelJobsChangeHandler(OnChange_ListenDatas);

                string paths = PATH + @"\" + names;
                ArrayList files = Common.FileHelp.GetFileslist(paths);
                if (files != null && files.Count > 0)
                {
                    files.Sort();
                    string file = files[files.Count - 1].ToString();
                    paths += @"\" + file;
                    BLL.DatasListen.PATH = paths;
                }

                datasListen.AddDatasChangeHandler(OnChange_ListenDatas);
            }
            catch (Exception ex)
            {
                //日志
                DJS.Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
        }
        #endregion

    }
}
