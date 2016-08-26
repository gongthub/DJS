using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Utils
{
    public class EmailUtility
    {
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="sTo">主送</param>
        /// <param name="sCC">抄送</param>
        /// <param name="sBcc">密送</param>
        /// <param name="sSubject">主题</param>
        /// <param name="sMessageBody">内容</param>
        public static void SendEmail(List<string> sTo, List<string> sCC, List<string> sBcc, string sSubject, string sMessageBody,string file)
        {
            try
            {
                if (sTo != null && sTo.Count > 0)
                {
                    string sSender = ConstUtility.SendEmail;
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
                        if(sCC != null)
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

                        if (!string.IsNullOrEmpty(file)) //如果附件不为空
                        {
                            System.Net.Mail.Attachment mailAttach = new Attachment(file);//附件  
                            message.Attachments.Add(mailAttach);
                            message.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");//邮件采用的编码  
                            message.Priority = MailPriority.High;//设置邮件的优先级为高  
                        }
                        SmtpClient client = new SmtpClient("smtp.exmail.qq.com");
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new System.Net.NetworkCredential(sSender, ConstUtility.SendPwd);
                        client.Send(message);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Instance().Error("Failed to send email(e.Message):" + e.Message);
                if (e.InnerException != null)
                {
                    Log.Instance().Error("Failed to send email(e.InnerException.Message):" + e.InnerException.Message);
                }
            }
        }
    }
}
