using DJS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DJS.WinApp.Setting
{
    public partial class JobConfig : Form
    {
        public string IDS = "";

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string SDKCONFIGPath = ConfigHelp.SDKCONFIGPath;

        /// <summary>
        /// 节点
        /// </summary>
        private static string CONFIGSPATH = @"/CONFIGS";
        public JobConfig()
        {
            InitializeComponent();
            CONFIGSPATH = @"/CONFIGS";
        }

        #region 窗体加载事件 -void JobConfig_Load(object sender, EventArgs e)
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobConfig_Load(object sender, EventArgs e)
        {
            XmlNodeList nodes = GetNodesById();
            if (nodes != null && nodes.Count > 0)
            {
                int xnum = 0;
                int ynum = 0;
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["Name"] != null)
                    {
                        string names = node.Attributes["Name"].Value.ToString();
                        string vals = node.Attributes["Value"].Value.ToString();

                        Panel pan = new Panel();
                        Label lbl = new Label();
                        TextBox txtbox = new TextBox();

                        lbl.Text = names;
                        lbl.SetBounds(0, 10, 120, 30);

                        txtbox.Name = names;
                        txtbox.Text = vals;
                        txtbox.SetBounds(120, 6, panlShow.Width - 120, 30);

                        pan.Controls.Add(lbl);
                        pan.Controls.Add(txtbox);
                        pan.SetBounds(xnum, ynum, panlShow.Width, 30);

                        panlShow.Controls.Add(pan);

                        ynum += 30;
                    }
                }
            }
        }
        #endregion

        #region 取消按钮 -void btnNo_Click(object sender, EventArgs e)
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 保存按钮 -void btnOK_Click(object sender, EventArgs e)
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> nameslist = new List<string>();
                XmlNodeList nodes = GetNodesById(out nameslist);
                List<Control> controls = GetAllControls(this);
                if (controls != null)
                {
                    Guid Id = Guid.Empty;
                    if (Guid.TryParse(IDS, out Id))
                    {
                        Model.Jobs model = BLL.Jobs.GetModelById(Id);
                        if (model != null)
                        {
                            string cponfignames = CONFIGSPATH + @"/" + model.ConfigName + @"/" + "CONFIG";
                            foreach (Control control in controls)
                            {
                                if (nameslist.Contains(control.Name))
                                {
                                    XmlHelp.xmlHelp.SetValue(SDKCONFIGPath, cponfignames, "Name", control.Name, "Value", control.Text);
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！"+ex.Message);
            }

        }
        #endregion

        #region 根据id获取节点 -XmlNodeList GetNodesById()
        /// <summary>
        /// 根据id获取节点
        /// </summary>
        /// <returns></returns>
        private XmlNodeList GetNodesById()
        {
            List<string> nameslist = new List<string>();
            XmlNodeList nodes = null;
            Guid Id = Guid.Empty;
            if (Guid.TryParse(IDS, out Id))
            {
                Model.Jobs model = BLL.Jobs.GetModelById(Id);
                if (model != null)
                {
                    string cponfignames = CONFIGSPATH + @"/" + model.ConfigName + @"/" + "CONFIG";
                    nodes = Common.XmlHelp.xmlHelp.GetNodes(SDKCONFIGPath, cponfignames); 
                }
            } 
            return nodes;

        }
        /// <summary>
        /// 根据id获取节点
        /// </summary>
        /// <returns></returns>
        private XmlNodeList GetNodesById(out List<string> names)
        {
            List<string> nameslist = new List<string>();
            XmlNodeList nodes = null;
            Guid Id = Guid.Empty;
            if (Guid.TryParse(IDS, out Id))
            {
                Model.Jobs model = BLL.Jobs.GetModelById(Id);
                if (model != null)
                {
                    string cponfignames = CONFIGSPATH + @"/" + model.ConfigName + @"/" + "CONFIG";
                    nodes = Common.XmlHelp.xmlHelp.GetNodes(SDKCONFIGPath, cponfignames);
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes["Name"] != null)
                        {
                            string name = node.Attributes["Name"].Value;
                            nameslist.Add(name);
                        }

                    }
                }
            }
            names = nameslist;
            return nodes;

        }
        #endregion

        #region 获取所有控件 + List<Control> GetAllControls(Control control)
        /// <summary>
        /// 获取所有控件
        /// </summary>
        /// <param name="control"></param>
        public List<Control> GetAllControls(Control control)
        {
            List<Control> controls = new List<Control>();
            foreach (Control con in control.Controls)
            {
                controls.Add(con);
                if (con.Controls.Count > 0)
                {
                    GetAllControlsN(con, controls); 
                }
            }
            return controls;
        }
        /// <summary>
        /// 获取所有控件
        /// </summary>
        /// <param name="control"></param>
        public List<Control> GetAllControlsN(Control control, List<Control> controls)
        { 
            foreach (Control con in control.Controls)
            {
                controls.Add(con);
                if (con.Controls.Count > 0)
                {
                    GetAllControlsN(con, controls);
                }
            }
            return controls;
        } 
        #endregion


    }
}
