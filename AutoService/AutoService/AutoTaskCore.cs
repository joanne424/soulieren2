// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTaskCore.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/06 04:20:46 </date>
// <summary>
//   The auto task core.
// </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/06 04:20:46
//      修改描述：新建 AutoTaskCore.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Infrastructure.Log;

    /// <summary>
    ///     The auto task core.
    /// </summary>
    public class AutoTaskCore
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly AutoTaskCore instance = new AutoTaskCore();

        #endregion

        #region Fields

        /// <summary>
        ///     The email thread.
        /// </summary>
        private Thread emailThread;

        /// <summary>
        ///     The msn thread.
        /// </summary>
        private Thread msnThread;

        /// <summary>
        ///     The remove thread.
        /// </summary>
        private Thread removeOrderThread;

        #endregion

        #region Public Properties

        /// <summary>
        ///     The instance.
        /// </summary>
        public static AutoTaskCore Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.emailThread != null)
                {
                    this.emailThread.Abort();
                }

                if (this.msnThread != null)
                {
                    this.msnThread.Abort();
                }

                if (this.removeOrderThread != null)
                {
                    this.removeOrderThread.Abort();
                }
            }
            catch (Exception ex)
            {
                TraceManager.Error.Write("Dispose", ex);
            }
        }

        /// <summary>
        ///     The run.
        /// </summary>
        public void Run()
        {
            this.emailThread = new Thread(this.SendEmail) { IsBackground = true };
            this.emailThread.Start();

            this.msnThread = new Thread(this.SendMSN) { IsBackground = true };
            this.msnThread.Start();

            this.removeOrderThread = new Thread(this.RemoveOrder) { IsBackground = true };
            this.removeOrderThread.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The remove order.
        /// </summary>
        private void RemoveOrder()
        {
            TraceManager.Info.Write("system", "开启移除过期订单");
            int removeOrderInterval = int.Parse(ConfigParameter.Instance.removeOrderInterval) * 1000;
            int orderExpired = int.Parse(ConfigParameter.Instance.orderExpired) * 1000;

            while (true)
            {
                List<OrderEntity> orders =
                    MySqlDbHelper.Query<OrderEntity>(
                        MySqlDbHelper.GetConnection(ConfigParameter.Instance.mySqlConnectionStr), 
                        MySqlExtention.GetOrdersSqlText);
                if (orders != null && orders.ToList().Any())
                {
                    foreach (OrderEntity orderEntity in orders)
                    {
                        TimeSpan span = DateTime.Now - orderEntity.create_time;
                        if (span.TotalSeconds > orderExpired)
                        {
                            try
                            {
                                MySqlDbHelper.ExecuteSql(
                                    MySqlDbHelper.GetConnection(ConfigParameter.Instance.mySqlConnectionStr), 
                                    MySqlExtention.GetUpdateOrderSqlText(orderEntity.id));
                                TraceManager.Debug.Write("RemoveOrder", "update order id:" + orderEntity.id);
                            }
                            catch (Exception ex)
                            {
                                TraceManager.Error.Write("update Order", ex);
                            }
                        }
                    }
                }

                Thread.Sleep(removeOrderInterval);
            }
        }

        /// <summary>
        ///     The send email.
        /// </summary>
        private void SendEmail()
        {
            TraceManager.Info.Write("system", "开启发送邮件");
            int interval = int.Parse(ConfigParameter.Instance.emailInterval) * 1000;
            while (true)
            {
                EmailOpt.Instance.SendEmail();
                Thread.Sleep(interval);
            }
        }

        /// <summary>
        ///     The send msn.
        /// </summary>
        private void SendMSN()
        {
            TraceManager.Info.Write("system", "开启发送短信");
            int interval = ConfigParameter.Instance.smsInternal * 1000;
            while (true)
            {
                SmsOpt.Instance.Send();
                Thread.Sleep(interval);
            }
        }

        #endregion
    }
}