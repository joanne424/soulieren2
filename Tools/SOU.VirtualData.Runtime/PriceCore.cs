// <copyright file="PriceCore.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date>2015/5/5 10:39:27</date>
// <modify>
//   修改人：zoukp
//   修改时间：2015/5/5 10:39:27
//   修改描述：新建 PriceCore.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

namespace DM2.Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BaseViewModel;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Common.ErrorMsg;
    using Infrastructure.Data;
    using Infrastructure.Data.Tools;
    using Infrastructure.Log;
    using Infrastructure.Models;

    /// <summary>
    ///     报价计算中心
    /// </summary>
    public partial class PriceCore : BaseVm
    {
        #region Constants

        /// <summary>
        ///     USD货币
        /// </summary>
        private const string USD = "USD";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static PriceCore instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="PriceCore" /> class from being created.
        ///     Initializes a new instance of the PriceCore class.
        /// </summary>
        private PriceCore()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static PriceCore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PriceCore();
                }

                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 计算挂单价格
        /// </summary>
        /// <param name="quoteConfig">
        /// </param>
        /// <param name="symbolVM">
        /// </param>
        /// <param name="dealPosition">
        /// </param>
        /// <param name="traderBid">
        /// </param>
        /// <param name="traderAsk">
        /// </param>
        /// <param name="customerBid">
        /// </param>
        /// <param name="customerAsk">
        /// </param>
        public void CalOrderRate(
            QuoteGroupModel.QuoteConfigModel quoteConfig, 
            BaseSymbolVM symbolVM, 
            decimal dealPosition, 
            decimal traderBid, 
            decimal traderAsk, 
            out decimal customerBid, 
            out decimal customerAsk)
        {
            customerBid = decimal.Zero;
            customerAsk = decimal.Zero;
            if (quoteConfig != null)
            {
                IEnumerable<QuoteGroupModel.CustomerSpreadConfigModel> fwdSpreadConfigList =
                    quoteConfig.CustomerSpreadConfigList.Where(
                        o =>
                        o.TradableInstrument == TradInstrumentEnum.FXSpot
                        && o.TradableOrderType == CustSpreadTradeOrderEnum.PendingOrder);

                if (fwdSpreadConfigList == null || !fwdSpreadConfigList.Any())
                {
                    return;
                }

                QuoteGroupModel.CustomerSpreadConfigModel findedCustSpreadConfig =
                    this.FindCustSpreadConfig(fwdSpreadConfigList, dealPosition);

                if (findedCustSpreadConfig == null)
                {
                    return;
                }

                // 挂单价要格式化的小数位数
                int decimalPlace = symbolVM.GetPriceDecimalPlace(TenorEnum.SP);

                // 计算出客户的Spot价格
                customerBid =
                    (traderBid - findedCustSpreadConfig.TTBid.SpreadToDecimal(symbolVM.BasisPoint))
                        .FormatPriceBySymbolPoint(decimalPlace);
                customerAsk =
                    (traderAsk + findedCustSpreadConfig.TTAsk.SpreadToDecimal(symbolVM.BasisPoint))
                        .FormatPriceBySymbolPoint(decimalPlace);
            }
        }

        /// <summary>
        /// 获取当前客户点差价格，包含trader spot rate，trader rate，forward point
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="valueDate">
        /// 交割日
        /// </param>
        /// <param name="tenor">
        /// tenor
        /// </param>
        /// <param name="busUnitID">
        /// The bus Unit ID.
        /// </param>
        /// <returns>
        /// 远期汇率
        /// </returns>
        /// <summary>
        /// 获取交易员远期价格
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="valueDate">
        /// 交割日
        /// </param>
        /// <returns>
        /// 远期汇率
        /// </returns>
        public BaseForwardRate CalTraderRate(SymbolModel symbol, DateTime valueDate, TenorEnum tenor, string busUnitID)
        {
            var forwardRate = new BaseForwardRate();
            BaseBusinessUnitVM bu = this.GetRepository<IBusinessUnitCacheRepository>().GetByID(busUnitID);
            List<ForwardPointModel> forwardPoints = this.GetRepository<IForwardPointCacheRepository>().GetForwardPoint();
            SwapPrice spotPrice = this.GetMarketQuote(symbol.CCY1, symbol.CCY2);
            if (spotPrice == null || spotPrice.PriceDirection == EnumDirection.Equals
                || spotPrice.PriceDirection == EnumDirection.NotExisting)
            {
                return null;
            }

            if (tenor == TenorEnum.SP)
            {
                string prompt;
                forwardRate.VauleDate = ValueDateCore.Instance.GetValueDateByTenor(
                    bu.BusinessUnitID, 
                    tenor, 
                    symbol.SymbolID, 
                    out prompt);
                forwardRate.SymbolId = symbol.SymbolID;
                forwardRate.SymbolName = symbol.SymbolName;
                forwardRate.IsBrokenDate = false;
                forwardRate.Tenor = tenor;
                forwardRate.ForwardPointBid = decimal.Zero;
                forwardRate.ForwardPointAsk = decimal.Zero;
                forwardRate.TraderRateBid = spotPrice.QuotePrice.TraderBid;
                forwardRate.TraderRateAsk = spotPrice.QuotePrice.TraderAsk;
            }
            else if (tenor == TenorEnum.BD)
            {
                forwardRate = this.GetTraderBrokenDate(symbol, valueDate, bu);
            }
            else
            {
                forwardRate = this.GetSwapTraderRateByTenor(symbol, bu, tenor);
            }

            return forwardRate;
        }

        /// <summary>
        /// 计算 customer rate
        /// </summary>
        /// <param name="symbolId">
        /// 商品Id
        /// </param>
        /// <param name="inputCcyid">
        /// 客户输入的ccy
        /// </param>
        /// <param name="inputAmount">
        /// 客户输入的amount
        /// </param>
        /// <param name="valueDate">
        /// 交割日
        /// </param>
        /// <param name="transType">
        /// The trans Type.
        /// </param>
        /// <param name="rateType">
        /// 交易类型
        /// </param>
        /// <param name="busiUnit">
        /// 所属Bu
        /// </param>
        /// <param name="quoteGroup">
        /// 报价组合
        /// </param>
        /// <param name="tradaOrder">
        /// 挂单类型
        /// </param>
        /// <param name="isCashDeal">
        /// 是否为现金交易，因为现金交易不受CuttoffTime和SettleTime的影响
        /// </param>
        /// <returns>
        /// 远期汇率
        /// </returns>
        public BaseForwardRate CalcCurrentCustomerRate(
            string symbolId, 
            string inputCcyid, 
            decimal inputAmount, 
            DateTime valueDate, 
            TransactionTypeEnum transType, 
            TradeRateTypeEnum rateType, 
            BaseBusinessUnitVM busiUnit, 
            QuoteGroupModel quoteGroup, 
            CustSpreadTradeOrderEnum tradaOrder = CustSpreadTradeOrderEnum.MarketOrder, 
            bool isCashDeal = false)
        {
            BaseSymbolVM symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(symbolId);
            int basePoint = symbol.BasisPoint;

            TenorEnum tenor;

            if (isCashDeal)
            {
                //// 对于现金交易，只需要判断是否在LocalTradeDate中
                DateTime localTradeDate =
                    busiUnit.GetLocalTradeDate(RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busiUnit.BusinessUnitID));
                if (localTradeDate == default(DateTime))
                {
                    return null;
                }

                tenor = TenorEnum.ON;
            }
            else
            {
                BaseSymbolTenorVDVM tenorVd = ValueDateCore.Instance.GetTenorByValueDateWeSellCcy(
                    symbolId, 
                    valueDate, 
                    busiUnit.BusinessUnitID, 
                    this.IsKVBSellCCY1(transType));
                if (tenorVd == null)
                {
                    return null;
                }

                tenor = tenorVd.Tenor;
            }

            int forwardDecimalPlace = tenor == TenorEnum.SP
                                          ? symbol.DecimalPlace
                                          : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            BaseQuoteVM swapPrice = instance.GetMarketTickQuote(symbol.SymbolID);
            if (swapPrice == null)
            {
                return null;
            }

            BaseForwardRate forwardRate = this.CalcCurrentTraderRate(
                symbolId, 
                valueDate, 
                transType, 
                busiUnit, 
                isCashDeal);

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
                inputCcyid, 
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
        /// 计算用于显示的Customer Rate(页面上显示的ForwardPoint在ON/TN时Bid和Ask是相反的)
        /// </summary>
        /// <param name="symbolId">
        /// 商品Id
        /// </param>
        /// <param name="inputCcyid">
        /// 客户输入的ccy
        /// </param>
        /// <param name="inputAmount">
        /// 客户输入的amount
        /// </param>
        /// <param name="valueDate">
        /// 交割日
        /// </param>
        /// <param name="transType">
        /// The trans Type.
        /// </param>
        /// <param name="rateType">
        /// 交易类型
        /// </param>
        /// <param name="busiUnit">
        /// 所属Bu
        /// </param>
        /// <param name="quoteGroup">
        /// 报价组合
        /// </param>
        /// <param name="tradaOrder">
        /// 挂单类型
        /// </param>
        /// <param name="isCashDeal">
        /// 是否为现金交易，因为现金交易不受CuttoffTime和SettleTime的影响
        /// </param>
        /// <returns>
        /// 远期汇率
        /// </returns>
        public BaseForwardRate CalcCurrentCustomerRateForDisplay(
            string symbolId, 
            string inputCcyid, 
            decimal inputAmount, 
            DateTime valueDate, 
            TransactionTypeEnum transType, 
            TradeRateTypeEnum rateType, 
            BaseBusinessUnitVM busiUnit, 
            QuoteGroupModel quoteGroup, 
            CustSpreadTradeOrderEnum tradaOrder = CustSpreadTradeOrderEnum.MarketOrder, 
            bool isCashDeal = false)
        {
            BaseForwardRate baseForwardRate = this.CalcCurrentCustomerRate(
                symbolId, 
                inputCcyid, 
                inputAmount, 
                valueDate, 
                transType, 
                rateType, 
                busiUnit, 
                quoteGroup, 
                tradaOrder, 
                isCashDeal);
            if (baseForwardRate == null)
            {
                return null;
            }

            string rstPrompt;
            TenorEnum tenor = ValueDateCore.Instance.GetTenorByValueDate(
                busiUnit.BusinessUnitID, 
                valueDate, 
                symbolId, 
                out rstPrompt);
            if (rstPrompt != Common.Success)
            {
                return null;
            }

            // ON/TN的情况下界面显示的ForwardPoint与需要的方向相反
            if (tenor == TenorEnum.ON || tenor == TenorEnum.TN)
            {
                decimal temp = baseForwardRate.ForwardPointBid;
                baseForwardRate.ForwardPointBid = baseForwardRate.ForwardPointAsk;
                baseForwardRate.ForwardPointAsk = temp;
            }

            return baseForwardRate;
        }

        /// <summary>
        /// 根据DealerRate计算对应的CustomerRate
        /// </summary>
        /// <param name="dealerRateBid">
        /// Bid方向的DealerRate
        /// </param>
        /// <param name="dealerRateAsk">
        /// Ask方向的DealerRate
        /// </param>
        /// <param name="customerSpread">
        /// 客户点差
        /// </param>
        /// <param name="symbol">
        /// 货币对
        /// </param>
        /// <param name="customerRateBid">
        /// Bid方向的CustomerRate
        /// </param>
        /// <param name="customerRateAsk">
        /// Ask方向的CustomerRate
        /// </param>
        public void CalcCustomerRateByDealerRate(
            decimal dealerRateBid, 
            decimal dealerRateAsk, 
            QuoteGroupModel.CustomerSpreadConfigModel customerSpread, 
            BaseSymbolVM symbol, 
            out decimal customerRateBid, 
            out decimal customerRateAsk)
        {
            customerRateBid = dealerRateBid;
            customerRateAsk = dealerRateAsk;

            // 找不到客户对应等级的报价
            if (customerSpread == null || symbol == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcCustomerRateByDealerRate", "Input parameter error!");
                return;
            }

            customerRateAsk = dealerRateAsk + customerSpread.TTAsk.SpreadToDecimal(symbol.BasisPoint);
            customerRateBid = dealerRateBid - customerSpread.TTBid.SpreadToDecimal(symbol.BasisPoint);
        }

        /// <summary>
        /// 计算Swap的用于显示的客户价格
        /// </summary>
        /// <param name="symbol">
        /// 货币对
        /// </param>
        /// <param name="nearLegValueDate">
        /// Near Leg 交割日
        /// </param>
        /// <param name="nearLegTenor">
        /// Near Leg Tenor
        /// </param>
        /// <param name="farLegValueDate">
        /// Far Leg 交割日
        /// </param>
        /// <param name="farLegTenor">
        /// Far Leg Tenor
        /// </param>
        /// <param name="quoteGroup">
        /// 报价组
        /// </param>
        /// <param name="businessUnit">
        /// 业务区
        /// </param>
        /// <param name="inputCcyid">
        /// 输入Currency id
        /// </param>
        /// <param name="inputAmount">
        /// 输入量
        /// </param>
        /// <param name="tradeInstrument">
        /// 交易方式
        /// </param>
        /// <param name="nearTransType">
        /// Near Leg交易方向
        /// </param>
        /// <returns>
        /// 价格信息
        /// </returns>
        public BaseSwapQuoteVM CalcSwapCustomerRateForDisplay(
            BaseSymbolVM symbol, 
            DateTime nearLegValueDate, 
            TenorEnum nearLegTenor, 
            DateTime farLegValueDate, 
            TenorEnum farLegTenor, 
            QuoteGroupModel quoteGroup, 
            BaseBusinessUnitVM businessUnit, 
            string inputCcyid, 
            decimal inputAmount, 
            TradInstrumentEnum tradeInstrument, 
            TransactionTypeEnum nearTransType)
        {
            if (symbol == null || nearLegValueDate == default(DateTime) || farLegValueDate == default(DateTime)
                || quoteGroup == null || businessUnit == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapCustomerRateForDisplay", "Input parameter error");
                return null;
            }

            string rstPrompt;
            TenorEnum nearLegVerifiedTenor = ValueDateCore.Instance.GetTenorByValueDate(
                businessUnit.BusinessUnitID, 
                nearLegValueDate, 
                symbol.SymbolID, 
                out rstPrompt);
            if (rstPrompt != Common.Success || nearLegTenor != nearLegVerifiedTenor)
            {
                TraceManager.Warn.Write(
                    "PriceCore.CalcSwapCustomerRateForDisplay", 
                    "Near leg ValueDate and Tenor are not match");
                return null;
            }

            TenorEnum farLegVerifiedTenor = ValueDateCore.Instance.GetTenorByValueDate(
                businessUnit.BusinessUnitID, 
                farLegValueDate, 
                symbol.SymbolID, 
                out rstPrompt);
            if (rstPrompt != Common.Success || farLegTenor != farLegVerifiedTenor)
            {
                TraceManager.Warn.Write(
                    "PriceCore.CalcSwapCustomerRateForDisplay", 
                    "Far leg ValueDate and Tenor are not match");
                return null;
            }

            // 计算CustomerRate
            BaseSwapQuoteVM baseSwapQuoteVm = this.CalcSwapCustomerRate(
                symbol, 
                nearLegValueDate, 
                nearLegTenor, 
                farLegValueDate, 
                farLegTenor, 
                quoteGroup, 
                businessUnit, 
                inputCcyid, 
                inputAmount, 
                tradeInstrument, 
                nearTransType);
            if (baseSwapQuoteVm == null)
            {
                return null;
            }

            // ON/TN情况下，显示的FP和计算的FP是Bid/Ask反向的
            if (nearLegTenor == TenorEnum.ON || nearLegTenor == TenorEnum.TN)
            {
                decimal calcNearLegForwardPointBid = baseSwapQuoteVm.NearLegFwdPointBid;
                decimal calcNearLegForwardPointAsk = baseSwapQuoteVm.NearLegFwdPointAsk;
                baseSwapQuoteVm.NearLegFwdPointBid = calcNearLegForwardPointAsk;
                baseSwapQuoteVm.NearLegFwdPointAsk = calcNearLegForwardPointBid;
            }

            return baseSwapQuoteVm;
        }

        /// <summary>
        /// 查找符合条件的点差配置
        /// </summary>
        /// <param name="custSpreadConfigList">
        /// 点差配置列表
        /// </param>
        /// <param name="tradePosition">
        /// 交易仓位
        /// </param>
        /// <returns>
        /// 客户点差配置
        /// </returns>
        public QuoteGroupModel.CustomerSpreadConfigModel FindCustSpreadConfig(
            IEnumerable<QuoteGroupModel.CustomerSpreadConfigModel> custSpreadConfigList, 
            decimal tradePosition)
        {
            var rstList = new List<QuoteGroupModel.CustomerSpreadConfigModel>();

            foreach (QuoteGroupModel.CustomerSpreadConfigModel item in custSpreadConfigList)
            {
                decimal upperLimit = item.AmountUpperLimit == null ? decimal.MaxValue : item.AmountUpperLimit.Value;
                if (tradePosition >= item.AmountLowerLimit && tradePosition < upperLimit)
                {
                    rstList.Add(item);
                }
            }

            // 找到符合条件当中AmountUpperLimit最小的那个
            if (rstList.Count >= 1)
            {
                return rstList[0];
            }

            return null;
        }

        /// <summary>
        /// 获取 value date为 tenor 的rate  包含trader spot rate，trader rate，forward point
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="inputCCYID">
        /// 货币ID
        /// </param>
        /// <param name="inputAmount">
        /// 金额
        /// </param>
        /// <param name="bu">
        /// 业务区
        /// </param>
        /// <param name="tenor">
        /// tenor
        /// </param>
        /// <param name="qgse">
        /// 报价组合组
        /// </param>
        /// <param name="rateType">
        /// 点差方式
        /// </param>
        /// <param name="tradaOrder">
        /// The trada Order.
        /// </param>
        /// <returns>
        /// 返回 Forward Curve
        /// </returns>
        public BaseForwardRate GetCustomerRateByTenor(
            SymbolModel symbol, 
            string inputCCYID, 
            decimal inputAmount, 
            BaseBusinessUnitVM bu, 
            TenorEnum tenor, 
            QuoteGroupModel qgse = null, 
            TradeRateTypeEnum rateType = TradeRateTypeEnum.TTRate, 
            CustSpreadTradeOrderEnum tradaOrder = CustSpreadTradeOrderEnum.MarketOrder)
        {
            var forwardRate = new BaseForwardRate();
            int basePoint = symbol.BasisPoint;
            int forwardDecimalPlace = tenor == TenorEnum.SP
                                          ? symbol.DecimalPlace
                                          : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;
            forwardRate = this.GetTraderForwardByTenor(symbol, bu, tenor);

            if (forwardRate != null)
            {
                // 计算TT点差的客户价
                this.CalcSpotTTRate(qgse, forwardRate, tenor, rateType, basePoint, inputCCYID, inputAmount, bu, symbol);
            }

            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            return forwardRate;
        }

        /// <summary>
        /// 获取客户点差配置
        /// </summary>
        /// <param name="quoteGroup">
        /// </param>
        /// <param name="busUnit">
        /// </param>
        /// <param name="symbol">
        /// </param>
        /// <param name="tradeInstrument">
        /// </param>
        /// <param name="inputCCYID">
        /// </param>
        /// <param name="inputAmount">
        /// </param>
        /// <returns>
        /// The <see cref="CustomerSpreadConfigModel"/>.
        /// </returns>
        public QuoteGroupModel.CustomerSpreadConfigModel GetCustomerSpread(
            QuoteGroupModel quoteGroup, 
            BaseBusinessUnitVM busUnit, 
            SymbolModel symbol, 
            TradInstrumentEnum tradeInstrument, 
            string inputCCYID, 
            decimal inputAmount)
        {
            decimal inputPosition = this.GetQuoteDealPosition(inputCCYID, inputAmount, busUnit);

            QuoteGroupModel.QuoteConfigModel fwdSpreadConfigList =
                quoteGroup.QuoteConfigList.FirstOrDefault(o => o.SymbolConfig.SymbolID == symbol.SymbolID);
            if (fwdSpreadConfigList == null)
            {
                return null;
            }

            IEnumerable<QuoteGroupModel.CustomerSpreadConfigModel> customerSpreads =
                fwdSpreadConfigList.CustomerSpreadConfigList.Where(
                    o =>
                    o.TradableInstrument == tradeInstrument
                    && o.TradableOrderType == CustSpreadTradeOrderEnum.MarketOrder);
            QuoteGroupModel.CustomerSpreadConfigModel customerSpread = this.FindCustSpreadConfig(
                customerSpreads, 
                inputPosition);

            return customerSpread;
        }

        /// <summary>
        /// 获取客户点差配置
        /// </summary>
        /// <param name="quoteGroup">
        /// </param>
        /// <param name="localCcyId">
        /// </param>
        /// <param name="symbol">
        /// </param>
        /// <param name="tradeInstrument">
        /// </param>
        /// <param name="orderType">
        /// The order Type.
        /// </param>
        /// <param name="inputCCYID">
        /// </param>
        /// <param name="inputAmount">
        /// </param>
        /// <returns>
        /// The <see cref="QuoteGroupModel.CustomerSpreadConfigModel"/>.
        /// </returns>
        public QuoteGroupModel.CustomerSpreadConfigModel GetCustomerSpread(
            QuoteGroupModel quoteGroup, 
            string localCcyId, 
            SymbolModel symbol, 
            TradInstrumentEnum tradeInstrument, 
            CustSpreadTradeOrderEnum orderType, 
            string inputCCYID, 
            decimal inputAmount)
        {
            decimal inputPosition = this.GetQuoteDealPosition(inputCCYID, inputAmount, localCcyId);

            QuoteGroupModel.QuoteConfigModel fwdSpreadConfigList =
                quoteGroup.QuoteConfigList.FirstOrDefault(o => o.SymbolConfig.SymbolID == symbol.SymbolID);
            if (fwdSpreadConfigList == null)
            {
                return null;
            }

            IEnumerable<QuoteGroupModel.CustomerSpreadConfigModel> customerSpreads =
                fwdSpreadConfigList.CustomerSpreadConfigList.Where(
                    o => o.TradableInstrument == tradeInstrument && o.TradableOrderType == orderType);
            QuoteGroupModel.CustomerSpreadConfigModel customerSpread = this.FindCustSpreadConfig(
                customerSpreads, 
                inputPosition);

            return customerSpread;
        }

        /// <summary>
        /// quote watch 查询的tenor报价列表
        /// </summary>
        /// <param name="weBuyCCY">
        /// </param>
        /// <param name="weSellCCY">
        /// </param>
        /// <param name="inputCCY">
        /// The input CCY.
        /// </param>
        /// <param name="inputAmount">
        /// The input Amount.
        /// </param>
        /// <param name="localTradeDate">
        /// </param>
        /// <param name="bu">
        /// </param>
        /// <param name="quoteGroup">
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<BaseForwardRate> GetCustomerTenorCurve(
            string weBuyCCY, 
            string weSellCCY, 
            string inputCCY, 
            decimal inputAmount, 
            DateTime localTradeDate, 
            BaseBusinessUnitVM bu, 
            QuoteGroupModel quoteGroup)
        {
            var curves = new List<BaseForwardRate>();
            TransactionTypeEnum transType;
            EnumDirection direction;
            var rateType = TradeRateTypeEnum.TTRate;
            BaseCurrencyVM currencyWeBuy = this.GetRepository<ICurrencyCacheRepository>().FindByID(weBuyCCY);
            BaseCurrencyVM currencyWeSell = this.GetRepository<ICurrencyCacheRepository>().FindByID(weSellCCY);
            BaseSymbolVM symbol = this.GetRepository<ISymbolCacheRepository>()
                .GetConvertSymbol(weBuyCCY, weSellCCY, out direction);
            string usdId = this.GetRepository<ICurrencyCacheRepository>().GetIdByName(USD);
            if (symbol == null)
            {
                return null;
            }

            if (direction == EnumDirection.Equals)
            {
                return null;
            }

            if (direction == EnumDirection.Before)
            {
                transType = TransactionTypeEnum.Sell;
            }
            else
            {
                transType = TransactionTypeEnum.Buy;
            }

            int basePoint = symbol.BasisPoint;
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

                // TODO 非常时期加的try catch，数据如果为完整的情况下，不会有null值
                try
                {
                    if ((currencyWeBuy.CurrencyID != usdId
                         && currencyWeBuy.ForwardPointConfigList.First(o => o.Tenor == tenorItem).Status == false)
                        || (currencyWeSell.CurrencyID != usdId
                            && currencyWeSell.ForwardPointConfigList.First(o => o.Tenor == tenorItem).Status == false))
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    TraceManager.Error.Write("GetCustomerTenorCurve", ex);
                    continue;
                }

                var forwardRate = new BaseForwardRate();

                if (tenorItem == TenorEnum.BD)
                {
                    continue;
                }

                string prompt;
                if (tenorItem == TenorEnum.SP)
                {
                    forwardRate.TraderRateBid = tickQuote.TraderSpotBid;
                    forwardRate.TraderRateAsk = tickQuote.TraderSpotAsk;
                    forwardRate.CustomerRateBid = tickQuote.TraderSpotBid;
                    forwardRate.CustomerRateAsk = tickQuote.TraderSpotAsk;
                    forwardRate.Tenor = TenorEnum.SP;
                    forwardRate.SymbolName = symbol.SymbolName;
                    forwardRate.SymbolId = symbol.SymbolID;

                    forwardRate.VauleDate = ValueDateCore.Instance.GetValueDateByTenor(
                        bu.BusinessUnitID, 
                        tenorItem, 
                        symbol.SymbolID, 
                        out prompt);
                    this.CalcSpotTTRate(
                        quoteGroup, 
                        forwardRate, 
                        tenorItem, 
                        rateType, 
                        basePoint, 
                        inputCCY, 
                        inputAmount, 
                        bu, 
                        symbol.SymbolModel);
                    curves.Add(forwardRate);
                    continue;
                }

                if (tenorItem == TenorEnum.TN)
                {
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        continue;
                    }
                }

                DateTime valueDate = ValueDateCore.Instance.GetValueDateByTenor(
                    bu.BusinessUnitID, 
                    tenorItem, 
                    symbol.SymbolID, 
                    out prompt);
                if (valueDate == default(DateTime))
                {
                    continue;
                }

                forwardRate = this.CalcCurrentCustomerRate(
                    symbol.SymbolID, 
                    inputCCY, 
                    inputAmount, 
                    valueDate, 
                    transType, 
                    rateType, 
                    bu, 
                    quoteGroup);
                if (forwardRate != null)
                {
                    curves.Add(forwardRate);
                }
            }

            return curves;
        }

        /// <summary>
        /// 获取 value date为 BrokenDate 的rate  包含trader spot rate，trader rate，forward point
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="brokenDate">
        /// brokenDate
        /// </param>
        /// <param name="inputCCYID">
        /// 货币ID
        /// </param>
        /// <param name="inputAmount">
        /// 金额
        /// </param>
        /// <param name="bu">
        /// 业务区
        /// </param>
        /// <param name="qgse">
        /// 报价组合组对象
        /// </param>
        /// <param name="rateType">
        /// 点差方式
        /// </param>
        /// <param name="tradaOrder">
        /// The trada Order.
        /// </param>
        /// <returns>
        /// 返回 BrokenDate 信息
        /// </returns>
        public BaseForwardRate GetDirectBDCustRate(
            SymbolModel symbol, 
            DateTime brokenDate, 
            string inputCCYID, 
            decimal inputAmount, 
            BaseBusinessUnitVM bu, 
            QuoteGroupModel qgse = null, 
            TradeRateTypeEnum rateType = TradeRateTypeEnum.TTRate, 
            CustSpreadTradeOrderEnum tradaOrder = CustSpreadTradeOrderEnum.MarketOrder)
        {
            var forwardRate = new BaseForwardRate();
            int basePoint = symbol.BasisPoint;
            int forwardDecimalPlace = symbol.DecimalPlace + symbol.ForwardPointDecimalPlace;

            forwardRate = this.GetTraderBrokenDate(symbol, brokenDate, bu);
            if (forwardRate == null)
            {
                return null;
            }

            this.CalcSpotTTRate(
                qgse, 
                forwardRate, 
                TenorEnum.BD, 
                rateType, 
                basePoint, 
                inputCCYID, 
                inputAmount, 
                bu, 
                symbol, 
                tradaOrder);
            forwardRate.CustomerRateBid = forwardRate.CustomerRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.CustomerRateAsk = forwardRate.CustomerRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            return forwardRate;
        }

        /// <summary>
        /// 根据CCy1和ccy2获取报价,并得到商品组成方向
        /// </summary>
        /// <param name="ccy1ID">
        /// ccy1 ID
        /// </param>
        /// <param name="ccy2ID">
        /// ccy2 ID
        /// </param>
        /// <returns>
        /// 返回swap价格
        /// </returns>
        public SwapPrice GetMarketQuote(string ccy1ID, string ccy2ID)
        {
            var dealPrice = new SwapPrice();

            dealPrice.BeforeCCYID = ccy1ID;
            dealPrice.AfterCCYID = ccy2ID;

            // 如果CCYID和CCY2ID相等，则返回报价方向相同
            if (ccy1ID == ccy2ID)
            {
                dealPrice.PriceDirection = EnumDirection.Equals;
                return dealPrice;
            }

            EnumDirection direction;

            // 获取CCY1CCY2组合成的所有商品
            BaseSymbolVM contracts = this.GetRepository<ISymbolCacheRepository>()
                .GetConvertSymbol(ccy1ID, ccy2ID, out direction);
            if (contracts != null)
            {
                dealPrice.SymbolID = contracts.SymbolID;
            }

            dealPrice.PriceDirection = direction;

            // 获取CCY1对应的货币名称
            BaseCurrencyVM ccy1Name =
                this.GetRepository<ICurrencyCacheRepository>().Filter(s => s.CurrencyID == ccy1ID).FirstOrDefault();

            if (ccy1Name == null)
            {
                return null;
            }

            // 获取CCY2对应的货币名称
            BaseCurrencyVM ccy2Name =
                this.GetRepository<ICurrencyCacheRepository>().Filter(s => s.CurrencyID == ccy2ID).FirstOrDefault();

            if (ccy2Name == null)
            {
                return null;
            }

            BaseQuoteVM quotePrice =
                this.GetRepository<IQuoteCacheRepository>()
                    .Filter(o => o.SymbolID == dealPrice.SymbolID)
                    .FirstOrDefault();
            if (quotePrice != null)
            {
                if (quotePrice.PropSet != null)
                {
                    dealPrice.QuotePrice = quotePrice.PropSet;
                }
            }

            return dealPrice;
        }

        /// <summary>
        /// 获取市场报价
        /// </summary>
        /// <param name="ccy1">
        /// </param>
        /// <param name="ccy2">
        /// </param>
        /// <returns>
        /// The <see cref="BaseQuoteVM"/>.
        /// </returns>
        public BaseQuoteVM GetMarketTickQuote(string ccy1, string ccy2)
        {
            EnumDirection direction;
            BaseSymbolVM symbol = this.GetRepository<ISymbolCacheRepository>()
                .GetConvertSymbol(ccy1, ccy2, out direction);
            if (symbol == null)
            {
                return null;
            }

            if (direction == EnumDirection.Equals || direction == EnumDirection.NotExisting)
            {
                return null;
            }

            BaseQuoteVM tickQuote =
                this.GetRepository<IQuoteCacheRepository>().Filter(o => o.SymbolID == symbol.SymbolID).FirstOrDefault();
            return tickQuote;
        }

        /// <summary>
        /// 获取市场报价
        /// </summary>
        /// <param name="symbolId">
        /// The symbol Id.
        /// </param>
        /// <returns>
        /// The <see cref="BaseQuoteVM"/>.
        /// </returns>
        public BaseQuoteVM GetMarketTickQuote(string symbolId)
        {
            BaseQuoteVM tickQuote =
                this.GetRepository<IQuoteCacheRepository>().Filter(o => o.SymbolID == symbolId).FirstOrDefault();
            return tickQuote;
        }

        /// <summary>
        /// 获取报价方块中输入的货币和交易量转换成DealPosition
        /// </summary>
        /// <param name="ccyId">
        /// 输入的货币Id
        /// </param>
        /// <param name="inputAmount">
        /// 输入的交易量
        /// </param>
        /// <param name="localCcy">
        /// 本币
        /// </param>
        /// <returns>
        /// 计算后的DealPosition
        /// </returns>
        public decimal GetQuoteDealPosition(string ccyId, decimal inputAmount, string localCcy)
        {
            decimal dealPosition = decimal.Zero;
            TransPrice ccyLocalPrice = this.GetTransitionPrice(ccyId, localCcy);

            if (ccyLocalPrice.PriceDirection == EnumDirection.NotExisting)
            {
                return dealPosition;
            }

            switch (ccyLocalPrice.PriceDirection)
            {
                case EnumDirection.Equals:
                    dealPosition = inputAmount;
                    break;
                case EnumDirection.Before:
                    dealPosition = inputAmount * ccyLocalPrice.QuotePrice.Mid;
                    break;
                case EnumDirection.After:
                    dealPosition = inputAmount / ccyLocalPrice.QuotePrice.Mid;
                    break;
            }

            return dealPosition;
        }

        /// <summary>
        /// 获取报价方块中输入的货币和交易量转换成DealPosition
        /// </summary>
        /// <param name="ccyId">
        /// 输入的货币Id
        /// </param>
        /// <param name="inputAmount">
        /// 输入的交易量
        /// </param>
        /// <param name="businessUnit">
        /// 业务区实体
        /// </param>
        /// <returns>
        /// 计算后的DealPosition
        /// </returns>
        public decimal GetQuoteDealPosition(string ccyId, decimal inputAmount, BaseBusinessUnitVM businessUnit)
        {
            if (businessUnit == null)
            {
                return decimal.Zero;
            }

            return this.GetQuoteDealPosition(ccyId, inputAmount, businessUnit.LocalCCYID);
        }

        /// <summary>
        /// 获取CCY1和CCY2的转化汇率，CCY2必须要是账户的结算货币或则LocalCurrency
        /// </summary>
        /// <param name="ccy1ID">
        /// 商品的CCY1
        /// </param>
        /// <param name="ccy2ID">
        /// CCY2，账户的结算货币，或则LocalCurrency
        /// </param>
        /// <returns>
        /// 返回自定义的报价对象
        /// </returns>
        public TransPrice GetTransitionPrice(string ccy1ID, string ccy2ID)
        {
            TransPrice propDealPrice = this.GetContractIDByCCYId(ccy1ID, ccy2ID);

            if (propDealPrice.PriceDirection == EnumDirection.Before
                || propDealPrice.PriceDirection == EnumDirection.After)
            {
                // 由于查询的报价分多种报来源，比如报价缓存、订单监控中的报价
                BaseQuoteVM tickQuote = this.GetRepository<IQuoteCacheRepository>().FindByID(propDealPrice.SymbolID);

                // 如果商品存在但没有查询到报价，则强制赋值为报价不存在
                if (tickQuote == null)
                {
                    propDealPrice.PriceDirection = EnumDirection.NotExisting;
                    return propDealPrice;
                }

                propDealPrice.QuotePrice = tickQuote.PropSet;
            }

            return propDealPrice;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 计算Dealer Swap Point
        /// </summary>
        /// <param name="nearLegTenor">
        /// Near Leg Tenor
        /// </param>
        /// <param name="farLegTenor">
        /// Far Leg Tenor
        /// </param>
        /// <param name="nearLegPointBid">
        /// Near Leg forward point bid
        /// </param>
        /// <param name="nearLegPointAsk">
        /// Near Leg forward point ask
        /// </param>
        /// <param name="farLegPointBid">
        /// Far Leg forward point bid
        /// </param>
        /// <param name="farLegPointAsk">
        /// Far Leg forward point ask
        /// </param>
        /// <param name="buysellSwapPoints">
        /// Dealer Buy-Sell swap point
        /// </param>
        /// <param name="sellbuySwapPoints">
        /// Dealer Sell-Buy swap point
        /// </param>
        private void CalSwapPoints(
            TenorEnum nearLegTenor, 
            TenorEnum farLegTenor, 
            decimal nearLegPointBid, 
            decimal nearLegPointAsk, 
            decimal farLegPointBid, 
            decimal farLegPointAsk, 
            out decimal buysellSwapPoints, 
            out decimal sellbuySwapPoints)
        {
            // 两条腿均晚于spot的forward：
            // 客户 Buy-Sell：Far Leg Forward Point Bid - Near Leg Forward Point Ask 
            // 客户 Sell-Buy : Far Leg forward Point Ask - Near Leg Forward Point Aid
            // Near Leg Spot : 
            // 客户 Buy-Sell：Far Leg Forward Point Bid
            // 客户 Sell-Buy :  Far Leg Forward Point Ask
            // Near Leg value Tom, Far Leg spot or later:    
            // 客户 Buy-Sell：Far Leg Forward Point Bid + TN Forward Point Bid
            // 客户 Sell-Buy : Far Leg Forward Point Ask + TN Forward Point Ask
            // Near Leg value today, Far Leg spot or bigger:    
            // 客户 Buy-Sell：Far Leg Forward Point Bid + TN Forward Point Bid + ON Forward Point Bid
            // 客户 Sell-Buy : Far Leg Forward Point Ask + TN Forward Point Ask + ON Forward Point Ask
            // Near Leg value today, Far Leg value Tom:    
            // 客户 Buy-Sell：ON Forward Point Ask
            // 客户 Sell-Buy :  ON Forward Point Bid
            buysellSwapPoints = decimal.Zero;
            sellbuySwapPoints = decimal.Zero;

            if (nearLegTenor > TenorEnum.SP && farLegTenor > TenorEnum.SP)
            {
                buysellSwapPoints = farLegPointBid - nearLegPointAsk;
                sellbuySwapPoints = farLegPointAsk - nearLegPointBid;
            }
            else if (nearLegTenor == TenorEnum.SP)
            {
                buysellSwapPoints = farLegPointBid;
                sellbuySwapPoints = farLegPointAsk;
            }
            else if (nearLegTenor == TenorEnum.TN && farLegTenor >= TenorEnum.SP)
            {
                buysellSwapPoints = farLegPointBid + nearLegPointBid;
                sellbuySwapPoints = farLegPointAsk + nearLegPointAsk;
            }
            else if (nearLegTenor == TenorEnum.ON && farLegTenor >= TenorEnum.SP)
            {
                buysellSwapPoints = farLegPointBid + nearLegPointBid;
                sellbuySwapPoints = farLegPointAsk + nearLegPointAsk;
            }
            else if (nearLegTenor == TenorEnum.ON && farLegTenor == TenorEnum.TN)
            {
                buysellSwapPoints = nearLegPointAsk - farLegPointAsk;
                sellbuySwapPoints = nearLegPointBid - farLegPointBid;
            }
        }

        /// <summary>
        /// 计算sopt的tt rate
        /// </summary>
        /// <param name="qgse">
        /// 报价组合组对象
        /// </param>
        /// <param name="forwardRate">
        /// 远期rate
        /// </param>
        /// <param name="tenor">
        /// 远期类型
        /// </param>
        /// <param name="rateType">
        /// 点差类型
        /// </param>
        /// <param name="basePoint">
        /// 基础点差
        /// </param>
        /// <param name="inputCCYID">
        /// 货币ID
        /// </param>
        /// <param name="inputAmount">
        /// 金额
        /// </param>
        /// <param name="busiUnit">
        /// 业务区
        /// </param>
        /// <param name="symbolModel">
        /// The symbol Model.
        /// </param>
        /// <param name="tradaOrder">
        /// The trada Order.
        /// </param>
        private void CalcSpotTTRate(
            QuoteGroupModel qgse, 
            BaseForwardRate forwardRate, 
            TenorEnum tenor, 
            TradeRateTypeEnum rateType, 
            int basePoint, 
            string inputCCYID, 
            decimal inputAmount, 
            BaseBusinessUnitVM busiUnit, 
            SymbolModel symbolModel, 
            CustSpreadTradeOrderEnum tradaOrder = CustSpreadTradeOrderEnum.MarketOrder)
        {
            if (qgse == null)
            {
                return;
            }

            int forwardDecimalPlace = tenor == TenorEnum.SP
                                          ? symbolModel.DecimalPlace
                                          : symbolModel.BasisPoint + symbolModel.ForwardPointDecimalPlace;
            QuoteGroupModel.QuoteConfigModel symbolSpreads =
                qgse.QuoteConfigList.FirstOrDefault(o => o.SymbolConfig.SymbolID == symbolModel.SymbolID);
            if (symbolSpreads != null)
            {
                decimal inputPosition = this.GetQuoteDealPosition(inputCCYID, inputAmount, busiUnit);
                IEnumerable<QuoteGroupModel.CustomerSpreadConfigModel> customerSpreads =
                    symbolSpreads.CustomerSpreadConfigList.Where(
                        o =>
                        o.TradableInstrument.Equals(
                            tenor.GenTradInstrumentEnumByTenor(symbolModel.ValueDate, symbolModel.FXSpotConfiguration))
                        && o.TradableOrderType.Equals(tradaOrder));
                QuoteGroupModel.CustomerSpreadConfigModel customerSpread = this.FindCustSpreadConfig(
                    customerSpreads, 
                    inputPosition);

                if (customerSpread != null)
                {
                    if (rateType == TradeRateTypeEnum.TTRate)
                    {
                        forwardRate.CustomerRateBid =
                            (forwardRate.TraderRateBid - customerSpread.TTBid.SpreadToDecimal(basePoint))
                                .FormatPriceBySymbolPoint(forwardDecimalPlace);
                        forwardRate.CustomerRateAsk =
                            (forwardRate.TraderRateAsk + customerSpread.TTAsk.SpreadToDecimal(basePoint))
                                .FormatPriceBySymbolPoint(forwardDecimalPlace);
                    }
                    else
                    {
                        forwardRate.CustomerRateBid =
                            (forwardRate.TraderRateBid - customerSpread.CashBid.SpreadToDecimal(basePoint))
                                .FormatPriceBySymbolPoint(forwardDecimalPlace);
                        forwardRate.CustomerRateAsk =
                            (forwardRate.TraderRateAsk + customerSpread.CashAsk.SpreadToDecimal(basePoint))
                                .FormatPriceBySymbolPoint(forwardDecimalPlace);
                    }
                }
                else
                {
                    forwardRate.CustomerRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(
                        forwardDecimalPlace);
                    forwardRate.CustomerRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(
                        forwardDecimalPlace);
                }
            }
        }

        /// <summary>
        /// 计算Swap的客户价格
        /// </summary>
        /// <param name="symbol">
        /// 货币对
        /// </param>
        /// <param name="nearLegValueDate">
        /// Near Leg 交割日
        /// </param>
        /// <param name="nearLegTenor">
        /// Near Leg Tenor
        /// </param>
        /// <param name="farLegValueDate">
        /// Far Leg 交割日
        /// </param>
        /// <param name="farLegTenor">
        /// Far Leg Tenor
        /// </param>
        /// <param name="quoteGroup">
        /// 报价组
        /// </param>
        /// <param name="businessUnit">
        /// 业务区
        /// </param>
        /// <param name="inputCcyid">
        /// 输入Currency id
        /// </param>
        /// <param name="inputAmount">
        /// 输入量
        /// </param>
        /// <param name="tradeInstrument">
        /// 交易方式
        /// </param>
        /// <param name="nearTransType">
        /// Near Leg交易方向
        /// </param>
        /// <returns>
        /// 价格信息
        /// </returns>
        private BaseSwapQuoteVM CalcSwapCustomerRate(
            BaseSymbolVM symbol, 
            DateTime nearLegValueDate, 
            TenorEnum nearLegTenor, 
            DateTime farLegValueDate, 
            TenorEnum farLegTenor, 
            QuoteGroupModel quoteGroup, 
            BaseBusinessUnitVM businessUnit, 
            string inputCcyid, 
            decimal inputAmount, 
            TradInstrumentEnum tradeInstrument, 
            TransactionTypeEnum nearTransType)
        {
            if (symbol == null || nearLegValueDate == default(DateTime) || farLegValueDate == default(DateTime)
                || quoteGroup == null || businessUnit == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapCustomerRate", "Input parameter error");
                return null;
            }

            string rstPrompt;
            TenorEnum nearLegVerifiedTenor = ValueDateCore.Instance.GetTenorByValueDate(
                businessUnit.BusinessUnitID, 
                nearLegValueDate, 
                symbol.SymbolID, 
                out rstPrompt);
            if (rstPrompt != Common.Success || nearLegTenor != nearLegVerifiedTenor)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapCustomerRate", "Near leg ValueDate and Tenor are not match");
                return null;
            }

            TenorEnum farLegVerifiedTenor = ValueDateCore.Instance.GetTenorByValueDate(
                businessUnit.BusinessUnitID, 
                farLegValueDate, 
                symbol.SymbolID, 
                out rstPrompt);
            if (rstPrompt != Common.Success || farLegTenor != farLegVerifiedTenor)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapCustomerRate", "Far leg ValueDate and Tenor are not match");
                return null;
            }

            // 计算Swap Dealer Rate
            BaseSwapQuoteVM baseSwapQuoteVm = this.CalcSwapDealerRate(
                symbol, 
                nearLegValueDate, 
                nearLegTenor, 
                farLegValueDate, 
                farLegTenor, 
                businessUnit, 
                nearTransType);

            // 计算Dealer相关的价格有误
            if (baseSwapQuoteVm == null)
            {
                return null;
            }

            // 计算CustomerSpread
            QuoteGroupModel.CustomerSpreadConfigModel customerSpread = this.GetCustomerSpread(
                quoteGroup, 
                businessUnit, 
                symbol.SymbolModel, 
                tradeInstrument, 
                inputCcyid, 
                inputAmount);
            if (customerSpread == null)
            {
                TraceManager.Warn.Write("PriceCore.CalSwapCustomerRate", "Cant find customer spread.");
                return null;
            }

            // 计算CustomerSwapPoint
            // 客户Buy-Sell情况： ( Dealer Swap Point - Cust. Bid Spread - Cust. Ask Spread )
            // 客户Sell-Buy情况： ( Dealer Swap Point + Cust. Bid Spread + Cust. Ask Spread )
            baseSwapQuoteVm.BuySellSwapPoints =
                (baseSwapQuoteVm.BuySellTraderSwapPoints - (customerSpread.TTAsk + customerSpread.TTBid))
                    .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
            baseSwapQuoteVm.SellBuySwapPoints =
                (baseSwapQuoteVm.SellBuyTraderSwapPoints + (customerSpread.TTAsk + customerSpread.TTBid))
                    .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
            int nearLegPoint = nearLegTenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;
            int farLegPoint = farLegTenor == TenorEnum.SP
                                  ? symbol.DecimalPlace
                                  : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

            // 计算CustomerRate
            // 客户 Buy-Sell ：
            // Cust. Near Leg : Trader Dealer Near Leg Rate + Cust. Spread Ask
            // Cust. Far Leg: Trader Dealer Far Leg Rate - Cust. Spread Bid
            // 客户 Sell-Buy ：
            // Cust. Near Leg : Trader Near Leg Dealer Rate - Cust. Spread Bid
            // Cust. Far Leg: Trader Far Leg Dealer Rate + Cust. Spread Ask
            baseSwapQuoteVm.BuySellNearLegRate =
                (baseSwapQuoteVm.BuySellTraderNearLegRate + customerSpread.TTAsk.SpreadToDecimal(symbol.BasisPoint))
                    .FormatPriceBySymbolPoint(nearLegPoint);
            baseSwapQuoteVm.BuySellFarLegRate =
                (baseSwapQuoteVm.BuySellTraderFarLegRate - customerSpread.TTBid.SpreadToDecimal(symbol.BasisPoint))
                    .FormatPriceBySymbolPoint(farLegPoint);
            baseSwapQuoteVm.SellBuyNearLegRate =
                (baseSwapQuoteVm.SellBuyTraderNearLegRate - customerSpread.TTBid.SpreadToDecimal(symbol.BasisPoint))
                    .FormatPriceBySymbolPoint(nearLegPoint);
            baseSwapQuoteVm.SellBuyFarLegRate =
                (baseSwapQuoteVm.SellBuyTraderFarLegRate + customerSpread.TTAsk.SpreadToDecimal(symbol.BasisPoint))
                    .FormatPriceBySymbolPoint(farLegPoint);
            return baseSwapQuoteVm;
        }

        /// <summary>
        /// 计算Swap的交易员价格
        /// </summary>
        /// <param name="symbol">
        /// 货币对
        /// </param>
        /// <param name="nearLegValueDate">
        /// Near Leg 交割日
        /// </param>
        /// <param name="nearLegTenor">
        /// Near Leg Tenor
        /// </param>
        /// <param name="farLegValueDate">
        /// Far Leg 交割日
        /// </param>
        /// <param name="farLegTenor">
        /// Far Leg Tenor
        /// </param>
        /// <param name="businessUnit">
        /// 业务区
        /// </param>
        /// <param name="nearTransType">
        /// Near Leg交易方向
        /// </param>
        /// <returns>
        /// 价格信息
        /// </returns>
        private BaseSwapQuoteVM CalcSwapDealerRate(
            BaseSymbolVM symbol, 
            DateTime nearLegValueDate, 
            TenorEnum nearLegTenor, 
            DateTime farLegValueDate, 
            TenorEnum farLegTenor, 
            BaseBusinessUnitVM businessUnit, 
            TransactionTypeEnum nearTransType)
        {
            if (symbol == null || nearLegValueDate == default(DateTime) || farLegValueDate == default(DateTime)
                || businessUnit == null)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapDealerRate", "Input parameter error");
                return null;
            }

            string rstPrompt;
            TenorEnum nearLegVerifiedTenor = ValueDateCore.Instance.GetTenorByValueDate(
                businessUnit.BusinessUnitID, 
                nearLegValueDate, 
                symbol.SymbolID, 
                out rstPrompt);
            if (rstPrompt != Common.Success || nearLegTenor != nearLegVerifiedTenor)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapDealerRate", "Near leg ValueDate and Tenor are not match");
                return null;
            }

            TenorEnum farLegVerifiedTenor = ValueDateCore.Instance.GetTenorByValueDate(
                businessUnit.BusinessUnitID, 
                farLegValueDate, 
                symbol.SymbolID, 
                out rstPrompt);
            if (rstPrompt != Common.Success || farLegTenor != farLegVerifiedTenor)
            {
                TraceManager.Warn.Write("PriceCore.CalcSwapDealerRate", "Far leg ValueDate and Tenor are not match");
                return null;
            }

            BaseSwapQuoteVM baseSwapQuoteVm;
            string usdCcyId = this.GetRepository<ICurrencyCacheRepository>().GetUsdCurrencyId();
            if (symbol.CCY1 == usdCcyId || symbol.CCY2 == usdCcyId)
            {
                // 直盘
                baseSwapQuoteVm = this.CalcDirectSwapDealerRate(
                    symbol, 
                    nearLegValueDate, 
                    nearLegTenor, 
                    farLegValueDate, 
                    farLegTenor, 
                    businessUnit, 
                    nearTransType);
            }
            else
            {
                // 交叉盘
                baseSwapQuoteVm = this.CalcCrossSwapDealerRate(
                    symbol, 
                    nearLegValueDate, 
                    nearLegTenor, 
                    farLegValueDate, 
                    farLegTenor, 
                    businessUnit, 
                    nearTransType);
            }

            return baseSwapQuoteVm;
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
        private ForwardPointTenorModel GetBrokenDateForwPoint(
            DateTime brokenValueDate, 
            CurrencyModel usdCCY, 
            SymbolModel symbol, 
            string busUnitID)
        {
            var intStartTenor = (int)TenorEnum.SN;
            var intEndTenor = (int)TenorEnum.Y1;

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
        /// 获取tenor对应的forward
        /// </summary>
        /// <param name="symbol">
        /// </param>
        /// <param name="tenor1">
        /// </param>
        /// <returns>
        /// The <see cref="BaseForwardRate"/>.
        /// </returns>
        private BaseForwardRate GetBrokenDateForwardPoint(SymbolModel symbol, TenorEnum tenor1)
        {
            // 直盘
            if (symbol.SymbolName.Contains(USD))
            {
                BaseForwardRate directRate = this.GetTenorDirectRate(symbol, tenor1);
                return directRate;
            }

            return this.GetTenorCrossRate(symbol, tenor1);
        }

        /// <summary>
        /// 根据货币ID，获取货币对应的商品
        /// </summary>
        /// <param name="ccy1Id">
        /// 货币1ID
        /// </param>
        /// <param name="ccy2Id">
        /// 货币2ID
        /// </param>
        /// <returns>
        /// 订单价格
        /// </returns>
        private TransPrice GetContractIDByCCYId(string ccy1Id, string ccy2Id)
        {
            var dealPrice = new TransPrice();
            if (ccy1Id == ccy2Id)
            {
                dealPrice.PriceDirection = EnumDirection.Equals;
                return dealPrice;
            }

            EnumDirection direction;

            // 获取CCY1CCY2组合成的所有商品
            BaseSymbolVM contracts = this.GetRepository<ISymbolCacheRepository>()
                .GetConvertSymbol(ccy1Id, ccy2Id, out direction);
            if (contracts != null)
            {
                dealPrice.SymbolID = contracts.SymbolID;
            }

            dealPrice.PriceDirection = direction;

            return dealPrice;
        }

        /// <summary>
        /// 获取 value date为 tenor 的rate  包含trader spot rate，trader rate，forward point
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="bu">
        /// 业务区
        /// </param>
        /// <param name="tenor">
        /// tenor
        /// </param>
        /// <returns>
        /// 返回 Forward Curve
        /// </returns>
        private BaseForwardRate GetSwapTraderRateByTenor(SymbolModel symbol, BaseBusinessUnitVM bu, TenorEnum tenor)
        {
            string ccy1Id = symbol.CCY1;
            string ccy2Id = symbol.CCY2;
            var forwardRate = new BaseForwardRate();
            List<ForwardPointModel> forwardPoints = this.GetRepository<IForwardPointCacheRepository>().GetForwardPoint();
            SwapPrice swapPrice = this.GetMarketQuote(ccy1Id, ccy2Id);
            if (swapPrice == null || swapPrice.QuotePrice == null || swapPrice.PriceDirection == EnumDirection.Equals
                || swapPrice.PriceDirection == EnumDirection.NotExisting)
            {
                return null;
            }

            if (tenor == TenorEnum.BD)
            {
                return null;
            }

            string prompt;
            forwardRate.VauleDate = ValueDateCore.Instance.GetValueDateByTenor(
                bu.BusinessUnitID, 
                tenor, 
                symbol.SymbolID, 
                out prompt);
            forwardRate.SymbolId = symbol.SymbolID;
            forwardRate.SymbolName = symbol.SymbolName;
            forwardRate.IsBrokenDate = false;
            forwardRate.Tenor = tenor;
            forwardRate.TraderSpotRateBid = swapPrice.QuotePrice.TraderBid;
            forwardRate.TraderSpotRateAsk = swapPrice.QuotePrice.TraderAsk;

            if (tenor == TenorEnum.SP)
            {
                forwardRate.TraderRateBid = swapPrice.QuotePrice.TraderBid;
                forwardRate.TraderRateAsk = swapPrice.QuotePrice.TraderAsk;
            }
            else
            {
                var costForwardRate = new BaseForwardRate();

                // 直盘
                if (swapPrice.QuotePrice.SymbolName.Contains(USD))
                {
                    costForwardRate = this.GetTenorDirectRate(symbol, tenor);
                }
                else
                {
                    costForwardRate = this.GetTenorCrossRate(symbol, tenor);
                }

                if (costForwardRate == null)
                {
                    return null;
                }

                forwardRate.TraderRateAsk = costForwardRate.TraderRateAsk;
                forwardRate.TraderRateBid = costForwardRate.TraderRateBid;
                forwardRate.ForwardPointAsk = costForwardRate.ForwardPointAsk;
                forwardRate.ForwardPointBid = costForwardRate.ForwardPointBid;
            }

            forwardRate.TraderSpotRateBid = swapPrice.QuotePrice.TraderBid;
            forwardRate.TraderSpotRateAsk = swapPrice.QuotePrice.TraderAsk;

            return forwardRate;
        }

        /// <summary>
        /// 获取交叉盘 forward rate
        /// </summary>
        /// <param name="symbol">
        /// 交易的商品
        /// </param>
        /// <param name="tenor">
        /// tenor
        /// </param>
        /// <returns>
        /// 返回 forward rate
        /// </returns>
        private BaseForwardRate GetTenorCrossRate(SymbolModel symbol, TenorEnum tenor)
        {
            string ccy1Id = symbol.CCY1;
            string ccy2Id = symbol.CCY2;
            var forwardPoint = new ForwardPointModel();
            var forwardRate = new BaseForwardRate();
            int decimalPlace = tenor == TenorEnum.SP
                                   ? symbol.DecimalPlace
                                   : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;
            List<ForwardPointModel> forwardPoints = this.GetForwardPoints();
            if (tenor == TenorEnum.SP)
            {
                SwapPrice spotRate = this.GetMarketQuote(ccy1Id, ccy2Id);
                forwardRate.SymbolId = symbol.SymbolID;
                forwardRate.TraderRateBid = spotRate.QuotePrice.TraderBid;
                forwardRate.TraderRateAsk = spotRate.QuotePrice.TraderAsk;
                forwardRate.ForwardPointBid = decimal.Zero;
                forwardRate.ForwardPointAsk = decimal.Zero;
            }

            var symbolRep = this.GetRepository<ISymbolCacheRepository>();
            var currencyRep = this.GetRepository<ICurrencyCacheRepository>();
            string usdId = currencyRep.GetUsdCurrencyId();

            // 1、ccy1Id+USD远期报价
            SwapPrice ccy1DirectSpot = this.GetMarketQuote(usdId, ccy1Id);
            if (ccy1DirectSpot.PriceDirection == EnumDirection.NotExisting
                || ccy1DirectSpot.PriceDirection == EnumDirection.Equals)
            {
                return null;
            }

            BaseSymbolVM ccy1DirectSymbol = symbolRep.GetSymbol(usdId, ccy1Id);
            BaseForwardRate ccy1DirectRate = this.GetTenorDirectRate(ccy1DirectSymbol.SymbolModel, tenor);

            // 2、ccy2Id+USD远期报价
            SwapPrice ccy2DirectSpot = this.GetMarketQuote(usdId, ccy2Id);
            if (ccy2DirectSpot.PriceDirection == EnumDirection.NotExisting
                || ccy2DirectSpot.PriceDirection == EnumDirection.Equals)
            {
                return null;
            }

            BaseSymbolVM ccy2DirectSymbol = symbolRep.GetSymbol(usdId, ccy2Id);
            BaseForwardRate ccy2DirectRate = this.GetTenorDirectRate(ccy2DirectSymbol.SymbolModel, tenor);

            // 3、计算两个直盘报价的交叉盘报价
            if (ccy1DirectSymbol.SymbolName == null || ccy2DirectSymbol.SymbolName == null)
            {
                return null;
            }

            // 间接报价的次数
            int indirect = 0;
            if (ccy1DirectSymbol.CCY2 == usdId)
            {
                indirect++;
            }

            if (ccy2DirectSymbol.CCY2 == usdId)
            {
                indirect++;
            }

            // 第二步：分别计算交叉盘远期的报价
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

            SwapPrice spot = this.GetMarketQuote(ccy1Id, ccy2Id);
            if (spot.PriceDirection == EnumDirection.NotExisting || spot.PriceDirection == EnumDirection.Equals)
            {
                return null;
            }

            switch (tenor)
            {
                case TenorEnum.ON:
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        forwardRate.ForwardPointBid =
                            (spot.QuotePrice.TraderAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                        forwardRate.ForwardPointAsk =
                            (spot.QuotePrice.TraderBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    }
                    else
                    {
                        BaseForwardRate tnRate = this.GetTenorCrossRate(symbol, TenorEnum.TN);

                        forwardRate.ForwardPointBid =
                            (tnRate.TraderRateAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                        forwardRate.ForwardPointAsk =
                            (tnRate.TraderRateBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                                .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    }

                    break;
                case TenorEnum.TN:

                    forwardRate.ForwardPointBid =
                        (spot.QuotePrice.TraderAsk - forwardRate.TraderRateAsk).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    forwardRate.ForwardPointAsk =
                        (spot.QuotePrice.TraderBid - forwardRate.TraderRateBid).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    break;
                case TenorEnum.SP:
                    forwardRate.ForwardPointBid = decimal.Zero;
                    forwardRate.ForwardPointAsk = decimal.Zero;
                    break;
                default:
                    forwardRate.ForwardPointBid =
                        (forwardRate.TraderRateBid - spot.QuotePrice.TraderBid).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    forwardRate.ForwardPointAsk =
                        (forwardRate.TraderRateAsk - spot.QuotePrice.TraderAsk).ToSpread(symbol.BasisPoint)
                            .FormatPriceBySymbolPoint(symbol.ForwardPointDecimalPlace);
                    break;
            }

            return forwardRate;
        }

        /// <summary>
        /// 获取直盘Teonr forward rate
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="tenor">
        /// tenor
        /// </param>
        /// <returns>
        /// 返回Forward Rate
        /// </returns>
        private BaseForwardRate GetTenorDirectRate(SymbolModel symbol, TenorEnum tenor)
        {
            var forwardPoint = new ForwardPointModel();
            var forwardRate = new BaseForwardRate();
            var spot = new TickQuoteModel();
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

            spot = marketSpot.QuotePrice;

            if (cur1 == USD)
            {
                forwardPoint = forwardPoints.FirstOrDefault(o => o.CurrencyName == cur2);
            }
            else
            {
                forwardPoint = forwardPoints.FirstOrDefault(o => o.CurrencyName == cur1);
            }

            if (forwardPoint == null)
            {
                return null;
            }

            ForwardPointTenorModel forwardPointTenor = forwardPoint.Tenors.FirstOrDefault(o => o.Tenor == tenor);
            if (forwardPointTenor == null)
            {
                return null;
            }

            forwardRate.SymbolId = marketSpot.SymbolID;

            // forwardRate.SymbolName = marketSpot.SymbolName;
            forwardRate.ForwardPointBid = forwardPointTenor.Bid;
            forwardRate.ForwardPointAsk = forwardPointTenor.Ask;
            if (tenor == TenorEnum.SP)
            {
                forwardRate.ForwardPointBid = decimal.Zero;
                forwardRate.ForwardPointAsk = decimal.Zero;
                forwardRate.TraderRateBid = spot.TraderBid;
                forwardRate.TraderRateAsk = spot.TraderAsk;
                return forwardRate;
            }

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
                    forwardRate.ForwardPointBid = forwardPointTenor.Bid;
                    forwardRate.ForwardPointAsk = forwardPointTenor.Ask;
                }

                // 格式化远期报价
                forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
                return forwardRate;
            }

            if (tenor == TenorEnum.TN)
            {
                forwardRate.TraderRateBid = spot.TraderBid - forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);
                forwardRate.TraderRateAsk = spot.TraderAsk - forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);

                forwardRate.ForwardPointBid = forwardPointTenor.Bid;
                forwardRate.ForwardPointAsk = forwardPointTenor.Ask;

                // 格式化远期报价
                forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
                forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
                return forwardRate;
            }

            forwardRate.TraderRateBid = spot.TraderBid + forwardPointTenor.Bid.SpreadToDecimal(symbol.BasisPoint);
            forwardRate.TraderRateAsk = spot.TraderAsk + forwardPointTenor.Ask.SpreadToDecimal(symbol.BasisPoint);

            // 格式化远期报价
            forwardRate.TraderRateBid = forwardRate.TraderRateBid.FormatPriceBySymbolPoint(forwardDecimalPlace);
            forwardRate.TraderRateAsk = forwardRate.TraderRateAsk.FormatPriceBySymbolPoint(forwardDecimalPlace);
            return forwardRate;
        }

        /// <summary>
        /// 获取 value date为 BrokenDate 的rate  包含trader spot rate，trader rate，forward point
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
        private BaseForwardRate GetTraderBrokenDate(SymbolModel symbol, DateTime brokenDate, BaseBusinessUnitVM bu)
        {
            var forwardRate = new BaseForwardRate();
            string ccy1Id = symbol.CCY1;
            string ccy2Id = symbol.CCY2;
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

            ForwardPointTenorModel forwardPoint = this.GetBrokenDateForwPoint(
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
            forwardRate.IsBrokenDate = true;
            forwardRate.VauleDate = brokenDate;
            return forwardRate;
        }

        /// <summary>
        /// 获取 value date为 tenor 的rate  包含trader spot rate，trader rate，forward point
        /// </summary>
        /// <param name="symbol">
        /// The symbol.
        /// </param>
        /// <param name="bu">
        /// 业务区
        /// </param>
        /// <param name="tenor">
        /// tenor
        /// </param>
        /// <returns>
        /// 返回 Forward Curve
        /// </returns>
        private BaseForwardRate GetTraderForwardByTenor(SymbolModel symbol, BaseBusinessUnitVM bu, TenorEnum tenor)
        {
            string ccy1Id = symbol.CCY1;
            string ccy2Id = symbol.CCY2;
            var forwardRate = new BaseForwardRate();
            SwapPrice swapPrice = this.GetMarketQuote(ccy1Id, ccy2Id);
            if (swapPrice == null || swapPrice.QuotePrice == null || swapPrice.PriceDirection == EnumDirection.Equals
                || swapPrice.PriceDirection == EnumDirection.NotExisting)
            {
                return null;
            }

            if (tenor == TenorEnum.BD)
            {
                return null;
            }

            string prompt;
            forwardRate.VauleDate = ValueDateCore.Instance.GetValueDateByTenor(
                bu.BusinessUnitID, 
                tenor, 
                symbol.SymbolID, 
                out prompt);
            forwardRate.SymbolId = symbol.SymbolID;
            forwardRate.SymbolName = symbol.SymbolName;
            forwardRate.IsBrokenDate = false;
            forwardRate.Tenor = tenor;
            forwardRate.TraderSpotRateBid = swapPrice.QuotePrice.TraderBid;
            forwardRate.TraderSpotRateAsk = swapPrice.QuotePrice.TraderAsk;

            if (tenor == TenorEnum.SP)
            {
                forwardRate.TraderRateBid = swapPrice.QuotePrice.TraderBid;
                forwardRate.TraderRateAsk = swapPrice.QuotePrice.TraderAsk;
            }
            else
            {
                var costForwardRate = new BaseForwardRate();

                // 直盘
                if (swapPrice.QuotePrice.SymbolName.Contains(USD))
                {
                    costForwardRate = this.GetTenorDirectRate(symbol, tenor);

                    // 如果是ON的情况远期点数应该加上TN的
                    if (symbol.ValueDate == ValueDateEnum.T2 && tenor == TenorEnum.ON)
                    {
                        BaseForwardRate tnRate = this.GetTenorDirectRate(symbol, TenorEnum.TN);
                        costForwardRate.ForwardPointBid += tnRate.ForwardPointBid;
                        costForwardRate.ForwardPointAsk += tnRate.ForwardPointAsk;
                    }
                }
                else
                {
                    costForwardRate = this.GetTenorCrossRate(symbol, tenor);

                    // 如果是ON的情况远期点数应该加上TN的
                    if (symbol.ValueDate == ValueDateEnum.T2 && tenor == TenorEnum.ON)
                    {
                        BaseForwardRate tnRate = this.GetTenorCrossRate(symbol, TenorEnum.TN);
                        costForwardRate.ForwardPointBid += tnRate.ForwardPointBid;
                        costForwardRate.ForwardPointAsk += tnRate.ForwardPointAsk;
                    }
                }

                if (costForwardRate == null)
                {
                    return null;
                }

                forwardRate.TraderRateAsk = costForwardRate.TraderRateAsk;
                forwardRate.TraderRateBid = costForwardRate.TraderRateBid;
                forwardRate.ForwardPointAsk = costForwardRate.ForwardPointAsk;
                forwardRate.ForwardPointBid = costForwardRate.ForwardPointBid;
            }

            forwardRate.TraderSpotRateBid = swapPrice.QuotePrice.TraderBid;
            forwardRate.TraderSpotRateAsk = swapPrice.QuotePrice.TraderAsk;

            return forwardRate;
        }

        #endregion

        /// <summary>
        ///     组合报价
        /// </summary>
        public class SwapPrice
        {
            #region Public Properties

            /// <summary>
            ///     该转换报价中，位于后面的货币ID
            /// </summary>
            /// <code>public string AfterCCYID { get; set; }</code>
            public string AfterCCYID { get; set; }

            /// <summary>
            ///     该转换报价中，位于前面的货币ID
            /// </summary>
            /// <code>public string BeforeCCYID { get; set; }</code>
            public string BeforeCCYID { get; set; }

            /// <summary>
            ///     报价的方向
            /// </summary>
            /// <code>public EnumDirection PriceDirection { get; set; }</code>
            public EnumDirection PriceDirection { get; set; }

            /// <summary>
            ///     当前报价列表中的报价信息
            /// </summary>
            /// <code>public EntityTickQuote QuotePrice { get; set; }</code>
            public TickQuoteModel QuotePrice { get; set; }

            /// <summary>
            ///     当前报价的商品ID
            /// </summary>
            /// <code>public string SymbolID { get; set; }</code>
            public string SymbolID { get; set; }

            #endregion
        }
    }
}