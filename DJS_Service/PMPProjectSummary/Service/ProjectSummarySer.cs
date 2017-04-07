
using PMPProjectSummary.Model;
using PMPProjectSummary.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Service
{
    public class ProjectSummarySer
    {
        public static DJS.SDK.ILog iLog = null;

        private static string SERVICECODE = "";

        #region 构造函数

        static ProjectSummarySer()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion



        /// <summary>
        /// 租金贷预警
        /// </summary>
        public static void SendDayTrialEmail()
        {
            try
            {

                try
                {

                    if (EmailService.ServiceIsStart(SERVICECODE))
                    {
                        List<Model.AMSServiceSetEmail> emailLists = EmailService.GetServiceSetEmailStoresList(SERVICECODE);
                        if (emailLists != null && emailLists.Count > 0)
                        {
                            foreach (Model.AMSServiceSetEmail itemT in emailLists)
                            {


                                #region
                                List<ProjectSummary> projects = ProjectSummaryList();

                                double AlreadyCompeReal0 = 0;
                                double AlreadyCompeReal1 = 0;


                                foreach (ProjectSummary p in projects)
                                {

                                    //project的状态
                                    p.Status = GetProjectStatus0(p, p.RealStartDate, p.RealEndDate);

                                    List<ProjectDetail> liProjectDetail = ProjectDetailList(p.ID);

                                    foreach (ProjectDetail projectDetail in liProjectDetail)
                                    {
                                        List<ProjectNode> ProjectNodes = GetProjectNodes(p, projectDetail.ID);//获取大节点下所有小节点
                                        if (ProjectNodes.Where(x => x.RealEndDate == null).Count() > 0)//如果有节点没有完成，则完成时间问null
                                        {
                                            projectDetail.Status = GetProjectStatus1(ProjectNodes.Min(x => x.StartDate),
                                                                    ProjectNodes.Max(x => x.EndDate),
                                                                    ProjectNodes.Where(x => x.RealStartDate.ToString() != "0001/1/1 0:00:00").Min(x => x.RealStartDate), null);
                                        }
                                        else
                                        {
                                            projectDetail.Status = GetProjectStatus1(ProjectNodes.Min(x => x.StartDate),
                                                                   ProjectNodes.Max(x => x.EndDate),
                                                                   ProjectNodes.Where(x => x.RealStartDate.ToString() != "0001/1/1 0:00:00").Min(x => x.RealStartDate),
                                                                   ProjectNodes.Where(x => x.RealStartDate.ToString() != "0001/1/1 0:00:00").Max(x => x.RealEndDate));
                                        }
                                    }

                                    //交付运营完工 且 项目完工
                                    if (liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").ToList().Count() > 0 && (liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").Select(x => x.Status).First() ==
                                        (int)ProjectNodeStatus.AheadSchedule ||
                                        liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").Select(x => x.Status).First() ==
                                        (int)ProjectNodeStatus.Complete ||
                                         liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").Select(x => x.Status).First() ==
                                         (int)ProjectNodeStatus.DelayComplete) &&
                                         (p.Status == (int)ProjectNodeStatus.AheadSchedule ||
                                          p.Status == (int)ProjectNodeStatus.Complete ||
                                          p.Status == (int)ProjectNodeStatus.DelayComplete)
                                        )
                                    {
                                        p.ProjectCycleState = "项目已完工";
                                        p.projectDetailName = "交付运营";

                                    }
                                    //除了交付运营 其它都已完工
                                    else if ((!liProjectDetail.Where(x => x.ModelDetail.Name != "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.BeforePlan)) &&
                                              !liProjectDetail.Where(x => x.ModelDetail.Name != "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.DelayNotStarted)) &&
                                              !liProjectDetail.Where(x => x.ModelDetail.Name != "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.Ongoing)) &&
                                              !liProjectDetail.Where(x => x.ModelDetail.Name != "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.DelayStart)) &&
                                              !liProjectDetail.Where(x => x.ModelDetail.Name != "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.EarlyStart)) &&
                                              !liProjectDetail.Where(x => x.ModelDetail.Name != "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.Unfinished))) &&
                                              (liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.Ongoing)) ||
                                               liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.EarlyStart)) ||
                                               liProjectDetail.Where(x => x.ModelDetail.Name == "交付运营").
                                               Select(x => x.Status).ToList().Contains((short)(ProjectNodeStatus.Unfinished))
                                               )
                                        )
                                    {
                                        p.ProjectCycleState = "项目完工，已交付运营";
                                        p.projectDetailName = "交付运营";
                                    }



                                    else
                                    {
                                        p.ProjectCycleState = GetEnumDescription((ProjectNodeStatus)p.Status);
                                        if (liProjectDetail.Where
                                            (x => x.Status == (int)ProjectNodeStatus.DelayNotStarted ||
                                                  x.Status == (int)ProjectNodeStatus.DelayStart ||
                                                  x.Status == (int)ProjectNodeStatus.Unfinished
                                                ).Count() > 0)
                                            p.projectDetailName = liProjectDetail.Where
                                                (x => x.Status == (int)ProjectNodeStatus.DelayNotStarted ||
                                                      x.Status == (int)ProjectNodeStatus.DelayStart ||
                                                      x.Status == (int)ProjectNodeStatus.Unfinished
                                                    ).LastOrDefault().ModelDetail.Name;
                                        else if (liProjectDetail.Where
                                            (x => x.Status == (int)ProjectNodeStatus.Ongoing ||
                                                  x.Status == (int)ProjectNodeStatus.EarlyStart
                                                ).Count() > 0)
                                            p.projectDetailName = liProjectDetail.Where
                                            (x => x.Status == (int)ProjectNodeStatus.Ongoing ||
                                             x.Status == (int)ProjectNodeStatus.EarlyStart
                                             ).LastOrDefault().ModelDetail.Name;
                                        else
                                            p.projectDetailName = "";
                                    }

                                    AlreadyCompeReal0 = GetProjectCompletionDays(p.ID);
                                    AlreadyCompeReal1 = GetProjectDays(p.ID);

                                    p.ProjectCompletion = (((AlreadyCompeReal0 / AlreadyCompeReal1) < 0 ? 0
                                                          : (AlreadyCompeReal0 / AlreadyCompeReal1)) * 100).ToString(" 0") + "%";

                                    p.ReasonDelaySummary = strDelayReasonName(p.ID);

                                }

                                #endregion

                                #region

                                if (projects != null && projects.Count > 0)
                                {

                                    PMPCombined Combined = DtPMPCombined();

                                    string strReasonDelay = string.Empty;


                                    StringBuilder sbTable = new StringBuilder();



                                    string str = "各位领导好！截止至" + DateTime.Now.Year + "年" + (DateTime.Now.Month) + "月" + DateTime.Now.Day + "日，已签约项目汇总信息如下：</br>";
                                    sbTable.Append(str);

                                    sbTable.Append("<table width='1500px'  style='text-align:center;font-size:10px;float:left;border-right:0px;border-left:0px;border-top:0px;border-bottom:0px';  cellpadding='0' cellspacing='0' border='1'>");
                                    sbTable.Append("<tr style='font-weight:bold;'><td colspan='4'>分类</td><td>未完工</td><td>已完工</td><td>合计</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    sbTable.Append("<tr><td rowspan='9'>大V</td><td rowspan='3'>直营</td><td colspan='2'>项目周期数</td>");
                                    //1大V 直营 项目周期数 未完工 已完工 合计
                                    sbTable.Append("<td>" + Convert.ToInt32(Combined.BigVDirectlyProject_unfinished) + "</td><td>" + Convert.ToInt32(Combined.BigVDirectlyProject_finished) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_unfinished) + Convert.ToInt32(Combined.BigVDirectlyProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //2大V 直营 房间数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>房间数</td><td>" + Convert.ToInt32(Combined.BigVDirectlyRoom_unfinished) + "</td><td>" + Convert.ToInt32(Combined.BigVDirectlyRoom_finished) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.BigVDirectlyRoom_finished)) + "</td><td colspan='9'  style='border-right:0px;border-top:0px;border-bottom:0px'  ></td></tr>");
                                    //3大V 直营 床位数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>床位数</td><td>" + Convert.ToInt32(Combined.BigVDirectlyBed_unfinished) + "</td><td>" + Convert.ToInt32(Combined.BigVDirectlyBed_finished) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyBed_unfinished) + Convert.ToInt32(Combined.BigVDirectlyBed_finished)) + "</td><td colspan='9'  style='border-right:0px;border-top:0px;border-bottom:0px'  ></td></tr>");


                                    //4大V 特许 项目数 未完工 已完工 合计
                                    sbTable.Append("<tr><td rowspan='3'>特许</td><td colspan='2'>项目周期数</td><td>" + Convert.ToInt32(Combined.BigVCharterProject_unfinished) + "</td><td>" + Convert.ToInt32(Combined.BigVCharterProject_finished) + "</td><td>" + (Convert.ToInt32(Combined.BigVCharterProject_unfinished) + Convert.ToInt32(Combined.BigVCharterProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //5大V 特许 房间数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>房间数</td><td>" + Convert.ToInt32(Combined.BigVCharterRoom_unfinished) + "</td><td>" + Convert.ToInt32(Combined.BigVCharterRoom_finished) + "</td><td>" + (Convert.ToInt32(Combined.BigVCharterRoom_unfinished) + Convert.ToInt32(Combined.BigVCharterRoom_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //6大V 特许 床位数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>床位数</td><td>" + Convert.ToInt32(Combined.BigVCharterBed_unfinished) + "</td><td>" + Convert.ToInt32(Combined.BigVCharterBed_finished) + "</td><td>" + (Convert.ToInt32(Combined.BigVCharterBed_unfinished) + Convert.ToInt32(Combined.BigVCharterBed_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");





                                    //7大V 小计 项目数 未完工 已完工 合计
                                    sbTable.Append("<tr><td rowspan='3'>小计</td><td colspan='2'>项目周期数</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_unfinished) + Convert.ToInt32(Combined.BigVCharterProject_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_finished) + Convert.ToInt32(Combined.BigVCharterProject_finished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_unfinished) + Convert.ToInt32(Combined.BigVDirectlyProject_finished) + Convert.ToInt32(Combined.BigVCharterProject_unfinished) + Convert.ToInt32(Combined.BigVCharterProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //8大V 小计 房间数 未完工 已完工 合计  
                                    sbTable.Append("<tr><td colspan='2'>房间数</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.BigVCharterRoom_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyRoom_finished) + Convert.ToInt32(Combined.BigVCharterRoom_finished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.BigVDirectlyRoom_finished) + Convert.ToInt32(Combined.BigVCharterRoom_unfinished) + Convert.ToInt32(Combined.BigVCharterRoom_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //9大V 小计 床位数 未完工 已完工 合计  
                                    sbTable.Append("<tr><td colspan='2'>床数</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyBed_unfinished) + Convert.ToInt32(Combined.BigVCharterBed_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyBed_finished) + Convert.ToInt32(Combined.BigVCharterBed_finished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyBed_unfinished) + Convert.ToInt32(Combined.BigVDirectlyBed_finished) + Convert.ToInt32(Combined.BigVCharterBed_unfinished) + Convert.ToInt32(Combined.BigVCharterBed_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");




                                    //1小V 直营 项目数 未完工 已完工 合计  
                                    sbTable.Append("<tr><td rowspan='9'>小V</td><td rowspan='3'>直营</td><td colspan='2' >项目周期数</td>");
                                    sbTable.Append("<td>" + Convert.ToInt32(Combined.SmallVDirectlyProject_unfinished) + "</td><td>" + Convert.ToInt32(Combined.SmallVDirectlyProject_finished) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyProject_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //2小V 直营 房间数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>房间数</td><td>" + Convert.ToInt32(Combined.SmallVDirectlyRoom_unfinished) + "</td><td>" + Convert.ToInt32(Combined.SmallVDirectlyRoom_finished) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyRoom_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //3小V 直营 床位数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>床位数</td><td>" + Convert.ToInt32(Combined.SmallVDirectlyBed_unfinished) + "</td><td>" + Convert.ToInt32(Combined.SmallVDirectlyBed_finished) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyBed_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyBed_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");



                                    //4小V 特许 项目数 未完工 已完工 合计
                                    sbTable.Append("<tr><td rowspan='3'>特许</td><td colspan='2'>项目周期数</td><td>" + Convert.ToInt32(Combined.SmallVCharterProject_unfinished) + "</td><td>" + Convert.ToInt32(Combined.SmallVCharterProject_finished) + "</td><td>" + (Convert.ToInt32(Combined.SmallVCharterProject_unfinished) + Convert.ToInt32(Combined.SmallVCharterProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //5小V 特许 房间数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>房间数</td><td>" + Convert.ToInt32(Combined.SmallVCharterRoom_unfinished) + "</td><td>" + Convert.ToInt32(Combined.SmallVCharterRoom_finished) + "</td><td>" + (Convert.ToInt32(Combined.SmallVCharterRoom_unfinished) + Convert.ToInt32(Combined.SmallVCharterRoom_finished)) + "</td><td colspan='9'style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //6小V 特许 床位数 未完工 已完工 合计
                                    sbTable.Append("<tr><td colspan='2'>床位数</td><td>" + Convert.ToInt32(Combined.SmallVCharterBed_unfinished) + "</td><td>" + Convert.ToInt32(Combined.SmallVCharterBed_finished) + "</td><td>" + (Convert.ToInt32(Combined.SmallVCharterBed_unfinished) + Convert.ToInt32(Combined.SmallVCharterBed_finished)) + "</td><td colspan='9'style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");


                                    //7小V 小计 项目数 未完工 已完工 合计
                                    sbTable.Append("<tr><td rowspan='3'>小计</td><td colspan='2'>项目周期数</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyProject_unfinished) + Convert.ToInt32(Combined.SmallVCharterProject_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyProject_finished) + Convert.ToInt32(Combined.SmallVCharterProject_finished)) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyProject_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyProject_finished) + Convert.ToInt32(Combined.SmallVCharterProject_unfinished) + Convert.ToInt32(Combined.SmallVCharterProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //8大V 小计 房间数 未完工 已完工 合计  
                                    sbTable.Append("<tr><td colspan='2'>房间数</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.SmallVCharterRoom_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyRoom_finished) + Convert.ToInt32(Combined.SmallVCharterRoom_finished)) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyRoom_finished) + Convert.ToInt32(Combined.SmallVCharterRoom_unfinished) + Convert.ToInt32(Combined.SmallVCharterRoom_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //9大V 小计 房间数 未完工 已完工 合计  
                                    sbTable.Append("<tr><td colspan='2'>床位数</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyBed_unfinished) + Convert.ToInt32(Combined.SmallVCharterBed_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyBed_finished) + Convert.ToInt32(Combined.SmallVCharterBed_finished)) + "</td><td>" + (Convert.ToInt32(Combined.SmallVDirectlyBed_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyBed_finished) + Convert.ToInt32(Combined.SmallVCharterBed_unfinished) + Convert.ToInt32(Combined.SmallVCharterBed_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");



                                    //合计 项目数
                                    sbTable.Append("<tr><td rowspan='2' colspan='2' style='border-bottom: 1px solid #9A9A9A;' >合并</td><td colspan='2'>项目周期数</td>");
                                    sbTable.Append("<td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_unfinished) + Convert.ToInt32(Combined.BigVCharterProject_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyProject_unfinished) + Convert.ToInt32(Combined.SmallVCharterProject_unfinished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_finished) + Convert.ToInt32(Combined.BigVCharterProject_finished) + Convert.ToInt32(Combined.SmallVDirectlyProject_finished) + Convert.ToInt32(Combined.SmallVCharterProject_finished)) + "</td><td>" + (Convert.ToInt32(Combined.BigVDirectlyProject_unfinished) + Convert.ToInt32(Combined.BigVDirectlyProject_finished) + Convert.ToInt32(Combined.BigVCharterProject_unfinished) + Convert.ToInt32(Combined.BigVCharterProject_finished) + Convert.ToInt32(Combined.SmallVDirectlyProject_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyProject_finished) + Convert.ToInt32(Combined.SmallVCharterProject_unfinished) + Convert.ToInt32(Combined.SmallVCharterProject_finished)) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");
                                    //合计 房间数+床位数
                                    sbTable.Append(" <tr><td colspan='2' style='border-bottom: 1px solid #9A9A9A;' >房间数+房床数</td><td style='border-bottom: 1px solid #9A9A9A;' >" + (Convert.ToInt32(Combined.BigVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.BigVCharterRoom_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.SmallVCharterRoom_unfinished)+
                                        Convert.ToInt32(Combined.BigVDirectlyBed_unfinished) + Convert.ToInt32(Combined.BigVCharterBed_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyBed_unfinished) + Convert.ToInt32(Combined.SmallVCharterBed_unfinished)
                                        ) + "</td><td style='border-bottom: 1px solid #9A9A9A;' >" + (Convert.ToInt32(Combined.BigVDirectlyRoom_finished) + Convert.ToInt32(Combined.BigVCharterRoom_finished) + Convert.ToInt32(Combined.SmallVDirectlyRoom_finished) + Convert.ToInt32(Combined.SmallVCharterRoom_finished)+
                                        Convert.ToInt32(Combined.BigVDirectlyBed_finished) + Convert.ToInt32(Combined.BigVCharterBed_finished) + Convert.ToInt32(Combined.SmallVDirectlyBed_finished) + Convert.ToInt32(Combined.SmallVCharterBed_finished)
                                        ) + "</td><td style='border-bottom: 1px solid #9A9A9A;'>" + (Convert.ToInt32(Combined.BigVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.BigVDirectlyRoom_finished) + Convert.ToInt32(Combined.BigVCharterRoom_unfinished) + Convert.ToInt32(Combined.BigVCharterRoom_finished) + Convert.ToInt32(Combined.SmallVDirectlyRoom_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyRoom_finished) + Convert.ToInt32(Combined.SmallVCharterRoom_unfinished) + Convert.ToInt32(Combined.SmallVCharterRoom_finished)+
                                        Convert.ToInt32(Combined.BigVDirectlyBed_unfinished) + Convert.ToInt32(Combined.BigVDirectlyBed_finished) + Convert.ToInt32(Combined.BigVCharterBed_unfinished) + Convert.ToInt32(Combined.BigVCharterBed_finished) + Convert.ToInt32(Combined.SmallVDirectlyBed_unfinished) + Convert.ToInt32(Combined.SmallVDirectlyBed_finished) + Convert.ToInt32(Combined.SmallVCharterBed_unfinished) + Convert.ToInt32(Combined.SmallVCharterBed_finished)
                                        ) + "</td><td colspan='9' style='border-right:0px;border-top:0px;border-bottom:0px' ></td></tr>");

                                    sbTable.Append(" <tr><td colspan='7' style='border-right:0px;border-left:0px;border-top:0px;border-bottom:0px;text-align: left;'>注：其中杭州教工路、绥德路项目包含大V和小V二种产品类型，系统默认项目计数是按产品类型统计。</td><td colspan='9'  style='border-right:0px;border-left:0px;border-top:0px;border-bottom:0px;'></td></tr>");


                                    //sbTable.Append("<table style='margin-top:5px;font-size:10px;float:left' width='1500px'  cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                                    sbTable.Append("<tr><td colspan='16'  style='border-right:0px;border-left:0px;border-top:0px; height:30px;'></td></tr>");
                                    sbTable.Append("<tr style='font-weight:bold;'><td style='width:50px;text-align: center'>序号</td><td style='width:70px;text-align: center' >城市</td><td style='width:60px;text-align: center'>项目类别</td><td style='width:50px;text-align: center'>性质</td><td style='width:90px;text-align: center'>项目编号</td><td style='width:150px;text-align: center'>项目名称</td>");
                                    sbTable.Append("<td style='width:80px;text-align: center'>项目周期</td><td style='width:60px;text-align: center'>房间/床位数</td><td style='width:80px;text-align: center'>状态</td>");
                                    sbTable.Append("<td style='width:100px;text-align: center'>计划开始时间</td><td style='width:100px;text-align: center'>计划完工时间</td><td style='width:80px;text-align: center'>项目节点</td><td style='width:100px;text-align: center'>实际开始时间</td>");
                                    //sbTable.Append("<td style='width:100px;text-align: center'>预计结束时间</td><td style='width:60px;text-align: center'>项目完成度</td><td style='width:160px;text-align: center; border-right: 1px solid #9A9A9A;'>延迟原因汇总</td></tr>");
                                    sbTable.Append("<td style='width:60px;text-align: center'>项目完成度</td><td style='width:160px;text-align: center; border-right: 1px solid #9A9A9A;'>延迟原因汇总</td></tr>");
                                    for (int i = 0; i < projects.Count(); i++)
                                    {
                                        strReasonDelay = "";
                                        if (projects[i].Status == (int)ProjectNodeStatus.DelayNotStarted ||
                                            projects[i].Status == (int)ProjectNodeStatus.DelayStart ||
                                            projects[i].Status == (int)ProjectNodeStatus.Unfinished)
                                        {
                                            strReasonDelay = "未填写";
                                        }

                                        if (i % 2 == 0)
                                        {
                                            if (projects[i].Status == (int)ProjectNodeStatus.DelayNotStarted ||
                                                projects[i].Status == (int)ProjectNodeStatus.DelayStart ||
                                                projects[i].Status == (int)ProjectNodeStatus.Unfinished)
                                                sbTable.Append("<tr style='background-color:#ff7575;'>");
                                            else
                                                sbTable.Append("<tr style='background-color:#DCECF8;'>");
                                        }
                                        else
                                        {
                                            if (projects[i].Status == (int)ProjectNodeStatus.DelayNotStarted ||
                                              projects[i].Status == (int)ProjectNodeStatus.DelayStart ||
                                              projects[i].Status == (int)ProjectNodeStatus.Unfinished)
                                                sbTable.Append("<tr style='background-color:#ff7575;'>");
                                            else
                                                sbTable.Append("<tr>");

                                        }


                                        #region

                                        int deayDate = 0;
                                        string ExpectedCompletionDate = "";
                                        List<DateTime> liTime = new List<DateTime>();
                                        IDBRepository dbContext = new IDBRepository();
                                        int projectID = projects[i].ID;
                                        //2017-2-20 新添预判完成字段 KIM
                                        List<int> projectlist = dbContext.ProjectDetails.Where(x => x.ProjectID == projectID).Select(x => x.ID).ToList();
                                        List<ProjectDetail> projectDetail = dbContext.ProjectDetails.Where(x => x.ProjectID == projectID).OrderBy(x => x.ModelDetail.Sort).ToList(); ;
                                        var listProjectNodes = dbContext.ProjectNodes.Where(x => projectlist.Contains(x.ProjectDetailID)).ToList();
                                        var projectNodeRealEndDate = listProjectNodes.Where(x => x.RealEndDate == null || x.RealEndDate.ToString() == "1900-1-1 0:00:00");
                                        if (projectNodeRealEndDate.Count() <= 0)
                                        {
                                            ExpectedCompletionDate = Convert.ToDateTime(listProjectNodes.Max(x => x.RealEndDate)).ToString("yyyy/MM/dd");
                                        }
                                        else
                                        {
                                            foreach (ProjectDetail detail in projectDetail)
                                            {
                                                var liNodes = dbContext.ProjectNodes.Where(x => x.ProjectDetailID == detail.ID).ToList();
                                                var nodeRealEndDate = liNodes.Where(x => x.RealEndDate == null || x.RealEndDate.ToString() == "1900-1-1 0:00:00");
                                                if (nodeRealEndDate.Count() <= 0)
                                                {
                                                    TimeSpan time = Convert.ToDateTime(liNodes.Max(x => x.RealEndDate)) - Convert.ToDateTime(liNodes.Max(x => x.EndDate));
                                                    deayDate += time.Days;
                                                }
                                                else
                                                {
                                                    if (DateTime.Now > Convert.ToDateTime(liNodes.Max(x => x.EndDate)))
                                                    {
                                                        TimeSpan time = DateTime.Now - Convert.ToDateTime(Convert.ToDateTime(liNodes.Max(x => x.EndDate)));
                                                        deayDate += time.Days;
                                                    }
                                                    liTime.Add(Convert.ToDateTime(liNodes.Max(x => x.EndDate)).AddDays(deayDate));
                                                }

                                            }
                                            ExpectedCompletionDate = liTime.Max().ToString("yyyy/MM/dd");

                                        }

                                        #endregion


                                        sbTable.Append("<td style='width:50px;text-align: center'>" + (i + 1) + "</td>");
                                        sbTable.Append("<td style='width:70px;text-align: center' >" + projects[i].CityName + "</td>");
                                        sbTable.Append("<td style='width:60px;text-align: center'>" + projects[i].ProjectType + "</td>");
                                        sbTable.Append("<td style='width:50px;text-align: center'>" + projects[i].ItemType + "</td>");
                                        sbTable.Append("<td style='width:90px;text-align: center'>" + projects[i].ItemCode + "</td>");
                                        sbTable.Append("<td style='width:150px;text-align:left' >" + projects[i].ItemName + "</td>");
                                        sbTable.Append("<td style='width:80px;'>" + projects[i].ProjectName + "</td>");
                                        sbTable.Append("<td style='width:60px;text-align: center'>" + 
                                            (Convert.ToInt32(projects[i].ProjectRoom)+Convert.ToInt32(projects[i].BedNumber)+
                                             Convert.ToInt32(projects[i].NotSellRoom)+Convert.ToInt32(projects[i].NotSellBedNumber)) + "</td>");
                                        sbTable.Append("<td style='width:80px;'>" + projects[i].ProjectCycleState + "</td>");
                                        sbTable.Append("<td style='width:100px;text-align: center'>" + (projects[i].StartDate.ToString() != "1900-1-1 0:00:00" && projects[i].StartDate != null
                                                                ? Convert.ToDateTime(projects[i].StartDate).ToString("yyyy/MM/dd") : "") + "</td>");
                                        sbTable.Append("<td style='width:100px;text-align: center'>" + (projects[i].EndDate.ToString() != "1900-1-1 0:00:00" && projects[i].EndDate != null
                                                                ? Convert.ToDateTime(projects[i].EndDate).ToString("yyyy/MM/dd") : "") + "</td>");
                                        sbTable.Append("<td style='width:80px;'>" + projects[i].projectDetailName + "</td>");
                                        sbTable.Append("<td style='width:100px;text-align: center'>" + (projects[i].RealStartDate.ToString() != "1900-1-1 0:00:00" && projects[i].RealStartDate != null
                                                                ? Convert.ToDateTime(projects[i].RealStartDate).ToString("yyyy/MM/dd") : "未填写") + "</td>");
                                        //sbTable.Append("<td style='width:100px;text-align: center'>" + ExpectedCompletionDate + "</td>");
                                        sbTable.Append("<td style='width:60px;text-align: center'>" + projects[i].ProjectCompletion + "</td>");
                                        sbTable.Append("<td style='width:160px;text-align:left; border-right: 1px solid #9A9A9A;'>" + (projects[i].ReasonDelaySummary != "" ? projects[i].ReasonDelaySummary : strReasonDelay) + "</td>");
                                        sbTable.Append("</tr>");
                                    }
                                    sbTable.Append(" <tr><td colspan='6' style='border-bottom-style:none;border-left-style:none;border-right-style:none;text-align:left;' > 本邮件发送自微领地PMP平台 （操作者：PMP系统）</td><td colspan='10' style='border-bottom-style:none;border-left-style:none;border-right-style:none;' ></td></tr>");
                                    sbTable.Append("</table></br>");

                                    if (!string.IsNullOrEmpty(sbTable.ToString()))
                                    {
                                        List<string> users = EmailService.GetServiceSetEmailToById(SERVICECODE, itemT.ID);
                                        List<string> ccusers = EmailService.GetServiceSetEmailCcById(SERVICECODE, itemT.ID);

                                        EmailService.SendEmail(users, ccusers, null, ConstUtility.EmailTitle, sbTable.ToString());
                                    }
                                    iLog.WriteLog("PMP邮件发送成功", 0);
                                }

                                #endregion

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    iLog.WriteLog("未日审邮件发送失败" + ex.Message, 1);
                }
            }
            //}
            //}
            //else
            //{
            //    iLog.WriteLog("邮件配置服务不启用！", 0);
            //}

            //}
            catch
            {
                //iLog.WriteLog("Error " + e.Message, 1);
                throw;
            }
        }

        public static List<Model.ProjectSummary> ProjectSummaryList()
        {
            List<Model.ProjectSummary> models = new List<Model.ProjectSummary>();
            IDBRepository dbContext = new IDBRepository();
            string sql = @" select Projects.ID,Citys.name CityName,
                            case Items.Type when 9 then '特许加盟' when 10 then '直营' end ItemType ,
                            Items.ItemCode ,
                            ItemName ,Projects.Status,
                            Projects.Name ProjectName,
                            Projects.Room ProjectRoom,
                            Projects.BedNumber,Projects.NotSellRoom,Projects.NotSellBedNumber,
                            case when Projects.Type=1 then '大V' when Projects.Type=5 then '小V'
                            when Projects.Type=6 then '商务' when Projects.Type=7 then '综合'
                            when Projects.Type=8 then '其他' end  ProjectType, 
                            Projects.StartDate ,
                            Projects.EndDate ,
                            Projects.RealStartDate  ,
                            Projects.RealEndDate 
                            from Items 
		                    inner join  Projects on Items.Id=Projects.ItemID
		                    left join Province on Items.ProvinceID=Province.pro_id
		                    left join Citys on Items.CityID=Citys.city_id
		                    left join UserProfiles  u on Projects.ManagerID=u.UserId
		                    left join UserProfiles r on Projects.RMID=r.UserId
			                left join Models m on Projects.ModelID=m.ID
			                where ( RealEndDate is null or  RealEndDate='') and
                             Projects.IsLead=1 order by Projects.Type,Items.CityID,Items.Type,Projects.StartDate";
            models = dbContext.Database.SqlQuery<Model.ProjectSummary>(sql).ToList();
            return models;
        }

        public static string strDelayReasonName(int projectID)
        {
            //            IDBRepository dbContext = new IDBRepository();
            //            var peojectDetailID = 0;
            //            var projectDetail = ProjectDetailList(projectID);
            //            if (projectDetail.Where
            //                                           (x => x.Status == (int)ProjectNodeStatus.DelayNotStarted ||
            //                                                 x.Status == (int)ProjectNodeStatus.DelayStart ||
            //                                                 x.Status == (int)ProjectNodeStatus.Unfinished
            //                                               ).Count() > 0)
            //                peojectDetailID = projectDetail.Where
            //                    (x => x.Status == (int)ProjectNodeStatus.DelayNotStarted ||
            //                          x.Status == (int)ProjectNodeStatus.DelayStart ||
            //                          x.Status == (int)ProjectNodeStatus.Unfinished
            //                        ).LastOrDefault().ID;
            //            else if (projectDetail.Where
            //               (x => x.Status == (int)ProjectNodeStatus.Ongoing ||
            //                                  x.Status == (int)ProjectNodeStatus.EarlyStart
            //                                  ).Count() > 0)
            //                peojectDetailID = projectDetail.Where
            //               (x => x.Status == (int)ProjectNodeStatus.Ongoing ||
            //                                x.Status == (int)ProjectNodeStatus.EarlyStart
            //                 ).LastOrDefault().ID;
            //            else
            //                return "";



            //            string sqlDelayReasonID = " select   DelayReasonID from ProjectNodes where ProjectDetailID " +
            //                                    " in (  select ID from ProjectDetails where ProjectID=" + peojectDetailID + ") " +
            //                                    " and DelayReasonID is not null and DelayReasonID!=''";
            //            var liReasonID = dbContext.Database.SqlQuery<string>(sqlDelayReasonID,
            //                               new SqlParameter("@projectID", projectID)).ToList();

            //            if (liReasonID.Count() <= 0)
            //                return "";

            //            string sql = @" select DelayName from dbo.DelayReasons
            //                           where ID in (" + liReasonID.Last().TrimEnd(',') + ")";
            //            var list = dbContext.Database.SqlQuery<string>(sql).ToList();
            //            string str = "";
            //            foreach (string s in list)
            //            {
            //                str += s + ",";
            //            }
            //            return str.TrimEnd(',');

            IDBRepository dbContext = new IDBRepository();

            string sqlDelayReasonID = @" select   DelayReasonID from ProjectNodes where ProjectDetailID
                                    in (  select ID from ProjectDetails where ProjectID=@projectID)
                                    and DelayReasonID is not null and DelayReasonID!=''";
            var liReasonID = dbContext.Database.SqlQuery<string>(sqlDelayReasonID,
                               new SqlParameter("@projectID", projectID)).ToList();

            if (liReasonID.Count() <= 0)
                return "";

            string sql = @" select DelayName from dbo.DelayReasons
                           where ID in (" + liReasonID.Last().TrimEnd(',') + ")";
            var list = dbContext.Database.SqlQuery<string>(sql).ToList();
            string str = "";
            foreach (string s in list)
            {
                str += s + ",";
            }
            return str.TrimEnd(',');
        }

        public static List<Model.Project> ProjectList()
        {
            List<Model.Project> plist = new List<Model.Project>();
            IDBRepository dbContext = new IDBRepository();

            plist = (from p in dbContext.Projects
                     join i in dbContext.Items on p.ItemID equals i.ID
                     where p.Name != "TEMPLATE" && p.ItemID > 0 && (i.Cancel == null || i.Cancel == false)
                     && p.IsLead == 1
                     select p).OrderBy(p => p.StartDate).ToList();

            return plist;
        }

        public static short GetProjectStatus0(ProjectSummary project, DateTime? startDate, DateTime? endDate)
        {
            short resultStatus = 0;
            if (startDate != null && startDate < project.StartDate)
            {
                resultStatus = (short)ProjectNodeStatus.EarlyStart;
            }
            else if (startDate != null && startDate > project.StartDate)
            {
                resultStatus = (short)ProjectNodeStatus.DelayStart;
            }
            else if (startDate != null && startDate == project.StartDate)
            {
                resultStatus = (short)ProjectNodeStatus.Ongoing;
            }
            else
            {
                //当前时间大于预计开始时间返回延迟未开始，否则返回未到计划
                resultStatus = DateTime.Now.Date.CompareTo(project.StartDate) >= 0 ? (short)ProjectNodeStatus.DelayNotStarted : (short)ProjectNodeStatus.BeforePlan;
            }
            //如果有结束时间则返回完成或者延时完成
            if (endDate != null)
            {
                if (project.EndDate.CompareTo(endDate) >= 0)
                {
                    resultStatus = (short)ProjectNodeStatus.AheadSchedule;
                }
                else
                {
                    resultStatus = (short)ProjectNodeStatus.DelayComplete;
                }
            }
            if (startDate <= project.StartDate && endDate == null)
            {
                resultStatus = DateTime.Now.Date.CompareTo(project.EndDate) > 0 ? (short)ProjectNodeStatus.Unfinished : (short)ProjectNodeStatus.Ongoing;
            }
            return resultStatus;
        }

        public static List<Model.ProjectDetail> ProjectDetailList(int projectId)
        {
            List<Model.ProjectDetail> ProjectDetailList = new List<Model.ProjectDetail>();
            IDBRepository dbContext = new IDBRepository();
            ProjectDetailList = dbContext.ProjectDetails.Where(x => x.ProjectID == projectId).OrderBy(x => x.ModelDetail.Sort).ToList();
            return ProjectDetailList;
        }


        public static List<ProjectNode> GetProjectNodes(ProjectSummary project, int pdid)
        {
            IDBRepository dbContext = new IDBRepository();
            List<ProjectNode> listNode = dbContext.ProjectNodes.Where(x => x.ProjectDetail.ProjectID == project.ID && x.ProjectDetailID == pdid).OrderBy(x => x.ProjectDetail.ModelDetail.Sort).ToList();

            return listNode;
        }


        public static short GetProjectStatus1(DateTime StartDate, DateTime EndDate, DateTime? RealStartDate, DateTime? RealEndDate)
        {
            short resultStatus = 0;
            if (RealStartDate != null && RealStartDate < StartDate)
            {
                resultStatus = (short)ProjectNodeStatus.EarlyStart;
            }
            else if (RealStartDate != null && RealStartDate > StartDate)
            {
                resultStatus = (short)ProjectNodeStatus.DelayStart;
            }
            else if (RealStartDate != null && RealStartDate == StartDate)
            {
                resultStatus = (short)ProjectNodeStatus.Ongoing;
            }
            else
            {
                //当前时间大于预计开始时间返回延迟未开始，否则返回未到计划
                resultStatus = DateTime.Now.Date.CompareTo(StartDate) > 0 ? (short)ProjectNodeStatus.DelayNotStarted : (short)ProjectNodeStatus.BeforePlan;
            }
            //如果有结束时间则返回完成或者延时完成
            if (RealEndDate != null)
            {
                if (EndDate.CompareTo(RealEndDate) > 0)
                {
                    resultStatus = (short)ProjectNodeStatus.AheadSchedule;
                }
                if (EndDate.CompareTo(RealEndDate) == 0)
                {
                    resultStatus = (short)ProjectNodeStatus.Complete;
                }
                else
                {
                    resultStatus = (short)ProjectNodeStatus.DelayComplete;
                }
            }
            if (RealStartDate <= StartDate && RealEndDate == null)
            {
                resultStatus = DateTime.Now.Date.CompareTo(EndDate) > 0 ? (short)ProjectNodeStatus.Unfinished : (short)ProjectNodeStatus.Ongoing;
            }
            return resultStatus;
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }

        public enum ProjectNodeStatus
        {
            [Description("未到计划")]
            BeforePlan = 1,
            [Description("延时未开始")]
            DelayNotStarted = 2,
            [Description("进行中")]
            Ongoing = 3,
            [Description("提前完成")]
            AheadSchedule = 4,
            [Description("延时完成")]
            DelayComplete = 5,
            [Description("完成")]
            Complete = 6,
            [Description("延时开始")]
            DelayStart = 7,
            [Description("提前开始")]
            EarlyStart = 8,
            [Description("延时未完成")]
            Unfinished = 9
        }


        public enum ItemTypes
        {
            [Description("特许加盟")]
            Franchise = 9,
            [Description("直营")]
            Directly = 10
        }

        public enum ProjectTypes
        {
            [Description("大V")]
            BigV = 1,
            [Description("小V")]
            SmallV = 5,
            [Description("商务")]
            Business = 6,
            [Description("综合")]
            Comprehensive = 7,
            [Description("其他")]
            other = 8
        }


        public static int GetProjectCompletionDays(int projectId)
        {
            IDBRepository dbContext = new IDBRepository();
            int alreadyDaysReal = GetTotalDaysByNodeList(dbContext.ProjectNodes.Where(x => x.ProjectDetail.ProjectID == projectId && (x.Status == (short)ProjectNodeStatus.DelayComplete
                                    || x.Status == (short)ProjectNodeStatus.Complete || x.Status == (short)ProjectNodeStatus.AheadSchedule)).ToList());
            return alreadyDaysReal;
        }
        //获取项目总节点天数
        public static int GetProjectDays(int projectId)
        {
            IDBRepository dbContext = new IDBRepository();
            int allDaysReal = GetTotalDaysByNodeList(dbContext.ProjectNodes.Where(x => x.ProjectDetail.ProjectID == projectId).ToList());//.Sum(x => x.ModelNode.Period);
            return allDaysReal;
        }

        public static int GetTotalDaysByNodeList(List<ProjectNode> nodeList)
        {
            int total = 0;
            foreach (ProjectNode pn in nodeList)
            {
                total += getPeriod(pn.StartDate, pn.EndDate);
            }
            return total;
        }

        //获取周期天数
        private static int getPeriod(DateTime startDate, DateTime endDate)
        {
            TimeSpan ts = endDate - startDate;
            return ts.Days + 1;
        }


        public static PMPCombined DtPMPCombined()
        {
            List<ProjectIntro> liFinished = new List<ProjectIntro>();
            IDBRepository dbContext = new IDBRepository();
            string sqlFinished = @" select project.Type ProjectType,item.Type ItemType ,
                            COUNT(1) ProjectNum,SUM(isnull(project.Room,0)+ isnull(project.NotSellRoom,0)) RoomNumber,
                           SUM( isnull(project.BedNumber,0)+isnull(project.NotSellBedNumber,0)) BedNumber1 from Items item
                            inner join Projects project on item.Id=project.ItemID
                            where  (project.RealEndDate is not null and project.RealEndDate!='')
                            group by project.Type ,item.Type ";
            liFinished = dbContext.Database.SqlQuery<Model.ProjectIntro>(sqlFinished).ToList();



            List<ProjectIntro> liUnFinished = new List<ProjectIntro>();
            string sqlUnFinished = @" select project.Type ProjectType,item.Type ItemType ,
                            COUNT(1) ProjectNum,SUM(isnull(project.Room,0)+isnull(project.NotSellRoom,0)) RoomNumber,
                             SUM(isnull(project.BedNumber,0)+isnull(project.NotSellBedNumber,0)) BedNumber1  from Items item
                            inner join Projects project on item.Id=project.ItemID
                            where  (project.RealEndDate is null or project.RealEndDate='')
                            group by project.Type ,item.Type ";
            liUnFinished = dbContext.Database.SqlQuery<Model.ProjectIntro>(sqlUnFinished).ToList();
            PMPCombined Combined = new PMPCombined();
            if (liUnFinished.Count() > 0)
            {
                for (int i = 0; i < liUnFinished.Count(); i++)
                {
                    if (liUnFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.BigV)
                       && liUnFinished[i].ItemType == Convert.ToInt32(ItemTypes.Directly))
                    {
                        //大V直营项目数未完工
                        Combined.BigVDirectlyProject_unfinished = liUnFinished[i].ProjectNum;
                        //大V直营房间数未完工
                        Combined.BigVDirectlyRoom_unfinished = liUnFinished[i].RoomNumber;
                        //大V直营床位数未完工
                        Combined.BigVDirectlyBed_unfinished = liUnFinished[i].BedNumber1;
                    }


                    if (liUnFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.BigV)
                      && liUnFinished[i].ItemType == Convert.ToInt32(ItemTypes.Franchise))
                    {
                        //大V特许项目数未完工
                        Combined.BigVCharterProject_unfinished = liUnFinished[i].ProjectNum;
                        //大V特许房间数未完工
                        Combined.BigVCharterRoom_unfinished = liUnFinished[i].RoomNumber;
                        //大V特许床位数未完工
                        Combined.BigVCharterBed_unfinished = liUnFinished[i].BedNumber1;
                    }

                    if (liUnFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.SmallV)
                       && liUnFinished[i].ItemType == Convert.ToInt32(ItemTypes.Directly))
                    {
                        //小V直营项目数未完工
                        Combined.SmallVDirectlyProject_unfinished = liUnFinished[i].ProjectNum;
                        //小V直营房间数未完工
                        Combined.SmallVDirectlyRoom_unfinished = liUnFinished[i].RoomNumber;
                        //小V直营床位数未完工
                        Combined.SmallVDirectlyBed_unfinished = liUnFinished[i].BedNumber1;
                    }

                    if (liUnFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.SmallV)
                   && liUnFinished[i].ItemType == Convert.ToInt32(ItemTypes.Franchise))
                    {
                        //小V特许项目数未完工
                        Combined.SmallVCharterProject_unfinished = liUnFinished[i].ProjectNum;
                        //小V特许房间数未完工
                        Combined.SmallVCharterRoom_unfinished = liUnFinished[i].RoomNumber;
                        //小V特许床位数未完工
                        Combined.SmallVCharterBed_unfinished = liUnFinished[i].BedNumber1;
                    }

                }

            }


            if (liFinished.Count() > 0)
            {
                for (int i = 0; i < liFinished.Count(); i++)
                {
                    if (liFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.BigV)
                       && liFinished[i].ItemType == Convert.ToInt32(ItemTypes.Directly))
                    {
                        //大V直营项目数完工
                        Combined.BigVDirectlyProject_finished = liFinished[i].ProjectNum;
                        //大V直营房间数完工
                        Combined.BigVDirectlyRoom_finished = liFinished[i].RoomNumber;
                        //大V直营床位数完工
                        Combined.BigVDirectlyBed_finished = liFinished[i].BedNumber1;
                    }


                    if (liFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.BigV)
                      && liFinished[i].ItemType == Convert.ToInt32(ItemTypes.Franchise))
                    {
                        //大V特许项目数完工
                        Combined.BigVCharterProject_finished = liFinished[i].ProjectNum;
                        //大V特许房间数完工
                        Combined.BigVCharterRoom_finished = liFinished[i].RoomNumber;
                        //大V特许床位数完工
                        Combined.BigVCharterBed_finished = liFinished[i].BedNumber1;
                    }

                    if (liFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.SmallV)
                       && liFinished[i].ItemType == Convert.ToInt32(ItemTypes.Directly))
                    {
                        //小V直营项目数完工
                        Combined.SmallVDirectlyProject_finished = liFinished[i].ProjectNum;
                        //小V直营房间数完工
                        Combined.SmallVDirectlyRoom_finished = liFinished[i].RoomNumber;
                        //小V直营床位数完工
                        Combined.SmallVDirectlyBed_finished = liFinished[i].BedNumber1;
                    }

                    if (liFinished[i].ProjectType == Convert.ToInt32(ProjectTypes.SmallV)
                   && liFinished[i].ItemType == Convert.ToInt32(ItemTypes.Franchise))
                    {
                        //小V特许项目数完工
                        Combined.SmallVCharterProject_finished = liFinished[i].ProjectNum;
                        //小V特许房间数完工
                        Combined.SmallVCharterRoom_finished = liFinished[i].RoomNumber;
                        //小V特许床位数完工
                        Combined.SmallVCharterBed_finished = liFinished[i].BedNumber1;
                    }

                }

            }

            return Combined;







        }





        /// <summary>
        /// 获取邮件收件人
        /// </summary>
        /// <param name="type">0：收件人 1：抄送人</param>
        /// <returns></returns>
        //private static List<string> GetEmailUser(int type)
        //{
        //    List<string> users = new List<string>();
        //    if (type == 0)
        //    {
        //        string userstrs = ConstUtility.EmailToUser;
        //        if (!string.IsNullOrEmpty(userstrs))
        //        {
        //            string[] strs = userstrs.Split(';');
        //            if (strs != null && strs.Length > 0)
        //            {
        //                foreach (string str in strs)
        //                {
        //                    if (!string.IsNullOrEmpty(str))
        //                    {
        //                        users.Add(str);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return users;
        //}

    }
}
