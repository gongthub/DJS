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

        private static int WarningDate = 0;
        private static int NotApply = 0;
        private static int NotAudit = 0;
        private static int AuditOK = 0;
        private static int OverdueNotRepay = 0;
        private static int WaitIntoPiece = 0;
        private static int WaitSubmit = 0;
        private static int NormalRepayment = 0;

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static RentLoanService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            WarningDate = Convert.ToInt32(ConstUtility.WarningDate);
            NotApply = Convert.ToInt32(ConstUtility.NotApply);
            NotAudit = Convert.ToInt32(ConstUtility.NotAudit);
            AuditOK = Convert.ToInt32(ConstUtility.AuditOK);
            OverdueNotRepay = Convert.ToInt32(ConstUtility.OverdueNotRepay);
            WaitIntoPiece = Convert.ToInt32(ConstUtility.WaitIntoPiece);
            WaitSubmit = Convert.ToInt32(ConstUtility.WaitSubmit);
            NormalRepayment = Convert.ToInt32(ConstUtility.NormalRepayment);
        }

        #endregion


        /// <summary>
        /// 租金贷预警
        /// </summary>
        public static void SendRentLoanEmail(int emailId, int CategoryID, int StoreID, int type)
        {
            try
            {
                //邮件
                if (type == (int)EnumUtility.EmialModel.Emial)
                {
                    List<RentLoanRisk> RiskList = GetRentLoanRisk(StoreID);
                    if (RiskList != null && RiskList.Count > 0)
                    {
                        DateTime nowTime = DateTime.Now;
                        string storeName = "";
                        StringBuilder sbTable = new StringBuilder();
                        sbTable.Append("<table><tr><td style='background: #f69a9a;width:150px;'></td><td>红色预警：负数日期</td></tr>");
                        sbTable.Append("<tr><td style='background: #e2b86b;width:150px;'></td><td>橙色预警：≤15天</td></tr>");
                        sbTable.Append("<tr><td style='background: #d3d376;width:150px;'></td><td>黄色预警：16-30天</td></tr>");
                        sbTable.Append("<tr><td style='background: #86d7fc;width:150px;'></td><td>蓝色预警：31-45天</td></tr></table>");
                        sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                        sbTable.Append("<tr><td>姓名</td><td>联系电话</td><td>合同编号</td><td>开始时间</td><td>结束时间</td><td>上次缴费覆盖结束日期</td><td>最后操作时间</td><td>房间号</td><td>押金</td><td>补录押金</td><td>押金可住天数</td><td>风控天数</td><td>状态</td><td>预警说明</td><td>申请银行</td></tr>");
                        foreach (var item in RiskList)
                        {
                            sbTable.Append("<tr>");
                            if (item.DayCountLive < 0)
                            {
                                sbTable.Append("<td style='background:#f69a9a'>" + item.Name + "</td>");
                            }
                            else if (item.DayCountLive >= 0 && item.DayCountLive <= 15)
                            {
                                sbTable.Append("<td style='background:#e2b86b'>" + item.Name + "</td>");
                            }
                            else if (item.DayCountLive >= 16 && item.DayCountLive <= 30)
                            {
                                sbTable.Append("<td style='background:#d3d376'>" + item.Name + "</td>");
                            }
                            else if (item.DayCountLive >= 31 && item.DayCountLive <= 45)
                            {
                                sbTable.Append("<td style='background:#86d7fc'>" + item.Name + "</td>");
                            }
                            else
                            {
                                sbTable.Append("<td>" + item.Name + "</td>");
                            }
                            sbTable.Append("<td>" + item.Phone + "</td>");
                            sbTable.Append("<td>" + item.ContractNo + "</td>");
                            sbTable.Append("<td>" + item.StartDate.ToShortDateString() + "</td>");
                            sbTable.Append("<td>" + item.EndDate.ToShortDateString() + "</td>");
                            sbTable.Append("<td>" + (item.PEndDate.HasValue ? item.PEndDate.Value.ToShortDateString() : "") + "</td>");
                            sbTable.Append("<td>" + item.ModifiedDate.ToShortDateString() + "</td>");
                            sbTable.Append("<td>" + item.FullName + "</td>");
                            sbTable.Append("<td>" + item.Deposit + "</td>");
                            sbTable.Append("<td>" + item.FillDeposit + "</td>");
                            sbTable.Append("<td>" + item.DepositLive + "</td>");
                            sbTable.Append("<td>" + item.DayCountLive + "</td>");
                            sbTable.Append("<td>" + GetEnumDescription(typeof(EnumUtility.RLoanStatus), item.Status) + "</td>");

                            int interval = new TimeSpan(nowTime.Ticks - item.ModifiedDate.Date.Ticks).Days;
                            if (item.Status == (int)EnumUtility.RLoanStatus.未申请租金贷 && interval >= NotApply)
                                sbTable.Append("<td>" + "资料不全未申请租金贷超过" + interval + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待提交 && interval >= WaitSubmit)
                                sbTable.Append("<td>" + "资料齐全待提交超过" + interval + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待进件 && interval >= WaitIntoPiece)
                                sbTable.Append("<td>" + "未进件超过" + interval + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待审核 && interval >= NotAudit)
                                sbTable.Append("<td>" + "未审核超过" + interval + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.审核通过 && interval >= AuditOK)
                                sbTable.Append("<td>" + "审核通过超过" + interval + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                                sbTable.Append("<td>" + "审核未通过" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.正常还款 && item.DayCountLive <= NormalRepayment)
                                sbTable.Append("<td>" + "风控天数等于或低于" + item.DayCountLive + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.逾期未还款 && item.DayCountLive <= item.DepositDayCount - OverdueNotRepay)
                                sbTable.Append("<td>" + "逾期未还款超过" + (item.DepositDayCount - item.DayCountLive) + "天" + "</td>");
                            else
                            {
                                if (item.Status != (int)EnumUtility.RLoanStatus.正常还款)
                                    sbTable.Append("<td>" + "风控天数等于或低于" + item.DayCountLive + "天" + "</td>");
                            }

                            sbTable.Append("<td>" + item.BankName + "</td>");
                            sbTable.Append("</tr>");
                            storeName = item.StoreName;
                        }
                        sbTable.Append("</table>");
                        EmailService.SendOverDueEmail(emailId, CategoryID, StoreID, storeName + EnumUtility.EmailCategory.租金贷预警.ToString(), sbTable.ToString());//调用邮件方法
                        EmailService.AddEmailLogs(CategoryID, StoreID, (int)EnumUtility.EmailLogStatu.成功, type, "成功");//日志
                    }

                }
                //短信
                if (type == (int)EnumUtility.EmialModel.SMS)
                {
                    //调用短信接口
                }
            }
            catch
            {
                //EmailService.AddEmailLogs(CategoryID, StoreID, (int)EnumUtility.EmailLogStatu.失败, type, e.Message);//日志
                //iLog.WriteLog("DiaryWarning Error " + e.Message, 1);
                throw;
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
                DateTime nowTime = DateTime.Now.Date;

                //状态不等于正常还款且风控天数<=公共阀值
                List<RentLoanRisk> riskList = dbContext.RentLoanRisks.Where(c => c.StoreID == StoreID && c.DayCountLive <= WarningDate && c.Status != (int)EnumUtility.RLoanStatus.正常还款).ToList();

                var riskListTemp = dbContext.RentLoanRisks.Where(c => c.StoreID == StoreID).ToList();
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
                        else if (t.Status == (int)EnumUtility.RLoanStatus.待进件 && interval >= WaitIntoPiece)
                        {
                            riskList.Add(t);
                        }
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
