using EmailToRevparExcel.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmailToRevparExcel.DBHelper
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

        public List<EmailToRevparExcel.Model.SumRepoertTable> GetListSumRepoertTable(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRevparExcel.Model.SumRepoertTable> ListSumRepoertTable = new List<Model.SumRepoertTable>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRevparExcel.Model.SumRepoertTable sumRepoertTable = new EmailToRevparExcel.Model.SumRepoertTable();
                    sumRepoertTable.Date = Convert.ToDateTime(dr["Date"].ToString());
                    sumRepoertTable.SubjectType = Convert.ToInt32(dr["SubjectType"].ToString());
                    sumRepoertTable.SUMCoutOrAccount = Convert.ToDecimal(dr["SUMCoutOrAccount"].ToString());
                    sumRepoertTable.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    ListSumRepoertTable.Add(sumRepoertTable);
                }
            }

            return ListSumRepoertTable;

        }


        public List<EmailToRevparExcel.Model.Store> GetListStore(string sql)
        {

            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRevparExcel.Model.Store> ListStore = new List<Model.Store>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRevparExcel.Model.Store store = new EmailToRevparExcel.Model.Store();
                    store.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    store.StoreName = dr["StoreName"].ToString();
                    ListStore.Add(store);
                }
            }

            return ListStore;

        }

        public List<EmailToRevparExcel.Model.AllSumRepoertTable> GetListAllSumRepoertTable(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRevparExcel.Model.AllSumRepoertTable> ListSumRepoertTable = new List<Model.AllSumRepoertTable>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRevparExcel.Model.AllSumRepoertTable sumRepoertTable = new EmailToRevparExcel.Model.AllSumRepoertTable();
                    sumRepoertTable.Date = Convert.ToDateTime(dr["Date"].ToString());
                    sumRepoertTable.SubjectType = Convert.ToInt32(dr["SubjectType"].ToString());
                    sumRepoertTable.SUMCoutOrAccount = Convert.ToDecimal(dr["SUMCoutOrAccount"].ToString());
                    //sumRepoertTable.StoreID = Convert.ToInt32(dr["StoreID"].ToString());
                    ListSumRepoertTable.Add(sumRepoertTable);
                }
            }

            return ListSumRepoertTable;

        }

        public List<EmailToRevparExcel.Model.UserStore> GetUserStores(string sql)
        {
            DataSet ds = ExecuteDataset(connectionStr, CommandType.Text, sql);
            List<EmailToRevparExcel.Model.UserStore> tempList = new List<Model.UserStore>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmailToRevparExcel.Model.UserStore temp = new EmailToRevparExcel.Model.UserStore();
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
