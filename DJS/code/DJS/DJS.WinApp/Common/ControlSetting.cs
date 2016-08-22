using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJS.WinApp
{
    public class ControlSetting
    {

        #region 单例模式创建对象
        //单例模式创建对象
        private static ControlSetting _controlSetting = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        ControlSetting()
        {
        }

        public static ControlSetting controlSetting
        {
            get
            {
                // Double-Checked Locking
                if (null == _controlSetting)
                {
                    lock (SynObject)
                    {
                        if (null == _controlSetting)
                        {
                            _controlSetting = new ControlSetting();
                        }
                    }
                }
                return _controlSetting;
            }
        }
        #endregion

        #region 初始化DataGridView +void DataGridViewSet(DataGridView dgv)
        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        /// <param name="dgv"></param>
        public void DataGridViewSet(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(170,212,255);
        } 
        #endregion
    }
}
