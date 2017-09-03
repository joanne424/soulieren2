// <copyright file="VisibilityConverter.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2013/12/27 10:07:40 </date>
// <modify>
//   修改人：donggj
//   修改时间：2013/12/27 10:07:40
//   修改描述：新建 VisibilityConverter
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// bool值转换为Visibility
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">国际化信息</param>
        /// <returns>转换结果</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = true;
            try
            {
                result = System.Convert.ToBoolean(value);
            }
            catch 
            { 
            }

            Visibility v;
            if (result)
            {
                v = Visibility.Visible;
            }
            else
            {
                v = Visibility.Collapsed;
            }

            return v;
        }

        /// <summary>
        /// 反向转换方法
        /// </summary>
        /// <param name="value">需转换的值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">参数</param>
        /// <param name="culture">国际化信息</param>
        /// <returns>转换结果</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
