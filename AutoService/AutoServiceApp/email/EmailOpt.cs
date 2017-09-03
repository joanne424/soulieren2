// <copyright file="EmailOpt.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:32:31 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:32:31
//      修改描述：新建 EmailOpt.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace AutoServiceApp
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Infrastructure.Log;

    /// <summary>
    /// The email opt.
    /// </summary>
    public class EmailOpt
    {
        private static EmailOpt instance = new EmailOpt();

        public static EmailOpt Instance
        {
            get
            {
                return instance;
            }
        }

        #region Public Methods and Operators

        /// <summary>
        /// The send email.
        /// </summary>
        public void SendEmail()
        {
            List<MailInfoEntity> emails =
                MySqlDbHelper.QueryList<MailInfoEntity>(
                    MySqlDbHelper.GetConnection(ConfigParameter.Instance.mySqlConnectionStr),
                    MySQLExtention.GetEmailListSqlText);
            TraceManager.Debug.WriteAdditional("sendemail", emails, "需要发送的email");
            foreach (MailInfoEntity mailInfoEntity in emails)
            {
                Message messge = Message.Factory(
                    mailInfoEntity.Subject,
                    Encoding.GetEncoding(mailInfoEntity.SubjectEncoding),
                    mailInfoEntity.Body,
                    Encoding.GetEncoding(mailInfoEntity.BodyEncoding),
                    mailInfoEntity.IsHtml,
                    mailInfoEntity.Priority,
                    mailInfoEntity.ToAddress);
                TraceManager.Debug.WriteAdditional("SendEmail", messge.GetMailMsg(), "需要发送的内容");
                this.UpdateSendMail(EmailClient.Instance.Send(messge.GetMailMsg()), mailInfoEntity.Id);
                TraceManager.Debug.Write("SendEmail", string.Format("发送 {0} 结束", mailInfoEntity.ToAddress));
            }

            TraceManager.Debug.Write("sendemail", "发送邮件结束");
        }

        #endregion

        #region Methods

        /// <summary>
        /// The update send mail.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        private void UpdateSendMail(bool result, int id)
        {
            TraceManager.Debug.Write("SendEmail", "开始更新数据");
            string sqlText = string.Empty;
            if (result)
            {
                sqlText = MySQLExtention.GetSendEmailSucceedSqlText(id);
            }
            else
            {
                sqlText = MySQLExtention.GetSendEmailFailSqlText(id);
            }

            TraceManager.Debug.Write("SendEmail", string.Format("更新语句：{0}", sqlText));
            try
            {
                MySqlDbHelper.ExecuteSql(
                    MySqlDbHelper.GetConnection(ConfigParameter.Instance.mySqlConnectionStr),
                    sqlText);
                TraceManager.Debug.Write("SendEmail", "执行成功");
            }
            catch (Exception ex)
            {
                TraceManager.Error.Write("UpdateSendMail", ex);
            }
        }

        #endregion
    }
}