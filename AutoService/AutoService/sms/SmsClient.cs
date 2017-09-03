// <copyright file="SmsClient.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:41:33 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:41:33
//      修改描述：新建 SmsClient.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace AutoService
{
    using System;

    using Infrastructure.Log;

    using Top.Api;
    using Top.Api.Response;

    /// <summary>
    /// The sms client.
    /// </summary>
    public class SmsClient
    {
        #region Constants

        /// <summary>
        /// The token.
        /// </summary>
        private const string smsCecret = "7c055c99f4a8716be42be7c498bd3765";

        /// <summary>
        /// The url.
        /// </summary>
        private const string url = @"http://gw.api.taobao.com/router/rest";

        /// <summary>
        /// The user.
        /// </summary>
        private const string user = "23461132";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="resultBody">
        /// The result body.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Send(SmsMessage msg, out string resultBody)
        {
            try
            {
                ITopClient client = new DefaultTopClient(url, user, smsCecret);
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(msg.Req);
                TraceManager.Debug.Write("Sms Send", rsp.Body);
                resultBody = rsp.Body;
                return true;
            }
            catch (Exception ex)
            {
                TraceManager.Error.Write("sms Send", ex);
                resultBody = ex.Message;
                return false;
            }
        }

        #endregion
    }
}