using PMPSmallNodesExtensionSummary.Service;
using PMPSmallNodesExtensionSummary.Model;
using PMPSmallNodesExtensionSummary.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMPProjectSummary.Model;

namespace PMPSmallNodesExtensionSummary.Service
{
    public class SmallNodesExtensionSummary
    {
        public static DJS.SDK.ILog iLog = null;

        private static string SERVICECODE = "";

        #region 构造函数

        static SmallNodesExtensionSummary()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion



        public static void SendSmallNodesExtensionSummaryEmail()
        {
            List<UserInfo> list = SmallNodesExtensionManagerList();
            List<string> lics = new List<string>();
            lics.Add("shishuailiang@vlinker.com.cn");
            foreach (UserInfo user in list)
            {
                List<string> li = new List<string>();
                li.Add(user.Email);
                List<SmallNodesExtensionModel> SmallNodes = SmallNodesExtensionSummaryList(user.UserId);
                StringBuilder sbTable = new StringBuilder();
                sbTable.Append("<table width='1300px'  style='font-size:10px;border-bottom:0px;border-left:0px;border-right:0px'; cellpadding='0' cellspacing='0' border='1'>");
                sbTable.Append("<tr style='font-weight:bold;border:1px;'><td>项目</td><td>工期</td><td>大节点</td>");
                sbTable.Append("<td>小节点</td><td>延迟类别</td><td>延时天数</td></tr>");
                foreach (SmallNodesExtensionModel smallNode in SmallNodes)
                {
                    sbTable.Append("<tr><td width='300px'>" + smallNode.ItemName + "</td><td width='150px'>" + smallNode.ProjectName + "</td><td width='200px'>" + smallNode.ModelDetailName + "</td>");
                    sbTable.Append("<td width='400px'>" + smallNode.ModelNodeName + "</td><td width='150px'>" + smallNode.DelayReason + "</td><td width='50px'>" + smallNode.DelayDate + "</td></tr>");
                }
                sbTable.Append(" <tr><td colspan='6' style='border-bottom:0px;border-left:0px;border-right:0px' > 本邮件发送自微领地PMP平台 （操作者：PMP系统）</td></tr>");
                sbTable.Append("</table></br>");
                EmailService.SendEmail(li, lics, null, ConstUtility.EmailTitle, sbTable.ToString());
            }

        }



        public static List<SmallNodesExtensionModel> SmallNodesExtensionSummaryList(int ManagerID)
        {
            List<Model.SmallNodesExtensionModel> models = new List<Model.SmallNodesExtensionModel>();
            IDBRepository dbContext = new IDBRepository();
            string sql = @" select Projects.ManagerID,UserProfiles.Email, Items.ItemName,Projects.Name ProjectName ,
                            ModelDetails.Name ModelDetailName  ,ModelNodes.Name
                            ModelNodeName,ProjectNodes.StartDate ProjectNodeStartDate,
                            ProjectNodes.EndDate ProjectNodeEndDate,
                            ProjectNodes.RealStartDate ProjectNodesRealStartDate,
                            ProjectNodes.RealEndDate ProjectNodesRealEndDate
                            from 
                            Items inner join Projects on Items.Id=Projects.ItemID
                            inner join ProjectDetails on Projects.ID=ProjectDetails.ProjectID
                            inner join ProjectNodes on ProjectDetails.ID=ProjectNodes.ProjectDetailID
                            inner join ModelDetails on ProjectDetails.ModelDetailID=ModelDetails.ID
                            inner join ModelNodes on ProjectNodes.ModelNodeID=ModelNodes.ID
                            inner join UserProfiles on  Projects.ManagerID=UserProfiles.UserId
                            where ( 
                            (ProjectNodes.RealStartDate is null and CONVERT(varchar(100), GETDATE(), 11) >
                            ProjectNodes.StartDate)
                            or (ProjectNodes.RealEndDate is null and ProjectNodes.RealStartDate is not null
                            and CONVERT(varchar(100), GETDATE(), 11)>
                            ProjectNodes.EndDate) or
                            ProjectNodes.RealStartDate>ProjectNodes.StartDate
                            or ProjectNodes.RealEndDate> ProjectNodes.EndDate) and Projects.ManagerID=@ManagerID
                            order by Projects.ManagerID";
            models = dbContext.Database.SqlQuery<Model.SmallNodesExtensionModel>(sql,
                      new SqlParameter("@ManagerID", ManagerID)).ToList();
            var timeNow = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            foreach (SmallNodesExtensionModel smallNodes in models)
            {

                if (smallNodes.ProjectNodesRealStartDate == null)
                {
                    if (timeNow > smallNodes.ProjectNodeStartDate)
                    {
                        smallNodes.DelayReason = "延时未开始";
                        TimeSpan time1 = timeNow - smallNodes.ProjectNodeStartDate;
                        smallNodes.DelayDate = time1.Days;

                    }
                    if (timeNow > smallNodes.ProjectNodeEndDate)
                    {
                        smallNodes.DelayReason = "延时未结束";
                        TimeSpan time2 = timeNow - smallNodes.ProjectNodeEndDate;
                        smallNodes.DelayDate = time2.Days;
                    }
                }
                else if (smallNodes.ProjectNodesRealStartDate != null && smallNodes.ProjectNodesRealEndDate == null)
                {
                    if (smallNodes.ProjectNodesRealStartDate > smallNodes.ProjectNodeStartDate)
                    {
                        smallNodes.DelayReason = "延时开始";
                        TimeSpan time1 = timeNow - smallNodes.ProjectNodeStartDate;
                        smallNodes.DelayDate = time1.Days;

                    }
                    if (timeNow > smallNodes.ProjectNodeEndDate)
                    {
                        smallNodes.DelayReason = "延时未结束";
                        TimeSpan time2 = timeNow - smallNodes.ProjectNodeEndDate;
                        smallNodes.DelayDate = time2.Days;
                    }
                }
                else if (smallNodes.ProjectNodesRealStartDate != null && smallNodes.ProjectNodesRealEndDate != null)
                {
                    if (smallNodes.ProjectNodesRealStartDate > smallNodes.ProjectNodeStartDate)
                    {
                        smallNodes.DelayReason = "延时开始";
                        TimeSpan time1 = timeNow - smallNodes.ProjectNodeStartDate;
                        smallNodes.DelayDate = time1.Days;

                    }
                    if (smallNodes.ProjectNodesRealEndDate > smallNodes.ProjectNodeEndDate)
                    {
                        smallNodes.DelayReason = "延时结束";
                        TimeSpan time2 = timeNow - smallNodes.ProjectNodeEndDate;
                        smallNodes.DelayDate = time2.Days;
                    }
                }

            }

            return models;

        }



        public static List<UserInfo> SmallNodesExtensionManagerList()
        {

            List<UserInfo> list = new List<UserInfo>();
            IDBRepository dbContext = new IDBRepository();

            string sql = @" select distinct UserProfiles.UserId,UserProfiles.Email from 
                            Items inner join Projects on Items.Id=Projects.ItemID
                            inner join ProjectDetails on Projects.ID=ProjectDetails.ProjectID
                            inner join ProjectNodes on ProjectDetails.ID=ProjectNodes.ProjectDetailID
                            inner join UserProfiles on  Projects.ManagerID=UserProfiles.UserId
                            where ( 
                            (ProjectNodes.RealStartDate is null and CONVERT(varchar(100), GETDATE(), 11) >
                            ProjectNodes.StartDate)
                            or (ProjectNodes.RealEndDate is null and ProjectNodes.RealStartDate is not null
                            and CONVERT(varchar(100), GETDATE(), 11)>
                            ProjectNodes.EndDate) or
                            ProjectNodes.RealStartDate>ProjectNodes.StartDate
                            or ProjectNodes.RealEndDate> ProjectNodes.EndDate) ";
            try
            {
                list = dbContext.Database.SqlQuery<UserInfo>(sql).ToList();
            }
            catch (Exception ex)
            {
 
            }
            return list;

        }
    }
}
