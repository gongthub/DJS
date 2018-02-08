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
            return iJobFiles.GetList();
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public static List<Model.JobFiles> GetModels(string jobId)
        {
            return iJobFiles.GetModels(jobId);
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

        #region 根据名称判断是否存在 +static bool IsExist(string name,string jobID)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        public static bool IsExist(string name, string jobID)
        {
            return iJobFiles.IsExist(name, jobID);
        }
        #endregion

        #region 根据ID物理删除
        /// <summary>
        /// 根据ID物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            DelById(keyValue);
            return iJobFiles.DeleteForm(keyValue);
        }
        #endregion

        #region 根据ID逻辑删除
        /// <summary>
        /// 根据ID逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;

            Model.JobFiles model = GetForm(keyValue);
            if (model != null)
            {
                string tarPath = ConfigHelp.SYSDELETESRC + model.ID + @"/";
                FileHelp.MoveFiles(model.Src, tarPath);

                model.Remove();
                ret = UpdateForm(model);
            }
            return ret;
        }
        #endregion

        #region 根据id物理删除 +void DelById(string Id)
        /// <summary>
        /// 根据id物理删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static void DelById(string Id)
        {
            Model.JobFiles model = GetForm(Id);
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

        #region 根据JobId物理删除 +bool DelByJobId(string JobId)
        /// <summary>
        /// 根据JobId物理删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByJobId(string JobId)
        {
            bool bState = true;
            try
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
                iJobFiles.DelByJobId(JobId);
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }
        #endregion

        #region 根据JobName物理删除 +bool DelByJobName(string JobName)
        /// <summary>
        /// 根据JobName物理删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByJobName(string JobName)
        {
            bool bState = true;
            try
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
                iJobFiles.DelByJobName(JobName);
            }
            catch
            {
                bState = false;
            }
            return bState;
        }
        #endregion

        #region 根据JobId逻辑删除 +bool DelLogicByJobID(string JobId)
        /// <summary>
        /// 根据JobId逻辑删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelLogicByJobID(string JobId)
        {
            bool bState = true;
            try
            {
                List<Model.JobFiles> models = GetModels(m => m.JobID == JobId).ToList();
                if (models != null && models.Count > 0)
                {
                    foreach (Model.JobFiles model in models)
                    {
                        string tarPath = ConfigHelp.SYSDELETESRC + model.ID + @"/";
                        FileHelp.MoveFiles(model.Src, tarPath);

                        model.Remove();
                        UpdateForm(model);
                    }
                }
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }
        #endregion

        #region 根据JobName逻辑删除DLL +bool DelLogicByJobName(string JobName)
        /// <summary>
        /// 根据id删除DLL
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelLogicByJobName(string JobName)
        {
            bool bState = true;
            try
            {
                List<Model.JobFiles> models = GetModels(m => m.JobName == JobName).ToList();
                if (models != null && models.Count > 0)
                {
                    foreach (Model.JobFiles model in models)
                    {
                        string tarPath = ConfigHelp.SYSDELETESRC + model.ID + @"/";
                        FileHelp.MoveFiles(model.Src, tarPath);

                        model.Remove();
                        UpdateForm(model);
                    }
                }
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }
        #endregion

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool RemoveByID(string keyValue)
        {
            bool bState = true;
            if (ConfigHelp.SYSDELETEMODEL == 0)
            {
                bState = DeleteByID(keyValue);
            }
            else
                if (ConfigHelp.SYSDELETEMODEL == 1)
                {
                    bState = DeleteForm(keyValue);
                }
            return bState;
        }

        /// <summary>
        /// 根根据JobID删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool RemoveByJobId(string jobID)
        {
            bool bState = true;
            if (ConfigHelp.SYSDELETEMODEL == 0)
            {
                bState = DelLogicByJobID(jobID);
            }
            else
                if (ConfigHelp.SYSDELETEMODEL == 1)
                {
                    bState = DelByJobId(jobID);
                }
            return bState;
        }
        /// <summary>
        /// 根根据JobName删除数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool RemoveByJobName(string jobName)
        {
            bool bState = true;
            if (ConfigHelp.SYSDELETEMODEL == 0)
            {
                bState = DelLogicByJobName(jobName);
            }
            else
                if (ConfigHelp.SYSDELETEMODEL == 1)
                {
                    bState = DelByJobName(jobName);
                }
            return bState;
        }

        #region 根据id获取数据 +static Model.JobFiles GetForm(string Id)
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static Model.JobFiles GetForm(string Id)
        {
            return iJobFiles.GetForm(Id);
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
            bool bState = true;
            if (!IsExist(model.Name, model.JobID))
            {
                model.Create();
                bState = iJobFiles.AddForm(model);
            }
            return bState;
        }
        #endregion

        public static bool UpdateForm(Model.JobFiles modelEntity)
        {
            return iJobFiles.UpdateForm(modelEntity);
        }

    }
}
