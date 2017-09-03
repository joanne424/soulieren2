// <copyright file="Int32Box.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/27 12:00:59</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/27 12:00:59
//   修改描述：新建 Int32Box.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

namespace Banclogix.Controls
{
    /// <summary>
    /// 用于输入整数的文本编辑控件。
    /// </summary>
    public class Int32Box : Primitives.NumberBox<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Int32Box" /> class.
        /// </summary>
        public Int32Box()
        {
            this.MinValue = int.MinValue;
            this.MaxValue = int.MaxValue;
            this.Text = "0";
            this.IsSigned = false;
        }

        /// <summary>
        /// 设置输入规则。
        /// </summary>
        protected override void SetRule()
        {
            if (this.IsSealed)
            {
                this.InputRule = @"^(\-|\+)?\d*$";
                this.ParseRule = @"^(\-|\+)?\d+$";
            }
            else
            {
                this.InputRule = @"^\d*$";
                this.ParseRule = @"^\d+$";
            }
        }

        /// <summary>
        /// 将输入的文本转换为数字。
        /// </summary>
        /// <returns>返回转换后的数字</returns>
        protected override int ParseNumber()
        {
            int number = 0;
            int.TryParse(this.Text, out number);
            return number;
        }

        /// <summary>
        /// 格式化当前数字。
        /// </summary>
        /// <returns>返回格式化之后的数字</returns>
        protected override string FormatNumber()
        {
            return this.Number.ToString(this.Format);
        }
    }
}