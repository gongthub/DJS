using DJS.Common;
using DJS.Common.Util;
using DJS.IDAL;
using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class UserMgr : XmlDB, IUserMgr
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/SYSUSERMGRS/SYSUSERMGR";

        private const string GROUPSPATH = @"/DB/SYSUSERMGRS";

        private const string ELENMENTNAME = "SYSUSERMGR";

        public UserMgr()
            : base(XMLDBCONFIGPATH, GROUPPATH, GROUPSPATH, ELENMENTNAME)
        {
        }

        public List<Model.UserEntity> GetAllList()
        {
            return GetModels<UserEntity>();
        }

        public List<Model.UserEntity> GetList()
        {
            List<Model.UserEntity> models = GetAllList();
            models = models.FindAll(m => m.DeleteMark != true);
            return models;
        }

        public List<Model.UserEntity> GetList(Pagination pagination)
        {
            List<Model.UserEntity> models = GetAllList();
            models = models.FindAll(m => m.DeleteMark != true);
            if (models == null)
            {
                models = new List<Model.UserEntity>();
            }
            models = models.Skip<Model.UserEntity>(pagination.rows * (pagination.page - 1)).Take<Model.UserEntity>(pagination.rows).ToList();
            return models;
        }

        public List<Model.UserEntity> GetList(Common.Pagination pagination, string keyword)
        {
            List<Model.UserEntity> models = GetAllList();

            models = models.FindAll(m => m.DeleteMark != true);
            if (keyword != null)
            {
                models = models.FindAll(m => (m.Account.Contains(keyword) || m.RealName.Contains(keyword) || m.MobilePhone.Contains(keyword)));
            }
            if (models == null)
            {
                models = new List<Model.UserEntity>();
            }
            models = models.Skip<Model.UserEntity>(pagination.rows * (pagination.page - 1)).Take<Model.UserEntity>(pagination.rows).ToList();
            return models;
        }

        public Model.UserEntity GetForm(string keyValue)
        {
            return GetModel<UserEntity>(keyValue);
        }
        public Model.UserEntity GetFormEnableByUserName(string userName)
        {
            Model.UserEntity model = new Model.UserEntity();
            List<Model.UserEntity> models = GetList();
            models = models.FindAll(m => m.EnabledMark == true);
            if (models != null && models.Count > 0)
            {
                model = models.Find(m => m.Account == userName);

            }
            return model;
        }

        public bool DeleteForm(string keyValue)
        {
            bool bState = false;
            try
            {
                Remove(ConfigHelp.SYSKEYNAME, keyValue);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

        public bool AddForm(Model.UserEntity userEntity)
        {
            bool bState = false;
            try
            {
                Add(userEntity);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

        public bool UpdateForm(Model.UserEntity userEntity)
        {
            bool bState = false;
            try
            {
                Update(userEntity, ConfigHelp.SYSKEYNAME, userEntity.ID);
                bState = true;
            }
            catch (Exception e)
            {
                bState = false;
                throw e;
            }
            return bState;
        }

    }
}
