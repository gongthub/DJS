using System.Collections.Generic;
using System.Data;


namespace EmailToRepoertExcel.DBHelper
{
    public interface IDBHelper
    {
        DataSet GetSysConfig();//不从数据库中读取

        List<EmailToRepoertExcel.Model.SumRepoertTable> GetListSumRepoertTable(string sql);


        List<EmailToRepoertExcel.Model.Store> GetListStore(string sql);

        List<EmailToRepoertExcel.Model.AllSumRepoertTable> GetListAllSumRepoertTable(string sql);

        List<EmailToRepoertExcel.Model.UserStore> GetUserStores(string sql);

        /// <summary>
        /// 根据sql获取table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetTableBySql(string sql);

    }
}
