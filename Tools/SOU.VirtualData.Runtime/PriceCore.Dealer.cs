// <copyright file="PriceCore.Dealer.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/06/02 08:40:26 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/06/02 08:40:26
//      修改描述：新建 PriceCore.Dealer.cs
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
    ///     报价dealer价计算类
    /// </summary>
    public partial class PriceCore
    {
        #region Public Methods and Operators

        /// <summary>
        /// 计算trader rate
        /// </summary>
        /// <param name="symbolId">
        /// 商品Id
        /// </param>
        /// <param name="valueDate">
        /// 交割日
        /// </param>
        /// <param name="transType">
        /// 交易单方向
        /// </param>
        /// <param name="busiUnit">
        /// 所属业务区
        /// </param>
        /// <param name="isCashDeal">
        /// 是否为现金交易
        /// </param>
        /// <returns>
        /// 目标TradeRate
        /// </returns>
        public BaseForwardRate CalcCurrentTraderRate(
            string symbolId, 
            DateTime valueDate, 
            TransactionTypeEnum transType, 
            BaseBusinessUnitVM busiUnit, 
            bool isCashDeal = false)
        {
            var forwardRate = new BaseForwardRate();
            List<ForwardPointModel> forwardPoints = this.GetForwardPoints();
            var symbolRep = this.GetRepository<ISymbolCacheRepository>();
            BaseSymbolVM symbol = symbolRep.FindByID(symbolId);
            if (symbol == null)
            {
                return null;
            }

            BaseCurrencyVM usdCcy =
                this.GetRepository<ICurrencyCacheRepository>().Filter(o => o.CurrencyName.Equals(USD)).FirstOrDefault();
            if (usdCcy == null)
            {
                TraceManager.Warn.Write("ViewModel", "Can't find usd in currency local cache repository.");
                return null;
            }

            bool isKvbSellCcy1 = this.IsKVBSellCCY1(transType);
            BaseQuoteVM tickQuote = this.GetMarketTickQuote(symbol.SymbolID);

            if (!isCashDeal)
            {
                BaseSymbolTenorVDVM tenor = ValueDateCore.Instance.GetTenorByValueDateWeSellCcy(
                    symbol.SymbolID, 
                    valueDate, 
                    busiUnit.BusinessUnitID, 
                    isKvbSellCcy1);
                if (tenor.Tenor == TenorEnum.SP)
                {
                    forwardRate.TraderRateBid = tickQuote.TraderSpotBid;
                    forwardRate.TraderRateAsk = tickQuote.TraderSpotAsk;
                    forwardRate.CustomerRateBid = tickQuote.TraderSpotBid;
                    forwardRate.CustomerRateAsk = tickQuote.TraderSpotAsk;
                    forwardRate.Tenor = TenorEnum.SP;
                    forwardRate.SymbolName = symbol.SymbolName;
                    forwardRate.SymbolId = symbol.SymbolID;
                    forwardRate.VauleDate = valueDate;
                    forwardRate.ForwardPointAsk = decimal.Zero;
                    forwardRate.ForwardPointBid = decimal.Zero;
                    forwardRate.TraderSpotRateBid = tickQuote.TraderSpotBid;
                    forwardRate.TraderSpotRateAsk = tickQuote.TraderSpotAsk;
                    return forwardRate;
                }
            }

            if (symbol.CCY1 == usdCcy.CurrencyID || symbol.CCY2 == usdCcy.CurrencyID)
            {
                if (isCashDeal)
                {
                    forwardRate = this.TempCalcDirectTenorTraderRate(symbol.SymbolModel, TenorEnum.ON);
                }
                else
                {
                    BaseSymbolTenorVDVM directTenor =
                        ValueDateCore.Instance.GetTenorByValueDateWeSellCcy(
                            symbol.SymbolID, 
                            valueDate, 
                            busiUnit.BusinessUnitID, 
                            isKvbSellCcy1);
                    if (directTenor == null)
                    {
                        RunTime.ShowFailInfoDialog("InvalidValueDate", string.Empty, this.OwnerId);
                        return null;
                    }

                    forwardRate = directTenor.Tenor == TenorEnum.BD
                                      ? this.TempGetDirectBDTraderRate(symbol.SymbolModel, valueDate, busiUnit)
                                      : this.TempCalcDirectTenorTraderRate(symbol.SymbolModel, directTenor.Tenor);
                }
            }
            else
            {
                BaseSymbolVM ccy1UsdSymbol = symbolRep.GetSymbol(symbol.CCY1, usdCcy.CurrencyID);
                BaseSymbolVM ccy2UsdSymbol = symbolRep.GetSymbol(symbol.CCY2, usdCcy.CurrencyID);
                if (ccy1UsdSymbol == null || ccy2UsdSymbol == null)
                {
                    return null;
                }

                forwardRate = this.TempCalcCrossTraderRate(
                    symbol.SymbolModel, 
                    ccy1UsdSymbol.SymbolModel, 
                    ccy2UsdSymbol.SymbolModel, 
                    valueDate, 
                    busiUnit, 
                    transType, 
                    isCashDeal);
            }

            if (forwardRate == null)
            {
                return null;
            }

            forwardRate.TraderSpotRateBid = tickQuote.TraderSpotBid;
            forwardRate.TraderSpotRateAsk = tickQuote.TraderSpotAsk;

            return forwardRate;
        }

        /// <summary>
        /// 报价列表获取 单个symbol的 Forward Curve使用，当前是和交易一样的
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="nowTradeDate">
        /// 当前tradeDate
        /// </param>
        /// <param name="bu">
        /// The bu.
        /// </param>
        /// <param name="qgse">
        /// 报价组合组
        /// </param>
        /// <param name="rateType">
        /// 点差方式
        /// </param>
        /// <returns>
        /// 返回 Forward Curve
        /// </returns>
        public List<BaseForwardRate> GetTraderTenorCurve(
            SymbolModel symbol, 
            DateTime nowTradeDate, 
            BaseBusinessUnitVM bu, 
            QuoteGroupModel qgse, 
            TradeRateTypeEnum rateType = TradeRateTypeEnum.TTRate)
        {
            var curves = new List<BaseForwardRate>();
            BaseQuoteVM tickQuote =
                this.GetRepository<IQuoteCacheRepository>().Filter(o => o.SymbolID == symbol.SymbolID).FirstOrDefault();
            if (tickQuote == null)
            {
                return null;
            }

            // 遍历tenor
            foreach (int item in Enum.GetValues(typeof(TenorEnum)))
            {
                var tenorItem = (TenorEnum)item;

                if (tenorItem == TenorEnum.TN && symbol.ValueDate == ValueDateEnum.T1)
                {
                    continue;
                }

                string prompt;
                DateTime valueDate = ValueDateCore.Instance.GetValueDateByTenor(
                    bu.BusinessUnitID, 
                    tenorItem, 
                    symbol.SymbolID, 
                    out prompt);
                if (valueDate == default(DateTime))
                {
                    continue;
                }

                BaseForwardRate forwardRate = this.CalcCurrentTraderRate(
                    symbol.SymbolID, 
                    valueDate, 
                    TransactionTypeEnum.Buy, 
                    bu);
                if (forwardRate == null)
                {
                    continue;
                }

                forwardRate.VauleDate = valueDate;
                forwardRate.Tenor = tenorItem;
                forwardRate.SymbolId = symbol.SymbolID;
                curves.Add(forwardRate);
            }

            return curves;
        }

        /// <summary>
        /// 是否为KVBSellCCY1
        /// </summary>
        /// <param name="transactionTypeEnum">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsKVBSellCCY1(TransactionTypeEnum transactionTypeEnum)
        {
            bool isKVBSellCCY1 = true;
            if (transactionTypeEnum == TransactionTypeEnum.Buy)
            {
                isKVBSellCCY1 = true;
            }
            else
            {
                isKVBSellCCY1 = false;
            }

            return isKVBSellCCY1;
        }

        /// <summary>
        /// 根据CCY对判断是否存在对应的报价,交叉盘货币对会判断两个对应的直盘
        /// </summary>
        /// <param name="oneCurrencyId">
        /// 其中一个货币的Id
        /// </param>
        /// <param name="anotherCurrencyId">
        /// 另外一个货币的Id
        /// </param>
        /// <returns>
        /// true: 存在对应货币组的报价
        /// </returns>
        public bool IsQuoteExistForCurrencyPair(string oneCurrencyId, string anotherCurrencyId)
        {
            var currencyRep = this.GetRepository<ICurrencyCacheRepository>();
            var symbolRep = this.GetRepository<ISymbolCacheRepository>();
            var quoteRep = this.GetRepository<IQuoteCacheRepository>();
            if (currencyRep == null || symbolRep == null || quoteRep == null)
            {
                return false;
            }

            // 判断原货币对及报价是否存在
            BaseSymbolVM symbol = symbolRep.GetSymbol(oneCurrencyId, anotherCurrencyId);
            if (symbol == null)
            {
                return false;
            }

            BaseQuoteVM quote = quoteRep.FindByID(symbol.SymbolID);
            if (quote == null)
            {
                return false;
            }

            string usdCcyId = currencyRep.GetIdByName(USD);
            if (usdCcyId != oneCurrencyId && usdCcyId != anotherCurrencyId)
            {
                // 交叉盘则判断两直盘
                // 判断直盘1
                BaseSymbolVM directSymbol1 = symbolRep.GetSymbol(oneCurrencyId, usdCcyId);
                if (directSymbol1 == null)
                {
                    return false;
                }

                BaseQuoteVM directQuote1 = quoteRep.FindByID(directSymbol1.SymbolID);
                if (directQuote1 == null)
                {
                    return false;
                }

                // 判断直盘2
                BaseSymbolVM directSymbol2 = symbolRep.GetSymbol(anotherCurrencyId, usdCcyId);
                if (directSymbol2 == null)
                {
                    return false;
                }

                BaseQuoteVM directQuote2 = quoteRep.FindByID(directSymbol2.SymbolID);
                if (directQuote2 == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 计算CustomerRate，和TempCalcCurrentCustomerRate获取Tenor方法不同
        /// </summary>
        /// <param name="symbolId">
        /// The symbol id.
        /// </param>
        /// <param name="inputCCYID">
        /// The input ccyid.
        /// </param>
        /// <param name="inputAmount">
        /// The input amount.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        /// <param name="rateType">
        /// The rate type.
        /// </param>
        /// <param name="busiUnit">
        /// The busi unit.
        /// </param>
        /// <param name="quoteGroup">
        /// The quote group.
        /// </param>
        /// <param name="tradaOrder">
        /// The trada order.
        /// </param>
        /// <returns>
        /// The <see cref="BaseForwardRate"/>.
        /// </returns>
        public BaseForwardRate TempCalcCurrentCustomerDsRate(
            string symbolId, 
            string inputCCYID, 
            decimal inputAmount, 
            DateTime valueDate, 
            TradeRateTypeEnum rateType, 
            BaseBusinessUnitVM busiUnit, 
            QuoteGroupModel quoteGroup, 
            CustSpreadTradeOrderEnum tradaOrder = CustSpreadTradeOrderEnum.MarketOrder)
        {
            BaseSymbolVM symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(symbolId);
            int basePoint = symbol.BasisPoint;
            TenorEnum tenor;
            DateTime expiretime;
            bool result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                busiUnit.BusinessUnitID, 
                valueDate, 
                symbolId, 
                out tenor, 
                out expiretime);
            if (!result)
            {
                return null;
            }

            int forwardDecimalPlace = tenor == TenorEnum.SP
                                          ? symbol.DecimalPlace
                                          : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            BaseQuoteVM swapPrice = instance.GetMarketTickQuote(symbol.SymbolID);
            if (swapPrice == null)
            {
                return null;
            }

            BaseForwardRate forwardRate = this.TempCalcCurrentDsRate(symbolId, valueDate, busiUnit.BusinessUnitID);
            if (forwardRate == null)
            {
                return null;
            }

            forwardRate.SymbolId = symbol.SymbolID;
            forwardRate.SymbolName = symbol.SymbolName;
            forwardRate.IsBrokenDate = false;
            forwardRate.Tenor = tenor;
            forwardRate.TraderSpotRateBid = swapPrice.TraderSpotBid;
            forwardRate.TraderSpotRateAsk = swapPrice.TraderSpotAsk;
            forwardRate.VauleDate = valueDate;

            // 计算TT点差的客户价
            this.CalcSpotTTRate(
                quoteGroup, 
                forwardRate, 
                tenor, 
                rateType, 
                basePoint, 
                inputCCYID, 
                inputAmount, 
                busiUnit, 
                symbol.SymbolModel);

            forwardRate.CustomerRateBid = forwardRate.CustomerRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.CustomerRateAsk = forwardRate.CustomerRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);

            return forwardRate;
        }

        /// <summary>
        /// The temp calc current ds rate.
        /// </summary>
        /// <param name="symbolId">
        /// The symbol id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        /// <param name="businessUnitId">
        /// The business unit id.
        /// </param>
        /// <returns>
        /// The <see cref="BaseForwardRate"/>.
        /// </returns>
        public BaseForwardRate TempCalcCurrentDsRate(string symbolId, DateTime valueDate, string businessUnitId)
        {
            var forwardRate = new BaseForwardRate();
            BaseSymbolVM symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(symbolId);
            if (symbol == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.TempCalcCurrentDsRate", 
                    "Can't find {0} in symbol local cache repository.", 
                    symbolId);
                return null;
            }

            BaseBusinessUnitVM businessUnit = this.GetRepository<IBusinessUnitCacheRepository>()
                .FindByID(businessUnitId);
            if (businessUnit == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.TempCalcCurrentDsRate", 
                    "Can't find {0} in business unit local cache repository.", 
                    businessUnitId);
                return null;
            }

            BaseCurrencyVM usdCcy =
                this.GetRepository<ICurrencyCacheRepository>().Filter(o => o.CurrencyName.Equals(USD)).FirstOrDefault();
            if (usdCcy == null)
            {
                TraceManager.Warn.Write(
                    "PriceCore.TempCalcCurrentDsRate", 
                    "Can't find usd in currency local cache repository.");
                return null;
            }

            if (symbol.CCY1 == usdCcy.CurrencyID || symbol.CCY2 == usdCcy.CurrencyID)
            {
                TenorEnum tenor;
                DateTime expireTime;
                bool result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                    businessUnitId, 
                    valueDate, 
                    symbolId, 
                    out tenor, 
                    out expireTime);
                if (!result)
                {
                    return null;
                }

                if (tenor == TenorEnum.BD)
                {
                    forwardRate = this.TempGetDirectBDTraderRate(symbol.SymbolModel, valueDate, businessUnit);
                }
                else
                {
                    forwardRate = this.TempCalcDirectTenorTraderRate(symbol.SymbolModel, tenor);
                }
            }
            else
            {
                BaseSymbolVM ccy1UsdSymbol = this.GetRepository<ISymbolCacheRepository>()
                    .GetSymbol(symbol.CCY1, usdCcy.CurrencyID);
                BaseSymbolVM ccy2UsdSymbol = this.GetRepository<ISymbolCacheRepository>()
                    .GetSymbol(symbol.CCY2, usdCcy.CurrencyID);
                if (ccy1UsdSymbol == null || ccy2UsdSymbol == null)
                {
                    return null;
                }

                forwardRate = this.TempCalcCrossDsRate(
                    symbol.SymbolModel, 
                    ccy1UsdSymbol.SymbolModel, 
                    ccy2UsdSymbol.SymbolModel, 
                    valueDate, 
                    businessUnit);
            }

            return forwardRate;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 计算交叉盘Swap的交易员价格
        /// </summary>
        /// <param name="symbol">货币对</param>
        /// <param name="nearLegValueDate">Near Leg 交割日</param>
        /// <param name="nearLegTenor">Near Leg Tenor</param>
        /// <param name="farLegValueDate">Far Leg 交割日</param>
        /// <param name="farLegTenor">Far Leg Tenor</param>
        /// <param name="businessUnit">业务区</param>
        /// <param name="nearTransType">Near Leg交易方向</param>
        /// <returns>价格信息</returns>
        private BaseSwapQuoteVM CalcCrossSwapDealerRate(
            BaseSymbolVM symbol,
            DateTime nearLegValueDate,
            TenorEnum nearLegTenor,
            DateTime farLegValueDate,
            TenorEnum farLegTenor,
            BaseBusinessUnitVM businessUnit, 
            TransactionTypeEnum nearTransType)
        {
            if (symbol == null || nearLegValueDate == default(DateTime) || farLegValueDate == default(DateTime) || businessUnit == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcDirectSwapDealerRate", "Input parameter error");
                return null;
            }

            var nearLegDealerRate = this.CalcCurrentTraderRate(
                symbol.SymbolID,
                nearLegValueDate,
                nearTransType,
                businessUnit);
            if (nearLegDealerRate == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcDirectSwapDealerRate", "Calculate near leg dealer rate error");
                return null;
            }

            var farLegDealerRate = this.CalcCurrentTraderRate(
                symbol.SymbolID,
                farLegValueDate,
                nearTransType == TransactionTypeEnum.Buy ? TransactionTypeEnum.Sell : TransactionTypeEnum.Buy,
                businessUnit);
            if (farLegDealerRate == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcDirectSwapDealerRate", "Calculate far leg dealer rate error");
                return null;
            }

            // 格式化
            nearLegDealerRate.FormatDecimalPlace();
            farLegDealerRate.FormatDecimalPlace();

            var baseSwapQuoteVm = new BaseSwapQuoteVM
            {
                TraderSpotRateBid = nearLegDealerRate.TraderSpotRateBid,
                TraderSpotRateAsk = nearLegDealerRate.TraderSpotRateAsk
            };

            // 计算NearLeg和FarLeg的Trader Forward Rate
            if (nearLegTenor == TenorEnum.ON && farLegTenor == TenorEnum.TN)
            {
                // 保留的小数位数
                int point = symbol.ForwardPointDecimalPlace + symbol.DecimalPlace;

                // NearLeg forward point
                baseSwapQuoteVm.NearLegFwdPointBid = nearLegDealerRate.ForwardPointBid;
                baseSwapQuoteVm.NearLegFwdPointAsk = nearLegDealerRate.ForwardPointAsk;

                // Dealer Swap Point :  ON Forward Point Bid/Ask
                baseSwapQuoteVm.BuySellTraderSwapPoints =
                    (nearLegDealerRate.ForwardPointAsk - farLegDealerRate.ForwardPointAsk).FormatPriceBySymbolPoint(
                        symbol.ForwardPointDecimalPlace);
                baseSwapQuoteVm.SellBuyTraderSwapPoints =
                    (nearLegDealerRate.ForwardPointBid - farLegDealerRate.ForwardPointBid).FormatPriceBySymbolPoint(
                        symbol.ForwardPointDecimalPlace);

                // Dealer rate
                baseSwapQuoteVm.BuySellTraderNearLegRate =
                    nearLegDealerRate.TraderRateAsk.FormatPriceBySymbolPoint(point);
                baseSwapQuoteVm.SellBuyTraderNearLegRate =
                    nearLegDealerRate.TraderRateBid.FormatPriceBySymbolPoint(point);

                // FL Dealer Rate : NL Dealer Rate + Dealer Swap Point / 10 的 Base Point 次方
                baseSwapQuoteVm.BuySellTraderFarLegRate =
                    (baseSwapQuoteVm.BuySellTraderNearLegRate
                     + baseSwapQuoteVm.BuySellTraderSwapPoints.SpreadToDecimal(symbol.BasisPoint))
                        .FormatPriceBySymbolPoint(point);
                baseSwapQuoteVm.SellBuyTraderFarLegRate =
                    (baseSwapQuoteVm.SellBuyTraderNearLegRate
                     + baseSwapQuoteVm.SellBuyTraderSwapPoints.SpreadToDecimal(symbol.BasisPoint))
                        .FormatPriceBySymbolPoint(point);

                // FL Forward Point :  ( FL Dealer Rate － NL Dealer Spot Rate）
                baseSwapQuoteVm.FarLegFwdPointBid =
                    (baseSwapQuoteVm.BuySellTraderFarLegRate - nearLegDealerRate.TraderSpotRateAsk).ToSpread(
                        symbol.BasisPoint).FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                baseSwapQuoteVm.FarLegFwdPointAsk =
                    (baseSwapQuoteVm.SellBuyTraderFarLegRate - nearLegDealerRate.TraderSpotRateBid).ToSpread(
                        symbol.BasisPoint).FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
            }
            else
            {
                int nearLegPoint = nearLegTenor == TenorEnum.SP
                                              ? symbol.DecimalPlace
                                              : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;
                int farLegPoint = farLegTenor == TenorEnum.SP
                                             ? symbol.DecimalPlace
                                             : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

                // NL Forward Point :  ( NL Dealer Spot Rate - NL Dealer Rate）*10 的 Base Point 次方
                baseSwapQuoteVm.NearLegFwdPointBid =
                    nearLegDealerRate.ForwardPointBid.FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                baseSwapQuoteVm.NearLegFwdPointAsk =
                    nearLegDealerRate.ForwardPointAsk.FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

                // Dealer Swap Point :  ( FL Dealer Rate － NL Dealer Rate）*10 的 Base Point 次方
                baseSwapQuoteVm.BuySellTraderSwapPoints =
                    (farLegDealerRate.TraderRateBid - nearLegDealerRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                        .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                baseSwapQuoteVm.SellBuyTraderSwapPoints =
                    (farLegDealerRate.TraderRateAsk - nearLegDealerRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                        .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

                // DealerRate
                baseSwapQuoteVm.BuySellTraderNearLegRate =
                    nearLegDealerRate.TraderRateAsk.FormatPriceBySymbolPoint(nearLegPoint);
                baseSwapQuoteVm.SellBuyTraderNearLegRate =
                    nearLegDealerRate.TraderRateBid.FormatPriceBySymbolPoint(nearLegPoint);
                baseSwapQuoteVm.BuySellTraderFarLegRate =
                    farLegDealerRate.TraderRateBid.FormatPriceBySymbolPoint(farLegPoint);
                baseSwapQuoteVm.SellBuyTraderFarLegRate =
                    farLegDealerRate.TraderRateAsk.FormatPriceBySymbolPoint(farLegPoint);

                // FL Forward Point :  ( FL Dealer Rate － NL Dealer Spot Rate）*10 的 Base Point 次方
                baseSwapQuoteVm.FarLegFwdPointAsk =
                    (baseSwapQuoteVm.SellBuyTraderFarLegRate - nearLegDealerRate.TraderSpotRateBid).ToSpread(
                        symbol.BasisPoint).FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                baseSwapQuoteVm.FarLegFwdPointBid =
                    (baseSwapQuoteVm.BuySellTraderFarLegRate - nearLegDealerRate.TraderSpotRateAsk).ToSpread(
                        symbol.BasisPoint).FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
            }

            return baseSwapQuoteVm;
        }

        /// <summary>
        /// 计算直盘Swap的交易员价格
        /// </summary>
        /// <param name="symbol">货币对</param>
        /// <param name="nearLegValueDate">Near Leg 交割日</param>
        /// <param name="nearLegTenor">Near Leg Tenor</param>
        /// <param name="farLegValueDate">Far Leg 交割日</param>
        /// <param name="farLegTenor">Far Leg Tenor</param>
        /// <param name="businessUnit">业务区</param>
        /// <param name="nearTransType">Near Leg交易方向</param>
        /// <returns>价格信息</returns>
        private BaseSwapQuoteVM CalcDirectSwapDealerRate(
            BaseSymbolVM symbol, 
            DateTime nearLegValueDate, 
            TenorEnum nearLegTenor,
            DateTime farLegValueDate,
            TenorEnum farLegTenor,
            BaseBusinessUnitVM businessUnit, 
            TransactionTypeEnum nearTransType)
        {
            if (symbol == null || nearLegValueDate == default(DateTime) || farLegValueDate == default(DateTime) || businessUnit == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcDirectSwapDealerRate", "Input parameter error");
                return null;
            }

            // 计算NearLeg和FarLeg的DealerRate相关数据
            BaseForwardRate nearLegDealerRate =
                this.CalcCurrentTraderRate(symbol.SymbolID, nearLegValueDate, nearTransType, businessUnit);
            if (nearLegDealerRate == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcDirectSwapDealerRate", "Calculate near leg dealer rate error");
                return null;
            }

            BaseForwardRate farLegDealerRate = this.CalcCurrentTraderRate(
                symbol.SymbolID,
                farLegValueDate,
                nearTransType == TransactionTypeEnum.Buy ? TransactionTypeEnum.Sell : TransactionTypeEnum.Buy,
                businessUnit);
            if (farLegDealerRate == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcDirectSwapDealerRate", "Calculate far leg dealer rate error");
                return null;
            }

            int farLegDecimalPlace = farLegTenor == TenorEnum.SP
                                        ? symbol.DecimalPlace
                                        : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            // 格式化
            nearLegDealerRate.FormatDecimalPlace();
            farLegDealerRate.FormatDecimalPlace();

            // 初始化DealerSpotRate, ForwardPoint
            var baseSwapQuoteVm = new BaseSwapQuoteVM
                                      {
                                          TraderSpotRateBid = nearLegDealerRate.TraderSpotRateBid,
                                          TraderSpotRateAsk = nearLegDealerRate.TraderSpotRateAsk,
                                          NearLegFwdPointBid = nearLegDealerRate.ForwardPointBid,
                                          NearLegFwdPointAsk = nearLegDealerRate.ForwardPointAsk,
                                          FarLegFwdPointBid = farLegDealerRate.ForwardPointBid,
                                          FarLegFwdPointAsk = farLegDealerRate.ForwardPointAsk
                                      };

            // 计算DealerSwapPoint
            decimal buysellSwapPoints;
            decimal sellbuySwapPoints;
            this.CalSwapPoints(
                nearLegTenor, 
                farLegTenor,
                nearLegDealerRate.ForwardPointBid,
                nearLegDealerRate.ForwardPointAsk,
                farLegDealerRate.ForwardPointBid,
                farLegDealerRate.ForwardPointAsk, 
                out buysellSwapPoints, 
                out sellbuySwapPoints);

            baseSwapQuoteVm.BuySellTraderSwapPoints = buysellSwapPoints;
            baseSwapQuoteVm.SellBuyTraderSwapPoints = sellbuySwapPoints;

            // 计算 Dealer Far Leg Rate 价格，并得出最终Swap的两个价格:
            // 客户 Buy-Sell ：
            // Dealer Near Leg Rate : Dealer Rate Ask
            // Dealer Far Leg Rate : Dealer Near Leg Rate + 客户Buy-Sell Dealer Swap Point
            // 客户 Sell-Buy ：
            // Dealer Near Leg Rate : Dealer Rate Bid
            // Dealer Far Leg Rate :  Dealer Near Leg Rate + 客户Sell-Buy Dealer Swap Point
            decimal buysellTraderNearLegRate = nearLegDealerRate.TraderRateAsk;
            decimal buysellTraderFarLegRate =
                (buysellTraderNearLegRate + buysellSwapPoints.SpreadToDecimal(symbol.BasisPoint))
                    .FormatPriceBySymbolPoint(farLegDecimalPlace);
            decimal sellbuyTraderNearLegRate = nearLegDealerRate.TraderRateBid;
            decimal sellbuyTraderFarLegRate =
                (sellbuyTraderNearLegRate + sellbuySwapPoints.SpreadToDecimal(symbol.BasisPoint))
                    .FormatPriceBySymbolPoint(farLegDecimalPlace);
            baseSwapQuoteVm.BuySellTraderNearLegRate = buysellTraderNearLegRate;
            baseSwapQuoteVm.BuySellTraderFarLegRate = buysellTraderFarLegRate;
            baseSwapQuoteVm.SellBuyTraderNearLegRate = sellbuyTraderNearLegRate;
            baseSwapQuoteVm.SellBuyTraderFarLegRate = sellbuyTraderFarLegRate;

            // Customer Buy-Sell
            // Tenor = TN ：(ON Bid + TN Bid) - ON Ask
            // 其他： Far Leg Forward Point Bid
            // Customer Sell-Buy
            // Tenor = TN ：(ON Ask + TN Ask) - ON Bid
            // 其他： Far Leg Forward Point Ask
            if (farLegTenor == TenorEnum.TN)
            {
                var farLegForwardPointAsk =
                    (baseSwapQuoteVm.NearLegFwdPointAsk
                     - (baseSwapQuoteVm.NearLegFwdPointBid - baseSwapQuoteVm.FarLegFwdPointBid))
                        .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                var farLegForwardPointBid =
                    (baseSwapQuoteVm.NearLegFwdPointBid
                     - (baseSwapQuoteVm.NearLegFwdPointAsk - baseSwapQuoteVm.FarLegFwdPointAsk))
                        .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                baseSwapQuoteVm.FarLegFwdPointAsk = farLegForwardPointAsk;
                baseSwapQuoteVm.FarLegFwdPointBid = farLegForwardPointBid;
            }
            
            return baseSwapQuoteVm;
        }

        /// <summary>
        /// 获取交叉盘中直盘1的交易方向
        /// </summary>
        /// <param name="crossSymbol">
        /// The cross Symbol.
        /// </param>
        /// <param name="directSymbol">
        /// The direct Symbol.
        /// </param>
        /// <param name="transType">
        /// </param>
        /// <returns>
        /// The <see cref="TransactionTypeEnum"/>.
        /// </returns>
        private TransactionTypeEnum GetCrossTransType1(
            SymbolModel crossSymbol, 
            SymbolModel directSymbol, 
            TransactionTypeEnum transType)
        {
            if (directSymbol.CCY1 == crossSymbol.CCY1)
            {
                return transType;
            }

            return transType == TransactionTypeEnum.Buy ? TransactionTypeEnum.Sell : TransactionTypeEnum.Buy;
        }

        /// <summary>
        /// 获取交叉盘中直盘2的交易方向
        /// </summary>
        /// <param name="crossSymbol">
        /// The cross Symbol.
        /// </param>
        /// <param name="directSymbol">
        /// The direct Symbol.
        /// </param>
        /// <param name="transType">
        /// </param>
        /// <returns>
        /// The <see cref="TransactionTypeEnum"/>.
        /// </returns>
        private TransactionTypeEnum GetCrossTransType2(
            SymbolModel crossSymbol, 
            SymbolModel directSymbol, 
            TransactionTypeEnum transType)
        {
            if (directSymbol.CCY1 == crossSymbol.CCY2)
            {
                return transType == TransactionTypeEnum.Buy ? TransactionTypeEnum.Sell : TransactionTypeEnum.Buy;
            }

            return transType;
        }

        /// <summary>
        ///     获取forwardpoints
        /// </summary>
        /// <returns>
        ///     The <see cref="List" />.
        /// </returns>
        private List<ForwardPointModel> GetForwardPoints()
        {
            return this.GetRepository<IForwardPointCacheRepository>().GetForwardPoint();
        }

        /// <summary>
        /// 获取交叉盘DsRate
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="symbol1">
        /// The symbol 1.
        /// </param>
        /// <param name="symbol2">
        /// The symbol 2.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        /// <param name="businessUnit">
        /// The business unit.
        /// </param>
        /// <returns>
        /// The <see cref="BaseForwardRate"/>.
        /// </returns>
        private BaseForwardRate TempCalcCrossDsRate(
            SymbolModel symbol, 
            SymbolModel symbol1, 
            SymbolModel symbol2, 
            DateTime valueDate, 
            BaseBusinessUnitVM businessUnit)
        {
            var forwardRate = new BaseForwardRate();
            string usdId = this.GetRepository<ICurrencyCacheRepository>().GetIdByName(USD);
            BaseQuoteVM spot = this.GetMarketTickQuote(symbol.SymbolID);

            TenorEnum tenor;
            DateTime expiretime;
            bool result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                businessUnit.BusinessUnitID, 
                valueDate, 
                symbol.SymbolID, 
                out tenor, 
                out expiretime);
            if (!result)
            {
                return null;
            }

            // 交叉盘报价位数
            int decimalPlace = tenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            // 即期交叉盘直接返回交叉盘报价
            if (tenor == TenorEnum.SP)
            {
                SwapPrice price = this.GetMarketQuote(symbol.CCY1, symbol.CCY2);
                forwardRate.DsBid = price.QuotePrice.DsBid;
                forwardRate.DsAsk = price.QuotePrice.DsAsk;
                forwardRate.TraderRateBid = price.QuotePrice.TraderBid;
                forwardRate.TraderRateAsk = price.QuotePrice.TraderAsk;
                forwardRate.DsBid = forwardRate.DsBid.FormatPriceBySymbolPoint(decimalPlace);
                forwardRate.DsAsk = forwardRate.DsAsk.FormatPriceBySymbolPoint(decimalPlace);
                forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(decimalPlace);
                forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(decimalPlace);
                return forwardRate;
            }

            TenorEnum tenor1;
            result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                businessUnit.BusinessUnitID, 
                valueDate, 
                symbol1.SymbolID, 
                out tenor1, 
                out expiretime);
            TenorEnum tenor2;
            result = ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                businessUnit.BusinessUnitID, 
                valueDate, 
                symbol2.SymbolID, 
                out tenor2, 
                out expiretime);

            BaseForwardRate ccy1DirectRate = null;
            BaseForwardRate ccy2DirectRate = null;
            if (tenor1 == TenorEnum.BD)
            {
                ccy1DirectRate = this.TempGetDirectBDTraderRate(symbol1, valueDate, businessUnit);
            }
            else
            {
                ccy1DirectRate = this.TempCalcDirectTenorTraderRate(symbol1, tenor1);
            }

            if (tenor2 == TenorEnum.BD)
            {
                ccy2DirectRate = this.TempGetDirectBDTraderRate(symbol2, valueDate, businessUnit);
            }
            else
            {
                ccy2DirectRate = this.TempCalcDirectTenorTraderRate(symbol2, tenor2);
            }

            if (ccy1DirectRate == null || ccy2DirectRate == null)
            {
                return null;
            }

            int indirect = 0;
            if (symbol1.CCY2 == usdId)
            {
                indirect++;
            }

            if (symbol2.CCY2 == usdId)
            {
                indirect++;
            }

            switch (indirect)
            {
                    // 两个直接报价 B2/A1         A2/B1
                case 0:
                    forwardRate.DsBid = ccy2DirectRate.DsBid / ccy1DirectRate.DsAsk;
                    forwardRate.DsAsk = ccy2DirectRate.DsAsk / ccy1DirectRate.DsBid;
                    forwardRate.TraderRateBid = ccy2DirectRate.TraderRateBid / ccy1DirectRate.TraderRateAsk;
                    forwardRate.TraderRateAsk = ccy2DirectRate.TraderRateAsk / ccy1DirectRate.TraderRateBid;
                    break;

                    // 混合报价  B1*B2                    A1*A2
                case 1:
                    forwardRate.DsBid = ccy1DirectRate.DsBid * ccy2DirectRate.DsBid;
                    forwardRate.DsAsk = ccy1DirectRate.DsAsk * ccy2DirectRate.DsAsk;
                    forwardRate.TraderRateBid = ccy1DirectRate.TraderRateBid * ccy2DirectRate.TraderRateBid;
                    forwardRate.TraderRateAsk = ccy1DirectRate.TraderRateAsk * ccy2DirectRate.TraderRateAsk;
                    break;

                    // 两个间接报价 B1/A2                    A1/B2
                case 2:
                    forwardRate.DsBid = ccy1DirectRate.DsBid / ccy2DirectRate.DsAsk;
                    forwardRate.DsAsk = ccy1DirectRate.DsAsk / ccy2DirectRate.DsBid;
                    forwardRate.TraderRateBid = ccy1DirectRate.TraderRateBid / ccy2DirectRate.TraderRateAsk;
                    forwardRate.TraderRateAsk = ccy1DirectRate.TraderRateAsk / ccy2DirectRate.TraderRateBid;
                    break;
            }

            forwardRate.DsBid = forwardRate.DsBid.FormatPriceBySymbolPoint(decimalPlace);
            forwardRate.DsAsk = forwardRate.DsAsk.FormatPriceBySymbolPoint(decimalPlace);
            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(decimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(decimalPlace);

            switch (tenor)
            {
                case TenorEnum.ON:
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        forwardRate.ForwardPointBid =
                            (spot.TraderSpotAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                        forwardRate.ForwardPointAsk =
                            (spot.TraderSpotBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    }
                    else
                    {
                        forwardRate.ForwardPointBid =
                            (spot.TraderSpotAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                        forwardRate.ForwardPointAsk =
                            (spot.TraderSpotBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    }

                    break;
                case TenorEnum.TN:

                    forwardRate.ForwardPointBid =
                        (spot.TraderSpotAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    forwardRate.ForwardPointAsk =
                        (spot.TraderSpotBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    break;
                case TenorEnum.SP:
                    forwardRate.ForwardPointBid = decimal.Zero;
                    forwardRate.ForwardPointAsk = decimal.Zero;
                    break;
                default:
                    forwardRate.ForwardPointBid =
                        (forwardRate.TraderRateBid - spot.TraderSpotBid).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    forwardRate.ForwardPointAsk =
                        (forwardRate.TraderRateAsk - spot.TraderSpotAsk).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    break;
            }

            return forwardRate;
        }

        /// <summary>
        /// 获取交叉盘的trader rate
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="symbol1">
        /// 交叉商品1
        /// </param>
        /// <param name="symbol2">
        /// 交叉商品2
        /// </param>
        /// <param name="valueDate">
        /// ValueDate
        /// </param>
        /// <param name="busiUnit">
        /// 所属业务区
        /// </param>
        /// <param name="transType">
        /// 交易方向
        /// </param>
        /// <param name="isCashDeal">
        /// 是否为现金交易
        /// </param>
        /// <returns>
        /// 目标TradeRate
        /// </returns>
        private BaseForwardRate TempCalcCrossTraderRate(
            SymbolModel symbol, 
            SymbolModel symbol1, 
            SymbolModel symbol2, 
            DateTime valueDate, 
            BaseBusinessUnitVM busiUnit, 
            TransactionTypeEnum transType, 
            bool isCashDeal = false)
        {
            var forwardRate = new BaseForwardRate();
            forwardRate.SymbolId = symbol.SymbolID;
            forwardRate.SymbolName = symbol.SymbolName;
            var currnecyRep = this.GetRepository<ICurrencyCacheRepository>();
            string usdId = currnecyRep.GetIdByName(USD);

            BaseQuoteVM ccy1DirectSpot = this.GetMarketTickQuote(symbol1.SymbolID);
            BaseQuoteVM ccy2DirectSpot = this.GetMarketTickQuote(symbol2.SymbolID);
            BaseQuoteVM spot = this.GetMarketTickQuote(symbol.SymbolID);
            if (ccy1DirectSpot == null || ccy2DirectSpot == null || spot == null)
            {
                return null;
            }

            TransactionTypeEnum transType1 = this.GetCrossTransType1(symbol, symbol1, transType);
            TransactionTypeEnum transType2 = this.GetCrossTransType2(symbol, symbol2, transType);
            TenorEnum tenor;
            TenorEnum tenor1;
            TenorEnum tenor2;

            if (isCashDeal)
            {
                ////对于现金交易无需进行Tener的计算，同时不考虑CutOffTime和SettleTime的影响
                tenor = TenorEnum.ON;
                tenor1 = TenorEnum.ON;
                tenor2 = TenorEnum.ON;
            }
            else
            {
                BaseSymbolTenorVDVM tenorVdvm = ValueDateCore.Instance.GetTenorByValueDateWeSellCcy(
                    symbol.SymbolID, 
                    valueDate, 
                    busiUnit.BusinessUnitID, 
                    this.IsKVBSellCCY1(transType));

                BaseSymbolTenorVDVM tenor1Vdvm = ValueDateCore.Instance.GetTenorByValueDateWeSellCcy(
                    symbol1.SymbolID, 
                    valueDate, 
                    busiUnit.BusinessUnitID, 
                    this.IsKVBSellCCY1(transType1));

                BaseSymbolTenorVDVM tenor2Vdvm = ValueDateCore.Instance.GetTenorByValueDateWeSellCcy(
                    symbol2.SymbolID, 
                    valueDate, 
                    busiUnit.BusinessUnitID, 
                    this.IsKVBSellCCY1(transType2));

                if (tenorVdvm == null || tenor1Vdvm == null || tenor2Vdvm == null)
                {
                    return null;
                }

                tenor = tenorVdvm.Tenor;
                tenor1 = tenor1Vdvm.Tenor;
                tenor2 = tenor2Vdvm.Tenor;
            }

            // 交叉盘报价位数
            int decimalPlace = tenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            BaseForwardRate ccy1DirectRate = tenor1 == TenorEnum.BD
                                                 ? this.TempGetDirectBDTraderRate(symbol1, valueDate, busiUnit)
                                                 : this.TempCalcDirectTenorTraderRate(symbol1, tenor1);

            BaseForwardRate ccy2DirectRate = tenor2 == TenorEnum.BD
                                                 ? this.TempGetDirectBDTraderRate(symbol2, valueDate, busiUnit)
                                                 : this.TempCalcDirectTenorTraderRate(symbol2, tenor2);

            if (ccy1DirectRate == null || ccy2DirectRate == null)
            {
                return null;
            }

            int indirect = 0;
            if (symbol1.CCY2 == usdId)
            {
                indirect++;
            }

            if (symbol2.CCY2 == usdId)
            {
                indirect++;
            }

            switch (indirect)
            {
                    // 两个直接报价 B2/A1         A2/B1
                case 0:
                    forwardRate.TraderRateBid = ccy2DirectRate.TraderRateBid / ccy1DirectRate.TraderRateAsk;
                    forwardRate.TraderRateAsk = ccy2DirectRate.TraderRateAsk / ccy1DirectRate.TraderRateBid;
                    break;

                    // 混合报价  B1*B2                    A1*A2
                case 1:
                    forwardRate.TraderRateBid = ccy1DirectRate.TraderRateBid * ccy2DirectRate.TraderRateBid;
                    forwardRate.TraderRateAsk = ccy1DirectRate.TraderRateAsk * ccy2DirectRate.TraderRateAsk;
                    break;

                    // 两个间接报价 B1/A2                    A1/B2
                case 2:
                    forwardRate.TraderRateBid = ccy1DirectRate.TraderRateBid / ccy2DirectRate.TraderRateAsk;
                    forwardRate.TraderRateAsk = ccy1DirectRate.TraderRateAsk / ccy2DirectRate.TraderRateBid;
                    break;
            }

            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(decimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(decimalPlace);

            switch (tenor)
            {
                case TenorEnum.ON:
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        forwardRate.ForwardPointBid =
                            (spot.TraderSpotAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                        forwardRate.ForwardPointAsk =
                            (spot.TraderSpotBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    }
                    else
                    {
                        forwardRate.ForwardPointBid =
                            (spot.TraderSpotAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                        forwardRate.ForwardPointAsk =
                            (spot.TraderSpotBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    }

                    break;
                case TenorEnum.TN:
                    forwardRate.ForwardPointBid =
                        (spot.TraderSpotAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    forwardRate.ForwardPointAsk =
                        (spot.TraderSpotBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    break;
                case TenorEnum.SP:
                    forwardRate.ForwardPointBid = decimal.Zero;
                    forwardRate.ForwardPointAsk = decimal.Zero;
                    break;
                default:
                    forwardRate.ForwardPointBid =
                        (forwardRate.TraderRateBid - spot.TraderSpotBid).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    forwardRate.ForwardPointAsk =
                        (forwardRate.TraderRateAsk - spot.TraderSpotAsk).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    break;
            }

            return forwardRate;
        }

        /// <summary>
        /// 获取BrokenDate的远期点数
        /// </summary>
        /// <param name="brokenValueDate">
        /// 非预设Tenor的ValueDate
        /// </param>
        /// <param name="usdCCY">
        /// USDID
        /// </param>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="busUnitID">
        /// The bus Unit ID.
        /// </param>
        /// <returns>
        /// 返回对应的远期点数信息
        /// </returns>
        private ForwardPointTenorModel TempCalcDirectBDForwardPoint(
            DateTime brokenValueDate, 
            CurrencyModel usdCCY, 
            SymbolModel symbol, 
            string busUnitID)
        {
            var intStartTenor = (int)TenorEnum.SN;
            var intEndTenor = (int)TenorEnum.Y1;
            List<ForwardPointModel> forwardPoints = this.GetForwardPoints();
            DateTime valueDate1 = Util.GetDefaultDateTime();
            DateTime valueDate2 = Util.GetDefaultDateTime();

            // 获取brokenValueDate相邻的两个Tenor
            while (intStartTenor <= intEndTenor)
            {
                // 如果startTenor为1Y
                if (intStartTenor == intEndTenor)
                {
                    return null;
                }

                // 根据Tenor获取ValueDate
                string prompt;
                valueDate1 = ValueDateCore.Instance.GetValueDateByTenorWithCurrentOrNextLocalTradedate(
                    busUnitID,
                    (TenorEnum)intStartTenor,
                    symbol.SymbolID,
                    out prompt);
                valueDate2 = ValueDateCore.Instance.GetValueDateByTenorWithCurrentOrNextLocalTradedate(
                    busUnitID,
                    (TenorEnum)(intStartTenor + 1),
                    symbol.SymbolID,
                    out prompt);

                // 判断brokenValueDate 是否处于 这两个tenor之间
                if (brokenValueDate.Date < valueDate2.Date && brokenValueDate.Date > valueDate1.Date)
                {
                    break;
                }

                intStartTenor++;
            }

            valueDate1 = Convert.ToDateTime(valueDate1.ToString("yyyy-MM-dd"));
            valueDate2 = Convert.ToDateTime(valueDate2.ToString("yyyy-MM-dd"));
            brokenValueDate = Convert.ToDateTime(brokenValueDate.ToString("yyyy-MM-dd"));

            // 得到两个VD之间的天数
            int valueDateDiffDays = (valueDate2 - valueDate1).Days;

            // 得到brokenValueDate 和 VD1之间的天数
            int brokenValueDateLeftDiffDays = (brokenValueDate - valueDate1).Days;

            // 得到两个相邻的tenor
            var tenor1 = (TenorEnum)intStartTenor;
            var tenor2 = (TenorEnum)(intStartTenor + 1);

            // 计算SwapPoints, 两个ValueDate肯定都晚于Spot
            BaseForwardRate nearForwPoint = this.GetBrokenDateForwardPoint(symbol, tenor1);

            // = this.GetForwardPointByTenor(tenor1, forwardPoints, usdCCY.ccy);
            BaseForwardRate farForwPoint = this.GetBrokenDateForwardPoint(symbol, tenor2);

            // = this.GetForwardPointByTenor(tenor2, forwardPoints, usdCCY);
            if (nearForwPoint == null || farForwPoint == null)
            {
                return null;
            }

            // 得到Swap点数
            decimal swapPointAsk = farForwPoint.ForwardPointAsk - nearForwPoint.ForwardPointAsk;

            // 需要考虑保留位数
            decimal swapPointPerDayAsk = swapPointAsk / valueDateDiffDays;
            decimal brokenForwPointAsk =
                (nearForwPoint.ForwardPointAsk + (swapPointPerDayAsk * brokenValueDateLeftDiffDays))
                    .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

            // 得到Swap点数
            decimal swapPointBid = farForwPoint.ForwardPointBid - nearForwPoint.ForwardPointBid;

            // 需要考虑保留位数
            decimal swapPointPerDayBid = swapPointBid / valueDateDiffDays;
            decimal brokenForwPointBid =
                (nearForwPoint.ForwardPointBid + (swapPointPerDayBid * brokenValueDateLeftDiffDays))
                    .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);

            return new ForwardPointTenorModel { Ask = brokenForwPointAsk, Bid = brokenForwPointBid };
        }

        /// <summary>
        /// 获取直盘Teonr forward rate
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="tenor">
        /// The tenor.
        /// </param>
        /// <returns>
        /// 返回Forward Rate
        /// </returns>
        private BaseForwardRate TempCalcDirectTenorTraderRate(SymbolModel symbol, TenorEnum tenor)
        {
            var forwardRate = new BaseForwardRate();
            List<ForwardPointModel> forwardPoints = this.GetForwardPoints();
            int forwardDecimalPlace = tenor == TenorEnum.SP
                                          ? symbol.DecimalPlace
                                          : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            if (forwardPoints == null || forwardPoints.Count == 0)
            {
                TraceManager.Warn.Write("GetTenorDirectRate", "ForwardPoints is null");
                return null;
            }

            forwardRate.SymbolId = symbol.SymbolID;
            forwardRate.SymbolName = symbol.SymbolName;
            forwardRate.Tenor = tenor;
            forwardRate.IsBrokenDate = false;

            string symbolName = symbol.SymbolName;
            string cur1 = symbolName.Substring(0, 3);
            string cur2 = symbolName.Substring(3, 3);
            SwapPrice marketSpot = this.GetMarketQuote(symbol.CCY1, symbol.CCY2);

            if (marketSpot.PriceDirection == EnumDirection.Equals
                || marketSpot.PriceDirection == EnumDirection.NotExisting)
            {
                TraceManager.Warn.Write("GetTenorDirectRate", "marketSpot PriceDirection error");
                return null;
            }

            TickQuoteModel spot = marketSpot.QuotePrice;

            forwardRate.SymbolId = marketSpot.SymbolID;

            // forwardRate.SymbolName = marketSpot.SymbolName;
            if (tenor == TenorEnum.SP)
            {
                forwardRate.ForwardPointBid = decimal.Zero;
                forwardRate.ForwardPointAsk = decimal.Zero;
                forwardRate.TraderRateBid = spot.TraderBid;
                forwardRate.TraderRateAsk = spot.TraderAsk;
                forwardRate.DsBid = spot.DsBid;
                forwardRate.DsAsk = spot.DsAsk;
                return forwardRate;
            }

            ForwardPointModel forwardPoint = cur1 == USD
                                                 ? forwardPoints.FirstOrDefault(o => o.CurrencyName == cur2)
                                                 : forwardPoints.FirstOrDefault(o => o.CurrencyName == cur1);

            if (forwardPoint == null)
            {
                return null;
            }

            ForwardPointTenorModel forwardPointTenor = forwardPoint.Tenors.FirstOrDefault(o => o.Tenor == tenor);
            if (forwardPointTenor == null)
            {
                return null;
            }

            forwardRate.ForwardPointBid = forwardPointTenor.Bid;
            forwardRate.ForwardPointAsk = forwardPointTenor.Ask;

            if (tenor == TenorEnum.ON)
            {
                if (symbol.ValueDate == ValueDateEnum.T1)
                {
                    forwardRate.ForwardPointBid = forwardPointTenor.Bid;
                    forwardRate.ForwardPointAsk = forwardPointTenor.Ask;

                    forwardRate.TraderRateBid = spot.TraderBid
                                                - forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);
                    forwardRate.TraderRateAsk = spot.TraderAsk
                                                - forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);

                    forwardRate.DsBid = spot.DsBid - forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);
                    forwardRate.DsAsk = spot.DsAsk - forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);
                }
                else
                {
                    ForwardPointTenorModel tenorPoint = forwardPoint.Tenors.FirstOrDefault(o => o.Tenor == TenorEnum.TN);
                    forwardRate.TraderRateBid = spot.TraderBid
                                                - (tenorPoint.Ask + forwardPointTenor.Ask).SpreadToDecimal(
                                                    symbol.BasisPoint);
                    forwardRate.TraderRateAsk = spot.TraderAsk
                                                - (tenorPoint.Bid + forwardPointTenor.Bid).SpreadToDecimal(
                                                    symbol.BasisPoint);
                    forwardRate.ForwardPointBid = tenorPoint.Bid + forwardPointTenor.Bid;
                    forwardRate.ForwardPointAsk = tenorPoint.Ask + forwardPointTenor.Ask;

                    forwardRate.DsBid = spot.DsBid
                                        - (tenorPoint.Ask + forwardPointTenor.Ask).SpreadToDecimal(symbol.BasisPoint);
                    forwardRate.DsAsk = spot.DsAsk
                                        - (tenorPoint.Bid + forwardPointTenor.Bid).SpreadToDecimal(symbol.BasisPoint);
                }

                // 格式化远期报价
                forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.DsBid = forwardRate.DsBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.DsAsk = forwardRate.DsAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
                return forwardRate;
            }

            if (tenor == TenorEnum.TN)
            {
                forwardRate.TraderRateBid = spot.TraderBid - forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);
                forwardRate.TraderRateAsk = spot.TraderAsk - forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);
                forwardRate.DsBid = spot.DsBid - forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);
                forwardRate.DsAsk = spot.DsAsk - forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);

                forwardRate.ForwardPointBid = forwardPointTenor.Bid;
                forwardRate.ForwardPointAsk = forwardPointTenor.Ask;

                // 格式化远期报价
                forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.DsBid = forwardRate.DsBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.DsAsk = forwardRate.DsAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
                return forwardRate;
            }

            forwardRate.TraderRateBid = spot.TraderBid + forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);
            forwardRate.TraderRateAsk = spot.TraderAsk + forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);
            forwardRate.DsBid = spot.DsBid + forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);
            forwardRate.DsAsk = spot.DsAsk + forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);

            // 格式化远期报价
            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.DsBid = forwardRate.DsBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.DsAsk = forwardRate.DsAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            return forwardRate;
        }

        /// <summary>
        /// 获取直盘BDtrader rate
        ///     改了名字
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="brokenDate">
        /// brokenDate
        /// </param>
        /// <param name="bu">
        /// 业务区
        /// </param>
        /// <returns>
        /// 返回 BrokenDate 信息
        /// </returns>
        private BaseForwardRate TempGetDirectBDTraderRate(
            SymbolModel symbol, 
            DateTime brokenDate, 
            BaseBusinessUnitVM bu)
        {
            var forwardRate = new BaseForwardRate();
            string ccy1Id = symbol.CCY1;
            string ccy2Id = symbol.CCY2;
            List<ForwardPointModel> forwardPoints = this.GetRepository<IForwardPointCacheRepository>().GetForwardPoint();
            SwapPrice spotPrice = this.GetMarketQuote(ccy1Id, ccy2Id);
            if (spotPrice == null || spotPrice.PriceDirection == EnumDirection.Equals
                || spotPrice.PriceDirection == EnumDirection.NotExisting)
            {
                return null;
            }

            int basePoint = symbol.BasisPoint;
            int forwardDecimalPlace = symbol.DecimalPlace + symbol.ForwardPointDecimalPlace;

            // token
            BaseCurrencyVM usdCCY =
                this.GetRepository<ICurrencyCacheRepository>().Filter(o => o.CurrencyName.Equals(USD)).FirstOrDefault();
            if (usdCCY == null)
            {
                TraceManager.Warn.Write("ViewModel", "Can't find usd in currency local cache repository.");
                return null;
            }

            ForwardPointTenorModel forwardPoint = this.TempCalcDirectBDForwardPoint(
                brokenDate, 
                usdCCY.PropSet, 
                symbol, 
                bu.BusinessUnitID);
            if (forwardPoint == null)
            {
                return null;
            }

            forwardRate.SymbolId = symbol.SymbolID;
            forwardRate.SymbolName = symbol.SymbolName;
            forwardRate.TraderSpotRateBid = spotPrice.QuotePrice.TraderBid;
            forwardRate.TraderSpotRateAsk = spotPrice.QuotePrice.TraderAsk;
            forwardRate.TraderRateBid =
                (spotPrice.QuotePrice.TraderBid + forwardPoint.Bid.SpreadToDecimal(basePoint)).FormatPriceBySymbolPoint(
                    forwardDecimalPlace);
            forwardRate.TraderRateAsk =
                (spotPrice.QuotePrice.TraderAsk + forwardPoint.Ask.SpreadToDecimal(basePoint)).FormatPriceBySymbolPoint(
                    forwardDecimalPlace);
            forwardRate.ForwardPointBid = forwardPoint.Bid;
            forwardRate.ForwardPointAsk = forwardPoint.Ask;
            forwardRate.DsBid =
                (spotPrice.QuotePrice.DsBid + forwardPoint.Bid.SpreadToDecimal(basePoint)).FormatPriceBySymbolPoint(
                    forwardDecimalPlace);
            forwardRate.DsAsk =
                (spotPrice.QuotePrice.DsAsk + forwardPoint.Ask.SpreadToDecimal(basePoint)).FormatPriceBySymbolPoint(
                    forwardDecimalPlace);
            forwardRate.IsBrokenDate = true;
            forwardRate.VauleDate = brokenDate;
            return forwardRate;
        }

        #endregion
    }
}