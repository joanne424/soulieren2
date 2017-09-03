// <copyright file="NumberBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/27 4:44:56</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/27 4:44:56
//   修改描述：新建 TimeBox.cs
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

namespace Banclogix.Controls.Primitives
{
    ///// <summary>
    ///// 数字输入控件。
    ///// </summary>
    ///// <typeparam name="T">指定的数字类型</typeparam>
    public abstract class NumberBox<T> : EditBox where T : struct, IComparable<T>
    {
        ///// <summary>
        ///// 用于绑定的数字属性。
        ///// </summary>
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number", typeof(T), typeof(NumberBox<T>), new PropertyMetadata(default(T), ValueChanged));

        ///// <summary>
        ///// 用户绑定的格式化属性。
        ///// </summary>
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            "Format", typeof(string), typeof(NumberBox<T>), new PropertyMetadata(null, ValueChanged));

        ///// <summary>
        ///// 用于绑定的允许输入的最小值。
        ///// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(T), typeof(NumberBox<T>), new PropertyMetadata(default(T), ValueChanged));

        ///// <summary>
        ///// 用于绑定的允许输入的最大值。
        ///// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(T), typeof(NumberBox<T>), new PropertyMetadata(default(T), ValueChanged));

        ///// <summary>
        ///// Initializes a new instance of the <see cref="NumberBox{T}" /> class.
        ///// </summary>
        protected NumberBox()
        {
        }

     ///// <summary>
     ///// 是否允许负号。
     ///// </summary>
        public bool IsSigned
        {
            get
            {
                return this.ProtectedIsSigned;
            }

            set
            {
                this.ProtectedIsSigned = value;
                this.SetRule();
            }
        }

     ///// <summary>
     ///// 用于显示的数字格式化。
     ///// </summary>
        public virtual string Format
        {
            get
            {
                return this.Protectedformat;
            }

            set
            {
                if (this.Protectedformat == value)
                {
                    return;
                }

                this.Protectedformat = value;
                this.SetValue(FormatProperty, value);
                this.DisplayFormat();
            }
        }

     ///// <summary>
     ///// 运行输入的最小值。
     ///// </summary>
        public virtual T MinValue
        {
            get
            {
                return this.ProtectedMinValue;
            }

            set
            {
                if (this.ProtectedMinValue.CompareTo(value) == 0)
                {
                    return;
                }

                if (this.ProtectedNumber.CompareTo(value) == -1)
                {
                    this.Number = value;
                }

                this.ProtectedMinValue = value;
            }
        }

     ///// <summary>
     ///// 允许输入的最大值。
     ///// </summary>
        public virtual T MaxValue
        {
            get
            {
                return this.ProtectedMaxValue;
            }

            set
            {
                if (this.ProtectedMaxValue.CompareTo(value) == 0)
                {
                    return;
                }

                if (this.ProtectedNumber.CompareTo(value) == 1)
                {
                    this.Number = value;
                }

                this.ProtectedMaxValue = value;
            }
        }

     ///// <summary>
     ///// 文本输入的数字。
     ///// </summary>
        public virtual T Number
        {
            get
            {
                return this.ProtectedNumber;
            }

            set
            {
                value = this.RangeNumber(value);
                if (this.ProtectedNumber.CompareTo(value) != 0)
                {
                    this.ProtectedNumber = value;
                    this.SetValue(NumberProperty, this.ProtectedNumber);
                }

                this.DisplayFormat();
            }
        }

     ///// <summary>
     ///// 输入规则。
     ///// </summary>
        protected string InputRule { get; set; }

     ///// <summary>
     ///// 文本输入的数字。
     ///// </summary>
        protected T ProtectedNumber { get; set; }

     ///// <summary>
     ///// 运行输入的最小值。
     ///// </summary>
        protected T ProtectedMinValue { get; set; }

     ///// <summary>
     ///// 允许输入的最大值。
     ///// </summary>
        protected T ProtectedMaxValue { get; set; }

     ///// <summary>
     ///// 是否允许负号。
     ///// </summary>
        protected bool ProtectedIsSigned { get; set; }

     ///// <summary>
     ///// 用于显示的数字格式化。
     ///// </summary>
        protected string Protectedformat { get; set; }

     ///// <summary>
     ///// 获取焦点事件。
     ///// </summary>
     ///// <param name="e">事件参数</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.Text = this.Number.ToString();
            base.OnGotFocus(e);
        }

     ///// <summary>
     ///// 失去焦点事件。
     ///// </summary>
     ///// <param name="e">事件参数</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (Regex.IsMatch(this.Text, this.ParseRule))
            {
                this.Number = this.ParseNumber();
            }
            else
            {
                this.Number = default(T);
            }

            base.OnLostFocus(e);
        }

     ///// <summary>
     ///// 编辑框的文本改变事件。
     ///// </summary>
     ///// <param name="e">事件参数</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
            }
            else if (this.Text == this.FormatNumber())
            {
            }
            else if (Regex.IsMatch(this.Text, this.InputRule))
            {
                if (!Regex.IsMatch(this.Text, this.ParseRule))
                {
                    return;
                }

                var number = this.ParseNumber();
                if (number.CompareTo(this.ProtectedMinValue) > -1 &&
                    number.CompareTo(this.ProtectedMaxValue) < 1 &&
                    number.CompareTo(this.ProtectedNumber) != 0)
                {
                    this.ProtectedNumber = number;
                    this.SetValue(NumberProperty, this.ProtectedNumber);
                }
            }
            else
            {
                this.Text = this.PreviousText;
                this.Select(this.PreviousSelectedIndex, this.PreviousSelectedLength);
            }

            base.OnTextChanged(e);
        }

     ///// <summary>
     ///// 用于显示数字的格式化。
     ///// </summary>
        protected void DisplayFormat()
        {
            if (!this.IsFocused)
            {
                var text = this.FormatNumber();
                if (text == this.Text)
                {
                    return;
                }

                this.Text = text;
            }
        }

     ///// <summary>
     ///// 设置输入规则。
     ///// </summary>
        protected abstract void SetRule();

     ///// <summary>
     ///// 转换输入的文本到数字。
     ///// </summary>
     ///// <returns>返回转换后的数字</returns>
        protected abstract T ParseNumber();

     ///// <summary>
     ///// 格式化数字。
     ///// </summary>
     ///// <returns>返回格式化的字符串</returns>
        protected abstract string FormatNumber();

     ///// <summary>
     ///// 限制数字的范围。
     ///// </summary>
     ///// <param name="value">指定的</param>
     ///// <returns>如果数字在不在范围内，那么返回允许的最小值</returns>
        protected T RangeNumber(T value)
        {
            if (value.CompareTo(this.ProtectedMinValue) == -1)
            {
                value = this.ProtectedMinValue;
            }
            else if (value.CompareTo(this.ProtectedMaxValue) == 1)
            {
                value = this.ProtectedMaxValue;
            }

            return value;
        }

     ///// <summary>
     ///// 属性变更时触发。
     ///// </summary>
     ///// <param name="obj">属性值变化的对象</param>
     ///// <param name="e">事件参数</param>
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Format":
                    ((NumberBox<T>)obj).Format = (string)e.NewValue;
                    break;
                case "Number":
                    ((NumberBox<T>)obj).Number = (T)e.NewValue;
                    break;
                case "MinValue":
                    ((NumberBox<T>)obj).MinValue = (T)e.NewValue;
                    break;
                case "MaxValue":
                    ((NumberBox<T>)obj).MaxValue = (T)e.NewValue;
                    break;
            }
        }
    }
}