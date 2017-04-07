using RentLoanSummaryService.Model;
using RentLoanSummaryService.Services;
using RentLoanSummaryService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using RentLoanService.Model;

namespace RentLoanSummaryService.Service
{
    public class RentLoanSummaryService
    {

        public static DJS.SDK.ILog iLog = null;

        private static int WarningDate = 0;
        private static int NotApply = 0;
        private static int NotAudit = 0;
        private static int AuditOK = 0;
        private static int OverdueNotRepay = 0;
        //private static int WaitIntoPiece = 0;
        private static int WaitSubmit = 0;
        private static int NormalRepayment = 0;
        private static string SystemName = null;

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static RentLoanSummaryService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            WarningDate = Convert.ToInt32(ConstUtility.WarningDate);
            NotApply = Convert.ToInt32(ConstUtility.NotApply);
            NotAudit = Convert.ToInt32(ConstUtility.NotAudit);
            AuditOK = Convert.ToInt32(ConstUtility.AuditOK);
            OverdueNotRepay = Convert.ToInt32(ConstUtility.OverdueNotRepay);
            //WaitIntoPiece = Convert.ToInt32(ConstUtility.WaitIntoPiece);
            WaitSubmit = Convert.ToInt32(ConstUtility.WaitSubmit);
            NormalRepayment = Convert.ToInt32(ConstUtility.NormalRepayment);
            SystemName = ConstUtility.SystemName;
        }

        #endregion


        #region 每个租金贷预警数据汇总
        /// <summary>
        /// 获取所有租金贷汇总个数信息
        /// </summary>
        /// <param name="storeentityList">需要查询的所有的社区</param>
        /// <param name="arryIds">所有的社区编号</param>
        /// <returns></returns>
        public static List<StoresRentLoad> GetRentLoadEnt(List<Stores> storeentityList, List<int> arryIds)
        {
            List<StoresRentLoad> entityList = new List<StoresRentLoad>();
            List<RentLoanRisk> RiskList = GetRentLoanRisk(arryIds);//查询所有社区的所有租金贷预警信息

            foreach (var storeentity in storeentityList)//根据社区列表将指定社区的租金贷预警数量统计出来
            {
                var RiskLoans = RiskList.Where(a => a.StoreID == storeentity.Id).ToList();
                if (RiskLoans != null && RiskLoans.Count() > 0)
                {
                    StoresRentLoad entity = new StoresRentLoad();
                    entity.StoreId = storeentity.Id;
                    entity.StoreName = storeentity.Name;
                    entity.RedWarnings = RiskLoans.Where(item => item.DayCountLive < 0).Count();
                    entity.OrangeWarnings = RiskLoans.Where(item => (item.DayCountLive >= 0 && item.DayCountLive <= 15)).Count();
                    entity.YellowWarnings = RiskLoans.Where(item => (item.DayCountLive >= 16 && item.DayCountLive <= 30)).Count();
                    entity.BlueWarnings = RiskLoans.Where(item => (item.DayCountLive >= 31 && item.DayCountLive <= 45)).Count();
                    entity.NormalWarnings = RiskLoans.Where(item => item.DayCountLive > 45).Count();
                    entity.Description = "";
                    entityList.Add(entity);//统计指定社区的租金贷预警信息
                }
            }
            return entityList;
        }
        public static void SendRentLoanEmail(List<StoresRentLoad> entityList, int emailId, int CategoryID, int StoreID, int type)
        {
            try
            {
                //邮件
                if (type == (int)EnumUtility.EmialModel.Emial && entityList.ToList().Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    var r1 = 0;
                    var o1 = 0;
                    var y1 = 0;
                    var b1 = 0;
                    var n1 = 0;
                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append("<tr><td style='background: #F2F2F2;min-width:170px;'>门店</td>");
                    sbTable.Append("<td style='background: #FF0000;min-width:150px;'>红色预警：负数日期</td>");
                    sbTable.Append("<td style='background: #FFC000;min-width:150px;'>橙色预警：≤15天</td>");
                    sbTable.Append("<td style='background: #FFFF00;min-width:150px;'>黄色预警：16-30天</td>");
                    sbTable.Append("<td style='background: #00B0F0;min-width:150px;'>蓝色预警：31-45天</td>");
                    sbTable.Append("<td style='min-width:150px;'>其他：>45天</td>");
                    sbTable.Append("<td style='min-width:50px;'>备注</td></tr>");

                    foreach (var item in entityList)
                    {
                        sbTable.Append("<tr><td>" + item.StoreName + "</td> <td>" + item.RedWarnings + "</td> <td>" + item.OrangeWarnings + "</td> <td>" + item.YellowWarnings + "</td> <td>" + item.BlueWarnings + "</td> <td>" + item.NormalWarnings + "</td> <td>" + item.Description + "</td> </tr>");
                        r1 += item.RedWarnings;
                        o1 += item.OrangeWarnings;
                        y1 += item.YellowWarnings;
                        b1 += item.BlueWarnings;
                        n1 += item.NormalWarnings;
                    }

                    sbTable.Append("<tr><td>汇总</td> <td>" + r1 + "</td> <td>" + o1 + "</td> <td>" + y1 + "</td> <td>" + b1 + "</td> <td>" + n1 + "</td> <td></td> </tr>");
                    sbTable.Append("</table>");

                    EmailService.SendOverDueEmail(emailId, CategoryID, StoreID, SystemName, sbTable.ToString());//调用邮件方法
                    EmailService.AddEmailLogs(CategoryID, StoreID, (int)EnumUtility.EmailLogStatu.成功, type, "成功");//日志
                }
                //短信
                if (type == (int)EnumUtility.EmialModel.SMS)
                {
                    //调用短信接口
                }

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取所有的租金贷信息
        /// </summary>
        /// <param name="arryIds"></param>
        /// <returns></returns>
        private static List<RentLoanRisk> GetRentLoanRisk(List<int> arryIds)
        {
            try
            {
                IDBRepository dbContext = new IDBRepository();
                DateTime nowTime = DateTime.Now.Date;

                List<RentLoanRisk> riskList = dbContext.RentLoanRisks.Where(c => arryIds.Contains(c.StoreID) && c.DayCountLive <= WarningDate && c.Status != (int)EnumUtility.RLoanStatus.正常还款).ToList();

                var riskListTemp = dbContext.RentLoanRisks.Where(c => arryIds.Contains(c.StoreID)).ToList();
                riskListTemp.ForEach(t =>
                {
                    if (!riskList.Contains(t))
                    {
                        int interval = new TimeSpan(nowTime.Ticks - t.ModifiedDate.Date.Ticks).Days;
                        if (t.Status == (int)EnumUtility.RLoanStatus.未申请租金贷 && interval >= NotApply)
                        {
                            riskList.Add(t);
                        }
                        else if (t.Status == (int)EnumUtility.RLoanStatus.待提交 && interval >= WaitSubmit)
                        {
                            riskList.Add(t);
                        }
                        //else if (t.Status == (int)EnumUtility.RLoanStatus.待进件 && interval >= WaitIntoPiece)
                        //{
                        //    riskList.Add(t);
                        //}
                        else if (t.Status == (int)EnumUtility.RLoanStatus.待审核 && interval >= NotAudit)
                        {
                            riskList.Add(t);
                        }
                        else if (t.Status == (int)EnumUtility.RLoanStatus.审核通过 && interval >= AuditOK)
                        {
                            riskList.Add(t);
                        }
                        else if (t.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                        {
                            riskList.Add(t);
                        }
                        else if (t.Status == (int)EnumUtility.RLoanStatus.正常还款 && t.DayCountLive <= NormalRepayment)
                        {
                            riskList.Add(t);
                        }
                        else if (t.Status == (int)EnumUtility.RLoanStatus.逾期未还款 && t.DayCountLive <= t.DepositDayCount - OverdueNotRepay)
                        {
                            riskList.Add(t);
                        }
                    }
                });

                riskList = riskList.OrderBy(t => t.Status).ThenBy(t => t.DayCountLive).ToList();
                return riskList;
            }
            catch
            {
                //iLog.WriteLog("DiaryWarning Error " + e.Message, 1);
                throw;
            }
        }

        #endregion




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
