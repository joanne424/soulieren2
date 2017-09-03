// <copyright file="TimeBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/29 2:20:05</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/29 2:20:05
//   修改描述：新建 TimeBox2.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banclogix.Controls
{
    /// <summary>
    /// 用于限制时间输入的文本编辑控件。
    /// </summary>
    public class TimeBox : Primitives.EditBox
    {
        /// <summary>
        /// 用于绑定的时间属性。
        /// </summary>
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(TimeSpan), typeof(TimeBox), new PropertyMetadata(TimeSpan.Zero, ValueChanged));

        /// <summary>
        /// 数字。
        /// </summary>
        private static readonly string[] Number = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBox" /> class.
        /// </summary>
        public TimeBox()
        {
            this.ParseRule = @"^((2[0-3])|([0|1]\d)):[0-5][0-9]$";
            this.Text = "00:00";
        }

        /// <summary>
        /// 获取或设置输入的时间。
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return (TimeSpan)GetValue(TimeProperty);
            }

            set
            {
                this.SetValue(TimeProperty, value);
                this.Text = value.ToString("hh':'mm");
            }
        }

        /// <summary>
        /// 处理键盘时间。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (this.SelectionLength > 1)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                e.Handled = true;
                this.OnBackspace();
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
                this.OnDelete();
            }
            else if (e.Key == Key.Space)
            {
                e.Handled = true;
                this.OnSpace();
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = true;
                this.OnNumber(Number[(int)e.Key - 74]);
            }
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                e.Handled = true;
                this.OnNumber(Number[(int)e.Key - 34]);
            }
        }

        /// <summary>
        /// 处理文本变化事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(this.Text, this.ParseRule))
            {
                this.Text = this.PreviousText;
            }
            else
            {
                this.Time = TimeSpan.Parse(this.Text);
            }

            this.Select(this.PreviousSelectedIndex, this.PreviousSelectedLength);
            base.OnTextChanged(e);
        }

        /// <summary>
        /// 处理失去焦点事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.Time = TimeSpan.Parse(this.Text);
            base.OnLostFocus(e);
        }

        /// <summary>
        /// 属性变更事件回掉方法。
        /// </summary>
        /// <param name="obj">属性变更的对象</param>
        /// <param name="e">包含属性变更信息的事件参数</param>
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Time":
                    ((TimeBox)obj).Time = (TimeSpan)e.NewValue;
                    break;
            }
        }

        /// <summary>
        /// 处理删除文本的动作。
        /// </summary>
        private void OnBackspace()
        {
            if (this.SelectionStart == 0)
            {
                this.SelectionStart = this.Text.Length - 1;
            }
            else if (this.SelectionStart == 3 ||
                     this.SelectionStart == 6)
            {
                this.SelectionStart -= 2;
            }
            else
            {
                this.SelectionStart -= 1;
            }

            var newText = Text;
            newText = newText.Insert(this.SelectionStart, "0");
            newText = newText.Remove(this.SelectionStart + 1, 1);
            this.PreviousSelectedIndex = this.SelectionStart;
            this.PreviousSelectedLength = this.SelectionLength;
            this.Text = newText;
        }

        /// <summary>
        /// 处理删除文本的动作
        /// </summary>
        private void OnDelete()
        {
            if (this.SelectionStart == 2 ||
                this.SelectionStart == 5)
            {
                this.SelectionStart += 1;
            }
            else if (this.SelectionStart == this.Text.Length)
            {
                this.SelectionStart = 0;
            }

            var newText = this.Text;
            newText = newText.Insert(this.SelectionStart, "0");
            newText = newText.Remove(this.SelectionStart + 1, 1);
            this.PreviousSelectedIndex = this.SelectionStart;
            this.PreviousSelectedLength = this.SelectionLength;
            this.Text = newText;
        }

        /// <summary>
        /// 处理输入数字的动作。
        /// </summary>
        /// <param name="number">指定的数字</param>
        private void OnNumber(string number)
        {
            if (this.SelectionStart == this.Text.Length)
            {
                this.SelectionStart = 0;
            }

            var newText = Text;
            newText = newText.Insert(this.SelectionStart, number);
            newText = newText.Remove(this.SelectionStart + 1, 1);
            if (Regex.IsMatch(newText, this.ParseRule))
            {
                if (this.SelectionStart == 1 ||
                    this.SelectionStart == 4)
                {
                    this.PreviousSelectedIndex = this.SelectionStart + 2;
                    this.PreviousSelectedLength = this.SelectionLength;
                }
                else
                {
                    this.PreviousSelectedIndex = this.SelectionStart + 1;
                    this.PreviousSelectedLength = this.SelectionLength;
                }

                if (this.Text == newText)
                {
                    this.SelectionStart = this.PreviousSelectedIndex;
                    this.SelectionLength = this.PreviousSelectedLength;
                }

                this.Text = newText;
            }
        }

        /// <summary>
        /// 处理输入空格的动作。
        /// </summary>
        private void OnSpace()
        {
            if (this.SelectionStart == this.Text.Length - 1)
            {
                this.SelectionStart = 0;
            }
            else if (this.SelectionStart == 1 ||
                     this.SelectionStart == 4 ||
                     this.SelectionStart == 7)
            {
                this.SelectionStart += 2;
            }
            else
            {
                this.SelectionStart += 1;
            }
        }
    }
}