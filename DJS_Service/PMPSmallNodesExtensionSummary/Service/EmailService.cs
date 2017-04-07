

using PMPSmallNodesExtensionSummary.Model;
using PMPSmallNodesExtensionSummary.Service;
using PMPSmallNodesExtensionSummary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PMPSmallNodesExtensionSummary.Service
{
    public class EmailService
    {
        public static DJS.SDK.ILog iLog = null;
        private static IDBRepository dbRepository = null;

        static EmailService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            dbRepository = new IDBRepository();
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="sTo">主送</param>
        /// <param name="sCC">抄送</param>
        /// <param name="sBcc">密送</param>
        /// <param name="sSubject">主题</param>
        /// <param name="sMessageBody">内容</param>
        public static void SendEmail(List<string> sTo, List<string> sCC, List<string> sBcc, string sSubject, string sMessageBody)
        {
            try
            {
                if (sTo != null && sTo.Count > 0)
                {
                    string sSender = ConstUtility.SendMailSender;
                    MailAddress from = new MailAddress(sSender);
                    using (MailMessage message = new MailMessage())
                    {
                        // 发件人
                        message.From = from;
                        // 收件人
                        foreach (string s in sTo)
                        {
                            MailAddress to = new MailAddress(s.Trim());
                            message.To.Add(to);
                        }
                        // 抄送人
                        if (sCC != null)
                        {
                            foreach (string s in sCC)
                            {
                                MailAddress to = new MailAddress(s.Trim());
                                message.CC.Add(to);
                            }
                        }
                        // 密送人
                        if (sBcc != null)
                        {
                            foreach (string s in sBcc)
                            {
                                MailAddress to = new MailAddress(s.Trim());
                                message.Bcc.Add(to);
                            }
                        }
                        //邮件内容
                        message.Body = sMessageBody;
                        //邮件主题
                        message.Subject = sSubject;
                        message.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient(ConstUtility.EmailSMTP);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new System.Net.NetworkCredential(sSender, ConstUtility.SendMailPassword);
                        client.Send(message);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #region 服务邮箱配置信息

        #region 根据服务code获取服务主信息 +static Model.AMSService GetService(string code)
        /// <summary>
        /// 根据服务code获取服务主信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <returns></returns>
        public static Model.AMSService GetService(string code)
        {
            AMSService model = new AMSService();

            model = dbRepository.AMSServices.AsNoTracking().Where(m => m.ServiceCode == code && m.Status == 1).FirstOrDefault();

            return model;
        }
        #endregion

        #region 根据服务code获取服务子表信息 +static List<Model.AMSServiceSet> GetServiceSet(string code)
        /// <summary>
        /// 根据服务code获取服务子表信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<Model.AMSServiceSet> GetServiceSet(string code)
        {

            List<Model.AMSServiceSet> models = new List<AMSServiceSet>();

            AMSService model = new AMSService();

            model = GetService(code);
            if (model != null)
            {
                models = dbRepository.AMSServiceSets.AsNoTracking().Where(m => m.AMSServiceID == model.ID && m.Status == 1).ToList();
            }

            return models;
        }
        #endregion

        #region 根据服务code获取服务子表信息 +static List<Model.AMSServiceSet> GetServiceSet(string code, int type)
        /// <summary>
        /// 根据服务code获取服务子表信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<Model.AMSServiceSet> GetServiceSet(string code, int type)
        {
            List<Model.AMSServiceSet> models = new List<AMSServiceSet>();
            AMSService model = new AMSService();
            model = GetService(code);
            if (model != null)
            {
                models = dbRepository.AMSServiceSets.AsNoTracking().Where(m => m.AMSServiceID == model.ID && m.Status == 1 && m.SendType == type).ToList();
            }

            return models;
        }
        #endregion

        #region 根据服务code获取服务子表详细配置信息 +static List<Model.AMSServiceSetEmail> GetServiceSet(string code)
        /// <summary>
        /// 根据服务code获取服务子表信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<Model.AMSServiceSetEmail> GetServiceSetEmail(string code)
        {
            List<Model.AMSServiceSetEmail> models = new List<AMSServiceSetEmail>();
            List<Model.AMSServiceSet> modelsets = new List<AMSServiceSet>();
            AMSService model = new AMSService();
            model = GetService(code);
            if (model != null)
            {
                modelsets = GetServiceSet(code);
                if (modelsets != null && modelsets.Count > 0)
                {

                    List<Model.AMSServiceSetEmail> modelsT = new List<AMSServiceSetEmail>();
                    foreach (AMSServiceSet item in modelsets)
                    {
                        modelsT = dbRepository.AMSServiceSetEmails.AsNoTracking().Where(m => m.AMSServiceSetID == item.ID && m.Status == 1).ToList();
                        if (modelsT != null && modelsT.Count > 0)
                        {
                            models.AddRange(modelsT);
                        }
                    }
                }
            }

            return models;
        }
        #endregion

        #region 根据服务code获取服务子表详细配置收件人信息 +static List<string> GetServiceSetEmailTo(string code)
        /// <summary>
        /// 根据服务code获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailTo(string code)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.TOPeople != null && list.TOPeople != ""
                         select list.TOPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailToL(string code)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.TOPeople != null && list.TOPeople != ""
                         select list.TOPeople).ToList();

                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailToByStoreId(string code, int storeId)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.TOPeople != null && list.TOPeople != ""
                         && list.StoreID == storeId.ToString()
                         select list.TOPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailToByStoreId(string code, string storeIds)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.TOPeople != null && list.TOPeople != ""
                         && list.StoreID == storeIds.ToString()
                         select list.TOPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailToById(string code, int Id)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.TOPeople != null && list.TOPeople != ""
                         && list.ID == Id
                         select list.TOPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        #endregion

        #region 根据服务code获取服务子表详细配置抄送人信息 +static List<string> GetServiceSetEmailCc(string code)
        /// <summary>
        /// 根据服务code获取服务子表详细配置抄送人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailCc(string code)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.CCPeople != null && list.CCPeople != ""
                         select list.CCPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code获取服务子表详细配置抄送人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailCcL(string code)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.CCPeople != null && list.CCPeople != ""
                         select list.CCPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置抄送人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailCcByStoreId(string code, int storeId)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.CCPeople != null && list.CCPeople != ""
                         && list.StoreID == storeId.ToString()
                         select list.CCPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailCcByStoreId(string code, string storeIds)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.CCPeople != null && list.CCPeople != ""
                         && list.StoreID == storeIds.ToString()
                         select list.CCPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置收件人信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailCcById(string code, int Id)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                List<string> strsT = new List<string>();
                strsT = (from list in models
                         where list.CCPeople != null && list.CCPeople != ""
                         && list.ID == Id
                         select list.CCPeople).ToList();
                strsT.ForEach(delegate(string model)
                {
                    string[] strsTs = model.Split(';');
                    strs.AddRange(strsTs.ToArray());
                });
            }

            return strs;
        }
        #endregion

        #region 根据服务code获取服务子表详细配置门店信息 +static List<string> GetServiceSetEmailStores(string code)
        /// <summary>
        /// 根据服务code获取服务子表详细配置门店信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailStores(string code)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                strs = (from list in models
                        select list.StoreID).ToList();
            }
            return strs;
        }
        /// <summary>
        /// 根据服务code获取服务子表详细配置门店信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<string> GetServiceSetEmailStoreids(string code)
        {
            List<string> strs = new List<string>();
            List<string> models = GetServiceSetEmailStores(code);
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(string model)
                {
                    string[] strsT = model.Split(',');
                    strs.AddRange(strsT.ToArray());
                });
            }
            return strs;
        }

        /// <summary>
        /// 根据服务code获取服务子表详细配置门店信息
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static List<Model.AMSServiceSetEmail> GetServiceSetEmailStoresList(string code)
        {
            List<string> strs = new List<string>();
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);

            return models;
        }
        #endregion

        #region 根据服务code,门店Id获取服务子表详细配置是否发送汇总
        /// <summary>
        /// 根据服务code,门店Id获取服务子表详细配置是否发送汇总
        /// </summary>
        /// <param name="code">服务编码</param>
        /// <param name="type">发送类型</param>
        /// <returns></returns>
        public static bool GetServiceSetEmailIsSum(string code, string storeIds)
        {
            bool ret = false;
            List<Model.AMSServiceSetEmail> models = GetServiceSetEmail(code);
            if (models != null && models.Count > 0)
            {
                ret = (from list in models
                       where list.TOPeople.Trim() != ""
                       && list.StoreID == storeIds.ToString()
                       select list.IsSum).FirstOrDefault();
            }
            return ret;
        }
        #endregion

        #region 根据服务code判断服务是否开启
        /// <summary>
        /// 根据服务code判断服务是否开启
        /// </summary>
        /// <param name="code">服务编码</param> 
        /// <returns></returns>
        public static bool ServiceIsStart(string code)
        {
            bool ret = false;

            AMSService model = GetService(code);
            if (model != null)
            {
                ret = model.IsStart;
            }
            return ret;
        }
        #endregion

        #region 获取所有门店id
        /// <summary>
        /// 获取所有门店id
        /// </summary>  
        /// <returns></returns>
        public static string GetStoreIds()
        {
            string storeIds = "";

            string sqls = @"select ID from Stores
                            where Status in(2,4,5)";

            List<int> ids = dbRepository.Database.SqlQuery<int>(sqls).ToList();
            if (ids != null && ids.Count > 0)
            {
                foreach (int item in ids)
                {
                    if (storeIds != "")
                    {
                        storeIds = storeIds + ",";
                    }
                    storeIds = storeIds + item;
                }
            }
            return storeIds;
        }

        #endregion

        #endregion
    }
}
