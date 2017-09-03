// <copyright file="PageChangedEventArgs.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2013/12/26 10:50:20 </date>
// <modify>
//   修改人：donggj
//   修改时间：2013/12/26 10:50:20
//   修改描述：新建 PageChangedEventArgs
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

using System.Windows;

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// 分页更改参数
    /// </summary>
    public class PageChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageChangedEventArgs"/> class.
        /// </summary>
        /// <param name="routeEvent">事件</param>
        /// <param name="pageSize">一页显示的数据个数</param>
        /// <param name="pageIndex">当前页数</param>
        public PageChangedEventArgs(RoutedEvent routeEvent, int pageSize, int pageIndex)
            : base(routeEvent)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        #region 属性

        /// <summary>
        /// 一页显示的数据个数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }

        #endregion
    }
}
