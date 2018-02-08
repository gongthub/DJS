using EmailToRevparExcel.DBHelper;
using EmailToRevparExcel.Model;
using EmailToRevparExcel.Utils;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRevparExcel.Service
{
    public class EmailToRevparExcel
    {
        public static readonly IDBHelper DbHelper = DBFactory.CreateDBHelper(ConstUtility.ConnType, ConstUtility.ConnStr);

        public static DJS.SDK.ILog iLog = null;


        private static string SERVICECODE = "";

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static EmailToRevparExcel()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion
        public void RepoertExcel()
        {
            if (EmailUtility.ServiceIsStart(SERVICECODE))
            {
                string strTemplateName = Achieve.TemplatePath + ConstUtility.Temp_Revpar;
                iLog.WriteLog("获取文件目录成功", 0);
                iLog.WriteLog(strTemplateName, 0);
                StoreIncomeRevparExportExcel("门店收入环比", strTemplateName);
                GC.Collect();
            }
            else
            {
                iLog.WriteLog("邮件配置服务不启用！", 0);
            }
        }


        /// <summary>
        /// 根据模板填充数据—门店收入环比
        /// </summary>
        /// <param name="currentSheetName"></param>
        /// <param name="strTemplateName"></param>
        public void StoreIncomeRevparExportExcel(string currentSheetName, string strTemplateName)
        {
            try
            {
                Application excelApp = new Application();

                string tempDD = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "\\";

                FileOper oper = new FileOper();


                
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

                        int numtemp = 0;
                        int numTab = 3;

                        string DayDirectory = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
                        string SaveExcelDirectory = ConstUtility.SavesFilePath + tempDD + DayDirectory + "\\";
                        oper.CreateDirectory(SaveExcelDirectory);//创建需要保存的文件夹
                        System.Data.DataTable tabledeteil = new System.Data.DataTable();
                        string storeIDs = itemT.StoreID;

                        //if (storeIDs.Length > 0) storeIDs = storeIDs.Substring(0, storeIDs.Length - 1);
                        System.Data.DataTable table = GetIncomeRevpar(out tabledeteil, storeIDs);

                        Workbook workbook = excelApp.Workbooks.Open(strTemplateName);
                        Sheets excelSheets = workbook.Worksheets;
                        string currentSheet = currentSheetName;
                        Worksheet excelWorksheet = (Worksheet)excelSheets.get_Item(currentSheet);

                        //当前时间
                        DateTime dt = DateTime.Now;
                        //当前月第一天
                        DateTime bDate = dt.AddMonths(-1);
                        //当前月最后一天
                        DateTime eDate = dt.AddDays(-1);

                        //上个月第一天
                        DateTime bLastDate = bDate.AddMonths(-1);

                        //上个月最后一天
                        DateTime eLastDate = bDate.AddDays(-1);

                        //上期时间
                        string lastdate = bLastDate.ToString("yyyy/MM/dd") + "-" + eLastDate.ToString("yyyy/MM/dd");

                        //本期时间
                        string thisdate = bDate.ToString("yyyy/MM/dd") + "-" + eDate.ToString("yyyy/MM/dd");

                        excelWorksheet.Cells[1, 2] = lastdate;
                        excelWorksheet.Cells[1, 5] = thisdate;

                        #region 汇总
                        if (table != null && table.Rows.Count > 0)
                        {

                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                string types = table.Rows[i]["Ctype"].ToString();
                                if (types == "0")
                                {
                                    numtemp++;
                                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)excelWorksheet.Rows[numTab + 1, Type.Missing];
                                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                                    if (table.Rows[i][2] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 1] = table.Rows[i][2].ToString();
                                    }
                                    if (table.Rows[i][3] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 2] = table.Rows[i][3].ToString();
                                    }
                                    if (table.Rows[i][4] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 15] = table.Rows[i][4].ToString();
                                    }
                                    if (table.Rows[i][5] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 16] = table.Rows[i][5].ToString();
                                    }
                                    if (table.Rows[i][6] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 17] = table.Rows[i][6].ToString();
                                    }
                                    if (table.Rows[i][7] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 6] = table.Rows[i][7].ToString();
                                    }
                                    if (table.Rows[i][8] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 7] = table.Rows[i][8].ToString();
                                    }
                                    if (table.Rows[i][9] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 8] = table.Rows[i][9].ToString();
                                    }
                                    if (table.Rows[i][10] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 9] = table.Rows[i][10].ToString();
                                    }
                                    if (table.Rows[i][11] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 10] = table.Rows[i][11].ToString();
                                    }
                                    if (table.Rows[i][12] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 11] = table.Rows[i][12].ToString();
                                    }
                                    if (table.Rows[i][13] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 12] = table.Rows[i][13].ToString();
                                    }
                                    if (table.Rows[i][14] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 13] = table.Rows[i][14].ToString();
                                    }
                                    if (table.Rows[i][15] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 14] = table.Rows[i][15].ToString();
                                    }
                                    if (table.Rows[i][16] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 3] = table.Rows[i][16].ToString();
                                    }
                                    if (table.Rows[i][17] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 4] = table.Rows[i][17].ToString();
                                    }
                                    if (table.Rows[i][18] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 5] = table.Rows[i][18].ToString();
                                    }
                                    if (table.Rows[i][19] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 18] = table.Rows[i][19].ToString();
                                    }
                                    if (table.Rows[i][20] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 19] = table.Rows[i][20].ToString();
                                    }
                                    if (table.Rows[i][21] != null)
                                    {
                                        excelWorksheet.Cells[numTab, 20] = table.Rows[i][21].ToString();
                                    }
                                    numTab++;
                                }
                            }
                        }
                        #endregion

                        #region 明细
                        if (tabledeteil != null && tabledeteil.Rows.Count > 0)
                        {

                            for (int i = 0; i < tabledeteil.Rows.Count; i++)
                            {
                                string types = tabledeteil.Rows[i]["Ctype"].ToString();
                                if (types == "1")
                                {
                                    numtemp++;
                                    int numtemp2 = numtemp + 9;
                                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)excelWorksheet.Rows[numtemp2 + 1, Type.Missing];
                                    range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);

                                    if (tabledeteil.Rows[i][2] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 1] = tabledeteil.Rows[i][2].ToString();
                                    }
                                    if (tabledeteil.Rows[i][3] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 2] = tabledeteil.Rows[i][3].ToString();
                                    }
                                    if (tabledeteil.Rows[i][4] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 15] = tabledeteil.Rows[i][4].ToString();
                                    }
                                    if (tabledeteil.Rows[i][5] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 16] = tabledeteil.Rows[i][5].ToString();
                                    }
                                    if (tabledeteil.Rows[i][6] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 17] = tabledeteil.Rows[i][6].ToString();
                                    }
                                    if (tabledeteil.Rows[i][7] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 6] = tabledeteil.Rows[i][7].ToString();
                                    }
                                    if (tabledeteil.Rows[i][8] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 7] = tabledeteil.Rows[i][8].ToString();
                                    }
                                    if (tabledeteil.Rows[i][9] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 8] = tabledeteil.Rows[i][9].ToString();
                                    }
                                    if (tabledeteil.Rows[i][10] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 9] = tabledeteil.Rows[i][10].ToString();
                                    }
                                    if (tabledeteil.Rows[i][11] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 10] = tabledeteil.Rows[i][11].ToString();
                                    }
                                    if (tabledeteil.Rows[i][12] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 11] = tabledeteil.Rows[i][12].ToString();
                                    }
                                    if (tabledeteil.Rows[i][13] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 12] = tabledeteil.Rows[i][13].ToString();
                                    }
                                    if (tabledeteil.Rows[i][14] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 13] = tabledeteil.Rows[i][14].ToString();
                                    }
                                    if (tabledeteil.Rows[i][15] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 14] = tabledeteil.Rows[i][15].ToString();
                                    }
                                    if (tabledeteil.Rows[i][16] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 3] = tabledeteil.Rows[i][16].ToString();
                                    }
                                    if (tabledeteil.Rows[i][17] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 4] = tabledeteil.Rows[i][17].ToString();
                                    }
                                    if (tabledeteil.Rows[i][18] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 5] = tabledeteil.Rows[i][18].ToString();
                                    }
                                    if (tabledeteil.Rows[i][19] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 18] = tabledeteil.Rows[i][19].ToString();
                                    }
                                    if (tabledeteil.Rows[i][20] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 19] = tabledeteil.Rows[i][20].ToString();
                                    }
                                    if (tabledeteil.Rows[i][21] != null)
                                    {
                                        excelWorksheet.Cells[numtemp2, 20] = tabledeteil.Rows[i][21].ToString();
                                    }

                                }
                            }
                        }
                        #endregion
                        string strFileName = SaveExcelDirectory + "收入汇总" + "." + ConstUtility.Suffix;

                        oper.ExistFile(strFileName);
                        workbook.SaveAs(strFileName);
                        workbook.Close();
                        excelApp.Quit();


                        List<string> sTo = EmailUtility.GetServiceSetEmailToById(SERVICECODE, itemT.ID);
                        List<string> sCC = EmailUtility.GetServiceSetEmailCcById(SERVICECODE, itemT.ID);

                        ZipFile(SaveExcelDirectory, DayDirectory, tempDD, sTo, sCC); //压缩文件 

                        }
                        catch (Exception ex)
                        {
                            iLog.WriteLog(ex.Message.ToString(), 1);
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
        }


        public void ZipFile(string SaveExcelDirectory, string DayDirectory, string tempDD, List<string> sTo, List<string> sCC)
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
                SendEmail(ZipFileName, sTo, sCC);
            }
        }

        /// <summary>
        /// 将压缩文件邮件发送出去
        /// </summary>
        /// <param name="sfile"></param>
        public void SendEmail(string sfile, List<string> sTo, List<string> sCC)
        {
            iLog.WriteLog("开始发送邮件", 0);
            EmailUtility.SendEmail(sTo, sCC, null, ConstUtility.EmailReportName + ConstUtility.EmailTitleRevpar, ConstUtility.EmailDescRevpar, sfile);
            iLog.WriteLog("收入环比报表发送成功", 0);
        }



        #region 门店收入环比

        /// <summary>
        /// 获取收入环比数据
        /// </summary>
        /// <param name="cType"></param>
        /// <param name="storeIDs"></param>
        /// <returns></returns>
        public System.Data.DataTable GetIncomeRevpar(out System.Data.DataTable tabledeteil, string storeIDs)
        {

            //当前时间
            DateTime dt = DateTime.Now;
            //当前月第一天
            DateTime bDate = dt.AddMonths(-1);
            //当前月最后一天
            DateTime eDate = dt.AddDays(-1);
            //上个月第一天
            DateTime bLastDate = bDate.AddMonths(-1);

            //上个月最后一天
            DateTime eLastDate = bDate.AddDays(-1);

            string sqlFileDirectory = Achieve.SqlPath;


            FileOper oper = new FileOper();


            object[] paramAll = new object[6];
            paramAll[0] = "0";
            paramAll[1] = bLastDate.ToString("yyyy-MM-dd") + " 00:00:00";
            paramAll[2] = eLastDate.ToString("yyyy-MM-dd") + " 23:59:59";
            paramAll[3] = bDate.ToString("yyyy-MM-dd") + " 00:00:00";
            paramAll[4] = eDate.ToString("yyyy-MM-dd") + " 23:59:59";
            paramAll[5] = storeIDs;

            System.Data.DataTable tempDt = SqlServerHelper.ExecuteDataset(ConstUtility.ConnStr, "SP_Revpar", paramAll).Tables[0];

            tempDt.Columns.Add("Ctype");

            if (tempDt.Rows.Count > 0)
            {
                for (int i = 0; i < tempDt.Rows.Count; i++)
                {
                    tempDt.Rows[i]["Ctype"] = "0";
                }
            }

            object[] paramD = new object[6];
            paramD[0] = "1";
            paramD[1] = bLastDate.ToString("yyyy-MM-dd") + " 00:00:00";
            paramD[2] = eLastDate.ToString("yyyy-MM-dd") + " 23:59:59";
            paramD[3] = bDate.ToString("yyyy-MM-dd") + " 00:00:00";
            paramD[4] = eDate.ToString("yyyy-MM-dd") + " 23:59:59";
            paramD[5] = storeIDs;
            System.Data.DataTable tempDtall = SqlServerHelper.ExecuteDataset(ConstUtility.ConnStr, "SP_Revpar", paramD).Tables[0];

            tempDtall.Columns.Add("Ctype");
            if (tempDtall.Rows.Count > 0)
            {
                for (int i = 0; i < tempDtall.Rows.Count; i++)
                {
                    tempDtall.Rows[i]["Ctype"] = "1";
                }
            }
            tabledeteil = tempDtall;

            return tempDt;

        }
        #endregion


    }
}
