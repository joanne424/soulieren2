// <copyright file="PagingChangedEventArgs.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2013/12/27 10:07:40 </date>
// <modify>
//   修改人：donggj
//   修改时间：2013/12/27 10:07:40
//   修改描述：新建 PagingChangedEventArgs
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
using System.Windows;

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// 翻页事件参数
    /// </summary>
    public class PagingChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagingChangedEventArgs" /> class.
        /// </summary>
        /// <param name="eventToRaise">路由事件</param>
        /// <param name="pageSize">每页显示数据的个数</param>
        /// <param name="pageIndex">当前显示的页号</param>
        /// <param name="sort">排序的字段</param>
        public PagingChangedEventArgs(RoutedEvent eventToRaise, int pageSize, int pageIndex, string sort = null)
            : base(eventToRaise)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Sort = sort;
        }

        #region 属性

        /// <summary>
        /// 每页显示数据的个数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前显示的页号
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 排序的字段
        /// </summary>
        public string Sort { get; set; }

        #endregion
    }
}
