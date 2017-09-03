// <copyright file="CaculateModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/2/24 11:31:07 </date>
// <summary> 实时计算Model </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/2/24 11:31:07
//      修改描述：新建 CaculateModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models.AccountRealTime
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    using BaseViewModel;

    using Infrastructure;
    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Log;
    using Infrastructure.Models;
    using Infrastructure.Service;
    using Infrastructure.Utils;

    using Microsoft.Practices.ObjectBuilder2;

    #endregion

    /// <summary>
    ///     实时计算Model
    /// </summary>
    public partial class CaculateModel : BaseModel
    {
        #region Constants

        /// <summary>
        ///     M字节比例
        /// </summary>
        private const int MbDiv = 1024 * 1024;

        /// <summary>
        ///     内存监控定时时间间隔(10分钟)
        /// </summary>
        private const int MemoryMonitorInterval = 10 * 60 * 1000;

        #endregion

        #region Fields

        /// <summary>
        ///     活跃的订单字典
        /// </summary>
        private readonly Dictionary<string, BaseDealVM> activeDealsDic = new Dictionary<string, BaseDealVM>();

        /// <summary>
        ///     活跃的挂单字典
        /// </summary>
        private readonly Dictionary<string, BaseOrderVM> activeOrdersDic = new Dictionary<string, BaseOrderVM>();

        /// <summary>
        ///     账号结构对象池
        /// </summary>
        private readonly ObjectPool<List<CustomerStructure>> customerlistObjectPool =
            new ObjectPool<List<CustomerStructure>>(() => new List<CustomerStructure>(), c => c.Clear());

        /// <summary>
        ///     订单列表对象池
        ///     用于订单的全部刷新处理
        /// </summary>
        private readonly ObjectPool<List<BaseDealVM>> deallistObjectPool =
            new ObjectPool<List<BaseDealVM>>(() => new List<BaseDealVM>(), c => c.Clear());

        /// <summary>
        ///     报价计算订单临时字典
        /// </summary>
        private readonly Dictionary<string, BaseDealVM> newPriceCalcDealDic = new Dictionary<string, BaseDealVM>();

        /// <summary>
        ///     报价计算挂单临时字典
        /// </summary>
        private readonly Dictionary<string, BaseOrderVM> newPriceCalcOrderDic = new Dictionary<string, BaseOrderVM>();

        /// <summary>
        ///     挂单列表对象池
        ///     用于挂单的全部刷新处理
        /// </summary>
        private readonly ObjectPool<List<BaseOrderVM>> orderlistObjectPool =
            new ObjectPool<List<BaseOrderVM>>(() => new List<BaseOrderVM>(), c => c.Clear());

        /// <summary>
        ///     是否结束
        /// </summary>
        private bool isEnded;

        /// <summary>
        ///     是否初始化
        ///     初始化时会初始化所有的报价和Customer结构
        /// </summary>
        private bool isInitial;

        /// <summary>
        ///     内存性能计数器
        /// </summary>
        private PerformanceCounter memoryCounter;

        /// <summary>
        ///     内存监控定时器
        /// </summary>
        private Timer memoryMonitorTimer;

        /// <summary>
        ///     The wait notifycount.
        /// </summary>
        private int waitNotifycount;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaculateModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        public CaculateModel(string varOwnerId)
            : base(varOwnerId)
        {
            // 注册退出释放
            Application.Current.Dispatcher.Invoke(
                new Action(() => { Application.Current.Exit += (s, e) => this.Dispose(); }), 
                null);

            this.customerRepository = this.GetRepository<ICustomerCacheRepository>();
            this.dealRepository = this.GetRepository<IDealCacheRepository>();
            this.orderRepository = this.GetRepository<IOrderCacheRepository>();
            this.pledgeRepository = this.GetRepository<IPledgeCacheRepository>();
            this.symbolRepository = this.GetRepository<ISymbolCacheRepository>();
            this.businessUnitrRepository = this.GetRepository<IBusinessUnitCacheRepository>();
            this.quoteGroupRepository = this.GetRepository<IQuoteGroupCacheRepository>();
            this.currencyRepository = this.GetRepository<ICurrencyCacheRepository>();
            this.tobeActiveRepository = this.GetRepository<ITobeActiveOrderCacheRepository>();
            this.generalSettingRepository = this.GetRepository<IGeneralSettingCacheRepository>();
            this.acctGroupRepository = this.GetRepository<ICustomerGroupCacheRepository>();
            this.creditRepository = this.GetRepository<ICreditCacheRepository>();
            this.forwardPointRepository = this.GetRepository<IForwardPointCacheRepository>();

            ////TODO:尚未订阅所有的关注事件
            // 注册订单添加事件
            this.dealRepository.SubscribeAddEvent(
                e =>
                this.PublishEvent(new CalculateEvent { EventType = CalculateEvent.EnumEventType.NewDeal, NewDeal = e }));

            // 注册订单修改事件
            this.dealRepository.SubscribeUpdateEvent(
                (olde, newe) =>
                this.PublishEvent(
                    new CalculateEvent
                        {
                            EventType = CalculateEvent.EnumEventType.UpdateDeal, 
                            NewDeal = newe, 
                            OldDeal = olde
                        }));

            // 注册订单删除事件
            this.dealRepository.SubscribeRemoveEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.RemoveDeal, OldDeal = e }));

            // 订阅挂单的添加事件
            this.orderRepository.SubscribeAddEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.NewOrder, NewOrder = e }));

            // 注册挂单修改事件
            this.orderRepository.SubscribeUpdateEvent(
                (olde, newe) =>
                this.PublishEvent(
                    new CalculateEvent
                        {
                            EventType = CalculateEvent.EnumEventType.UpdateOrder, 
                            NewOrder = newe, 
                            OldOrder = olde
                        }));

            // 注册挂单删除事件
            this.orderRepository.SubscribeRemoveEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.RemoveOrder, OldOrder = e }));

            // 注册账户修改事件
            this.customerRepository.SubscribeUpdateEvent(
                (olde, newe) =>
                this.PublishEvent(
                    new CalculateEvent
                        {
                            EventType = CalculateEvent.EnumEventType.UpdateCustomer, 
                            NewCustomer = newe, 
                            OldCustoemr = olde
                        }));

            // 注册抵押金添加事件
            this.pledgeRepository.SubscribeAddEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.NewPledge, NewPledge = e }));

            // 注册抵押金修改事件
            this.pledgeRepository.SubscribeUpdateEvent(
                (olde, newe) =>
                this.PublishEvent(
                    new CalculateEvent
                        {
                            EventType = CalculateEvent.EnumEventType.UpdatePledge, 
                            NewPledge = newe, 
                            OldPledge = olde
                        }));

            // 注册抵押金删除事件
            this.pledgeRepository.SubscribeRemoveEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.RemovePledge, OldPledge = e }));

            // 注册BU修改事件
            this.businessUnitrRepository.SubscribeUpdateEvent(
                (olde, newe) =>
                this.PublishEvent(
                    new CalculateEvent
                        {
                            EventType = CalculateEvent.EnumEventType.UpdateBusinessUnit, 
                            NewBu = newe, 
                            OldBu = olde
                        }));

            // 注册授信添加事件
            this.creditRepository.SubscribeAddEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.NewCredit, NewCredit = e }));

            // 注册授信删除事件
            this.creditRepository.SubscribeRemoveEvent(
                e =>
                this.PublishEvent(
                    new CalculateEvent { EventType = CalculateEvent.EnumEventType.RemoveCredit, OldCredit = e }));

            // 注册会导致订单的CalcPriceInfo结构变化的仓储变更
            this.RegisterUpdateSettingsEvent();

            // 启动实时事件消费线程
            this.backgroudThread = new Thread(
                () =>
                    {
                        TraceManager.Debug.Write("实时计算", "实时计算启动");
                        this.StartMemoryMonitor();
                        CalculateEvent item = null;
                        while (true)
                        {
                            this.newCalcEventNotify.WaitOne();
                            TraceManager.Debug.Write(
                                "实时计算", 
                                "实时计算收到新的计算请求，通用事件队列：{0}, 报价事件队列:{1}", 
                                this.normalEventQueue.Count, 
                                this.newPriceEventQueue.Count);

                            while (this.highEventQueue.TryTake(out item))
                            {
                                this.ConsumerCalculateEvent(item);
                            }

                            while (this.normalEventQueue.TryTake(out item))
                            {
                                this.ConsumerCalculateEvent(item);
                            }

                            TraceManager.Debug.Write(
                                "实时计算", 
                                "重新计算Account列表：{0}", 
                                string.Join(";", this.activeCustomerStructureDic.Keys));

                            var newPriceDic = new Dictionary<string, TickQuoteModel>();
                            while (this.newPriceEventQueue.TryTake(out item))
                            {
                                if (newPriceDic.ContainsKey(item.NewSinglePrice.SymbolID))
                                {
                                    newPriceDic[item.NewSinglePrice.SymbolID] = item.NewSinglePrice;
                                }
                                else
                                {
                                    newPriceDic.Add(item.NewSinglePrice.SymbolID, item.NewSinglePrice);
                                }
                            }

                            if (newPriceDic.Count > 0)
                            {
                                TraceManager.Debug.Write("实时计算", "新报价携带报价数量：{0}", newPriceDic.Count);
                                var priceEvt = new CalculateEvent();
                                priceEvt.EventType = CalculateEvent.EnumEventType.NewPrice;
                                priceEvt.NewPriceList = newPriceDic.Values.ToList();
                                this.ConsumerCalculateEvent(priceEvt);
                            }

                            if (this.isEnded)
                            {
                                // 结束消费线程
                                return;
                            }
                        }
                    });

            this.backgroudThread.IsBackground = true;
            this.backgroudThread.Start();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     货币仓储
        /// </summary>
        public ICurrencyCacheRepository CurrencyRepository
        {
            get
            {
                return this.currencyRepository;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     清空
        /// </summary>
        public void Clear()
        {
            this.PublishEvent(new CalculateEvent { EventType = CalculateEvent.EnumEventType.Terminal });
            this.ClearRepository();
        }

        /// <summary>
        ///     Release resource
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 发布通知事件
        /// </summary>
        /// <param name="evt">
        /// 通知事件
        /// </param>
        public void PublishEvent(CalculateEvent evt)
        {
            if (evt == null)
            {
                return;
            }

            if (!this.isInitial)
            {
                if (evt.EventType != CalculateEvent.EnumEventType.Initail)
                {
                    return;
                }
            }

            switch (evt.EventType)
            {
                case CalculateEvent.EnumEventType.Initail:
                case CalculateEvent.EnumEventType.Terminal:
                    this.highEventQueue.Add(evt);
                    break;
                case CalculateEvent.EnumEventType.NewPrice:
                    this.newPriceEventQueue.Add(evt);
                    break;
                case CalculateEvent.EnumEventType.NewCredit:
                case CalculateEvent.EnumEventType.RemoveCredit:
                case CalculateEvent.EnumEventType.UpdateCustomer:
                case CalculateEvent.EnumEventType.UpdateBusinessUnit:
                case CalculateEvent.EnumEventType.NewDeal:
                case CalculateEvent.EnumEventType.UpdateDeal:
                case CalculateEvent.EnumEventType.RemoveDeal:
                case CalculateEvent.EnumEventType.NewOrder:
                case CalculateEvent.EnumEventType.UpdateOrder:
                case CalculateEvent.EnumEventType.RemoveOrder:
                case CalculateEvent.EnumEventType.NewPledge:
                case CalculateEvent.EnumEventType.UpdatePledge:
                case CalculateEvent.EnumEventType.RemovePledge:
                case CalculateEvent.EnumEventType.CloseWindow:
                case CalculateEvent.EnumEventType.OpenWindow:
                case CalculateEvent.EnumEventType.AddMarginCall:
                case CalculateEvent.EnumEventType.RemoveMarginCall:
                case CalculateEvent.EnumEventType.ShowCustChangeInRequestHandling:
                case CalculateEvent.EnumEventType.UpdateSettings:
                case CalculateEvent.EnumEventType.DealOrderListOpen:
                case CalculateEvent.EnumEventType.DealOrderListClose:
                case CalculateEvent.EnumEventType.AddActiveDeals:
                case CalculateEvent.EnumEventType.RemoveActiveDeals:
                case CalculateEvent.EnumEventType.AddActiveOrders:
                case CalculateEvent.EnumEventType.RemoveActiveOrders:
                    this.normalEventQueue.Add(evt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.newCalcEventNotify.Set();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 向系统中添加新的挂单信息
        /// </summary>
        /// <param name="newOrder">
        /// 新的挂单信息
        /// </param>
        private void AddPendingOrder(BaseOrderVM newOrder)
        {
            if (newOrder.PendingOrderModel.Status != PendingStatusEnum.Pending)
            {
                return;
            }

            PriceStructure directPriceStruct = this.GetPriceStructureBySymbol(newOrder.Symbol);
            if (directPriceStruct == null)
            {
                TraceManager.Warn.WriteAdditional("实时计算", newOrder, "处理新Order添加时，直接报价结构中尚不存在");
            }
            else
            {
                if (directPriceStruct.PendingDealList.ContainsKey(newOrder.GetID()))
                {
                    TraceManager.Warn.WriteAdditional("实时计算", newOrder, "处理新Order添加时，挂单已经在直接报价结构中存在存在");
                }
                else
                {
                    directPriceStruct.PendingDealList.Add(newOrder.GetID(), newOrder);
                }
            }

            this.UpdateOrderMarketPriceUnNotify(newOrder);
            newOrder.NotifyMarketTraderSpotRate();

            CustomerStructure custStruct = this.GetAcctStructureById(newOrder.CustomerNo);
            if (custStruct == null)
            {
                return;
            }

            custStruct.AddOrder(newOrder);
            custStruct.CalculateCustomerWhenNewPrice();
            custStruct.NotifyCustomer();
        }

        /// <summary>
        ///     计算刷新所有的活跃账户
        /// </summary>
        private void CalculateAllActiveCustomer()
        {
            ObjectPoolItem<List<CustomerStructure>> notifyCustomerList = this.customerlistObjectPool.GetPoolItem();
            foreach (CustomerStructure activeCustStruct in this.activeCustomerStructureDic.Values)
            {
                activeCustStruct.CalculateCustomerWhenNewPrice();
                notifyCustomerList.Value.Add(activeCustStruct);
            }

            Application.Current.Dispatcher.Invoke(
                new Action(
                    () =>
                        {
                            try
                            {
                                foreach (CustomerStructure acct in notifyCustomerList.Value)
                                {
                                    acct.NotifyCustomer();
                                }
                            }
                            catch (Exception exception)
                            {
                                TraceManager.Error.Write("Calculate", exception, "界面通知时后台变更，此错误无业务影响");
                            }
                            finally
                            {
                                notifyCustomerList.Close();
                            }
                        }), 
                null);
        }

        /// <summary>
        /// 更新受新报价影响的订单字典
        /// </summary>
        /// <param name="varDealDictionary">
        /// 订单字典
        /// </param>
        private void CalculateDealInDic(Dictionary<string, BaseDealVM> varDealDictionary)
        {
            // 处理所有的订单
            foreach (BaseDealVM item in varDealDictionary.Values)
            {
                this.CalculateDealWhenNewPriceNoNotify(item);
            }

            TraceManager.Debug.Write("实时计算", "{0}个订单，总计算时间：", varDealDictionary.Count);
            if (varDealDictionary.Count > 0)
            {
                ObjectPoolItem<List<BaseDealVM>> notifyDealList = this.deallistObjectPool.GetPoolItem();
                foreach (BaseDealVM dealItem in varDealDictionary.Values)
                {
                    notifyDealList.Value.Add(dealItem);
                }

                TraceManager.Debug.Write("实时计算", "待通知订单列表数量：{0}", Interlocked.Increment(ref this.waitNotifycount));

                Application.Current.Dispatcher.Invoke(
                    new Action(
                        () =>
                            {
                                try
                                {
                                    foreach (BaseDealVM varDeal in notifyDealList.Value)
                                    {
                                        // 将实时值更新到订单上
                                        varDeal.MarketRate = varDeal.MarketRate;
                                        varDeal.FloatingPL = varDeal.FloatingPL;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    TraceManager.Warn.Write("实时计算", exception, "界面通知时后台变更，此错误无业务影响");
                                }
                                finally
                                {
                                    notifyDealList.Close();
                                    Interlocked.Decrement(ref this.waitNotifycount);
                                }
                            }), 
                    null);
            }
        }

        /// <summary>
        /// The update deal and acct.
        /// </summary>
        /// <param name="deal">
        /// The item.
        /// </param>
        private void CalculateDealWhenNewPriceNoNotify(BaseDealVM deal)
        {
            try
            {
                // 是否需要将订单重新注册进报价结构中
                bool isNeedReregisterDealInPriceStruct = false;
                DateTime oldExpireTime = DateTime.MinValue;
                if (deal.PriceCalcInfo != null)
                {
                    oldExpireTime = deal.PriceCalcInfo.TenorExpireTime;
                }
                else
                {
                    isNeedReregisterDealInPriceStruct = true;
                }

                decimal gapOfRate;
                PriceCore.Instance.UpdatePriceCalcInfoByNewPrice(deal);
                decimal customerRate = PriceCore.Instance.CalculateCustomerRate(deal.PriceCalcInfo);
                if (!isNeedReregisterDealInPriceStruct)
                {
                    if (oldExpireTime != deal.PriceCalcInfo.TenorExpireTime)
                    {
                        isNeedReregisterDealInPriceStruct = true;
                    }
                }

                if (isNeedReregisterDealInPriceStruct)
                {
                    this.RegistDealInPriceStruct(deal);
                }

                if (deal.TransactionType == TransactionTypeEnum.Buy)
                {
                    gapOfRate = customerRate - deal.OpenRate;
                }
                else
                {
                    gapOfRate = deal.OpenRate - customerRate;
                }

                deal.SetMarketRateNoNotify(customerRate);

                // add by donggj, 2015-12-09 21:19,格式化订单的FloatingPL
                BaseCustomerViewModel custVM = this.customerRepository.FindByID(deal.CustomerNo);

                if (custVM == null)
                {
                    TraceManager.Error.WriteAdditional("实时计算", deal, "找不到订单对应的账户。");
                    return;
                }

                BaseCurrencyVM ccyVM = this.currencyRepository.FindByID(custVM.LocalCCYID);

                if (ccyVM == null)
                {
                    TraceManager.Error.WriteAdditional("实时计算", deal, "找不到账户本币的货币信息。");
                    return;
                }

                decimal floatingPl = gapOfRate * deal.CCY1Amount;

                if (deal.PriceCalcInfo.PlccyToLocalCcyDirectType
                    == PlccyToLocalCcyDirectEnum.Equal)
                {
                }
                else
                {
                    if (deal.PriceCalcInfo.PlccyToLocalCcyDirectType
                        == PlccyToLocalCcyDirectEnum.LocalCcyBefore)
                    {
                        if (deal.NewTrancePrice != null && deal.NewTrancePrice.Mid != decimal.Zero)
                        {
                            floatingPl = floatingPl / deal.NewTrancePrice.Mid;
                        }
                    }
                    else
                    {
                        if (deal.NewTrancePrice != null)
                        {
                            floatingPl = floatingPl * deal.NewTrancePrice.Mid;
                        }
                    }
                }

                ////System.Diagnostics.Debug.WriteLine("DealID:{0},unformat floatingPL: {1},Mid Rate:{2}", deal.DealID, floatingPl, deal.NewTrancePrice.Mid);
                floatingPl = floatingPl.FormatAmountByCCYConfig(ccyVM.RoundingMethod, ccyVM.AmountDecimals);

                ////System.Diagnostics.Debug.WriteLine("DealID:{0},format floatingPL: {1}", deal.DealID, floatingPl);
                deal.SetFloatingPlNoNotify(floatingPl);
            }
            catch (Exception exception)
            {
                TraceManager.Error.WriteAdditional(
                    "实时计算", 
                    deal, 
                    exception, 
                    "新报价时计算订单出现异常。DealId：{0}, CustomerNo:{0}", 
                    deal.DealID, 
                    deal.CustomerNo);
                deal.SetFloatingPlNoNotify(0);
            }
        }

        /// <summary>
        /// 消费事件
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void ConsumerCalculateEvent(CalculateEvent evt)
        {
            try
            {
                switch (evt.EventType)
                {
                    case CalculateEvent.EnumEventType.UpdateCustomer:
                        this.DealingUpdateCustomer(evt);
                        break;
                    case CalculateEvent.EnumEventType.UpdateBusinessUnit:
                        this.DealingUpdateBu(evt);
                        break;
                    case CalculateEvent.EnumEventType.NewDeal:
                        this.HanldingAddDeal(evt);
                        break;
                    case CalculateEvent.EnumEventType.UpdateDeal:
                        this.HanldingUpdateDeal(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemoveDeal:
                        this.HanldingRemoveDeal(evt);
                        break;
                    case CalculateEvent.EnumEventType.NewOrder:
                        this.HanldingAddOrder(evt);
                        break;
                    case CalculateEvent.EnumEventType.UpdateOrder:
                        this.HanldingUpdateOrder(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemoveOrder:
                        this.HanldingRemoveOrder(evt);
                        break;
                    case CalculateEvent.EnumEventType.OpenWindow:
                        this.DealingOpenCustomerWindow(evt);
                        break;
                    case CalculateEvent.EnumEventType.CloseWindow:
                        this.DealingCloseCustomerWindow(evt);
                        break;
                    case CalculateEvent.EnumEventType.NewCredit:
                        this.DealingAddCredit(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemoveCredit:
                        this.DealingRemoveCredit(evt);
                        break;
                    case CalculateEvent.EnumEventType.Terminal:
                        this.isEnded = true;
                        break;
                    case CalculateEvent.EnumEventType.NewPledge:
                        this.HanldingNewPledge(evt);
                        break;
                    case CalculateEvent.EnumEventType.UpdatePledge:
                        this.HanldingUpdatePledge(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemovePledge:
                        this.HanldingRemovePledge(evt);
                        break;
                    case CalculateEvent.EnumEventType.Initail:
                        this.Initial();
                        break;
                    case CalculateEvent.EnumEventType.NewPrice:
                        TraceManager.Debug.Write("实时计算", "开始处理报价到订单");

                        // 更改相关的交易单和挂单
                        this.DealingNewPrice(evt);

                        TraceManager.Debug.Write("实时计算", "完成处理报价到订单，开始计算需要计算的账户");

                        // 重新计算需要计算的账户信息                    
                        this.CalculateAllActiveCustomer();

                        TraceManager.Debug.Write("实时计算", "完成新报价处理");
                        break;
                    case CalculateEvent.EnumEventType.Online:

                        // 处理账户Online事件
                        this.DealingOnline(evt);
                        break;
                    case CalculateEvent.EnumEventType.Offline:

                        // 处理账户Offline事件
                        this.DealingOffline(evt);
                        break;
                    case CalculateEvent.EnumEventType.AddMarginCall:

                        // 处理账户MarginCall更改事件
                        this.DealingAddMarginCall(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemoveMarginCall:

                        // 处理账户MarginCall更改事件
                        this.DealingRemoveMarginCall(evt);
                        break;
                    case CalculateEvent.EnumEventType.ShowCustChangeInRequestHandling:

                        // 处理请求列表显示账户更改事件
                        this.DealingShowCustChangeInRequestHandling(evt);
                        break;
                    case CalculateEvent.EnumEventType.UpdateSettings:

                        // 处理影响CalcPriceInfo结构的配置更改事件
                        this.DealingUpdateSettings(evt);
                        break;
                    case CalculateEvent.EnumEventType.DealOrderListOpen:
                        if (!this.isSubcribePrice)
                        {
                            PriceReceiveCore.Instance.ReadQuotePriceEvent += this.OnQuotePriceReceived;
                            this.isSubcribePrice = true;
                            this.isDealOrderListOpen = true;
                        }

                        break;
                    case CalculateEvent.EnumEventType.DealOrderListClose:
                        if (this.isSubcribePrice && this.activeCustomerStructureDic.Count == 0)
                        {
                            PriceReceiveCore.Instance.ReadQuotePriceEvent -= this.OnQuotePriceReceived;
                            this.isSubcribePrice = false;
                            this.isDealOrderListOpen = false;
                        }

                        break;
                    case CalculateEvent.EnumEventType.AddActiveDeals:
                        this.HandlingAddActiveDeals(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemoveActiveDeals:
                        this.HandlingRemoveActiveDeals(evt);
                        break;
                    case CalculateEvent.EnumEventType.AddActiveOrders:
                        this.HandlingAddActiveOrders(evt);
                        break;
                    case CalculateEvent.EnumEventType.RemoveActiveOrders:
                        this.HandlingRemoveActiveOrders(evt);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (RealtimeCalculateException calculateException)
            {
                TraceManager.Error.Write("实时计算", calculateException, "出现严重逻辑错误");
                throw;
            }
            catch (Exception exception)
            {
                TraceManager.Error.Write("实时计算", exception, "出现异常");
            }
        }

        /// <summary>
        /// 处理授信添加事件
        /// </summary>
        /// <param name="evt">
        /// 授信添加事件
        /// </param>
        private void DealingAddCredit(CalculateEvent evt)
        {
            if (this.activeCustomerStructureDic.ContainsKey(evt.NewCredit.CustomerNo))
            {
                CustomerStructure custoemrStruct = this.activeCustomerStructureDic[evt.NewCredit.CustomerNo];
                custoemrStruct.AddCredit(evt.NewCredit);
                custoemrStruct.CalculateCustomerWhenNewPrice();
                custoemrStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 处理添加MarginCall事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingAddMarginCall(CalculateEvent evt)
        {
            if (!this.margincallAccts.Contains(evt.MarginCallCustomerNo))
            {
                // 添加到账户列表中
                this.margincallAccts.Add(evt.MarginCallCustomerNo);
                BaseMarginCallVm marginCallItem =
                    this.GetRepository<IMarginCallCacheRepository>()
                        .Filter(item => item.BelongCustomer.CustomerNo == evt.MarginCallCustomerNo)
                        .FirstOrDefault();
                if (marginCallItem != null)
                {
                    Application.Current.Dispatcher.Invoke(() => this.orderedMarginCallList.Add(marginCallItem));
                }

                if (!this.activeCustomerStructureDic.ContainsKey(evt.MarginCallCustomerNo))
                {
                    this.InitialCustomerStructInActiveCustDic(evt.MarginCallCustomerNo);
                }
            }
        }

        /// <summary>
        /// 处理账户CloseWindow事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingCloseCustomerWindow(CalculateEvent evt)
        {
            this.openWindowCustomers.Remove(evt.WindwoCustomerNo);
            if (!this.onlineAccts.Contains(evt.WindwoCustomerNo) && !this.margincallAccts.Contains(evt.WindwoCustomerNo)
                && this.showCustInRequestHandling != evt.WindwoCustomerNo)
            {
                this.activeCustomerStructureDic.Remove(evt.WindwoCustomerNo);
                this.OnActiveCustomerChanged(false);
            }
        }

        /// <summary>
        /// 处理新报价
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingNewPrice(CalculateEvent evt)
        {
            List<TickQuoteModel> newPrices = evt.NewPriceList;
            if (newPrices == null)
            {
                TraceManager.Error.Write("Service", "新报价事件，新报价列表为空。");
                return;
            }

            // 新策略
            this.newPriceCalcDealDic.Clear();
            this.newPriceCalcOrderDic.Clear();

            foreach (TickQuoteModel item in newPrices)
            {
                PriceStructure price = null;
                bool isExist = this.priceDic.TryGetValue(item.SymbolID, out price);
                if (isExist)
                {
                    // 报价未发生变化，不进行计算
                    if (price.Price.TraderAsk == item.TraderAsk && price.Price.TraderBid == item.TraderBid)
                    {
                        continue;
                    }

                    price.Price = item;

                    // 判断所有活跃账户的订单是否要实时计算
                    IEnumerable<List<BaseDealVM>> deals =
                        this.activeCustomerStructureDic.Values.Select(o => o.GetRelatedDeals());
                    deals.ForEach(
                        o => o.ForEach(
                            p =>
                            {
                                if (price.DealList.ContainsKey(p.GetID())
                                    && !this.newPriceCalcDealDic.ContainsKey(p.GetID()))
                                {
                                    this.newPriceCalcDealDic.Add(p.GetID(), p);
                                }
                            }));

                    this.activeDealsDic.ForEach(
                        o =>
                        {
                            if (price.DealList.ContainsKey(o.Key)
                                && !this.newPriceCalcDealDic.ContainsKey(o.Key))
                            {
                                // 订单是从查询推送查的，不包含PriceCalcInfo的信息，所以要添加上
                                o.Value.PriceCalcInfo = price.DealList[o.Key].PriceCalcInfo;
                                o.Value.NewTrancePrice = price.DealList[o.Key].NewTrancePrice;
                                this.newPriceCalcDealDic.Add(o.Key, o.Value);
                            }
                        });

                    // 判断所有活跃账户的挂单是否要实时计算
                    IEnumerable<List<BaseOrderVM>> orders =
                        this.activeCustomerStructureDic.Values.Select(o => o.GetRelatedOrders());
                    orders.ForEach(
                        o => o.ForEach(
                            p =>
                                {
                                    if (price.PendingDealList.ContainsKey(p.GetID())
                                        && !this.newPriceCalcOrderDic.ContainsKey(p.GetID()))
                                    {
                                        this.newPriceCalcOrderDic.Add(p.GetID(), p);
                                    }
                                }));

                    this.activeOrdersDic.ForEach(
                        o =>
                            {
                                if (price.PendingDealList.ContainsKey(o.Key)
                                    && !this.newPriceCalcOrderDic.ContainsKey(o.Key))
                                {
                                    this.newPriceCalcOrderDic.Add(o.Key, o.Value);
                                }
                            });
                }
                else
                {
                    // 添加报价结构
                    var priceStru = new PriceStructure();
                    priceStru.Price = item;
                    this.priceDic.Add(item.SymbolID, priceStru);

                    string symbolId = item.SymbolID;
                    List<BaseOrderVM> orders = this.orderRepository.Filter(d => d.Symbol == symbolId).ToList();
                    for (int i = 0; i < orders.Count(); i++)
                    {
                        BaseOrderVM order = orders[i];

                        priceStru.PendingDealList.Add(order.GetID(), order);

                        if (!this.newPriceCalcOrderDic.ContainsKey(order.GetID()))
                        {
                            this.newPriceCalcOrderDic.Add(order.GetID(), order);
                        }
                    }
                }
            }

            if (this.newPriceCalcDealDic.Any())
            {
                // 更新订单和账户实时值
                TraceManager.Debug.Write("实时计算", "开始处理订单" + this.newPriceCalcDealDic.Count);
                this.CalculateDealInDic(this.newPriceCalcDealDic);
                TraceManager.Debug.Write("实时计算", "完成处理订单");
            }

            if (this.newPriceCalcOrderDic.Any())
            {
                // 处理所有的挂单
                TraceManager.Debug.Write("实时计算", "开始处理挂单" + this.newPriceCalcOrderDic.Count);
                this.UpdatePendingOrderInDic(this.newPriceCalcOrderDic);
                TraceManager.Debug.Write("实时计算", "完成处理挂单");
            }
        }

        /// <summary>
        /// 处理账户Offline事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingOffline(CalculateEvent evt)
        {
            ////TODO：待处理
            ////var offlinelist = evt.NotifyData as List<BaseAccountViewModel>;
            ////foreach (var acct in offlinelist)
            ////{
            ////    var offAcctId = acct.AccountNo;
            ////    this.onlineAccts.Remove(offAcctId);
            ////    if (!this.openWindowAccts.Contains(offAcctId) && !this.margincallAccts.Contains(offAcctId))
            ////    {
            ////        this.acctDic.Remove(offAcctId);
            ////    }
            ////}
        }

        /// <summary>
        /// 处理账户Online事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingOnline(CalculateEvent evt)
        {
            ////TODO:待处理
            ////var onlinelist = evt.NotifyData as List<BaseAccountViewModel>;
            ////foreach (var acct in onlinelist)
            ////{
            ////    var acctId = acct.AccountNo;
            ////    if (!this.onlineAccts.Contains(acctId))
            ////    {
            ////        // 添加到账户列表中
            ////        this.onlineAccts.Add(acctId);

            ////        if (this.acctDic.ContainsKey(acctId) == false)
            ////        {
            ////            // 构建并添加账户结构
            ////            CustomerStructure acctStru = new CustomerStructure();
            ////            var dealVms = this.dealRepository.Filter(d => d.AccountID == acctId).ToList();
            ////            foreach (var item in dealVms)
            ////            {
            ////                if (item.IsPendingOrder)
            ////                {
            ////                    continue;
            ////                }

            ////                acctStru.DealDictionary.Add(item.ExecutionID, item);
            ////            }

            ////            this.acctDic.Add(acctId, acctStru);

            ////            // 立即重新计算账户信息
            ////            this.CalculateAcct(acctId);
            ////        }
            ////    }
            ////}
        }

        /// <summary>
        /// 处理打开Customer窗口事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingOpenCustomerWindow(CalculateEvent evt)
        {
            if (!this.openWindowCustomers.Contains(evt.WindwoCustomerNo))
            {
                // 添加到账户列表中
                this.openWindowCustomers.Add(evt.WindwoCustomerNo);
                if (!this.activeCustomerStructureDic.ContainsKey(evt.WindwoCustomerNo))
                {
                    this.InitialCustomerStructInActiveCustDic(evt.WindwoCustomerNo);
                }
            }
        }

        /// <summary>
        /// 处理授信删除事件
        /// </summary>
        /// <param name="evt">
        /// 授信删除事件
        /// </param>
        private void DealingRemoveCredit(CalculateEvent evt)
        {
            if (this.activeCustomerStructureDic.ContainsKey(evt.OldCredit.CustomerNo))
            {
                CustomerStructure custoemrStruct = this.activeCustomerStructureDic[evt.OldCredit.CustomerNo];
                custoemrStruct.RemoveCredit(evt.OldCredit);
                custoemrStruct.CalculateCustomerWhenNewPrice();
                custoemrStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 处理删除MarginCall事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingRemoveMarginCall(CalculateEvent evt)
        {
            this.margincallAccts.Remove(evt.MarginCallCustomerNo);
            Application.Current.Dispatcher.Invoke(
                new Action(
                    () =>
                    this.orderedMarginCallList.Remove(
                        this.orderedMarginCallList.FirstOrDefault(
                            item => item.BelongCustomer.CustomerNo == evt.MarginCallCustomerNo))));

            if (!this.onlineAccts.Contains(evt.MarginCallCustomerNo)
                && !this.openWindowCustomers.Contains(evt.MarginCallCustomerNo)
                && this.showCustInRequestHandling != evt.MarginCallCustomerNo)
            {
                this.activeCustomerStructureDic.Remove(evt.MarginCallCustomerNo);
                this.OnActiveCustomerChanged(false);
            }
        }

        /// <summary>
        /// 处理请求列表显示账户更改事件
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void DealingShowCustChangeInRequestHandling(CalculateEvent evt)
        {
            if (this.showCustInRequestHandling == evt.RequestHandingShowCustomerNo)
            {
                return;
            }

            if (!this.showCustInRequestHandling.IsNullOrSpace())
            {
                if (!this.onlineAccts.Contains(this.showCustInRequestHandling)
                    && !this.openWindowCustomers.Contains(this.showCustInRequestHandling)
                    && !this.margincallAccts.Contains(this.showCustInRequestHandling))
                {
                    this.activeCustomerStructureDic.Remove(this.showCustInRequestHandling);
                    this.OnActiveCustomerChanged(false);
                }
            }

            this.showCustInRequestHandling = evt.RequestHandingShowCustomerNo;

            if (!evt.RequestHandingShowCustomerNo.IsNullOrSpace())
            {
                if (!this.activeCustomerStructureDic.ContainsKey(evt.RequestHandingShowCustomerNo))
                {
                    this.InitialCustomerStructInActiveCustDic(evt.RequestHandingShowCustomerNo);
                }
            }
        }

        /// <summary>
        /// 处理Bu的更新事件
        /// </summary>
        /// <param name="evt">
        /// Bu更新事件
        /// </param>
        private void DealingUpdateBu(CalculateEvent evt)
        {
            foreach (CustomerStructure customerStructure in this.activeCustomerStructureDic.Values)
            {
                if (customerStructure.BelongBu(evt.NewBu))
                {
                    customerStructure.ReInitialCalculateCustomer();
                    customerStructure.NotifyCustomer();
                }
            }
        }

        /// <summary>
        /// 处理账户的更新
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void DealingUpdateCustomer(CalculateEvent evt)
        {
            if (this.activeCustomerStructureDic.ContainsKey(evt.NewCustomer.CustmerNo))
            {
                CustomerStructure customerStruct = this.activeCustomerStructureDic[evt.NewCustomer.CustmerNo];
                customerStruct.ReInitialCalculateCustomer();
                customerStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 处理配置更新事件
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void DealingUpdateSettings(CalculateEvent evt)
        {
            this.newPriceCalcDealDic.Clear();
            foreach (PriceStructure priceStructure in this.priceDic.Values)
            {
                foreach (var pair in priceStructure.DealList)
                {
                    if (!this.newPriceCalcDealDic.ContainsKey(pair.Key))
                    {
                        this.newPriceCalcDealDic.Add(pair.Key, pair.Value);
                    }
                }
            }

            // 重新计算
            this.newPriceCalcDealDic.Values.ForEach(
                o => PriceCore.Instance.InitialPriceCalcInfoForDeal(o));

            // 重新计算Deal
            this.CalculateDealInDic(this.newPriceCalcDealDic);

            // 重算活跃账户信息
            this.CalculateAllActiveCustomer();
        }

        /// <summary>
        ///     获取Mb为单位的内存使用量
        /// </summary>
        /// <returns>内存使用量</returns>
        private double GetMemoryUsageInMb()
        {
            if (ConfigParameter.EnabelPerformanceCounters)
            {
                try
                {
                    return this.memoryCounter.NextValue() / MbDiv;
                }
                catch (Exception exception)
                {
                    ConfigParameter.EnabelPerformanceCounters = false;
                    TraceManager.Error.Write("MemoryMonitor", exception, "Exception when monitor,Stop monitor。");
                }
            }

            return 0.0;
        }

        /// <summary>
        /// 添加活跃订单的事件处理函数
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HandlingAddActiveDeals(CalculateEvent evt)
        {
            if (evt.ActiveDeals != null && evt.ActiveDeals.Count > 0)
            {
                evt.ActiveDeals.ForEach(
                    activeDeal =>
                        {
                            if (!this.activeDealsDic.ContainsKey(activeDeal.GetID()))
                            {
                                this.activeDealsDic.Add(activeDeal.GetID(), activeDeal);
                            }
                        });
            }
        }

        /// <summary>
        /// 添加活跃挂单的事件处理函数
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HandlingAddActiveOrders(CalculateEvent evt)
        {
            if (evt.ActiveOrders != null && evt.ActiveOrders.Count > 0)
            {
                evt.ActiveOrders.ForEach(
                    activeOrder =>
                        {
                            if (!this.activeOrdersDic.ContainsKey(activeOrder.GetID()))
                            {
                                this.activeOrdersDic.Add(activeOrder.GetID(), activeOrder);
                            }
                        });
            }
        }

        /// <summary>
        /// 移除活跃订单的事件处理函数
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HandlingRemoveActiveDeals(CalculateEvent evt)
        {
            if (evt.ActiveDeals != null && evt.ActiveDeals.Count > 0)
            {
                evt.ActiveDeals.ForEach(
                    activeDeal =>
                        {
                            if (this.activeDealsDic.ContainsKey(activeDeal.GetID()))
                            {
                                this.activeDealsDic.Remove(activeDeal.GetID());
                            }
                        });
            }
        }

        /// <summary>
        /// 移除活跃挂单的事件处理函数
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HandlingRemoveActiveOrders(CalculateEvent evt)
        {
            if (evt.ActiveOrders != null && evt.ActiveOrders.Count > 0)
            {
                evt.ActiveOrders.ForEach(
                    activeOrder =>
                        {
                            if (this.activeOrdersDic.ContainsKey(activeOrder.GetID()))
                            {
                                this.activeOrdersDic.Remove(activeOrder.GetID());
                            }
                        });
            }
        }

        /// <summary>
        /// 处理DealingAddTrade事件
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void HanldingAddDeal(CalculateEvent evt)
        {
            if (evt.NewDeal.Status == DealStatusEnum.Settled)
            {
                return;
            }

            PriceCore.Instance.InitialPriceCalcInfoForDeal(evt.NewDeal);
            this.RegistDealInPriceStruct(evt.NewDeal);

            this.CalculateDealWhenNewPriceNoNotify(evt.NewDeal);
            evt.NewDeal.FloatingPL = evt.NewDeal.FloatingPL;
            evt.NewDeal.MarketRate = evt.NewDeal.MarketRate;

            CustomerStructure custStruct = this.GetAcctStructureById(evt.NewDeal.CustomerNo);
            if (custStruct == null)
            {
                return;
            }

            custStruct.AddDeal(evt.NewDeal);
            custStruct.CalculateCustomerWhenNewPrice();
            custStruct.NotifyCustomer();
        }

        /// <summary>
        /// 处理挂单添加事件
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HanldingAddOrder(CalculateEvent evt)
        {
            BaseOrderVM newOrder = evt.NewOrder;
            if (newOrder == null)
            {
                TraceManager.Warn.Write("实时计算", "处理新Order添加时，新Order为空。");
                return;
            }

            this.AddPendingOrder(newOrder);
        }

        /// <summary>
        /// 处理新的抵押金事件
        /// </summary>
        /// <param name="evt">
        /// 新抵押金事件
        /// </param>
        private void HanldingNewPledge(CalculateEvent evt)
        {
            if (this.activeCustomerStructureDic.ContainsKey(evt.NewPledge.CustomerNo))
            {
                CustomerStructure custoemrStruct = this.activeCustomerStructureDic[evt.NewPledge.CustomerNo];
                custoemrStruct.AddPledge(evt.NewPledge);
                custoemrStruct.CalculateCustomerWhenNewPrice();
                custoemrStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 处理订单的删除
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void HanldingRemoveDeal(CalculateEvent evt)
        {
            CustomerStructure custStruct = this.GetAcctStructureById(evt.OldDeal.CustomerNo);
            if (custStruct == null)
            {
                return;
            }

            if (custStruct.RemoveDeal(evt.OldDeal))
            {
                custStruct.CalculateCustomerWhenNewPrice();
                custStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 处理挂单的删除
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HanldingRemoveOrder(CalculateEvent evt)
        {
            BaseOrderVM oldOrder = evt.OldOrder;
            if (oldOrder == null)
            {
                TraceManager.Warn.Write("实时计算", "处理Order移除时，旧Order为空。");
                return;
            }

            this.RemovePendingOrder(oldOrder);
        }

        /// <summary>
        /// 处理删除抵押金事件
        /// </summary>
        /// <param name="evt">
        /// 删除抵押金事件
        /// </param>
        private void HanldingRemovePledge(CalculateEvent evt)
        {
            if (this.activeCustomerStructureDic.ContainsKey(evt.OldPledge.CustomerNo))
            {
                CustomerStructure custoemrStruct = this.activeCustomerStructureDic[evt.OldPledge.CustomerNo];
                custoemrStruct.RemovePledge(evt.OldPledge);
                custoemrStruct.CalculateCustomerWhenNewPrice();
                custoemrStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 处理订单的修改
        /// </summary>
        /// <param name="evt">
        /// The evt.
        /// </param>
        private void HanldingUpdateDeal(CalculateEvent evt)
        {
            this.HanldingRemoveDeal(evt);
            this.HanldingAddDeal(evt);
        }

        /// <summary>
        /// 处理挂单的修改
        /// </summary>
        /// <param name="evt">
        /// 事件
        /// </param>
        private void HanldingUpdateOrder(CalculateEvent evt)
        {
            BaseOrderVM newOrder = evt.NewOrder;
            BaseOrderVM oldOrder = evt.OldOrder;
            if (newOrder == null || oldOrder == null)
            {
                TraceManager.Warn.Write("实时计算", "处理Order修改时，新Order或者旧Order为空。");
                return;
            }

            this.RemovePendingOrder(oldOrder);
            this.AddPendingOrder(newOrder);
        }

        /// <summary>
        /// 处理更新抵押金事件
        /// </summary>
        /// <param name="evt">
        /// 更新抵押金事件
        /// </param>
        private void HanldingUpdatePledge(CalculateEvent evt)
        {
            if (this.activeCustomerStructureDic.ContainsKey(evt.NewPledge.CustomerNo))
            {
                CustomerStructure custoemrStruct = this.activeCustomerStructureDic[evt.NewPledge.CustomerNo];
                custoemrStruct.CalculateCustomerWhenNewPrice();
                custoemrStruct.NotifyCustomer();
            }
        }

        /// <summary>
        ///     进行初始化
        /// </summary>
        private void Initial()
        {
            this.isInitial = true;
            var newPriceEvt = new CalculateEvent();
            newPriceEvt.EventType = CalculateEvent.EnumEventType.NewPrice;
            newPriceEvt.NewPriceList =
                this.GetRepository<IQuoteCacheRepository>().Filter(c => true).Select(c => c.PropSet.Clone()).ToList();

            this.DealingNewPrice(newPriceEvt);

            // 此处不能放在DealingNewPrice内部，因为DealingNewPrice里面每次处理一个报价
            // 一个订单可能关联多个报价，会造成查询时没有报价的情况，为避免此问题
            // 取得所有报价并调用DealingNewPrice后会将PriceDic填满，就不会出现上述问题
            this.newPriceCalcDealDic.Clear();
            foreach (BaseDealVM baseDealVm in this.dealRepository.Filter(c => true))
            {
                try
                {
                    this.newPriceCalcDealDic.Add(baseDealVm.GetID(), baseDealVm);
                    PriceCore.Instance.InitialPriceCalcInfoForDeal(baseDealVm);
                    this.RegistDealInPriceStruct(baseDealVm);
                }
                catch (Exception exception)
                {
                    TraceManager.Error.WriteAdditional("Calculate", baseDealVm, exception, "初始化订单的计算结构时发生异常");
                }
            }

            this.CalculateDealInDic(this.newPriceCalcDealDic);

            IEnumerable<BaseMarginCallVm> margincallList =
                this.GetRepository<IMarginCallCacheRepository>().Filter(c => true);
            foreach (BaseMarginCallVm baseMarginCallVm in margincallList)
            {
                this.DealingAddMarginCall(
                    new CalculateEvent
                        {
                            EventType = CalculateEvent.EnumEventType.AddMarginCall, 
                            MarginCallCustomerNo = baseMarginCallVm.CustoemrNo
                        });
            }

            this.ReorderMarginCallList();
        }

        /// <summary>
        /// 初始化客户结构
        /// </summary>
        /// <param name="custNo">
        /// The cust No.
        /// </param>
        private void InitialCustomerStructInActiveCustDic(string custNo)
        {
            if (!this.activeCustomerStructureDic.ContainsKey(custNo))
            {
                BaseCustomerViewModel custVm = this.customerRepository.FindByID(custNo);
                if (custVm == null)
                {
                    TraceManager.Warn.Write("实时计算", "初始化账户结构时，仓储中找不到对应的客户,客户Id：{0}", custNo);
                    return;
                }

                List<BaseDealVM> deals =
                    this.dealRepository.Filter(c => c.CustomerNo == custNo && c.PriceCalcInfo != null)
                        .ToList();
                List<BaseOrderVM> orders = this.orderRepository.Filter(c => c.CustomerNo == custNo).ToList();
                List<BasePledgeVM> pledgeList = this.pledgeRepository.Filter(c => c.CustomerNo == custNo).ToList();
                List<BaseCreditVM> credist = this.creditRepository.Filter(c => c.CustomerNo == custNo).ToList();
                BaseBusinessUnitVM busuniessUnit = this.businessUnitrRepository.FindByID(custVm.BusinessUnitID);
                var custStruct = new CustomerStructure(deals, orders, pledgeList, credist, busuniessUnit, custVm, this);
                this.activeCustomerStructureDic.Add(custNo, custStruct);
                this.OnActiveCustomerChanged(true);
                custStruct.CalculateCustomerWhenNewPrice();
                custStruct.NotifyCustomer();
            }
        }

        /// <summary>
        /// 内存监控定时执行Action
        /// </summary>
        /// <param name="state">
        /// 状态
        /// </param>
        private void MemoryMonitorAction(object state)
        {
            TraceManager.Info.Write("Memory management", "Begin force GC, current memory: " + this.GetMemoryUsageInMb());
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            TraceManager.Info.Write("Memory management", "End force GC, current memory: " + this.GetMemoryUsageInMb());
            this.memoryMonitorTimer.Change(MemoryMonitorInterval, Timeout.Infinite);
        }

        /// <summary>
        /// ActiveCustomer变更事件
        /// </summary>
        /// <param name="isAdd">
        /// true: add ActiveCustomer; false: remove ActiveCustomer
        /// </param>
        private void OnActiveCustomerChanged(bool isAdd)
        {
            if (isAdd)
            {
                if (!this.isSubcribePrice && this.activeCustomerStructureDic.Keys.Count == 1)
                {
                    PriceReceiveCore.Instance.ReadQuotePriceEvent += this.OnQuotePriceReceived;
                    this.isSubcribePrice = true;
                }
            }
            else
            {
                if (this.isSubcribePrice && this.activeCustomerStructureDic.Keys.Count == 0 && !this.isDealOrderListOpen)
                {
                    PriceReceiveCore.Instance.ReadQuotePriceEvent -= this.OnQuotePriceReceived;
                    this.isSubcribePrice = false;
                }
            }
        }

        /// <summary>
        /// 报价到达处理
        /// </summary>
        /// <param name="tickQuote">
        /// 报价
        /// </param>
        private void OnQuotePriceReceived(TickQuoteModel tickQuote)
        {
            this.PublishEvent(
                new CalculateEvent
                    {
                        EventType = CalculateEvent.EnumEventType.NewPrice, 
                        NewSinglePrice = tickQuote.Clone()
                    });
        }

        /// <summary>
        ///     发布配置变更事件
        /// </summary>
        private void PublishUpdateSettingsEvent()
        {
            this.PublishEvent(new CalculateEvent { EventType = CalculateEvent.EnumEventType.UpdateSettings });
        }

        /// <summary>
        /// 向报价结构中注册订单
        /// </summary>
        /// <param name="deal">
        /// 订单信息
        /// </param>
        private void RegistDealInPriceStruct(BaseDealVM deal)
        {
            if (deal.PriceCalcInfo == null)
            {
                TraceManager.Warn.WriteAdditional("Calculate", deal, "PriceCalcInfo not initial");
                return;
            }

            foreach (string symbolId in deal.PriceCalcInfo.PriceRelatedSymbolIdList)
            {
                if (this.priceDic.ContainsKey(symbolId))
                {
                    if (!this.priceDic[symbolId].DealList.ContainsKey(deal.GetID()))
                    {
                        this.priceDic[symbolId].DealList.Add(deal.GetID(), deal);
                    }

                    if (symbolId == deal.PriceCalcInfo.PlccyToLocalCcySymbol)
                    {
                        deal.NewTrancePrice = this.priceDic[symbolId].Price.Clone();
                    }
                }
                else
                {
                    TraceManager.Warn.WriteAdditional(
                        "Calculate", 
                        deal, 
                        "Cant find related price structure, symbol:{0}", 
                        symbolId);
                }
            }
        }

        /// <summary>
        ///     注册会导致CalcPriceInfo结构变化的仓储变更事件
        ///     1. Forward Point
        ///     2. Symbol
        ///     3. Quote Group
        ///     4. Business Unit
        /// </summary>
        private void RegisterUpdateSettingsEvent()
        {
            this.forwardPointRepository.SubscribeUpdateEvent((oldVm, newVm) => this.PublishUpdateSettingsEvent());
            this.symbolRepository.SubscribeUpdateEvent((oldVm, newVm) => this.PublishUpdateSettingsEvent());
            this.quoteGroupRepository.SubscribeUpdateEvent((oldVm, newVm) => this.PublishUpdateSettingsEvent());
            this.businessUnitrRepository.SubscribeUpdateEvent((oldVm, newVm) => this.PublishUpdateSettingsEvent());
        }

        /// <summary>
        /// 从系统中移除挂单信息
        /// </summary>
        /// <param name="removeOrder">
        /// 移除的挂单信息
        /// </param>
        private void RemovePendingOrder(BaseOrderVM removeOrder)
        {
            PriceStructure directPriceStruct = this.GetPriceStructureBySymbol(removeOrder.Symbol);
            if (directPriceStruct == null)
            {
                TraceManager.Warn.WriteAdditional("实时计算", removeOrder, "处理Order移除时，直接报价结构中尚不存在");
            }
            else
            {
                if (directPriceStruct.PendingDealList.ContainsKey(removeOrder.GetID()))
                {
                    directPriceStruct.PendingDealList.Remove(removeOrder.GetID());
                }
                else
                {
                    TraceManager.Warn.WriteAdditional("实时计算", removeOrder, "处理Order移除时，挂单并没有在直接报价结构中存在");
                }
            }

            CustomerStructure custStruct = this.GetAcctStructureById(removeOrder.CustomerNo);
            if (custStruct == null)
            {
                return;
            }

            custStruct.RemoveOrder(removeOrder);
            custStruct.CalculateCustomerWhenNewPrice();
            custStruct.NotifyCustomer();
        }

        /// <summary>
        ///     MarginCall列表重新排序
        /// </summary>
        private void ReorderMarginCallList()
        {
            IOrderedEnumerable<BaseMarginCallVm> orderedMarginCallList =
                this.orderedMarginCallList.ToList()
                    .OrderBy(item => Math.Abs(item.BelongCustomer.MarginRatio - item.BelongCustomer.ForceSellLevel));
            Application.Current.Dispatcher.Invoke(
                () =>
                    {
                        this.orderedMarginCallList.Clear();
                        orderedMarginCallList.ForEach(item => this.orderedMarginCallList.Add(item));
                    });
        }

        /// <summary>
        ///     启动内存监控
        /// </summary>
        private void StartMemoryMonitor()
        {
            if (ConfigParameter.EnabelPerformanceCounters)
            {
                if (this.memoryCounter == null)
                {
                    try
                    {
                        TraceManager.Info.Write("Memory management", "Start memory monitor");

                        // 初始化性能收集器
                        Process cur = Process.GetCurrentProcess();
                        this.memoryCounter = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);
                        this.memoryMonitorTimer = new Timer(
                            this.MemoryMonitorAction, 
                            null, 
                            MemoryMonitorInterval, 
                            Timeout.Infinite);
                    }
                    catch (Exception exception)
                    {
                        ConfigParameter.EnabelPerformanceCounters = false;
                        TraceManager.Error.Write("MemoryMonitor", exception, "Exception when monitor,Stop monitor。");
                    }
                }
            }
        }

        /// <summary>
        /// 更新挂单报价信息
        /// </summary>
        /// <param name="order">
        /// 挂单
        /// </param>
        private void UpdateOrderMarketPriceUnNotify(BaseOrderVM order)
        {
            if (order.TriggerStatus == OrderTriggerEnum.Triggered)
            {
                return;
            }

            // 挂单的TraderSpotRate不应变化
            PriceStructure varPriceStru;
            if (!this.priceDic.TryGetValue(order.Symbol, out varPriceStru))
            {
                TraceManager.Warn.WriteAdditional("实时计算", order, "找不到订单对应的报价结构");
                return;
            }

            // 设置最新报价
            order.PendingOrderModel.MarketTraderSpotRateBid = varPriceStru.Price.TraderBid;
            order.PendingOrderModel.MarketTraderSpotRateAsk = varPriceStru.Price.TraderAsk;

            BaseSymbolVM symbol = this.symbolRepository.FindByID(order.Symbol);
            order.Spread = Math.Abs(order.MarketTraderSpotRate - order.TraderSpotRate)
                           * Convert.ToDecimal(Math.Pow(10.0, symbol.BasisPoint));
        }

        /// <summary>
        /// 更新挂单列表中的挂单信息
        /// </summary>
        /// <param name="pendingList">
        /// 挂单信息
        /// </param>
        private void UpdatePendingOrderInDic(Dictionary<string, BaseOrderVM> pendingList)
        {
            // 处理挂单
            foreach (BaseOrderVM varDeal in pendingList.Values)
            {
                // 更新订单的当前市场价
                this.UpdateOrderMarketPriceUnNotify(varDeal);
            }

            if (pendingList.Count > 0)
            {
                ObjectPoolItem<List<BaseOrderVM>> notifyPendingList = this.orderlistObjectPool.GetPoolItem();
                foreach (BaseOrderVM dealItem in pendingList.Values)
                {
                    if (dealItem.TriggerStatus == OrderTriggerEnum.None)
                    {
                        notifyPendingList.Value.Add(dealItem);
                    }
                }

                // 使用Invoke的方式，在UI线程中修改MarketPrice
                Application.Current.Dispatcher.Invoke(
                    new Action(
                        () =>
                            {
                                try
                                {
                                    foreach (BaseOrderVM orderVm in notifyPendingList.Value)
                                    {
                                        orderVm.NotifyMarketTraderSpotRate();
                                    }
                                }
                                catch (Exception exception)
                                {
                                    TraceManager.Error.Write("Service", exception, "界面通知时后台变更，此错误无业务影响", exception);
                                }
                                finally
                                {
                                    notifyPendingList.Close();
                                }
                            }), 
                    null);
            }
        }

        #endregion
    }
}