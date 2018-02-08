using DJS.SDK;
using Quartz;
using RentLoanFinancialService.Service;
using RentLoanFinancialService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService
{
    public class Achieve:IJob,IConfigClient
    {
        public static IConfigMgr iConfigMgr = null;
        public static ILog ilog = null;

        static Achieve()
        {
            ilog = DataAccess.CreateILog();
        }

        public void Execute(IJobExecutionContext context)
        {
            SetConfig(context.JobDetail.Key.Name);
            DateTime times = DateTime.Now;
            ilog.WriteLog(context.JobDetail.Key.Name + "接口调用开始！", 0);
            IRentLoanFinancialService rentLoanFinancialService = new RentLoanFinancialService.Service.RentLoanFinancialService();
            rentLoanFinancialService.PullRentLoanFinancialData();
            ilog.WriteLog(context.JobDetail.Key.Name + "接口调用结束！", 0);
            DateTime timee = DateTime.Now;
            TimeSpan travelTime = timee - times;
            ilog.WriteLog(context.JobDetail.Key.Name + "接口调用耗时:" + travelTime, 0);

        }

        public void SetConfig(string jobName)
        {
            string configName = ilog.GetConfigNameByJobName(jobName);
            if (!string.IsNullOrEmpty(configName))
            {
                iConfigMgr = DataAccess.CreateIConfigMgr(configName);

                iConfigMgr.SetConfig("IDBRepositoy", "Data Source=.;User Id=sa;Password=1234567890;Database=AMS;");
                iConfigMgr.SetConfig("crmURL", "http://sitwebapi.vlinker.com.cn:8888");
                iConfigMgr.SetConfig("APIUSERID", "10001");
                iConfigMgr.SetConfig("SIGNATURE", "7c6d977fa475444086f47687f1b8c2ea");
                iConfigMgr.SetConfig("PullStartDateTime", DateTime.Now.Date.ToString());
            }
            else
            {
                ilog.WriteLog("获取任务配置名称错误！"+jobName,1,true);
            }
            ConstUtility consts = new ConstUtility();
        }

        public void ExecuteT(string name)
        {
            SetConfig(name);
            IRentLoanFinancialService rentLoanFinancialService = new RentLoanFinancialService.Service.RentLoanFinancialService();
            rentLoanFinancialService.BankLoanDetail();
        }
    }
}
