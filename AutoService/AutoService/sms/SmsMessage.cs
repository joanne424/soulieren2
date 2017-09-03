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
namespace AutoService
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

        #endregion
    }
}