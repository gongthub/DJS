using DJS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public static class DllMgr
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static IDAL.IDllMgr iDllMgr = null;

        #endregion

        #region 构造函数

        static DllMgr()
        {
            iDllMgr = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIDllMgr();
        }

        #endregion

        #region 获取数据集合 +static List<Model.DllMgr> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.DllMgr> GetModels()
        {
            return iDllMgr.GetModels();
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.DllMgr> GetModels(Predicate<Model.DllMgr> pre)
        {
            List<Model.DllMgr> models = GetModels();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
        }
        #endregion

        /// <summary>
        /// 验证名称是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsExistName(string name, string keyvalue = "")
        {
            bool bSatae = false;
            if (!string.IsNullOrEmpty(keyvalue))
            {
                List<Model.DllMgr> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.DllMgr model = models.Where(m => m.Name == name && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.DllMgr> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.DllMgr model = models.Where(m => m.Name == name).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            return bSatae;
        }
        #region 根据名称判断是否存在 +static bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        public static bool IsExist(string name)
        {
            return iDllMgr.IsExist(name);
        }
        #endregion

        #region 根据id删除数据 +static bool DelById(string Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelById(string Id)
        {
            DelByDllId(Id);
            return iDllMgr.DeleteForm(Id);
        }
        #endregion

        #region 根据id获取程序集实现类集合 +static List<SelectLists> GetDllNameList(string keyValue)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<SelectLists> GetDllNameList(string keyValue)
        {
            List<SelectLists> list = new List<SelectLists>();
            Model.DllMgr modelEntity = BLL.DllMgr.GetForm(keyValue);
            if (modelEntity != null && !string.IsNullOrEmpty(modelEntity.ID))
            {
                ArrayList arry = Common.QuartzHelp.quartzHelp.GetIClassName(modelEntity.Name, modelEntity.NameSpace);
                if (arry != null && arry.Count > 0)
                {
                    for (int i = 0; i < arry.Count; i++)
                    {
                        string val = string.Empty;
                        if (arry[i] != null)
                        {
                            val = arry[i].ToString();
                        }
                        SelectLists sel = new SelectLists();
                        sel.Name = val;
                        list.Add(sel);
                    }
                }
            }
            return list;
        }
        #endregion

        #region 根据id获取数据 +static Model.DllMgr GetModelById(string Id)
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Model.DllMgr GetModelById(string Id)
        {
            return iDllMgr.GetForm(Id);
        }
        #endregion

        #region 根据id删除DLL +void DelByDllId(string Id)
        /// <summary>
        /// 根据id删除DLL
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static void DelByDllId(string Id)
        {
            Model.DllMgr model = GetModelById(Id);
            if (model != null)
            {
                string filepath = FileHelp.GetFullPath(model.Url);
                //文件存在时先删除
                if (FileHelp.FileExists(filepath))
                {
                    FileHelp.DeleteFiles(filepath);
                }
            }
        }
        #endregion

        #region 添加 +static bool Add(Model.DllMgr model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Model.DllMgr model)
        {
            return iDllMgr.AddForm(model);
        }
        #endregion

        #region 获取所有数据集合（包括已删除数据） +static List<Model.DllMgr> GetAllList()
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<Model.DllMgr> GetAllList()
        {
            return iDllMgr.GetAllList();
        }
        #endregion

        #region 获取所有未删除数据集合
        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.DllMgr> GetList()
        {
            List<Model.DllMgr> models = GetAllList();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m => m.DeleteMark != true);
            }
            return models;
        }

        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<Model.DllMgr> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<Model.DllMgr>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
                expression = expression.Or(t => t.NameSpace.Contains(keyword));
            }
            return iDllMgr.GetList(pagination, expression);
        }
        #endregion

        public static Model.DllMgr GetForm(string keyValue)
        {
            return iDllMgr.GetForm(keyValue);
        }

        public static bool SubmitForm(Model.DllMgr modelEntity, string keyValue)
        {
            if (IsExistName(modelEntity.Name, keyValue))
            {
                throw new Exception("名称已存在！");
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                modelEntity.Modify(keyValue);
                return UpdateForm(modelEntity);
            }
            else
            {
                modelEntity.Create();
                return AddForm(modelEntity);
            }
        }
        public static bool AddForm(Model.DllMgr modelEntity)
        {
            return iDllMgr.AddForm(modelEntity);
        }
        public static bool UpdateForm(Model.DllMgr modelEntity)
        {
            return iDllMgr.UpdateForm(modelEntity);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            return iDllMgr.DeleteForm(keyValue);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;
            Model.DllMgr model = GetForm(keyValue);
            if (model != null)
            {
                model.Remove();
                ret = UpdateForm(model);
            }
            return ret;
        }

        /// <summary>
        /// 生成文件保存目录
        /// </summary>
        /// <returns></returns>
        public static string GenFilePath(string name)
        {
            return ConfigHelp.AssemblySrcPath + "/" + name + "/";
        }

    }
}
