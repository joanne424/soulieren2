// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashLadderDataGridTool.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/05/26 04:55:53 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/05/26 04:55:53
//      修改描述：新建 CashLadderDataGridTool.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using DM2.Ent.Client.Runtime;

    /// <summary>
    ///     The cash ladder data grid tool.
    /// </summary>
    public class CashLadderDataGridTool
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create data grid.
        /// </summary>
        /// <param name="dataTable">
        /// The data table.
        /// </param>
        /// <param name="showDetail">
        /// The show detail.
        /// </param>
        /// <param name="isTool">
        /// The is tool.
        /// </param>
        /// <returns>
        /// The <see cref="System.Windows.Controls.DataGrid"/>.
        /// </returns>
        public static DataGrid CreateDataGrid(
            DataTable dataTable,
            Action<object, MouseButtonEventArgs> showDetail,
            bool isTool = false)
        {
            if (dataTable == null)
            {
                return null;
            }

            int colCount = dataTable.Columns.Count;
            Color alternationColor = Color.FromRgb(246, 246, 246);
            Brush brush = new SolidColorBrush(alternationColor);
            int tatolwidth = 0;
            var dataGrid = new DataGrid
            {
                ItemsSource = dataTable.DefaultView,
                SelectionMode = DataGridSelectionMode.Single,
                SelectionUnit = DataGridSelectionUnit.Cell,
                VerticalContentAlignment = VerticalAlignment.Top,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                CanUserSortColumns = true,
                AlternatingRowBackground = brush,
                AlternationCount = 2,
                AutoGenerateColumns = false,
                Width = double.NaN
            };

            foreach (DataColumn item in dataTable.Columns)
            {
                Binding binding;
                if (item.ColumnName.Equals("TotalAmountColor"))
                {
                    continue;
                }

                if (item.ColumnName == "CurrencyName")
                {
                    binding = new Binding("CurrencyName") { Mode = BindingMode.OneWay };
                    var tempCol = new DataGridTemplateColumn { Header = RunTime.FindStringResource("CCY"), Width = 80 };
                    tatolwidth += 80;
                    var cellTemp = new DataTemplate();
                    tempCol.CellTemplate = cellTemp;

                    // 创建textbox项
                    var tb = new FrameworkElementFactory(typeof(DataGridText));
                    tb.SetBinding(TextBlock.TextProperty, binding);
                    tb.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tb.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    cellTemp.VisualTree = tb;

                    dataGrid.Columns.Add(tempCol);
                    continue;
                }

                if (item.ColumnName == "TotalAmountStr")
                {
                    binding = new Binding("TotalAmountStr") { Mode = BindingMode.OneWay };

                    var tempCol = new DataGridTemplateColumn
                                      {
                                          Header = RunTime.FindStringResource("Total"),
                                          Width = 140
                                      };
                    if (isTool)
                    {
                        switch (colCount)
                        {
                            case 2:
                                tempCol.Width = 360;
                                tatolwidth += 360;
                                break;
                            case 3:
                                tempCol.Width = 300;
                                tatolwidth += 300;
                                break;
                            case 4:
                                tempCol.Width = 240;
                                tatolwidth += 240;
                                break;
                            case 5:
                                tempCol.Width = 160;
                                tatolwidth += 160;
                                break;
                            default:
                                tempCol.Width = 120;
                                tatolwidth += 120;
                                break;
                        }
                    }

                    var cellTemp = new DataTemplate();
                    tempCol.CellTemplate = cellTemp;

                    // 创建textbox项
                    var tb = new FrameworkElementFactory(typeof(DataGridText));
                    tb.SetBinding(TextBlock.TextProperty, binding);
                    tb.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tb.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    tb.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 0, 5, 0));
                    tb.SetValue(FrameworkElement.CursorProperty, Cursors.Hand);

                    // 显示颜色绑定
                    var bindingForeground = new Binding("TotalAmountColor") { Mode = BindingMode.TwoWay };
                    tb.SetBinding(TextBlock.ForegroundProperty, bindingForeground);

                    cellTemp.VisualTree = tb;

                    // 为DataGrid添加列
                    dataGrid.Columns.Add(tempCol);
                }
                else
                {
                    binding = new Binding(item.ColumnName + ".LadderAmountStr") { Mode = BindingMode.OneWay };

                    var tempCol = new DataGridTemplateColumn { Header = item.ColumnName, Width = 120 };
                    if (isTool)
                    {
                        switch (colCount)
                        {
                            case 2:
                                tempCol.Width = 300;
                                tatolwidth += 300;
                                break;
                            case 3:
                                tempCol.Width = 220;
                                tatolwidth += 220;
                                break;
                            case 4:
                                tempCol.Width = 180;
                                tatolwidth += 180;
                                break;
                            case 5:
                                tempCol.Width = 140;
                                tatolwidth += 140;
                                break;
                            default:
                                tempCol.Width = 100;
                                tatolwidth += 100;
                                break;
                        }
                    }

                    var cellTemp = new DataTemplate();
                    tempCol.CellTemplate = cellTemp;

                    // 创建textbox项
                    var tb = new FrameworkElementFactory(typeof(DataGridText));
                    tb.SetBinding(TextBlock.TextProperty, binding);
                    tb.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
                    tb.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    tb.SetValue(TextBlock.TextDecorationsProperty, TextDecorations.Underline);
                    tb.SetValue(FrameworkElement.CursorProperty, Cursors.Hand);
                    tb.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 0, 5, 0));
                    tb.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(showDetail));

                    // 显示颜色绑定
                    var bindingForeground = new Binding(item.ColumnName + ".LadderAmountColor")
                    {
                        Mode = BindingMode.OneWay
                    };
                    tb.SetBinding(TextBlock.ForegroundProperty, bindingForeground);

                    // 构造点击事件及参数赋值
                    var bindingEvent = new Binding(item.ColumnName) { Mode = BindingMode.OneWay };
                    tb.SetBinding(DataGridText.StationInfoDisplayProperty, bindingEvent);

                    // 将DataGridText添加到DataTemplate
                    cellTemp.VisualTree = tb;

                    // 为DataGrid添加列
                    dataGrid.Columns.Add(tempCol);
                }
            }

            if (isTool && tatolwidth > 0)
            {
                dataGrid.Width = tatolwidth + 5;
            }

            // 表头居中
            var style = new Style(typeof(DataGridColumnHeader));
            var setCenter = new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            style.Setters.Add(setCenter);

            dataGrid.ColumnHeaderStyle = style;
            return dataGrid;
        }

        #endregion
    }
}