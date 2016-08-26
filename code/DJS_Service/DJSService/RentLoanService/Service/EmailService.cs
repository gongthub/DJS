using RentLoanService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentLoanService.Model;
using System.Data.Entity;
using RentLoanService.Utils;

namespace RentLoanService.Service
{
    public class EmailService
    {
        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static EmailService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion
        public static void SendEmail()
        {
            try
            {
                IDBRepository dbRepository = new IDBRepository();
                var UserProfileList = from q in dbRepository.EmailUserProfileDetails
                                      group q by new { q.CategoryID, q.StoreID, q.Type } into g
                                      select new
                                      {
                                          g.Key.CategoryID,
                                          g.Key.StoreID,
                                          g.Key.Type
                                      };
                foreach (var item in UserProfileList)
                {
                    switch (item.CategoryID)
                    {
                        case (int)EnumUtility.EmailCategory.租金贷预警:
                            RentLoanService.SendRentLoanEmail((int)EnumUtility.EmailCategory.租金贷预警, item.StoreID, item.Type);
                            break;
                    }
                }

            }
            catch (Exception e)
            {

                iLog.WriteLog(e.Message, 0);
                //NLog.LogManager.GetCurrentClassLogger().Info("DiaryWarning Error " + e.Message);
            }

        }



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="CategoryID"></param>
        public static void SendOverDueEmail(int CategoryID, int StoreID, string Category, string strTable)
        {
            try
            {
                string sLeaderNames = string.Empty;
                // 发邮件所需内容准备
                List<string> sTo = GetDiaryOverDueWarning(CategoryID, StoreID, (int)EnumUtility.EmialStatu.主送);
                List<string> sCC = GetDiaryOverDueWarning(CategoryID, StoreID, (int)EnumUtility.EmialStatu.抄送);
                string sSubject = Category;

                string sMessageBody = GetDiaryFillInWarningMessage(CategoryID, Category) + "<br/>" + strTable;

                // 发邮件
                if (!string.IsNullOrEmpty(sMessageBody))
                    EmailUtility.SendEmail(sTo, sCC, null, sSubject, sMessageBody);

            }
            catch (Exception e)
            {

                iLog.WriteLog(e.Message, 0);
                //NLog.LogManager.GetCurrentClassLogger().Info("DiaryWarning Error " + e.Message);
            }
        }
        /// <summary>
        /// 获取邮件发送内容
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="Categorys"></param>
        /// <returns></returns>
        private static string GetDiaryFillInWarningMessage(int CategoryID, string Category)
        {
            IDBRepository dbContext = new IDBRepository();
            List<EmailCategoryDetail> CategoryDetailList = dbContext.EmailCategoryDetails.Where(c => c.CategoryID == CategoryID).ToList();
            string content = string.Empty;
            foreach (var item in CategoryDetailList)
            {
                List<EmailModel> ModelList = dbContext.EmailModels.Where(c => c.ModelID == item.ModelID).ToList();
                foreach (var model in ModelList)
                {
                    //邮件内容
                    if (model.Type == (int)EnumUtility.EmialModel.Emial)
                    {
                        content = model.Contents;
                        content = content.Replace("{Category}", Category);
                        content = content.Replace("{DiaryDate}", DateTime.Now.ToString("yyyy年MM月dd日"));
                    }
                    //短信内容
                    if (model.Type == (int)EnumUtility.EmialModel.SMS)
                    {

                    }
                }

            }
            return content;
        }

        /// <summary>
        /// 获取发送人
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <param name="Statu"></param>
        /// <returns></returns>
        private static List<string> GetDiaryOverDueWarning(int CategoryID, int StoreID, int Status)
        {
            try
            {
                IDBRepository dbContext = new IDBRepository();
                List<EmailUserProfileDetail> EmailList = dbContext.EmailUserProfileDetails.Where(c => c.CategoryID == CategoryID && c.StoreID == StoreID).ToList();

                List<string> ccs = new List<string>();
                foreach (var item in EmailList)
                {
                    List<UserProfile> UserList = dbContext.UserProfiles.Where(c => c.UserID == item.UserID).ToList();
                    foreach (var model in UserList)
                    {
                        if (item.Status == Status)
                        {
                            ccs.Add(model.Email);
                        }
                        else if (item.Status == (int)EnumUtility.EmialStatu.短信)
                        {
                            ccs.Add(model.Phone);
                        }
                    }
                }
                return ccs;
            }
            catch (Exception e)
            {
                iLog.WriteLog("DiaryWarning Error " + e.Message,1);
                return null;
            }

        }

        /// <summary>
        /// 邮件发送日志表
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <param name="statu">1:成功 2:失败</param>
        /// <param name="type">0:邮件 1:短信</param>
        /// <param name="message">内容</param>
        public static void AddEmailLogs(int CategoryID, int StoreID, int statu, int type, string message)
        {
            try
            {
                IDBRepository dbContext = new IDBRepository();
                List<EmailUserProfileDetail> EmailList = dbContext.EmailUserProfileDetails.Where(c => c.CategoryID == CategoryID && c.StoreID == StoreID).ToList();
                string strUserID = string.Empty;
                foreach (var item in EmailList)
                {
                    strUserID += item.UserID + ",";
                }
                EmailLog model = new EmailLog();
                model.CategoryID = CategoryID;
                model.Status = statu;
                model.Type = type;
                model.CreateDate = DateTime.Now;
                model.Message = message;
                model.UserID = strUserID;
                model.StoreID = StoreID;
                dbContext.EmailLogs.Add(model);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                iLog.WriteLog("DiaryWarning Error " + e.Message,1);
            }
        }

    }
}
