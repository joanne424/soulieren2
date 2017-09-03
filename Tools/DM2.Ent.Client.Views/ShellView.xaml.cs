// <copyright file="ShellView.xaml.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2017/05/27 05:56:55 </date>
// <summary> 主界面后台交互逻辑 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2017/05/27 05:56:55
//      修改描述：新建 ShellView.xaml.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Ent.Client.Views
{
    #region

    using System;
    using System.Windows;
    using System.Windows.Input;

    using DM2.Ent.Client.Views.ExtendClass;

    #endregion

    /// <summary>
    ///     ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView : Window
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShellView" /> class.
        /// </summary>
        public ShellView()
        {
            try
            {
                this.InitializeComponent();
                FullScreenManager.RepairWpfWindowFullScreenBehavior(this);

                // twChartWindow.CurrentStyleColor = shellViewModel.CurrentChartStyle;
                //// this.twChartWindow.Activate();
                // this.twChartWindow.StockChartX_ChartLoaded(string.Empty, new EventArgs());

                /*
                ShellViewModel shellViewModel = this.DataContext as ShellViewModel;
                string symbol = "USDJPY";
                List<string> symbolList = new List<string>() { "USDJPY", "USDCAD", "EURUSD" };
                ChartWindow chart = new ChartWindow();
                chart.Symbol = symbol;
                chart.HistoryPriceCycle = HistoryPriceCycle.M1;
                chart.stockChartX.SymbolList = symbolList;
                chart.Tag = Guid.NewGuid().ToString();
                chart.Title = symbol + "," + HistoryPriceCycle.M1.ToString();
                chart.CanRaft = true;
                chart.CanClose = true;

                chart.HistoryUseType = HistoryPriceModel.HistoryUseType.Bid;
             
                chart.ShellViewModel = shellViewModel;
                dockSiteChart.DocumentWindows.Add(chart);
                chart.CurrentStyleColor = new StyleColor();
                chart.Activate();
*/
                // this.InitChartWindow();
                // Create layout serializer
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 左键按下拖动逻辑
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MouseLeftDragMove(object sender, MouseButtonEventArgs e)
        {
            var win = (Window)((FrameworkElement)sender).TemplatedParent;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                win.DragMove();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The init chart window.
        /// </summary>
        private void InitChartWindow()
        {
        }

        /// <summary>
        /// 记录性能信息是否可以执行
        /// </summary>
        /// <param name="sender">
        /// 命令发出者
        /// </param>
        /// <param name="e">
        /// 事件
        /// </param>
        private void LogPerformanceInfoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
    }
}