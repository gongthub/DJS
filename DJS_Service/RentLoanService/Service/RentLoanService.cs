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
using System.Data.Entity.SqlServer;

namespace RentLoanService.Service
{
    public class RentLoanService
    {
        public static DJS.SDK.ILog iLog = null;

        private static int WarningDate = 0;
        private static int MinWarningDate = 0;

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
            MinWarningDate = Convert.ToInt32(ConstUtility.MinWarningDate);
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

                        #region 描述
                        sbTable.Append(@"<table><tr><td style='font-size:14px;'><span style='margin-left:24px;'>预警信息说明：</span></td></tr>
                                            <tr><td style='font-size:14px;'><span style='background-color:#95CACA'>未申请-已签订租金贷合同，未确定签约银行，社区需提醒客人进行面签，出现在预警报表时社区需进行催费</span></td></tr>
                                            <tr><td style='font-size:14px;'><span style='background-color:#95CACA'>待提交-已签订租金贷合同，已确定签约银行，社区需提醒客人进行面签，出现在预警报表时社区需进行催费</span></td></tr>
                                            <tr><td style='font-size:14px;'><span style='background-color:#95CACA'>审核未通过-银行审核未通过，社区需联系客人询问客人意向（换银行继续申请，转普通合同，退租），出现在预警报表时社区需进行催费</span></td></tr>
                                            <tr><td style='font-size:14px;'><span style='background-color:#FF8000'>待审核-租户已完成面签，等待银行审核，财务部/融资部根据银行回馈信息进行提交，如有意外情况与银行进行沟通核对，出现在预警报表时社区需进行催费</span></td></tr>
                                            <tr><td style='font-size:14px;'><span style='background-color:#FF8000'>逾期未还款-银行已放款，财务部根据银行反馈信息进行核对是否正常还款，社区根据财务部反馈信息进行催费</span></td></tr>
                                            <tr><td style='font-size:14px;'>通过未放款-银行审核已通过，财务部/融资部根据银行回馈信息进行提交，如有意外情况与银行进行沟通核对</td></tr>
                                            <tr><td style='font-size:14px;'>已放款-银行审核已通过并放款成功，财务部/融资部根据银行回馈信息进行提交</td></tr>
                                            <tr><td style='font-size:14px;'>正常还款-银行审核已通过并放款成功，客人开始正常还款，财务部/融资部根据银行回馈信息进行提交</td></tr>
                                     </table><br/>");
                        #endregion

                        sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                        sbTable.Append("<tr><td>姓名</td><td>联系电话</td><td>合同编号</td><td>开始时间</td><td>结束时间</td><td>上次缴费覆盖结束日期</td><td>最后操作时间</td><td>房间号</td><td>押金</td><td>补录押金</td><td>押金可住天数</td><td>风控天数</td><td>状态</td><td>预警说明</td><td>申请银行</td></tr>");
                        foreach (var item in RiskList)
                        {
                            sbTable.Append("<tr>");
                            if (item.Status == (int)EnumUtility.RLoanStatus.未申请租金贷 || item.Status == (int)EnumUtility.RLoanStatus.待提交 || item.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                            {
                                sbTable.Append("<td style='background:#95CACA'>" + item.Name + "</td>");
                            }
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待审核 || item.Status == (int)EnumUtility.RLoanStatus.逾期未还款)
                            {
                                sbTable.Append("<td style='background:#FF8000'>" + item.Name + "</td>");
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
                            if (item.Status == (int)EnumUtility.RLoanStatus.未申请租金贷 || item.Status == (int)EnumUtility.RLoanStatus.待提交 || item.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                            {
                                sbTable.Append("<td style='background:#95CACA'>" + item.DayCountLive + "</td>");
                            }
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待审核 || item.Status == (int)EnumUtility.RLoanStatus.逾期未还款)
                            {
                                sbTable.Append("<td style='background:#FF8000'>" + item.DayCountLive + "</td>");
                            }
                            else
                            {
                                sbTable.Append("<td>" + item.DayCountLive + "</td>");
                            }
                            sbTable.Append("<td>" + GetEnumDescription(typeof(EnumUtility.RLoanStatus), item.Status) + "</td>");

                            if (item.Status == (int)EnumUtility.RLoanStatus.未申请租金贷)
                                sbTable.Append("<td>" + "资料不全未申请租金贷超过" + WarningDate + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待提交)
                                sbTable.Append("<td>" + "资料齐全待提交超过" + WarningDate + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.待审核)
                                sbTable.Append("<td>" + "未审核超过" + WarningDate + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                                sbTable.Append("<td>" + "审核未通过" + WarningDate + "天" + "</td>");

                            else if (item.Status == (int)EnumUtility.RLoanStatus.审核通过)
                                sbTable.Append("<td>" + "审核通过超过" + MinWarningDate + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.正常还款)
                                sbTable.Append("<td>" + "正常还款超过" + MinWarningDate + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.逾期未还款)
                                sbTable.Append("<td>" + "逾期未还款超过" + MinWarningDate + "天" + "</td>");
                            else if (item.Status == (int)EnumUtility.RLoanStatus.已放款)
                                sbTable.Append("<td>" + "已放款超过" + MinWarningDate + "天" + "</td>");
                            else
                                sbTable.Append("<td></td>");

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
                string strSQL = string.Format(@"select * from (
	                                                select * from RentLoanRisks
	                                                 where DayCountLive<={0} 
	                                                 and Status in (100,0,3,1) 
	                                                 union 
	                                                 select * from RentLoanRisks
	                                                 where DayCountLive<={1} 
	                                                 and Status in (9,2,4,5)
                                                 ) te where StoreID = {2}
                                                 order by case when Status=100 then 1
				                                                when Status=0 then 2
				                                                when Status=3 then 3
				                                                when Status=1 then 4
				                                                when Status=9 then 5
				                                                when Status=2 then 6
				                                                when Status=4 then 7
				                                                else 8 end asc ,DayCountLive", WarningDate, MinWarningDate, StoreID);
                List<RentLoanRisk> riskList = SqlHelper.ListEntity<RentLoanRisk>(strSQL);
                return riskList;
            }
            catch
            {
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
