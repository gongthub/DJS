using System.Collections.Generic;
using System.Data;


namespace EmailToRevparExcel.DBHelper
{
    public interface IDBHelper
    {
        DataSet GetSysConfig();//不从数据库中读取

        List<EmailToRevparExcel.Model.SumRepoertTable> GetListSumRepoertTable(string sql);


        List<EmailToRevparExcel.Model.Store> GetListStore(string sql);

        List<EmailToRevparExcel.Model.AllSumRepoertTable> GetListAllSumRepoertTable(string sql);

        List<EmailToRevparExcel.Model.UserStore> GetUserStores(string sql);

        /// <summary>
        /// 根据sql获取table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetTableBySql(string sql);

    }
}
