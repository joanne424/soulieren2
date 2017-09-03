using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceApp
{
    public class Message
    {
        private MailMessage mailMessage;

        private Message()
        {
        }

        public static Message Factory(string subject,
            Encoding subjectEncoding,
            string body,
            Encoding bodyEncoding,
            bool isHtml,
            int priority,
            string to)
        {
            Message message = new Message();
            message.mailMessage = new MailMessage();
            message.mailMessage.Subject = subject;
            message.mailMessage.SubjectEncoding = subjectEncoding;
            message.mailMessage.Body = body;
            message.mailMessage.BodyEncoding = bodyEncoding;
            message.mailMessage.IsBodyHtml = isHtml;
            message.mailMessage.Priority = PriorityLevel(priority);
            message.mailMessage.To.Add(new MailAddress(to));
            return message;
        }

        private static MailPriority PriorityLevel(int level)
        {
            switch (level)
            {
                case 0:
                    return MailPriority.High;
                    break;
                case 1:
                    return MailPriority.Normal;
                    break;
                case 2:
                    return MailPriority.Low;
                    break;
                default:
                    return MailPriority.Normal;
                    break;
            }
        }

        /// <summary>
        /// 电子邮件地址解析
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public ArrayList EmailAddressArray(string strSrc)
        {
            string[] strEmailAddress = strSrc.Split(';');
            return GetAddressList(strEmailAddress);
        }

        public MailMessage GetMailMsg()
        {
            return mailMessage;
        }

        /// <summary>
        /// 返回电子邮件地址列表,不含空值
        /// </summary>
        /// <param name="srcArr"></param>
        /// <returns></returns>
        private ArrayList GetAddressList(string[] srcArr)
        {
            ArrayList addressList = new ArrayList();
            for (int i = 0; i < srcArr.Length; i++)
            {
                if ((srcArr[i].ToString().Trim() != "") && (srcArr[i].ToString().Trim() != null) && (srcArr[i].ToString().Trim() != string.Empty))
                {
                    addressList.Add(srcArr[i].ToString().Trim());
                }
            }

            return addressList;
        }


    }
}
