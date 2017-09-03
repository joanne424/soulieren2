﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SMSOpt.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoServiceApp
{
    using System.Collections.Generic;
    using System.Text;

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
        public void Send()
        {
            var client = new SmsClient();
            string sql = MySQLExtention.GetSendPhoneSqlText;
            List<SmsEntity> smsList =
                MySqlDbHelper.QueryList<SmsEntity>(
                    MySqlDbHelper.GetConnection(ConfigParameter.Instance.mySqlConnectionStr), 
                    sql);
            string resultBody = string.Empty;
            foreach (SmsEntity sms in smsList)
            {
                string phone = sms.to_phone;
                string template = sms.template_id;
                string param = sms.param_list;
                bool result = client.Send(SmsMessage.Factoy(phone, template, param), out resultBody);
                this.UpdateSms(sms.id, result, resultBody);
            }

            TraceManager.Info.Write("phone send ", "send end");
        }

        /// <summary>
        /// The update sms.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="msg">
        /// The msg.
        /// </param>
        public void UpdateSms(int id, bool result, string msg)
        {
            string sql = MySQLExtention.GetUpdatePhoneSqlText(id, result, msg);
            bool exResult =
                MySqlDbHelper.ExecuteSql(MySqlDbHelper.GetConnection(ConfigParameter.Instance.mySqlConnectionStr), sql);
            TraceManager.Info.Write("UpdateSms", string.Format("update sms {0}", exResult));
        }

        #endregion
    }
}