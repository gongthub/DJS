using EmailToDayReportSumExcel.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmailToDayReportSumExcel.DBHelper
{
    internal class SqlServerHelper : SQLServerBase, IDBHelper
    {
        private string connectionStr;
        private SqlServerHelper()
        {
        }

        public SqlServerHelper(string conn)
        {
            ConnectionStr = conn;
        }

        public string ConnectionStr
        {
            get { return connectionStr; }
            set { connectionStr = value; }
        }

        public DataSet GetSysConfig()
        {
            DataSet ds = new DataSet();
            return ds;
        }

        public List<EmailToDayReportSumExcel.Model.SumRepoertTable> GetListSumRepoertTable(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToDayReportSumExcel.Model.SumRepoertTable> ListSumRepoertTable = new List<Model.SumRepoertTable>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToDayReportSumExcel.Model.SumRepoertTable sumRepoertTable = new EmailToDayReportSumExcel.Model.SumRepoertTable();
                    sumRepoertTable.Date = Convert.ToDateTime(dr["Date"].ToString());
                    sumRepoertTable.SubjectType = Convert.ToInt32(dr["SubjectType"].ToString());
                    sumRepoertTable.SUMCoutOrAccount = Convert.ToDecimal(dr["SUMCoutOrAccount"].ToString());
                    sumRepoertTable.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    ListSumRepoertTable.Add(sumRepoertTable);
                }
            }

            return ListSumRepoertTable;

        }


        public List<EmailToDayReportSumExcel.Model.Store> GetListStore(string sql)
        {

            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToDayReportSumExcel.Model.Store> ListStore = new List<Model.Store>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToDayReportSumExcel.Model.Store store = new EmailToDayReportSumExcel.Model.Store();
                    store.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    store.StoreName = dr["StoreName"].ToString();
                    ListStore.Add(store);
                }
            }

            return ListStore;

        }

        public List<EmailToDayReportSumExcel.Model.AllSumRepoertTable> GetListAllSumRepoertTable(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToDayReportSumExcel.Model.AllSumRepoertTable> ListSumRepoertTable = new List<Model.AllSumRepoertTable>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToDayReportSumExcel.Model.AllSumRepoertTable sumRepoertTable = new EmailToDayReportSumExcel.Model.AllSumRepoertTable();
                    sumRepoertTable.Date = Convert.ToDateTime(dr["Date"].ToString());
                    sumRepoertTable.SubjectType = Convert.ToInt32(dr["SubjectType"].ToString());
                    sumRepoertTable.SUMCoutOrAccount = Convert.ToDecimal(dr["SUMCoutOrAccount"].ToString());
                    //sumRepoertTable.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    ListSumRepoertTable.Add(sumRepoertTable);
                }
            }

            return ListSumRepoertTable;

        }

        public List<EmailToDayReportSumExcel.Model.UserStore> GetUserStores(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToDayReportSumExcel.Model.UserStore> tempList = new List<Model.UserStore>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToDayReportSumExcel.Model.UserStore temp = new EmailToDayReportSumExcel.Model.UserStore();
                    temp.StoreId = dr["StoreId"].ToString();
                    temp.TOUserEmail = dr["TOUserEmail"].ToString();
                    temp.CCUserEmail = dr["CCUserEmail"].ToString();
                    temp.Type = Convert.ToInt32(dr["Type"]);
                    tempList.Add(temp);
                }
            }
            return tempList;
        }


        /// <summary>
        /// 根据sql获取table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTableBySql(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            DataTable table = ds.Tables[0];
            return table;
        }
    }
}
