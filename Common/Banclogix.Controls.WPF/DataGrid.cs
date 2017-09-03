// <copyright file="DataGrid.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/12/23 14:19:36</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/12/23 14:19:36
//   修改描述：新建 DataGrid.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Banclogix.Controls
{
    /// <summary>
    /// 获取鼠标位置的函数指针。
    /// </summary>
    /// <param name="theElement">当前处于鼠标焦点的控件</param>
    /// <returns>返回鼠标相对于控件所在的坐标</returns>
    public delegate Point GetPosition(IInputElement theElement);

    /// <summary>
    /// 支持双击和拖放的 DataGrid。
    /// </summary>
    public class DataGrid : System.Windows.Controls.DataGrid
    {
        /// <summary>
        /// 用于支持命令绑定的依赖属性。
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(DataGrid), new PropertyMetadata(Callback));

        /// <summary>
        /// 用于指示选中列表绑定的依赖属性。
        /// </summary>
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register(
            "SelectedItemsList", typeof(IList), typeof(DataGrid), new PropertyMetadata(Callback));

        /// <summary>
        /// 属性变更的回掉函数。
        /// </summary>
        private static readonly PropertyChangedCallback Callback = new PropertyChangedCallback(ValueChanged);

        /// <summary>
        /// 双击选中行的命令。
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// 选中行的集合。
        /// </summary>
        public IList SelectedItemsList
        {
            get { return (IList)this.GetValue(SelectedItemsListProperty); }
            set { }
        }

        /// <summary>
        /// 拖放的开始索引。
        /// </summary>
        protected int DragIndex { get; set; }

        /// <summary>
        /// 拖放的开始坐标。
        /// </summary>
        protected Point DragPoint { get; set; }

        /// <summary>
        /// 选中的行变更时。
        /// </summary>
        /// <param name="e">时间参数</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            this.SetValue(SelectedItemsListProperty, this.SelectedItems);
            base.OnSelectionChanged(e);
        }

        /// <summary>
        /// 拖放完成时。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (this.DragIndex < 0)
            {
                return;
            }

            var index = this.GetRowAtPoint(e.GetPosition);
            if (index < 0 || index == this.DragIndex)
            {
                return;
            }

            var list = ItemsSource as IList;
            if (list == null)
            {
                return;
            }

            var dragItem = list[this.DragIndex];
            list.RemoveAt(this.DragIndex);
            list.Insert(index, dragItem);
        }

        /// <summary>
        /// 鼠标按下时。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.DragIndex = this.GetRowAtPoint(e.GetPosition);
            this.DragPoint = e.GetPosition(this);
        }

        /// <summary>
        /// 鼠标移动时。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            if (!this.AllowDrop)
            {
                return;
            }

            var point = e.GetPosition(this);
            if (Math.Abs(this.DragPoint.X - point.X) < 5 &&
                Math.Abs(this.DragPoint.Y - point.Y) < 5)
            {
                return;
            }

            if (this.DragIndex == -1)
            {
                return;
            }

            if (this.DragIndex >= this.Items.Count)
            {
                return;
            }

            var selected = this.Items[this.DragIndex];
            if (DragDrop.DoDragDrop(this, selected, DragDropEffects.Move) !=
                DragDropEffects.None)
            {
                this.SelectedItem = selected;
            }
        }

        /// <summary>
        /// 鼠标双击时。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
            if (this.Command == null)
            {
                return;
            }

            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            this.SelectedIndex = this.GetRowAtPoint(e.GetPosition);
            if (this.SelectedIndex < 0)
            {
                return;
            }

            this.Command.Execute(this.SelectedItem);
        }

        /// <summary>
        /// 获取鼠标所在行的索引。
        /// </summary>
        /// <param name="getPosition">获取鼠标位置的函数指针</param>
        /// <returns>返回取到的索引</returns>
        protected int GetRowAtPoint(GetPosition getPosition)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var hoverRow = this.GetRowAtIndex(i);
                if (this.IsInBounds(hoverRow, getPosition))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 鼠标焦点是否当前的 Visual 内。
        /// </summary>
        /// <param name="visual">指定的控件</param>
        /// <param name="getPosition">获取鼠标位置的函数指针</param>
        /// <returns>返回鼠标焦点是否当前的 Visual 内</returns>
        protected bool IsInBounds(Visual visual, GetPosition getPosition)
        {
            if (visual == null)
            {
                return false;
            }

            var bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var point = getPosition((IInputElement)visual);
            return bounds.Contains(point);
        }

        /// <summary>
        /// 获取指定索引出的 DataGridRow
        /// </summary>
        /// <param name="index">指定的索引</param>
        /// <returns>返回索引出的 DataGridRow</returns>
        protected Visual GetRowAtIndex(int index)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return null;
            }

            return ItemContainerGenerator.ContainerFromIndex(index) as Visual;
        }

        /// <summary>
        /// 属性变更时触发。
        /// </summary>
        /// <param name="obj">属性变更的对象</param>
        /// <param name="e">事件的参数</param>
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Command":
                    ((DataGrid)obj).Command = (ICommand)e.NewValue;
                    break;
                case "SelectedItemsList":
                    ((DataGrid)obj).SelectedItemsList = (IList)e.NewValue;
                    break;
            }
        }
    }
}