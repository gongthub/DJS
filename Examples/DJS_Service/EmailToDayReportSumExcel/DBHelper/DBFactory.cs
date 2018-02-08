#region
/*
 * ========================================================================
 * Copyright(c) 2012上海声通科技发展有限公司公司, All Rights Reserved.
 * ========================================================================
 * 
 * 作者：周赞  
 * 时间：2012-05-16 11:45:20
 * Email：zan.zhou@voicecomm.cn
 * 功能描述：数据库连接工厂
 * 文件名：DBFactory.cs
 * 版本：1.0.1001             
 * 修改说明：
 * ========================================================================
 * 
 */
#endregion


namespace EmailToDayReportSumExcel.DBHelper
{
    public class DBFactory
    {
        private DBFactory()
        {
        }

        public static IDBHelper CreateDBHelper(string connType,string conrnSt)
        {
            IDBHelper dbHelper = new SqlServerHelper(conrnSt);
            switch (connType)
            {
                case "SQLServer":
                    dbHelper = new SqlServerHelper(conrnSt); break; ;
                case "ORACLE":
                    dbHelper = new SqlServerHelper(conrnSt); break; ;
                case "MYSQL":
                    dbHelper = new SqlServerHelper(conrnSt); break; ;
            }
            return dbHelper;
        }
    }
}
