// <copyright file="SessionHelper.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangrx</author>
// <date> 2016/07/13 14:49:37 </date>
// <summary></summary>
// <modify>
//      修改人：wangrx
//      修改时间：2016/07/13 14:49:37
//      修改描述：新建 SessionHelper
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Infrastructure.Utils
{
    /// <summary>
    /// Session验证助手
    /// </summary>
    public class SessionHelper
    {
        #region 静态实例

        /// <summary>
        /// Session助手
        /// </summary>
        public static SessionHelper Instance = new SessionHelper();

        #endregion

        #region 字段

        Timer timer;

        TimeSpan noTimeout = new TimeSpan(0, 0, 0);

        bool isHandSessionTimeout = false;

        #endregion

        private SessionHelper()
        {
            timer = new Timer(800);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        #region 属性

        TimeSpan idleSpan = new TimeSpan(0, 30, 0);
        /// <summary>
        /// 指定Session空闲多长时间过期
        /// </summary>
        public TimeSpan IdleSpan
        {
            get { return idleSpan; }
            private set { idleSpan = value; }
        }

        DateTime lastUpdateTime = DateTime.Now;
        /// <summary>
        /// 最后一次操作时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            private set { lastUpdateTime = value; }
        }

        #endregion

        #region 事件

        /// <summary>
        /// Session过期事件：
        /// 每当最后一次调用UpdateSession方法的时间距离现在超过或者等于IdleSpan时触发
        /// </summary>
        public event EventHandler SessionTimeout;

        /// <summary>
        /// 触发Session过期事件
        /// </summary>
        protected void OnSessionTimeout()
        {
            try
            {
                if (SessionTimeout != null)
                {
                    this.isHandSessionTimeout = true;
                    System.Windows.Application.Current.Dispatcher.Invoke(() => {
                        SessionTimeout(this, new EventArgs());
                    });
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("Sorry ! {0}.", ex.Message), "Error Message", System.Windows.MessageBoxButton.OK);
            }
        }

        #endregion

        #region 方法
        int reHandMax = 100000;
        
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IdleSpan == this.noTimeout)
            {
                return;
            }

            if (this.isHandSessionTimeout)
            {
                reHandMax--;
                if (reHandMax < 0)
                {
                    this.isHandSessionTimeout = false;
                    reHandMax = 100000;
                }
            }
            else
            {
                TimeSpan idleTimeFromLastUpdate = DateTime.Now - this.lastUpdateTime;
                if (idleTimeFromLastUpdate >= this.IdleSpan)
                {
                    OnSessionTimeout();
                }
            }
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="idleSpan"></param>
        public void SetIdleTime(TimeSpan idleSpan)
        {
            this.IdleSpan = idleSpan;
        }

        /// <summary>
        /// 更新Session
        /// </summary>
        public void UpdateSession()
        {
            LastUpdateTime = DateTime.Now;
        }

        #endregion
    }
}
