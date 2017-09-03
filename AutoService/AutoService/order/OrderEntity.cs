// <copyright file="OrderEntity.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:41:26 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:41:26
//      修改描述：新建 OrderEntity.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

using System;

namespace AutoService
{
    public class OrderEntity
    {
        public string id { get; set; }

        // public EnumOrderStatus order_status { get; set; }

        public DateTime create_time { get; set; }
    }

    public enum EnumOrderStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        open = 1,

        /// <summary>
        /// 已确认
        /// </summary>
        confrim = 2,

        /// <summary>
        /// 库房处理中
        /// </summary>
        storeroom = 3,

        /// <summary>
        /// 已发货
        /// </summary>
        send = 4,

        /// <summary>
        /// 订单完成
        /// </summary>
        succeed = 5,

        /// <summary>
        /// 订单取消
        /// </summary>
        cancel = 6,

        /// <summary>
        /// 无效
        /// </summary>
        fail = 7,

        /// <summary>
        /// 异常
        /// </summary>
        error = 8
    }
}
