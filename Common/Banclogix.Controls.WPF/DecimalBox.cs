// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalBox.cs" company="">
//   
// </copyright>
// <author>tangl</author>
// <date>2013/10/28 9:29:35</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/28 9:29:35
//   修改描述：新建 DecimalBox.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace Banclogix.Controls
{
    using System.Text;
    using System.Windows;

    using Banclogix.Controls.Primitives;

    /// <summary>
    ///     用于输入数字的文本编辑控件。
    /// </summary>
    public class DecimalBox : NumberBox<decimal>
    {
        #region Static Fields

        /// <summary>
        ///     用于绑定的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
            "Point", 
            typeof(int), 
            typeof(DecimalBox), 
            new PropertyMetadata(2, ValueChanged));

        #endregion

        #region Fields

        /// <summary>
        ///     允许的输入的小数位。
        /// </summary>
        private int point;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DecimalBox" /> class.
        /// </summary>
        public DecimalBox()
        {
            this.MinValue = decimal.MinValue;
            this.MaxValue = decimal.MaxValue;

            this.ProtectedIsSigned = true;
            this.Point = 2;
            this.Format = "#,##0.00";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     允许的输入的小数位。
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

        #endregion

        #region Methods

        /// <summary>
        ///     格式化当前数字。
        /// </summary>
        /// <returns>返回格式化之后的数字</returns>
        protected override string FormatNumber()
        {
            return this.Number.ToString(this.Format);
        }

        /// <summary>
        ///     将输入的文本转换为数字。
        /// </summary>
        /// <returns>返回转换后的数字</returns>
        protected override decimal ParseNumber()
        {
            decimal number = 0.0M;
            decimal.TryParse(this.AutoComplete(), out number);
            return number;
        }

        /// <summary>
        ///     设置文本规则。
        /// </summary>
        protected override void SetRule()
        {
            if (this.IsSigned)
            {
                this.InputRule = string.Format(@"^(\-|\+)?((\d*)|(\d+\.\d{0}))$", "{0," + this.point + "}");
                this.ParseRule = string.Format(@"(\-|\+)?\d+(\.\d{0})?$", "{1," + this.point + "}");
            }
            else
            {
                this.InputRule = string.Format(@"^((\d*)|(\d+\.\d{0}))$", "{0," + this.point + "}");
                this.ParseRule = string.Format(@"\d+(\.\d{0})?$", "{1," + this.point + "}");
            }
        }

        /// <summary>
        /// 依赖属性变更时触发。
        /// </summary>
        /// <param name="obj">
        /// 属性变更的对象
        /// </param>
        /// <param name="e">
        /// 事件的参数
        /// </param>
        private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Point":
                    ((DecimalBox)obj).Point = (int)e.NewValue;
                    break;
            }
        }

        /// <summary>
        ///     自动补 0。
        /// </summary>
        /// <returns>返回补 0 之后的文本</returns>
        private string AutoComplete()
        {
            string text = this.Text;
            if (string.IsNullOrEmpty(text))
            {
                text = "0";
            }

            int index = text.IndexOf('.');
            int point = this.Point - (text.Length - (index + 1));
            if (point == 0)
            {
                return text;
            }

            var number = new StringBuilder(this.Text);
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

        #endregion
    }
}