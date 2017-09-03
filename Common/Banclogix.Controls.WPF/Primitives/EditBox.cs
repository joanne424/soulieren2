// <copyright file="EditBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/29 1:07:54</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/29 1:07:54
//   修改描述：新建 EditBox.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banclogix.Controls.Primitives
{
    /// <summary>
    /// 带有输入验证规则的文本编辑控件。
    /// </summary>
    public abstract class EditBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditBox" /> class.
        /// </summary>
        public EditBox()
        {
            InputMethod.SetIsInputMethodEnabled(this, false);
        }

        /// <summary>
        /// 转换规则。
        /// </summary>
        protected string ParseRule { get; set; }

        /// <summary>
        /// Text 变更前的文本。
        /// </summary>
        protected string PreviousText { get; set; }

        /// <summary>
        /// Text 变更前的光标索引。
        /// </summary>
        protected int PreviousSelectedIndex { get; set; }

        /// <summary>
        /// Text 变更前的选中文本长度。
        /// </summary>
        protected int PreviousSelectedLength { get; set; }

        /// <summary>
        /// 处理键盘事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            this.PreviousSelectedIndex = this.SelectionStart;
            this.PreviousSelectedLength = this.SelectionLength;

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// 文本框的鼠标弹起事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.PreviousSelectedIndex = this.SelectionStart;
            this.PreviousSelectedLength = this.SelectionLength;

            base.OnMouseUp(e);
        }

        /// <summary>
        /// 下拉框的属性变更事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Text")
            {
                this.PreviousText = (string)e.OldValue;
            }

            base.OnPropertyChanged(e);
        }
    }
}