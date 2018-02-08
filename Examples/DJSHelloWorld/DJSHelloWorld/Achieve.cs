using DJS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DJSHelloWorld
{
    public class Achieve : DJSClient
    {
        #region 属性
        private const string VERSION = "V4.1";

        #endregion

        #region 构造函数

        static Achieve()
        {
        }

        #endregion


        public override void ExceteJob(string jobName)
        {
            try
            {

                SetConfig(jobName);
                DateTime times = DateTime.Now;

                Thread.Sleep(5000);
                DateTime timee = DateTime.Now;
                TimeSpan travelTime = timee - times;

            }
            catch (Exception ex)
            {
                iLog.WriteLog(jobName + "接口调用失败！" + ex.Message + "---------------版本" + VERSION, 1, true);
            }
        }

        public override void SetConfigs(string jobName)
        {
            base.SetConfigs(jobName);

            iLog.WriteLog(jobName + "配置开始！---------------版本" + VERSION, 0);

            iConfigMgr.SetConfig("ServiceCode", "S013");
            iConfigMgr.SetConfig("SendMailSender", "testadmin@vlinker.com.cn");
            iConfigMgr.SetConfig("SendMailPassword", "Vlinker123");
            iConfigMgr.SetConfig("EmailSMTP", "smtp.exmail.qq.com");
            iConfigMgr.SetConfig("IDBRepositoryEmailService", "Data Source=.;User Id=sa;Password=111111;Database=AMS_SIT;");

            //未申请，待提交，审核未通过，待审核风控天数小于45天
            iConfigMgr.SetConfig("WarningDate", "45");
            //逾期未还款，通过未放款，已放款，正常还款风控天数小于5天
            iConfigMgr.SetConfig("MinWarningDate", "5");
            iLog.WriteLog(jobName + "配置结束！---------------版本" + VERSION, 0);
        }
    }
}
