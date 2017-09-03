// <copyright file="CaculateModel_Patial.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/2/24 11:34:35 </date>
// <summary> 实时计算Model </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/2/24 11:34:35
//      修改描述：新建 CaculateModel_Patial.cs
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    using BaseViewModel;

    using Infrastructure;
    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Data.Tools;
    using Infrastructure.Log;
    using Infrastructure.Models;

    #endregion

    /// <summary>
    ///     客户端实时中心
    /// </summary>
    public partial class CaculateModel
    {
        #region Fields

        /// <summary>
        ///     活跃账户信息缓存字典
        /// </summary>
        private readonly Dictionary<string, CustomerStructure> activeCustomerStructureDic =
            new Dictionary<string, CustomerStructure>();

        /// <summary>
        ///     实时计算后台线程
        /// </summary>
        private readonly Thread backgroudThread;

        /// <summary>
        ///     Bu仓储
        /// </summary>
        private readonly IBusinessUnitCacheRepository businessUnitrRepository;

        /// <summary>
        ///     授信仓储
        /// </summary>
        private readonly ICreditCacheRepository creditRepository;

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyCacheRepository currencyRepository;

        /// <summary>
        ///     账户仓储
        /// </summary>
        private readonly ICustomerCacheRepository customerRepository;

        /// <summary>
        ///     订单仓储
        /// </summary>
        private readonly IDealCacheRepository dealRepository;

        /// <summary>
        ///     远期点数仓储
        /// </summary>
        private readonly IForwardPointCacheRepository forwardPointRepository;

        /// <summary>
        ///     全局通用配置仓储
        /// </summary>
        private readonly IGeneralSettingCacheRepository generalSettingRepository;

        /// <summary>
        ///     高级别实时计算事件缓存队列，用于计算控制
        /// </summary>
        private readonly BlockingCollection<CalculateEvent> highEventQueue = new BlockingCollection<CalculateEvent>();

        /// <summary>
        ///     margincall用户列表
        /// </summary>
        private readonly HashSet<string> margincallAccts = new HashSet<string>();

        /// <summary>
        ///     新计算事件通知
        /// </summary>
        private readonly AutoResetEvent newCalcEventNotify = new AutoResetEvent(false);

        /// <summary>
        ///     新报价事件缓存队列
        /// </summary>
        private readonly BlockingCollection<CalculateEvent> newPriceEventQueue =
            new BlockingCollection<CalculateEvent>();

        /// <summary>
        ///     实时计算事件缓存队列，用于报价之外的业务变更事件
        /// </summary>
        private readonly BlockingCollection<CalculateEvent> normalEventQueue = new BlockingCollection<CalculateEvent>();

        /// <summary>
        ///     在线用户列表
        /// </summary>
        private readonly HashSet<string> onlineAccts = new HashSet<string>();

        /// <summary>
        ///     打开的账户窗口的账户列表
        /// </summary>
        private readonly HashSet<string> openWindowCustomers = new HashSet<string>();

        /// <summary>
        ///     挂单仓储
        /// </summary>
        private readonly IOrderCacheRepository orderRepository;

        /// <summary>
        ///     按Margin Ratio 减 Force Sell Level的绝对值从小到大的顺序进行排序的MarginCall列表
        /// </summary>
        private readonly ObservableCollection<BaseMarginCallVm> orderedMarginCallList =
            new ObservableCollection<BaseMarginCallVm>();

        /// <summary>
        ///     授信仓储
        /// </summary>
        private readonly IPledgeCacheRepository pledgeRepository;

        /// <summary>
        ///     报价结构缓存
        ///     当前未使用
        /// </summary>
        private readonly Dictionary<string, PriceStructure> priceDic = new Dictionary<string, PriceStructure>();

        /// <summary>
        ///     报价组合配置
        /// </summary>
        private readonly IQuoteGroupCacheRepository quoteGroupRepository;

        /// <summary>
        ///     商品仓储
        /// </summary>
        private readonly ISymbolCacheRepository symbolRepository;

        /// <summary>
        ///     账户组仓储
        /// </summary>
        private ICustomerGroupCacheRepository acctGroupRepository;

        /// <summary>
        ///     主界面的Deal/Order列表是否开启,用于判断是否订阅报价
        /// </summary>
        private volatile bool isDealOrderListOpen;

        /// <summary>
        ///     是否订阅报价
        /// </summary>
        private volatile bool isSubcribePrice;

        /// <summary>
        ///     在Requesthandling中展示的Customer，如果变更为空则表示无显示Cust
        /// </summary>
        private string showCustInRequestHandling;

        /// <summary>
        ///     待激活订单仓储
        /// </summary>
        private ITobeActiveOrderCacheRepository tobeActiveRepository;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     获取按Margin Ratio 减 Force Sell Level的绝对值从小到大的顺序进行排序的MarginCall绑定列表
        /// </summary>
        /// <returns>绑定列表</returns>
        public ObservableCollection<BaseMarginCallVm> GetMarginCallBindList()
        {
            return this.orderedMarginCallList;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 根据账户id获取账户结构
        /// </summary>
        /// <param name="acctId">
        /// 账户号
        /// </param>
        /// <returns>
        /// 返回账户结构
        /// </returns>
        private CustomerStructure GetAcctStructureById(string acctId)
        {
            CustomerStructure acctStru;
            if (this.activeCustomerStructureDic.TryGetValue(acctId, out acctStru))
            {
                return acctStru;
            }

            return null;
        }

        /// <summary>
        /// 根据账户id获取账户结构
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <returns>
        /// 返回账户结构
        /// </returns>
        private PriceStructure GetPriceStructureBySymbol(string symbol)
        {
            var priceStru = new PriceStructure();
            if (this.priceDic.TryGetValue(symbol, out priceStru))
            {
                return priceStru;
            }

            return null;
        }

        #endregion

        /// <summary>
        ///     实时计算异常
        /// </summary>
        public class RealtimeCalculateException : Exception
        {
            #region Public Properties

            /// <summary>
            ///     重写消息
            /// </summary>
            public override string Message
            {
                get
                {
                    return "实时计算触发严重错误";
                }
            }

            #endregion
        }

        /// <summary>
        ///     账户结构
        ///     当前账户结构中的暂时未使用
        /// </summary>
        private class CustomerStructure
        {
            #region Fields

            /// <summary>
            ///     所属计算模型
            /// </summary>
            private readonly CaculateModel belowCalculateModel;

            /// <summary>
            ///     BU
            /// </summary>
            private readonly BaseBusinessUnitVM businessUnit;

            /// <summary>
            ///     授信信息字典
            /// </summary>
            private readonly Dictionary<string, BaseCreditVM> creditDictionary = new Dictionary<string, BaseCreditVM>();

            /// <summary>
            ///     客户账户
            /// </summary>
            private readonly BaseCustomerViewModel customer;

            /// <summary>
            ///     计算AccountPosition的订单汇总字典
            /// </summary>
            private readonly Dictionary<string, decimal> dealAccountPositionDictionary =
                new Dictionary<string, decimal>();

            /// <summary>
            ///     相关订单列表
            /// </summary>
            private readonly Dictionary<string, BaseDealVM> dealDictionary = new Dictionary<string, BaseDealVM>();

            /// <summary>
            ///     相关挂单列表
            /// </summary>
            private readonly Dictionary<string, BaseOrderVM> orderDictionary = new Dictionary<string, BaseOrderVM>();

            /// <summary>
            ///     抵押金信息字典
            /// </summary>
            private readonly Dictionary<string, BasePledgeVM> pledgeDictionary = new Dictionary<string, BasePledgeVM>();

            /// <summary>
            ///     挂单汇总得到的AccountPosition
            /// </summary>
            private decimal pendingOrderAccountPosition;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomerStructure"/> class.
            /// </summary>
            /// <param name="deals">
            /// The deals.
            /// </param>
            /// <param name="orders">
            /// The orders.
            /// </param>
            /// <param name="pledges">
            /// 抵押金列表
            /// </param>
            /// <param name="credits">
            /// 授信信息列表
            /// </param>
            /// <param name="varbusinessUnit">
            /// The varbusiness Unit.
            /// </param>
            /// <param name="varCustomer">
            /// The customer.
            /// </param>
            /// <param name="calcModel">
            /// 所属计算模型
            /// </param>
            public CustomerStructure(
                IEnumerable<BaseDealVM> deals, 
                IEnumerable<BaseOrderVM> orders, 
                IEnumerable<BasePledgeVM> pledges, 
                IEnumerable<BaseCreditVM> credits, 
                BaseBusinessUnitVM varbusinessUnit, 
                BaseCustomerViewModel varCustomer, 
                CaculateModel calcModel)
            {
                this.customer = varCustomer;
                this.belowCalculateModel = calcModel;
                this.businessUnit = varbusinessUnit;
                foreach (BaseDealVM baseDealVm in deals)
                {
                    this.dealDictionary.Add(baseDealVm.GetID(), baseDealVm);
                }

                this.ReinitialDealAccountPositionDic();

                foreach (BaseOrderVM baseOrderVm in orders)
                {
                    this.orderDictionary.Add(baseOrderVm.GetID(), baseOrderVm);
                }

                this.ReCalculatePendingAccountPosition();

                foreach (BasePledgeVM pledgeVm in pledges)
                {
                    this.pledgeDictionary.Add(pledgeVm.GetID(), pledgeVm);
                }

                this.customer.Account.CustomerCapital.Credit = 0M;

                foreach (BaseCreditVM creditVm in credits)
                {
                    this.AddCredit(creditVm);
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// 添加授信信息
            /// </summary>
            /// <param name="creditInfo">
            /// 授信信息
            /// </param>
            /// <returns>
            /// true表示正常添加，false表示已经存在
            /// </returns>
            public bool AddCredit(BaseCreditVM creditInfo)
            {
                if (this.creditDictionary.ContainsKey(creditInfo.GetID()))
                {
                    return false;
                }

                this.creditDictionary.Add(creditInfo.GetID(), creditInfo);
                this.customer.CustomerCapital.Credit += creditInfo.Amount;
                return true;
            }

            /// <summary>
            /// 向客户结构中添加Deal
            /// </summary>
            /// <param name="dealInfo">
            /// 订单信息
            /// </param>
            /// <returns>
            /// false表示已经在账户的订单列表信息中，true表示正常添加 <see cref="bool"/>.
            /// </returns>
            public bool AddDeal(BaseDealVM dealInfo)
            {
                if (this.dealDictionary.ContainsKey(dealInfo.GetID()))
                {
                    return false;
                }

                this.dealDictionary.Add(dealInfo.GetID(), dealInfo);
                this.AddToDealAccountPositionDic(dealInfo);
                return true;
            }

            /// <summary>
            /// 向客户结构中添加Order
            /// </summary>
            /// <param name="orderInfo">
            /// 订单信息
            /// </param>
            /// <returns>
            /// false表示已经在账户的订单列表信息中，true表示正常添加 <see cref="bool"/>.
            /// </returns>
            public bool AddOrder(BaseOrderVM orderInfo)
            {
                if (this.orderDictionary.ContainsKey(orderInfo.GetID()))
                {
                    return false;
                }

                this.orderDictionary.Add(orderInfo.GetID(), orderInfo);
                this.ReCalculatePendingAccountPosition();
                return true;
            }

            /// <summary>
            /// 添加新抵押金
            /// </summary>
            /// <param name="newPledge">
            /// 新抵押金
            /// </param>
            /// <returns>
            /// 如果为true则表示添加成功，false表示已经存在 <see cref="bool"/>.
            /// </returns>
            public bool AddPledge(BasePledgeVM newPledge)
            {
                if (this.pledgeDictionary.ContainsKey(newPledge.GetID()))
                {
                    return false;
                }

                this.pledgeDictionary.Add(newPledge.GetID(), newPledge);
                return true;
            }

            /// <summary>
            /// 是否率属于Bu下
            /// </summary>
            /// <param name="newBu">
            /// Bu
            /// </param>
            /// <returns>
            /// 是否率属
            /// </returns>
            public bool BelongBu(BaseBusinessUnitVM newBu)
            {
                return this.businessUnit.GetID() == newBu.GetID();
            }

            /// <summary>
            ///     在只是新报价时计算Customer
            /// </summary>
            public void CalculateCustomerWhenNewPrice()
            {
                // 计算客户AccountPosition
                BaseCurrencyVM ccyVM = this.belowCalculateModel.CurrencyRepository.FindByID(this.customer.LocalCCYID);
                var roundingMethod = RoundingEmun.Rounding;
                int amountDecimal = 2;

                if (ccyVM != null)
                {
                    roundingMethod = ccyVM.RoundingMethod;
                    amountDecimal = ccyVM.AmountDecimals;
                }

                decimal dealAccountPosition = this.GetDealsAccountPosition();
                if (this.businessUnit.EnablePendingCalcMargin)
                {
                    this.customer.AccountPosition =
                        (dealAccountPosition + this.pendingOrderAccountPosition).FormatAmountByCCYConfig(
                            roundingMethod, 
                            amountDecimal);
                }
                else
                {
                    this.customer.AccountPosition = dealAccountPosition.FormatAmountByCCYConfig(
                        roundingMethod, 
                        amountDecimal);
                }

                this.CalculateCustomeFloatingPl();
                this.customer.CustomerCapital.FloatingPL =
                    this.customer.CustomerCapital.FloatingPL.FormatAmountByCCYConfig(roundingMethod, amountDecimal);

                this.CalculateAccountCollateralBalance();
                this.customer.CustomerCapital.CollateralBalance =
                    this.customer.CustomerCapital.CollateralBalance.FormatAmountByCCYConfig(
                        roundingMethod, 
                        amountDecimal);

                this.ReCalculateAccountPledge();
                this.customer.CustomerCapital.PledgeAmount =
                    this.customer.CustomerCapital.PledgeAmount.FormatAmountByCCYConfig(roundingMethod, amountDecimal);

                this.customer.CustomerCapital.Equity =
                    (this.customer.CustomerCapital.CollateralBalance + this.customer.CustomerCapital.Credit
                     + this.customer.CustomerCapital.PledgeAmount).FormatAmountByCCYConfig(
                         roundingMethod, 
                         amountDecimal);
                if (this.businessUnit.IncludingPLType != IncludingPLTypeEnum.CalculateIfNegative
                    || this.customer.CustomerCapital.FloatingPL < 0)
                {
                    this.customer.CustomerCapital.Equity += this.customer.CustomerCapital.FloatingPL;
                }

                this.customer.CustomerCapital.MarginRequired =
                    (this.customer.AccountPosition / (int)this.customer.Leverage).FormatAmountByCCYConfig(
                        roundingMethod, 
                        amountDecimal);

                this.customer.CustomerCapital.FreeMargin =
                    (this.customer.CustomerCapital.Equity - this.customer.CustomerCapital.MarginRequired)
                        .FormatAmountByCCYConfig(roundingMethod, amountDecimal);

                this.customer.CustomerCapital.FreePosition =
                    (this.customer.CustomerCapital.FreeMargin * this.customer.Leverage).FormatAmountByCCYConfig(
                        roundingMethod, 
                        amountDecimal);

                if (this.customer.AccountPosition == 0)
                {
                    this.customer.MarginRatio = decimal.MaxValue;
                }
                else
                {
                    this.customer.MarginRatio = this.customer.CustomerCapital.Equity / this.customer.AccountPosition
                                                * 100; // ToFixed(2);
                }

                decimal callAmount =
                    ((this.customer.AccountPosition * this.customer.MarginCallLevel / 100)
                     - this.customer.CustomerCapital.Equity).FormatAmountByCCYConfig(roundingMethod, amountDecimal);
                if (callAmount < 0)
                {
                    this.customer.Margincallamount = 0M.FormatAmountByCCYConfig(roundingMethod, amountDecimal);
                }
                else
                {
                    this.customer.Margincallamount = callAmount;
                }

                this.customer.FloatingPL = this.customer.CustomerCapital.FloatingPL;
                this.customer.Equity = this.customer.CustomerCapital.Equity;
                this.customer.MarginRequired = this.customer.CustomerCapital.MarginRequired;
                this.customer.FreeMargin = this.customer.CustomerCapital.FreeMargin;
                this.customer.FreePosition = this.customer.CustomerCapital.FreePosition;
                this.customer.CollateralBalance = this.customer.CustomerCapital.CollateralBalance;
                this.customer.PledgeAmount = this.customer.CustomerCapital.PledgeAmount;
                this.customer.Credit = this.customer.CustomerCapital.Credit;
            }

            /// <summary>
            ///     获取Deal统计得到的AccountPosition
            /// </summary>
            /// <returns>
            ///     deal统计的得到的AccountPosition <see cref="decimal" />.
            /// </returns>
            public decimal GetDealsAccountPosition()
            {
                decimal targetAccountPosition = 0M;
                foreach (var kvalue in this.dealAccountPositionDictionary)
                {
                    if (kvalue.Value > 0)
                    {
                        continue;
                    }

                    string currencyId = kvalue.Key.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).First();

                    if (this.customer.LocalCCYID == currencyId)
                    {
                        targetAccountPosition -= kvalue.Value;
                        continue;
                    }

                    TransPrice transferQuote = PriceCore.Instance.GetTransitionPrice(
                        currencyId, 
                        this.customer.LocalCCYID);
                    switch (transferQuote.PriceDirection)
                    {
                        case EnumDirection.NotExisting:
                            TraceManager.Warn.Write(
                                "实时计算", 
                                "计算账户:{0}的AccountPosition时找不到,源货币：{1},目标货币：{2}的转换报价，货币量{3},不予计算包含。", 
                                this.customer.CustmerNo, 
                                currencyId, 
                                this.customer.LocalCCYID, 
                                kvalue.Value);
                            continue;
                        case EnumDirection.Equals:
                            targetAccountPosition -= kvalue.Value;
                            continue;
                        case EnumDirection.Before:
                            targetAccountPosition -= kvalue.Value * transferQuote.QuotePrice.Mid;
                            continue;
                        case EnumDirection.After:
                            targetAccountPosition -= kvalue.Value / transferQuote.QuotePrice.Mid;
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return targetAccountPosition;
            }

            /// <summary>
            ///     获取Customer相关的所有订单
            /// </summary>
            /// <returns>
            ///     Customer相关的订单列表
            /// </returns>
            public List<BaseDealVM> GetRelatedDeals()
            {
                return this.dealDictionary.Values.ToList();
            }

            /// <summary>
            ///     获取Customer相关的所有挂单
            /// </summary>
            /// <returns>
            ///     Customer相关的挂单列表
            /// </returns>
            public List<BaseOrderVM> GetRelatedOrders()
            {
                return this.orderDictionary.Values.ToList();
            }

            /// <summary>
            ///     账户信息修改的界面更新
            /// </summary>
            public void NotifyCustomer()
            {
                // 变更账户
                this.customer.MarginRatio = this.customer.MarginRatio;
                this.customer.FreeMargin = this.customer.FreeMargin;
                this.customer.FreePosition = this.customer.FreePosition;
                this.customer.Equity = this.customer.Equity;
                this.customer.FloatingPL = this.customer.FloatingPL;
                this.customer.Credit = this.customer.Credit;
                this.customer.PledgeAmount = this.customer.PledgeAmount;
                this.customer.AccountPosition = this.customer.AccountPosition;
                this.customer.MarginRequired = this.customer.MarginRequired;
                this.customer.CollateralBalance = this.customer.CollateralBalance;
            }

            /// <summary>
            ///     重新计算账户的AccountPosition
            /// </summary>
            public void ReCalculatePendingAccountPosition()
            {
                this.pendingOrderAccountPosition = 0M;
                if (this.businessUnit.EnablePendingCalcMargin)
                {
                    if (this.businessUnit.PendingMarginAlgo == PendingMarginAlgoEnum.BOP)
                    {
                        this.CalculateCustomerPendingPositionByBop();
                    }
                    else if (this.businessUnit.PendingMarginAlgo == PendingMarginAlgoEnum.NOP)
                    {
                        this.CalculateCustomerPendingPositionByNop();
                    }
                    else if (this.businessUnit.PendingMarginAlgo == PendingMarginAlgoEnum.Total)
                    {
                        this.CalculateCustomerPendingPositionByTotal();
                    }
                }
            }

            /// <summary>
            ///     在账户信息变动时重新计算客户
            /// </summary>
            public void ReInitialCalculateCustomer()
            {
                this.ReinitialDealAccountPositionDic();

                this.ReCalculatePendingAccountPosition();

                this.ReInitialCredit();

                this.CalculateCustomerWhenNewPrice();
            }

            /// <summary>
            ///     重新计算订单的AccountPosition统计字典
            /// </summary>
            public void ReinitialDealAccountPositionDic()
            {
                this.dealAccountPositionDictionary.Clear();
                foreach (BaseDealVM deal in this.dealDictionary.Values)
                {
                    this.AddToDealAccountPositionDic(deal);
                }
            }

            /// <summary>
            /// 移除授信信息
            /// </summary>
            /// <param name="creditInfo">
            /// 授信信息
            /// </param>
            /// <returns>
            /// true表示正常移除，false表示并不存在
            /// </returns>
            public bool RemoveCredit(BaseCreditVM creditInfo)
            {
                if (!this.creditDictionary.ContainsKey(creditInfo.GetID()))
                {
                    return false;
                }

                this.creditDictionary.Remove(creditInfo.GetID());
                this.customer.CustomerCapital.Credit -= creditInfo.Amount;
                return true;
            }

            /// <summary>
            /// 从客户结构中删除Deal
            /// </summary>
            /// <param name="dealInfo">
            /// 订单信息
            /// </param>
            /// <returns>
            /// false账户的订单列表信息中不存在，true表示正常删除 <see cref="bool"/>.
            /// </returns>
            public bool RemoveDeal(BaseDealVM dealInfo)
            {
                if (!this.dealDictionary.ContainsKey(dealInfo.GetID()))
                {
                    return false;
                }

                this.dealDictionary.Remove(dealInfo.GetID());
                this.RemoveFromeDealAccountPositionDic(dealInfo);
                return true;
            }

            /// <summary>
            /// 向客户结构中移除Order
            /// </summary>
            /// <param name="removeOrder">
            /// 订单信息
            /// </param>
            /// <returns>
            /// false表示并没有在账户的订单列表信息中存在，true表示正常移除 <see cref="bool"/>.
            /// </returns>
            public bool RemoveOrder(BaseOrderVM removeOrder)
            {
                if (!this.orderDictionary.ContainsKey(removeOrder.GetID()))
                {
                    return false;
                }

                this.orderDictionary.Remove(removeOrder.GetID());
                this.ReCalculatePendingAccountPosition();
                return true;
            }

            /// <summary>
            /// 移除抵押金
            /// </summary>
            /// <param name="removePledge">
            /// 抵押金
            /// </param>
            /// <returns>
            /// 如果为true则表示移除成功，false表示不存在 <see cref="bool"/>.
            /// </returns>
            public bool RemovePledge(BasePledgeVM removePledge)
            {
                if (!this.pledgeDictionary.ContainsKey(removePledge.GetID()))
                {
                    return false;
                }

                this.pledgeDictionary.Remove(removePledge.GetID());
                return true;
            }

            #endregion

            #region Methods

            /// <summary>
            /// 向账户的订单仓位字典中添加订单
            /// </summary>
            /// <param name="dealInfo">
            /// 订单信息
            /// </param>
            private void AddToDealAccountPositionDic(BaseDealVM dealInfo)
            {
                if (this.businessUnit.MarginCalcType == MarginCalculationTypeEnum.ByCCY)
                {
                    if (!this.dealAccountPositionDictionary.ContainsKey(dealInfo.CCY1))
                    {
                        this.dealAccountPositionDictionary.Add(dealInfo.CCY1, 0.00M);
                    }

                    if (!this.dealAccountPositionDictionary.ContainsKey(dealInfo.CCY2))
                    {
                        this.dealAccountPositionDictionary.Add(dealInfo.CCY2, 0.00M);
                    }

                    if (dealInfo.TransactionType == TransactionTypeEnum.Buy)
                    {
                        this.dealAccountPositionDictionary[dealInfo.CCY1] += dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[dealInfo.CCY2] -= dealInfo.CCY2Amount;
                    }
                    else
                    {
                        this.dealAccountPositionDictionary[dealInfo.CCY1] -= dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[dealInfo.CCY2] += dealInfo.CCY2Amount;
                    }
                }
                else
                {
                    string keyCcy1 = dealInfo.CCY1 + "_" + dealInfo.ValueDate.ToString("yyyy-MM-dd");
                    string keyCcy2 = dealInfo.CCY2 + "_" + dealInfo.ValueDate.ToString("yyyy-MM-dd");

                    if (!this.dealAccountPositionDictionary.ContainsKey(keyCcy1))
                    {
                        this.dealAccountPositionDictionary.Add(keyCcy1, 0.00M);
                    }

                    if (!this.dealAccountPositionDictionary.ContainsKey(keyCcy2))
                    {
                        this.dealAccountPositionDictionary.Add(keyCcy2, 0.00M);
                    }

                    if (dealInfo.TransactionType == TransactionTypeEnum.Buy)
                    {
                        this.dealAccountPositionDictionary[keyCcy1] += dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[keyCcy2] -= dealInfo.CCY2Amount;
                    }
                    else
                    {
                        this.dealAccountPositionDictionary[keyCcy1] -= dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[keyCcy2] += dealInfo.CCY2Amount;
                    }
                }
            }

            /// <summary>
            ///     重算帐户的CollateralBalance
            /// </summary>
            private void CalculateAccountCollateralBalance()
            {
                decimal targetCollateralBalance = 0M;
                foreach (CustInternalAcctModel account in this.customer.Account.CollateralAccounts)
                {
                    if (account.AvailableBalance == 0)
                    {
                        continue;
                    }

                    TransPrice transferQuote = PriceCore.Instance.GetTransitionPrice(
                        this.customer.LocalCCYID, 
                        account.CurrencyID);
                    switch (transferQuote.PriceDirection)
                    {
                        case EnumDirection.NotExisting:
                            TraceManager.Warn.Write(
                                "实时计算", 
                                "When calculate banlance,cant find transfer quote ,Local ccy:{0}, transferccy:{1},amount:{2}", 
                                this.customer.LocalCCYID, 
                                account.CurrencyID, 
                                account.AvailableBalance);
                            continue;
                        case EnumDirection.Equals:
                            targetCollateralBalance += account.AvailableBalance;
                            continue;
                        case EnumDirection.Before:
                            if (transferQuote.QuotePrice.Mid == 0)
                            {
                                TraceManager.Warn.WriteAdditional("实时计算", transferQuote.QuotePrice, "计算时，Mid加为0，除零崩溃。");
                                continue;
                            }

                            targetCollateralBalance += account.AvailableBalance / transferQuote.QuotePrice.Mid;
                            continue;
                        case EnumDirection.After:
                            targetCollateralBalance += account.AvailableBalance * transferQuote.QuotePrice.Mid;
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // var ccyVM = this.belowCalculateModel.CurrencyRepository.FindByID(this.customer.LocalCCYID);
                // RoundingEmun roundingMethod = RoundingEmun.Rounding;
                // int amountDecimal = 2;

                // if (ccyVM != null)
                // {
                // roundingMethod = ccyVM.RoundingMethod;
                // amountDecimal = ccyVM.AmountDecimals;
                // }
                this.customer.CustomerCapital.CollateralBalance = targetCollateralBalance;

                // .FormatAmountByCCYConfig(roundingMethod, amountDecimal);
            }

            /// <summary>
            ///     计算账户的浮动盈亏
            /// </summary>
            private void CalculateCustomeFloatingPl()
            {
                this.customer.Account.CustomerCapital.FloatingPL =
                    this.dealDictionary.Values.Sum(deal => deal.FloatingPL);
            }

            /// <summary>
            ///     计算帐户的PendingOrder在Bop模式下的仓位
            /// </summary>
            private void CalculateCustomerPendingPositionByBop()
            {
                var pendingSymbolBuyPositionDic = new Dictionary<string, decimal>();
                var pendingSymbolSellPositionDic = new Dictionary<string, decimal>();
                foreach (BaseOrderVM order in this.orderDictionary.Values)
                {
                    if (order.Status != PendingStatusEnum.Pending)
                    {
                        continue;
                    }

                    if (!pendingSymbolBuyPositionDic.ContainsKey(order.Symbol))
                    {
                        pendingSymbolBuyPositionDic.Add(order.Symbol, 0.00M);
                        pendingSymbolSellPositionDic.Add(order.Symbol, 0.00M);
                    }

                    if (order.TransactionType == TransactionTypeEnum.Buy)
                    {
                        pendingSymbolBuyPositionDic[order.Symbol] += order.PendingOrderModel.PerOrderPosition;
                    }
                    else
                    {
                        pendingSymbolSellPositionDic[order.Symbol] += order.PendingOrderModel.PerOrderPosition;
                    }
                }

                foreach (var kvalue in pendingSymbolBuyPositionDic)
                {
                    this.pendingOrderAccountPosition += Math.Max(kvalue.Value, pendingSymbolSellPositionDic[kvalue.Key]);
                }
            }

            /// <summary>
            ///     计算帐户的PendingOrder在Nop模式下的仓位
            /// </summary>
            private void CalculateCustomerPendingPositionByNop()
            {
                var pendingSymbolBuyPositionDic = new Dictionary<string, decimal>();
                var pendingSymbolSellPositionDic = new Dictionary<string, decimal>();
                foreach (BaseOrderVM order in this.orderDictionary.Values)
                {
                    if (order.Status != PendingStatusEnum.Pending)
                    {
                        continue;
                    }

                    if (!pendingSymbolBuyPositionDic.ContainsKey(order.Symbol))
                    {
                        pendingSymbolBuyPositionDic.Add(order.Symbol, 0.00M);
                        pendingSymbolSellPositionDic.Add(order.Symbol, 0.00M);
                    }

                    if (order.TransactionType == TransactionTypeEnum.Buy)
                    {
                        pendingSymbolBuyPositionDic[order.Symbol] += order.PendingOrderModel.PerOrderPosition;
                    }
                    else
                    {
                        pendingSymbolSellPositionDic[order.Symbol] += order.PendingOrderModel.PerOrderPosition;
                    }
                }

                foreach (var kvalue in pendingSymbolBuyPositionDic)
                {
                    this.pendingOrderAccountPosition += Math.Abs(
                        kvalue.Value - pendingSymbolSellPositionDic[kvalue.Key]);
                }
            }

            /// <summary>
            ///     计算帐户的PendingOrder在Total模式下的仓位
            /// </summary>
            private void CalculateCustomerPendingPositionByTotal()
            {
                foreach (BaseOrderVM order in this.orderDictionary.Values)
                {
                    if (order.Status != PendingStatusEnum.Pending)
                    {
                        continue;
                    }

                    this.pendingOrderAccountPosition += order.PendingOrderModel.PerOrderPosition;
                }
            }

            /// <summary>
            ///     重算帐户的Pledge
            /// </summary>
            private void ReCalculateAccountPledge()
            {
                BaseCurrencyVM ccyVM = this.belowCalculateModel.CurrencyRepository.FindByID(this.customer.LocalCCYID);
                var roundingMethod = RoundingEmun.Rounding;
                int amountDecimal = 2;

                if (ccyVM != null)
                {
                    roundingMethod = ccyVM.RoundingMethod;
                    amountDecimal = ccyVM.AmountDecimals;
                }

                decimal pledgeAmount = 0M;
                foreach (BasePledgeVM pledge in this.pledgeDictionary.Values)
                {
                    TransPrice transferQuote = PriceCore.Instance.GetTransitionPrice(
                        this.customer.LocalCCYID, 
                        pledge.CCY);
                    switch (transferQuote.PriceDirection)
                    {
                        case EnumDirection.NotExisting:
                            TraceManager.Warn.Write(
                                "实时计算", 
                                "计算抵押金时找不到转换报价,Local ccy:{0}, transferccy:{1},PledgeCustomer:{2},Pledge:{3}", 
                                this.customer.LocalCCYID, 
                                pledge.CCY, 
                                this.customer.CustmerNo, 
                                pledge.GetID());
                            continue;
                        case EnumDirection.Equals:
                            pledgeAmount += (pledge.Amount * pledge.Percentage).FormatAmountByCCYConfig(
                                roundingMethod, 
                                amountDecimal);
                            continue;
                        case EnumDirection.Before:
                            if (transferQuote.QuotePrice.Mid == 0)
                            {
                                TraceManager.Warn.WriteAdditional(
                                    "实时计算", 
                                    transferQuote.QuotePrice, 
                                    "计算抵押金时转换报价为0，出现除零,Local ccy:{0}, transferccy:{1},PledgeCustomer:{2},Pledge:{3}", 
                                    this.customer.LocalCCYID, 
                                    pledge.CCY, 
                                    this.customer.CustmerNo, 
                                    pledge.GetID());
                                continue;
                            }

                            pledgeAmount +=
                                (pledge.Amount * pledge.Percentage / transferQuote.QuotePrice.Mid)
                                    .FormatAmountByCCYConfig(roundingMethod, amountDecimal);
                            continue;
                        case EnumDirection.After:
                            pledgeAmount +=
                                (pledge.Amount * pledge.Percentage * transferQuote.QuotePrice.Mid)
                                    .FormatAmountByCCYConfig(roundingMethod, amountDecimal);
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                this.customer.CustomerCapital.PledgeAmount = pledgeAmount;
            }

            /// <summary>
            ///     重新计算Credit
            /// </summary>
            private void ReInitialCredit()
            {
                decimal creditAmount = decimal.Zero;
                foreach (BaseCreditVM baseCreditVm in this.creditDictionary.Values)
                {
                    creditAmount += baseCreditVm.Amount;
                }

                this.customer.CustomerCapital.Credit = creditAmount;
            }

            /// <summary>
            /// 从账户的订单仓位字典中删除订单
            /// </summary>
            /// <param name="dealInfo">
            /// 订单信息
            /// </param>
            private void RemoveFromeDealAccountPositionDic(BaseDealVM dealInfo)
            {
                if (this.businessUnit.MarginCalcType == MarginCalculationTypeEnum.ByCCY)
                {
                    if (!this.dealAccountPositionDictionary.ContainsKey(dealInfo.CCY1))
                    {
                        return;
                    }

                    if (!this.dealAccountPositionDictionary.ContainsKey(dealInfo.CCY2))
                    {
                        return;
                    }

                    if (dealInfo.TransactionType == TransactionTypeEnum.Buy)
                    {
                        this.dealAccountPositionDictionary[dealInfo.CCY1] -= dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[dealInfo.CCY2] += dealInfo.CCY2Amount;
                    }
                    else
                    {
                        this.dealAccountPositionDictionary[dealInfo.CCY1] += dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[dealInfo.CCY2] -= dealInfo.CCY2Amount;
                    }
                }
                else
                {
                    string keyCcy1 = dealInfo.CCY1 + "_" + dealInfo.ValueDate.ToString("yyyy-MM-dd");
                    string keyCcy2 = dealInfo.CCY2 + "_" + dealInfo.ValueDate.ToString("yyyy-MM-dd");

                    if (!this.dealAccountPositionDictionary.ContainsKey(keyCcy1))
                    {
                        return;
                    }

                    if (!this.dealAccountPositionDictionary.ContainsKey(keyCcy2))
                    {
                        return;
                    }

                    if (dealInfo.TransactionType == TransactionTypeEnum.Buy)
                    {
                        this.dealAccountPositionDictionary[keyCcy1] -= dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[keyCcy2] += dealInfo.CCY2Amount;
                    }
                    else
                    {
                        this.dealAccountPositionDictionary[keyCcy1] += dealInfo.CCY1Amount;
                        this.dealAccountPositionDictionary[keyCcy2] -= dealInfo.CCY2Amount;
                    }
                }
            }

            /// <summary>
            ///     根据当前客户刷新MarginCall列表
            /// </summary>
            private void ReorderMarginCallListForCurrentCustomer()
            {
                // 当前客户Margin Ratio 减 Force Sell Level的绝对值
                decimal currentCustomerValue = Math.Abs(this.customer.MarginRatio - this.customer.ForceSellLevel);
                BaseMarginCallVm itemToBeInsert =
                    this.belowCalculateModel.orderedMarginCallList.FirstOrDefault(
                        item => item.BelongCustomer.CustomerNo == this.customer.CustomerNo);

                if (itemToBeInsert != null)
                {
                    // 当前客户MarginCall在列表中原来的位置
                    int originalIndex = this.belowCalculateModel.orderedMarginCallList.IndexOf(itemToBeInsert);

                    // 应该插入的Index
                    int toBeInsertedIndex = originalIndex;
                    for (int i = 0; i < this.belowCalculateModel.orderedMarginCallList.Count; i++)
                    {
                        // 获取各个项的Margin Ratio 减 Force Sell Level的绝对值
                        decimal tempValue = this.belowCalculateModel.orderedMarginCallList[i].BelongCustomer.MarginRatio
                                            - this.belowCalculateModel.orderedMarginCallList[i].BelongCustomer
                                                  .ForceSellLevel;

                        // 找到第一个大于的项
                        if (tempValue > currentCustomerValue)
                        {
                            toBeInsertedIndex = i;
                            break;
                        }
                    }

                    if (toBeInsertedIndex > originalIndex)
                    {
                        Application.Current.Dispatcher.Invoke(
                            () =>
                                {
                                    this.belowCalculateModel.orderedMarginCallList.Remove(itemToBeInsert);
                                    this.belowCalculateModel.orderedMarginCallList.Insert(
                                        toBeInsertedIndex - 1, 
                                        itemToBeInsert);
                                });
                    }
                    else if (toBeInsertedIndex < originalIndex)
                    {
                        Application.Current.Dispatcher.Invoke(
                            () =>
                                {
                                    this.belowCalculateModel.orderedMarginCallList.Remove(itemToBeInsert);
                                    this.belowCalculateModel.orderedMarginCallList.Insert(
                                        toBeInsertedIndex, 
                                        itemToBeInsert);
                                });
                    }
                }
            }

            #endregion
        }

        /// <summary>
        ///     报价结构体，结构体中记录所有相关订单
        /// </summary>
        private class PriceStructure
        {
            #region Fields

            /// <summary>
            ///     相关订单列表
            /// </summary>
            private readonly Dictionary<string, BaseDealVM> dealList = new Dictionary<string, BaseDealVM>();

            /// <summary>
            ///     相关挂单列表
            /// </summary>
            private readonly Dictionary<string, BaseOrderVM> pendingDealList = new Dictionary<string, BaseOrderVM>();

            #endregion

            #region Public Properties

            /// <summary>
            ///     订单列表
            /// </summary>
            public Dictionary<string, BaseDealVM> DealList
            {
                get
                {
                    return this.dealList;
                }
            }

            /// <summary>
            ///     挂单列表
            /// </summary>
            public Dictionary<string, BaseOrderVM> PendingDealList
            {
                get
                {
                    return this.pendingDealList;
                }
            }

            /// <summary>
            ///     报价
            /// </summary>
            public TickQuoteModel Price { get; set; }

            #endregion
        }
    }
}