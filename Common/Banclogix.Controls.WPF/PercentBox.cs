// <copyright file="PercentBox.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/11/14 10:04:51</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/11/14 10:04:51
//   修改描述：新建 PercentBox.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System.Windows;

namespace Banclogix.Controls
{
    /// <summary>
    /// 用于输入百分数的文本编辑控件。
    /// </summary>
    public class PercentBox : DecimalBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PercentBox" /> class.
        /// </summary>
        public PercentBox()
        {
            this.Format = "0.00 %";
        }

        /// <summary>
        /// 鼠标获取焦点时。
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.Text = (this.ProtectedNumber * 100).ToString();
        }

        /// <summary>
        /// 将输入的文本转换为数字。
        /// </summary>
        /// <returns>返回转换后的数字</returns>
        protected override decimal ParseNumber()
        {
            return base.ParseNumber() / 100;
        }

        /// <summary>
        /// 格式化当前数字。
        /// </summary>
        /// <returns>返回格式化之后的数字</returns>
        protected override string FormatNumber()
        {
            return this.ProtectedNumber.ToString(this.Format);
        }
    }
}