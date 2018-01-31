using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IBaseMgr<TEntity>
    {
        /// <summary>
        /// 获取所有数据集合（包括未删除）
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAllList();
        /// <summary>
        /// 获取所有数据集合（不包括未删除）
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetList();
        /// <summary>
        /// 获取所有数据集合，分页获取（不包括未删除）
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        List<TEntity> GetList(Pagination pagination);
        /// <summary>
        /// 分页获取所有数据集合，分页并筛选（不包括未删除）
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<TEntity> GetList(Pagination pagination, Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        TEntity GetForm(string keyValue);
        /// <summary>
        /// 物理删除一条数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool DeleteForm(string keyValue);
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="modelEntity"></param>
        /// <returns></returns>
        bool AddForm(TEntity modelEntity);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="modelEntity"></param>
        /// <returns></returns>
        bool UpdateForm(TEntity modelEntity);
    }
}
