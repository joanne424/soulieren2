using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceApp
{
    public interface IExternalOpt
    {
        string SendEmail(string templateid, string msg);

        string SendSms(string phone, string templateId, string param);

        //string RemoveOrder();

        //string ResetLogin();
    }
}
