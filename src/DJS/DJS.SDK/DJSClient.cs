using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.SDK
{
    public abstract class DJSClient : DJSClientBase, IConfigClient
    {
        public static DJS.SDK.IConfigMgr iConfigMgr = null;
        public void SetConfig(string jobName)
        {
            try
            {
                SetConfigs(jobName);
            }
            catch (Exception e)
            {
                iLog.WriteLog(jobName + "配置失败--SYSTEM！" + e.Message, 0);
                throw e;
            }
        }

        public virtual void SetConfigs(string jobName)
        {
            string configName = iLog.GetConfigNameByJobName(jobName);
            if (!string.IsNullOrEmpty(configName))
            {
                iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr(configName);
            }

        }
    }
}
