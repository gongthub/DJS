using EmailToRepoertExcel.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmailToRepoertExcel.DBHelper
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

        public List<EmailToRepoertExcel.Model.SumRepoertTable> GetListSumRepoertTable(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRepoertExcel.Model.SumRepoertTable> ListSumRepoertTable = new List<Model.SumRepoertTable>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRepoertExcel.Model.SumRepoertTable sumRepoertTable = new EmailToRepoertExcel.Model.SumRepoertTable();
                    sumRepoertTable.Date = Convert.ToDateTime(dr["Date"].ToString());
                    sumRepoertTable.SubjectType = Convert.ToInt32(dr["SubjectType"].ToString());
                    sumRepoertTable.SUMCoutOrAccount = Convert.ToDecimal(dr["SUMCoutOrAccount"].ToString());
                    sumRepoertTable.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    ListSumRepoertTable.Add(sumRepoertTable);
                }
            }

            return ListSumRepoertTable;

        }


        public List<EmailToRepoertExcel.Model.Store> GetListStore(string sql)
        {

            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRepoertExcel.Model.Store> ListStore = new List<Model.Store>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRepoertExcel.Model.Store store = new EmailToRepoertExcel.Model.Store();
                    store.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    store.StoreName = dr["StoreName"].ToString();
                    ListStore.Add(store);
                }
            }

            return ListStore;

        }

        public List<EmailToRepoertExcel.Model.AllSumRepoertTable> GetListAllSumRepoertTable(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRepoertExcel.Model.AllSumRepoertTable> ListSumRepoertTable = new List<Model.AllSumRepoertTable>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRepoertExcel.Model.AllSumRepoertTable sumRepoertTable = new EmailToRepoertExcel.Model.AllSumRepoertTable();
                    sumRepoertTable.Date = Convert.ToDateTime(dr["Date"].ToString());
                    sumRepoertTable.SubjectType = Convert.ToInt32(dr["SubjectType"].ToString());
                    sumRepoertTable.SUMCoutOrAccount = Convert.ToDecimal(dr["SUMCoutOrAccount"].ToString());
                    //sumRepoertTable.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    ListSumRepoertTable.Add(sumRepoertTable);
                }
            }

            return ListSumRepoertTable;

        }

        public List<EmailToRepoertExcel.Model.UserStore> GetUserStores(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRepoertExcel.Model.UserStore> tempList = new List<Model.UserStore>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRepoertExcel.Model.UserStore temp = new EmailToRepoertExcel.Model.UserStore();
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
