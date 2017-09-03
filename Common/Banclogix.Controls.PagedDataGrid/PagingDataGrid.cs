// <copyright file="PagingDataGrid.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-12-26 </date>
// <summary>分页控件类</summary>
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// 分页DataGrid
    /// </summary>
    [TemplatePart(Name = PagingDataGrid.ElementFirstPageImageButton, Type = typeof(ImageButton))]
    [TemplatePart(Name = PagingDataGrid.ElementPerviousPageImageButton, Type = typeof(ImageButton))]
    [TemplatePart(Name = PagingDataGrid.ElementNextPageImageButton, Type = typeof(ImageButton))]
    [TemplatePart(Name = PagingDataGrid.ElementLastPageImageButton, Type = typeof(ImageButton))]
    [TemplatePart(Name = PagingDataGrid.ElementPageSizeListComBox, Type = typeof(ComboBox))]
    [TemplatePart(Name = PagingDataGrid.ElementPageIndexTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PagingDataGrid.ElementPageCountTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PagingDataGrid.ElementStartTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PagingDataGrid.ElementEndTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PagingDataGrid.ElementCountTextBlock, Type = typeof(TextBlock))]
    public class PagingDataGrid : DataGrid
    {
        #region 字段和常量

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsShowRowHeaderNumber.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsShowRowHeaderNumberProperty =
            DependencyProperty.Register("IsShowRowHeaderNumber", typeof(bool), typeof(PagingDataGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// 是否显示分页
        /// </summary>
        public static readonly DependencyProperty IsShowPagingProperty =
            DependencyProperty.Register("IsShowPaging", typeof(bool), typeof(PagingDataGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// 是否允许分页
        /// </summary>
        public static readonly DependencyProperty AllowPagingProperty =
            DependencyProperty.Register("AllowPaging", typeof(bool), typeof(PagingDataGrid), new UIPropertyMetadata(true));

        /// <summary>
        /// 命令字符串
        /// </summary>
        public static readonly DependencyProperty CommandStringProperty = DependencyProperty.Register(
            "CommandString",
            typeof(string),
            typeof(PagingDataGrid),
            new PropertyMetadata(
                string.Empty,
                (s, e) =>
                {
                    PagingDataGrid dp = s as PagingDataGrid;
                    if (e.NewValue == null)
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(e.NewValue.ToString()) == true)
                    {
                        return;
                    }

                    var cmdString = e.NewValue.ToString();
                    string[] cmdParams = cmdString.Split(',');

                    if (cmdParams[0].Equals("Reload") == true)
                    {
                        dp.PageIndex = Convert.ToInt32(cmdParams[1]);
                        dp.PageCount = Convert.ToInt32(cmdParams[2]);

                        dp.CalStartEnd();
                    }
                    else if (cmdParams[0].Equals("Init") == true)
                    {
                        if (dp.PageCount != 0)
                        {
                            dp.CalStartEnd();
                        }
                    }

                    dp.btnFirst.IsEnabled = dp.PageIndex != 1;
                    dp.btnPrevious.IsEnabled = dp.btnFirst.IsEnabled;
                    dp.btnNext.IsEnabled = dp.PageIndex != dp.PageCount;
                    dp.btnLast.IsEnabled = dp.btnNext.IsEnabled;
        }));

        /// <summary>
        /// 显示每页记录数字符串列表
        /// 例:10,20,30
        /// </summary>
        public static readonly DependencyProperty PageSizeListProperty = DependencyProperty.Register(
            "PageSizeList",
            typeof(string),
            typeof(PagingDataGrid),
            new UIPropertyMetadata(
                null,
                (s, e) =>
                {
                    PagingDataGrid dp = s as PagingDataGrid;
                    if (dp.PageSizeItemsSource == null)
                    {
                        dp.PageSizeItemsSource = new List<int>();
                    }

                    if (dp.PageSizeItemsSource != null)
                    {
                        List<string> strs = e.NewValue.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        dp.PageSizeItemsSource.Clear();
                        strs.ForEach(c =>
                        {
                            dp.PageSizeItemsSource.Add(Convert.ToInt32(c));
                        });
                    }
                }));

        /// <summary>
        /// 总记录数
        /// </summary>
        public static readonly DependencyProperty TotalProperty = DependencyProperty.Register("Total", typeof(int), typeof(PagingDataGrid), new UIPropertyMetadata(0));

        /// <summary>
        /// 总页数
        /// </summary>
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(PagingDataGrid), new UIPropertyMetadata(1));

        /// <summary>
        /// 每页记录数，默认：15
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagingDataGrid), new UIPropertyMetadata(15));

        /// <summary>
        /// 当前页码，默认：1
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(PagingDataGrid), new UIPropertyMetadata(1));

        /// <summary>
        /// 排序字段
        /// </summary>
        public static readonly DependencyProperty SortFieldProperty =
            DependencyProperty.Register("SortField", typeof(string), typeof(PagingDataGrid), new UIPropertyMetadata(null));

        /// <summary>
        /// 分页路由事件
        /// </summary>
        public static readonly RoutedEvent PagingChangedEvent = EventManager.RegisterRoutedEvent("PagingChangedEvent", RoutingStrategy.Bubble, typeof(PagingChangedEventHandler), typeof(PagingDataGrid));

        /// <summary>
        /// 行双击依赖属性
        /// </summary>
        public static readonly DependencyProperty RowDoubleClickCommandProperty = DependencyProperty.Register("RowDoubleClickCommand", typeof(ICommand), typeof(PagingDataGrid), new PropertyMetadata(null));

        /// <summary>
        /// 显示每页记录数集合
        /// </summary>
        protected static readonly DependencyProperty PageSizeItemsSourceProperty =
            DependencyProperty.Register("PageSizeItemsSource", typeof(IList<int>), typeof(PagingDataGrid), new UIPropertyMetadata(new List<int> { 15, 20, 50, 100 }));

        /// <summary>
        /// 起始记录数
        /// </summary>
        protected static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(int), typeof(PagingDataGrid), new UIPropertyMetadata(0));

        /// <summary>
        /// 终止记录数
        /// </summary>
        protected static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(int), typeof(PagingDataGrid), new UIPropertyMetadata(0));

        /// <summary>
        /// 第一页
        /// </summary>
        private const string ElementFirstPageImageButton = "PART_FirstPage";

        /// <summary>
        /// 上一页
        /// </summary>
        private const string ElementPerviousPageImageButton = "PART_PerviousPage";

        /// <summary>
        /// 下一页
        /// </summary>
        private const string ElementNextPageImageButton = "PART_NextPage";

        /// <summary>
        /// 最后一页
        /// </summary>
        private const string ElementLastPageImageButton = "PART_LastPage";

        /// <summary>
        /// 每页数据个数集合
        /// </summary>
        private const string ElementPageSizeListComBox = "PART_PageSizeList";

        /// <summary>
        /// 当前页数
        /// </summary>
        private const string ElementPageIndexTextBox = "PART_PageIndex";

        /// <summary>
        /// 每页数据个数
        /// </summary>
        private const string ElementPageCountTextBlock = "PART_PageCount";

        /// <summary>
        /// 显示起始数据的索引
        /// </summary>
        private const string ElementStartTextBlock = "PART_Start";

        /// <summary>
        /// 显示结束数据的索引
        /// </summary>
        private const string ElementEndTextBlock = "PART_End";

        /// <summary>
        /// 数据总个数
        /// </summary>
        private const string ElementCountTextBlock = "PART_Count";

        /// <summary>
        /// 图片按钮
        /// </summary>
        private ImageButton btnFirst, btnPrevious, btnNext, btnLast;

        /// <summary>
        /// 分页大小集合
        /// </summary>
        private ComboBox cboPageSize;

        /// <summary>
        /// 当前页
        /// </summary>
        private TextBox txtPageIndex;

        /// <summary>
        /// 显示 页数、开始索引、结束索引、数据总个数
        /// </summary>
        private TextBlock txtPageCount, txtStart, txtEnd, txtCount;

        /// <summary>
        /// 翻页事件参数
        /// </summary>
        private PagingChangedEventArgs pagingChangedEventArgs;

        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="PagingDataGrid" /> class.
        /// </summary>
        static PagingDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PagingDataGrid), new FrameworkPropertyMetadata(typeof(PagingDataGrid)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingDataGrid"/> class.
        /// </summary>
        public PagingDataGrid()
        {
            this.CanUserAddRows = false;

            MouseButtonEventHandler handler = (sender, args) =>
            {
                var row = sender as DataGridRow;
                if (row != null && row.IsSelected)
                {
                    ////System.Diagnostics.Debug.WriteLine("行被双击" + row.GetIndex());

                    if (RowDoubleClickCommand != null)
                    {
                        RowDoubleClickCommand.Execute(null);
                    }

                    if (RowDoubleClick != null)
                    {
                        RowDoubleClick(row, new EventArgs());
                    }
                    //自定义RowDoubleClick调用后将Handled设置为true，解决在此事件中弹窗未能获取输入焦点的问题
                    args.Handled = true;
                }
            };

            this.LoadingRow += (s, e) =>
            {
                e.Row.MouseDoubleClick += handler;
            };

            this.UnloadingRow += (s, e) =>
            {
                e.Row.MouseDoubleClick -= handler;
            };
        }

        #region 委托

        /// <summary>
        /// 翻页事件的委托
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="args">事件参数</param>
        public delegate void PagingChangedEventHandler(object sender, PagingChangedEventArgs args);

        #endregion

        #region 事件

        /// <summary>
        /// 分页事件
        /// </summary>
        public event PagingChangedEventHandler PagingChanged
        {
            add
            {
                this.AddHandler(PagingChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(PagingChangedEvent, value);
            }
        }

        /// <summary>
        /// 行双击事件
        /// </summary>
        public event EventHandler RowDoubleClick;

        #endregion

        #region 依赖属性

        /// <summary>
        /// 命令字符串
        /// </summary>
        public string CommandString
        {
            get
            {
                return (string)this.GetValue(CommandStringProperty);
            }

            set
            {
                this.SetValue(CommandStringProperty, value);
            }
        }

        /// <summary>
        /// 是否显示行号
        /// </summary>
        public bool IsShowRowHeaderNumber
        {
            get
            {
                return (bool)this.GetValue(IsShowRowHeaderNumberProperty);
            }

            set
            {
                this.SetValue(IsShowRowHeaderNumberProperty, value);
            }
        }

        /// <summary>
        /// 是否显示分页
        /// </summary>
        public bool IsShowPaging
        {
            get
            {
                return (bool)this.GetValue(IsShowPagingProperty);
            }

            set
            {
                this.SetValue(IsShowPagingProperty, value);
            }
        }

        /// <summary>
        /// 是否允许分页
        /// </summary>
        public bool AllowPaging
        {
            get
            {
                return (bool)this.GetValue(AllowPagingProperty);
            }

            set
            {
                this.SetValue(AllowPagingProperty, value);
            }
        }

        /// <summary>
        /// 分页大小的集合
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
        /// 数据总个数
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
        /// 页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return (int)this.GetValue(PageCountProperty);
            }

            set
            {
                this.SetValue(PageCountProperty, value);
            }
        }

        /// <summary>
        /// 每页显示数据的个数
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
        /// 当前页号
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
        /// 排序字段
        /// </summary>
        public string SortField
        {
            get
            {
                return (string)this.GetValue(SortFieldProperty);
            }

            set
            {
                this.SetValue(SortFieldProperty, value);
            }
        }

        /// <summary>
        /// 行双击命令
        /// </summary>
        public ICommand RowDoubleClickCommand
        {
            get
            {
                return (ICommand)this.GetValue(RowDoubleClickCommandProperty);
            }

            set
            {
                this.SetValue(RowDoubleClickCommandProperty, value);
            }
        } 

        /// <summary>
        /// 分页大小的集合
        /// </summary>
        protected IList<int> PageSizeItemsSource
        {
            get
            {
                return (IList<int>)this.GetValue(PageSizeItemsSourceProperty);
            }

            set
            {
                this.SetValue(PageSizeItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// 开始显示数据的行号
        /// </summary>
        protected int Start
        {
            get
            {
                return (int)this.GetValue(StartProperty);
            }

            set
            {
                this.SetValue(StartProperty, value);
            }
        }

        /// <summary>
        /// 结束显示数据的行号
        /// </summary>
        protected int End
        {
            get
            {
                return (int)this.GetValue(EndProperty);
            }

            set
            {
                this.SetValue(EndProperty, value);
            }
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 应用样式
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.btnFirst = this.GetTemplateChild(ElementFirstPageImageButton) as ImageButton;
            this.btnPrevious = this.GetTemplateChild(ElementPerviousPageImageButton) as ImageButton;
            this.btnNext = this.GetTemplateChild(ElementNextPageImageButton) as ImageButton;
            this.btnLast = this.GetTemplateChild(ElementLastPageImageButton) as ImageButton;

            this.cboPageSize = this.GetTemplateChild(ElementPageSizeListComBox) as ComboBox;
            this.txtPageIndex = this.GetTemplateChild(ElementPageIndexTextBox) as TextBox;

            this.txtPageCount = this.GetTemplateChild(ElementPageCountTextBlock) as TextBlock;
            this.txtStart = this.GetTemplateChild(ElementStartTextBlock) as TextBlock;
            this.txtEnd = this.GetTemplateChild(ElementEndTextBlock) as TextBlock;
            this.txtCount = this.GetTemplateChild(ElementCountTextBlock) as TextBlock;

            this.btnFirst.Click += new RoutedEventHandler(this.BtnFirst_Click);
            this.btnLast.Click += new RoutedEventHandler(this.BtnLast_Click);
            this.btnNext.Click += new RoutedEventHandler(this.BtnNext_Click);
            this.btnPrevious.Click += new RoutedEventHandler(this.BtnPrevious_Click);

            this.cboPageSize.SelectionChanged += new SelectionChangedEventHandler(this.CBPageSize_SelectionChanged);
            this.txtPageIndex.PreviewKeyDown += new KeyEventHandler(this.TxtPageIndex_PreviewKeyDown);
            this.txtPageIndex.LostFocus += new RoutedEventHandler(this.TxtPageIndex_LostFocus);

            this.Loaded += new RoutedEventHandler(this.PagingDataGrid_Loaded);

            this.btnFirst.IsEnabled = false;
            this.btnPrevious.IsEnabled = false;
            this.btnNext.IsEnabled = false;
            this.btnLast.IsEnabled = false;
        }

        /// <summary>
        /// 重载的加载行处理函数
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            if (this.IsShowRowHeaderNumber)
            {
                e.Row.Header = e.Row.GetIndex() + 1;
            }
        }

        #endregion

        #region 分页事件

        /// <summary>
        /// 触发分页事件
        /// </summary>
        private void RaisePageChanged()
        {
            if (this.pagingChangedEventArgs == null)
            {
                this.pagingChangedEventArgs = new PagingChangedEventArgs(PagingChangedEvent, this.PageSize, this.PageIndex);
            }

            if (this.AllowPaging)
            {
                this.pagingChangedEventArgs.PageSize = this.PageSize;
                this.pagingChangedEventArgs.PageIndex = this.PageIndex;
            }
            else
            {
                this.pagingChangedEventArgs.PageSize = this.PageSize = int.MaxValue;
                this.pagingChangedEventArgs.PageIndex = 1;
            }

            this.RaiseEvent(this.pagingChangedEventArgs);

            this.CalStartEnd();

            // 调整图片的显示
            this.btnFirst.IsEnabled = this.PageIndex != 1;
            this.btnPrevious.IsEnabled = this.btnFirst.IsEnabled;
            this.btnNext.IsEnabled = this.PageIndex != this.PageCount;
            this.btnLast.IsEnabled = this.btnNext.IsEnabled;
        }

        /// <summary>
        /// 计算开始显示和结束显示的数据的索引值
        /// </summary>
        private void CalStartEnd()
        {
            // calc start、end
            if (this.ItemsSource != null)
            {
                int curCount = 0;
                IEnumerator enumrator = this.ItemsSource.GetEnumerator();
                while (enumrator.MoveNext())
                {
                    curCount++;
                }

                // 不允许分页处理
                if (!this.AllowPaging)
                {
                    this.PageSizeItemsSource.Clear();
                    this.PageSizeItemsSource.Add(curCount);
                    this.PageSize = curCount;
                }

                this.Start = ((this.PageIndex - 1) * this.PageSize) + 1;
                this.End = this.Start + curCount - 1;

                //if (this.Total % this.PageSize != 0)
                //{
                //    this.PageCount = (this.Total / this.PageSize) + 1;
                //}
                //else
                //{
                //    this.PageCount = this.Total / this.PageSize;
                //}
            }
            else
            {
                this.Start = this.End = this.PageCount = this.Total = 0;
            }
        }

        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void PagingDataGrid_Loaded(object sender, RoutedEventArgs e)
        {            
            this.Start = this.PageIndex;
            this.End = this.PageSizeItemsSource[0];
        }

        /// <summary>
        /// 跳转到第一页
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            this.PageIndex = 1;
            this.RaisePageChanged();
        }

        /// <summary>
        /// 跳转到上一页
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (this.PageIndex > 1)
            {
                --this.PageIndex;
            }

            this.RaisePageChanged();
        }

        /// <summary>
        /// 跳转到下一页
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //if (this.Total % this.PageSize != 0)
            //{
            //    this.PageCount = (this.Total / this.PageSize) + 1;
            //}
            //else
            //{
            //    this.PageCount = this.Total / this.PageSize;
            //}

            if (this.PageIndex < this.PageCount)
            {
                ++this.PageIndex;
            }

            this.RaisePageChanged();
        }

        /// <summary>
        /// 跳转到最后一页
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            //if (this.Total % this.PageSize != 0)
            //{
            //    this.PageCount = (this.Total / this.PageSize) + 1;
            //}
            //else
            //{
            //    this.PageCount = this.Total / this.PageSize;
            //}

            this.PageIndex = this.PageCount;
            this.RaisePageChanged();
        }

        /// <summary>
        /// 刷新当前页的数据
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.RaisePageChanged();
        }

        /// <summary>
        /// 当前页输入处理函数
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void TxtPageIndex_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.TxtPageIndex_LostFocus(sender, null);
            }
        }

        /// <summary>
        /// 当前页输入失去焦点的处理函数
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void TxtPageIndex_LostFocus(object sender, RoutedEventArgs e)
        {
            int pageIndex = 0;
            try
            {
                pageIndex = Convert.ToInt32(this.txtPageIndex.Text);
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

        /// <summary>
        /// 每页显示数据大小改变的事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void CBPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                this.PageSize = (int)this.cboPageSize.SelectedItem;
                this.PageIndex = 1;
                this.RaisePageChanged();
            }
        }
        #endregion
    }
}
