using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using EmailToRepoertExcel.DBHelper;
using EmailToRepoertExcel.Utils;
using EmailToRepoertExcel.Model;
using System.IO;
namespace EmailToRepoertExcel.Service
{
    public class EmailToRepoertExcel
    {
        public static readonly IDBHelper DbHelper = DBFactory.CreateDBHelper(ConstUtility.ConnType, ConstUtility.ConnStr);

        
        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static EmailToRepoertExcel()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion

        public void RepoertExcel()
        {
            //iLog.WriteLog(SysConfig.Instance().ConnStr);
            //获取文件目录
            //string strTemplateName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temp\\Temp_DaylReportSum.xls";
            string strTemplateName = Achieve.TemplatePath + ConstUtility.Temp_DaylReportSum;
            iLog.WriteLog("获取文件目录成功",0);
            iLog.WriteLog(strTemplateName, 0);
            DayRepExportExcel("Sheet1", strTemplateName);
            GC.Collect();
        }

        /// <summary>
        /// 根据模板填充数据
        /// </summary>
        /// <param name="currentSheetName"></param>
        /// <param name="strTemplateName"></param>
        public void DayRepExportExcel(string currentSheetName, string strTemplateName)
        {
            try
            {
                Application excelApp = new Application();
                string tempDD = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "\\";

                FileOper oper = new FileOper();
                string sqlFileDirectory = Achieve.SqlPath;
                //获取日报表汇总sql
                //string dayRepoertFileName = "DayRepoertExcel.txt";
                string dayRepoertFileName = ConstUtility.DayRepoertFileName;

                //获取门店的sql
                // string storeFileName = "Store.txt";
                string storeFileName = ConstUtility.StoreFileName;

                string dayRepoertSql = oper.readOneFile(sqlFileDirectory, dayRepoertFileName); //读取sql

                string storeRepoertSql = oper.readOneFile(sqlFileDirectory, storeFileName);     //读取sql

                // string Month=DateTime.Now.AddDays(-1).ToString("yyyy-MM");
                // dayRepoertSql = string.Format(dayRepoertSql, Month);

                List<SumRepoertTable> ListSumRepoert = DbHelper.GetListSumRepoertTable(dayRepoertSql);

                List<Store> ListStore = DbHelper.GetListStore(storeRepoertSql);

                //循环店长，并发送邮件（该店长所管辖的所有门店）
                var userStore = GetUserStore().Where(t => t.Type == 1).ToList();
                foreach (var temp in userStore)
                {
                    string DayDirectory = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
                    string SaveExcelDirectory = ConstUtility.SavesFilePath + tempDD + DayDirectory + "\\";
                    oper.CreateDirectory(SaveExcelDirectory);//创建需要保存的文件夹

                    #region 循环门店
                    foreach (Store s in ListStore)
                    {
                        //如果该店长管辖该门店，则生成一文件
                        if (temp.StoreId.Split(';').Contains(s.StoreID.ToString()))
                        {
                            iLog.WriteLog("门店" + s.StoreName + "开始写入日报表", 0);

                            Workbook workbook = excelApp.Workbooks.Open(strTemplateName);
                            Sheets excelSheets = workbook.Worksheets;
                            string currentSheet = currentSheetName;
                            Worksheet excelWorksheet = (Worksheet)excelSheets.get_Item(currentSheet);
                            excelWorksheet.Cells[1, 1] = s.StoreName + "租赁情况日报";
                            string strFileName = SaveExcelDirectory + s.StoreName + "." + ConstUtility.Suffix;
                            var sums = ListSumRepoert.Where(p => p.StoreID == s.StoreID).GroupBy(p => new { p.Date, p.StoreID }).Select(group => new
                            {
                                Peo = group.Key,
                                Count = group.Count()
                            });

                            decimal sumMoney = 0;      //平均房价(月) = 在住合同月租金总和/在住房间数量(包括青硕青客)
                            decimal telCount = 0;      //来电转化率 = 当日意向量/当日接听量
                            decimal visitCount = 0;    //来访转化率 = 潜在客户数量/来人数量
                            decimal allRooms = 0;      //总房间数
                            decimal zaiZhuCount = 0;   //在住房间数
                            decimal yuDingCount = 0;   //预订房间数
                            decimal ydAndyyCount = 0;  //预订和预约数 
                            decimal newSignAll = 0;    //房间新签数量（1号到当天的总签约数）
                            decimal dayCount = 0;      //1号到当天之间的天数

                            //获取门店和日期列表
                            foreach (var employee in sums)
                            {
                                List<SumRepoertTable> DayRepoerts = ListSumRepoert.Where(p => p.StoreID == employee.Peo.StoreID && p.Date == employee.Peo.Date).ToList();
                                iLog.WriteLog("门店：" + s.StoreName + "日期：" + employee.Peo.Date + "报表数据开始写入", 0);
                                dayCount = new TimeSpan(employee.Peo.Date.Ticks - new DateTime(employee.Peo.Date.Year, employee.Peo.Date.Month, 1).Ticks).Days + 1;

                                decimal newSignCount = 0;   //房间新签数量（当天的总签约数）
                                foreach (SumRepoertTable dayRow in DayRepoerts)
                                {
                                    if (dayRow.SubjectType == 9)
                                    {
                                        sumMoney = (int)dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 36)
                                    {
                                        telCount = (int)dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 44)
                                    {
                                        visitCount = (int)dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 59)
                                    {
                                        newSignCount = dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 53)
                                    {
                                        allRooms = (int)dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 51)
                                    {
                                        zaiZhuCount = (int)dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 58)
                                    {
                                        ydAndyyCount = (int)dayRow.SUMCoutOrAccount;
                                    }
                                    if (dayRow.SubjectType == 54)
                                    {
                                        yuDingCount = (int)dayRow.SUMCoutOrAccount;
                                    }
                                }

                                newSignAll += newSignCount;
                                int columnIndex = employee.Peo.Date.Day + 3;
                                foreach (SumRepoertTable dayRow in DayRepoerts)
                                {
                                    iLog.WriteLog("门店：" + s.StoreName + "日期：" + employee.Peo.Date + "报表模板第" + columnIndex + "列数据写入" + "类型" + dayRow.SubjectType + "数量" + dayRow.SUMCoutOrAccount, 0);

                                    switch (dayRow.SubjectType)
                                    {
                                        case 1: //定金【预定，预约】
                                            excelWorksheet.Cells[23, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 2: //租金收入【合同】
                                            excelWorksheet.Cells[24, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 3: //押金收入【合同,商铺】
                                            excelWorksheet.Cells[25, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 4: //水电收入【合同,门店,商铺】
                                            excelWorksheet.Cells[26, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 5: //停车费收入【合同,门店】
                                            excelWorksheet.Cells[27, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 6: //公共厨房收入【合同】
                                            excelWorksheet.Cells[28, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 7: //线上租金收入【官网】
                                            excelWorksheet.Cells[29, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 8: //商铺收入【合同(租金)】
                                            excelWorksheet.Cells[30, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 11: //其他收入【合同，门店】
                                            excelWorksheet.Cells[31, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 10: //成交均价（月）
                                            excelWorksheet.Cells[22, columnIndex] = dayRow.SUMCoutOrAccount == 0 ? 0 : (sumMoney / dayRow.SUMCoutOrAccount);
                                            break;
                                        case 30: //来电--58同城
                                            excelWorksheet.Cells[3, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 31: //来电--赶集
                                            excelWorksheet.Cells[4, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 32: //来电--地推
                                            excelWorksheet.Cells[5, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 33: //来电--公司线上
                                            excelWorksheet.Cells[6, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 34: //来电--公司线下
                                            excelWorksheet.Cells[7, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 35: //来电--其它
                                            excelWorksheet.Cells[8, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 36: //来电--接听量（起）
                                            excelWorksheet.Cells[9, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 37: //来电--转化率 = 当日意向量/当日接听量
                                            excelWorksheet.Cells[10, columnIndex] = telCount == 0 ? 0 : dayRow.SUMCoutOrAccount / telCount;
                                            break;
                                        case 38: //来访--58同城
                                            excelWorksheet.Cells[11, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 39: //来访--赶集
                                            excelWorksheet.Cells[12, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 40: //来访--地推
                                            excelWorksheet.Cells[13, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 41: //来访--公司线上
                                            excelWorksheet.Cells[14, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 42: //来访--公司线下
                                            excelWorksheet.Cells[15, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 43: //来访--其它
                                            excelWorksheet.Cells[16, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 44: //来访--来人数量
                                            excelWorksheet.Cells[17, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 45: //来访--潜在客户数量、转化率=潜在客户数量/来人数量
                                            excelWorksheet.Cells[19, columnIndex] = dayRow.SUMCoutOrAccount;
                                            excelWorksheet.Cells[18, columnIndex] = visitCount == 0 ? 0 : dayRow.SUMCoutOrAccount / visitCount;
                                            break;
                                        case 46: //新签数量、日平均去化房量(MTD) = 新签数量/当前日期-1号
                                            excelWorksheet.Cells[20, columnIndex] = dayRow.SUMCoutOrAccount;
                                            //excelWorksheet.Cells[40, columnIndex] = (dayCount == 0 ? 1 : dayRow.SUMCoutOrAccount / dayCount);
                                            break;
                                        case 47: //续签数量
                                            excelWorksheet.Cells[21, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 48: //租金贷数量
                                            excelWorksheet.Cells[32, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 49: //累计租金贷签约数量
                                            excelWorksheet.Cells[33, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 50: //累计租金贷放款数量
                                            excelWorksheet.Cells[34, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 57: //累计租金贷放款金额
                                            excelWorksheet.Cells[35, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 51:  //36在住房间数   38在住率=在住房间数量/总房间数  39预定+在住（率）= （预定+在住）/运营房总间数 
                                            //42可出租房间数（总房量-在住-预定和预约）44空置房间数量（总房量-在住）
                                            excelWorksheet.Cells[36, columnIndex] = dayRow.SUMCoutOrAccount;
                                            excelWorksheet.Cells[38, columnIndex] = allRooms == 0 ? 0 : dayRow.SUMCoutOrAccount / allRooms;
                                            excelWorksheet.Cells[39, columnIndex] = allRooms == 0 ? 0 : (yuDingCount + zaiZhuCount) / allRooms;
                                            excelWorksheet.Cells[42, columnIndex] = (allRooms - zaiZhuCount - ydAndyyCount);
                                            excelWorksheet.Cells[44, columnIndex] = allRooms - zaiZhuCount;
                                            break;
                                        case 52: //预定房间数【预定，预约】
                                            excelWorksheet.Cells[37, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 55: //退租间数
                                            excelWorksheet.Cells[40, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        case 56: //续租间数
                                            excelWorksheet.Cells[41, columnIndex] = dayRow.SUMCoutOrAccount;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                excelWorksheet.Cells[43, columnIndex] = dayCount == 0 ? 0 : newSignAll / dayCount;
                                iLog.WriteLog("门店：" + s.StoreName + "日期：" + employee.Peo.Date + "报表数据写入完成", 0);
                            }

                            excelWorksheet.Cells[1, 10] = DateTime.Now.AddDays(-1).Month + "月";
                            excelWorksheet.Cells[1, 14] = allRooms;
                            oper.ExistFile(strFileName);
                            workbook.SaveAs(strFileName);
                            iLog.WriteLog("门店" + s.StoreName + "日报表保存路径" + strFileName, 0);
                            workbook.Close();
                            excelApp.Quit();
                            iLog.WriteLog("门店" + s.StoreName + "结束日报表写入", 0);
                        }
                    }
                    #endregion

                    //2016/4/22 angelali beging
                    //0为所有门店权限
                    if (temp.StoreId.Split(';').Contains("0"))
                    {
                        #region 汇总
                        Application excelApp1 = new Application();
                        FileOper oper1 = new FileOper();

                        //获取日报表汇总sql
                        string dayRepoertFileName1 = "ALLdayrepor.txt";
                        //string dayRepoertFileName1 = SysConfig.Instance().DayRepoertFileName;

                        //获取门店的sql
                        // string storeFileName = "Store.txt";
                        //string storeFileName1 = SysConfig.Instance().StoreFileName;

                        string dayRepoertSql1 = oper1.readOneFile(sqlFileDirectory, dayRepoertFileName1); //读取sql

                        //string storeRepoertSql1 = oper.readOneFile(sqlFileDirectory, storeFileName);     //读取sql

                        // string Month=DateTime.Now.AddDays(-1).ToString("yyyy-MM");
                        // dayRepoertSql = string.Format(dayRepoertSql, Month);

                        List<AllSumRepoertTable> ListSumRepoert1 = DbHelper.GetListAllSumRepoertTable(dayRepoertSql1);

                        iLog.WriteLog("总部开始写入日报表", 0);

                        Workbook workbook1 = excelApp.Workbooks.Open(strTemplateName);
                        Sheets excelSheets1 = workbook1.Worksheets;
                        string currentSheet1 = currentSheetName;
                        Worksheet excelWorksheet1 = (Worksheet)excelSheets1.get_Item(currentSheet1);
                        excelWorksheet1.Cells[1, 1] = "总部租赁情况日报";
                        string strFileName1 = SaveExcelDirectory + "总部运营." + ConstUtility.Suffix;
                        //var sums1 = ListSumRepoert1.GroupBy(p => new { p.Date}).Select(group => new
                        //{
                        //    Peo = group.Key,
                        //    Count = group.Count()
                        //});
                        var sums1 = from ps in ListSumRepoert1
                                    group ps by ps.Date into g
                                    select new { Peo = g.Key, Count = g.Count() };

                        decimal sumMoney = 0;    //平均房价(月) = 在住合同月租金总和/在住房间数量(包括青硕青客)
                        decimal telCount = 0;    //来电转化率 = 当日意向量/当日接听量
                        decimal visitCount = 0;  //来访转化率 = 潜在客户数量/来人数量
                        decimal allRooms = 0;    //总房间数
                        decimal zaiZhuCount = 0; //在住房间数
                        decimal yuDingCount = 0; //预订房间数
                        decimal ydAndyyCount = 0;  //预订和预约数                       
                        decimal newSignAll = 0;    //房间新签数量（1号到当天的总签约数）
                        decimal dayCount = 0;      //当月1号-当天之间的天数

                        //获取门店和日期列表
                        foreach (var employee in sums1)
                        {
                            List<AllSumRepoertTable> DayRepoerts1 = ListSumRepoert1.Where(p => p.Date == employee.Peo.Date).ToList();
                            iLog.WriteLog(" 总部 日期：" + employee.Peo.Date + "报表数据开始写入", 0);
                            dayCount = new TimeSpan(employee.Peo.Date.Ticks - new DateTime(employee.Peo.Date.Year, employee.Peo.Date.Month, 1).Ticks).Days + 1;

                            decimal newSignCount = 0;  //房间新签数量（当天的总签约数）
                            foreach (AllSumRepoertTable dayRow in DayRepoerts1)
                            {
                                if (dayRow.SubjectType == 9)
                                {
                                    sumMoney = (int)dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 36)
                                {
                                    telCount = (int)dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 44)
                                {
                                    visitCount = (int)dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 59)
                                {
                                    newSignCount = dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 53)
                                {
                                    allRooms = (int)dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 51)
                                {
                                    zaiZhuCount = (int)dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 58)
                                {
                                    ydAndyyCount = (int)dayRow.SUMCoutOrAccount;
                                }
                                if (dayRow.SubjectType == 54)
                                {
                                    yuDingCount = (int)dayRow.SUMCoutOrAccount;
                                }
                            }

                            newSignAll += newSignCount;
                            int columnIndex = employee.Peo.Date.Day + 3;
                            foreach (AllSumRepoertTable dayRow in DayRepoerts1)
                            {
                                iLog.WriteLog("总部日期：" + employee.Peo.Date + "报表模板第" + columnIndex + "列数据写入" + "类型" + dayRow.SubjectType + "数量" + dayRow.SUMCoutOrAccount, 0);

                                switch (dayRow.SubjectType)
                                {
                                    case 1: //定金【预定，预约】
                                        excelWorksheet1.Cells[23, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 2: //租金收入【合同】
                                        excelWorksheet1.Cells[24, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 3: //押金收入【合同,商铺】
                                        excelWorksheet1.Cells[25, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 4: //水电收入【合同,门店,商铺】
                                        excelWorksheet1.Cells[26, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 5: //停车费收入【合同,门店】
                                        excelWorksheet1.Cells[27, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 6: //公共厨房收入【合同】
                                        excelWorksheet1.Cells[28, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 7: //线上租金收入【官网】
                                        excelWorksheet1.Cells[29, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 8: //商铺收入【合同(租金)】
                                        excelWorksheet1.Cells[30, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 11: //其他收入【合同，门店】
                                        excelWorksheet1.Cells[31, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 10: //成交均价（月）
                                        excelWorksheet1.Cells[22, columnIndex] = dayRow.SUMCoutOrAccount == 0 ? 0 : (sumMoney / dayRow.SUMCoutOrAccount);
                                        break;
                                    case 30: //来电--58同城
                                        excelWorksheet1.Cells[3, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 31: //来电--赶集
                                        excelWorksheet1.Cells[4, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 32: //来电--地推
                                        excelWorksheet1.Cells[5, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 33: //来电--公司线上
                                        excelWorksheet1.Cells[6, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 34: //来电--公司线下
                                        excelWorksheet1.Cells[7, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 35: //来电--其它
                                        excelWorksheet1.Cells[8, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 36: //来电--接听量（起）
                                        excelWorksheet1.Cells[9, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 37: //来电--转化率 = 当日意向量/当日接听量
                                        excelWorksheet1.Cells[10, columnIndex] = telCount == 0 ? 0 : dayRow.SUMCoutOrAccount / telCount;
                                        break;
                                    case 38: //来访--58同城
                                        excelWorksheet1.Cells[11, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 39: //来访--赶集
                                        excelWorksheet1.Cells[12, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 40: //来访--地推
                                        excelWorksheet1.Cells[13, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 41: //来访--公司线上
                                        excelWorksheet1.Cells[14, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 42: //来访--公司线下
                                        excelWorksheet1.Cells[15, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 43: //来访--其它
                                        excelWorksheet1.Cells[16, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 44: //来访--来人数量
                                        excelWorksheet1.Cells[17, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 45: //来访--潜在客户数量、转化率=潜在客户数量/来人数量
                                        excelWorksheet1.Cells[19, columnIndex] = dayRow.SUMCoutOrAccount;
                                        excelWorksheet1.Cells[18, columnIndex] = visitCount == 0 ? 0 : dayRow.SUMCoutOrAccount / visitCount;
                                        break;
                                    case 46: //新签数量、日平均去化房量(MTD) = 新签数量/当前日期-1号
                                        excelWorksheet1.Cells[20, columnIndex] = dayRow.SUMCoutOrAccount;
                                        //excelWorksheet.Cells[40, columnIndex] = (dayCount == 0 ? 1 : dayRow.SUMCoutOrAccount / dayCount);
                                        break;
                                    case 47: //续签数量
                                        excelWorksheet1.Cells[21, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 48: //租金贷数量
                                        excelWorksheet1.Cells[32, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 49: //累计租金贷签约数量
                                        excelWorksheet1.Cells[33, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 50: //累计租金贷放款数量
                                        excelWorksheet1.Cells[34, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 57: //累计租金贷放款金额
                                        excelWorksheet1.Cells[35, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 51:  //36在住房间数   38在住率=在住房间数量/总房间数  39预定+在住（率）= （预定+在住）/运营房总间数 
                                        //42可出租房间数（总房量-在住-预定和预约）44空置房间数量（总房量-在住）
                                        excelWorksheet1.Cells[36, columnIndex] = dayRow.SUMCoutOrAccount;
                                        excelWorksheet1.Cells[38, columnIndex] = allRooms == 0 ? 0 : dayRow.SUMCoutOrAccount / allRooms;
                                        excelWorksheet1.Cells[39, columnIndex] = allRooms == 0 ? 0 : (yuDingCount + zaiZhuCount) / allRooms;
                                        excelWorksheet1.Cells[42, columnIndex] = (allRooms - zaiZhuCount - ydAndyyCount);
                                        excelWorksheet1.Cells[44, columnIndex] = allRooms - zaiZhuCount;
                                        break;
                                    case 52: //预定房间数【预定，预约】
                                        excelWorksheet1.Cells[37, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 55: //退租间数
                                        excelWorksheet1.Cells[40, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    case 56: //续租间数
                                        excelWorksheet1.Cells[41, columnIndex] = dayRow.SUMCoutOrAccount;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            excelWorksheet1.Cells[43, columnIndex] = dayCount == 0 ? 0 : newSignAll / dayCount;
                            iLog.WriteLog("总部日期：" + employee.Peo.Date + "报表数据写入完成", 0);
                        }

                        excelWorksheet1.Cells[1, 10] = DateTime.Now.AddDays(-1).Month + "月";
                        excelWorksheet1.Cells[1, 14] = allRooms;
                        oper1.ExistFile(strFileName1);
                        workbook1.SaveAs(strFileName1);
                        iLog.WriteLog("总部日报表保存路径" + strFileName1, 0);
                        workbook1.Close();
                        excelApp1.Quit();
                        iLog.WriteLog("总部结束日报表写入", 0);

                        #endregion
                    }
                    //2016/4/22 angelali end 

                    ZipFile(SaveExcelDirectory, DayDirectory, tempDD, temp.TOUserEmail, temp.CCUserEmail); //压缩文件
                }
            }
            catch (Exception ex)
            {
                iLog.WriteLog(ex.Message.ToString(), 1);
            }
        }

        public void ZipFile(string SaveExcelDirectory, string DayDirectory, string tempDD, string toEmail, string ccEmail)
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
                SendEmail(ZipFileName, toEmail, ccEmail);
            }
        }

        /// <summary>
        /// 将压缩文件邮件发送出去
        /// </summary>
        /// <param name="sfile"></param>
        public void SendEmail(string sfile, string toEmail, string ccEmail)
        {
            iLog.WriteLog("开始发送邮件", 0);

            List<string> sTo = new List<string>();
            List<string> sCC = new List<string>(); 
            //string[] arr = sendToEmail.Split(';');
            if (!string.IsNullOrEmpty(toEmail))
            {
                string[] toUser = toEmail.Split(';');
                foreach (var temp in toUser)
                {
                    sTo.Add(temp);
                }
            }

            if (!string.IsNullOrEmpty(ccEmail))
            {
                string[] ccUser = ccEmail.Split(';');
                foreach (var temp in ccUser)
                {
                    sCC.Add(temp);
                }
            }

            EmailUtility.SendEmail(sTo, sCC, null, ConstUtility.EmailTitle, ConstUtility.EmailDesc, sfile);
            iLog.WriteLog("日报表发送成功", 0);
        }

        public List<UserStore> GetUserStore()
        {
            FileOper oper = new FileOper();
            string sqlFileDirectory = Achieve.SqlPath;
            string userStore = ConstUtility.UserStoreName;
            string sManagerEmailSql = oper.readOneFile(sqlFileDirectory, userStore); //读取sql
            return DbHelper.GetUserStores(sManagerEmailSql);
        }
    }
}
