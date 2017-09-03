using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api.Request;

namespace AutoServiceApp
{
    public class SmsMessage
    {
        public AlibabaAliqinFcSmsNumSendRequest Req { get; private set; }

        public string RecNum { get; set; }

        public int MyProperty { get; set; }

        public static SmsMessage Factoy(string phone,string template, string param)
        {
            SmsMessage message = new SmsMessage();
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "搜猎人";
            req.SmsParam = param;
            req.RecNum = phone;
            req.SmsTemplateCode = template;
            message.Req = req;
            return message;
        }
    }
}
