// <copyright file="OdAlertCalculateModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/08/02 05:02:58 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/08/02 05:02:58
//      修改描述：新建 OdAlertCalculateModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models.AccountRealTime
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    using BaseViewModel;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Data.Base;
    using Infrastructure.Log;
    using Infrastructure.Models;

    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    ///     The od alert calculate model.
    /// </summary>
    public class OdAlertCalculateModel : BaseModel
    {
        #region Constants

        /// <summary>
        ///     定期检查ValueDate的时间间隔(ms)
        /// </summary>
        private const int CheckValueDateTimerInterval = 60 * 1000;

        #endregion

        #region Fields

        /// <summary>
        ///     Bu仓储
        /// </summary>
        private readonly IBusinessUnitCacheRepository businessUnitRepository;

        /// <summary>
        ///     账户组仓储
        /// </summary>
        private readonly ICustomerGroupCacheRepository customerGroupRepository;

        /// <summary>
        ///     账户仓储
        /// </summary>
        private readonly ICustomerCacheRepository customerRepository;

        /// <summary>
        ///     订单仓储
        /// </summary>
        private readonly IDealCacheRepository dealRepository;

        /// <summary>
        ///     高级别实时计算事件缓存队列，用于计算控制
        /// </summary>
        private readonly BlockingCollection<OdAlertCalculateEvent> highEventQueue =
            new BlockingCollection<OdAlertCalculateEvent>();

        /// <summary>
        ///     新计算事件通知
        /// </summary>
        private readonly AutoResetEvent newCalcEventNotify = new AutoResetEvent(false);

        /// <summary>
        ///     实时计算事件缓存队列，用于报价之外的业务变更事件
        /// </summary>
        private readonly BlockingCollection<OdAlertCalculateEvent> normalEventQueue =
            new BlockingCollection<OdAlertCalculateEvent>();

        /// <summary>
        ///     ValueDate为Key的订单列表
        /// </summary>
        private readonly Dictionary<DateTime, List<BaseDealVM>> valueDateDealDic =
            new Dictionary<DateTime, List<BaseDealVM>>();

        /// <summary>
        ///     The check value date timer.
        /// </summary>
        private Timer checkValueDateTimer;

        /// <summary>
        ///     是否结束
        /// </summary>
        private bool isEnded;

        /// <summary>
        ///     是否初始化
        /// </summary>
        private bool isInitial;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OdAlertCalculateModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", 
            Justification = "Reviewed. Suppression is OK here.")]
        public OdAlertCalculateModel(string ownerId)
            : base(ownerId)
        {
            this.customerRepository = this.GetRepository<ICustomerCacheRepository>();
            this.dealRepository = this.GetRepository<IDealCacheRepository>();
            this.businessUnitRepository = this.GetRepository<IBusinessUnitCacheRepository>();
            this.customerGroupRepository = this.GetRepository<ICustomerGroupCacheRepository>();

            // 订阅仓储变更事件
            this.customerRepository.SubscribeAddEvent(
                newItem =>
                this.PublishEvent(
                    new OdAlertCalculateEvent
                        {
                            Event = OdAlertCalculateEvent.EventType.NewCustomer, 
                            NewCustomer = newItem, 
                        }));

            this.businessUnitRepository.SubscribeUpdateEvent(
                (oldItem, newItem) =>
                this.PublishEvent(
                    new OdAlertCalculateEvent
                        {
                            Event = OdAlertCalculateEvent.EventType.UpdateBusinessUnit, 
                            NewBu = newItem, 
                            OldBu = oldItem, 
                        }));

            this.dealRepository.SubscribeAddEvent(
                newItem =>
                this.PublishEvent(
                    new OdAlertCalculateEvent { Event = OdAlertCalculateEvent.EventType.NewDeal, NewDeal = newItem, }));

            this.dealRepository.SubscribeUpdateEvent(
                (newItem, oldItem) =>
                this.PublishEvent(
                    new OdAlertCalculateEvent
                        {
                            Event = OdAlertCalculateEvent.EventType.UpdateDeal, 
                            NewDeal = newItem, 
                            OldDeal = oldItem
                        }));

            this.dealRepository.SubscribeRemoveEvent(
                oldItem =>
                this.PublishEvent(
                    new OdAlertCalculateEvent { Event = OdAlertCalculateEvent.EventType.RemoveDeal, OldDeal = oldItem, }));

            ThreadPool.QueueUserWorkItem(
                o =>
                    {
                        Util.SetThreadName("OdAlertCalculateThread");
                        while (true)
                        {
                            this.newCalcEventNotify.WaitOne();
                            TraceManager.Debug.Write(
                                "ODAlertCalculateModel", 
                                "Queue items count: {0}", 
                                this.normalEventQueue.Count);

                            OdAlertCalculateEvent item;
                            while (this.highEventQueue.TryTake(out item))
                            {
                                this.ConsumerCalculateEvent(item);
                            }

                            while (this.normalEventQueue.TryTake(out item))
                            {
                                this.ConsumerCalculateEvent(item);
                            }

                            if (this.isEnded)
                            {
                                return;
                            }
                        }
                    });
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 发布通知事件
        /// </summary>
        /// <param name="evt">
        /// 通知事件
        /// </param>
        public void PublishEvent(OdAlertCalculateEvent evt)
        {
            if (evt == null)
            {
                return;
            }

            if (!this.isInitial && evt.Event != OdAlertCalculateEvent.EventType.Initail)
            {
                return;
            }

            switch (evt.Event)
            {
                case OdAlertCalculateEvent.EventType.Initail:
                case OdAlertCalculateEvent.EventType.Terminal:
                    this.highEventQueue.Add(evt);
                    break;
                case OdAlertCalculateEvent.EventType.NewCustomer:
                case OdAlertCalculateEvent.EventType.UpdateBusinessUnit:
                case OdAlertCalculateEvent.EventType.NewDeal:
                case OdAlertCalculateEvent.EventType.UpdateDeal:
                case OdAlertCalculateEvent.EventType.RemoveDeal:
                case OdAlertCalculateEvent.EventType.CheckValueDateChanged:
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
        /// 向ValueDate对订单的字典中新增订单
        /// </summary>
        /// <param name="newDeal">
        /// 新订单
        /// </param>
        private void AddNewDealToValueDateDealDic(BaseDealVM newDeal)
        {
            if (newDeal == null)
            {
                return;
            }

            if (this.valueDateDealDic.ContainsKey(newDeal.ValueDate.Date))
            {
                if (this.valueDateDealDic[newDeal.ValueDate.Date].Any(o => o.ExecutionID == newDeal.ExecutionID))
                {
                    this.valueDateDealDic[newDeal.ValueDate.Date].RemoveAll(o => o.ExecutionID == newDeal.ExecutionID);
                }

                this.valueDateDealDic[newDeal.ValueDate.Date].Add(newDeal);
            }
            else
            {
                this.valueDateDealDic.Add(newDeal.ValueDate.Date, new List<BaseDealVM> { newDeal });
            }
        }

        /// <summary>
        /// 计算当天需要结算的金额
        /// </summary>
        /// <param name="dealList">
        /// 交易单集合
        /// </param>
        /// <param name="currencyId">
        /// 货币ID
        /// </param>
        /// <returns>
        /// 返回计算出来当天的总共的金额
        /// </returns>
        private decimal AvaliableBlance(List<DealModel> dealList, string currencyId)
        {
            decimal buy = dealList.Sum(
                p =>
                    {
                        if (p.CCY1 == currencyId && p.TransactionType == TransactionTypeEnum.Buy)
                        {
                            return p.CCY1Amount;
                        }

                        if (p.CCY2 == currencyId && p.TransactionType == TransactionTypeEnum.Sell)
                        {
                            return p.CCY2Amount;
                        }

                        return decimal.Zero;
                    });
            decimal sell = dealList.Sum(
                p =>
                    {
                        if (p.CCY1 == currencyId && p.TransactionType == TransactionTypeEnum.Sell)
                        {
                            return p.CCY1Amount;
                        }

                        if (p.CCY2 == currencyId && p.TransactionType == TransactionTypeEnum.Buy)
                        {
                            return p.CCY2Amount;
                        }

                        return decimal.Zero;
                    });
            return buy - sell;
        }

        /// <summary>
        /// 当前时间是否超过了结算时间
        /// </summary>
        /// <param name="tradeWeekDay">
        /// 星期
        /// </param>
        /// <param name="time">
        /// BU时间
        /// </param>
        /// <param name="businessUnit">
        /// 业务区Id
        /// </param>
        /// <returns>
        /// true: 已经过了结算时间
        /// </returns>
        private bool BeyondSettleTime(WeekDayEnum tradeWeekDay, DateTime time, BaseBusinessUnitVM businessUnit)
        {
            BusinessUnitModel.BUGLSettlementTime settlementTime =
                businessUnit.GLSettlementTimes.FirstOrDefault(o => o.WeekDayID == tradeWeekDay);
            if (settlementTime == null)
            {
                return false;
            }

            if (time.TimeOfDay >= settlementTime.GLTime.TimeOfDay)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     检查ValueDate是否变化的响应
        /// </summary>
        private void CheckValueDateChanged()
        {
            this.businessUnitRepository.Filter(o => true).ForEach(this.ReCalculateOdAlertInfo);
        }

        /// <summary>
        /// 消费OdAlertCalculateEvent事件
        /// </summary>
        /// <param name="event">
        /// 事件
        /// </param>
        private void ConsumerCalculateEvent(OdAlertCalculateEvent @event)
        {
            try
            {
                switch (@event.Event)
                {
                    case OdAlertCalculateEvent.EventType.Initail:
                        this.Initial();
                        break;
                    case OdAlertCalculateEvent.EventType.Terminal:
                        this.isEnded = true;
                        break;
                    case OdAlertCalculateEvent.EventType.NewCustomer:
                        this.HandleNewCustomer(@event.NewCustomer);
                        break;
                    case OdAlertCalculateEvent.EventType.UpdateBusinessUnit:
                        this.HandleUpdateBusinessUnit(@event.OldBu, @event.NewBu);
                        break;
                    case OdAlertCalculateEvent.EventType.NewDeal:
                        this.HandleNewDeal(@event.NewDeal);
                        break;
                    case OdAlertCalculateEvent.EventType.UpdateDeal:
                        this.HandleUpdateDeal(@event.OldDeal, @event.NewDeal);
                        break;
                    case OdAlertCalculateEvent.EventType.RemoveDeal:
                        this.HandleRemoveDeal(@event.OldDeal);
                        break;
                    case OdAlertCalculateEvent.EventType.CheckValueDateChanged:
                        this.CheckValueDateChanged();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception)
            {
                TraceManager.Error.Write("ODAlertCalculateModel", exception);
            }
        }

        /// <summary>
        /// 新用户创建处理程序
        /// </summary>
        /// <param name="newCustomer">
        /// 新客户
        /// </param>
        private void HandleNewCustomer(BaseCustomerViewModel newCustomer)
        {
            BaseBusinessUnitVM businessUnit = this.businessUnitRepository.GetByID(newCustomer.BusinessUnitID);
            if (businessUnit == null)
            {
                return;
            }

            DateTime businessUnitTime = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(businessUnit.BusinessUnitID);
            DateTime localTradeDate = businessUnit.GetLocalTradeDate(businessUnitTime);
            var tradeWeekDay = (WeekDayEnum)((int)localTradeDate.DayOfWeek);
            if (!this.BeyondSettleTime(tradeWeekDay, businessUnitTime, businessUnit))
            {
                localTradeDate = localTradeDate.AddDays(1);
            }

            newCustomer.FirstValueDate = ValueDateCore.Instance.GetValueDateForReport(localTradeDate).Date;
            newCustomer.SecondValueDate =
                ValueDateCore.Instance.GetValueDateForReport(newCustomer.FirstValueDate.AddDays(1)).Date;
            newCustomer.ThirdValueDate =
                ValueDateCore.Instance.GetValueDateForReport(newCustomer.SecondValueDate.AddDays(1)).Date;
        }

        /// <summary>
        /// 新订单处理程序
        /// </summary>
        /// <param name="newDeal">
        /// 新订单
        /// </param>
        private void HandleNewDeal(BaseDealVM newDeal)
        {
            if (newDeal.Status == DealStatusEnum.Open)
            {
                BaseCustomerViewModel customer = this.customerRepository.FindByID(newDeal.CustomerNo);
                if (customer == null)
                {
                    return;
                }

                if (newDeal.ValueDate.Date == customer.FirstValueDate.Date)
                {
                    customer.SettlementAccounts.Where(o => o.CurrencyID == newDeal.CCY1 || o.CurrencyID == newDeal.CCY2)
                        .ForEach(
                            p =>
                                {
                                    p.DealsOfFirstValueDate.Add(newDeal.propSet);
                                    decimal balance = this.AvaliableBlance(
                                        new List<DealModel> { newDeal.propSet }, 
                                        p.CurrencyID);
                                    p.ExpectBalanceOfFirstValueDate += balance;
                                    p.ExpectBalanceOfSecondValueDate += balance;
                                    p.ExpectBalanceOfThirdValueDate += balance;
                                });
                }
                else if (newDeal.ValueDate.Date == customer.SecondValueDate.Date)
                {
                    customer.SettlementAccounts.Where(o => o.CurrencyID == newDeal.CCY1 || o.CurrencyID == newDeal.CCY2)
                        .ForEach(
                            p =>
                                {
                                    p.DealsOfSecondValueDate.Add(newDeal.propSet);
                                    decimal balance = this.AvaliableBlance(
                                        new List<DealModel> { newDeal.propSet }, 
                                        p.CurrencyID);
                                    p.ExpectBalanceOfSecondValueDate += balance;
                                    p.ExpectBalanceOfThirdValueDate += balance;
                                });
                }
                else if (newDeal.ValueDate.Date == customer.ThirdValueDate.Date)
                {
                    customer.SettlementAccounts.Where(o => o.CurrencyID == newDeal.CCY1 || o.CurrencyID == newDeal.CCY2)
                        .ForEach(
                            p =>
                                {
                                    p.DealsOfThirdValueDate.Add(newDeal.propSet);
                                    decimal balance = this.AvaliableBlance(
                                        new List<DealModel> { newDeal.propSet }, 
                                        p.CurrencyID);
                                    p.ExpectBalanceOfThirdValueDate += balance;
                                });
                }

                this.AddNewDealToValueDateDealDic(newDeal);
            }
        }

        /// <summary>
        /// 移除订单处理程序
        /// </summary>
        /// <param name="oldDeal">
        /// 移除的订单
        /// </param>
        private void HandleRemoveDeal(BaseDealVM oldDeal)
        {
            BaseCustomerViewModel customer = this.customerRepository.FindByID(oldDeal.CustomerNo);
            if (customer == null)
            {
                return;
            }

            if (oldDeal.ValueDate.Date == customer.FirstValueDate.Date)
            {
                customer.SettlementAccounts.Where(o => o.CurrencyID == oldDeal.CCY1 || o.CurrencyID == oldDeal.CCY2)
                    .ForEach(
                        p =>
                            {
                                p.DealsOfFirstValueDate.RemoveAll(q => q.ExecutionID == oldDeal.ExecutionID);
                                decimal balance = this.AvaliableBlance(
                                    new List<DealModel> { oldDeal.propSet }, 
                                    p.CurrencyID);
                                p.ExpectBalanceOfFirstValueDate -= balance;
                                p.ExpectBalanceOfSecondValueDate -= balance;
                                p.ExpectBalanceOfThirdValueDate -= balance;
                            });
            }
            else if (oldDeal.ValueDate.Date == customer.SecondValueDate.Date)
            {
                customer.SettlementAccounts.Where(o => o.CurrencyID == oldDeal.CCY1 || o.CurrencyID == oldDeal.CCY2)
                    .ForEach(
                        p =>
                            {
                                p.DealsOfSecondValueDate.RemoveAll(q => q.ExecutionID == oldDeal.ExecutionID);
                                decimal balance = this.AvaliableBlance(
                                    new List<DealModel> { oldDeal.propSet }, 
                                    p.CurrencyID);
                                p.ExpectBalanceOfSecondValueDate -= balance;
                                p.ExpectBalanceOfThirdValueDate -= balance;
                            });
            }
            else if (oldDeal.ValueDate.Date == customer.ThirdValueDate.Date)
            {
                customer.SettlementAccounts.Where(o => o.CurrencyID == oldDeal.CCY1 || o.CurrencyID == oldDeal.CCY2)
                    .ForEach(
                        p =>
                            {
                                p.DealsOfThirdValueDate.RemoveAll(q => q.ExecutionID == oldDeal.ExecutionID);
                                decimal balance = this.AvaliableBlance(
                                    new List<DealModel> { oldDeal.propSet }, 
                                    p.CurrencyID);
                                p.ExpectBalanceOfThirdValueDate -= balance;
                            });
            }

            this.RemoveDealInValueDateDealDic(oldDeal);
        }

        /// <summary>
        /// 业务区内容更新处理程序
        /// </summary>
        /// <param name="oldBusinessUnit">
        /// 原BU
        /// </param>
        /// <param name="newBusinessUnit">
        /// 新BU
        /// </param>
        private void HandleUpdateBusinessUnit(BaseBusinessUnitVM oldBusinessUnit, BaseBusinessUnitVM newBusinessUnit)
        {
            IEnumerable<BusinessUnitModel.BUGLSettlementTime> different =
                newBusinessUnit.GLSettlementTimes.Except(oldBusinessUnit.GLSettlementTimes);
            if (different.Any())
            {
                this.ReCalculateOdAlertInfo(newBusinessUnit);
            }
        }

        /// <summary>
        /// 更新订单处理程序
        /// </summary>
        /// <param name="oldDeal">
        /// 原订单
        /// </param>
        /// <param name="newDeal">
        /// 新订单
        /// </param>
        private void HandleUpdateDeal(BaseDealVM oldDeal, BaseDealVM newDeal)
        {
            if (this.valueDateDealDic.ContainsKey(oldDeal.ValueDate.Date)
                && this.valueDateDealDic[oldDeal.ValueDate.Date].Any(o => o.ExecutionID == oldDeal.ExecutionID))
            {
                this.HandleRemoveDeal(oldDeal);
                if (newDeal.Status == DealStatusEnum.Open)
                {
                    this.HandleNewDeal(newDeal);
                }
            }
            else
            {
                this.HandleNewDeal(newDeal);
            }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        private void Initial()
        {
            TraceManager.Info.Write("ODAlertCalculateModel", "Initial start");

            // 添加所有订单到字典中
            this.dealRepository.Filter(o => o.Status == DealStatusEnum.Open).ForEach(this.AddNewDealToValueDateDealDic);
            IEnumerable<IGrouping<string, BaseCustomerViewModel>> customerGroupByCustomerGroup =
                this.customerRepository.Filter(o => true).GroupBy(o => o.CustGroupID);
            foreach (var item in customerGroupByCustomerGroup)
            {
                BaseCustomerGroupVM customerGroup = this.customerGroupRepository.FindByID(item.Key);
                if (customerGroup == null)
                {
                    continue;
                }

                BaseBusinessUnitVM businessUnit = this.businessUnitRepository.GetByID(customerGroup.BusinessUnitID);
                if (businessUnit == null)
                {
                    continue;
                }

                DateTime businessUnitTime = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(businessUnit.BusinessUnitID);
                DateTime localTradeDate = businessUnit.GetLocalTradeDate(businessUnitTime);
                var tradeWeekDay = (WeekDayEnum)((int)localTradeDate.DayOfWeek);
                if (!this.BeyondSettleTime(tradeWeekDay, businessUnitTime, businessUnit))
                {
                    localTradeDate = localTradeDate.AddDays(1);
                }

                DateTime firstValueDate = ValueDateCore.Instance.GetValueDateForReport(localTradeDate).Date;
                DateTime secondValueDate = ValueDateCore.Instance.GetValueDateForReport(firstValueDate.AddDays(1)).Date;
                DateTime thirdValueDate = ValueDateCore.Instance.GetValueDateForReport(secondValueDate.AddDays(1)).Date;
                item.ForEach(
                    o =>
                        {
                            o.FirstValueDate = firstValueDate;
                            o.SecondValueDate = secondValueDate;
                            o.ThirdValueDate = thirdValueDate;
                            o.SettlementAccounts.ForEach(
                                p =>
                                    {
                                        if (this.valueDateDealDic.ContainsKey(o.FirstValueDate))
                                        {
                                            p.DealsOfFirstValueDate =
                                                this.valueDateDealDic[o.FirstValueDate].Where(
                                                    q =>
                                                    q.CustomerNo == o.CustmerNo
                                                    && (p.CurrencyID == q.CCY1 || p.CurrencyID == q.CCY2))
                                                    .Select(m => m.propSet)
                                                    .ToList();
                                        }

                                        if (this.valueDateDealDic.ContainsKey(o.SecondValueDate))
                                        {
                                            p.DealsOfSecondValueDate =
                                                this.valueDateDealDic[o.SecondValueDate].Where(
                                                    q =>
                                                    q.CustomerNo == o.CustmerNo
                                                    && (p.CurrencyID == q.CCY1 || p.CurrencyID == q.CCY2))
                                                    .Select(m => m.propSet)
                                                    .ToList();
                                        }

                                        if (this.valueDateDealDic.ContainsKey(o.ThirdValueDate))
                                        {
                                            p.DealsOfThirdValueDate =
                                                this.valueDateDealDic[o.ThirdValueDate].Where(
                                                    q =>
                                                    q.CustomerNo == o.CustmerNo
                                                    && (p.CurrencyID == q.CCY1 || p.CurrencyID == q.CCY2))
                                                    .Select(m => m.propSet)
                                                    .ToList();
                                        }

                                        p.ExpectBalanceOfFirstValueDate = p.AvailableBalance
                                                                          + this.AvaliableBlance(
                                                                              p.DealsOfFirstValueDate, 
                                                                              p.CurrencyID);
                                        p.ExpectBalanceOfSecondValueDate = p.ExpectBalanceOfFirstValueDate
                                                                           + this.AvaliableBlance(
                                                                               p.DealsOfSecondValueDate, 
                                                                               p.CurrencyID);
                                        p.ExpectBalanceOfThirdValueDate = p.ExpectBalanceOfSecondValueDate
                                                                          + this.AvaliableBlance(
                                                                              p.DealsOfThirdValueDate, 
                                                                              p.CurrencyID);
                                    });
                        });
            }

            // 启动检查ValueDate过期的Timer
            this.checkValueDateTimer = new Timer(
                this.PublishCheckValueDateEvent, 
                null, 
                CheckValueDateTimerInterval, 
                Timeout.Infinite);

            this.isInitial = true;
            TraceManager.Info.Write("ODAlertCalculateModel", "Initial end");
        }

        /// <summary>
        /// 发布检查ValueDate变更事件
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void PublishCheckValueDateEvent(object state)
        {
            this.PublishEvent(
                new OdAlertCalculateEvent { Event = OdAlertCalculateEvent.EventType.CheckValueDateChanged, });
            this.checkValueDateTimer.Change(CheckValueDateTimerInterval, Timeout.Infinite);
        }

        /// <summary>
        /// The on business unit value date changed.
        /// </summary>
        /// <param name="businessUnit">
        /// The business unit.
        /// </param>
        private void ReCalculateOdAlertInfo(BaseBusinessUnitVM businessUnit)
        {
            DateTime businessUnitTime = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(businessUnit.BusinessUnitID);
            DateTime localTradeDate = businessUnit.GetLocalTradeDate(businessUnitTime);
            var tradeWeekDay = (WeekDayEnum)((int)localTradeDate.DayOfWeek);
            if (!this.BeyondSettleTime(tradeWeekDay, businessUnitTime, businessUnit))
            {
                localTradeDate = localTradeDate.AddDays(1);
            }

            DateTime firstValueDate = ValueDateCore.Instance.GetValueDateForReport(localTradeDate).Date;
            DateTime secondValueDate = ValueDateCore.Instance.GetValueDateForReport(firstValueDate.AddDays(1)).Date;
            DateTime thirdValueDate = ValueDateCore.Instance.GetValueDateForReport(secondValueDate.AddDays(1)).Date;

            this.customerRepository.Filter(o => o.BusinessUnitID == businessUnit.BusinessUnitID).ForEach(
                o =>
                    {
                        // ValueDate有变化
                        if (
                            !(o.FirstValueDate == firstValueDate && o.SecondValueDate == secondValueDate
                              && o.ThirdValueDate == thirdValueDate))
                        {
                            o.FirstValueDate = firstValueDate;
                            o.SecondValueDate = secondValueDate;
                            o.ThirdValueDate = thirdValueDate;
                            o.SettlementAccounts.ForEach(
                                p =>
                                    {
                                        if (this.valueDateDealDic.ContainsKey(o.FirstValueDate))
                                        {
                                            p.DealsOfFirstValueDate =
                                                this.valueDateDealDic[o.FirstValueDate].Where(
                                                    q =>
                                                    q.CustomerNo == o.CustmerNo
                                                    && (p.CurrencyID == q.CCY1 || p.CurrencyID == q.CCY2))
                                                    .Select(m => m.propSet)
                                                    .ToList();
                                        }

                                        if (this.valueDateDealDic.ContainsKey(o.SecondValueDate))
                                        {
                                            p.DealsOfSecondValueDate =
                                                this.valueDateDealDic[o.SecondValueDate].Where(
                                                    q =>
                                                    q.CustomerNo == o.CustmerNo
                                                    && (p.CurrencyID == q.CCY1 || p.CurrencyID == q.CCY2))
                                                    .Select(m => m.propSet)
                                                    .ToList();
                                        }

                                        if (this.valueDateDealDic.ContainsKey(o.ThirdValueDate))
                                        {
                                            p.DealsOfThirdValueDate =
                                                this.valueDateDealDic[o.ThirdValueDate].Where(
                                                    q =>
                                                    q.CustomerNo == o.CustmerNo
                                                    && (p.CurrencyID == q.CCY1 || p.CurrencyID == q.CCY2))
                                                    .Select(m => m.propSet)
                                                    .ToList();
                                        }

                                        p.ExpectBalanceOfFirstValueDate = p.AvailableBalance
                                                                          + this.AvaliableBlance(
                                                                              p.DealsOfFirstValueDate, 
                                                                              p.CurrencyID);
                                        p.ExpectBalanceOfSecondValueDate = p.ExpectBalanceOfFirstValueDate
                                                                           + this.AvaliableBlance(
                                                                               p.DealsOfSecondValueDate, 
                                                                               p.CurrencyID);
                                        p.ExpectBalanceOfThirdValueDate = p.ExpectBalanceOfSecondValueDate
                                                                          + this.AvaliableBlance(
                                                                              p.DealsOfThirdValueDate, 
                                                                              p.CurrencyID);
                                    });
                        }
                    });
        }

        /// <summary>
        /// 从Vluedate订单列表中移除订单ValueDate
        /// </summary>
        /// <param name="oldDeal">
        /// 移除的订单
        /// </param>
        private void RemoveDealInValueDateDealDic(BaseDealVM oldDeal)
        {
            if (oldDeal == null)
            {
                return;
            }

            if (this.valueDateDealDic.ContainsKey(oldDeal.ValueDate.Date))
            {
                this.valueDateDealDic[oldDeal.ValueDate.Date].RemoveAll(o => o.ExecutionID == oldDeal.ExecutionID);
            }
        }

        #endregion
    }
}