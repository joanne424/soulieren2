// <copyright file="OrderBookCallBackModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2015/09/09 10:24:59 </date>
// <summary> 挂单激活请求回调处理 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2015/09/09 10:24:59
//      修改描述：新建 OrderBookCallBackModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Manager.Models
{
    #region

    using System;
    using System.Linq;

    using BaseViewModel;

    using Caliburn.Micro;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Models;
    using Infrastructure.Service;

    #endregion

    /// <summary>
    /// 挂单激活请求回调处理
    /// </summary>
    public class OrderBookCallBackModel : BaseModel
    {
        #region Fields

        /// <summary>
        /// IEventAggregator事件
        /// </summary>
        private IEventAggregator eventAggregator;

        /// <summary>
        /// 运行实例
        /// </summary>
        private readonly RunTime runtime;

        /// <summary>
        /// 挂单服务
        /// </summary>
        private readonly OrderService orderService;

        private readonly IOrderCacheRepository orderRep;

        private readonly ICustomerCacheRepository customerRep;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBookCallBackModel"/> class. 
        /// </summary>
        /// <param name="ownerId">
        /// 拥有者ID
        /// </param>
        public OrderBookCallBackModel(string ownerId)
            : base(ownerId)
        {
            this.runtime = RunTime.GetCurrentRunTime(ownerId);
            this.eventAggregator = /*RunTime.GetCurrentRunTime(this.OwnerId)*/ this.runtime.GetCurrentEventAggregator();
            this.orderService = new OrderService(ownerId);
            this.orderRep = this.GetRepository<IOrderCacheRepository>();
            this.customerRep = this.GetRepository<ICustomerCacheRepository>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 接受OrderBook请求推送
        /// </summary>
        /// <param name="pushType">
        /// 推送类型
        /// </param>
        /// <param name="orderInfo">
        /// 挂单信息
        /// </param>
        public void PushBack(OrderBookPushTypeEnum pushType, PendingOrderModel orderInfo)
        {
            this.runtime.WaitOrderBookRelatedInitial();
            try
            {
                switch (pushType)
                {
                    case OrderBookPushTypeEnum.WaitActive:
                        // 挂单请求触发等待激活
                        if (!this.ActivatePendingOrder(orderInfo))
                        {

                            var trriggerOrder = new BaseOrderVM(orderInfo, OwnerId);
                            orderRep.AddOrUpdate(trriggerOrder);
                            // 通知时注意Order的触发状态赋值,在BaseOrderVm中去掉了Coppy中对触发状态的赋值
                            //BaseOrderVM cancelTrriggerOrder = orderRep.FindByID(orderInfo.OrderID);
                            //if (cancelTrriggerOrder == null)
                            //{
                            //    cancelTrriggerOrder = new BaseOrderVM(orderInfo, OwnerId);
                            //    orderRep.AddOrUpdate(cancelTrriggerOrder);
                            //}
                            //else
                            //{
                            //    cancelTrriggerOrder = new BaseOrderVM(orderInfo, OwnerId)
                            //    {
                            //        TriggerStatus = OrderTriggerEnum.Triggered
                            //    };
                            //    orderRep.AddOrUpdate(cancelTrriggerOrder);
                            //}
                        }
                        break;
                    case OrderBookPushTypeEnum.CancelOrComplete:
                        // 激活或者已经移除的订单通过其他退缩进行移除
                        // 只有取消挂单状态的挂单，才在这里进行移除
                        if (orderInfo.Status != PendingStatusEnum.Cancelled && orderInfo.Status != PendingStatusEnum.Filled)
                        {
                            // 通知时注意Order的触发状态赋值
                            BaseOrderVM cancelTrriggerOrder = orderRep.FindByID(orderInfo.OrderID);
                            if (cancelTrriggerOrder == null)
                            {
                                cancelTrriggerOrder = new BaseOrderVM(orderInfo, OwnerId);
                                this.GetRepository<IOrderCacheRepository>().AddOrUpdate(cancelTrriggerOrder);
                            }
                            else
                            {
                                cancelTrriggerOrder.Status = orderInfo.Status;
                                cancelTrriggerOrder.PendingOrderModel.TiggerTradeSpotRateAsk = 0;
                                cancelTrriggerOrder.PendingOrderModel.TiggerTradeSpotRateBid = 0;
                                cancelTrriggerOrder.TriggerStatus = OrderTriggerEnum.None;
                                orderRep.AddOrUpdate(cancelTrriggerOrder);
                            }
                        }
                        else
                        {
                            orderRep.Remove(orderInfo.OrderID);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("pushType");
                }
            }
            catch (Exception exception)
            {
                Infrastructure.Log.TraceManager.Error.WriteAdditional("订单触发处理", orderInfo, exception, "接受到订单触发推送时出现异常。");
            }
        }

        /// <summary>
        /// 过滤到没有权限的挂单
        /// </summary>
        private bool FilterRequest(PendingOrderModel order, StaffModel staff)
        {
            var customer = customerRep.FindByID(order.CustomerNo);
            if (customer == null)
                return true;

            if (!staff.StaffBaseInfo.CustomerGroup.Contains(customer.CustGroupID))
            {
                //当前客户不在交易员的客户帐户组
                return true;
            }

            if (staff.Symbols.Any(p => p.SymbolID == order.Symbol && p.Status == EnableDisStatusEnum.Disabled))
            {
                //交易员对请求的商品无权限
                return true;
            }

            var staffCurrency = staff.Currencys.FirstOrDefault(p => p.CurrencyID == customer.LocalCCYID);
            if (staffCurrency != null &&
                (order.PerOrderPosition < staffCurrency.MinimumAmount || order.PerOrderPosition > staffCurrency.MaximumAmount))
            {
                return true;
            }

            if (staff.StaffBaseInfo.CustomerNos != null && staff.StaffBaseInfo.CustomerNos.Contains(order.CustomerNo))
            {
                return true;
            }


            return false;
        }

        /// <summary>
        /// 自动激动挂单
        /// </summary>
        /// <param name="order">挂单信息</param>
        /// <returns>如果符合半自动条件则返回true。</returns>
        private bool ActivatePendingOrder(PendingOrderModel order)
        {
            var dealerConfig = this.runtime.CurrentDealerConfig;
            if (!dealerConfig.Online)
                return false;
            if (dealerConfig.Automation && dealerConfig.ActivatePendingOrderAutomationConfig != null
                && dealerConfig.ActivatePendingOrderAutomationConfig.Enabled)
            {
                var customer = GetRepository<ICustomerCacheRepository>().FindByID(order.CustomerNo).Account;
                var localCCY = GetRepository<ICurrencyCacheRepository>().GetName(customer.BaseInfo.LocalCCY);
                var currencySetting =
                    dealerConfig.ActivatePendingOrderAutomationConfig.CurrencySettings.FirstOrDefault(
                        p => p.Currency == localCCY);
                if (currencySetting != null && currencySetting.MaxAmount > 0
                    && order.PerOrderPosition <= currencySetting.MaxAmount)
                {
                    this.orderService.AutoActiveOrder(order, string.Empty);
                    return true;
                }
            }

            return false;
        }


        #endregion
    }
}