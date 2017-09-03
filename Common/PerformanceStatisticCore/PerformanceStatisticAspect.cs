// <copyright file="PerformanceStatisticAspect.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/09/26 12:42:17 </date>
// <summary> 性能统计切面 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/09/26 12:42:17
//      修改描述：新建 PerformanceStatisticAspect.cs
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
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using Infrastructure.Utils;
    #endregion

    /// <summary>
    /// 性能统计切面
    /// </summary>
    [Serializable]
    public class PerformanceStatisticAspect : OnMethodBoundaryAspect
    {
        #region Public Methods and Operators

        /// <summary>
        /// 添加切面的校验规则
        /// </summary>
        /// <param name="method">
        /// 目标方法
        /// </param>
        /// <returns>
        /// 是否对目标方法添加切面 <see cref="bool"/>.
        /// </returns>
        public override bool CompileTimeValidate(MethodBase method)
        {
            // 属性自动生成的get, set方法，如果没有特殊标识，则不统计
            if (method.Name.Contains("get_") || method.Name.Contains("set_"))
            {
                if (method.GetCustomAttributes(typeof(PerformanceStatisticAttribute)).Any())
                {
                    return true;
                }

                return false;
            }

            if (method.DeclaringType.GetCustomAttributes(typeof(NonPerformanceStatisticAttribute)).Any())
            {
                return false;
            }

            if (method.DeclaringType.GetCustomAttributes(typeof(PerformanceStatisticAttribute)).Any())
            {
                return true;
            }

            // 只统计公共方法
            if (method.IsPublic)
            {
                if (method.GetCustomAttributes(typeof(NonPerformanceStatisticAttribute)).Any())
                {
                    return false;
                }

                return true;
            }

            if (method.GetCustomAttributes(typeof(PerformanceStatisticAttribute)).Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 函数进入
        /// </summary>
        /// <param name="args">
        /// 函数参数
        /// </param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = new PerformanceInfo();
        }

        /// <summary>
        /// 函数异常
        /// </summary>
        /// <param name="args">
        /// 函数参数
        /// </param>
        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Method.GetCustomAttributes(typeof(NonExceptionCtrlAttribute)).Any())
            {
                args.FlowBehavior = FlowBehavior.RethrowException;
                return;
            }

            Infrastructure.Log.TraceManager.Error.Write(
                "Exception", 
                args.Exception, 
                "Exception class:{0}, Exception Method:{1}", 
                args.Method.ReflectedType == null ? "NonClass" : args.Method.Name, 
                args.Method.Name);
            MessageBox.Show("Unknown Exception:" + args.Exception.Message);
            args.FlowBehavior = FlowBehavior.Continue;
        }

        /// <summary>
        /// 函数进入
        /// </summary>
        /// <param name="args">
        /// 函数参数
        /// </param>
        public override void OnExit(MethodExecutionArgs args)
        {
            var info = args.MethodExecutionTag as PerformanceInfo;
            if (info == null)
            {
                return;
            }

            info.StopTick = DateTime.UtcNow.Ticks;
            if (info.Watch != null)
            {
                info.Watch.Stop();
            }

            string methodname = null;
            if (args.Method.ReflectedType != null)
            {
                methodname = args.Method.ReflectedType.Name + "." + args.Method.Name;
            }
            else
            {
                methodname = "NonClass" + "." + args.Method.Name;
            }

            info.StaticToPerformanceCore(methodname);
        }

        #endregion

        /// <summary>
        /// 性能信息
        /// 此信息封装的意义是为了可以进行空判断
        /// 同时为以后的扩展提供方便
        /// 计时不使用Stopwatch是因为Stopwatch使用pinvoke进行了调用，有比较大的性能损耗
        /// </summary>
        private class PerformanceInfo
        {
            /// <summary>
            /// 性能统计是否启用精确度更高但是性能损失较大的StopWatch
            /// </summary>
            public static readonly bool IsAccurateStatistics = ConfigurationManager.AppSettings["IsAccurateStatistics"] != null
                                       && bool.Parse(ConfigurationManager.AppSettings["IsAccurateStatistics"]);

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="PerformanceInfo"/> class.
            /// </summary>
            public PerformanceInfo()
            {
                this.StartTick = DateTime.UtcNow.Ticks;
                if (IsAccurateStatistics)
                {
                    this.Watch = new Stopwatch();
                    this.Watch.Start();
                }
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// 起始Tick
            /// </summary>
            public long StartTick { get; set; }

            /// <summary>
            /// 结束Tick
            /// </summary>
            public long StopTick { get; set; }

            /// <summary>
            /// 在精确计数状态下使用的秒表
            /// </summary>
            public Stopwatch Watch { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// 获取总的耗时，单位毫秒
            /// </summary>
            /// <returns>总的耗时</returns>
            public double GetTotalMilliseconds()
            {
                long diff = this.StopTick - this.StartTick;
                if (diff < 0)
                {
                    diff = 0;
                }

                return diff / 10000.0;
            }

            /// <summary>
            /// 将统计添加进性能统计中心
            /// </summary>
            /// <param name="methodname">
            /// 函数名称
            /// </param>
            public void StaticToPerformanceCore(string methodname)
            {
                if (IsAccurateStatistics)
                {
                    PerformanceCore.Instance.AddMethordStaticInfo(methodname, this.Watch.Elapsed.TotalMilliseconds, new DateTime(this.StartTick));
                }
                else
                {
                    PerformanceCore.Instance.AddMethordStaticInfo(methodname, this.GetTotalMilliseconds(), new DateTime(this.StartTick));
                }
            }

            #endregion
        }
    }
}