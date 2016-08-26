using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace EmailToRepoertExcel.DBHelper.Oracle
{
    public class OracleHelper : OracleBase
    {
        private string connectionStr;
        private OracleHelper()
        {
        }

        public OracleHelper(string conn)
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
