// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmsClient.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AutoServiceApp
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
                TraceManager.Info.Write("Send", rsp.Body);
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