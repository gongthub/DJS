using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using EmailToDayReportSumExcel.DBHelper;
using EmailToDayReportSumExcel.Utils;
using EmailToDayReportSumExcel.Model;
using System.IO;
namespace EmailToDayReportSumExcel.Service
{
    public class EmailToDayReportSumExcel
    {
        public static readonly IDBHelper DbHelper = DBFactory.CreateDBHelper(ConstUtility.ConnType, ConstUtility.ConnStr);

        public static DJS.SDK.ILog iLog = null;
        private static string SERVICECODE = "";


        static EmailToDayReportSumExcel()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        public void RepoertExcel()
        {
            if (EmailUtility.ServiceIsStart(SERVICECODE))
            {
                string strTemplateName = Achieve.TemplatePath + ConstUtility.Temp_DayReprotSum;
                iLog.WriteLog("获取文件目录成功", 0);
                iLog.WriteLog(strTemplateName, 0);
                DayReprotSumExportExcel("门店汇总日报", strTemplateName);
                GC.Collect();
            }
            else
            {
                iLog.WriteLog("邮件配置服务不启用！", 0);
            }
        }


        /// <summary>
        /// 根据模板填充数据—门店汇总日报
        /// </summary>
        /// <param name="currentSheetName"></param>
        /// <param name="strTemplateName"></param>
        public void DayReprotSumExportExcel(string currentSheetName, string strTemplateName)
        {
            try
            {
                Application excelApp = new Application();

                string tempDD = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "\\";

                FileOper oper = new FileOper();

                string sqlFileDirectory = Achieve.SqlPath;

                //获取需要取数的门店
                //List<UserStore> userStores = GetUserStore().Where(t => t.Type == 3).ToList();


                List<Model.AMSServiceSetEmail> emailLists = EmailUtility.GetServiceSetEmailStoresList(SERVICECODE);
                if (emailLists != null && emailLists.Count > 0)
                {
                    foreach (Model.AMSServiceSetEmail itemT in emailLists)
                    {
                        try
                        {
                            if (itemT.IsSum)
                            {
                                itemT.StoreID = EmailUtility.GetStoreIds();
                            }
                            string DayDirectory = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
                            string SaveExcelDirectory = ConstUtility.SavesFilePath + tempDD + DayDirectory + "\\";
                            oper.CreateDirectory(SaveExcelDirectory);//创建需要保存的文件夹
                            System.Data.DataTable tabledeteil = new System.Data.DataTable();
                            string storeIDs = itemT.StoreID;

                            storeIDs = storeIDs.Replace(",", "','");

                            System.Data.DataTable dt = GetDayReportSumData(storeIDs);

                            Workbook workbook = excelApp.Workbooks.Open(strTemplateName);
                            Sheets excelSheets = workbook.Worksheets;
                            string currentSheet = currentSheetName;
                            Worksheet excelWorksheet = (Worksheet)excelSheets.get_Item(currentSheet);

                            excelWorksheet.Cells[1, 5] = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                            int nColumnCount = 4;
                            //1.填写各个门店数据
                            List<string> storeIDList = storeIDs.Split(new string[] { "','" }, System.StringSplitOptions.None).ToList();
                            storeIDList.Add("999");
                            for (int i = 0; i < storeIDList.Count; i++)
                            {
                                if (dt.Select(string.Format(@"StoreID='{0}'", storeIDList[i])).Length > 0)
                                {
                                    //门店名称
                                    excelWorksheet.Cells[2, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}'", storeIDList[i]))[0]["StoreName"].ToString();
                                    //来电：58同城
                                    excelWorksheet.Cells[3, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=30", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：58同城累计
                                    excelWorksheet.Cells[4, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=301", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：赶集
                                    excelWorksheet.Cells[5, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=31", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：赶集累计
                                    excelWorksheet.Cells[6, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=311", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：地推
                                    excelWorksheet.Cells[7, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=32", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：地推累计
                                    excelWorksheet.Cells[8, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=321", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：公司线上
                                    excelWorksheet.Cells[9, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=33", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：公司线上累计
                                    excelWorksheet.Cells[10, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=331", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：公司线下
                                    excelWorksheet.Cells[11, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=34", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：公司线下累计
                                    excelWorksheet.Cells[12, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=341", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：其它
                                    excelWorksheet.Cells[13, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=35", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：其它累计
                                    excelWorksheet.Cells[14, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=351", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来电：接听量(起)
                                    double d36 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=36", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[15, nColumnCount] = d36;
                                    //来电：来访转化率
                                    double d37 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=37", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[16, nColumnCount] = d36 == 0 ? 0 : d37 / d36;
                                    //来访：58同城
                                    excelWorksheet.Cells[17, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=38", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：58同城累计
                                    excelWorksheet.Cells[18, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=381", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：赶集
                                    excelWorksheet.Cells[19, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=39", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：赶集累计
                                    excelWorksheet.Cells[20, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=391", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：地推
                                    excelWorksheet.Cells[21, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=40", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：地推累计
                                    excelWorksheet.Cells[22, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=401", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：公司线上
                                    excelWorksheet.Cells[23, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=41", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：公司线上累计
                                    excelWorksheet.Cells[24, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=411", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：公司线下
                                    excelWorksheet.Cells[25, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=42", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：公司线下累计
                                    excelWorksheet.Cells[26, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=421", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：其它
                                    excelWorksheet.Cells[27, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=43", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：其它累计
                                    excelWorksheet.Cells[28, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=431", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //来访：来人数量
                                    double d44 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=44", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[29, nColumnCount] = d44;
                                    //来访：潜在客户数量
                                    double d45 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=45", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[31, nColumnCount] = d45;
                                    //来访：订单转化率
                                    excelWorksheet.Cells[30, nColumnCount] = d44 == 0 ? 0 : d45 / d44;
                                    //签约量：新签数量【线上】
                                    double d46 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=46", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    double d461 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=461", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[32, nColumnCount] = d461;
                                    //签约量：新签数量【线下】
                                    excelWorksheet.Cells[33, nColumnCount] = d46 - d461;
                                    //签约量：续签数量
                                    excelWorksheet.Cells[34, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=47", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：成交均价(月)
                                    double d9 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=9", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    double d10 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=10", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[35, nColumnCount] = d10 == 0 ? 0 : Math.Round(d9 / d10, 0, MidpointRounding.AwayFromZero);
                                    //流水：定金【预定，预约】
                                    excelWorksheet.Cells[36, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=1", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：定金个数【预定，预约】
                                    excelWorksheet.Cells[37, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=1001", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：租金收入【合同】
                                    excelWorksheet.Cells[38, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=2", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：押金收入【合同,商铺】
                                    excelWorksheet.Cells[39, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=3", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：水电收入【合同,门店,商铺】
                                    excelWorksheet.Cells[40, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=4", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：停车费收入【合同,门店】
                                    excelWorksheet.Cells[41, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=5", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：公共厨房收入【合同】
                                    excelWorksheet.Cells[42, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=6", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：线上租金收入【官网】
                                    excelWorksheet.Cells[43, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=7", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：商铺收入【合同(租金)】
                                    excelWorksheet.Cells[44, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=8", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //流水：其他收入【合同.门店】
                                    excelWorksheet.Cells[45, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=11", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //租金贷：租金贷数量
                                    double d48 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=48", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[46, nColumnCount] = d48;
                                    //租金贷：租金贷比例
                                    excelWorksheet.Cells[47, nColumnCount] = d46 == 0 ? 0 : d48 / d46;
                                    //租金贷：累计租金贷签约数量
                                    excelWorksheet.Cells[48, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=49", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //租金贷：累计租金贷放款数量
                                    excelWorksheet.Cells[49, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=50", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //租金贷：累计租金贷放款金额
                                    excelWorksheet.Cells[50, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=57", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //租金贷：累计租金贷退场数量
                                    excelWorksheet.Cells[51, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=551", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //综合：总房间数
                                    double d53 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=53", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[52, nColumnCount] = d53;
                                    //综合：在住房间数【线上】
                                    double d51 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=51", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    double d511 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=511", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[53, nColumnCount] = d511;
                                    //综合：在住房间数【线下】
                                    excelWorksheet.Cells[54, nColumnCount] = d51 - d511;
                                    //综合：预定房间数【预定,预约】
                                    double d52 = double.Parse(dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=52", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString());
                                    excelWorksheet.Cells[55, nColumnCount] = d52;
                                    //综合：退租房间数
                                    excelWorksheet.Cells[56, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=55", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //综合：续租数
                                    excelWorksheet.Cells[57, nColumnCount] = dt.Select(string.Format(@"StoreID='{0}' AND SubjectType=56", storeIDList[i]))[0]["SUMCoutOrAccount"].ToString();
                                    //综合：可出租房间数(总房量-在住-预定房间数)
                                    excelWorksheet.Cells[58, nColumnCount] = d53 - d51 - d52;
                                    //综合：日平均去化房间量(MTD)
                                    double dQuHua = GetQuHuaByStoreID(storeIDList[i]);
                                    if (storeIDList[i] == "999" && storeIDList.Count > 1)
                                    {
                                        dQuHua = Math.Round(dQuHua / (storeIDList.Count - 1), 0, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        dQuHua = Math.Round(dQuHua, 0, MidpointRounding.AwayFromZero);
                                    }
                                    excelWorksheet.Cells[59, nColumnCount] = dQuHua;
                                    //综合：空置房间数量(总房量-在住)
                                    excelWorksheet.Cells[60, nColumnCount] = d53 - d51;
                                    //综合：在住率
                                    excelWorksheet.Cells[61, nColumnCount] = d53 == 0 ? 0 : d51 / d53;
                                    //综合：预定+在住(率)
                                    excelWorksheet.Cells[62, nColumnCount] = d53 == 0 ? 0 : (d51 + d52) / d53;

                                    nColumnCount++;
                                }
                            }
                            //2.填写门店汇总数据

                            string strFileName = SaveExcelDirectory + "门店汇总日报" + "." + ConstUtility.Suffix;

                            oper.ExistFile(strFileName);
                            workbook.SaveAs(strFileName);
                            workbook.Close();
                            excelApp.Quit();

                            List<string> sTo = EmailUtility.GetServiceSetEmailToById(SERVICECODE, itemT.ID);
                            List<string> sCC = EmailUtility.GetServiceSetEmailCcById(SERVICECODE, itemT.ID);

                            ZipFile(SaveExcelDirectory, DayDirectory, tempDD, sTo, sCC);

                        }
                        catch (Exception ex)
                        {
                            iLog.WriteLog("异常：" + ex.Message, 1);
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
        /// 压缩文件
        /// </summary>
        /// <param name="SaveExcelDirectory"></param>
        /// <param name="DayDirectory"></param>
        /// <param name="tempDD"></param>
        /// <param name="toEmail"></param>
        /// <param name="ccEmail"></param>
        public void ZipFile(string SaveExcelDirectory, string DayDirectory, string tempDD, List<string> toEmail, List<string> ccEmail)
        {
            iLog.WriteLog("开始压缩" + DayDirectory + "文件夹", 0);
            string ZipDirectory = ConstUtility.ZipFilePath;
            string ZipFileName = ZipDirectory + tempDD + DayDirectory + ".zip";
            FileOper oper = new FileOper();
            oper.CreateDirectory(ZipDirectory + tempDD);
            oper.ExistFile(ZipFileName);
            if (ZipHelper.Zip(SaveExcelDirectory, ZipFileName))
            {
                iLog.WriteLog("压缩" + DayDirectory + "文件夹成功", 0);
                SendEmail(ZipFileName, toEmail, ccEmail);
            }
        }
        /// <summary>
        /// 将压缩文件邮件发送出去
        /// </summary>
        /// <param name="sfile"></param>
        public void SendEmail(string sfile, List<string> sTo, List<string> sCC)
        {
            iLog.WriteLog("开始发送邮件", 0);
            EmailUtility.SendEmail(sTo, sCC, null, ConstUtility.EmailReportName + ConstUtility.EmailTitleDayReprotSum, ConstUtility.EmailDescDayReprotSum, sfile);
            iLog.WriteLog("门店汇总日报 发送成功", 0);
        }
        /// <summary>
        /// 获取发送人、抄送人及需要统计的门店信息
        /// </summary>
        /// <returns></returns>
        public List<UserStore> GetUserStore()
        {
            FileOper oper = new FileOper();
            string sqlFileDirectory = Achieve.SqlPath;
            string userStore = ConstUtility.UserStoreName;
            string sManagerEmailSql = oper.readOneFile(sqlFileDirectory, userStore);
            return DbHelper.GetUserStores(sManagerEmailSql);
        }
        /// <summary>
        /// 获取门店汇总日报数据源
        /// </summary>
        /// <param name="storeIDs"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDayReportSumData(string storeIDs)
        {
            object[] param = new object[1];
            param[0] = storeIDs;
            System.Data.DataTable tempDt = SqlServerHelper.ExecuteDataset(ConstUtility.ConnStr, "SP_DayReportSum", param).Tables[0];
            return tempDt;
        }
        /// <summary>
        /// 门店获取去化率
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public double GetQuHuaByStoreID(string storeId)
        {
            double q = 0;
            string filter = "";
            if (storeId != "999")
            {
                filter = string.Format(@" and StoreID='{0}'", storeId);
            }
            object obj = SqlServerHelper.ExecuteScalar(ConstUtility.ConnStr, System.Data.CommandType.Text
                , string.Format(@"select case when DATEDIFF(d,dateadd(d,-day(getdate())+1,getdate()),GETDATE())=0 
                                                then sum(count)/(DATEDIFF(d,dateadd(d,-day(DATEADD(day,-1,GETDATE()))+1,DATEADD(day,-1,GETDATE())),DATEADD(day,-1,GETDATE()))+1)
                                                else sum(count)/DATEDIFF(d,dateadd(d,-day(getdate())+1,getdate()),GETDATE()) 
			                                    end
                                    from DayReportCountSum                                     
                                    where Date>= (case when DATEDIFF(d,dateadd(d,-day(getdate())+1,getdate()),GETDATE())=0 
                                                 then  CONVERT(varchar(100),dateadd(d,-day(DATEADD(MONTH,-1,GETDATE()))+1,DATEADD(MONTH,-1,GETDATE())) ,23) 
			                                     else  CONVERT(varchar(100),dateadd(d,-day(getdate())+1,getdate()) ,23)  end)                                   
                                    and SubjectType in (59)
                                  {0}", filter));
            if (obj != null)
            {
                q = double.Parse(obj.ToString());
            }

            return q;
        }
    }
}
