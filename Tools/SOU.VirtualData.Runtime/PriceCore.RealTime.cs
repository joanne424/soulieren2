// <copyright file="PriceCore.RealTime.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/05/09 04:59:06 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/05/09 04:59:06
//      修改描述：新建 PriceCore.RealTime.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BaseViewModel;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Log;
    using Infrastructure.Models;

    /// <summary>
    ///     The price core.
    /// </summary>
    public partial class PriceCore
    {
        #region Public Methods and Operators

        /// <summary>
        /// 根据价格结构计算CustomerRate
        /// </summary>
        /// <param name="priceCalcInfo">
        /// 价格结构
        /// </param>
        /// <returns>
        /// CustomerRate结果
        /// </returns>
        public decimal CalculateCustomerRate(PriceCalcInfo priceCalcInfo)
        {
            if (priceCalcInfo == null || priceCalcInfo.OriginalPriceCalcInfo == null)
            {
                TraceManager.Warn.Write("PriceCore.CalculateCustomerRate", "PriceCalcInfo is null");
                return decimal.Zero;
            }

            BaseSymbolVM symbol =
                this.GetRepository<ISymbolCacheRepository>().FindByID(priceCalcInfo.OriginalPriceCalcInfo.SymbolId);
            if (symbol == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.CalculateCustomerRate", 
                    "Cant find symbol {0} in local storage.", 
                    priceCalcInfo.OriginalPriceCalcInfo.SymbolId);
                return decimal.Zero;
            }

            int forwardDecimalPlace = priceCalcInfo.OriginalPriceCalcInfo.Tenor == TenorEnum.SP
                                          ? symbol.DecimalPlace
                                          : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            if (priceCalcInfo.OriginalPriceCalcInfo.IsBid)
            {
                // CustomerRate = DealerRate - CustomerSpread
                return
                    (priceCalcInfo.OriginalPriceCalcInfo.DealerRate
                     - priceCalcInfo.CustomerSpread.SpreadToDecimal(symbol.BasisPoint)).FormatPriceBySymbolPoint(
                         forwardDecimalPlace);
            }

            // CustomerRate = DealerRate + CustomerSpread
            return
                (priceCalcInfo.OriginalPriceCalcInfo.DealerRate
                 + priceCalcInfo.CustomerSpread.SpreadToDecimal(symbol.BasisPoint)).FormatPriceBySymbolPoint(
                     forwardDecimalPlace);
        }

        /// <summary>
        /// 计算订单实时计算使用的价格结构
        /// </summary>
        /// <param name="deal">
        /// 订单实体
        /// </param>
        /// <returns>
        /// 报价结构
        /// </returns>
        public bool InitialPriceCalcInfoForDeal(BaseDealVM deal)
        {
            if (deal == null)
            {
                TraceManager.Warn.Write("PriceCore.InitialPriceCalcInfoForDeal", "Deal is null");
                return false;
            }

            // 获取USD
            BaseCurrencyVM usdCcy =
                this.GetRepository<ICurrencyCacheRepository>().Filter(o => o.CurrencyName.Equals(USD)).FirstOrDefault();
            if (usdCcy == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.InitialPriceCalcInfoForDeal", 
                    "Can't find usd in currency local cache repository.");
                return false;
            }

            // 获取订单对应的Customer信息
            BaseCustomerViewModel customer = this.GetRepository<ICustomerCacheRepository>().FindByID(deal.CustomerNo);
            if (customer == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.InitialPriceCalcInfoForDeal", 
                    "Can't find customer {0} in customer repository", 
                    deal.CustomerNo);
                return false;
            }

            // 获取订单的货币对
            var symbolRep = this.GetRepository<ISymbolCacheRepository>();
            BaseSymbolVM dealSymbol = symbolRep.FindByID(deal.Symbol);
            if (dealSymbol == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.InitialPriceCalcInfoForDeal", 
                    "Can't find {0} in symbol local cache repository.", 
                    deal.Symbol);
                return false;
            }

            List<TickQuoteModel> tickQuoteList =
                this.GetRepository<IQuoteCacheRepository>().Filter(o => true).Select(p => p.PropSet).ToList();

            // 获取订单货币对的报价
            TickQuoteModel tickQuote = tickQuoteList.FirstOrDefault(o => o.SymbolID == dealSymbol.SymbolID);
            if (tickQuote == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.InitialPriceCalcInfoForDeal", 
                    "Can't find tick quote for symbol {0}", 
                    deal.Symbol);
                return false;
            }

            bool isBid = deal.TransactionType == TransactionTypeEnum.Buy;
            var priceCalcInfo = new PriceCalcInfo();

            // 初始化PlccyToLocalCcy结构
            if (dealSymbol.PLCCY == customer.LocalCCYID)
            {
                priceCalcInfo.PlccyToLocalCcyDirectType = PlccyToLocalCcyDirectEnum.Equal;
            }
            else
            {
                BaseSymbolVM plccyToLocalccySymbol = symbolRep.GetSymbol(dealSymbol.PLCCY, customer.LocalCCYID);
                if (plccyToLocalccySymbol == null)
                {
                    TraceManager.Warn.Write(
                        "PriceCore.InitialPriceCalcInfoForDeal", 
                        "Can't find pl to local symbol for ccy {0} to ccy {1}", 
                        dealSymbol.PLCCY, 
                        customer.LocalCCYID);
                    return false;
                }

                priceCalcInfo.PlccyToLocalCcySymbol = plccyToLocalccySymbol.SymbolID;
                priceCalcInfo.PlccyToLocalCcyDirectType = customer.LocalCCYID == dealSymbol.CCY1
                                                              ? PlccyToLocalCcyDirectEnum.LocalCcyBefore
                                                              : PlccyToLocalCcyDirectEnum.LocalCcyAfter;

                // 转换货币添加到报价相关货币列表
                priceCalcInfo.PriceRelatedSymbolIdList.Add(plccyToLocalccySymbol.SymbolID);
            }

            // 原始货币对的Tenor
            TenorEnum originalTenor;
            DateTime expireTime;
            bool result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                customer.BusinessUnitID, 
                deal.ValueDate, 
                dealSymbol.SymbolID, 
                out originalTenor, 
                out expireTime);
            if (!result)
            {
                TraceManager.Warn.Write(
                    "PriceCore.InitialPriceCalcInfoForDeal", 
                    "Can't find tenor for symbol {0} value date {1} business unit {2}", 
                    deal.Symbol, 
                    deal.ValueDate, 
                    customer.BusinessUnitID);
                return false;
            }

            int originalDecimalPlace = originalTenor == TenorEnum.SP
                                           ? dealSymbol.DecimalPlace
                                           : dealSymbol.BasisPoint + dealSymbol.ForwardPointDecimalPlace;

            // Tenor过期时间
            priceCalcInfo.TenorExpireTime = expireTime;

            // 直盘
            if (dealSymbol.CCY1 == usdCcy.CurrencyID || dealSymbol.CCY2 == usdCcy.CurrencyID)
            {
                // 直盘
                priceCalcInfo.IsDirect = true;

                // 获取直盘的非USD的CCY Id
                string nonUsdCcyId = dealSymbol.CCY1 == usdCcy.CurrencyID ? dealSymbol.CCY2 : dealSymbol.CCY1;
                priceCalcInfo.OriginalPriceCalcInfo = this.GetDirectInnerPriceCalcInfo(
                    tickQuote, 
                    dealSymbol, 
                    deal.ValueDate, 
                    originalTenor, 
                    usdCcy, 
                    customer.BusinessUnitID, 
                    nonUsdCcyId, 
                    isBid);

                if (priceCalcInfo.OriginalPriceCalcInfo != null)
                {
                    // 添加关注报价变化的SymbolId
                    priceCalcInfo.PriceRelatedSymbolIdList.Add(priceCalcInfo.OriginalPriceCalcInfo.SymbolId);
                }
            }
            else
            {
                // 交叉盘
                priceCalcInfo.IsDirect = false;

                // 交叉盘SP
                if (originalTenor == TenorEnum.SP)
                {
                    priceCalcInfo.OriginalPriceCalcInfo = new PriceCalcInfo.InnerPriceCalcInfo
                                                              {
                                                                  SymbolId =
                                                                      dealSymbol
                                                                      .SymbolID, 
                                                                  Tenor = originalTenor, 
                                                                  DealerSpotRate =
                                                                      isBid
                                                                          ? tickQuote
                                                                                .TraderBid
                                                                          : tickQuote
                                                                                .TraderAsk, 
                                                                  ForwardPoint =
                                                                      decimal.Zero
                                                                      .FormatPriceBySymbolPoint
                                                                      (
                                                                          dealSymbol
                                                                      .ForwardPointDecimalPlace), 
                                                                  IsBid = isBid, 
                                                              };
                    priceCalcInfo.OriginalPriceCalcInfo.DealerRate =
                        priceCalcInfo.OriginalPriceCalcInfo.DealerSpotRate.FormatPriceBySymbolPoint(
                            originalDecimalPlace);
                    priceCalcInfo.PriceRelatedSymbolIdList.Add(priceCalcInfo.OriginalPriceCalcInfo.SymbolId);
                }
                else
                {
                    // 取交叉盘对应的两个直盘的货币对
                    BaseSymbolVM ccy1UsdSymbol = symbolRep.GetSymbol(dealSymbol.CCY1, usdCcy.CurrencyID);
                    BaseSymbolVM ccy2UsdSymbol = symbolRep.GetSymbol(dealSymbol.CCY2, usdCcy.CurrencyID);
                    if (ccy1UsdSymbol == null || ccy2UsdSymbol == null)
                    {
                        TraceManager.Warn.Write(
                            "PriceCore.InitialPriceCalcInfoForDeal", 
                            "Can't find direct symbol for ccy {0} or {1} in symbol local cache repository.", 
                            dealSymbol.CCY1, 
                            dealSymbol.CCY2);
                        return false;
                    }

                    TickQuoteModel ccy1TickQuote =
                        tickQuoteList.FirstOrDefault(o => o.SymbolID == ccy1UsdSymbol.SymbolID);
                    TickQuoteModel ccy2TickQuote =
                        tickQuoteList.FirstOrDefault(o => o.SymbolID == ccy2UsdSymbol.SymbolID);
                    if (ccy1TickQuote == null || ccy2TickQuote == null)
                    {
                        TraceManager.Warn.Write(
                            "PriceCore.InitialPriceCalcInfoForDeal", 
                            "Can't find tick quote for symbol {0} or {1}", 
                            ccy1UsdSymbol.SymbolID, 
                            ccy2UsdSymbol.SymbolID);
                        return false;
                    }

                    TenorEnum ccy1Tenor;
                    result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                        customer.BusinessUnitID, 
                        deal.ValueDate, 
                        ccy1UsdSymbol.SymbolID, 
                        out ccy1Tenor, 
                        out expireTime);
                    if (!result)
                    {
                        TraceManager.Warn.Write(
                            "PriceCore.InitialPriceCalcInfoForDeal", 
                            "Can't find tenor for symbol {0} value date {1} business unit {2}", 
                            ccy1UsdSymbol.SymbolID, 
                            deal.ValueDate, 
                            customer.BusinessUnitID);
                        return false;
                    }

                    TenorEnum ccy2Tenor;
                    result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                        customer.BusinessUnitID, 
                        deal.ValueDate, 
                        ccy2UsdSymbol.SymbolID, 
                        out ccy2Tenor, 
                        out expireTime);

                    if (!result)
                    {
                        TraceManager.Warn.Write(
                            "PriceCore.InitialPriceCalcInfoForDeal", 
                            "Can't find tenor for symbol {0} value date {1} business unit {2}", 
                            ccy2UsdSymbol.SymbolID, 
                            deal.ValueDate, 
                            customer.BusinessUnitID);
                        return false;
                    }

                    int indirect = 0;
                    bool isCcy1Bid = false;
                    bool isCcy2Bid = false;
                    if (ccy1UsdSymbol.CCY2 == usdCcy.CurrencyID)
                    {
                        indirect++;
                    }

                    if (ccy2UsdSymbol.CCY2 == usdCcy.CurrencyID)
                    {
                        indirect++;
                    }

                    switch (indirect)
                    {
                        case 0:
                            priceCalcInfo.IsCrossDealerRateMultiply = false;
                            priceCalcInfo.IsDirect1First = false;
                            isCcy1Bid = !isBid;
                            isCcy2Bid = isBid;
                            break;
                        case 1:
                            priceCalcInfo.IsCrossDealerRateMultiply = true;
                            priceCalcInfo.IsDirect1First = true;
                            isCcy1Bid = isBid;
                            isCcy2Bid = isBid;
                            break;
                        case 2:
                            priceCalcInfo.IsCrossDealerRateMultiply = false;
                            priceCalcInfo.IsDirect1First = true;
                            isCcy1Bid = isBid;
                            isCcy2Bid = !isBid;
                            break;
                    }

                    // 计算CCY1对应直盘的价格信息
                    priceCalcInfo.Direct1PriceCalcInfo = this.GetDirectInnerPriceCalcInfo(
                        ccy1TickQuote, 
                        ccy1UsdSymbol, 
                        deal.ValueDate, 
                        ccy1Tenor, 
                        usdCcy, 
                        customer.BusinessUnitID, 
                        dealSymbol.CCY1, 
                        isCcy1Bid);

                    // 计算CCY2对应直盘的价格信息
                    priceCalcInfo.Direct2PriceCalcInfo = this.GetDirectInnerPriceCalcInfo(
                        ccy2TickQuote, 
                        ccy2UsdSymbol, 
                        deal.ValueDate, 
                        ccy2Tenor, 
                        usdCcy, 
                        customer.BusinessUnitID, 
                        dealSymbol.CCY2, 
                        isCcy2Bid);

                    // 计算原始交叉盘的价格信息
                    priceCalcInfo.OriginalPriceCalcInfo = this.GetCrossInnerPriceCalcInfo(
                        tickQuote, 
                        dealSymbol, 
                        originalTenor, 
                        priceCalcInfo.Direct1PriceCalcInfo, 
                        priceCalcInfo.Direct2PriceCalcInfo, 
                        priceCalcInfo.IsCrossDealerRateMultiply, 
                        priceCalcInfo.IsDirect1First, 
                        isBid);

                    if (priceCalcInfo.Direct1PriceCalcInfo != null && priceCalcInfo.Direct2PriceCalcInfo != null
                        && priceCalcInfo.OriginalPriceCalcInfo != null)
                    {
                        priceCalcInfo.PriceRelatedSymbolIdList.Add(priceCalcInfo.Direct1PriceCalcInfo.SymbolId);
                        priceCalcInfo.PriceRelatedSymbolIdList.Add(priceCalcInfo.Direct2PriceCalcInfo.SymbolId);

                        // TODO: 计算交叉盘的ForwardPoint需要交叉盘的报价，此处需确认是否需要
                        priceCalcInfo.PriceRelatedSymbolIdList.Add(priceCalcInfo.OriginalPriceCalcInfo.SymbolId);
                    }
                }
            }

            // 客户点差
            priceCalcInfo.CustomerSpread = this.CalcCustomerSpread(deal, customer, isBid);
            deal.PriceCalcInfo = priceCalcInfo;
            return true;
        }

        /// <summary>
        /// 报价变化时更新PriceCalcInfo的价格结构
        /// </summary>
        /// <param name="deal">
        /// 订单实体
        /// </param>
        /// <returns>
        /// 更新是否成功
        /// </returns>
        public bool UpdatePriceCalcInfoByNewPrice(BaseDealVM deal)
        {
            if (deal == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.UpdatePriceCalcInfoByCurrentMarketPrice", 
                    "Deal is null when UpdatePriceCalcInfoByCurrentMarketPrice");
                return false;
            }

            // 获取订单对应的Customer信息
            BaseCustomerViewModel customer = this.GetRepository<ICustomerCacheRepository>().FindByID(deal.CustomerNo);
            if (customer == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.UpdatePriceCalcInfoByNewPrice", 
                    "Can't find customer {0} in customer repository", 
                    deal.CustomerNo);
                return false;
            }

            // 当原结构不存在或已经过了Tenor的过期时间, 则重新计算PriceCalcInfo结构
            if (deal.PriceCalcInfo == null
                || deal.PriceCalcInfo.TenorExpireTime < RunTime.GetCurrentRunTime(this.OwnerId).GetSystemGmtTime())
            {
                return this.InitialPriceCalcInfoForDeal(deal);
            }

            // PlToLocal信息不存在则重新计算
            if (string.IsNullOrEmpty(deal.PriceCalcInfo.PlccyToLocalCcySymbol)
                && deal.PriceCalcInfo.PlccyToLocalCcyDirectType != PlccyToLocalCcyDirectEnum.Equal)
            {
                return this.InitialPriceCalcInfoForDeal(deal);
            }

            if (deal.PriceCalcInfo.IsDirect)
            {
                // 直盘：原价格结构中不存在直盘信息则重新计算，否则直接更新DealerRate及DealerSpotRate
                if (deal.PriceCalcInfo.OriginalPriceCalcInfo == null)
                {
                    return this.InitialPriceCalcInfoForDeal(deal);
                }

                // 获取直盘报价
                BaseQuoteVM newTickQuote =
                    this.GetRepository<IQuoteCacheRepository>()
                        .FindByID(deal.PriceCalcInfo.OriginalPriceCalcInfo.SymbolId);
                if (newTickQuote != null)
                {
                    deal.PriceCalcInfo.OriginalPriceCalcInfo =
                        this.UpdateDirectInnerPriceCalcInfo(
                            deal.PriceCalcInfo.OriginalPriceCalcInfo, 
                            newTickQuote.PropSet);
                }

                return true;
            }

            // 交叉盘：原价格结构中不存在交叉盘及直盘的信息则重新计算，否则更新受影响货币对的
            if (deal.PriceCalcInfo.OriginalPriceCalcInfo == null || deal.PriceCalcInfo.Direct1PriceCalcInfo == null
                || deal.PriceCalcInfo.Direct2PriceCalcInfo == null)
            {
                return this.InitialPriceCalcInfoForDeal(deal);
            }

            // 获取受影响的报价
            BaseQuoteVM direct1TickQuote =
                this.GetRepository<IQuoteCacheRepository>().FindByID(deal.PriceCalcInfo.Direct1PriceCalcInfo.SymbolId);
            BaseQuoteVM direct2TickQuote =
                this.GetRepository<IQuoteCacheRepository>().FindByID(deal.PriceCalcInfo.Direct2PriceCalcInfo.SymbolId);
            BaseQuoteVM originalTickQuote =
                this.GetRepository<IQuoteCacheRepository>().FindByID(deal.PriceCalcInfo.OriginalPriceCalcInfo.SymbolId);

            // 直盘1报价变化
            if (direct1TickQuote != null)
            {
                deal.PriceCalcInfo.Direct1PriceCalcInfo =
                    this.UpdateDirectInnerPriceCalcInfo(
                        deal.PriceCalcInfo.Direct1PriceCalcInfo, 
                        direct1TickQuote.PropSet);
            }

            // 直盘2报价变化
            if (direct2TickQuote != null)
            {
                deal.PriceCalcInfo.Direct2PriceCalcInfo =
                    this.UpdateDirectInnerPriceCalcInfo(
                        deal.PriceCalcInfo.Direct2PriceCalcInfo, 
                        direct2TickQuote.PropSet);
            }

            // TODO: 交叉盘报价变化(此处影响交叉盘的DealerSpotRate以及ForwardPoint，确认是否需要)
            if (originalTickQuote != null)
            {
                BaseSymbolVM symbol =
                    this.GetRepository<ISymbolCacheRepository>()
                        .FindByID(deal.PriceCalcInfo.OriginalPriceCalcInfo.SymbolId);
                if (symbol != null)
                {
                    deal.PriceCalcInfo.OriginalPriceCalcInfo = this.GetCrossInnerPriceCalcInfo(
                        originalTickQuote.PropSet, 
                        symbol, 
                        deal.PriceCalcInfo.OriginalPriceCalcInfo.Tenor, 
                        deal.PriceCalcInfo.Direct1PriceCalcInfo, 
                        deal.PriceCalcInfo.Direct2PriceCalcInfo, 
                        deal.PriceCalcInfo.IsCrossDealerRateMultiply, 
                        deal.PriceCalcInfo.IsDirect1First, 
                        deal.PriceCalcInfo.OriginalPriceCalcInfo.IsBid);
                }
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 计算客户点差
        /// </summary>
        /// <param name="deal">
        /// 订单实体
        /// </param>
        /// <param name="customer">
        /// 客户实体
        /// </param>
        /// <param name="isBid">
        /// 是否Bid
        /// </param>
        /// <param name="rateType">
        /// 类型
        /// </param>
        /// <returns>
        /// 客户点差
        /// </returns>
        private decimal CalcCustomerSpread(
            BaseDealVM deal, 
            BaseCustomerViewModel customer, 
            bool isBid, 
            TradeRateTypeEnum rateType = TradeRateTypeEnum.TTRate)
        {
            BaseQuoteGroupConfigVM quoteGroup =
                this.GetRepository<IQuoteGroupCacheRepository>().FindByID(customer.QuoteGroupID);
            if (quoteGroup == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.CalcCustomerSpread", 
                    "Can't find quote group {0} in quote group repository", 
                    customer.QuoteGroupID);
                return decimal.Zero;
            }

            QuoteGroupModel.CustomerSpreadConfigModel customerSpread = this.GetSpreadConfig(deal, quoteGroup);
            if (customerSpread == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcCustomerSpread", "Can't find customerSpread");
                return decimal.Zero;
            }

            if (rateType == TradeRateTypeEnum.TTRate)
            {
                return isBid ? customerSpread.TTBid : customerSpread.TTAsk;
            }

            return isBid ? customerSpread.CashBid : customerSpread.CashAsk;
        }

        /// <summary>
        /// 根据两直盘价格内容获取(计算)交叉盘价格内容
        /// </summary>
        /// <param name="tickQuote">
        /// 交叉盘报价
        /// </param>
        /// <param name="symbol">
        /// 交叉盘货币对
        /// </param>
        /// <param name="crossTenor">
        /// The cross Tenor.
        /// </param>
        /// <param name="direct1PriceCalcInfo">
        /// 直盘1的价格结构
        /// </param>
        /// <param name="direct2PriceCalcInfo">
        /// 直盘2的价格结构
        /// </param>
        /// <param name="isCrossDealerRateMultiply">
        /// 计算交叉盘的DealerRate使用的运算符
        /// </param>
        /// <param name="isDirect1First">
        /// 计算交叉盘的DealerRate时, 两直盘的DealerRate先后顺序
        /// </param>
        /// <param name="isBid">
        /// 是否使用Bid
        /// </param>
        /// <returns>
        /// 交叉盘的价格结构
        /// </returns>
        private PriceCalcInfo.InnerPriceCalcInfo GetCrossInnerPriceCalcInfo(
            TickQuoteModel tickQuote, 
            BaseSymbolVM symbol, 
            TenorEnum crossTenor, 
            PriceCalcInfo.InnerPriceCalcInfo direct1PriceCalcInfo, 
            PriceCalcInfo.InnerPriceCalcInfo direct2PriceCalcInfo, 
            bool isCrossDealerRateMultiply, 
            bool isDirect1First, 
            bool isBid)
        {
            int decimalPlace = crossTenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;
            if (direct1PriceCalcInfo == null || direct2PriceCalcInfo == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.GetCrossInnerPriceCalcInfo", 
                    "Can't find two direct info for symbol {0}", 
                    symbol.SymbolID);
                return null;
            }

            var innerPriceCalcInfo = new PriceCalcInfo.InnerPriceCalcInfo
                                         {
                                             SymbolId = symbol.SymbolID, 
                                             DealerSpotRate =
                                                 isBid
                                                     ? tickQuote.TraderBid
                                                     : tickQuote.TraderAsk, 
                                             Tenor = crossTenor, 
                                             IsBid = isBid, 
                                         };

            if (isCrossDealerRateMultiply)
            {
                innerPriceCalcInfo.DealerRate = direct1PriceCalcInfo.DealerRate * direct2PriceCalcInfo.DealerRate;
            }
            else
            {
                if (isDirect1First)
                {
                    innerPriceCalcInfo.DealerRate = direct1PriceCalcInfo.DealerRate / direct2PriceCalcInfo.DealerRate;
                }
                else
                {
                    innerPriceCalcInfo.DealerRate = direct2PriceCalcInfo.DealerRate / direct1PriceCalcInfo.DealerRate;
                }
            }

            if (crossTenor == TenorEnum.ON || crossTenor == TenorEnum.TN)
            {
                innerPriceCalcInfo.ForwardPoint = innerPriceCalcInfo.DealerSpotRate - innerPriceCalcInfo.DealerRate;
            }
            else
            {
                innerPriceCalcInfo.ForwardPoint = innerPriceCalcInfo.DealerRate - innerPriceCalcInfo.DealerSpotRate;
            }

            // 格式化DealerRate以及ForwardPoint
            innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerRate.FormatPriceBySymbolPoint(decimalPlace);
            innerPriceCalcInfo.ForwardPoint =
                innerPriceCalcInfo.ForwardPoint.ToSpread(symbol.BasisPoint)
                    .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

            return innerPriceCalcInfo;
        }

        /// <summary>
        /// 获取(计算)直盘Tenor为BD的内部计算信息
        /// </summary>
        /// <param name="tickQuote">
        /// 直盘货币对报价
        /// </param>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="directTenor">
        /// 直盘的Tenor
        /// </param>
        /// <param name="usdCcy">
        /// Usd货币信息
        /// </param>
        /// <param name="valueDate">
        /// The value Date.
        /// </param>
        /// <param name="businessUnitId">
        /// 业务区Id
        /// </param>
        /// <param name="isBid">
        /// The is Bid.
        /// </param>
        /// <returns>
        /// InnerPriceCalcInfo结构
        /// </returns>
        private PriceCalcInfo.InnerPriceCalcInfo GetDirectBrokenDateInnerPriceCalcInfo(
            TickQuoteModel tickQuote, 
            BaseSymbolVM symbol, 
            TenorEnum directTenor, 
            BaseCurrencyVM usdCcy, 
            DateTime valueDate, 
            string businessUnitId, 
            bool isBid)
        {
            int decimalPlace = directTenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            var innerPriceCalcInfo = new PriceCalcInfo.InnerPriceCalcInfo
                                         {
                                             Tenor = directTenor, 
                                             DealerSpotRate =
                                                 isBid
                                                     ? tickQuote.TraderBid
                                                     : tickQuote.TraderAsk, 
                                             IsBid = isBid, 
                                         };

            // 计算BD的ForwardPoint
            ForwardPointTenorModel brokenDateForwardPoint = this.TempCalcDirectBDForwardPoint(
                valueDate, 
                usdCcy.PropSet, 
                symbol.SymbolModel, 
                businessUnitId);

            innerPriceCalcInfo.ForwardPoint = isBid ? brokenDateForwardPoint.Bid : brokenDateForwardPoint.Ask;
            innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerSpotRate
                                            + innerPriceCalcInfo.ForwardPoint.SpreadToDecimal(symbol.BasisPoint);
            innerPriceCalcInfo.SymbolId = symbol.SymbolID;

            // 格式化DealerRate和ForwardPoint
            innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerRate.FormatPriceBySymbolPoint(decimalPlace);
            innerPriceCalcInfo.ForwardPoint =
                innerPriceCalcInfo.ForwardPoint.FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

            return innerPriceCalcInfo;
        }

        /// <summary>
        /// 计算(获取)直盘的内部价格信息
        /// </summary>
        /// <param name="tickQuote">
        /// 直盘货币对报价信息
        /// </param>
        /// <param name="symbol">
        /// 货币对
        /// </param>
        /// <param name="valueDate">
        /// value date
        /// </param>
        /// <param name="directTenor">
        /// 直盘的Tenor
        /// </param>
        /// <param name="usdCcy">
        /// Usd货币对信息
        /// </param>
        /// <param name="businessUnitId">
        /// 业务区Id
        /// </param>
        /// <param name="nonUsdCcyId">
        /// 直盘货币对中非Usd货币对应的Id
        /// </param>
        /// <param name="isBid">
        /// 是否为Bid
        /// </param>
        /// <returns>
        /// InnerPriceCalcInfo结构
        /// </returns>
        private PriceCalcInfo.InnerPriceCalcInfo GetDirectInnerPriceCalcInfo(
            TickQuoteModel tickQuote, 
            BaseSymbolVM symbol, 
            DateTime valueDate, 
            TenorEnum directTenor, 
            BaseCurrencyVM usdCcy, 
            string businessUnitId, 
            string nonUsdCcyId, 
            bool isBid)
        {
            int decimalPlace = directTenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            // 直接返回直盘SP的报价信息
            if (directTenor == TenorEnum.SP)
            {
                return new PriceCalcInfo.InnerPriceCalcInfo
                           {
                               DealerSpotRate =
                                   isBid ? tickQuote.TraderBid : tickQuote.TraderAsk, 
                               DealerRate =
                                   (isBid ? tickQuote.TraderBid : tickQuote.TraderAsk)
                                   .FormatPriceBySymbolPoint(decimalPlace), 
                               ForwardPoint =
                                   decimal.Zero.FormatPriceBySymbolPoint(
                                       symbol.ForwardPointDecimalPlace), 
                               Tenor = directTenor, 
                               SymbolId = symbol.SymbolID, 
                               IsBid = isBid, 
                           };
            }

            // 获取直盘的ForwardPoint
            ForwardPointModel forwardPointForNonUsdCcy =
                this.GetForwardPoints().FirstOrDefault(o => o.CurrencyID == nonUsdCcyId);
            if (forwardPointForNonUsdCcy == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.GetDirectInnerPriceCalcInfo", 
                    "Can't find forward point for currency {0}", 
                    nonUsdCcyId);
                return null;
            }

            // 计算并返回BD的报价信息
            if (directTenor == TenorEnum.BD)
            {
                return this.GetDirectBrokenDateInnerPriceCalcInfo(
                    tickQuote, 
                    symbol, 
                    directTenor, 
                    usdCcy, 
                    valueDate, 
                    businessUnitId, 
                    isBid);
            }

            // 计算并返回非SP/BD报价信息
            return this.GetDirectTenorInnerPriceCalcInfo(
                tickQuote, 
                symbol, 
                directTenor, 
                forwardPointForNonUsdCcy, 
                isBid);
        }

        /// <summary>
        /// 计算直盘对应Tenor的报价结构
        /// </summary>
        /// <param name="tickQuote">
        /// 当前货币对报价
        /// </param>
        /// <param name="symbol">
        /// 货币对信息
        /// </param>
        /// <param name="directTenor">
        /// 直盘的Tenor
        /// </param>
        /// <param name="forwardPointForNonUsdCcy">
        /// 直盘的非USD对应货币的ForwardPoint结构
        /// </param>
        /// <param name="isBid">
        /// 是否为Bid
        /// </param>
        /// <returns>
        /// InnerPriceCalcInfo结构
        /// </returns>
        private PriceCalcInfo.InnerPriceCalcInfo GetDirectTenorInnerPriceCalcInfo(
            TickQuoteModel tickQuote, 
            BaseSymbolVM symbol, 
            TenorEnum directTenor, 
            ForwardPointModel forwardPointForNonUsdCcy, 
            bool isBid)
        {
            int decimalPlace = directTenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            ForwardPointTenorModel forwardPointTenor =
                forwardPointForNonUsdCcy.Tenors.FirstOrDefault(o => o.Tenor == directTenor);
            if (forwardPointTenor == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.GetDirectTenorInnerPriceCalcInfo", 
                    "Can't find forward point for tenor {0}", 
                    directTenor);
                return null;
            }

            var innerPriceCalcInfo = new PriceCalcInfo.InnerPriceCalcInfo
                                         {
                                             DealerSpotRate =
                                                 isBid
                                                     ? tickQuote.TraderBid
                                                     : tickQuote.TraderAsk, 
                                             Tenor = directTenor, 
                                             SymbolId = symbol.SymbolID, 
                                             IsBid = isBid, 
                                         };

            // 计算Tenor对应的DealerRate及ForwardPoint
            if (directTenor == TenorEnum.ON)
            {
                if (symbol.ValueDate == ValueDateEnum.T1)
                {
                    // DealerRateAsk(Bid) = DealerSpotRateAsk(Bid) - ForwardPointBid(Ask)
                    innerPriceCalcInfo.ForwardPoint = isBid ? forwardPointTenor.Ask : forwardPointTenor.Bid;
                    innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerSpotRate
                                                    - innerPriceCalcInfo.ForwardPoint.SpreadToDecimal(symbol.BasisPoint);
                }
                else
                {
                    // DealerRateAsk(Bid) = DealerSpotRateAsk(Bid) - ONForwardPointBid(Ask) - TNForwardPointBid(Ask)
                    ForwardPointTenorModel forwardPointTenorForTn =
                        forwardPointForNonUsdCcy.Tenors.FirstOrDefault(o => o.Tenor == TenorEnum.TN);
                    if (forwardPointTenorForTn == null)
                    {
                        TraceManager.Warn.Write(
                            "PriceCore.GetDirectTenorInnerPriceCalcInfo", 
                            "Can't find forward point for tenor tn");
                        return null;
                    }

                    innerPriceCalcInfo.ForwardPoint = isBid
                                                          ? forwardPointTenor.Ask + forwardPointTenorForTn.Ask
                                                          : forwardPointTenor.Bid + forwardPointTenorForTn.Bid;
                    innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerSpotRate
                                                    - innerPriceCalcInfo.ForwardPoint.SpreadToDecimal(symbol.BasisPoint);
                }
            }
            else if (directTenor == TenorEnum.TN)
            {
                // DealerRateAsk(Bid) = DealerSpotRateAsk(Bid) - ForwardPointBid(Ask)
                innerPriceCalcInfo.ForwardPoint = isBid ? forwardPointTenor.Ask : forwardPointTenor.Bid;
                innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerSpotRate
                                                - innerPriceCalcInfo.ForwardPoint.SpreadToDecimal(symbol.BasisPoint);
            }
            else
            {
                // DealerRateAsk(Bid) = DealerSpotRateAsk(Bid) + ForwardPointAsk(Bid)
                innerPriceCalcInfo.ForwardPoint = isBid ? forwardPointTenor.Bid : forwardPointTenor.Ask;
                innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerSpotRate
                                                + innerPriceCalcInfo.ForwardPoint.SpreadToDecimal(symbol.BasisPoint);
            }

            // 格式化DealerRate
            innerPriceCalcInfo.DealerRate = innerPriceCalcInfo.DealerRate.FormatPriceBySymbolPoint(decimalPlace);
            innerPriceCalcInfo.ForwardPoint =
                innerPriceCalcInfo.ForwardPoint.FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

            return innerPriceCalcInfo;
        }

        /// <summary>
        /// 获取点差配置
        /// </summary>
        /// <param name="deal">
        /// 订单实体
        /// </param>
        /// <param name="quoteGroup">
        /// QuoteGroup
        /// </param>
        /// <returns>
        /// 客户点差配置
        /// </returns>
        private QuoteGroupModel.CustomerSpreadConfigModel GetSpreadConfig(
            BaseDealVM deal, 
            BaseQuoteGroupConfigVM quoteGroup)
        {
            QuoteGroupModel.QuoteConfigModel quoteConfig =
                quoteGroup.PropSet.QuoteConfigList.FirstOrDefault(o => o.SymbolConfig.SymbolID == deal.Symbol);
            if (quoteConfig == null)
            {
                TraceManager.Warn.Write(
                    "QuoteDomainService.GetSpreadConfig", 
                    "Can't find quote config for symbol {0} in quote group QuoteConfigList", 
                    deal.Symbol);
                return null;
            }

            // 获取报价配置中，符合交易量的项
            QuoteGroupModel.CustomerSpreadConfigModel custSpreadConfig =
                quoteConfig.CustomerSpreadConfigList.FirstOrDefault(
                    c =>
                    c.TradableInstrument == deal.Instrument.ConvertToTradInstrumentEnum()
                    && c.TradableOrderType == CustSpreadTradeOrderEnum.MarketOrder
                    && (c.AmountUpperLimit.HasValue == false || c.AmountUpperLimit >= deal.PerDealPosition)
                    && c.AmountLowerLimit <= deal.PerDealPosition);

            return custSpreadConfig;
        }

        /// <summary>
        /// 根据新报价更新直盘的InnerPriceCalcInfo结构
        /// </summary>
        /// <param name="originalInnerPriceCalcInfo">
        /// 原InnerPriceCalcInfo结构
        /// </param>
        /// <param name="newTickQuote">
        /// 新报价
        /// </param>
        /// <returns>
        /// 新的InnerPriceCalcInfo结构
        /// </returns>
        private PriceCalcInfo.InnerPriceCalcInfo UpdateDirectInnerPriceCalcInfo(
            PriceCalcInfo.InnerPriceCalcInfo originalInnerPriceCalcInfo, 
            TickQuoteModel newTickQuote)
        {
            BaseSymbolVM symbol =
                this.GetRepository<ISymbolCacheRepository>().FindByID(originalInnerPriceCalcInfo.SymbolId);
            if (symbol == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.UpdateDirectInnerPriceCalcInfo", 
                    "Cant find symbol {0} from local storage", 
                    originalInnerPriceCalcInfo.SymbolId);
                return null;
            }

            int decimalPlace = originalInnerPriceCalcInfo.Tenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            // 新的DealerSpotRate
            decimal newDealerSpotRate = originalInnerPriceCalcInfo.IsBid
                                            ? newTickQuote.TraderBid
                                            : newTickQuote.TraderAsk;

            // 新的DealerSpotRate和旧的DealerSpotRate的差值
            decimal different = newDealerSpotRate - originalInnerPriceCalcInfo.DealerSpotRate;

            // 更新差值到
            originalInnerPriceCalcInfo.DealerRate += different;
            originalInnerPriceCalcInfo.DealerSpotRate += different;

            // 格式化DealerRate
            originalInnerPriceCalcInfo.DealerRate =
                originalInnerPriceCalcInfo.DealerRate.FormatPriceBySymbolPoint(decimalPlace);
            return originalInnerPriceCalcInfo;
        }

        #endregion
    }
}