// <copyright file="ExtendedDataGrid.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>董国君</author>
// <date> 2013/10/9 16:27:38 </date>
// <summary> 扩展DataGrid </summary>
// <modify>
//      修改人：XXX
//      修改时间：XXXXXX
//      修改描述：XXXXX
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

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

namespace BancLogix.ExtendedDataGrid
{
    /// <summary>
    /// 扩展DataGrid
    /// </summary>
    public partial class ExtendedDataGrid : DataGrid
    {
        #region DependencyProperty
        
        /// <summary>
        /// 底部显示块的依赖属性
        /// </summary>
        public static readonly DependencyProperty FooterDataTemplateProperty = DependencyProperty.Register("FooterDataTemplate", typeof(DataTemplate), typeof(ExtendedDataGrid));

        /// <summary>
        /// 行双击依赖属性
        /// </summary>
        public static readonly DependencyProperty RowDoubleClickCommandProperty = DependencyProperty.Register("RowDoubleClickCommand", typeof(ICommand), typeof(ExtendedDataGrid), new PropertyMetadata(null));
        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="ExtendedDataGrid" /> class.
        /// </summary>
        static ExtendedDataGrid()
        {
            // Workaround
            // 暂时注释掉此处处理，因为当前CustomerList使用的是此DataGrid加载会很缓慢且ToolWindow切换都卡顿
            // 修改成DataGrid就不存在卡顿问题，但是需要支持行的双击事件，所以需要用ExtendedDataGrid
            // 故调查ExtendedDataGrid启动慢并且卡顿的原因，最终定位到此处，此处标识将取ExtendedDataGrid的Sytle
            // 已经尝试过注释修改ExtendedDataGrid相关的Style都没有效果，而ExtendedDataGrid相对于DataGrid仅仅
            // 多了一个双击行的事件，所以暂时注释掉此处，待后续调查到影响的Style可恢复此处
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedDataGrid), new FrameworkPropertyMetadata(typeof(ExtendedDataGrid)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedDataGrid" /> class.
        /// </summary>
        public ExtendedDataGrid()
        {
            this.AddResources();
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

        #region 事件
        /// <summary>
        /// 行双击事件
        /// </summary>
        public event EventHandler RowDoubleClick;
        #endregion        
        
        #region 依赖属性
        /// <summary>
        /// 列表底部
        /// </summary>
        public DataTemplate FooterDataTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(FooterDataTemplateProperty);
            }

            set
            {
                this.SetValue(FooterDataTemplateProperty, value);
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
        #endregion
    }
}
