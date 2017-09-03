// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MySQLExtention.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date>2014/9/30 13:47:18</date>
// <modify>
//   修改人：zoukp
//   修改时间：2014/9/30 13:47:18
//   修改描述：新建 MSSQLExtention.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>
// <summary>
//   The my sql extention.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoService
{
    using System;

    /// <summary>
    ///     The my sql extention.
    /// </summary>
    public class MySqlExtention
    {
        #region Static Fields

        /// <summary>
        ///     The get email list sql text.
        /// </summary>
        public static string GetEmailListSqlText = "SELECT * FROM sou_sendemail WHERE `Status`<>1 AND `Trytime`<5;";

        /// <summary>
        ///     The get orders sql text.
        /// </summary>
        public static string GetOrdersSqlText =
            "select id,`status`,create_time from sou_order where `status`=0;";

        /// <summary>
        /// The get send phone sql text.
        /// </summary>
        public static string GetSendPhoneSqlText = "SELECT * from sou_send_phone WHERE `Status` <> 1 AND try_time<6;";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get send email fail sql text.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSendEmailFailSqlText(int id)
        {
            return string.Format("UPDATE sou_sendemail SET `Status` = 2,Trytime = Trytime + 1 where Id = {0};", id);
        }

        /// <summary>
        /// The get send email succeed sql text.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSendEmailSucceedSqlText(int id)
        {
            return string.Format("UPDATE sou_sendemail SET `Status` = 1 where Id = {0};", id);
        }

        /// <summary>
        /// The get update order sql text.
        /// </summary>
        /// <param name="orderId">
        /// The order id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUpdateOrderSqlText(string orderId)
        {
            return string.Format("update sou_order SET status = 5 WHERE id = {0};", orderId);
        }

        /// <summary>
        /// The get update phone sql text.
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
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUpdatePhoneSqlText(int id, bool result, string msg)
        {
            string sql;
            if (result)
            {
                sql =
                    string.Format(
                        "UPDATE sou_send_phone SET `STATUS`=1,last_update_time='{1}',result=1,result_body='Succeed' WHERE id = {0};", 
                        id, 
                        DateTime.Now);
            }
            else
            {
                sql =
                    string.Format(
                        "UPDATE sou_send_phone SET `STATUS`=3,last_update_time='{2}',result=0,result_body=@'{1}',try_time=try_time+1 WHERE id = {0};", 
                        id, 
                        msg, 
                        DateTime.Now);
            }

            return sql;
        }

        #endregion
    }
}