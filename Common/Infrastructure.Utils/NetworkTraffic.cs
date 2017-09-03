// <copyright file="NetworkTraffic.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2015/12/23 12:02:18 </date>
// <summary> 网络流量统计 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2015/12/23 12:02:18
//      修改描述：新建 NetworkTraffic.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace Infrastructure.Utils
{
    #region

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    using Infrastructure.Common;

    #endregion

    /// <summary>
    /// 网络流量统计
    /// </summary>
    public class NetworkTraffic
    {
        #region Static Fields

        /// <summary>
        /// 静态只读唯一实例
        /// </summary>
        private static readonly NetworkTraffic StaticInstance = new NetworkTraffic();

        #endregion

        #region Fields

        /// <summary>
        /// 当前进程Id
        /// </summary>
        private readonly int currentProcessId;

        /// <summary>
        /// IP地址
        /// </summary>
        private readonly string ipadress;

        /// <summary>
        /// 接受数据性能计数器
        /// </summary>
        private PerformanceCounter bytesReceivedPerformanceCounter;

        /// <summary>
        /// 发送数据性能计数器
        /// </summary>
        private PerformanceCounter bytesSentPerformanceCounter;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool isInitial;

        /// <summary>
        /// 上次获取统计信息的时间
        /// </summary>
        private DateTime lastReceiveTime;

        /// <summary>
        /// 上次获取到的总的接受Kb
        /// </summary>
        private double lastTotalReceiveKBytes;

        /// <summary>
        /// 上次获取到的总的发送Kb
        /// </summary>
        private double lastTotalSendKBytes;

        /// <summary>
        /// 线程同步锁对象
        /// </summary>
        private object syncLockFlag;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="NetworkTraffic"/> class from being created.
        /// </summary>
        private NetworkTraffic()
        {
            this.syncLockFlag = new object();
            this.ipadress = this.GetAddressIp();
            this.currentProcessId = Process.GetCurrentProcess().Id;
            this.TryToInitializeCounters();
            this.lastReceiveTime = DateTime.MinValue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 单例获取
        /// </summary>
        public static NetworkTraffic Instance
        {
            get
            {
                return StaticInstance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 获取网络的统计信息
        /// </summary>
        /// <returns>
        /// The <see cref="NetworkInfo"/>.
        /// </returns>
        public NetworkInfo GetNetworkPerformanceInfo()
        {
            try
            {
                lock (this.syncLockFlag)
                {
                    this.TryToInitializeCounters();
                    if (!this.isInitial)
                    {
                        return null;
                    }

                    var info = new NetworkInfo();
                    info.Ip = this.ipadress;
                    double bytesReceive = this.bytesReceivedPerformanceCounter.RawValue / 1024.0;
                    double bytesSent = this.bytesSentPerformanceCounter.RawValue / 1024.0;
                    info.TotalKBytesReceive = (decimal)bytesReceive;
                    info.TotalKBytesSend = (decimal)bytesSent;
                    info.CurrentReceiveRate = 0;
                    info.CurrentSendRate = 0;
                    if (this.lastReceiveTime != DateTime.MinValue)
                    {
                        info.CurrentReceiveRate =
                            (decimal)
                            ((bytesReceive - this.lastTotalReceiveKBytes)
                             / (DateTime.Now - this.lastReceiveTime).TotalSeconds);
                        info.CurrentSendRate =
                            (decimal)
                            ((bytesSent - this.lastTotalSendKBytes) / (DateTime.Now - this.lastReceiveTime).TotalSeconds);
                    }

                    this.lastTotalReceiveKBytes = bytesReceive;
                    this.lastTotalSendKBytes = bytesSent;
                    this.lastReceiveTime = DateTime.Now;
                    return info;
                }
            }
            catch (Exception exception)
            {
                Log.TraceManager.Error.Write("NetworkTraffic", exception, "获取网络的统计信息出现异常。");
                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取本机的IP地址
        /// </summary>
        /// <returns>本机IP地址</returns>
        private string GetAddressIp()
        {
            // 获取本地的IP地址
            string addressIp = string.Empty;
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    addressIp = address.ToString();
                }
            }

            return addressIp;
        }

        /// <summary>
        /// The try to initialize counters.
        /// </summary>
        private void TryToInitializeCounters()
        {
            if (!this.isInitial)
            {
                var category = new PerformanceCounterCategory(".NET CLR Networking 4.0.0.0");
                var instanceNames = category.GetInstanceNames();
                var instanceName =
                    instanceNames.FirstOrDefault(i => i.Contains(string.Format("p{0}", this.currentProcessId)));

                if (!instanceName.IsNullOrSpace())
                {
                    this.bytesSentPerformanceCounter = new PerformanceCounter();
                    this.bytesSentPerformanceCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
                    this.bytesSentPerformanceCounter.CounterName = "Bytes Sent";
                    this.bytesSentPerformanceCounter.InstanceName = instanceName;
                    this.bytesSentPerformanceCounter.ReadOnly = true;

                    this.bytesReceivedPerformanceCounter = new PerformanceCounter();
                    this.bytesReceivedPerformanceCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
                    this.bytesReceivedPerformanceCounter.CounterName = "Bytes Received";
                    this.bytesReceivedPerformanceCounter.InstanceName = instanceName;
                    this.bytesReceivedPerformanceCounter.ReadOnly = true;

                    this.isInitial = true;
                }
                else
                {
                    Log.TraceManager.Warn.WriteAdditional(
                        "NetworkTraffic", 
                        instanceNames, 
                        "初始化网络性能统计失败，找不到当前的对应实例，当前的实例列表如下。此时系统的网络流量将无法统计。");
                }
            }
        }

        #endregion

        /// <summary>
        /// 网络统计信息
        /// </summary>
        public class NetworkInfo
        {
            #region Fields

            /// <summary>
            /// 当前接受速率Kb/s
            /// </summary>
            private decimal currentReceiveRate;

            /// <summary>
            /// 当前发送速率Kb/s
            /// </summary>
            private decimal currentSendRate;

            /// <summary>
            /// 总发送流量Kb
            /// </summary>
            private decimal totalKBytesReceive;

            /// <summary>
            /// 总发送流量Kb
            /// </summary>
            private decimal totalKBytesSend;

            #endregion

            #region Public Properties

            /// <summary>
            /// 当前接受速率Kb/s
            /// </summary>
            public decimal CurrentReceiveRate
            {
                get
                {
                    return this.currentReceiveRate;
                }

                set
                {
                    this.currentReceiveRate = value.ToFixed(2);
                }
            }

            /// <summary>
            /// 当前发送速率Kb/s
            /// </summary>
            public decimal CurrentSendRate
            {
                get
                {
                    return this.currentSendRate;
                }

                set
                {
                    this.currentSendRate = value.ToFixed(2);
                }
            }

            /// <summary>
            /// 本机Ip地址
            /// </summary>
            public string Ip { get; set; }

            /// <summary>
            /// 总发送流量Kb
            /// </summary>
            public decimal TotalKBytesReceive
            {
                get
                {
                    return this.totalKBytesReceive;
                }

                set
                {
                    this.totalKBytesReceive = value.ToFixed(2);
                }
            }

            /// <summary>
            /// 总发送流量Kb
            /// </summary>
            public decimal TotalKBytesSend
            {
                get
                {
                    return this.totalKBytesSend;
                }

                set
                {
                    this.totalKBytesSend = value.ToFixed(2);
                }
            }

            #endregion
        }
    }
}