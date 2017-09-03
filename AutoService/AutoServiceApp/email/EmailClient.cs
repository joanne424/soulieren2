using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceApp
{
    public class EmailClient
    {
        private static EmailClient instance;

        private SmtpClient smtpClient;
        private EmailClient(string host, int port, string from, string pwd)
        {
            this.smtpClient = new SmtpClient();
            this.smtpClient.UseDefaultCredentials = true;
            this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.smtpClient.Host = host;
            this.smtpClient.Port = port;

            System.Net.NetworkCredential userInfo = new System.Net.NetworkCredential(from, pwd);
            this.smtpClient.Credentials = userInfo;
        }

        public static EmailClient Instance
        {
            get
            {
                if (instance == null)
                {
                    string host = ConfigParameter.Instance.FromHost;
                    int port = int.Parse(ConfigParameter.Instance.FromPort);
                    string from = ConfigParameter.Instance.FromUser;
                    string pwd = ConfigParameter.Instance.FromPwd;
                    instance = new EmailClient(host, port, from, pwd);
                }

                return instance;
            }
        }

        public bool Send(MailMessage message)
        {
            try
            {
                message.From = new MailAddress(ConfigParameter.Instance.FromUser);
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Infrastructure.Log.TraceManager.Error.Write("Send", ex);
                return false;
            }
        }
    }
}
