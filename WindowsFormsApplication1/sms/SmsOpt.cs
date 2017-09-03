// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SMSOpt.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:32:55 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:32:55
//      修改描述：新建 SMSOpt.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Forms;

namespace SendMSM
{
    using System.Collections.Generic;

    using Infrastructure.Log;

    /// <summary>
    ///     The sms opt.
    /// </summary>
    public class SmsOpt
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly SmsOpt instance = new SmsOpt();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static SmsOpt Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The send.
        /// </summary>
        public void Send(List<ContractEntity> contractList, string templateId, string paras, ListBox listBox)
        {
            TraceManager.Info.Write("phone send ", "send start");
            var client = new SmsClient();
            string resultBody = string.Empty;
            foreach (var sms in contractList)
            {
                TraceManager.Debug.Write("sms send", string.Format("send sms phone:{0}", sms.Phone));
                string phone = sms.Phone;
                string template = templateId;
                string param = paras;
                bool result = client.Send(SmsMessage.Factoy(phone, template, param), out resultBody);
                if (result)
                {
                    listBox.Items.Add(sms.Phone + " 发送成功");
                }
                else
                {
                    listBox.Items.Add(sms.Phone + " 发送失败");
                }
            }

            TraceManager.Info.Write("phone send ", "send end");
        }
        #endregion
    }
}