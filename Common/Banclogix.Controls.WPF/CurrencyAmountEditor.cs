// <copyright file="CurrencyAmountEditor.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2016/2/26 17:26:09 </date>
// <summary></summary>
// <modify>
//      修改人：donggj
//      修改时间：2016/2/26 17:26:09
//      修改描述：新建 CurrencyAmountEditor
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banclogix.Controls
{
    /// <summary> 
    ///  货币金额输入框自定义控件
    /// </summary> 
    public class CurrencyAmountEditor : TextBox
    {
        /// <summary>
        /// 上次输入的值
        /// </summary>
        private decimal lastValue;

        /// <summary>
        /// 上次光标位置
        /// </summary>
        private int lastCaretIndex;

        /// <summary>
        /// 小数位数
        /// </summary>
        public int AmountDecimal
        {
            get { return (int)GetValue(AmountDecimalProperty); }
            set { SetValue(AmountDecimalProperty, value); }
        }

        /// <summary>
        /// 小数位数
        /// </summary>
        public static readonly DependencyProperty AmountDecimalProperty =
            DependencyProperty.Register("AmountDecimal", typeof(int), typeof(CurrencyAmountEditor), new PropertyMetadata(0));

        /// <summary>
        /// 用于后台VM中绑定输入的数值
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal), typeof(CurrencyAmountEditor),
                new FrameworkPropertyMetadata(0M,
                    new PropertyChangedCallback(OnValueChanged),
                    new CoerceValueCallback(CoerceValueValue)));

        /// <summary>
        /// 用于后台VM中绑定输入的数值
        /// </summary>
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CurrencyAmountEditor)d).OnValueChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.isSetValueByTextChanged == true)
            {
                return;
            }

            this.lastValue = (decimal)e.NewValue;

            this.Text = ((decimal)e.NewValue).ToString();

            this.FormatTextAndSetValue();
        }

        /// <summary>
        /// Coerces the Value value.
        /// </summary>
        private static object CoerceValueValue(DependencyObject d, object value)
        {
            var amount = (decimal)value;
            ////if (amount.Equals(0M))
            ////    return GetDefaultDecimal((CultureInfo)d.GetValue(CultureProperty));

            return ((CurrencyAmountEditor)d).EnsureValue((decimal)value);
        }

        #region OnPreviewKeyDown

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // 以下是针对输入按键的处理，所以有其他控制键的不处理
            if (Keyboard.Modifiers != ModifierKeys.None)
            {
                return;
            }

            // 输入逗号则过滤
            if (e.Key == Key.OemComma)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                // 如果是按下小数点，并且金额带小数点，则把光标挪到小数点后
                if (this.AmountDecimal > 0)
                {
                    int curCaretIndex = this.GetDecimalSepIndex(this.Text);

                    if (curCaretIndex != -1)
                    {
                        this.CaretIndex = GetDecimalSepIndex(this.Text) + 1;
                    }
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                // 如果是按下删除键
                var currentCaretIndex = this.CaretIndex;

                // 如果有小数点
                if (this.AmountDecimal > 0)
                {
                    var pointIndex = this.GetDecimalSepIndex(this.Text);
                    if (currentCaretIndex == pointIndex)
                    {
                        // 如果现在光标在小数点之前，则挪到小数点之后
                        this.CaretIndex = pointIndex + 1;
                        e.Handled = true;
                    }
                    else if (currentCaretIndex == pointIndex + 1)
                    {
                        // 如果光标在小数点之后，则允许删除，但是光标还是要保留在小数点之后
                        // 这里用异步的处理，让文本先删除，再执行光标移动
                        this.AsyncSetCaretIndex(pointIndex + 1);
                    }
                }
            }
            else if (e.Key == Key.Back)
            {
                // 如果是按下向前删除键
                var currentCaretIndex = this.CaretIndex;

                // 如果有小数点
                if (this.AmountDecimal > 0)
                {
                    var pointIndex = this.GetDecimalSepIndex(this.Text);
                    if (currentCaretIndex == pointIndex + 1)
                    {
                        // 如果现在光标在小数点之后，则挪到小数点之前
                        this.CaretIndex = pointIndex == -1 ? 0 : pointIndex;

                        if (pointIndex == -1)
                        {
                            this.Text = "0";
                            this.Value = decimal.Zero;
                        }

                        e.Handled = true;
                    }
                    else if (currentCaretIndex > pointIndex + 1)
                    {
                        // 如果光标在小数点后面的文本中，则记录当前的光标位置
                        lastCaretIndex = this.CaretIndex;
                    }
                }
            }
            else if (e.Key == Key.Left)
            {
                lastCaretIndex = this.CaretIndex;
            }
            else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                // 这里不允许输入负号
                e.Handled = true;
            }
            else if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                lastCaretIndex = this.CaretIndex;
            }
            else
            {
                // 处理正常输入的情况
                lastCaretIndex = CaretIndex;
            }

            base.OnPreviewKeyDown(e);
        }

        #endregion

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            this.FormatTextAndSetValue();
            base.OnTextChanged(e);
        }

        /// <summary>
        /// 格式化文本并且赋值Value
        /// </summary>
        private void FormatTextAndSetValue()
        {
            var curText = this.Text;

            ////System.Diagnostics.Debug.WriteLine("This.Text is " + curText);



            // 移除文本中的逗号
            curText = curText.Replace(",", string.Empty);

            decimal convertRst;
            if (decimal.TryParse(curText, out convertRst) == false)
            {
                if (string.IsNullOrEmpty(curText) == true)
                {
                    this.Value = decimal.Zero;
                }

                return;
            }
            else
            {
                this.FormatText(convertRst);
                decimal temp = decimal.Zero;

                if (decimal.TryParse(this.Text, out temp))
                {
                    this.isSetValueByTextChanged = true;
                    this.Value = temp;
                    this.isSetValueByTextChanged = false;
                }
            }
        }

        /// <summary>
        /// 是否是因为Text属性变更引起的Value值的变化
        /// </summary>
        private bool isSetValueByTextChanged = false;

        // 保证金额不超限
        private decimal EnsureValue(decimal value)
        {
            // 大于decimal的最大值或小于0，即代表输入的数值超出范围，则显示上次的值
            return (value > decimal.MaxValue || value < decimal.Zero) ? lastValue : value;
        }

        // 获取文本中小数点的位置
        private int GetDecimalSepIndex(string text)
        {
            var decimalSepartor = ".";
            return text.LastIndexOf(decimalSepartor);
        }

        // 异步设置光标位置
        private void AsyncSetCaretIndex(int caretIndex)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.CaretIndex = caretIndex;
            }));
        }

        /// <summary>
        /// 格式化文本
        /// </summary>
        /// <param name="value">输入的文本（去掉文本中的逗号之后转成decimal的数值）</param>
        private void FormatText(decimal value)
        {
            ////System.Diagnostics.Debug.WriteLine("this.Text is " + this.Text);

            // 数值（意思是不包括逗号）中小数点的索引位置
            int dotIndex = value.ToString().IndexOf(".");

            // 数值（意思是不包括逗号）的整数部分长度
            int intTxtLenth = 0;

            if (dotIndex < 0)
            {
                intTxtLenth = value.ToString().Length;
            }
            else
            {
                intTxtLenth = dotIndex;
            }

            if (intTxtLenth <= 3)
            {
                int curCaretIndex = this.CaretIndex;
                int newCaretIndex = curCaretIndex;
                // 格式化小数部分
                var formattedText = this.Text;

                // 如果一开始就剩下小数点了，则在小数点前面补0
                if (dotIndex == 0 || this.Text.IndexOf(".") == 0)
                {
                    formattedText = "0" + formattedText;
                }

                int commaIndex = formattedText.IndexOf(",");
                if (commaIndex != -1)
                {
                    if (curCaretIndex > commaIndex)
                    {
                        newCaretIndex--;
                    }

                    formattedText = formattedText.Replace(",", string.Empty);
                }

                formattedText = this.FormatFloatText(formattedText);

                if (formattedText.Equals(this.Text) == false)
                {
                    this.Text = formattedText;

                    this.AsyncSetCaretIndex(newCaretIndex);
                    return;
                }
            }
            else
            {
                // 整数部分除以3之后剩余几位
                int leftDecimals = intTxtLenth % 3;

                // 所需添加逗号的个数
                int dotNumbers = intTxtLenth / 3;

                // 如果整数部分可以被3整除，代表添加逗号的个数等于商数减1
                if (leftDecimals == 0)
                {
                    dotNumbers = dotNumbers - 1;
                }

                if (dotNumbers < 1)
                {
                    return;
                }

                // 当前光标索引位置
                int curCaretIndex = this.CaretIndex;
                ////System.Diagnostics.Debug.WriteLine(this.Text);
                ////System.Diagnostics.Debug.WriteLine("curCaretIndex is " + curCaretIndex);

                #region 格式化整数部分
                StringBuilder sb = new StringBuilder(value.ToString());
                for (int i = 0; i < dotNumbers; i++)
                {
                    if (leftDecimals == 0)
                    {
                        sb.Insert(i * 3 + i + 3, ",");
                    }
                    else
                    {
                        sb.Insert(i * 3 + leftDecimals + i, ",");
                    }
                }
                #endregion

                var formatText = sb.ToString();

                // 格式化小数部分
                formatText = this.FormatFloatText(formatText);

                #region 计算格式化前和格式化后光标位置的偏移量，好用于重置光标的位置
                // 光标偏差位置
                int pos = 0;

                var unformatText = this.Text;


                var unformatTextDotIndex = unformatText.IndexOf(".");
                var formatTextDotIndex = formatText.IndexOf(".");

                // 如果没有小数点，那么取文本的长度
                if (dotIndex < 0)
                {
                    unformatTextDotIndex = unformatText.Length;
                    formatTextDotIndex = formatText.Length;
                }

                if (curCaretIndex <= unformatTextDotIndex)
                {
                    pos = formatTextDotIndex - (unformatTextDotIndex - curCaretIndex) - curCaretIndex;
                }
                #endregion

                this.Text = formatText;

                // 重置格式化后文本中的光标位置
                if (curCaretIndex != this.CaretIndex)
                {
                    this.AsyncSetCaretIndex(curCaretIndex + pos);
                }
            }
        }

        /// <summary>
        /// 格式化小数部分，现在是把超过位数的部分直接截断
        /// </summary>
        /// <param name="formattedText">要格式化小数部分的文本文字</param>
        /// <returns>返回格式化小数部分后的文本文字</returns>
        private string FormatFloatText(string formattedText)
        {
            var dotIndex = formattedText.IndexOf(".");

            // 没有小数点
            if (dotIndex < 0)
            {
                ////System.Diagnostics.Debug.WriteLine("this.AmountDecimal is " + this.AmountDecimal);
                string zeroText = new string(Enumerable.Repeat('0', this.AmountDecimal).ToArray());

                if (string.IsNullOrEmpty(zeroText) == false)
                {
                    formattedText = string.Format("{0}.{1}", formattedText, zeroText);
                }
            }
            else
            {
                // 小数部分的长度
                int floatCount = formattedText.Length - (formattedText.IndexOf(".") + 1);

                // 小数位数多余设置的值，就截断
                if (floatCount > this.AmountDecimal)
                {
                    formattedText = formattedText.Substring(0, dotIndex + this.AmountDecimal + 1);
                }

                // 小数位数少余设置的值，就补0
                if (floatCount < this.AmountDecimal)
                {
                    string zeroText = new string(Enumerable.Repeat('0', this.AmountDecimal - floatCount).ToArray());
                    formattedText = string.Format("{0}{1}", formattedText, zeroText);
                }
            }

            return formattedText;
        }
    }
}
