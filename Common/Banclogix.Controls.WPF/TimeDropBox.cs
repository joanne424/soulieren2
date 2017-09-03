// <copyright file="TimeDropBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/30 8:35:24</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/30 8:35:24
//   修改描述：新建 DropBox.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banclogix.Controls
{
    /// <summary>
    /// 限制时间输入的下拉框控件。
    /// </summary>
    public class TimeDropBox : Primitives.DropBox
    {
        /// <summary>
        /// 用于时间绑定的依赖属性。
        /// </summary>
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(DateTime), typeof(TimeDropBox), new PropertyMetadata(DateTime.MinValue, ValueChanged));

        /// <summary>
        /// 数字。
        /// </summary>
        private static readonly string[] Number = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDropBox" /> class.
        /// </summary>
        public TimeDropBox()
        {
            this.ParseRule = @"^((2[0-3])|((0|1)?\d)):[0-5]?\d$";
            this.Text = "00:00";
            this.ItemsSource = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                ((IList)ItemsSource).Add(i.ToString("00") + ":00");
            }

            ((IList)ItemsSource).Add("23:59");
        }

        /// <summary>
        /// 获取或设置时间。
        /// </summary>
        public DateTime Time
        {
            get
            {
                return (DateTime)GetValue(TimeProperty);
            }

            set
            {
                this.SetValue(TimeProperty, value);
                this.Text = value.ToString(@"HH:mm");
            }
        }

        /// <summary>
        /// 处理键盘事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                return;
            }

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

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// 处理键盘事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = true;
                this.OnNumber(Number[(int)e.Key - 74]);
            }
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                e.Handled = true;
                this.OnNumber(Number[(int)e.Key - 34]);
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// 处理文本改变事件。
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
                this.Time = DateTime.Parse(this.Text);
            }

            this.EditablePart.Select(this.PreviousSelectedIndex, 0);
            base.OnTextChanged(e);
        }

        /// <summary>
        /// 处理失去焦点事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.Time = DateTime.Parse(this.Text);
            base.OnLostFocus(e);
        }

        /// <summary>
        /// 属性变更回掉函数。
        /// </summary>
        /// <param name="obj">属性变更的对象</param>
        /// <param name="e">包含属性变更信息的事件参数</param>
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Time":
                    ((TimeDropBox)obj).Time = (DateTime)e.NewValue;
                    break;
            }
        }

        /// <summary>
        /// 处理 Backspace 按键。
        /// </summary>
        private void OnBackspace()
        {
            if (this.SelectionStart == 0)
            {
                this.SelectionStart = this.Text.Length - 1;
            }
            else if (this.SelectionStart == 3)
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
            this.Text = newText;
        }

        /// <summary>
        /// 处理 Delete 按键。
        /// </summary>
        private void OnDelete()
        {
            if (this.SelectionStart == 2)
            {
                this.SelectionStart += 1;
            }
            else if (this.SelectionStart == this.Text.Length)
            {
                this.SelectionStart = 0;
            }

            var newText = Text;
            newText = newText.Insert(this.SelectionStart, "0");
            newText = newText.Remove(this.SelectionStart + 1, 1);
            this.PreviousSelectedIndex = this.SelectionStart;
            this.Text = newText;
        }

        /// <summary>
        /// 处理数字按键。
        /// </summary>
        /// <param name="number">输入的数字</param>
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
                }
                else
                {
                    this.PreviousSelectedIndex = this.SelectionStart + 1;
                }

                if (this.Text == newText)
                {
                    this.SelectionStart = this.PreviousSelectedIndex;
                }

                this.Text = newText;
            }
        }

        /// <summary>
        /// 处理 Space 按键。
        /// </summary>
        private void OnSpace()
        {
            if (this.SelectionStart == this.Text.Length - 1)
            {
                this.SelectionStart = 0;
            }
            else if (this.SelectionStart == 1 ||
                     this.SelectionStart == 4)
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