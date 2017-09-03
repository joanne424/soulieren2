// <copyright file="DropBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/30 8:43:49</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/30 8:43:49
//   修改描述：新建 DropBox.cs
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
    /// 带有输入验证规则的下拉框控件。
    /// </summary>
    public class DropBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropBox" /> class.
        /// </summary>
        public DropBox()
        {
            this.IsEditable = true;
            this.Loaded += (s, e) =>
            {
                this.EditablePart = (TextBox)Template.FindName("PART_EditableTextBox", this);
                this.EditablePart.TextChanged += (s1, e1) => OnTextChanged(e1);
                this.EditablePart.MouseLeftButtonUp += (s2, e2) => OnTextMouseUp(e2);
                this.EditablePart.MouseRightButtonUp += (s2, e2) => OnTextMouseUp(e2);
                InputMethod.SetIsInputMethodEnabled(EditablePart, false);
            };
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
        /// 下拉框控件中的编辑框。
        /// </summary>
        protected TextBox EditablePart { get; private set; }

        /// <summary>
        /// 编辑框的光标索引。
        /// </summary>
        protected int SelectionStart
        {
            get { return this.EditablePart.SelectionStart; }
            set { this.EditablePart.SelectionStart = value; }
        }

        /// <summary>
        /// 编辑框的选中文本长度
        /// </summary>
        protected int SelectionLength
        {
            get { return this.EditablePart.SelectionLength; }
            set { this.EditablePart.SelectionLength = value; }
        }

        /// <summary>
        /// 处理键盘事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.PreviousSelectedIndex = this.SelectionStart;
            this.PreviousSelectedLength = this.SelectionLength;

            base.OnKeyDown(e);
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

        /// <summary>
        /// 文本框的鼠标弹起事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnTextMouseUp(MouseButtonEventArgs e)
        {
            this.PreviousSelectedIndex = this.SelectionStart;
            this.PreviousSelectedLength = this.SelectionLength;
        }

        /// <summary>
        /// 编辑框的文本改变事件。
        /// </summary>
        /// <param name="e">时间参数</param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
        }
    }
}