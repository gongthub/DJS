using RentLoanService.Model;
using RentLoanService.Services;
using RentLoanService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace RentLoanService.Service
{
    public class RentLoanService
    {
        
        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static RentLoanService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion
        /// <summary>
        /// 租金贷预警
        /// </summary>
        public static void SendRentLoanEmail(int CategoryID, int StoreID, int type)
        {
            try
            {
                //邮件
                if (type == (int)EnumUtility.EmialModel.Emial)
                {
                    List<RentLoanRisk> RiskList = GetRentLoanRisk(StoreID);
                    if (RiskList != null && RiskList.Count > 0)
                    {
                        string storeName = "";
                        StringBuilder sbTable = new StringBuilder();
                        sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                        sbTable.Append("<tr><td>店名</td><td>合同编号</td><td>姓名</td><td>联系电话</td><td>开始时间</td><td>结束时间</td><td>上次缴费覆盖结束日期</td><td>楼栋</td><td>房间号</td><td>风控天数</td><td>状态</td></tr>");
                        foreach (var item in RiskList)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>" + item.StoreName + "</td>");
                            sbTable.Append("<td>" + item.ContractNo + "</td>");
                            sbTable.Append("<td>" + item.Name + "</td>");
                            sbTable.Append("<td>" + item.Phone + "</td>");
                            sbTable.Append("<td>" + item.StartDate.ToShortDateString() + "</td>");
                            sbTable.Append("<td>" + item.EndDate.ToShortDateString() + "</td>");
                            sbTable.Append("<td>" + (item.PEndDate.HasValue ? item.PEndDate.Value.ToShortDateString() : "") + "</td>");
                            sbTable.Append("<td>" + item.BuildingName + "</td>");
                            sbTable.Append("<td>" + item.FullName + "</td>");
                            sbTable.Append("<td>" + item.Risk + "</td>");
                            sbTable.Append("<td>" + GetEnumDescription(typeof(EnumUtility.RLoanStatus), item.Status) + "</td>");
                            sbTable.Append("</tr>");
                            storeName = item.StoreName;
                        }
                        sbTable.Append("</table>");
                        EmailService.SendOverDueEmail(CategoryID, StoreID, storeName + EnumUtility.EmailCategory.租金贷预警.ToString(), sbTable.ToString());//调用邮件方法
                        EmailService.AddEmailLogs(CategoryID, StoreID, (int)EnumUtility.EmailLogStatu.成功, type, "成功");//日志
                    }

                }
                //短信
                if (type == (int)EnumUtility.EmialModel.SMS)
                {
                    //调用短信接口
                }
            }
            catch (Exception e)
            {
                EmailService.AddEmailLogs(CategoryID, StoreID, (int)EnumUtility.EmailLogStatu.失败, type, e.Message);//日志
                iLog.WriteLog("DiaryWarning Error " + e.Message,1);
            }
        }

        /// <summary>
        /// 租金贷预警信息
        /// </summary>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        private static List<RentLoanRisk> GetRentLoanRisk(int StoreID)
        {
            try
            {
                IDBRepository dbContext = new IDBRepository();
                int WarningDate = Convert.ToInt32(ConstUtility.WarningDate);//预警天数
                //List<RentLoanRisk> riskList = dbContext.RentLoanRisks.Where(c => c.StoreID == StoreID && c.Risk <= WarningDate).ToList();
                var riskList = new List<RentLoanRisk>();
                var riskListTemp = dbContext.RentLoanRisks.Where(c => c.StoreID == StoreID).ToList();
                riskListTemp.ForEach(t =>
                {
                    if (t.Status == 100)
                    {
                        riskList.Add(t);
                    }
                    else
                    {
                        if (t.Risk <= WarningDate)
                        {
                            riskList.Add(t);
                        }
                    }
                });

                return riskList;
            }
            catch (Exception e)
            {
                iLog.WriteLog("DiaryWarning Error " + e.Message,1);
                return null;
            }
        }

        /// <summary>
        /// 共用方法--取枚举的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type type, object value)
        {
            string description = "";
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                FieldInfo field = fields[i];

                if (field.Name.Equals(Enum.GetName(type, value)))
                {
                    object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (objs == null || objs.Length == 0)
                    {
                        description = field.Name;
                    }
                    else
                    {
                        DescriptionAttribute da = (DescriptionAttribute)objs[0];
                        description = da.Description;
                    }
                }
            }
            return description;
        }
    }
}
