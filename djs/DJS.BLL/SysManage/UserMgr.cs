using DJS.Common;
using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class UserMgr
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static IDAL.IUserMgr iUserMgr = null;
        private const string SECRETKEY = "djs2018w";

        #endregion

        #region 构造函数

        static UserMgr()
        {
            iUserMgr = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIUserMgr();
        }

        #endregion

        #region 获取所有数据集合（包括已删除数据） +static List<UserEntity> GetModels()
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<UserEntity> GetModels()
        {
            return iUserMgr.GetModels();
        }
        /// <summary>
        /// 获取所有数据集合（包括已删除数据）
        /// </summary>
        /// <returns></returns>
        public static List<UserEntity> GetModels(Predicate<UserEntity> pre)
        {
            List<UserEntity> models = GetModels();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
        }
        #endregion

        #region 获取所有未删除数据集合
        /// <summary>
        /// 获取所有未删除数据集合
        /// </summary>
        /// <returns></returns>
        public static List<UserEntity> GetList()
        {
            List<UserEntity> models = GetModels();
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
        public static List<UserEntity> GetList(Pagination pagination, string keyword)
        {
            return iUserMgr.GetList(pagination, keyword);
        }
        #endregion

        public static UserEntity GetForm(string keyValue)
        {
            return iUserMgr.GetForm(keyValue);
        }
        public static UserEntity GetFormEnableByUserName(string username)
        {
            return iUserMgr.GetFormEnableByUserName(username);
        }

        public static bool SubmitForm(UserEntity userEntity, string keyValue)
        {
            if (IsExistAccount(userEntity.Account, keyValue))
            {
                throw new Exception("用户名已存在！");
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
                Model.UserEntity userEntityT = GetForm(userEntity.ID);
                if (userEntityT != null)
                {
                    userEntity.Password = userEntityT.Password;
                }
                return UpdateForm(userEntity);
            }
            else
            {
                userEntity.Create();
                userEntity.Password = Common.SecurityHelp.md5(userEntity.Password, 32);
                userEntity.Password = GetPwdMd5(userEntity.Password);
                return AddForm(userEntity);
            }
        }
        public static bool AddForm(UserEntity userEntity)
        {
            return iUserMgr.AddForm(userEntity);
        }
        public static bool UpdateForm(UserEntity userEntity)
        {
            return iUserMgr.UpdateForm(userEntity);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteForm(string keyValue)
        {
            return iUserMgr.DeleteForm(keyValue);
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DeleteByID(string keyValue)
        {
            bool ret = false;
            UserEntity model = GetForm(keyValue);
            if (model != null)
            {
                model.Remove();
                ret = UpdateForm(model);
            }
            return ret;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool RevisePassword(string keyValue, string userPassword)
        {
            bool ret = false;
            UserEntity model = GetForm(keyValue);
            if (model != null)
            {
                userPassword = Common.SecurityHelp.md5(userPassword, 32);
                string pwds = GetPwdMd5(userPassword);
                model.Password = pwds;
                ret = UpdateForm(model);
            }
            return ret;
        }

        /// <summary>
        /// 禁止用户
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool DisabledAccount(string keyValue)
        {
            bool ret = false;
            UserEntity model = GetForm(keyValue);
            if (model != null)
            {
                model.Modify(keyValue);
                model.EnabledMark = false;
                ret = UpdateForm(model);
            }
            return ret;
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool EnabledAccount(string keyValue)
        {
            bool ret = false;
            UserEntity model = GetForm(keyValue);
            if (model != null)
            {
                model.Modify(keyValue);
                model.EnabledMark = true;
                ret = UpdateForm(model);
            }
            return ret;
        }

        /// <summary>
        /// 用户登陆认证
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static bool CheckLogin(string username, string password, ref UserEntity userModel)
        {
            bool bState = false;
            UserEntity model = GetFormEnableByUserName(username);
            if (model != null)
            {
                string pwdmd5 = GetPwdMd5(password);
                if (pwdmd5.Equals(model.Password))
                {
                    bState = true;
                    userModel = model;
                }
            }
            return bState;
        }

        /// <summary>
        /// 验证用户名是否已存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private static bool IsExistAccount(string account, string keyvalue = "")
        {
            bool bSatae = false;
            if (!string.IsNullOrEmpty(keyvalue))
            {
                List<Model.UserEntity> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.UserEntity model = models.Where(m => m.Account == account && m.ID != keyvalue).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            else
            {
                List<Model.UserEntity> models = GetList();
                if (models != null && models.Count > 0)
                {
                    Model.UserEntity model = models.Where(m => m.Account == account).FirstOrDefault();
                    if (model != null && !string.IsNullOrEmpty(model.ID))
                    {
                        bSatae = true;
                    }
                }
            }
            return bSatae;
        }

        /// <summary>
        /// 获取密码MD5
        /// </summary>
        /// <param name="pwds"></param>
        /// <returns></returns>
        private static string GetPwdMd5(string pwds)
        {
            string strpwds = string.Empty;
            strpwds = Common.SecurityHelp.md5(Common.SecurityHelp.Encrypt(pwds.ToLower(), SECRETKEY).ToLower(), 32).ToLower();
            return strpwds;
        }
    }
}
