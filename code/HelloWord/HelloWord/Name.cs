using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWord
{
    public class Name : IJob
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static DJS.SDK.ILog iLog = null;

        #endregion

        #region 构造函数

        static Name()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public string GetNames()
        {
            return "HelloWord!";
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);

                string namespaces = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
                iLog.WirteDatas(namespaces, "HelloWord!");
                iLog.WriteLog("HelloWord执行成功！", 0);
            }
            catch (Exception ex)
            {
                iLog.WriteLog("HelloWord执行失败！" + ex.Message, 1);
            }
        }
    }
}
