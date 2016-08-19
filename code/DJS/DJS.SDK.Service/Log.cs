using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.SDK.Service
{
    public class Log : ILog
    { 
        #region 写入日志 +void WriteLog(string msg, int type)
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public void WriteLog(string msg, int type)
        {
            try
            {
                Common.LogHelp.logHelp.WriteLog(msg, type);
            }
            catch (Exception ex)
            {
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
        }
        #endregion

        #region 回写数据 +void WirteDatas(string namespaces, string str)
        /// <summary>
        /// 回写数据
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <param name="str">数据</param>
        public void WirteDatas(string namespaces, string str)
        {
            try
            {
                Common.FileHelp.WirteDatas(namespaces, str);
            }
            catch (Exception ex)
            {
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
        }
        #endregion
         
    }
}
