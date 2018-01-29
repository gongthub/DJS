using DJS.Common;
using DJS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DJS.DAL.XML
{
    public class UserMgr : IUserMgr
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string XMLDBCONFIGPATH = ConfigHelp.XmlDBConfigPath;

        private const string GROUPPATH = @"/DB/SYSUSERMGRS/SYSUSERMGR";

        private const string GROUPSPATH = @"/DB/SYSUSERMGRS";

        private const string ELENMENTNAME = "SYSUSERMGR";

        public List<Model.UserEntity> GetModels()
        {
            List<Model.UserEntity> models = new List<Model.UserEntity>();
            XmlNodeList list = XmlHelp.xmlHelp.GetNodes(XMLDBCONFIGPATH, GROUPPATH);
            if (list != null && list.Count > 0)
            {
                Model.UserEntity group = new Model.UserEntity();
                foreach (XmlNode node in list)
                {
                    group = new Model.UserEntity();
                    group = XmlHelp.xmlHelp.SetNodeToModel(group, node);
                    models.Add(group);
                }
            }
            return models;
        }

        public List<Model.UserEntity> GetList()
        {
            List<Model.UserEntity> models = GetModels();
            models = models.FindAll(m => m.DeleteMark != true);
            return models;
        }

        public List<Model.UserEntity> GetList(Pagination pagination)
        {
            List<Model.UserEntity> models = GetModels();
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
            List<Model.UserEntity> models = GetModels();

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
            Model.UserEntity model = new Model.UserEntity();
            List<Model.UserEntity> models = GetList();
            if (models != null && models.Count > 0)
            {
                model = models.Find(m => m.ID == keyValue);

            }
            return model;
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
            bool ret = false;
            ret = XmlHelp.xmlHelp.RemoveNode(XMLDBCONFIGPATH, GROUPPATH, ConfigHelp.SYSKEYNAME, keyValue);
            return ret;
        }

        public bool AddForm(Model.UserEntity userEntity)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.AppendNode<Model.UserEntity>(userEntity, XMLDBCONFIGPATH, GROUPSPATH, ELENMENTNAME);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public bool UpdateForm(Model.UserEntity userEntity)
        {
            bool ret = true;
            try
            {
                XmlHelp.xmlHelp.UpdateNode<Model.UserEntity>(userEntity, XMLDBCONFIGPATH, GROUPPATH, ConfigHelp.SYSKEYNAME, userEntity.ID);
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

    }
}
