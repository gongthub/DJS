using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class EmailHelp
    {
         
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
                Common.LogHelp.logHelp.WriteLog("错误邮件发送开始！", Model.Enums.LogType.Normal);
                if (sTo != null && sTo.Count > 0)
                {
                    string sSender = ConfigHelp.ERRORSENDUSER;
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


                        SmtpClient client = new SmtpClient(ConfigHelp.ERRORSENDSMTP);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new System.Net.NetworkCredential(sSender, ConfigHelp.ERRORSENDPWD);
                        client.Send(message);
                    }
                }
                Common.LogHelp.logHelp.WriteLog("错误邮件发送结束！", Model.Enums.LogType.Normal);
            }
            catch (Exception e)
            {
                Common.LogHelp.logHelp.WriteLog("Failed to send email(e.Message):" + e.Message, 1);
                if (e.InnerException != null)
                {
                    Common.LogHelp.logHelp.WriteLog("Failed to send email(e.InnerException.Message):" + e.InnerException.Message, 1);
                }
            }
        }


        /// <summary>
        /// 邮件发送
        /// </summary> 
        /// <param name="sSubject">主题</param>
        /// <param name="sMessageBody">内容</param>
        public static void SendEmail(string sSubject, string sMessageBody)
        {
            List<string> sTo = new List<string>();
            List<string> sCC = new List<string>();
            List<string> sBC = new List<string>();
            //string[] arr = sendToEmail.Split(';');
            if (!string.IsNullOrEmpty(ConfigHelp.ERRORTOUSER))
            {
                string[] toUser = ConfigHelp.ERRORTOUSER.Split(';');
                foreach (var temp in toUser)
                {
                    sTo.Add(temp);
                }
            }

            if (!string.IsNullOrEmpty(ConfigHelp.ERRORCCUSER))
            {
                string[] ccUser = ConfigHelp.ERRORCCUSER.Split(';');
                foreach (var temp in ccUser)
                {
                    sCC.Add(temp);
                }
            }
            if (!string.IsNullOrEmpty(ConfigHelp.ERRORBCUSER))
            {
                string[] bcUser = ConfigHelp.ERRORBCUSER.Split(';');
                foreach (var temp in bcUser)
                {
                    sBC.Add(temp);
                }
            }
            SendEmail(sTo, sCC, sBC, sSubject, sMessageBody);
        }
    }
}
