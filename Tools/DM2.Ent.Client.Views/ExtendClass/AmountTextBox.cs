// <copyright file="AmountTextBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tongly</author>
// <date>2013/10/31 7:49:33</date>
// <modify>
//   修改人：tongly
//   修改时间：2014/02/12 11:00
//   修改描述：解决光标位置问题
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// 显示为:111,111格式的货币文本框，取值Value属性。
    /// 设置DecimalNumber 开启小数点位数验证。
    /// 设置ShowMessage 开启消息提示。
    /// </summary>
    public class AmountTextBox : TextBox
    {
        #region 字段
        /// <summary>
        /// 显示消息依赖项属性
        /// </summary>
        public static readonly DependencyProperty ShowMessageProperty =
            DependencyProperty.Register("ShowMessage", typeof(string), typeof(AmountTextBox), new PropertyMetadata(new PropertyChangedCallback(ShowMessageOnValueChanged)));

        /// <summary>
        /// 小数点位数依赖项属性
        /// </summary>
        public static readonly DependencyProperty DecimalNumberProperty =
            DependencyProperty.Register("DecimalNumber", typeof(int), typeof(AmountTextBox), new PropertyMetadata(2, new PropertyChangedCallback(DecimalNumberOnValueChanged)));

        /// <summary>
        /// 数值依赖项属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
           DependencyProperty.Register("Value", typeof(decimal), typeof(AmountTextBox), new FrameworkPropertyMetadata(0M, new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValueValue)));

        /// <summary>
        /// 数值的最大长度
        /// </summary>
        private static readonly decimal MaxDecimal = 9999999999999999.99m;

        /// <summary>
        /// 是否验证
        /// </summary>
        private bool isCheckingText = false;

        /// <summary>
        /// 是否格式化
        /// </summary>
        private bool isFormattingText = false;

        /// <summary>
        /// 是否设置值
        /// </summary>
        private bool isSettingValue = false;

        /// <summary>
        /// 光标索引
        /// </summary>
        private int lastCaretIndex;

        /// <summary>
        /// 最后验证的值
        /// </summary>
        private decimal lastValidValue;

        /// <summary>
        /// 最后显示的文本
        /// </summary>
        private string lastText = string.Empty;

        /// <summary>
        /// 记录分隔符的数量
        /// </summary>
        private int delimiterNumber = 0;

        /// <summary>
        /// 记录按键
        /// </summary>
        private Key keyNum;

        /// <summary>
        /// 记录逗号增减
        /// </summary>
        private int delimiterStauts = 0;

        /// <summary>
        /// 显示验证
        /// </summary>
        private bool isShowToolTip = false;

        /// <summary>
        /// 是否验证小数位
        /// </summary>
        private bool isCheckDecimal = true;

        #endregion
        /// <summary>
        /// Initializes static members of the <see cref="AmountTextBox"/> class.
        /// </summary>
        static AmountTextBox()
        {
            TextAlignmentProperty.OverrideMetadata(typeof(AmountTextBox), new FrameworkPropertyMetadata(TextAlignment.Right, null, CoerceTextAlignmentValue));
        }

        #region 属性
        /// <summary>
        /// 显示消息属性
        /// </summary>
        public string ShowMessage
        {
            get
            {
                return (string)this.GetValue(ShowMessageProperty);
            }

            set
            {
                this.SetValue(ShowMessageProperty, value);
            }
        }

        /// <summary>
        /// 小数点位数
        /// </summary>
        public int DecimalNumber
        {
            get
            {
                return (int)this.GetValue(DecimalNumberProperty);
            }

            set
            {
                this.SetValue(DecimalNumberProperty, value);
            }
        }

        /// <summary>
        /// 转换后的文本框中的数值
        /// </summary>
        [Category("Common")]
        public decimal Value
        {
            get
            {
                return (decimal)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 扩展值改变通知方法，可以进行扩展
        /// </summary>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = (decimal)e.NewValue;
            this.lastValidValue = newValue;

            if (!this.isFormattingText && !this.isSettingValue)
            {
                this.isSettingValue = true;
                this.Text = newValue == 0 ? string.Empty : this.FormatDecimal(newValue);
                this.lastText = this.Text;

                this.isSettingValue = false;
            }
        }

        /// <summary>
        /// 获取焦点事件
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (this.isShowToolTip)
            {
                this.isShowToolTip = false;
                this.Text = string.Empty;
            }

            base.OnGotFocus(e);
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                if (!this.isShowToolTip)
                {
                    this.isShowToolTip = true;
                    this.Text = this.ShowMessage;
                }
            }

            base.OnLostFocus(e);
        }

        /// <summary>
        /// 选中改变事件
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            ////获取光标位置
            this.lastCaretIndex = this.CaretIndex;
            base.OnSelectionChanged(e);
        }

        /// <summary>
        /// 值改变事件
        /// </summary>
        /// <param name="e">TextChangedEventArgs</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!this.isShowToolTip)
            {
                ////未开启提示，使用在线验证
                this.CheckAndFormatText();
            }

            base.OnTextChanged(e);
        }

        /// <summary>
        /// 重写键盘按下向下路由事件
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            //// 以下是针对输入按键的处理，所以有其他控制键的不处理
            if (Keyboard.Modifiers != ModifierKeys.None)
            {
                return;
            }

            ////记录按键
            this.keyNum = e.Key;

            if (e.Key == Key.Add || e.Key == Key.Subtract)
            {
                e.Handled = true;
            }

            if (e.Key == Key.OemComma)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    e.Handled = true;
                    return;
                }

                var point = this.Text.LastIndexOf('.');

                if (point == 1)
                {
                    e.Handled = true;
                }
                else if (this.Text.Length != this.CaretIndex)
                {
                    e.Handled = true;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// 显示消息依赖项属性
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void ShowMessageOnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AmountTextBox amount = d as AmountTextBox;
            amount.isShowToolTip = true;
            amount.Text = amount.ShowMessage;
        }

        /// <summary>
        /// 小数点位数依赖项属性
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void DecimalNumberOnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AmountTextBox amount = d as AmountTextBox;
            amount.isCheckDecimal = true;
        }

        /// <summary>
        /// Value值改变事件方法
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AmountTextBox)d).OnValueChanged(e);
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="d">依赖对象</param>
        /// <param name="value">最新的值</param>
        /// <returns>返回Object</returns>
        private static object CoerceTextAlignmentValue(DependencyObject d, object value)
        {
            return TextAlignment.Right;
        }

        /// <summary>
        /// 值类型进行转换
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="value">value</param>
        /// <returns>object</returns>
        private static object CoerceValueValue(DependencyObject d, object value)
        {
            var amount = (decimal)value;
            return ((AmountTextBox)d).EnsureValue((decimal)value);
        }

        /// <summary>
        /// 检验并且进行格式化
        /// </summary>
        private void CheckAndFormatText()
        {
            if (this.isCheckingText || this.isFormattingText || this.isSettingValue)
            {
                return;
            }

            var currentText = this.Text;

            if (string.IsNullOrEmpty(currentText))
            {
                this.Value = 0;
                this.lastCaretIndex = 0;
                this.delimiterNumber = 0;
                return;
            }

            decimal value;
            if (this.TryConvertValue(currentText, out value))
            {
                this.isCheckingText = true;
                this.isFormattingText = true;

                string[] val = currentText.Split('.');
                var formattedText = string.Empty;

                if (val[0].Equals(string.Empty) || val[0].Equals(","))
                {
                    formattedText = string.Empty;
                    this.Text = formattedText;
                }
                else
                {
                    formattedText = this.FormatDecimal(decimal.Parse(val[0].Replace(",", string.Empty)));
                }

                if (val.Length == 2)
                {
                    ////说明有小数位置
                    if (this.isCheckDecimal)
                    {
                        if (val[1].Length == this.DecimalNumber + 1)
                        {
                            this.Text = formattedText + "." + val[1].Remove(this.DecimalNumber);
                            this.lastText = formattedText + "." + val[1].Remove(this.DecimalNumber);
                            value = decimal.Parse(this.lastText);
                        }
                        else if (val[1].Length > 2)
                        {
                            this.Text = formattedText + "." + val[1].Remove(this.DecimalNumber);
                            this.lastText = formattedText + "." + val[1].Remove(this.DecimalNumber);
                            value = decimal.Round(Convert.ToDecimal(currentText), this.DecimalNumber);
                        }
                    }
                    else
                    {
                        this.Text = formattedText + "." + val[1];
                        this.lastText = formattedText + "." + val[1];
                    }
                }
                else
                {
                    this.Text = formattedText;
                }

                ////根据逗号获取光标位置

                var tempNumber = formattedText.Split(',');

                if (tempNumber.Length - 1 > this.delimiterNumber)
                {
                    this.delimiterStauts = 1;
                }
                else if (tempNumber.Length - 1 < this.delimiterNumber)
                {
                    this.delimiterStauts = 2;
                }
                else
                {
                    this.delimiterStauts = 0;
                }

                this.delimiterNumber = tempNumber.Length - 1;

                ////验证删除还是增加
                if (this.keyNum != Key.Back && this.keyNum != Key.Delete)
                {
                    switch (this.delimiterStauts)
                    {
                        case 0:
                            this.CaretIndex = this.lastCaretIndex + 1;
                            this.lastText = formattedText;
                            break;
                        case 1:
                            this.CaretIndex = this.lastCaretIndex + 2;
                            break;
                        case 2:

                            if (this.lastCaretIndex != 0)
                            {
                                this.CaretIndex = this.lastCaretIndex;
                            }

                            break;
                    }
                }
                else
                {
                    if (this.lastCaretIndex != 0)
                    {
                        if (this.delimiterStauts == 2)
                        {
                            try
                            {
                                this.CaretIndex = this.lastCaretIndex - 2;
                            }
                            catch
                            {
                                this.CaretIndex = this.CaretIndex;
                            }
                        }
                        else
                        {
                            this.CaretIndex = this.lastCaretIndex - 1;
                        }
                    }
                    else
                    {
                        this.CaretIndex = this.lastCaretIndex;
                    }
                }
                ////设置值
                this.Value = value;
                ////解决二次转换问题
                ////允许小数点输入

                string[] val1 = currentText.Split('.');

                if (val1[0].Equals(string.Empty))
                {
                    this.Text = string.Empty;
                }
                else
                {
                    if (val1.Length == 2)
                    {
                        if (this.isCheckDecimal)
                        {
                            if (val1[1].Length == this.DecimalNumber)
                            {
                                this.Text = formattedText + "." + val1[1];
                                this.lastText = formattedText + "." + val1[1];
                            }
                        }
                        else
                        {
                            this.Text = formattedText + "." + val[1];
                            this.lastText = formattedText + "." + val1[1];
                        }
                    }
                }

                this.isFormattingText = false;
            }
            else
            {
                this.Text = this.lastText;
            }

            this.isCheckingText = false;
        }

        /// <summary>
        /// 格式话输入数据
        /// </summary>
        private void CheckAndFormatTextA()
        {
            if (this.isCheckingText || this.isFormattingText || this.isSettingValue)
            {
                return;
            }

            var currentText = this.Text;

            if (string.IsNullOrEmpty(currentText))
            {
                this.Value = 0;
                this.lastCaretIndex = 0;
                this.delimiterNumber = 0;
                return;
            }

            decimal value;
            if (this.TryConvertValue(currentText, out value))
            {
                this.isCheckingText = true;
                this.isFormattingText = true;
                ////允许小数点输入

                string[] val = currentText.Split('.');
                var formattedText = string.Empty;

                if (val[0].Equals(string.Empty))
                {
                    formattedText = string.Empty;
                }
                else
                {
                    formattedText = this.FormatDecimal(decimal.Parse(val[0].Replace(",", string.Empty)));
                }

                if (val.Length == 2)
                {
                    ////说明有小数位置
                    if (this.isCheckDecimal)
                    {
                        if (val[1].Length == this.DecimalNumber + 1)
                        {
                            this.Text = formattedText + "." + val[1].Remove(this.DecimalNumber);
                            this.lastText = formattedText + "." + val[1].Remove(this.DecimalNumber);

                            value = decimal.Parse(this.lastText);
                        }
                    }
                    else
                    {
                        this.Text = formattedText + "." + val[1];
                        this.lastText = formattedText + "." + val[1];
                    }
                }
                else
                {
                    this.Text = formattedText;
                }

                ////根据逗号获取光标位置

                var tempNumber = formattedText.Split(',');

                if (tempNumber.Length - 1 > this.delimiterNumber)
                {
                    this.delimiterStauts = 1;
                }
                else if (tempNumber.Length - 1 < this.delimiterNumber)
                {
                    this.delimiterStauts = 2;
                }
                else
                {
                    this.delimiterStauts = 0;
                }

                this.delimiterNumber = tempNumber.Length - 1;

                ////验证删除还是增加
                if (this.keyNum != Key.Back && this.keyNum != Key.Delete)
                {
                    switch (this.delimiterStauts)
                    {
                        case 0:
                            this.CaretIndex = this.lastCaretIndex + 1;
                            this.lastText = formattedText;
                            break;
                        case 1:
                            this.CaretIndex = this.lastCaretIndex + 2;
                            break;
                        case 2:

                            if (this.lastCaretIndex != 0)
                            {
                                this.CaretIndex = this.lastCaretIndex;
                            }

                            break;
                    }
                }
                else
                {
                    if (this.lastCaretIndex != 0)
                    {
                        if (this.delimiterStauts == 2)
                        {
                            try
                            {
                                this.CaretIndex = this.lastCaretIndex - 2;
                            }
                            catch
                            {
                                this.CaretIndex = this.CaretIndex;
                            }
                        }
                        else
                        {
                            this.CaretIndex = this.lastCaretIndex - 1;
                        }
                    }
                    else
                    {
                        this.CaretIndex = this.lastCaretIndex;
                    }
                }
                ////设置值
                this.Value = value;
                ////解决二次转换问题
                ////允许小数点输入
                ////string[] val1 = currentText.Split('.');

                ////if (val1[0].Equals(string.Empty))
                ////{
                ////    this.Text = string.Empty;
                ////}
                ////else
                ////{
                ////    if (val1.Length == 2)
                ////    {
                ////        this.Text = this.FormatDecimal(decimal.Parse(val1[0].Replace(",", string.Empty))) + "." + val1[1];
                ////    }
                ////}

                this.isFormattingText = false;
            }
            else
            {
                this.Text = this.lastText;
            }

            this.isCheckingText = false;
        }

        /// <summary>
        /// 验证是否可以转换（判断输入类型)
        /// </summary>
        /// <param name="text">验证的内容</param>
        /// <param name="value">验证通过的数值</param>
        /// <returns>是否验证通过</returns>
        private bool TryConvertValue(string text, out decimal value)
        {
            ////空字符串返回默认值
            if (string.IsNullOrEmpty(text))
            {
                value = decimal.Zero;
                return true;
            }

            ////去掉一些金额符号
            var separator = ".";
            var valueText = text.Replace(",", string.Empty);

            ////如果只剩下小数点，返回默认值
            if (valueText == separator || string.IsNullOrEmpty(valueText))
            {
                value = decimal.Zero;
                return true;
            }

            ////验证是否为小数类型
            var result = decimal.TryParse(valueText, out value);

            ////保证金额不超限
            if (result)
            {
                value = this.EnsureValue(value);
            }

            return result;
        }

        /// <summary>
        /// 验证最大限制值
        /// </summary>
        /// <param name="value">验证的数值</param>
        /// <returns>返回验证的数值</returns>
        private decimal EnsureValue(decimal value)
        {
            return value > AmountTextBox.MaxDecimal ? this.lastValidValue : value;
        }

        /// <summary>
        /// 格式化货币
        /// </summary>
        /// <param name="value">整数部分</param>
        /// <returns>整数格式化，</returns>
        private string FormatDecimal(decimal value)
        {
            ////没有小数点的格式化
            var formatString = string.Format("{0:C0}", value).Substring(1);

            return formatString;
        }

        /// <summary> 
        /// 重写应用模板方法 
        /// </summary> 
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InputMethod.SetIsInputMethodEnabled(this, false);
        }


        #endregion
    }
}
