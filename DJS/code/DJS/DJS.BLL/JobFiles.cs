using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public static class JobFiles
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static IDAL.IJobFiles iJobFiles = null;

        #endregion

        #region 构造函数

        static JobFiles()
        {
            iJobFiles = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIJobFiles();
        }

        #endregion


        #region 获取数据集合 +static List<Model.JobFiles> GetModels()
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobFiles> GetModels()
        {
            return iJobFiles.GetModels();
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobFiles> GetModels(Predicate<Model.JobFiles> pre)
        {
            List<Model.JobFiles> models = GetModels();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
        }
        #endregion

        #region 根据名称判断是否存在 +static bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        public static bool IsExist(string name)
        {
            return iJobFiles.IsExist(name);
        }
        #endregion

        #region 根据id删除数据 +static bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelById(Guid Id)
        {
            DelByFileId(Id);
            return iJobFiles.DelById(Id);
        }
        #endregion

        #region 根据id删除数据 +static bool  DelByJobId(Guid JobId)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByJobId(Guid JobId)
        {
            DelByFilesId(JobId);
            return iJobFiles.DelByJobId(JobId);
        }
        #endregion

        #region 根据JobName删除数据 +static bool  DelByJobName(string JobName)
        /// <summary>
        /// 根据JobName删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByJobName(string JobName)
        {
            DelByFilesJobName(JobName);
            return iJobFiles.DelByJobName(JobName);
        }
        #endregion

        #region 根据id获取数据 +static Model.JobFiles GetModelById(Guid Id)
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Model.JobFiles GetModelById(Guid Id)
        {
            return iJobFiles.GetModelById(Id);
        }
        #endregion

        #region 根据id删除DLL +void DelByFileId(Guid Id)
        /// <summary>
        /// 根据id删除DLL
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static void DelByFileId(Guid Id)
        {
            Model.JobFiles model = GetModelById(Id);
            if (model != null)
            {
                string filepath = FileHelp.GetFullPath(model.Src);
                //文件存在时先删除
                if (FileHelp.FileExists(filepath))
                {
                    FileHelp.DeleteFiles(filepath);
                }
            }
        }
        #endregion

        #region 根据id删除DLL +void DelByFilesId(Guid JobId)
        /// <summary>
        /// 根据id删除DLL
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static void DelByFilesId(Guid JobId)
        {
            List<Model.JobFiles> models = GetModels(m => m.JobID == JobId).ToList();
            if (models != null && models.Count > 0)
            {
                foreach (Model.JobFiles model in models)
                {
                    string filepath = FileHelp.GetFullPath(model.Src);
                    //文件存在时先删除
                    if (FileHelp.FileExists(filepath))
                    {
                        FileHelp.DeleteFiles(filepath);
                    }
                }
            }
        }
        #endregion

        #region 根据id删除DLL +void DelByFilesJobName(string JobName)
        /// <summary>
        /// 根据id删除DLL
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static void DelByFilesJobName(string JobName)
        {
            List<Model.JobFiles> models = GetModels(m => m.JobName == JobName).ToList();
            if (models != null && models.Count > 0)
            {
                foreach (Model.JobFiles model in models)
                {
                    string filepath = FileHelp.GetFullPath(model.Src);
                    //文件存在时先删除
                    if (FileHelp.FileExists(filepath))
                    {
                        FileHelp.DeleteFiles(filepath);
                    }
                }
            }
        }
        #endregion

        #region 添加 +static bool Add(Model.JobFiles model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Model.JobFiles model)
        {
            return iJobFiles.Add(model);
        }
        #endregion

    }
}
