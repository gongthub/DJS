using Microsoft.Office.Interop.Excel;
using RentLoanSummaryNewService.Model;
using RentLoanSummaryNewService.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Service
{
    public class RentLoanSummaryNewService
    {

        public static DJS.SDK.ILog iLog = null;

        private static string ServiceCode = "";
        private static int WarningDate = 0;
        private static int MinWarningDate = 0;
        private static string SystemName = null;
        private static string System = null;
        private IDBRepository dbContext = new IDBRepository();
        //顺序必须与业务环比中的顺序一致，汇总与环比顺序一致
        private static List<string> StatusList = new List<string>() { "未申请租金贷", "待提交", "审核未通过", "待审核", "逾期未还款", "审核通过", "已放款", "正常还款" };

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static RentLoanSummaryNewService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            ServiceCode = ConstUtility.ServiceCode;
            WarningDate = Convert.ToInt32(ConstUtility.WarningDate);
            MinWarningDate = Convert.ToInt32(ConstUtility.MinWarningDate);
            SystemName = ConstUtility.SystemName;
            System = ConstUtility.System;
        }

        #endregion


        /// <summary>
        /// 发送租金贷汇总
        /// </summary>
        public void SendRentLoanSum()
        {
            try
            {
                if (EmailService.ServiceIsStart(ServiceCode))
                {
                    List<Model.AMSServiceSetEmail> emailLists = EmailService.GetServiceSetEmailStoresList(ServiceCode);
                    if (emailLists != null && emailLists.Count > 0)
                    {
                        foreach (Model.AMSServiceSetEmail itemT in emailLists)
                        {
                            try
                            {
                                if (itemT.IsSum)
                                {
                                    itemT.StoreID = EmailService.GetStoreIds();
                                }

                                string strTable = GetContent();
                                if (!string.IsNullOrEmpty(strTable))
                                {
                                    string file = null;
                                    List<string> users = EmailService.GetServiceSetEmailToById(ServiceCode, itemT.ID);
                                    List<string> ccusers = EmailService.GetServiceSetEmailCcById(ServiceCode, itemT.ID);

                                    EmailService.SendEmail(users, ccusers, null, SystemName, strTable, file);
                                }
                            }
                            catch (Exception ex)
                            {
                                iLog.WriteLog(ex.Message, 1);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 生成附件--暂时没用
        /// </summary>
        /// <returns></returns>
        public string GetFile()
        {
            string file = null;

            //获取模板名
            string strTemplateName = Achieve.TemplatePath + ConstUtility.Temp_RentLoan;
            iLog.WriteLog("获取文件目录成功", 0);
            iLog.WriteLog(strTemplateName, 0);

            Application excelApp = new Application();
            FileOper oper = new FileOper();

            //创建需要保存的本地文件夹
            string tempDD = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "\\";
            string DayDirectory = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
            string SaveExcelDirectory = ConstUtility.SavesFilePath + tempDD + DayDirectory + "\\";
            oper.CreateDirectory(SaveExcelDirectory);

            var rentLoanRiskLists = GetRentLoanRisk();
            if (rentLoanRiskLists != null && rentLoanRiskLists.Count > 0)
            {
                //打开模板
                Workbook workbook = excelApp.Workbooks.Open(strTemplateName);
                Sheets excelSheets = workbook.Worksheets;
                string currentSheet = "租金贷明细";
                Worksheet excelWorksheet = (Worksheet)excelSheets.get_Item(currentSheet);

                int numTab = 2;
                DateTime nowTime = DateTime.Now;
                foreach (var temp in rentLoanRiskLists)
                {
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)excelWorksheet.Rows[numTab + 1, Type.Missing];
                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                    excelWorksheet.Cells[numTab, 1] = temp.StoreName;
                    excelWorksheet.Cells[numTab, 2] = temp.Name;
                    excelWorksheet.Cells[numTab, 3] = temp.Phone;
                    excelWorksheet.Cells[numTab, 4] = temp.ContractNo;
                    excelWorksheet.Cells[numTab, 5] = temp.StartDate;
                    excelWorksheet.Cells[numTab, 6] = temp.EndDate;
                    excelWorksheet.Cells[numTab, 7] = temp.PEndDate;
                    excelWorksheet.Cells[numTab, 8] = temp.ModifiedDate;
                    excelWorksheet.Cells[numTab, 9] = temp.FullName;
                    excelWorksheet.Cells[numTab, 10] = temp.Deposit;
                    excelWorksheet.Cells[numTab, 11] = temp.FillDeposit;
                    excelWorksheet.Cells[numTab, 12] = temp.DepositLive;
                    excelWorksheet.Cells[numTab, 13] = temp.DayCountLive;
                    excelWorksheet.Cells[numTab, 14] = GetEnumDescription(typeof(EnumUtility.RLoanStatus), temp.Status);
                    if (temp.Status == (int)EnumUtility.RLoanStatus.未申请租金贷)
                        excelWorksheet.Cells[numTab, 15] = "资料不全未申请租金贷超过" + WarningDate + "天";
                    else if (temp.Status == (int)EnumUtility.RLoanStatus.待提交)
                        excelWorksheet.Cells[numTab, 15] = "资料齐全待提交超过" + WarningDate + "天";
                    else if (temp.Status == (int)EnumUtility.RLoanStatus.待审核)
                        excelWorksheet.Cells[numTab, 15] = "未审核超过" + WarningDate + "天";
                    else if (temp.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                        excelWorksheet.Cells[numTab, 15] = "审核未通过" + WarningDate + "天";

                    else if (temp.Status == (int)EnumUtility.RLoanStatus.审核通过)
                        excelWorksheet.Cells[numTab, 15] = "审核通过超过" + MinWarningDate + "天";
                    else if (temp.Status == (int)EnumUtility.RLoanStatus.正常还款)
                        excelWorksheet.Cells[numTab, 15] = "正常还款超过" + MinWarningDate + "天";
                    else if (temp.Status == (int)EnumUtility.RLoanStatus.逾期未还款)
                        excelWorksheet.Cells[numTab, 15] = "逾期未还款超过" + MinWarningDate + "天";
                    else if (temp.Status != (int)EnumUtility.RLoanStatus.已放款)
                        excelWorksheet.Cells[numTab, 15] = "已放款超过" + MinWarningDate + "天";
                    else
                        excelWorksheet.Cells[numTab, 15] = null;

                    excelWorksheet.Cells[numTab, 16] = temp.BankName;


                    if (temp.Status == (int)EnumUtility.RLoanStatus.未申请租金贷 || temp.Status == (int)EnumUtility.RLoanStatus.待提交 || temp.Status == (int)EnumUtility.RLoanStatus.审核未通过)
                    {
                        //42和bgcolor='#95CACA'类似
                        excelWorksheet.Cells[numTab, 1].Interior.ColorIndex = 42;
                        excelWorksheet.Cells[numTab, 2].Interior.ColorIndex = 42;
                        excelWorksheet.Cells[numTab, 13].Interior.ColorIndex = 42;
                    }
                    else if (temp.Status == (int)EnumUtility.RLoanStatus.待审核 || temp.Status == (int)EnumUtility.RLoanStatus.逾期未还款)
                    {
                        //Orange和bgcolor='#FF8000'类似
                        excelWorksheet.Cells[numTab, 1].Interior.Color = Color.Orange;
                        excelWorksheet.Cells[numTab, 2].Interior.Color = Color.Orange;
                        excelWorksheet.Cells[numTab, 13].Interior.Color = Color.Orange;
                    }
                    numTab++;
                }

                string strFileName = SaveExcelDirectory + "租金贷汇总明细" + "." + ConstUtility.Suffix;

                oper.ExistFile(strFileName);
                workbook.SaveAs(strFileName);
                workbook.Close();
                excelApp.Quit();

                file = ZipFile(SaveExcelDirectory, DayDirectory, tempDD); //压缩文件 
            }
            return file;
        }

        public string ZipFile(string SaveExcelDirectory, string DayDirectory, string tempDD)
        {
            iLog.WriteLog("开始压缩" + DayDirectory + "文件夹", 0);
            string ZipDirectory = ConstUtility.ZipFilePath;
            string ZipFileName = ZipDirectory + tempDD + DayDirectory + ".zip";
            FileOper oper = new FileOper();
            oper.CreateDirectory(ZipDirectory + tempDD);//创建文件夹
            oper.ExistFile(ZipFileName);
            if (ZipHelper.Zip(SaveExcelDirectory, ZipFileName)) //判断是否压缩成功
            {
                iLog.WriteLog("压缩" + DayDirectory + "文件夹成功", 0);
                return ZipFileName;
            }
            else
            {
                iLog.WriteLog("压缩" + DayDirectory + "文件夹失败", 0);
                return null;
            }
        }

        private static List<RentLoanRisk> GetRentLoanRisk()
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
                                                 ) te
                                                 order by case when Status=100 then 1
				                                                when Status=0 then 2
				                                                when Status=3 then 3
				                                                when Status=1 then 4
				                                                when Status=9 then 5
				                                                when Status=2 then 6
				                                                when Status=4 then 7
				                                                else 8 end asc ,DayCountLive", WarningDate, MinWarningDate);
                List<RentLoanRisk> riskList = SqlHelper.ListEntity<RentLoanRisk>(strSQL);
                return riskList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 租金贷汇总所有内容
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            string content = null;
            var storeSystems = dbContext.Stores.ToList();

            //邮件发送是早上7点左右，内容应为昨天和前天的
            DateTime dtNow = DateTime.Now.AddDays(-1).Date;
            DateTime dtUpDay = DateTime.Now.AddDays(-2).Date;
            List<RentLoanSummary> statusSumList = dbContext.RentLoanSummaries.Where(t => t.Date == dtNow).ToList();
            List<RentLoanSummary> statusSumListUpDay = dbContext.RentLoanSummaries.Where(t => t.Date == dtUpDay).ToList();

            //content = GetRentLoanStatusSum(statusSumList, storeSystems);

            content = GetProportionTable(statusSumList, statusSumListUpDay, storeSystems);

            return content;
        }

        /// <summary>
        /// 租金贷状态汇总内容--暂时没用
        /// </summary>
        private string GetRentLoanStatusSum(List<RentLoanSummary> statusSumList, List<Store> storeSystems)
        {
            try
            {
                if (statusSumList != null && statusSumList.Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='color: red;'>您好，以下是截止昨日" + SystemName + "的信息，请知晓！</td></tr></table><br/>");

                    #region 社区租金贷业务汇总
                    sbTable.Append("<span style='margin-left:10px;font-size:18px;font-weight:bold;color: red;'>社区租金贷业务汇总(" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + ")</span>");
                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");

                    //1.动态标题
                    sbTable.Append("<tr><td style='text-align:center;'>社区</td>");
                    foreach (var statusTemp in StatusList)
                    {
                        sbTable.Append("<td>" + statusTemp + "</td>");
                    }
                    sbTable.Append("<td>合计</td></tr>");

                    var storeList = statusSumList.GroupBy(t => t.StoreID);
                    foreach (var store in storeList)
                    {
                        var listTemp = statusSumList.Where(t => t.StoreID == store.Key).ToList();
                        var storeSystem = storeSystems.Where(t => t.ID == store.Key).FirstOrDefault();

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + storeSystem.Name + "</td>");
                        //2.动态明细值
                        foreach (var statusTemp in StatusList)
                        {
                            int status = GetValueByDesc(typeof(EnumUtility.RLoanStatus), statusTemp);
                            var statusSum = listTemp.Where(t => t.RentLoanStatus == status).FirstOrDefault();
                            sbTable.Append("<td>" + (statusSum == null ? 0 : statusSum.ContractCount) + "</td>");
                        }
                        sbTable.Append("<td>" + listTemp.Sum(t => t.ContractCount) + "</td>");
                        sbTable.Append("</tr>");
                    }

                    sbTable.Append("<tr>");
                    sbTable.Append("<td>合计</td>");
                    //3.动态合计值
                    foreach (var statusTemp in StatusList)
                    {
                        int status = GetValueByDesc(typeof(EnumUtility.RLoanStatus), statusTemp);
                        var statusSum = statusSumList.Where(t => t.RentLoanStatus == status).ToList();
                        sbTable.Append("<td>" + statusSum.Sum(t => t.ContractCount) + "</td>");
                    }
                    sbTable.Append("<td>" + statusSumList.Sum(t => t.ContractCount) + "</td>");

                    sbTable.Append("</tr>");
                    sbTable.Append("</table><br/>");
                    #endregion

                    return sbTable.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 租金贷环比样表
        /// </summary>
        private string GetProportionTable(List<RentLoanSummary> statusSumList, List<RentLoanSummary> statusSumListUpDay, List<Store> storeSystems)
        {
            try
            {
                if (statusSumList.Count > 0 || statusSumListUpDay.Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='font-size:14px;'>各位领导, 好：<br /><span style=\"margin-left:24px;\">下表为" + DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日") + "" + System + "门店租金贷业务信息汇总，请您查收。</span></td></tr></table>");

                    #region 描述
                    sbTable.Append(@"<br /><table><tr><td style='font-size:14px;'><span style='margin-left:24px;'>预警信息说明：</span></td></tr>
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

                    #region 标题
                    //sbTable.Append("<span style='margin-left:10px;font-size:18px;font-weight:bold;color: red;'>社区租金贷业务预警汇总(" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + ")</span>");
                    sbTable.Append("<table style=\"text-align:center;\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><span style=\"font-size:20px;font-weight:bold;\">社区租金贷业务预警汇总</span></td></tr><tr><td>" + DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日") + "</td></tr><tr><td>");
                    sbTable.Append("<table style='width:4600px;' cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append(@"<tr style='height:30px;text-align:center;font-size:16px;font-weight:bold;' bgcolor='#E0E0E0'>
                                        <td style='width:250px;'>预警跟踪部门</td>
                                        <td colspan='6' style='width:480px;'>对应社区</td>
                                        <td colspan='6' style='width:480px;'>对应社区</td>
                                        <td colspan='6' style='width:480px;'>对应社区</td>
                                        <td colspan='6' style='width:480px;'>财务部/融资部协助</td>
                                        <td colspan='6' style='width:480px;'>财务部/对应社区</td>
                                        <td colspan='6' style='width:480px;'>财务部/融资部协助</td>
                                        <td colspan='6' style='width:480px;'>财务部/融资部协助</td>
                                        <td colspan='6' style='width:480px;'>财务部</td>
                                        <td colspan='6' style='width:480px;'></td></tr>");
                    sbTable.Append("<tr><td rowspan='2' style='width:250px;height:40px;text-align:center;font-size:14px;font-weight:bold;'>社区</td>");
                    //动态状态标题
                    foreach (var statusTemp in StatusList)
                    {
                        if (statusTemp == EnumUtility.RLoanStatus.未申请租金贷.ToString() || statusTemp == EnumUtility.RLoanStatus.待提交.ToString() || statusTemp == EnumUtility.RLoanStatus.审核未通过.ToString())
                            sbTable.Append("<td colspan='6' style='width:480px;text-align:center;font-size:14px;font-weight:bold;' bgcolor='#95CACA'>" + statusTemp + "</td>");
                        else if (statusTemp == EnumUtility.RLoanStatus.待审核.ToString() || statusTemp == EnumUtility.RLoanStatus.逾期未还款.ToString())
                            sbTable.Append("<td colspan='6' style='width:480px;text-align:center;font-size:14px;font-weight:bold;' bgcolor='#FF8000'>" + statusTemp + "</td>");
                        else
                            sbTable.Append("<td colspan='6' style='width:480px;text-align:center;font-size:14px;font-weight:bold;'>" + statusTemp + "</td>");
                    }
                    sbTable.Append("<td colspan='6' style='width:480px;text-align:center;font-size:14px;font-weight:bold;'>合计</td></tr>");

                    //标题明细
                    sbTable.Append(@"<tr><td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:100px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:100px;'>金额</td>

                                        <td style='width:60px;'>上日数</td>
                                        <td style='width:120px;'>金额</td>
                                        <td style='width:60px;'>本日数</td>
                                        <td style='width:120px;'>金额</td>
                                        <td style='width:60px;'>环比</td>
                                        <td style='width:120px;'>金额</td></tr>");
                    #endregion

                    IEnumerable<IGrouping<int, RentLoanSummary>> storeList = null;
                    if (statusSumList.Count > 0)
                        storeList = statusSumList.GroupBy(t => t.StoreID);
                    else
                        storeList = statusSumListUpDay.GroupBy(t => t.StoreID);

                    foreach (var store in storeList)
                    {
                        var listTemp = statusSumList.Where(t => t.StoreID == store.Key).ToList();
                        var listTempUpDay = statusSumListUpDay.Where(t => t.StoreID == store.Key).ToList();

                        var storeSystem = storeSystems.Where(t => t.ID == store.Key).FirstOrDefault();
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + storeSystem.Name + "</td>");

                        #region 门店明细
                        foreach (var statusTemp in StatusList)
                        {
                            int status = GetValueByDesc(typeof(EnumUtility.RLoanStatus), statusTemp);
                            var commonSumUp = listTempUpDay.Where(t => t.RentLoanStatus == status).FirstOrDefault();
                            var commonSum = listTemp.Where(t => t.RentLoanStatus == status).FirstOrDefault();

                            //上日数和金额
                            sbTable.Append("<td>" + (commonSumUp == null ? 0 : commonSumUp.ContractCount) + "</td>");
                            sbTable.Append("<td>" + (commonSumUp == null ? "0" : commonSumUp.TotalAmount.ToString("N0")) + "</td>");
                            //本日数和金额
                            sbTable.Append("<td>" + (commonSum == null ? 0 : commonSum.ContractCount) + "</td>");
                            sbTable.Append("<td>" + (commonSum == null ? "0" : commonSum.TotalAmount.ToString("N0")) + "</td>");
                            //环比和金额
                            if (commonSum != null && commonSumUp == null)
                            {
                                sbTable.Append("<td>" + commonSum.ContractCount + "</td>");
                                sbTable.Append("<td>" + commonSum.TotalAmount.ToString("N0") + "</td>");
                            }
                            else if (commonSum == null && commonSumUp != null)
                            {
                                sbTable.Append("<td style='color: red;'>" + (-commonSumUp.ContractCount) + "</td>");
                                sbTable.Append("<td style='color: red;'>" + (-commonSumUp.TotalAmount).ToString("N0") + "</td>");
                            }
                            else if (commonSum != null && commonSumUp != null)
                            {
                                if (commonSum.ContractCount - commonSumUp.ContractCount < 0)
                                {
                                    sbTable.Append("<td style='color: red;'>" + (commonSum.ContractCount - commonSumUp.ContractCount) + "</td>");
                                }
                                else
                                {
                                    sbTable.Append("<td>" + (commonSum.ContractCount - commonSumUp.ContractCount) + "</td>");
                                }

                                if (commonSum.TotalAmount - commonSumUp.TotalAmount < 0)
                                {
                                    sbTable.Append("<td style='color: red;'>" + (commonSum.TotalAmount - commonSumUp.TotalAmount).ToString("N0") + "</td>");
                                }
                                else
                                {
                                    sbTable.Append("<td>" + (commonSum.TotalAmount - commonSumUp.TotalAmount).ToString("N0") + "</td>");
                                }
                            }
                            else
                            {
                                sbTable.Append("<td>" + 0 + "</td>");
                                sbTable.Append("<td>" + 0 + "</td>");
                            }
                        }
                        //合计列
                        //上日数和金额
                        sbTable.Append("<td>" + listTempUpDay.Sum(t => t.ContractCount) + "</td>");
                        sbTable.Append("<td>" + listTempUpDay.Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                        //本日数和金额
                        sbTable.Append("<td>" + listTemp.Sum(t => t.ContractCount) + "</td>");
                        sbTable.Append("<td>" + listTemp.Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                        //环比和金额
                        var cCount = listTemp.Sum(t => t.ContractCount) - listTempUpDay.Sum(t => t.ContractCount);
                        var cTotalAmount = listTemp.Sum(t => t.TotalAmount) - listTempUpDay.Sum(t => t.TotalAmount);
                        if (cCount < 0)
                        {
                            sbTable.Append("<td style='color: red;'>" + cCount + "</td>");
                        }
                        else
                        {
                            sbTable.Append("<td>" + cCount + "</td>");
                        }
                        if (cTotalAmount < 0)
                        {
                            sbTable.Append("<td style='color: red;'>" + cTotalAmount.ToString("N0") + "</td>");
                        }
                        else
                        {
                            sbTable.Append("<td>" + cTotalAmount.ToString("N0") + "</td>");
                        }
                        sbTable.Append("</tr>");

                        #endregion
                    }

                    #region 合计行
                    sbTable.Append("<tr>");
                    sbTable.Append("<td>合计</td>");

                    foreach (var statusTemp in StatusList)
                    {
                        int status = GetValueByDesc(typeof(EnumUtility.RLoanStatus), statusTemp);
                        //上日数和金额
                        sbTable.Append("<td>" + statusSumListUpDay.Where(t => t.RentLoanStatus == status).ToList().Sum(t => t.ContractCount) + "</td>");
                        sbTable.Append("<td>" + statusSumListUpDay.Where(t => t.RentLoanStatus == status).ToList().Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                        //本日数和金额
                        sbTable.Append("<td>" + statusSumList.Where(t => t.RentLoanStatus == status).ToList().Sum(t => t.ContractCount) + "</td>");
                        sbTable.Append("<td>" + statusSumList.Where(t => t.RentLoanStatus == status).ToList().Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                        //环比
                        var sumList = statusSumList.Where(t => t.RentLoanStatus == status).ToList();
                        var sumListUpDay = statusSumListUpDay.Where(t => t.RentLoanStatus == status).ToList();
                        if (sumList.Count == 0 && sumListUpDay.Count > 0)
                        {
                            sbTable.Append("<td style='color: red;'>" + (-sumListUpDay.Sum(t => t.ContractCount)) + "</td>");
                            sbTable.Append("<td style='color: red;'>" + (-sumListUpDay.Sum(t => t.TotalAmount)).ToString("N0") + "</td>");
                        }
                        else if (sumList.Count > 0 && sumListUpDay.Count == 0)
                        {
                            sbTable.Append("<td>" + sumList.Sum(t => t.ContractCount) + "</td>");
                            sbTable.Append("<td>" + sumList.Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                        }
                        else if (sumList.Count > 0 && sumListUpDay.Count > 0)
                        {
                            var tempCount = sumList.Sum(t => t.ContractCount) - sumListUpDay.Sum(t => t.ContractCount);
                            var tempTotalAmount = sumList.Sum(t => t.TotalAmount) - sumListUpDay.Sum(t => t.TotalAmount);
                            if (tempCount < 0)
                            {
                                sbTable.Append("<td  style='color: red;'>" + tempCount + "</td>");
                            }
                            else
                            {
                                sbTable.Append("<td>" + tempCount + "</td>");
                            }
                            if (tempTotalAmount < 0)
                            {
                                sbTable.Append("<td  style='color: red;'>" + tempTotalAmount.ToString("N0") + "</td>");
                            }
                            else
                            {
                                sbTable.Append("<td>" + tempTotalAmount.ToString("N0") + "</td>");
                            }
                        }
                        else
                        {
                            sbTable.Append("<td>" + 0 + "</td>");
                            sbTable.Append("<td>" + 0 + "</td>");
                        }
                    }

                    //合计中的合计
                    //上日数和金额
                    sbTable.Append("<td>" + statusSumListUpDay.Sum(t => t.ContractCount) + "</td>");
                    sbTable.Append("<td>" + statusSumListUpDay.Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                    //本日数和金额
                    sbTable.Append("<td>" + statusSumList.Sum(t => t.ContractCount) + "</td>");
                    sbTable.Append("<td>" + statusSumList.Sum(t => t.TotalAmount).ToString("N0") + "</td>");
                    //环比和金额
                    var ccount = statusSumList.Sum(t => t.ContractCount) - statusSumListUpDay.Sum(t => t.ContractCount);
                    var totalAmount = statusSumList.Sum(t => t.TotalAmount) - statusSumListUpDay.Sum(t => t.TotalAmount);
                    if (ccount < 0)
                    {
                        sbTable.Append("<td style='color: red;'>" + (statusSumList.Sum(t => t.ContractCount) - statusSumListUpDay.Sum(t => t.ContractCount)) + "</td>");
                    }
                    else
                    {
                        sbTable.Append("<td>" + (statusSumList.Sum(t => t.ContractCount) - statusSumListUpDay.Sum(t => t.ContractCount)) + "</td>");
                    }
                    if (totalAmount < 0)
                    {
                        sbTable.Append("<td style='color: red;'>" + (statusSumList.Sum(t => t.TotalAmount) - statusSumListUpDay.Sum(t => t.TotalAmount)).ToString("N0") + "</td>");
                    }
                    else
                    {
                        sbTable.Append("<td>" + (statusSumList.Sum(t => t.TotalAmount) - statusSumListUpDay.Sum(t => t.TotalAmount)).ToString("N0") + "</td>");
                    }
                    sbTable.Append("</tr>");
                    #endregion

                    sbTable.Append("</table></td></tr></table><br/>");

                    return sbTable.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// /根据枚举描述获取枚举值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetValueByDesc(Type type, string desc)
        {
            int valueT = 0;
            if (type.IsEnum)
            {
                Array _enumValues = Enum.GetValues(type);
                foreach (Enum value in _enumValues)
                {
                    EnumModel model = new EnumModel();
                    model.Value = Convert.ToInt32(value);
                    string desct = GetEnumDescription(value);
                    if (desc == desct)
                    {
                        valueT = Convert.ToInt32(value);
                    }
                }
            }
            return valueT;
        }

        /// <summary>
        /// 获取枚举类子项描述信息
        /// </summary>
        /// <param name="enumSubitem">枚举类子项</param>        
        public static string GetEnumDescription(Enum enumSubitem)
        {
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
            Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return strValue;
            }
            else
            {
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
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

    public class EnumModel
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
