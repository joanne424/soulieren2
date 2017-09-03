// <copyright file="String2ListConverter.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-12-26 </date>
// <summary>字符串转换成列表的转换器</summary>
// <modify>
//      修改人：donggj
//      修改时间：2013-12-26
//      修改描述：
//      版本：2.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// 字符串转换成列表的转换器
    /// </summary>
    public class String2ListConverter : IValueConverter
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
            List<string> list = new List<string>();
            if (value != null)
            {
                string str = value as string;

                // 如果字符串不为空，则以"，"分割这个字符串成List<string>
                if (!string.IsNullOrWhiteSpace(str))
                {
                    list = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }

            return list;
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
