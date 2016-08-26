using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace EmailToRepoertExcel.DBHelper.MySQL
{
    public class MySQLHelper : MySQLBase
    {
        private string connectionStr;
        private MySQLHelper()
        {
        }

        public MySQLHelper(string conn)
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

        public List<EmailToRepoertExcel.Model.SumRepoertTable> GetListSumRepoertTable()
        {

            return null;
        }
    }
}
