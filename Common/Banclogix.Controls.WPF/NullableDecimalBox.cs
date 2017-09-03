// <copyright file="NullableDecimalBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2014/8/14 10:44:54 </date>
// <modify>
//   修改人：donggj
//   修改时间：2014/8/14 10:44:54
//   修改描述：新建 NullableDecimalBox
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

using Banclogix.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Banclogix.Controls
{
    /// <summary>
    /// 可空的数字输入控件。
    /// </summary>
    public class NullableDecimalBox : EditBox
    {
        #region 字段
        #region 依赖属性

        /// <summary>
        /// 用于绑定的数字属性。
        /// </summary>
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number", typeof(string), typeof(NullableDecimalBox), new PropertyMetadata(default(string), ValueChanged));

        /// <summary>
        /// 用户绑定的格式化属性。
        /// </summary>
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            "Format", typeof(string), typeof(NullableDecimalBox), new PropertyMetadata(default(string), ValueChanged));

        /// <summary>
        /// 用于绑定的允许输入的最小值。
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(string), typeof(NullableDecimalBox), new PropertyMetadata(default(string), ValueChanged));

        /// <summary>
        /// 用于绑定的允许输入的最大值。
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(string), typeof(NullableDecimalBox), new PropertyMetadata(default(string), ValueChanged));

        /// <summary>
        /// 用于绑定的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
            "Point", typeof(int), typeof(NullableDecimalBox), new PropertyMetadata(2, ValueChanged));
        #endregion

        /// <summary>
        /// 文本输入的数字。
        /// </summary>
        private string protectedNumber = string.Empty;

        /// <summary>
        /// 运行输入的最小值。
        /// </summary>
        private string protectedMinValue = "0";

        /// <summary>
        /// 允许输入的最大值。
        /// </summary>
        private string protectedMaxValue = "0";

        /// <summary>
        /// 允许的输入的小数位。
        /// </summary>
        private int point;

        /// <summary>
        /// 是否允许为空
        /// </summary>
        private bool allowNull;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimalBox" /> class.
        /// </summary>
        public NullableDecimalBox()
        {
            this.MinValue = Convert.ToString(decimal.MinValue);
            this.MaxValue = Convert.ToString(decimal.MaxValue);

            this.Point = 2;
            this.Format = "#,##0.00";
        }

        #region 属性

        /// <summary>
        /// 是否允许为空,备注：不为空的情况还没测会不会有问题
        /// </summary>
        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }

            set
            {
                this.allowNull = value;

                if (this.allowNull == true)
                {
                    this.protectedNumber = string.Empty;
                }
                else
                {
                    this.protectedNumber = "0";
                }
            }
        }

        /// <summary>
        /// 是否允许负号。
        /// </summary>
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

        /// <summary>
        /// 用于显示的数字格式化。
        /// </summary>
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

        /// <summary>
        /// 运行输入的最小值。
        /// </summary>
        public virtual string MinValue
        {
            get
            {
                return this.protectedMinValue;
            }

            set
            {
                if (this.StringToDecimalCompare(this.protectedMinValue, value) == 0)
                {
                    return;
                }

                if (this.StringToDecimalCompare(this.protectedNumber, value) == -1)
                {
                    this.Number = value;
                }

                this.protectedMinValue = value;
            }
        }

        /// <summary>
        /// 允许输入的最大值。
        /// </summary>
        public virtual string MaxValue
        {
            get
            {
                return this.protectedMaxValue;
            }

            set
            {
                if (this.StringToDecimalCompare(this.protectedMaxValue, value) == 0)
                {
                    return;
                }

                if (this.StringToDecimalCompare(this.protectedNumber, value) == 1)
                {
                    this.Number = value;
                }

                this.protectedMaxValue = value;
            }
        }

        /// <summary>
        /// 文本输入的数字。
        /// </summary>
        public virtual string Number
        {
            get
            {
                return this.protectedNumber;
            }

            set
            {
                value = this.RangeNumber(value);
                if (this.StringToDecimalCompare(this.protectedNumber, value) != 0)
                {
                    this.protectedNumber = value;
                    this.SetValue(NumberProperty, this.protectedNumber);
                }

                this.DisplayFormat();
            }
        }

        /// <summary>
        /// 允许的输入的小数位。
        /// </summary>
        public int Point
        {
            get
            {
                return this.point;
            }

            set
            {
                if (this.point == value)
                {
                    return;
                }

                this.point = value;
                this.SetValue(PointProperty, value);
                this.SetRule();
            }
        }

        /// <summary>
        /// 输入规则。
        /// </summary>
        protected string InputRule { get; set; }

        /// <summary>
        /// 是否允许负号。
        /// </summary>
        protected bool ProtectedIsSigned { get; set; }

        /// <summary>
        /// 用于显示的数字格式化。
        /// </summary>
        protected string Protectedformat { get; set; }

        #endregion

        /// <summary>
        /// 获取焦点事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.Text = this.Number.ToString();
            base.OnGotFocus(e);
        }

        #region 受保护的方法

        /// <summary>
        /// 失去焦点事件。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                decimal rst = decimal.Zero;

                decimal.TryParse(this.Text, out rst);

                if (Regex.IsMatch(Convert.ToString(rst), this.ParseRule))
                {
                    this.Number = this.ParseNumber();
                }
                else
                {
                    this.Number = string.Empty;
                }
            }
            else
            {
                // 如果this.Text为空的话，Number也必须为空；如果不为空则要转换成decimal
                this.Number = string.Empty;
            }

            base.OnLostFocus(e);
        }

        /// <summary>
        /// 编辑框的文本改变事件。
        /// </summary>
        /// <param name="e">事件参数</param>
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
                if (this.StringToDecimalCompare(number, this.protectedMinValue) > -1 &&
                    this.StringToDecimalCompare(number, this.protectedMaxValue) < 1 &&
                    this.StringToDecimalCompare(number, this.protectedNumber) != 0)
                {
                    this.protectedNumber = number;
                    this.SetValue(NumberProperty, this.protectedNumber);
                }
            }
            else
            {
                this.Text = this.PreviousText;
                this.Select(this.PreviousSelectedIndex, this.PreviousSelectedLength);
            }

            base.OnTextChanged(e);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 属性变更时触发。
        /// </summary>
        /// <param name="obj">属性值变化的对象</param>
        /// <param name="e">事件参数</param>
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Format":
                    ((NullableDecimalBox)obj).Format = (string)e.NewValue;
                    break;
                case "Number":
                    ((NullableDecimalBox)obj).Number = (string)e.NewValue;
                    break;
                case "MinValue":
                    ((NullableDecimalBox)obj).MinValue = (string)e.NewValue;
                    break;
                case "MaxValue":
                    ((NullableDecimalBox)obj).MaxValue = (string)e.NewValue;
                    break;
                case "Point":
                    ((NullableDecimalBox)obj).Point = (int)e.NewValue;
                    break;
            }
        }

        /// <summary>
        /// 用于显示数字的格式化。
        /// </summary>
        private void DisplayFormat()
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

        /// <summary>
        /// 设置文本规则。
        /// </summary>
        private void SetRule()
        {
            if (this.IsSigned)
            {
                this.InputRule = string.Format(@"^(\-|\+)?((\d*)|(\d+\.\d{0}))$", "{0," + this.point + "}");
                this.ParseRule = string.Format(@"(\-|\+)?\d+(\.\d{0})?$", "{1," + this.point + "}");
                this.MinValue = Convert.ToString(decimal.MinValue);
            }
            else
            {
                this.InputRule = string.Format(@"^((\d*)|(\d+\.\d{0}))$", "{0," + this.point + "}");
                this.ParseRule = string.Format(@"\d+(\.\d{0})?$", "{1," + this.point + "}");
                this.MinValue = Convert.ToString(decimal.Zero);
            }
        }

        /// <summary>
        /// 将输入的文本转换为数字。
        /// </summary>
        /// <returns>返回转换后的数字</returns>
        private string ParseNumber()
        {
            decimal rst = decimal.Zero;

            decimal.TryParse(this.AutoComplete(), out rst);

            return Convert.ToString(rst);
        }

        /// <summary>
        /// 格式化当前数字。
        /// </summary>
        /// <returns>返回格式化之后的数字</returns>
        private string FormatNumber()
        {
            decimal rst = decimal.Zero;

            if (decimal.TryParse(this.Number, out rst))
            {
                return rst.ToString(this.Format);
            }
            else
            {
                return this.Number;
            }
        }

        /// <summary>
        /// 自动补 0。
        /// </summary>
        /// <returns>返回补 0 之后的文本</returns>
        private string AutoComplete()
        {
            var text = Text;
            if (string.IsNullOrEmpty(text))
            {
                text = "0";
            }

            var index = text.IndexOf('.');
            var point = this.Point - (text.Length - (index + 1));
            if (point == 0)
            {
                return text;
            }

            var number = new StringBuilder(Text);
            if (index == -1)
            {
                number.Append(".");
                point = this.Point;
            }

            while (point-- > 0)
            {
                number.Append("0");
            }

            return number.ToString();
        }

        /// <summary>
        /// 限制数字的范围。
        /// </summary>
        /// <param name="value">指定的</param>
        /// <returns>如果数字在不在范围内，那么返回允许的最小值</returns>
        private string RangeNumber(string value)
        {
            if (this.StringToDecimalCompare(value, this.protectedMinValue) == -1)
            {
                value = this.protectedMinValue;
            }
            else if (this.StringToDecimalCompare(value, this.protectedMaxValue) == 1)
            {
                value = this.protectedMaxValue;
            }

            return value;
        }

        /// <summary>
        /// 把string 转换成 decimal 然后再比较大小
        /// </summary>
        /// <param name="strA">A值</param>
        /// <param name="strB">B值</param>
        /// <returns>返回比较结果，如果A等于B返回0，A小于B返回-1，A大于B返回1</returns>
        private int StringToDecimalCompare(string strA, string strB)
        {
            decimal rstA = decimal.Zero;
            decimal rstB = decimal.Zero;

            decimal.TryParse(strA, out rstA);
            decimal.TryParse(strB, out rstB);

            if (rstA == rstB)
            {
                return 0;
            }
            else if (rstA < rstB)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        #endregion
    }
}
