// <copyright file="MethodPerformanceItem.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/09/26 12:32:25 </date>
// <summary> 函数性能统计项 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/09/26 12:32:25
//      修改描述：新建 MethodPerformanceItem.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace PerformanceStatisticCore
{
    #region

    using System;
    using System.Text;

    #endregion

    /// <summary>
    /// 函数性能统计项
    /// </summary>
    public class MethodPerformanceItem
    {
        #region Constants

        /// <summary>
        /// 分割符
        /// </summary>
        private const string spiltStr = ",";

        #endregion

        #region Fields

        /// <summary>
        /// 平均执行时间
        /// </summary>
        private double averageConsumerTime;

        /// <summary>
        /// 展现层中执行次数
        /// </summary>
        private long callInPresentationCount;

        /// <summary>
        /// 最大执行时间
        /// </summary>
        private double maxCounsumerTime;

        /// <summary>
        /// 最小执行时间
        /// </summary>
        private double minCounsumerTime = double.MaxValue;

        /// <summary>
        /// 最后一次开始执行时间
        /// </summary>
        private DateTime latestActionTime;

        /// <summary>
        /// 同步锁标识
        /// </summary>
        private object syncRoot = new object();

        /// <summary>
        /// 总执行此数
        /// </summary>
        private long totallCallCount;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodPerformanceItem"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public MethodPerformanceItem(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 函数名称
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get header.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetHeader()
        {
            var builder = new StringBuilder();
            builder.Append("函数名称");
            builder.Append(spiltStr);
            builder.Append("平均执行时间");
            builder.Append(spiltStr);
            builder.Append("总执行次数");
            builder.Append(spiltStr);
            builder.Append("界面线程执行次数");
            builder.Append(spiltStr);
            builder.Append("最大执行时间");
            builder.Append(spiltStr);
            builder.Append("最小执行时间");
            builder.Append(spiltStr);
            builder.Append("最后一次开始执行时间");
            builder.Append(spiltStr);
            return builder.ToString();
        }

        /// <summary>
        /// The add excute info.
        /// </summary>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <param name="varLatestActionTime">
        /// The var Latest Action Time.
        /// </param>
        public void AddExcuteInfo(double time, DateTime varLatestActionTime)
        {
            lock (this.syncRoot)
            {
                this.totallCallCount ++;
                if (PerformanceCore.Instance.IsInPresentationThread())
                {
                    this.callInPresentationCount ++;
                }

                this.averageConsumerTime += (time - this.averageConsumerTime) / this.totallCallCount;
                if (time > this.maxCounsumerTime)
                {
                    this.maxCounsumerTime = time;
                }

                if (time < this.minCounsumerTime)
                {
                    this.minCounsumerTime = time;
                }

                this.latestActionTime = varLatestActionTime;
            }
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(this.Name.Replace(",", ":"));
            builder.Append(spiltStr);
            builder.Append(this.averageConsumerTime);
            builder.Append(spiltStr);
            builder.Append(this.totallCallCount);
            builder.Append(spiltStr);
            builder.Append(this.callInPresentationCount);
            builder.Append(spiltStr);
            builder.Append(this.maxCounsumerTime);
            builder.Append(spiltStr);
            builder.Append(this.minCounsumerTime);
            builder.Append(spiltStr);
            builder.Append("'");
            builder.Append(this.latestActionTime.ToString(
                "yyyy-MM-dd HH:mm:ss.fff",
                System.Globalization.CultureInfo.InvariantCulture));
            builder.Append("'");
            builder.Append(spiltStr);
            return builder.ToString();
        }

        #endregion
    }
}