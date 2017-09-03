using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceApp
{
    public class OrderEntity
    {
        public string id { get; set; }

        public EnumOrderStatus order_status { get; set; }

        public EnumPayStatus pay_status { get; set; }

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

    public enum EnumPayStatus
    {
        /// <summary>
        /// 未付款
        /// </summary>
        notPay = 1,

        /// <summary>
        /// 已付款
        /// </summary>
        succeed = 2
    }
}
