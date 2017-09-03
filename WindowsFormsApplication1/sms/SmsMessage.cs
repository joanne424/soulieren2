// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmsMessage.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:41:54 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:41:54
//      修改描述：新建 SmsMessage.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;

namespace SendMSM
{
    using Top.Api.Request;

    /// <summary>
    /// The sms message.
    /// </summary>
    public class SmsMessage
    {
        #region Public Properties
        /// <summary>
        /// Gets the req.
        /// </summary>
        public AlibabaAliqinFcSmsNumSendRequest Req { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The factoy.
        /// </summary>
        /// <param name="phone">
        /// The phone.
        /// </param>
        /// <param name="template">
        /// The template.
        /// </param>
        /// <param name="param">
        /// The param.
        /// </param>
        /// <returns>
        /// The <see cref="SmsMessage"/>.
        /// </returns>
        public static SmsMessage Factoy(string phone, string template, string param)
        {
            var message = new SmsMessage();
            var req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = string.Empty;
            req.SmsType = "normal";
            req.SmsFreeSignName = "搜猎人";
            req.SmsParam = param;
            req.RecNum = phone;
            req.SmsTemplateCode = template;
            message.Req = req;
            return message;
        }

        private static List<TemplateInfo> Templates;

        public static List<TemplateInfo> GetTemplate()
        {
            if (Templates == null)
            {
                Templates = new List<TemplateInfo>();
                LoadEventSettings();
            }

            return Templates;
        }

        /// <summary>
        /// The load event settings.
        /// </summary>
        private static void LoadEventSettings()
        {
            var xmlSettings = new System.Xml.XmlDocument();
            try
            {
                xmlSettings.Load("settings.xml");
                var events = xmlSettings.SelectSingleNode("/settings/template");
                foreach (XmlNode _event in events.ChildNodes)
                {
                    var item = new TemplateInfo();
                    item.Id = _event.Attributes["id"].Value;
                    item.Name = _event.Attributes["name"].Value;
                    item.Content = _event.Attributes["content"].Value;
                    Templates.Add(item);
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Log.TraceManager.Error.Write("LoadEventSettings", ex);
            }
        }

        #endregion
    }

    public class TemplateInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}