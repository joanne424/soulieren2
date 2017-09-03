// <copyright file="LoadingControl.xaml.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> wangmy </author>
// <date> 2015/11/19 20:23:50 </date>
// <modify>
//   修改人：wangmy
//   修改时间：2015/11/19 20:23:50
//   修改描述：新建 LoadingControl
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Banclogix.Controls
{
    /// <summary>
    /// LoadingControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingControl : UserControl
    {
        #region 常量
        /// <summary>
        /// 派
        /// </summary>
        private const double Offset = Math.PI;

        /// <summary>
        /// 步长
        /// </summary>
        private const double Step = Math.PI * 2 / 10.0;
        #endregion

        #region 静态字段
        /// <summary>
        /// Canvas旋转Timer
        /// </summary>
        private readonly DispatcherTimer loadingTimer;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingControl"/> class.
        /// </summary>
        public LoadingControl()
        {
            InitializeComponent();

            this.loadingTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
            this.loadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 120);
        }

        #region 属性
        /// <summary>
        /// 设置中心显示消息
        /// </summary>
        public string Text
        {
            get
            {
                return this.tbLoading.Text;
            }

            set
            {
                this.tbLoading.Text = value;
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// UserControl显示变化事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">参数</param>
        private void LoadingControl_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ExecuteDispatcher();
        }

        /// <summary>
        /// UserControl加载完成事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">参数</param>
        private void LoadingControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            // 设置各个圆形的位置
            this.SetPosition(this.Circle0, 0.0);
            this.SetPosition(this.Circle1, 1.0);
            this.SetPosition(this.Circle2, 2.0);
            this.SetPosition(this.Circle3, 3.0);
            this.SetPosition(this.Circle4, 4.0);
            this.SetPosition(this.Circle5, 5.0);
            this.SetPosition(this.Circle6, 6.0);
            this.SetPosition(this.Circle7, 7.0);
            this.SetPosition(this.Circle8, 8.0);
            this.SetPosition(this.Circle9, 9.0);

            // 根据当前系统状态启动或停止
            this.ExecuteDispatcher();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 根据显示状态启动停止Loading
        /// </summary>
        private void ExecuteDispatcher()
        {
            switch (this.Visibility)
            {
                case Visibility.Visible:
                    this.Start();
                    break;
                default:
                    this.Stop();
                    break;
            }
        }

        /// <summary>
        /// 停止Timer
        /// </summary>
        private void Stop()
        {
            if (this.loadingTimer.IsEnabled)
            {
                this.loadingTimer.Stop();
                this.loadingTimer.Tick -= this.LoadingTimerTick;
            }
        }

        /// <summary>
        /// 开启Timer
        /// </summary>
        private void Start()
        {
            if (!this.loadingTimer.IsEnabled)
            {
                this.loadingTimer.Tick += this.LoadingTimerTick;
                this.loadingTimer.Start();
            }
        }

        /// <summary>
        /// Timer tick响应
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">参数</param>
        private void LoadingTimerTick(object sender, EventArgs e)
        {
            // 每次旋转1/10圈
            this.SpinRotate.Angle = (SpinRotate.Angle + 36) % 360;
        }

        /// <summary>
        /// 设置圆位置
        /// </summary>
        /// <param name="ellipse">圆形实例</param>
        /// <param name="offset">偏移</param>
        /// <param name="posOffSet">偏移量</param>
        /// <param name="step">步长</param>
        private void SetPosition(Ellipse ellipse, double posOffSet)
        {
            // 设置圆形左侧距离
            ellipse.SetValue(Canvas.LeftProperty, 30.0 + Math.Sin(Offset + posOffSet * Step) * 30.0);

            // 设置圆形上侧距离
            ellipse.SetValue(Canvas.TopProperty, 30.0 + Math.Cos(Offset + posOffSet * Step) * 30.0);
        }
        #endregion
    }
}
