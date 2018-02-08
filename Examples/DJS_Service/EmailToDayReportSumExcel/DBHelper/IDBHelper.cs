using System.Collections.Generic;
using System.Data;


namespace EmailToDayReportSumExcel.DBHelper
{
    public interface IDBHelper
    {
        DataSet GetSysConfig();//不从数据库中读取

        List<EmailToDayReportSumExcel.Model.SumRepoertTable> GetListSumRepoertTable(string sql);


        List<EmailToDayReportSumExcel.Model.Store> GetListStore(string sql);

        List<EmailToDayReportSumExcel.Model.AllSumRepoertTable> GetListAllSumRepoertTable(string sql);

        List<EmailToDayReportSumExcel.Model.UserStore> GetUserStores(string sql);

        /// <summary>
        /// 根据sql获取table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetTableBySql(string sql);

    }
}
