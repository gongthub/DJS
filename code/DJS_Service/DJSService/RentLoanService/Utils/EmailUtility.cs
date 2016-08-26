using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Utils
{
    public class EmailUtility
    {
        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static EmailUtility()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion
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
                    string sSender = ConstUtility.SEND_EMAIL_SENDER;
                    MailAddress from = new MailAddress(sSender);
                    using (MailMessage message = new MailMessage())
                    {
                        // 发件人
                        message.From = from;
                        // 收件人
                        foreach (string s in sTo)
                        {
                            MailAddress to = new MailAddress(s);
                            message.To.Add(to);
                        }
                        // 抄送人
                        if (sCC != null)
                        {
                            foreach (string s in sCC)
                            {
                                MailAddress to = new MailAddress(s);
                                message.CC.Add(to);
                            }
                        }
                        // 密送人
                        if (sBcc != null)
                        {
                            foreach (string s in sBcc)
                            {
                                MailAddress to = new MailAddress(s);
                                message.Bcc.Add(to);
                            }
                        }
                        //邮件内容
                        message.Body = sMessageBody;
                        //邮件主题
                        message.Subject = sSubject;
                        message.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient(ConstUtility.EMAIL_SMTP);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new System.Net.NetworkCredential(sSender, ConstUtility.SEND_EMAIL_PASSWORD);
                        client.Send(message);
                    }
                }
            }
            catch (Exception e)
            {
                iLog.WriteLog("Failed to send email(e.Message):" + e.Message, 1);
                if (e.InnerException != null)
                {
                    iLog.WriteLog("Failed to send email(e.InnerException.Message):" + e.InnerException.Message, 1);
                }
            }
        }
    }
}
