// <copyright file="DataPager.xaml.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-12-26 </date>
// <summary>分页控件的布局</summary>
// <modify>
//      修改人：donggj
//      修改时间：2013-12-26
//      修改描述：
//      版本：2.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// DataPager.xaml 的交互逻辑
    /// </summary>
    public partial class DataPager : UserControl, INotifyPropertyChanged
    {
        #region 字段

        /// <summary>
        /// Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new UIPropertyMetadata(10));

        /// <summary>
        /// Using a DependencyProperty as the backing store for Total.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(DataPager), new UIPropertyMetadata(0));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PageIndex.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new UIPropertyMetadata(1));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PageSizeList.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PageSizeListProperty = DependencyProperty.Register(
            "PageSizeList",
            typeof(string),
            typeof(DataPager),
            new UIPropertyMetadata(
                "5,10,20",
                (s, e) =>
                {
                    DataPager dp = s as DataPager;
                    if (dp.PageSizeItems == null)
                    {
                        dp.PageSizeItems = new List<int>();
                    }
                    else
                    {
                        dp.PageSizeItems.Clear();
                    }

                    dp.RaisePropertyChanged("PageSizeItems");
                }));

        /// <summary>
        /// ItemsSource数据源
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(DataPager), new UIPropertyMetadata(null));

        /// <summary>
        /// 翻页的路由事件
        /// </summary>
        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent("PageChanged", RoutingStrategy.Bubble, typeof(PageChangedEventHandler), typeof(DataPager));

        /// <summary>
        /// 字段 显示每页数据数集合
        /// </summary>
        private List<int> pageSizeItems;

        /// <summary>
        /// 字段 总页数
        /// </summary>
        private int pageCount;

        /// <summary>
        /// 字段 当前页显示的第一个数据的索引
        /// </summary>
        private int start;

        /// <summary>
        /// 字段 当前页显示的最后一个数据的索引
        /// </summary>
        private int end;

        /// <summary>
        /// 翻页事件参数
        /// </summary>
        private PageChangedEventArgs pageChangedEventArgs;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPager"/> class.
        /// </summary>
        public DataPager()
        {
            this.InitializeComponent();
        }

        #region 委托

        /// <summary>
        /// 翻页的委托
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="args">事件参数</param>
        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs args);

        #endregion

        #region 事件

        /// <summary>
        /// 分页更改事件
        /// </summary>
        public event PageChangedEventHandler PageChanged
        {
            add
            {
                this.AddHandler(PageChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(PageChangedEvent, value);
            }
        }

        /// <summary>
        /// 属性值改变通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region 属性

        /// <summary>
        /// 属性 显示每页数据数集合
        /// </summary>
        public List<int> PageSizeItems
        {
            get
            {
                if (this.pageSizeItems == null)
                {
                    this.pageSizeItems = new List<int>();
                }

                if (this.PageSizeList != null)
                {
                    List<string> strs = this.PageSizeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    this.pageSizeItems.Clear();
                    strs.ForEach(c =>
                    {
                        this.pageSizeItems.Add(Convert.ToInt32(c));
                    });
                }

                return this.pageSizeItems;
            }

            set
            {
                if (this.pageSizeItems != value)
                {
                    this.pageSizeItems = value;
                    this.RaisePropertyChanged("PageSizeItems");
                }
            }
        }

        /// <summary>
        /// 属性 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.pageCount;
            }

            set
            {
                if (this.pageCount != value)
                {
                    this.pageCount = value;
                    this.RaisePropertyChanged("PageCount");
                }
            }
        }

        /// <summary>
        /// 属性 当前页显示的第一个数据的索引
        /// </summary>
        public int Start
        {
            get
            {
                return this.start;
            }

            set
            {
                if (this.start != value)
                {
                    this.start = value;
                    this.RaisePropertyChanged("Start");
                }
            }
        }

        /// <summary>
        /// 属性 当前页显示的最后一个数据的索引
        /// </summary>
        public int End
        {
            get
            {
                return this.end;
            }

            set
            {
                if (this.end != value)
                {
                    this.end = value;
                    this.RaisePropertyChanged("End");
                }
            }
        }

        #region 依赖属性和事件

        /// <summary>
        /// 一页显示的数据个数
        /// </summary>
        public int PageSize
        {
            get
            {
                return (int)this.GetValue(PageSizeProperty);
            }

            set
            {
                this.SetValue(PageSizeProperty, value);
            }
        }

        /// <summary>
        /// 数据的总个数
        /// </summary>
        public int Total
        {
            get
            {
                return (int)this.GetValue(TotalProperty);
            }

            set
            {
                this.SetValue(TotalProperty, value);
            }
        }

        /// <summary>
        /// 当前的页数
        /// </summary>
        public int PageIndex
        {
            get
            {
                return (int)this.GetValue(PageIndexProperty);
            }

            set
            {
                this.SetValue(PageIndexProperty, value);
            }
        }

        /// <summary>
        /// 每页显示数据个数的集合
        /// </summary>
        public string PageSizeList
        {
            get
            {
                return (string)this.GetValue(PageSizeListProperty);
            }

            set
            {
                this.SetValue(PageSizeListProperty, value);
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable<object> ItemsSource
        {
            get
            {
                return (IEnumerable<object>)this.GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        #endregion
        #endregion

        #region 公开方法

        /// <summary>
        /// 属性值改变通知方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region 私有方法
        #region 分页操作事件

        /// <summary>
        /// 分页控件加载事件处理
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void DataPager_Loaded(object sender, RoutedEventArgs e)
        {
            this.RaisePageChanged();
        }

        /// <summary>
        /// 每页显示的数据个数改变事件的处理函数
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void CBPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                this.PageSize = (int)this.cboPageSize.SelectedItem;
                this.RaisePageChanged();
            }
        }

        /// <summary>
        /// 跳转到第一页事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            this.PageIndex = 1;
            this.RaisePageChanged();
        }

        /// <summary>
        /// 跳转到上一页事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.PageIndex > 1)
            {
                --this.PageIndex;
            }

            this.RaisePageChanged();
        }

        /// <summary>
        /// 跳转到下一页事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.Total % this.PageSize != 0)
            {
                this.PageCount = (this.Total / this.PageSize) + 1;
            }
            else
            {
                this.PageCount = this.Total / this.PageSize;
            }

            if (this.PageIndex < this.PageCount)
            {
                ++this.PageIndex;
            }

            this.RaisePageChanged();
        }

        /// <summary>
        /// 跳转到最后一页事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            if (this.Total % this.PageSize != 0)
            {
                this.PageCount = (this.Total / this.PageSize) + 1;
            }
            else
            {
                this.PageCount = this.Total / this.PageSize;
            }

            this.PageIndex = this.PageCount;
            this.RaisePageChanged();
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.RaisePageChanged();
        }

        /// <summary>
        /// 输入当前页触发的事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void TBPageIndex_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.TBPageIndex_LostFocus(sender, null);
            }
        }

        /// <summary>
        /// 当前页输入框失去焦点事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void TBPageIndex_LostFocus(object sender, RoutedEventArgs e)
        {
            int pageIndex = 0;
            try
            {
                pageIndex = Convert.ToInt32(this.tbPageIndex.Text);
            }
            catch
            {
                pageIndex = 1;
            }

            if (pageIndex < 1)
            {
                this.PageIndex = 1;
            }
            else if (pageIndex > this.PageCount)
            {
                this.PageIndex = this.PageCount;
            }
            else
            {
                this.PageIndex = pageIndex;
            }

            this.RaisePageChanged();
        }
        #endregion

        #region 引发分页更改事件

        /// <summary>
        /// 引发分页更改事件
        /// </summary>
        private void RaisePageChanged()
        {
            if (this.pageChangedEventArgs == null)
            {
                this.pageChangedEventArgs = new PageChangedEventArgs(PageChangedEvent, this.PageSize, this.PageIndex);
            }
            else
            {
                this.pageChangedEventArgs.PageSize = this.PageSize;
                this.pageChangedEventArgs.PageIndex = this.PageIndex;
            }

            this.RaiseEvent(this.pageChangedEventArgs);

            // 计算 Start、End的值
            if (this.ItemsSource != null)
            {
                int curCount = this.ItemsSource.Count();
                this.Start = ((this.PageIndex - 1) * this.PageSize) + 1;
                this.End = this.Start + curCount - 1;

                if (this.Total % this.PageSize != 0)
                {
                    this.PageCount = (this.Total / this.PageSize) + 1;
                }
                else
                {
                    this.PageCount = this.Total / this.PageSize;
                }
            }
            else
            {
                this.Start = this.End = this.PageCount = this.Total = 0;
            }

            // 调整图片的显示
            this.btnFirst.IsEnabled = this.PageIndex != 1;
            this.btnPrev.IsEnabled = this.btnFirst.IsEnabled;

            this.btnNext.IsEnabled = this.PageIndex != this.PageCount;
            this.btnLast.IsEnabled = this.btnNext.IsEnabled;
        }
        #endregion
        #endregion
    }
}
