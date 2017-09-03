// <copyright file="CalculateCore.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/07/01 03:07:41 </date>
// <summary> 原CommonCalculate代码整理优化类 </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/07/01 03:07:41
//      修改描述：新建 CalculateCore.cs
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
    ///     The calculate core.
    /// </summary>
    public class CalculateCore : BaseVm
    {
        #region Static Fields

        /// <summary>
        ///     静态只读唯一实例
        /// </summary>
        private static readonly CalculateCore StaticInstance = new CalculateCore();

        #endregion

        #region Fields

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyCacheRepository currencyRep;

        /// <summary>
        ///     客户仓储
        /// </summary>
        private readonly ICustomerCacheRepository customerRep;

        /// <summary>
        ///     报价仓储
        /// </summary>
        private readonly IQuoteCacheRepository quoteCacheRepository;

        /// <summary>
        ///     货币对仓储
        /// </summary>
        private readonly ISymbolCacheRepository symbolRep;

        /// <summary>
        ///     VIP客户配置项仓储
        /// </summary>
        private readonly IVIPCustSettingCacheRepository vipRep;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="CalculateCore" /> class from being created.
        /// </summary>
        private CalculateCore()
        {
            this.currencyRep = this.GetRepository<ICurrencyCacheRepository>();
            this.symbolRep = this.GetRepository<ISymbolCacheRepository>();
            this.quoteCacheRepository = this.GetRepository<IQuoteCacheRepository>();
            this.vipRep = this.GetRepository<IVIPCustSettingCacheRepository>();
            this.customerRep = this.GetRepository<ICustomerCacheRepository>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     单例获取
        /// </summary>
        public static CalculateCore Instance
        {
            get
            {
                return StaticInstance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 计算利润
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="customerSellCcy">
        /// 客户卖货币
        /// </param>
        /// <param name="localCcy">
        /// 本币
        /// </param>
        /// <param name="dealerRate">
        /// TraderRateAsk: Customer Buy CCY to Customer Sell CCY
        ///     TraderRateBid: Customer Sell CCY to Customer Buy CCY
        /// </param>
        /// <param name="customerBuyAmount">
        /// 客户买数量
        /// </param>
        /// <param name="customerSellAmount">
        /// 客户卖数量
        /// </param>
        /// <returns>
        /// 计算的Profit值（0=当前利润，1=LocalCCY利润，2=USD利润）
        /// </returns>
        public decimal[] CalculateAllProfits(
            string customerBuyCcy, 
            string customerSellCcy, 
            string localCcy, 
            decimal dealerRate, 
            decimal customerBuyAmount, 
            decimal customerSellAmount)
        {
            var profits = new decimal[3];
            if (string.IsNullOrWhiteSpace(customerBuyCcy) || string.IsNullOrWhiteSpace(customerSellCcy)
                || string.IsNullOrEmpty(localCcy))
            {
                TraceManager.Warn.Write("CalculateCore.CalculateAllProfits", "Input currency is empty");
                return profits;
            }

            profits[0] = this.CalculateProfit(
                customerBuyCcy, 
                customerSellCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);
            profits[1] = this.CalculateProfitByLocalCcy(
                customerBuyCcy, 
                customerSellCcy, 
                localCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);
            profits[2] = this.CalculateProfitByUsd(
                customerBuyCcy, 
                customerSellCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);

            if (this.currencyRep != null)
            {
                BaseCurrencyVM customerBuyCurrency = this.currencyRep.FindByID(customerBuyCcy);
                BaseCurrencyVM localCurrency = this.currencyRep.FindByID(localCcy);
                BaseCurrencyVM usdCurrency = this.currencyRep.FindByID(this.currencyRep.GetUsdCurrencyId());
                if (customerBuyCurrency != null && localCurrency != null && usdCurrency != null)
                {
                    profits[0] = profits[0].FormatAmountByCCYConfig(
                        customerBuyCurrency.RoundingMethod, 
                        customerBuyCurrency.AmountDecimals);
                    profits[1] = profits[1].FormatAmountByCCYConfig(
                        localCurrency.RoundingMethod, 
                        localCurrency.AmountDecimals);
                    profits[2] = profits[2].FormatAmountByCCYConfig(
                        usdCurrency.RoundingMethod, 
                        usdCurrency.AmountDecimals);
                }
            }

            return profits;
        }

        /// <summary>
        /// 计算佣金
        /// </summary>
        /// <param name="currencyId">
        /// 货币Id
        /// </param>
        /// <param name="amount">
        /// 交易量
        /// </param>
        /// <param name="customerId">
        /// 客户账户ID
        /// </param>
        /// <returns>
        /// 佣金
        /// </returns>
        public decimal CalculateCommission(string currencyId, decimal amount, string customerId)
        {
            // 佣金金额计算公式： Customer Buy/Sell Amount * Commission rate  得出的金额
            BaseCustomerViewModel customer = this.customerRep.FindByID(customerId);
            if (customer == null)
            {
                TraceManager.Warn.Write("CalculateCore.CalculateCommission", "Cant find customer");
                return amount;
            }

            BaseCurrencyVM currency = this.currencyRep.FindByID(currencyId);
            if (currency == null)
            {
                TraceManager.Warn.Write("CalculateCore.CalculateCommission", "Cant find currency");
                return amount;
            }

            BaseVIPCustSettingVM vipSetting = this.vipRep.FindByID(customerId);
            decimal commissionRate = vipSetting == null ? customer.CommissionRate : vipSetting.CommissionRate;
            return (amount * commissionRate / 100).FormatAmountByCCYConfig(currency.RoundingMethod, currency.AmountDecimals);
        }

        /// <summary>
        /// 根据输入的货币量计算非输入货币的货币量
        /// </summary>
        /// <param name="inputAmount">
        /// 输入货币的货币量
        /// </param>
        /// <param name="contractRate">
        /// 输入货币和非输入货币的交易比率
        /// </param>
        /// <param name="direction">
        /// 组成的货币对(交易比率)方向
        /// </param>
        /// <returns>
        /// 非输入货币对应的货币量
        /// </returns>
        public decimal CalculateCounterCurrencyAmount(
            decimal inputAmount, 
            decimal contractRate, 
            EnumDirection direction)
        {
            decimal counterCurrencyAmount = decimal.Zero;
            if (direction == EnumDirection.Before)
            {
                // 输入货币对非输入货币
                counterCurrencyAmount = Math.Round(inputAmount * contractRate, 2);
            }
            else if (direction == EnumDirection.After)
            {
                // 非输入货币对输入货币
                counterCurrencyAmount = contractRate == decimal.Zero
                                            ? counterCurrencyAmount
                                            : (inputAmount / contractRate).DiscardedDecimals();
            }

            return counterCurrencyAmount;
        }

        /// <summary>
        /// 按货币对计算MTM
        /// </summary>
        /// <param name="symbol">
        /// 货币对
        /// </param>
        /// <param name="ccy1Amount">
        /// CCY1 数量
        /// </param>
        /// <param name="ccy2Amount">
        /// CCY2 数量
        /// </param>
        /// <param name="marketRate">
        /// 市场价
        /// </param>
        /// <param name="localCcyId">
        /// 本币ID
        /// </param>
        /// <returns>
        /// MTM值
        /// </returns>
        public decimal CalculateMtm(
            BaseSymbolVM symbol, 
            decimal ccy1Amount, 
            decimal ccy2Amount, 
            decimal marketRate, 
            string localCcyId)
        {
            if (symbol == null)
            {
                return decimal.Zero;
            }

            // MTM（CCY2）＝ CCY1 Amount * Market Rate + CCY2Amount
            decimal mtmCcy2 = (ccy1Amount * marketRate) + ccy2Amount;
            if (symbol.CCY2 == localCcyId)
            {
                return Math.Round(mtmCcy2, 2);
            }

            // 如 MTM要求按 Local CCY（或者是其他货币NZD）显示，则需要使用 Mid Rate转换为 Local CCY（NZD）
            return this.GetTransferAmount(symbol.CCY2, localCcyId, mtmCcy2);
        }

        /// <summary>
        /// 计算利润
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="customerSellCcy">
        /// 客户卖货币
        /// </param>
        /// <param name="dealerRate">
        /// TraderRateAsk: Customer Buy CCY to Customer Sell CCY
        ///     TraderRateBid: Customer Sell CCY to Customer Buy CCY
        /// </param>
        /// <param name="customerBuyAmount">
        /// 客户买数量
        /// </param>
        /// <param name="customerSellAmount">
        /// 客户卖数量
        /// </param>
        /// <returns>
        /// 计算的Profit值
        /// </returns>
        public decimal CalculateProfit(
            string customerBuyCcy, 
            string customerSellCcy, 
            decimal dealerRate, 
            decimal customerBuyAmount, 
            decimal customerSellAmount)
        {
            // 利润（Customer Buy CCY）即 Profit Customer By CCY 公式：
            // 货币对：Customer Buy CCY & Customer Sell CCY
            // 利润（Customer Buy CCY） = Customer Sell Amount / Trader Rate Ask - Customer Buy Amount 
            // 货币对：Customer Sell CCY & Customer Buy CCY 
            // 利润（Customer Buy CCY） = Customer Sell Amount * Trader Rate Bid - Customer Buy Amount 
            if (string.IsNullOrWhiteSpace(customerBuyCcy) || string.IsNullOrWhiteSpace(customerSellCcy))
            {
                TraceManager.Warn.Write("CalculateCore.CalculateProfit", "Input currency is empty");
                return decimal.Zero;
            }

            EnumDirection direction;
            BaseSymbolVM symbol = this.symbolRep.GetConvertSymbol(customerBuyCcy, customerSellCcy, out direction);
            if (symbol == null || direction == EnumDirection.NotExisting || direction == EnumDirection.Equals)
            {
                TraceManager.Warn.Write(
                    "CalculateCore.CalculateProfit", 
                    "Can't find symbol by CCY1: {0} CCY2: {1}", 
                    customerBuyCcy, 
                    customerSellCcy);
                return decimal.Zero;
            }

            return direction == EnumDirection.Before
                       ? (dealerRate == decimal.Zero
                              ? decimal.Zero
                              : (customerSellAmount / dealerRate) - customerBuyAmount)
                       : (customerSellAmount * dealerRate) - customerBuyAmount;
        }

        /// <summary>
        /// 计算本币利润
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="customerSellCcy">
        /// 客户卖货币
        /// </param>
        /// <param name="localCcy">
        /// 本币
        /// </param>
        /// <param name="dealerRate">
        /// TraderRateAsk: Customer Buy CCY to Customer Sell CCY
        ///     TraderRateBid: Customer Sell CCY to Customer Buy CCY
        /// </param>
        /// <param name="customerBuyAmount">
        /// 客户买数量
        /// </param>
        /// <param name="customerSellAmount">
        /// 客户卖数量
        /// </param>
        /// <returns>
        /// 计算的Profit值
        /// </returns>
        public decimal CalculateProfitByLocalCcy(
            string customerBuyCcy, 
            string customerSellCcy, 
            string localCcy, 
            decimal dealerRate, 
            decimal customerBuyAmount, 
            decimal customerSellAmount)
        {
            // 利润（本币）即 Profit Local ccy 公式：
            // 货币对：Customer Buy CCY & Local CCY 
            // 利润（本币）= 利润（by Customer Buy CCY）* Mid Rate
            // 货币对：Local CCY & Customer Buy
            // 利润（本币）= 利润（by Customer Buy CCY）/ Mid Rate
            if (string.IsNullOrEmpty(customerBuyCcy) || string.IsNullOrEmpty(customerSellCcy)
                || string.IsNullOrEmpty(localCcy))
            {
                TraceManager.Warn.Write("CalculateCore.CalculateProfitByLocalCcy", "Input currency is empty");
                return decimal.Zero;
            }

            decimal customerBuyCcyProfit = this.CalculateProfit(
                customerBuyCcy, 
                customerSellCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);
            return this.TransferProfit(customerBuyCcy, localCcy, customerBuyCcyProfit);
        }

        /// <summary>
        /// 根据当前报价计算本币的Profit
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="customerSellCcy">
        /// 客户卖货币
        /// </param>
        /// <param name="localCcy">
        /// 本币
        /// </param>
        /// <param name="customerBuyAmount">
        /// 客户买数量
        /// </param>
        /// <param name="customerSellAmount">
        /// 客户卖数量
        /// </param>
        /// <returns>
        /// 计算的Profit值
        /// </returns>
        public decimal CalculateProfitByLocalCcyAndCurrentPrice(
            string customerBuyCcy, 
            string customerSellCcy, 
            string localCcy, 
            decimal customerBuyAmount, 
            decimal customerSellAmount)
        {
            PriceCore.SwapPrice swapPrice = PriceCore.Instance.GetMarketQuote(customerBuyCcy, customerSellCcy);
            if (swapPrice == null || swapPrice.PriceDirection == EnumDirection.Equals
                || swapPrice.PriceDirection == EnumDirection.NotExisting || swapPrice.QuotePrice == null)
            {
                TraceManager.Warn.Write("CalculateCore.CalculateProfitByLocalCcyAndCurrentPrice", "Can not find quote");
                return customerBuyAmount;
            }

            decimal dealerRate = swapPrice.PriceDirection == EnumDirection.Before
                                     ? swapPrice.QuotePrice.TraderAsk
                                     : swapPrice.QuotePrice.TraderBid;
            decimal localProfit = this.CalculateProfitByLocalCcy(
                customerBuyCcy, 
                customerSellCcy, 
                localCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);
            return Math.Round(localProfit, 2);
        }

        /// <summary>
        /// 计算USD利润
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="customerSellCcy">
        /// 客户卖货币
        /// </param>
        /// <param name="dealerRate">
        /// TraderRateAsk: Customer Buy CCY to Customer Sell CCY
        ///     TraderRateBid: Customer Sell CCY to Customer Buy CCY
        /// </param>
        /// <param name="customerBuyAmount">
        /// 客户买数量
        /// </param>
        /// <param name="customerSellAmount">
        /// 客户卖数量
        /// </param>
        /// <returns>
        /// 计算的Profit值
        /// </returns>
        public decimal CalculateProfitByUsd(
            string customerBuyCcy, 
            string customerSellCcy, 
            decimal dealerRate, 
            decimal customerBuyAmount, 
            decimal customerSellAmount)
        {
            // 利润（美元）即 Profit USD 公式：
            // 货币对：Customer Buy CCY & USD
            // 利润（美元）= 利润（by Customer Buy CCY）* Mid Rate
            // 货币对：USD & Customer Buy CCY
            // 利润（美元）= 利润（by Customer Buy CCY）/ Mid Rate
            if (string.IsNullOrEmpty(customerBuyCcy) || string.IsNullOrEmpty(customerSellCcy))
            {
                TraceManager.Warn.Write("CalculateCore.CalcuateProfitByUsd", "Input currency is empty");
                return decimal.Zero;
            }

            string usdCcy = this.currencyRep.GetUsdCurrencyId();
            if (string.IsNullOrEmpty(usdCcy))
            {
                TraceManager.Warn.Write("CalculateCore.CalcuateProfitByUsd", "Cant find USD currency id");
                return decimal.Zero;
            }

            decimal customerBuyCcyProfit = this.CalculateProfit(
                customerBuyCcy, 
                customerSellCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);
            return this.TransferProfit(customerBuyCcy, usdCcy, customerBuyCcyProfit);
        }

        /// <summary>
        /// 计算USD利润
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="customerSellCcy">
        /// 客户卖货币
        /// </param>
        /// <param name="customerBuyAmount">
        /// 客户买数量
        /// </param>
        /// <param name="customerSellAmount">
        /// 客户卖数量
        /// </param>
        /// <returns>
        /// 计算的Profit值
        /// </returns>
        public decimal CalculateProfitByUsdAndCurrentPrice(
            string customerBuyCcy, 
            string customerSellCcy, 
            decimal customerBuyAmount, 
            decimal customerSellAmount)
        {
            PriceCore.SwapPrice swapPrice = PriceCore.Instance.GetMarketQuote(customerBuyCcy, customerSellCcy);
            if (swapPrice == null || swapPrice.PriceDirection == EnumDirection.Equals
                || swapPrice.PriceDirection == EnumDirection.NotExisting || swapPrice.QuotePrice == null)
            {
                TraceManager.Warn.Write("CalculateCore.CalculateProfitByLocalCcyAndCurrentPrice", "Can not find quote");
                return customerBuyAmount;
            }

            decimal dealerRate = swapPrice.PriceDirection == EnumDirection.Before
                                     ? swapPrice.QuotePrice.TraderAsk
                                     : swapPrice.QuotePrice.TraderBid;
            decimal localProfit = this.CalculateProfitByUsd(
                customerBuyCcy, 
                customerSellCcy, 
                dealerRate, 
                customerBuyAmount, 
                customerSellAmount);
            return Math.Round(localProfit, 2);
        }

        /// <summary>
        /// 计算ProfitMargin
        /// </summary>
        /// <param name="inputCcy">
        /// 输入货币
        /// </param>
        /// <param name="localCcy">
        /// 本币
        /// </param>
        /// <param name="inputAmount">
        /// 输入货币量
        /// </param>
        /// <param name="localCcyProfit">
        /// 本币利润
        /// </param>
        /// <returns>
        /// ProfitMargin
        /// </returns>
        public decimal CalculateProfitMargin(
            string inputCcy, 
            string localCcy, 
            decimal inputAmount, 
            decimal localCcyProfit)
        {
            if (string.IsNullOrEmpty(inputCcy) || string.IsNullOrEmpty(localCcy))
            {
                TraceManager.Warn.Write("CalculateCore.CalculateProfitMargin", "Input parameter error!");
                return decimal.Zero;
            }

            // Spot / Forward / Limit Order / Stop Order / OCO Order （只计算 A 单的）
            // IF Done Order 主动单、被动单均未激活，只计算主动单的  
            // IF Done Order 主动单激活、被动单未激活，只计算被动单的 : 
            // Profit Margin = 利润（本币）/  Per Deal Position
            decimal perDealPosition = inputCcy == localCcy
                                          ? inputAmount
                                          : PriceCore.Instance.GetQuoteDealPosition(inputCcy, inputAmount, localCcy);
            return perDealPosition == decimal.Zero ? decimal.Zero : localCcyProfit / perDealPosition;
        }

        /// <summary>
        /// 计算Withdrawal Balance
        /// </summary>
        /// <param name="customer">
        /// 客户账户
        /// </param>
        /// <param name="accountsType">
        /// 内部账户类型
        /// </param>
        /// <param name="ccyId">
        /// 内部账户的Currency
        /// </param>
        /// <returns>
        /// Withdrawal Balance结果
        /// </returns>
        public decimal CalculateWithdrawalBalance(
            BaseCustomerViewModel customer, 
            InternalAccountTypeEnum accountsType, 
            string ccyId)
        {
            if (customer == null)
            {
                TraceManager.Warn.Write("CalculateCore.CalculateWithdrawalBalance", "Input parameter error!");
                return decimal.Zero;
            }

            // Local CCY 子账户： Available Balance + Credit Interest + Overdraft Penalty
            // 非 Local CCY 子账户：Available Balance + Overdraft Penalty
            decimal availableBalance = decimal.Zero;
            decimal creditInterest = decimal.Zero;
            decimal overdraftPenalty = decimal.Zero;
            List<CustInternalAcctModel> internalAccountList = accountsType == InternalAccountTypeEnum.SettlementForCash
                                                                  ? customer.SettlementAccounts
                                                                  : customer.CollateralAccounts;
            CustInternalAcctModel internalAccountForCurrency =
                internalAccountList.FirstOrDefault(o => o.CurrencyID == ccyId);
            if (internalAccountForCurrency != null)
            {
                availableBalance = internalAccountForCurrency.AvailableBalance;
                overdraftPenalty = internalAccountForCurrency.OverdraftPenalty;
            }

            // 如果是LocalCCY
            if (customer.LocalCCYID == ccyId)
            {
                creditInterest = customer.Account.CustomerCapital.CreditInterest;
            }

            return Math.Round(availableBalance + creditInterest + overdraftPenalty, 2);
        }

        /// <summary>
        /// 根据CCY1的货币及数量计算CCY2的货币数量
        /// </summary>
        /// <param name="ccy1Id">
        /// CCY1 ID
        /// </param>
        /// <param name="ccy2Id">
        /// CCY2 ID
        /// </param>
        /// <param name="ccy1Amount">
        /// CCY1 数量
        /// </param>
        /// <returns>
        /// CCY2 数量
        /// </returns>
        public decimal GetTransferAmount(string ccy1Id, string ccy2Id, decimal ccy1Amount)
        {
            return Math.Round(this.TransferProfit(ccy1Id, ccy2Id, ccy1Amount), 2);
        }

        /// <summary>
        /// 计算转换Profit
        /// </summary>
        /// <param name="customerBuyCcy">
        /// 客户买货币
        /// </param>
        /// <param name="otherCcy">
        /// 转换的货币
        /// </param>
        /// <param name="customerBuyCcyProfit">
        /// 客户买货币的Profit
        /// </param>
        /// <returns>
        /// 转换后的货币Profit
        /// </returns>
        public decimal TransferProfit(string customerBuyCcy, string otherCcy, decimal customerBuyCcyProfit)
        {
            if (customerBuyCcy == otherCcy)
            {
                TraceManager.Warn.Write("CalculateCore.TransferProfit", "Input currency are same");
                return customerBuyCcyProfit;
            }

            EnumDirection direction;
            BaseSymbolVM symbol = this.symbolRep.GetConvertSymbol(customerBuyCcy, otherCcy, out direction);
            if (symbol == null || direction == EnumDirection.Equals || direction == EnumDirection.NotExisting)
            {
                TraceManager.Warn.Write(
                    "CalculateCore.TransferProfit", 
                    "Cant find symbol for CCY1: {0} CCY2: {1}", 
                    customerBuyCcy, 
                    otherCcy);
                return customerBuyCcyProfit;
            }

            BaseQuoteVM quote = this.quoteCacheRepository.FindByID(symbol.SymbolID);
            if (quote == null || quote.MidRate == decimal.Zero)
            {
                TraceManager.Warn.Write(
                    "CalculateCore.TransferProfit", 
                    "Cant find quote by symbol: {0}", 
                    symbol.SymbolName);
                return customerBuyCcyProfit;
            }

            return direction == EnumDirection.Before
                       ? customerBuyCcyProfit * quote.MidRate
                       : customerBuyCcyProfit / quote.MidRate;
        }

        #endregion
    }
}