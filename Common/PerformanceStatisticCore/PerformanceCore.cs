namespace PerformanceStatisticCore
{
    #region

    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The performance core.
    /// </summary>
    public class PerformanceCore
    {
        #region Static Fields

        /// <summary>
        /// 延迟加载容器
        /// </summary>
        private static readonly Lazy<PerformanceCore> StoreLazy = new Lazy<PerformanceCore>(() => new PerformanceCore());

        #endregion

        #region Fields

        /// <summary>
        /// 是否启用性能检测
        /// </summary>
        private bool isEnablePerformanceStatistic;

        /// <summary>
        /// 统计数据集合
        /// </summary>
        private ConcurrentDictionary<string, MethodPerformanceItem> performanceItems =
            new ConcurrentDictionary<string, MethodPerformanceItem>();

        /// <summary>
        /// 界面线程的Id号
        /// </summary>
        private int presentationThreadId = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="PerformanceCore"/> class from being created.
        /// </summary>
        private PerformanceCore()
        {
            if ((bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)
            {
                this.isEnablePerformanceStatistic = false;
                return;
            }

            try
            {
                this.isEnablePerformanceStatistic =
                    bool.Parse(System.Configuration.ConfigurationManager.AppSettings["IsEnablePerformanceStatistic"]);
            }
            catch (Exception ex)
            {
                Infrastructure.Log.TraceManager.Error.Write("PerformanceCore", ex, "读取性能统计配置信息异常");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 单例获取
        /// </summary>
        public static PerformanceCore Instance
        {
            get
            {
                return StoreLazy.Value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 记录函数的监控数据
        /// </summary>
        /// <param name="methordName">
        /// 监控函数的名称
        /// </param>
        /// <param name="consumTime">
        /// 消耗的时间
        /// </param>
        /// <param name="lastActionTime">
        /// 最后执行时间
        /// </param>
        public void AddMethordStaticInfo(string methordName, double consumTime, DateTime lastActionTime)
        {
            if (!this.isEnablePerformanceStatistic)
            {
                return;
            }

            MethodPerformanceItem outItem;
            if (this.performanceItems.TryGetValue(methordName, out outItem))
            {
                outItem.AddExcuteInfo(consumTime, lastActionTime);
            }
            else
            {
                outItem = new MethodPerformanceItem(methordName);
                if (!this.performanceItems.TryAdd(methordName, outItem))
                {
                    this.performanceItems.TryGetValue(methordName, out outItem);
                }

                outItem.AddExcuteInfo(consumTime, lastActionTime);
            }
        }

        /// <summary>
        /// 获取性能统计信息
        /// </summary>
        /// <returns>性能统计信息</returns>
        private string GetPerformanceInfo()
        {
            var builder = new StringBuilder();

            builder.Append("统计函数总数：" + this.performanceItems.Count);
            builder.AppendLine();
            builder.AppendLine(MethodPerformanceItem.GetHeader());
            foreach (var methodPerformanceItem in this.performanceItems)
            {
                builder.AppendLine(methodPerformanceItem.Value.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// 在界面线程中初始化
        /// </summary>
        public void InitialInPerformanceThread()
        {
            this.presentationThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// 是否处于界面展示线程中
        /// </summary>
        /// <returns>
        /// 是否为界面线程 <see cref="bool"/>.
        /// </returns>
        public bool IsInPresentationThread()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId == this.presentationThreadId;
        }

        /// <summary>
        /// 保存性能信息
        /// </summary>
        /// <param name="fileHeadInfo">
        /// 保存文件命名前缀
        /// </param>
        public void SavePerformanceInfo(String fileHeadInfo)
        {
            var dialog = new SaveFileDialog();
            dialog.Title = "Plese select a location for the performanceinfo file";
            dialog.FileName = string.Format("{0}Performance_{1:yyyyMMddHHmmss}.csv", fileHeadInfo, DateTime.Now);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.FileName;
                var stream = new StreamWriter(path, false, Encoding.Default);
                stream.Write(this.GetPerformanceInfo());
                stream.Flush();
                stream.Close();
            }
        }

        #endregion
    }
}