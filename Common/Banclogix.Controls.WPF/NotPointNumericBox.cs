// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotPointNumericBox.cs" company="">
// </copyright>
// <author>wangrx</author>
// <date>2016/05/25 9:29:35</date>
// <modify>
//   修改人：wangrx
//   修改时间：2016/05/25 9:29:35
//   修改描述：新建 NotPointNumericBox.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace Banclogix.Controls
{
    #region

    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    #endregion

    /// <summary>
    ///     数字输入框自定义控件
    /// </summary>
    public class NotPointNumericBox : TextBox
    {
        #region Static Fields

        /// <summary>
        /// The amount property.
        /// </summary>
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            "Amount", 
            typeof(decimal), 
            typeof(NotPointNumericBox), 
            new PropertyMetadata(decimal.Zero, OnAmountPropertyChanged));

        /// <summary>
        /// The is allow negative property.
        /// </summary>
        public static readonly DependencyProperty IsAllowNegativeProperty =
            DependencyProperty.Register(
                "IsAllowNegative", 
                typeof(bool), 
                typeof(NotPointNumericBox), 
                new PropertyMetadata(false, OnIsAllowNegativePropertyChanged));

        /// <summary>
        /// The max value property.
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", 
            typeof(decimal), 
            typeof(NotPointNumericBox), 
            new PropertyMetadata(decimal.MaxValue / 10, OnMaxValuePropertyChanged));

        /// <summary>
        /// The min value property.
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", 
            typeof(decimal), 
            typeof(NotPointNumericBox), 
            new PropertyMetadata(decimal.MinValue / 10, OnMinValuePropertyChanged));

        #endregion

        #region Fields

        /// <summary>
        ///     记录当前按键发生时CaretIndex的值
        /// </summary>
        private int caretOnPreviewKeyDown;

        /// <summary>
        ///     记录当前按键
        /// </summary>
        private Key currentPressKey = Key.None;

        /// <summary>
        ///     指示是否由Text变化引发赋值操作
        /// </summary>
        private bool isByTextChanged;

        /// <summary>
        ///     记录上一次Amount的值
        /// </summary>
        private decimal lastAmountValue = decimal.Zero;

        /// <summary>
        ///     记录上一次光标位置
        /// </summary>
        private int lastCaretIndex;

        /// <summary>
        ///     记录上一次Text的值
        /// </summary>
        private string lastTextValue = string.Empty;

        #endregion

        #region Enums

        /// <summary>
        ///     舍入方式
        /// </summary>
        public enum RoundingEmun
        {
            /// <summary>
            ///     四舍五入
            /// </summary>
            Rounding = 0, 

            /// <summary>
            ///     直接舍去
            /// </summary>
            CutOff = 1, 
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     TextAmount
        /// </summary>
        [Description("数额")]
        [Bindable(true, BindingDirection.TwoWay)]
        public decimal Amount
        {
            get
            {
                return (decimal)this.GetValue(AmountProperty);
            }

            set
            {
                this.SetValue(AmountProperty, value);
            }
        }

        /// <summary>
        ///     是否允许输入负数
        /// </summary>
        [Description("是否允许输入负数")]
        [Category("输入限定设置")]
        public bool IsAllowNegative
        {
            get
            {
                return (bool)this.GetValue(IsAllowNegativeProperty);
            }

            set
            {
                this.SetValue(IsAllowNegativeProperty, value);
            }
        }

        /// <summary>
        ///     允许输入的最大值
        /// </summary>
        [Description("允许输入的最大值")]
        [Category("输入限定设置")]
        public decimal MaxValue
        {
            get
            {
                return (decimal)this.GetValue(MaxValueProperty);
            }

            set
            {
                this.SetValue(MaxValueProperty, value);
            }
        }

        /// <summary>
        ///     允许输入的最小值
        /// </summary>
        [Description("允许输入的最小值")]
        [Category("输入限定设置")]
        public decimal MinValue
        {
            get
            {
                return (decimal)this.GetValue(MinValueProperty);
            }

            set
            {
                this.SetValue(MinValueProperty, value);
            }
        }

        /// <summary>
        ///     重写过的Text属性
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Bindable(false)]
        public new string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);

                // return base.Text;
            }

            set
            {
                // base.Text = value;
                this.SetValue(TextProperty, value);
                this.lastTextValue = this.Text;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 格式化Decimal
        /// </summary>
        /// <param name="amountTemp">
        /// Decimal值
        /// </param>
        /// <param name="decimals">
        /// 指定有效小数位数
        /// </param>
        /// <param name="roundType">
        /// 指定舍入方式
        /// </param>
        /// <param name="withBracket">
        /// 指示负数是否加括号
        /// </param>
        /// <returns>
        /// 格式化后的值
        /// </returns>
        public static string FormatAmount(
            decimal amountTemp, 
            int decimals, 
            RoundingEmun roundType, 
            bool withBracket = false)
        {
            return amountTemp.ToString();
        }

        /// <summary>
        /// 将Text转到Decimal
        /// </summary>
        /// <param name="text">
        /// 指定Text值
        /// </param>
        /// <param name="isAllowNegative">
        /// 指示是否允许负数
        /// </param>
        /// <returns>
        /// decimal值
        /// </returns>
        public static decimal ToDecimal(string text, bool isAllowNegative)
        {
            decimal d = decimal.Zero;
            if (string.IsNullOrEmpty(text))
            {
                return d;
            }

            var result = new StringBuilder();
            char point = '.';
            char firstChar = '+';
            int pointIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (i == 0)
                {
                    firstChar = c;
                    if (c == point)
                    {
                        result.Append('0');
                    }
                    else if (c == '-')
                    {
                        int nextIndex = i + 1;
                        if (nextIndex < text.Length)
                        {
                            if (text[nextIndex] == point)
                            {
                                result.Append('0');
                            }
                        }
                    }
                }

                if (char.IsDigit(c))
                {
                    result.Append(c);
                }
                else if (c == point)
                {
                    pointIndex = result.Length;
                }
            }

            if (0 < pointIndex && pointIndex < result.Length)
            {
                result.Insert(pointIndex, point);
            }

            string str = result.ToString();
            decimal.TryParse(str, out d);
            if (isAllowNegative && firstChar == '-')
            {
                d = decimal.Negate(d);
            }

            return d;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on amount property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void OnAmountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var NotPointNumericBox = d as NotPointNumericBox;
            if (NotPointNumericBox != null)
            {
                int currentCaretIndex = NotPointNumericBox.CaretIndex;
                if (!NotPointNumericBox.isByTextChanged)
                {
                    decimal newValue = Convert.ToDecimal(e.NewValue);
                    NotPointNumericBox.Text = newValue.ToString();
                }
            }
        }

        /// <summary>
        /// The on decimals property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void OnDecimalsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTextBySetingChanged(d, ref e);
        }

        /// <summary>
        /// The on is allow negative property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void OnIsAllowNegativePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTextBySetingChanged(d, ref e);
        }

        /// <summary>
        /// The on max value property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void OnMaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTextBySetingChanged(d, ref e);
        }

        /// <summary>
        /// The on min value property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void OnMinValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTextBySetingChanged(d, ref e);
        }

        /// <summary>
        /// The on rounding type property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void OnRoundingTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateTextBySetingChanged(d, ref e);
        }

        /// <summary>
        /// 当格式化配置的依赖项属性变化后更新Text
        /// </summary>
        /// <param name="d">
        /// 依赖对象
        /// </param>
        /// <param name="e">
        /// 参数
        /// </param>
        protected static void UpdateTextBySetingChanged(DependencyObject d, ref DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var NotPointNumericBox = d as NotPointNumericBox;
            if (NotPointNumericBox != null)
            {
                NotPointNumericBox.UpdateText(true);
            }
        }

        /// <summary>
        /// OnPreviewKeyDown方法重写
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            int currentCaretIndex = this.CaretIndex;
            this.caretOnPreviewKeyDown = this.CaretIndex;
            this.currentPressKey = e.Key;
            if (Keyboard.Modifiers != ModifierKeys.None)
            {
            }

            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                int pointIndex = this.GetDecimalPointIndex(this.Text);
                if (pointIndex > 0 && this.SelectedText != this.Text)
                {
                    this.CaretIndex = pointIndex + 1;
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                // || e.Key == Key.ImeProcessed)
                if (!this.IsAllowNegative)
                {
                    e.Handled = true;
                }
                else
                {
                    int daf = this.CaretIndex;
                    if (!string.IsNullOrEmpty(this.Text))
                    {
                        if (this.SelectedText != this.Text)
                        {
                            if (this.CaretIndex != 0)
                            {
                                e.Handled = true;
                                return;
                            }

                            if (this.Text[0] == '-')
                            {
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
            else if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                if (!string.IsNullOrEmpty(this.SelectedText))
                {
                    return;
                }

                char? deleteChar = this.GetCharByIndex(this.Text, currentCaretIndex - 1);
                if (deleteChar.HasValue)
                {
                    if (deleteChar.Value == '.' || deleteChar.Value == ',')
                    {
                        this.CaretIndex = currentCaretIndex - 1;
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.Delete)
            {
                if (!string.IsNullOrEmpty(this.SelectedText))
                {
                    return;
                }

                char? deleteChar = this.GetCharByIndex(this.Text, currentCaretIndex);
                if (deleteChar.HasValue)
                {
                    if (deleteChar.Value == '.' || deleteChar.Value == ',')
                    {
                        this.CaretIndex = currentCaretIndex + 1;
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// OnTextChanged方法重写
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            this.UpdateText();
            base.OnTextChanged(e);
        }

        /// <summary>
        /// 获取文本中指定的位置字符
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="char?"/>.
        /// </returns>
        private char? GetCharByIndex(string text, int index)
        {
            if (text == null)
            {
                return null;
            }

            if (index < 0 || index > text.Length - 1)
            {
                return null;
            }

            return text[index];
        }

        /// <summary>
        /// 获取文本中小数点的位置
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetDecimalPointIndex(string text)
        {
            string decimalSepartor = ".";
            return text.LastIndexOf(decimalSepartor);
        }

        /// <summary>
        /// 更新Text
        /// </summary>
        /// <param name="byFormaterChanged">
        /// 指示是否由格式化配置变更引发
        /// </param>
        private void UpdateText(bool byFormaterChanged = false)
        {
            try
            {
                int currentCaretIndex = this.CaretIndex;
                string lastValueTemp = this.lastTextValue;
                if (byFormaterChanged == false && lastValueTemp == this.Text)
                {
                    return;
                }

                if (string.IsNullOrEmpty(this.Text))
                {
                    return;
                }

                if (this.IsAllowNegative)
                {
                    if (this.Text == "-" || this.Text == "-0" || this.Text == "-0.")
                    {
                        return;
                    }
                }

                int inputIndex = this.CaretIndex - 1;
                if (inputIndex > this.Text.Length - 1)
                {
                    inputIndex = this.Text.Length - 1;
                }

                if (inputIndex < 0)
                {
                    inputIndex = 0;
                }

                char inputChar = this.Text[inputIndex];
                decimal amountTemp = ToDecimal(this.Text, this.IsAllowNegative);

                if (amountTemp > this.MaxValue)
                {
                    if (byFormaterChanged == false)
                    {
                        amountTemp = ToDecimal(lastValueTemp, this.IsAllowNegative);
                        currentCaretIndex = this.lastCaretIndex;
                    }
                    else
                    {
                        amountTemp = this.MaxValue;
                    }
                }
                else if (amountTemp < this.MinValue)
                {
                    if (byFormaterChanged == false)
                    {
                        amountTemp = ToDecimal(lastValueTemp, this.IsAllowNegative);
                        currentCaretIndex = this.lastCaretIndex;
                    }
                    else
                    {
                        amountTemp = this.MinValue;
                    }
                }

                string textTemp = FormatAmount(amountTemp, 0, RoundingEmun.Rounding);
                if (inputChar == '.' && base.Text.IndexOf('0') <= 0)
                {
                    this.Amount = Convert.ToDecimal(base.Text);
                }
                else
                {
                    base.Text = textTemp;
                }

                // Delete键产生的变化光标前面字符不变。
                if (this.currentPressKey == Key.Delete)
                {
                    int offset1 = 0, offset2 = 0;
                    for (int i = 0; i < lastValueTemp.Length; i++)
                    {
                        char chr1 = lastValueTemp[i];
                        if (chr1 == ',')
                        {
                            offset1++;
                        }

                        if (i == this.caretOnPreviewKeyDown)
                        {
                            break;
                        }
                    }

                    for (int i = 0; i < textTemp.Length; i++)
                    {
                        char chr2 = textTemp[i];
                        if (chr2 == ',')
                        {
                            offset2++;
                        }

                        if (i == currentCaretIndex)
                        {
                            break;
                        }
                    }

                    int offset = offset2 - offset1;
                    currentCaretIndex = this.caretOnPreviewKeyDown + offset;
                }
                else
                {
                    // 非Delete键产生的变化光标后面字符不变。
                    int changes = Math.Abs(textTemp.Length - lastValueTemp.Length);
                    if (changes == 0)
                    {
                        bool shouldCorrect = false;
                        if (!char.IsDigit(inputChar))
                        {
                            shouldCorrect = true;
                            if (inputChar == '-')
                            {
                                if (inputIndex == 0)
                                {
                                    shouldCorrect = false;
                                }
                            }
                            else if (inputChar == '.')
                            {
                                shouldCorrect = false;
                            }
                        }

                        if (shouldCorrect)
                        {
                            currentCaretIndex = currentCaretIndex - 1;
                        }
                    }
                    else if (changes <= 2)
                    {
                        int offset = lastValueTemp.Length - this.caretOnPreviewKeyDown;
                        currentCaretIndex = textTemp.Length - offset;
                    }
                }

                if (currentCaretIndex < 0)
                {
                    currentCaretIndex = 0;
                }

                this.CaretIndex = currentCaretIndex;
                amountTemp = ToDecimal(textTemp, this.IsAllowNegative);

                // if (Decimals > 0)
                // {
                // if (0 <= amountTemp && amountTemp < 10)
                // {
                // if (this.CaretIndex == 2 || this.CaretIndex == 0)
                // {
                // this.CaretIndex = 1;
                // }
                // }
                // else if (-10 < amountTemp && amountTemp < 0)
                // {
                // if (this.CaretIndex == 3)
                // {
                // this.CaretIndex = 2;
                // }
                // }
                // }
                this.isByTextChanged = true;
                this.Amount = amountTemp;
                this.isByTextChanged = false;

                this.lastAmountValue = amountTemp;
                this.lastTextValue = this.Text;
                this.lastCaretIndex = currentCaretIndex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}