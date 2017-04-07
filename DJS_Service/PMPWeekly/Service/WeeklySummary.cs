using PMPWeekly.Model;
using PMPWeekly.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPWeekly.Service
{
    public class WeeklySummary
    {
        public static DJS.SDK.ILog iLog = null;

        private static string SERVICECODE = "";

        #region 构造函数

        static WeeklySummary()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion

        public static void SendWeeklySummaryEmail()
        {
            List<UserInfo> list = WeeklyManagerList();
            List<string> lics = new List<string>();
            lics.Add("shishuailiang@vlinker.com.cn");
            lics.Add("pengkepan@vlinker.com.cn");
            foreach (UserInfo user in list)
            {
                List<string> li = new List<string>();
                li.Add(user.Email);
                List<Weekly> weekly = WeeklySummaryList(user.UserId);
                StringBuilder sbTable = new StringBuilder();
                sbTable.Append("<table width='5100px'  style='font-size:10px;border-bottom:0px;border-left:0px;border-right:0px'; cellpadding='0' cellspacing='0' border='1'>");

                sbTable.Append("<tr style='font-weight:bold;border:1px;'>");
                sbTable.Append("<td style='width: 150px'>项目</td>");
                sbTable.Append("<td style='width: 150px'>工期</td>");
                sbTable.Append("<td style='width: 150px'>施工单位</td>");
                sbTable.Append("<td style='width: 250px'>主要风险点</td>");    
                sbTable.Append("<td style='width: 350px'>本周项目形象进度</td>");     
                sbTable.Append("<td style='width: 550px'> 项目主要工程事项（施工单位工作）</td>");    
                sbTable.Append("<td style='width: 350px'>本周项目工作完成情况</td>");    
                sbTable.Append("<td style='width: 350px'>未完成原因及修改措施</td>");      
                sbTable.Append("<td style='width: 500px'>下周主要工程事项</td>");
                sbTable.Append("<td style='width: 500px'>下周工作计划</td>");
                sbTable.Append("<td style='width: 150px'>计划完成时间</td>");    
                sbTable.Append("<td style='width: 250px'>项目阶段验收情况</td>");     
                sbTable.Append("<td style='width: 250px'>签约总造价及付款情况</td>");    
                sbTable.Append("<td style='width: 250px'>消防、报建手续取证计划</td>");     
                sbTable.Append("<td style='width: 250px'>预算外成本增加记录</td>");    
                sbTable.Append("<td style='width: 150px'>施工现场人员数量</td>");     
                sbTable.Append("<td style='width: 250px'>甲供材料到货情况</td>");     
                sbTable.Append("<td style='width: 250px'>乙供材料到货情况</td>");
                sbTable.Append("<td style='width: 250px'>对该项目建议</td>");     
                sbTable.Append("</tr>");      
                    
                foreach (Weekly week in weekly)
                {
                    sbTable.Append("<tr><td style='width: 150px'>" + week.ItemName + "</td>");
                    sbTable.Append("<td style='width: 150px'>" + week.Name + "</td>");
                    sbTable.Append("<td style='width: 150px'>" + week.Builder + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.Risk + "</td>");
                    sbTable.Append("<td style='width: 350px'>" + week.LayoutProcess + "</td>");
                    sbTable.Append("<td style='width: 550px'>" + week.WeekProjectItems + "</td>");
                    sbTable.Append("<td style='width: 350px'>" + week.CompleteSituation + "</td>");
                    sbTable.Append("<td style='width: 350px'>" + week.DelayReason + "</td>");
                    sbTable.Append("<td style='width: 500px'>" + week.NextWeekContent + "</td>");
                    sbTable.Append("<td style='width: 500px'>" + week.NextWeekPlan + "</td>");
                    sbTable.Append("<td style='width: 150px'>" + week.PlanCompleteDate + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.AcceptanceSituation + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.PaymentContent + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.FirePlan + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.BudgetRecord + "</td>");
                    sbTable.Append("<td style='width: 150px'>" + week.WorkPeoples + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.AMaterialPresent + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.BMaterialPresent + "</td>");
                    sbTable.Append("<td style='width: 250px'>" + week.ProjectOpinion + "</td>");
                    sbTable.Append("</tr>");
                }
                sbTable.Append(" <tr><td colspan='6' style='border-bottom:0px;border-left:0px;border-right:0px' > 本邮件发送自微领地PMP平台 （操作者：PMP系统）</td><td  style='border-bottom:0px;border-left:0px;border-right:0px' colspan='13'></td></tr>");
                sbTable.Append("</table></br>");
                EmailService.SendEmail(li, lics, null, ConstUtility.EmailTitle, sbTable.ToString());
            }
        }


        public static List<UserInfo> WeeklyManagerList()
        {

            List<UserInfo> list = new List<UserInfo>();
            IDBRepository dbContext = new IDBRepository();

            string sql = @" select distinct UserProfiles.UserId,UserProfiles.Email 
                            from Projects inner join Weeklies
                            on Projects.ID =Weeklies.ProjectID
                            inner join UserProfiles on Projects.ManagerID=UserProfiles.UserId ";
            try
            {
                list = dbContext.Database.SqlQuery<UserInfo>(sql).ToList();
            }
            catch (Exception ex)
            {

            }
            return list;

        }

        public static List<Weekly> WeeklySummaryList(int ManagerID)
        {
            List<Model.Weekly> models = new List<Model.Weekly>();
            IDBRepository dbContext = new IDBRepository();
            string sql = @"  select Projects.ManagerID, ItemName,Name,Builder,Risk,LayoutProcess,
                             WeekProjectItems, CompleteSituation,DelayReason,NextWeekContent,
                             NextWeekPlan,PlanCompleteDate, AcceptanceSituation,PaymentContent,
                             FirePlan,BudgetRecord,WorkPeoples, AMaterialPresent,BMaterialPresent,
                             ProjectOpinion from Items
                             inner join  Projects on   Items.Id=Projects.ItemID
                             inner join Weeklies on Projects.ID=Weeklies.ProjectID
                             where Projects.ManagerID=@ManagerID order by Projects.ManagerID";
            models = dbContext.Database.SqlQuery<Model.Weekly>(sql,
                      new SqlParameter("@ManagerID", ManagerID)).ToList();

            return models;

        }
    }
}
