using EmailToRepoertExcel.DBHelper;
using EmailToRepoertExcel.Model;
using EmailToRepoertExcel.Utils;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Service
{
    public class EmailToRevparExcel
    {
        public static readonly IDBHelper DbHelper = DBFactory.CreateDBHelper(ConstUtility.ConnType, ConstUtility.ConnStr);
        
        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static EmailToRevparExcel()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion
        public void RepoertExcel()
        {

            string strTemplateName = Achieve.TemplatePath + ConstUtility.Temp_Revpar;
            iLog.WriteLog("获取文件目录成功",0);
            iLog.WriteLog(strTemplateName, 0);
            StoreIncomeRevparExportExcel("门店收入环比", strTemplateName);
            GC.Collect();
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

                string sqlFileDirectory = Achieve.SqlPath;

                //获取门店的sql
                // string storeFileName = "Store.txt";
                string storeFileName = ConstUtility.StoreFileName;
                string storeRepoertSql = oper.readOneFile(sqlFileDirectory, storeFileName);     //读取sql

                //List<Store> ListStore = DbHelper.GetListStore(storeRepoertSql);

                //循环店长，并发送邮件（该店长所管辖的所有门店）
                List<UserStore> userStores = GetUserStore().Where(t => t.Type == 2).ToList();
                //foreach (var temp in userStore)
                //{

                int numtemp = 0;
                int numTab = 3;

                string DayDirectory = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
                string SaveExcelDirectory = ConstUtility.SavesFilePath + tempDD + DayDirectory + "\\";
                oper.CreateDirectory(SaveExcelDirectory);//创建需要保存的文件夹
                System.Data.DataTable tabledeteil = new System.Data.DataTable();
                string storeIDs = "";

                foreach (UserStore item in userStores)
                {
                    item.StoreId = item.StoreId.Replace(";", ",");
                    storeIDs = storeIDs + item.StoreId + ",";
                }
                if (storeIDs.Length > 0) storeIDs = storeIDs.Substring(0, storeIDs.Length - 1);
                System.Data.DataTable table = GetIncomeRevpar(out tabledeteil, storeIDs);

                Workbook workbook = excelApp.Workbooks.Open(strTemplateName);
                Sheets excelSheets = workbook.Worksheets;
                string currentSheet = currentSheetName;
                Worksheet excelWorksheet = (Worksheet)excelSheets.get_Item(currentSheet);

                //当前时间
                DateTime dt = DateTime.Now;
                //当前月第一天
                DateTime bDate = new DateTime(dt.Year, dt.Month, 1);
                //当前月最后一天
                DateTime eDate = bDate.AddMonths(1).AddDays(-1);

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
                                excelWorksheet.Cells[numTab, 3] = table.Rows[i][4].ToString();
                            }
                            if (table.Rows[i][5] != null)
                            {
                                excelWorksheet.Cells[numTab, 4] = table.Rows[i][5].ToString();
                            }
                            if (table.Rows[i][6] != null)
                            {
                                excelWorksheet.Cells[numTab, 5] = table.Rows[i][6].ToString();
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
                                excelWorksheet.Cells[numTab, 15] = table.Rows[i][16].ToString();
                            }
                            if (table.Rows[i][17] != null)
                            {
                                excelWorksheet.Cells[numTab, 16] = table.Rows[i][17].ToString();
                            }
                            if (table.Rows[i][18] != null)
                            {
                                excelWorksheet.Cells[numTab, 17] = table.Rows[i][18].ToString();
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
                                excelWorksheet.Cells[numtemp2, 3] = tabledeteil.Rows[i][4].ToString();
                            }
                            if (tabledeteil.Rows[i][5] != null)
                            {
                                excelWorksheet.Cells[numtemp2, 4] = tabledeteil.Rows[i][5].ToString();
                            }
                            if (tabledeteil.Rows[i][6] != null)
                            {
                                excelWorksheet.Cells[numtemp2, 5] = tabledeteil.Rows[i][6].ToString();
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
                                excelWorksheet.Cells[numtemp2, 15] = tabledeteil.Rows[i][16].ToString();
                            }
                            if (tabledeteil.Rows[i][17] != null)
                            {
                                excelWorksheet.Cells[numtemp2, 16] = tabledeteil.Rows[i][17].ToString();
                            }
                            if (tabledeteil.Rows[i][18] != null)
                            {
                                excelWorksheet.Cells[numtemp2, 17] = tabledeteil.Rows[i][18].ToString();
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

                UserStore temp = userStores.FirstOrDefault();

                ZipFile(SaveExcelDirectory, DayDirectory, tempDD, temp.TOUserEmail, temp.CCUserEmail); //压缩文件
                //}

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
            iLog.WriteLog("收入环比报表发送成功", 0);
        }

        public List<UserStore> GetUserStore()
        {
            FileOper oper = new FileOper();
            string sqlFileDirectory = Achieve.SqlPath;
            string userStore = ConstUtility.UserStoreName;
            string sManagerEmailSql = oper.readOneFile(sqlFileDirectory, userStore); //读取sql
            return DbHelper.GetUserStores(sManagerEmailSql);
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
            string strSQL = "";

            //当前时间
            DateTime dt = DateTime.Now;
            //当前月第一天
            DateTime bDate = new DateTime(dt.Year, dt.Month, 1);
            //当前月最后一天
            DateTime eDate = bDate.AddMonths(1).AddDays(-1);

            //上个月第一天
            DateTime bLastDate = bDate.AddMonths(-1);

            //上个月最后一天
            DateTime eLastDate = bDate.AddDays(-1);

            string sqlFileDirectory = Achieve.SqlPath;

            //获取报表汇总sql
            string searchAll = ConstUtility.SearchAll;

            FileOper oper = new FileOper();


            //获取门店的sql
            string searchStore = ConstUtility.SearchStore;

            string searchAllSql = oper.readOneFile(sqlFileDirectory, searchAll); //读取sql

            string searchStoreSql = oper.readOneFile(sqlFileDirectory, searchStore);     //读取sql

            strSQL = string.Format(searchAllSql, bLastDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                     , eLastDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                     , bDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                     , eDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                   );
            //获取汇总
            System.Data.DataTable tempDt = DbHelper.GetTableBySql(strSQL);
            tempDt.Columns.Add("Ctype");

            if (tempDt.Rows.Count > 0)
            {
                for (int i = 0; i < tempDt.Rows.Count; i++)
                {
                    tempDt.Rows[i]["Ctype"] = "0";
                }
            }

            strSQL = string.Format(searchStoreSql, bLastDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                     , eLastDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                     , bDate.ToString("yyyy-MM-dd") + " 00:00:00"
                                     , eDate.ToString("yyyy-MM-dd") + " 23:59:59"
                                         , storeIDs
                                   );
            //获取明细
            System.Data.DataTable tempDtall = DbHelper.GetTableBySql(strSQL);

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
