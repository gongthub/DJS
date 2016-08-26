using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SyncCustomerCodeService
{
    public class Util
    { 

        public static DataTable ExecuteSqlSelect(SqlConnection sqlCon, String sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlCon);
            sda.Fill(dt);
            return dt;
        }

        public static int ExecuteSqlHasReturn(SqlConnection sqlCon, SqlTransaction sqlTransaction, String sql)
        {
            sql += "SELECT @@identity";
            SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
            sqlCmd.Transaction = sqlTransaction;
            return Convert.ToInt32(sqlCmd.ExecuteScalar());
        }

        public static void ExecuteSqlNoReturn(SqlConnection sqlCon, SqlTransaction sqlTransaction, String sql)
        {
            SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
            sqlCmd.Transaction = sqlTransaction;
            sqlCmd.ExecuteScalar();
        }
    }
}
