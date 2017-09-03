// <copyright file="CommonVerify.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2013/11/27 10:49:22 </date>
// <modify>
//   修改人：donggj
//   修改时间：2013/11/27 10:49:22
//   修改描述：新建 CommonVerify
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

using Infrastructure.Common;
using Infrastructure.Common.Enums;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrorMsg = Infrastructure.Common.ErrorMsg;
namespace Infrastructure
{
    /// <summary>
    /// 公共的逻辑判断
    /// </summary>
    public class CommonVerify
    {
        /// <summary>
        /// 验证账户是否为Active状态
        /// </summary>
        /// <param name="custModel">客户Model实体</param>
        public static bool IsCustomerEnabledActive(CustomerModel custModel)
        {
            return custModel.BaseInfo.AccountStatus == CustomerStatusEnum.EnabledActive;
        }

        /// <summary>
        /// 验证BU是否可以交易
        /// </summary>
        /// <param name="clientTradeStatus">客户端交易状态</param>
        /// <returns>返回BU是否可以交易</returns>
        public static bool IsBUVerify(bool isOTC, BusinessUnitModel bUnitModel, string ccy1ID, string ccy2ID, string symbolID, string inputCCY, decimal amount, out string rstPrompt)
        {
            rstPrompt = ErrorMsg.Common.Success;

            if (isOTC == false)
            {
                // 验证BU是否禁用客户端交易
                if (bUnitModel.BaseInfo.ClientTradeStatus == ClientTradeStatusEnum.TotalForbidden)
                {
                    rstPrompt = ErrorMsg.Client.BU_TotalForbidden;
                    return false;
                }

                if (bUnitModel.BaseInfo.ClientTradeStatus == ClientTradeStatusEnum.TradeForbidden)
                {
                    rstPrompt = ErrorMsg.Client.BU_TotalForbidden;
                    return false;
                }
            }


            // 验证BUCCY1是否存在，是否启用，是否小于最小交易量
            var buCCY1 = bUnitModel.BUCurrencyList.FirstOrDefault(c => c.CurrencyID == ccy1ID);

            if (buCCY1 == null)
            {
                rstPrompt = ErrorMsg.Common.BUCCY_NotExist;
                return false;
            }

            if (buCCY1.Status == false)
            {
                rstPrompt = isOTC == true ? ErrorMsg.Manager.BUCCY_NotEnabled : ErrorMsg.Client.BUCCY_NotEnabled;
                return false;
            }
            if (isOTC == false)
            {
                if (inputCCY == ccy1ID)
                {
                    if (buCCY1.MinimumAmount > amount)
                    {
                        rstPrompt = ErrorMsg.Client.BUCCY_BelowMinAmount;
                        return false;
                    }
                }
            }

            // 验证BUCCY2是否存在、是否启用、是否小于最小交易量
            var buCCY2 = bUnitModel.BUCurrencyList.FirstOrDefault(c => c.CurrencyID == ccy2ID);

            if (buCCY2 == null)
            {
                rstPrompt = ErrorMsg.Common.BUCCY_NotExist;
                return false;
            }

            if (buCCY2.Status == false)
            {
                rstPrompt = isOTC == true ? ErrorMsg.Manager.BUCCY_NotEnabled : ErrorMsg.Client.BUCCY_NotEnabled;
                return false;
            }

            if (isOTC == false)
            {
                if (inputCCY == ccy2ID)
                {
                    if (buCCY2.MinimumAmount > amount)
                    {
                        rstPrompt = ErrorMsg.Client.BUCCY_BelowMinAmount;
                        return false;
                    }
                }
            }

            // 验证BUSymbol是否存在、是否启用
            var buSymbol = bUnitModel.BUSymbolList.FirstOrDefault(s => s.SymbolID == symbolID);

            if (buSymbol == null)
            {
                rstPrompt = ErrorMsg.Common.BUSymbol_NotExist;
                return false;
            }

            // TODO 这个版本暂时禁用
            //if (buSymbol.Status != SymbolStatusEnum.Enabled)
            //{
            //    rstPrompt = isOTC == true ? ErrorMsg.Manager.BUSymbol_Disabled : ErrorMsg.Client.BUSymbol_Disabled;
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 验证商品是否可以交易
        /// </summary>
        /// <param name="clientTradeStatus">客户端交易状态</param>
        /// <returns>返回商品是否可以交易</returns>
        public static bool IsSymbolVerify(bool isOTC, SymbolModel symbolModel, TradaOrderEnum orderType, TradInstrumentEnum instrument, out string rstPrompt)
        {
            rstPrompt = ErrorMsg.Common.Success;

            // 验证商品是否允许交易
            if (symbolModel.Tradable == TradableEnum.ViewOnly)
            {
                rstPrompt = isOTC == true ? ErrorMsg.Manager.Symbol_ViewOnly : ErrorMsg.Client.Symbol_ViewOnly;
                return false;
            }

            // 判断是否包含参数中的TradableOrdertype枚举项
            if (symbolModel.TradableOrderType.Contains(orderType) == false)
            {
                rstPrompt = isOTC == true ? ErrorMsg.Manager.Symbol_OrderTypeDisabled : ErrorMsg.Client.Symbol_OrderTypeDisabled;
                return false;
            }

            // 判断是否包含参数中的TradInstrumentEnum枚举项
            if (symbolModel.TradableInstrument.Contains(instrument) == false)
            {
                rstPrompt = isOTC == true ? ErrorMsg.Manager.Symbol_InstrumentDisabled : ErrorMsg.Client.Symbol_InstrumentDisabled;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Spot关于商品的验证
        /// </summary>
        /// <param name="symbolModel">商品</param>
        /// <param name="instrument">交易手段</param>
        /// <param name="tenor">远期类型</param>
        /// <param name="rstPrompt"></param>
        /// <returns></returns>
        public static bool IsSpotSymbolVerify(SymbolModel symbolModel, TradInstrumentEnum instrument, TenorEnum tenor, out string rstPrompt)
        {
            rstPrompt = ErrorMsg.Common.Success;

            // 验证Instrument为Spot时，是否存在该Tenor的配置
            if (instrument == TradInstrumentEnum.FXSpot)
            {
                if (symbolModel.FXSpotConfiguration.Any(s => (int)s == (int)tenor) == false)
                {
                    rstPrompt = ErrorMsg.Common.Symbol_InstrumentInvalid;
                    return false;
                }
            }

            return true;
        }
    }
}