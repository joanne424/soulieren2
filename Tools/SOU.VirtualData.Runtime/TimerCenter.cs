// <copyright file="TimerCenter.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2015/11/16 05:31:04 </date>
// <summary> 时间中心 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2015/11/16 05:31:04
//      修改描述：新建 TimerCenter.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Manager.Models
{
    #region

    using System;
    using System.Diagnostics;
    using System.Threading;

    using Infrastructure.Data;
    using Infrastructure.Service;

    #endregion

    /// <summary>
    /// The timer center.
    /// </summary>
    public class TimerCenter : BaseModel
    {
        #region Constants

        /// <summary>
        /// 刷新时间间隔
        /// </summary>
        private const int RefreshInterval = 60 * 1000;

        #endregion

        #region Fields

        /// <summary>
        /// 是否允许
        /// </summary>
        private bool isRun;

        /// <summary>
        /// 上次获取到的系统时间
        /// </summary>
        private DateTime lastGetSystemTime;

        /// <summary>
        /// 时间刷新定时器
        /// </summary>
        private Timer refreshTimer;

        /// <summary>
        /// 同步标识
        /// </summary>
        private object synacRoot = new object();

        /// <summary>
        /// 时间计时器，用于两次刷新间隔间的对外时间提供
        /// </summary>
        private Stopwatch tiemrWathc;

        /// <summary>
        /// 时间服务
        /// </summary>
        private TimerService timerService;

        /// <summary>
        /// bu仓储
        /// </summary>
        private IBusinessUnitCacheRepository burep;

        /// <summary>
        /// 所属运行时
        /// </summary>
        private RunTime belongRuntime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerCenter"/> class.
        /// </summary>
        /// <param name="varRuntime">
        /// 所属运行时
        /// </param>
        /// <param name="varOwnerId">
        /// The var owner id.
        /// </param>
        public TimerCenter(RunTime varRuntime, string varOwnerId)
            : base(varOwnerId)
        {
            this.belongRuntime = varRuntime;
            this.burep = this.GetRepository<IBusinessUnitCacheRepository>();
            this.refreshTimer = new System.Threading.Timer(
                this.RefreshTimerAction,
                null,
                Timeout.Infinite,
                Timeout.Infinite);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 获取Bu的当前时间
        /// </summary>
        /// <param name="buid">
        /// 目的Bu的编号
        /// </param>
        /// <returns>
        /// 目标时间
        /// </returns>
        public DateTime GetCurrentTimeForBu(string buid)
        {
            var bu = this.burep.FindByID(buid);
            if (bu == null)
            {
                Infrastructure.Log.TraceManager.Error.Write("TimeCenter", "GetCurrentTimeForBu时，找不到对应的Bu，Buid:{0}", buid);
                return this.lastGetSystemTime + this.tiemrWathc.Elapsed;
            }

            var gmtNow = this.GetSystemGmtTime();

            return gmtNow.AddHours((int)bu.GetBuTimeZoneSavingsByGmtTime(gmtNow));
        }

        /// <summary>
        /// 获取当前登录员工所在Bu的当前时间
        /// </summary>
        /// <returns>
        /// 目标时间
        /// </returns>
        public DateTime GetCurrentTimeForCurrentStaffBu()
        {
            return this.GetSystemGmtTime().AddHours(RunTime.BusUnitTimeZone);
        }

        /// <summary>
        /// 获取系统GMT时间
        /// </summary>
        /// <returns>
        /// 目标时间
        /// </returns>
        public DateTime GetSystemGmtTime()
        {
            lock (this.synacRoot)
            {
                return this.lastGetSystemTime + this.tiemrWathc.Elapsed;
            }
        }

        /// <summary>
        /// 获取GMT时间对应指定BU的时间
        /// </summary>
        /// <param name="sourceGmtTime">
        /// 源时间
        /// </param>
        /// <param name="buid">
        /// 指定BU的Id
        /// </param>
        /// <returns>
        /// 目标时间
        /// </returns>
        public DateTime GetTimeForBu(DateTime sourceGmtTime, string buid)
        {
            var bu = this.burep.FindByID(buid);
            if (bu == null)
            {
                Infrastructure.Log.TraceManager.Error.Write("TimeCenter", "GetTimeForBu时，找不到对应的Bu，Buid:{0}", buid);
                return sourceGmtTime;
            }

            return sourceGmtTime.AddHours((int)bu.GetBuTimeZoneSavingsByGmtTime(sourceGmtTime));
        }

        /// <summary>
        /// 获取Local时间对应指定GMT的时间,包含夏令时
        /// </summary>
        /// <param name="sourceGmtTime">
        /// 源时间
        /// </param>
        /// <param name="buid">
        /// 指定BU的Id
        /// </param>
        /// <returns>
        /// 目标时间
        /// </returns>
        public DateTime GetGMTTimeForBu(DateTime sourceLocalTime, string buid)
        {
            var bu = this.burep.FindByID(buid);
            if (bu == null)
            {
                Infrastructure.Log.TraceManager.Error.Write("TimeCenter", "GetTimeForBu时，找不到对应的Bu，Buid:{0}", buid);
                return sourceLocalTime;
            }

            return sourceLocalTime.AddHours(-(int)bu.GetBuTimeZoneSavingsByGmtTime(sourceLocalTime));
        }

        /// <summary>
        /// 获取GMT时间对应当前登录员工所属BU的时间
        /// </summary>
        /// <param name="sourceGmtTime">
        /// 源时间
        /// </param>
        /// <returns>
        /// 目标时间
        /// </returns>
        public DateTime GetTimeForCurrentStaffBu(DateTime sourceGmtTime)
        {
            if (sourceGmtTime == DateTime.MinValue)
            {
                return sourceGmtTime;
            }

            return this.GetTimeForBu(sourceGmtTime, RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo.BusinessUnitID);
        }

        /// <summary>
        /// 获取当前登录员工所属BU的时间对应的GMT时间
        /// </summary>
        /// <param name="sourceGmtTime">
        /// BU时间
        /// </param>
        /// <returns>
        /// Gmt时间
        /// </returns>
        public DateTime GetGmtTimeFromCurrentStaffBu(DateTime sourceGmtTime)
        {
            return sourceGmtTime.Subtract(TimeSpan.FromHours(RunTime.BusUnitTimeZone));
        }

        /// <summary>
        /// 启动时间的后台同步
        /// </summary>
        public void RunTimerRefresh()
        {
            lock (this.synacRoot)
            {
                if (!this.isRun)
                {
                    this.tiemrWathc = new Stopwatch();
                    this.timerService = this.GetSevice<TimerService>();
                    this.RefreshTimerAction(null);
                    this.isRun = true;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 时间刷新动作
        /// </summary>
        /// <param name="state">
        /// 状态参数
        /// </param>
        private void RefreshTimerAction(object state)
        {
            DateTime targetTime;
            if (this.timerService.GetSystemGmtTime(out targetTime))
            {
                lock (this.synacRoot)
                {
                    this.tiemrWathc.Restart();
                    this.lastGetSystemTime = targetTime;
                }
            }
            else
            {
                // 用于首次时间获取即发生错误
                if (!this.isRun)
                {
                    lock (this.synacRoot)
                    {
                        this.tiemrWathc.Restart();
                        this.lastGetSystemTime = DateTime.UtcNow;
                    }
                }
            }

            this.refreshTimer.Change(RefreshInterval, Timeout.Infinite);
        }

        #endregion
    }
}