﻿// <copyright file="LeftUpperRightConverter.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> zhangwz </author>
// <date> 2014/2/25 11:29:39 </date>
// <modify>
//   修改人：zhangwz
//   修改时间：2014/2/25 11:29:39
//   修改描述：新建 LeftUpperRightConverter
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review >

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// 左侧的右上坐标转换器
    /// </summary>
    public class LeftUpperRightConverter : IValueConverter
    {
        /// <summary>
        /// 左侧的右上坐标转换器
        /// </summary>
        /// <param name="value">value值</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">parameter</param>
        /// <param name="culture">culture</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Point pret = new Point((double)value - 9D, 0D);
            return pret;
        }

        /// <summary>
        /// 回转
        /// </summary>
        /// <param name="value">value值</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">parameter</param>
        /// <param name="culture">culture</param>
        /// <returns>未应用</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
