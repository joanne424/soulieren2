// <copyright file="CallBackModel.partial.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/03/03 06:14:54 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/03/03 06:14:54
//      修改描述：新建 CallBackModel.partial.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models
{
    #region Using

    using BaseViewModel;
    using DestributeService.Dto;
    using DestributeService.Seedwork;
    using GalaSoft.MvvmLight.Messaging;
    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Models;
    using Infrastructure.Service;
    using Infrastructure.Service.Base;
    using Infrastructure.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    #endregion


    /// <summary>
    /// The call back model.
    /// </summary>
    public partial class CallBackModel
    {
        #region DtoCurrency

        /// <summary>
        /// 货币推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCurrency")]
        private void HandleCurrencyCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoCurrency
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseCurrencyVM baseCurrencyVm = BaseCurrencyVM.Factory(contentData);
                if (baseCurrencyVm != null)
                {
                    this.GetRepository<ICurrencyCacheRepository>().AddOrUpdate(baseCurrencyVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseCurrencyVM baseCurrencyVm = BaseCurrencyVM.Factory(contentData);
                if (baseCurrencyVm != null)
                {
                    this.GetRepository<ICurrencyCacheRepository>().Remove(baseCurrencyVm.GetID());
                }
            }
        }

        #endregion

        #region DtoSymbol

        /// <summary>
        /// 货币对推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoSymbol")]
        private void HandleSymbolCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoSymbol
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseSymbolVM baseSymbolVm = BaseSymbolVM.Factory(contentData);
                if (baseSymbolVm != null)
                {
                    this.GetRepository<ISymbolCacheRepository>().AddOrUpdate(baseSymbolVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseSymbolVM baseSymbolVm = BaseSymbolVM.Factory(contentData);
                if (baseSymbolVm != null)
                {
                    this.GetRepository<ISymbolCacheRepository>().Remove(baseSymbolVm.GetID());
                }
            }
        }

        #endregion

        #region DtoGeneralSettings

        /// <summary>
        /// 通用配置推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoGeneralSettings")]
        private void HandleGeneralSettingsCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseGeneralSettingVM baseGeneralSettingVm = BaseGeneralSettingVM.Factory(contentData);
                if (baseGeneralSettingVm != null)
                {
                    this.GetRepository<IGeneralSettingCacheRepository>().AddOrUpdate(baseGeneralSettingVm);
                    SessionHelper.Instance.SetIdleTime(new TimeSpan(0, baseGeneralSettingVm.LogoutIdleTime, 0));
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseGeneralSettingVM baseGeneralSettingVm = BaseGeneralSettingVM.Factory(contentData);
                if (baseGeneralSettingVm != null)
                {
                    this.GetRepository<IGeneralSettingCacheRepository>().Remove(baseGeneralSettingVm.GetID());
                }
            }
        }

        #endregion

        #region DtoCustomerGroup

        /// <summary>
        /// 客户组推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCustomerGroup")]
        private void HandleCustomerGroupCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseCustomerGroupVM baseCustomerGroupVm = BaseCustomerGroupVM.Factory(contentData);
                if (baseCustomerGroupVm != null)
                {
                    this.GetRepository<ICustomerGroupCacheRepository>().AddOrUpdate(baseCustomerGroupVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseCustomerGroupVM baseCustomerGroupVm = BaseCustomerGroupVM.Factory(contentData);
                if (baseCustomerGroupVm != null)
                {
                    this.GetRepository<ICustomerGroupCacheRepository>().Remove(baseCustomerGroupVm.GetID());
                }
            }
        }

        #endregion

        #region DtoBusinessUnit

        /// <summary>
        /// 业务区推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoBusinessUnit")]
        private void HandleBusinessUnitCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseBusinessUnitVM baseBusinessUnitVm = BaseBusinessUnitVM.Factory(contentData);
                if (baseBusinessUnitVm != null)
                {
                    this.GetRepository<IBusinessUnitCacheRepository>().AddOrUpdate(baseBusinessUnitVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseBusinessUnitVM baseBusinessUnitVm = BaseBusinessUnitVM.Factory(contentData);
                if (baseBusinessUnitVm != null)
                {
                    this.GetRepository<IBusinessUnitCacheRepository>().Remove(baseBusinessUnitVm.GetID());
                }
            }
        }

        #endregion

        #region DtoQuoteGroup

        /// <summary>
        /// Quote组推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoQuoteGroup")]
        private void HandleQuoteGroupCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseQuoteGroupConfigVM quoteGroupVm = BaseQuoteGroupConfigVM.Factory(contentData);
                if (quoteGroupVm != null)
                {
                    this.GetRepository<IQuoteGroupCacheRepository>().AddOrUpdate(quoteGroupVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseQuoteGroupConfigVM quoteGroupVm = BaseQuoteGroupConfigVM.Factory(contentData);
                if (quoteGroupVm != null)
                {
                    this.GetRepository<IQuoteGroupCacheRepository>().Remove(quoteGroupVm.GetID());
                }
            }
        }

        #endregion

        #region DtoSettlementHoliday

        /// <summary>
        /// 结算节假日推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoSettlementHoliday")]
        private void HandleSettlementHolidayCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseSettlementHolidayVM baseSettlementHolidayVm = BaseSettlementHolidayVM.Factory(contentData);
                if (baseSettlementHolidayVm != null)
                {
                    this.GetRepository<ISettlementHolidayCacheRepository>().AddOrUpdate(baseSettlementHolidayVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseSettlementHolidayVM baseSettlementHolidayVm = BaseSettlementHolidayVM.Factory(contentData);
                if (baseSettlementHolidayVm != null)
                {
                    this.GetRepository<ISettlementHolidayCacheRepository>().Remove(baseSettlementHolidayVm.GetID());
                }
            }
        }

        #endregion

        #region DtoCountry

        /// <summary>
        /// 国家推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCountry")]
        private void HandleCountryCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseCountryVM baseCountryVm = BaseCountryVM.Factory(contentData);
                if (baseCountryVm != null)
                {
                    this.GetRepository<ICountryCacheRepository>().AddOrUpdate(baseCountryVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseCountryVM baseCountryVm = BaseCountryVM.Factory(contentData);
                if (baseCountryVm != null)
                {
                    this.GetRepository<ICountryCacheRepository>().Remove(baseCountryVm.GetID());
                }
            }
        }

        #endregion

        #region DtoCustomerCategory

        /// <summary>
        /// 账户种类推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCustomerCategory")]
        private void HandleCustomerCategoryCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseCustomerCategoryVM baseCustomerCategoryVm = BaseCustomerCategoryVM.Factory(contentData);
                if (baseCustomerCategoryVm != null)
                {
                    this.GetRepository<ICustomerCategoryCacheRepository>().AddOrUpdate(baseCustomerCategoryVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseCustomerCategoryVM baseCustomerCategoryVm = BaseCustomerCategoryVM.Factory(contentData);
                if (baseCustomerCategoryVm != null)
                {
                    this.GetRepository<ICustomerCategoryCacheRepository>().Remove(baseCustomerCategoryVm.GetID());
                }
            }
        }

        #endregion

        #region DtoCustomerGrading

        /// <summary>
        /// 账户等级推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCustomerGrading")]
        private void HandleCustomerGradingCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseCustGradingVM baseCustGradingVm = BaseCustGradingVM.Factory(contentData);
                if (baseCustGradingVm != null)
                {
                    this.GetRepository<ICustGradingCacheRepository>().AddOrUpdate(baseCustGradingVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseCustGradingVM baseCustGradingVm = BaseCustGradingVM.Factory(contentData);
                if (baseCustGradingVm != null)
                {
                    this.GetRepository<ICustGradingCacheRepository>().Remove(baseCustGradingVm.GetID());
                }
            }
        }

        #endregion

        #region DtoStaff

        /// <summary>
        /// 员工推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoStaff")]
        private void HandleStaffCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseStaffVM baseStaffVm = BaseStaffVM.Factory(contentData);
                if (baseStaffVm != null)
                {
                    this.GetRepository<IStaffCacheRepository>().AddOrUpdate(baseStaffVm);

                    // 当Admin更改的是当前Staff时更新当前RunTime的当前Staff
                    if (actionType == PushTypeEnum.Update
                        && baseStaffVm.StaffID == RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo.StaffID)
                    {
                        RunTime.GetCurrentRunTime(this.OwnerId).SetCurrentUer(baseStaffVm.PropSet);
                        if (baseStaffVm.Locked)
                        {
                            var msgDic = new Dictionary<string, string>
                                             {
                                                 { "Content", "Userlock" },
                                                 { "PromptEnum", string.Empty }
                                             };

                            RunTime.GetCurrentRunTime(this.OwnerId).SendMessege(msgDic, "ErrorDialogAndExit");
                        }
                    }
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseStaffVM baseStaffVm = BaseStaffVM.Factory(contentData);
                if (baseStaffVm != null)
                {
                    this.GetRepository<IStaffCacheRepository>().Remove(baseStaffVm.GetID());
                }
            }
        }

        #endregion

        #region DtoBankAccount

        /// <summary>
        /// 银行帐号推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoBankAccount")]
        private void HandleBankAccountCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoBankAccount
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseBankAccountVM baseBankAccountVm = BaseBankAccountVM.Factory(contentData);
                if (baseBankAccountVm != null)
                {
                    this.GetRepository<IBankAccountCacheRepository>().AddOrUpdate(baseBankAccountVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseBankAccountVM baseBankAccountVm = BaseBankAccountVM.Factory(contentData);
                if (baseBankAccountVm != null)
                {
                    this.GetRepository<IBankAccountCacheRepository>().Remove(baseBankAccountVm.GetID());
                }
            }
        }

        #endregion

        #region DtoDefaultBankAcct

        /// <summary>
        /// 默认银行帐号推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoDefaultBankAcct")]
        private void HandleDefaultBankAcctCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseDefaultBankAccVM baseDefaultBankAccVm = BaseDefaultBankAccVM.Factory(contentData);
                if (baseDefaultBankAccVm != null)
                {
                    this.GetRepository<IDefaultBankAccCacheRepository>().AddOrUpdate(baseDefaultBankAccVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseDefaultBankAccVM baseDefaultBankAccVm = BaseDefaultBankAccVM.Factory(contentData);
                if (baseDefaultBankAccVm != null)
                {
                    this.GetRepository<IDefaultBankAccCacheRepository>().Remove(baseDefaultBankAccVm.GetID());
                }
            }
        }

        #endregion

        #region DtoRole

        /// <summary>
        /// 角色推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoRole")]
        private void HandleRoleCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseRoleVM baseRoleVm = BaseRoleVM.Factory(contentData);
                if (baseRoleVm != null)
                {
                    this.GetRepository<IRoleModelCasheRepository>().AddOrUpdate(baseRoleVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseRoleVM baseRoleVm = BaseRoleVM.Factory(contentData);
                if (baseRoleVm != null)
                {
                    this.GetRepository<IRoleModelCasheRepository>().Remove(baseRoleVm.GetID());
                }
            }
        }

        #endregion

        #region DtoGroup

        /// <summary>
        /// 组推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoGroup")]
        private void HandleGroupCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseGroupVM baseGroupVm = BaseGroupVM.Factory(contentData);
                if (baseGroupVm != null)
                {
                    this.GetRepository<IGroupCacheRepository>().AddOrUpdate(baseGroupVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseGroupVM baseGroupVm = BaseGroupVM.Factory(contentData);
                if (baseGroupVm != null)
                {
                    this.GetRepository<IGroupCacheRepository>().Remove(baseGroupVm.GetID());
                }
            }
        }

        #endregion

        #region DtoBank

        /// <summary>
        /// 银行推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoBank")]
        private void HandleBankCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseBankVM baseBankVm = BaseBankVM.Factory(contentData);
                if (baseBankVm != null)
                {
                    this.GetRepository<IBankCacheRepository>().AddOrUpdate(baseBankVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseBankVM baseBankVm = BaseBankVM.Factory(contentData);
                if (baseBankVm != null)
                {
                    this.GetRepository<IBankCacheRepository>().Remove(baseBankVm.GetID());
                }
            }
        }

        #endregion

        #region DtoLocale

        /// <summary>
        /// 区域性推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoLocale")]
        private void HandleLocaleCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                BaseLocaleVM baseLocaleVm = BaseLocaleVM.Factory(contentData);
                if (baseLocaleVm != null)
                {
                    this.GetRepository<ILocaleCacheRepository>().AddOrUpdate(baseLocaleVm);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                BaseLocaleVM baseLocaleVm = BaseLocaleVM.Factory(contentData);
                if (baseLocaleVm != null)
                {
                    this.GetRepository<ILocaleCacheRepository>().Remove(baseLocaleVm.GetID());
                }
            }
        }

        #endregion

        #region DtoMarginCall

        /// <summary>
        /// MarginCall推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoMarginCall")]
        private void HandleMarginCallCallBack(PushTypeEnum actionType, string contentData)
        {
            this.HandleMarginCallPushBack(actionType, contentData);
        }

        #endregion

        #region DtoDeal

        /// <summary>
        /// 交易推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoDeal")]
        private void HandleDealCallBack(PushTypeEnum actionType, string contentData)
        {
            var dtoDeal = JsonConvert.DeserializeObject<DtoDeal>(contentData);

            if (this.IsUnderStaffManage(dtoDeal.CustomerNo) == false)
            {
                return;
            }

            // DtoDeal
            if (actionType == PushTypeEnum.Create)
            {
                // Spot、Forward交易成功提示
                var dealmodel = (DealModel)dtoDeal;

                if (this.GetRepository<ICustomerCacheRepository>().FindByID(dealmodel.CustomerNo) == null)
                {
                    return;
                }

                this.AddAndUpdateDeal(dealmodel);

                // 新创建的交易单如果是当前Staff代客创建的就弹出成功提示框
                if (dealmodel.Channel == DealChannelEnum.OTC
                    && dtoDeal.StaffID == this.currRunTime.CurrentStaff.StaffBaseInfo.StaffID)
                {
                    if (dealmodel.Instrument != DealInstrumentEnum.FXSpot
                        && dealmodel.Instrument != DealInstrumentEnum.FXForward)
                    {
                        // Swap、RolloverSwap
                        // 如果已经存在swap的一张交易单，就取出来显示创建Swap成功信息，否则插入字典中等待第二张交易单推送过来
                        if (this.swapDealTemp.ContainsKey(dealmodel.DealID))
                        {
                            DealModel farLegModel = null;
                            DealModel nearLegModel = null;

                            var tempDeal = this.swapDealTemp[dealmodel.DealID].Clone();
                            this.swapDealTemp.Remove(dealmodel.DealID);

                            // 分清NearLegDeal和FarLegDeal
                            if (dealmodel.ValueDate.Date < tempDeal.ValueDate.Date)
                            {
                                nearLegModel = dealmodel;
                                farLegModel = tempDeal;
                            }
                            else
                            {
                                nearLegModel = tempDeal;
                                farLegModel = dealmodel;
                            }

                            // Swap 提示信息中的买卖方向
                            string transTypeInfo;
                            string weBuySell = RunTime.FindStringResource("WeBuySell");
                            string weSellBuy = RunTime.FindStringResource("WeSellBuy");

                            if (nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount)
                            {
                                transTypeInfo = nearLegModel.TransactionType == TransactionTypeEnum.Buy ? weSellBuy : weBuySell;
                            }
                            else
                            {
                                transTypeInfo = nearLegModel.TransactionType == TransactionTypeEnum.Buy ? weBuySell : weSellBuy;
                            }

                            var symbolVm = this.symbolRep.GetSymbol(nearLegModel.CCY1, nearLegModel.CCY2);
                            decimal swapPoints = decimal.Zero;

                            if (symbolVm != null)
                            {
                                swapPoints =
                                    (farLegModel.OpenRate - nearLegModel.OpenRate).ToSpread(symbolVm.BasisPoint)
                                        .FormatPriceBySymbolPoint(symbolVm.ForwardPointDecimalPlace);
                            }

                            // Swap
                            if (nearLegModel.Instrument == DealInstrumentEnum.FXSwapSpot
                                || nearLegModel.Instrument == DealInstrumentEnum.FXSwapForward)
                            {
                                string createSwapSuccessMsg = string.Format(
                                                RunTime.FindStringResource("MSG_SUC_106"),
                                                nearLegModel.DealID,
                                                nearLegModel.ExternalDealSetID,
                                                transTypeInfo,
                                                this.currencyRep.GetName(nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? nearLegModel.CCY1 : nearLegModel.CCY2),
                                                nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? nearLegModel.CCY1Amount : nearLegModel.CCY2Amount,
                                                this.currencyRep.GetName(nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? nearLegModel.CCY2 : nearLegModel.CCY1),
                                                swapPoints,
                                                nearLegModel.OpenRate,
                                                nearLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty),
                                                farLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty));
                                // 显示下Swap单成功消息
                                Application.Current.Dispatcher.Invoke(
                                    System.Windows.Threading.DispatcherPriority.Normal,
                                    new ShowInfo(
                                        () =>
                                        RunTime.ShowContractNoteInfoDialogWithoutRes("Swap", createSwapSuccessMsg, nearLegModel, farLegModel, this.OwnerId)));

                                Messenger.Default.Send(nearLegModel, "AddSwapSuceess");
                            }
                            else if (nearLegModel.Instrument == DealInstrumentEnum.FXRolloverSwapSpot
                                     || nearLegModel.Instrument == DealInstrumentEnum.FXRolloverSwapForward)
                            {
                                transTypeInfo = nearLegModel.TransactionType == TransactionTypeEnum.Buy ? weSellBuy : weBuySell;
                                if (string.IsNullOrEmpty(nearLegModel.RelatedDealID)
                                    || string.IsNullOrEmpty(farLegModel.RelatedDealID))
                                {
                                    // 按仓位展期
                                    // Rollover
                                    // 是提前交割还是展期
                                    string preOrRollover = string.Empty;

                                    // 原仓位ValueDate和展期的ValueDate
                                    DateTime originalValueDate;
                                    DateTime newValueDate;

                                    if (farLegModel.ActionType == TradeActionTypeEnum.Rollover)
                                    {
                                        preOrRollover = RunTime.FindStringResource("Predelivery");
                                        originalValueDate = farLegModel.ValueDate;
                                        newValueDate = nearLegModel.ValueDate;
                                    }
                                    else
                                    {
                                        preOrRollover = RunTime.FindStringResource("Rollover");
                                        originalValueDate = nearLegModel.ValueDate;
                                        newValueDate = farLegModel.ValueDate;
                                    }

                                    var ccy =
                                        this.currencyRep.GetName(
                                            nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                                ? nearLegModel.CCY1
                                                : nearLegModel.CCY2);
                                    var amount = nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                                     ? nearLegModel.CCY1Amount
                                                     : nearLegModel.CCY2Amount;
                                    var counterCcy =
                                        this.currencyRep.GetName(
                                            nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                                ? nearLegModel.CCY2
                                                : nearLegModel.CCY1);

                                    string rolloverMessage = string.Format(
                                        RunTime.FindStringResource("MSG_SUC_118"),
                                        preOrRollover,
                                        ccy,
                                        amount,
                                        counterCcy,
                                        originalValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty),
                                        newValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty),
                                        nearLegModel.DealID,
                                        nearLegModel.ExternalDealSetID,
                                        transTypeInfo,
                                        ccy,
                                        amount,
                                        swapPoints,
                                        nearLegModel.OpenRate,
                                        nearLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty),
                                        farLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty));

                                    // 显示下Swap单成功消息
                                    Application.Current.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.Normal,
                                        new ShowInfo(
                                            () =>
                                            RunTime.ShowContractNoteInfoDialogWithoutRes(
                                                "RolloverSwapByPos",
                                                rolloverMessage,
                                                nearLegModel,
                                                farLegModel,
                                                this.OwnerId)));

                                    Messenger.Default.Send(nearLegModel, "AddRolloverSwapPosSuceess");
                                }
                                else
                                {
                                    // Rollover
                                    // 是提前交割还是展期
                                    string preOrRollover = string.Empty;

                                    if (farLegModel.ValueDate.Date > nearLegModel.ValueDate.Date
                                        && farLegModel.ActionType == TradeActionTypeEnum.Rollover)
                                    {
                                        preOrRollover = RunTime.FindStringResource("Predelivery");
                                    }
                                    else if (farLegModel.ValueDate.Date > nearLegModel.ValueDate.Date
                                             && nearLegModel.ActionType == TradeActionTypeEnum.Rollover)
                                    {
                                        preOrRollover = RunTime.FindStringResource("Rollover");
                                    }
                                    else if (farLegModel.ValueDate.Date < nearLegModel.ValueDate.Date
                                             && farLegModel.ActionType == TradeActionTypeEnum.Rollover)
                                    {
                                        preOrRollover = RunTime.FindStringResource("Rollover");
                                    }
                                    else if (farLegModel.ValueDate.Date < nearLegModel.ValueDate.Date
                                             && nearLegModel.ActionType == TradeActionTypeEnum.Rollover)
                                    {
                                        preOrRollover = RunTime.FindStringResource("Predelivery");
                                    }

                                    #region 被提前交割或展期的货币和交易量

                                    // 被提前交割或展期的货币
                                    string ccy = string.Empty;

                                    // 被提前交割或展期的交易量
                                    decimal amount = decimal.Zero;

                                    // Swap中对冲原单的那个单子
                                    var rolloverSwapDeal = nearLegModel.ActionType == TradeActionTypeEnum.Rollover
                                                               ? nearLegModel
                                                               : farLegModel;
                                    ccy =
                                        this.currencyRep.GetName(
                                            rolloverSwapDeal.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                                ? rolloverSwapDeal.CCY1
                                                : rolloverSwapDeal.CCY2);
                                    amount = rolloverSwapDeal.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                                 ? rolloverSwapDeal.CCY1Amount
                                                 : rolloverSwapDeal.CCY2Amount;

                                    #endregion

                                    #region 被提前交割或展期的原单的DealID和External Deal Set ID、ValueDate

                                    // 被提前交割或展期的原单的DealID和External Deal Set ID
                                    string oriDealId = string.Empty;
                                    string oriExternalDealSetId = string.Empty;
                                    string oriValueDate = string.Empty;

                                    // 被提前交割或展期的原单
                                    BaseDealVM oriDealVm = null;
                                    oriDealVm =
                                        this.GetRepository<IDealCacheRepository>()
                                            .FindByExeID(nearLegModel.RelatedDealID);
                                    if (oriDealVm != null)
                                    {
                                        oriDealId = oriDealVm.DealID;
                                        oriExternalDealSetId = oriDealVm.ExternalDealSetID;
                                        oriValueDate = oriDealVm.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty);
                                    }

                                    #endregion

                                    // 提交交割或展期到的ValueDate
                                    string newValueDate = string.Empty;
                                    if (oriDealVm != null)
                                    {
                                        newValueDate = nearLegModel.ValueDate.Date != oriDealVm.ValueDate.Date
                                                           ? nearLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty)
                                                           : farLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty);
                                    }

                                    var rolloverMessage = string.Format(
                                        RunTime.FindStringResource("MSG_SUC_115"),
                                        preOrRollover,
                                        ccy,
                                        amount,
                                        oriDealId,
                                        oriExternalDealSetId,
                                        oriValueDate,
                                        newValueDate,
                                        nearLegModel.DealID,
                                        nearLegModel.ExternalDealSetID,
                                        transTypeInfo,
                                        this.currencyRep.GetName(
                                            nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                                ? nearLegModel.CCY1
                                                : nearLegModel.CCY2),
                                        nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount
                                            ? nearLegModel.CCY1Amount
                                            : nearLegModel.CCY2Amount,
                                        swapPoints,
                                        nearLegModel.OpenRate,
                                        nearLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty),
                                        farLegModel.ValueDate.FormatDateTimeByBuID(nearLegModel.BusinessUnitID, string.Empty));

                                    // 显示下Swap单成功消息
                                    Application.Current.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.Normal,
                                        new ShowInfo(
                                            () =>
                                            RunTime.ShowContractNoteInfoDialogWithoutRes(
                                                "RolloverSwapByDeal",
                                                rolloverMessage,
                                                oriDealVm.ExecutionID,
                                                nearLegModel,
                                                farLegModel,
                                                this.OwnerId)));

                                    // 显示下Rollover Swap单成功消息
                                    //Application.Current.Dispatcher.Invoke(
                                    //    System.Windows.Threading.DispatcherPriority.Normal,
                                    //    new ShowInfo(
                                    //        () =>
                                    //        RunTime.ShowInfoDialogWithoutRes(
                                    //            string.Format(
                                    //                RunTime.FindStringResource("MSG_SUC_115"),
                                    //                preOrRollover,
                                    //                ccy,
                                    //                amount,
                                    //                oriDealId,
                                    //                oriExternalDealSetId,
                                    //                oriValueDate,
                                    //                newValueDate,
                                    //                nearLegModel.DealID,
                                    //                nearLegModel.ExternalDealSetID,
                                    //                transTypeInfo,
                                    //                this.currencyRep.GetName(nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? nearLegModel.CCY1 : nearLegModel.CCY2),
                                    //                nearLegModel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? nearLegModel.CCY1Amount : nearLegModel.CCY2Amount,
                                    //                swapPoints,
                                    //                nearLegModel.OpenRate,
                                    //                nearLegModel.ValueDate.ToString("yyyy-MM-dd"),
                                    //                farLegModel.ValueDate.ToString("yyyy-MM-dd")),
                                    //            string.Empty,
                                    //            this.OwnerId)));

                                    Messenger.Default.Send(nearLegModel, "AddRolloverSwapSuceess");
                                }
                            }
                        }
                        else
                        {
                            this.swapDealTemp.Add(dealmodel.DealID, dealmodel);
                        }
                    }
                    else
                    {
                        var msg = string.Format(
                            RunTime.FindStringResource("MSG_SUC_104"),
                            dtoDeal.DealID,
                            dtoDeal.ExternalDealSetID,
                            dealmodel.TransactionType == TransactionTypeEnum.Buy ? TransactionTypeEnum.Sell : TransactionTypeEnum.Buy,
                            this.currencyRep.GetName(dealmodel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? dtoDeal.CCY1 : dtoDeal.CCY2),
                            (dealmodel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? dtoDeal.CCY1Amount.ToDecimal().FormatAmountByCCYID(dtoDeal.CCY2)
                                    : dtoDeal.CCY2Amount.ToDecimal().FormatAmountByCCYID(dtoDeal.CCY2)),
                            this.currencyRep.GetName(dealmodel.CustomerTyping == CustomerTradeTypingEnum.CCY1Amount ? dtoDeal.CCY2 : dtoDeal.CCY1),
                            dtoDeal.OpenRate,
                            Convert.ToDateTime(dtoDeal.ValueDate).FormatDateTimeByCurrStaff(string.Empty));

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new ShowInfo(() => RunTime.ShowContractNoteInfoDialogWithoutRes("SpotForwardDeal", msg, dealmodel, this.OwnerId)));

                        if (dealmodel.CCY1ForCash || dealmodel.CCY2ForCash)
                        {
                            Messenger.Default.Send(dealmodel, "AddCashDealSuceess");
                        }
                        else
                        {
                            Messenger.Default.Send(dealmodel, "AddDealSuceess");
                        }
                    }
                }
            }
            else if (actionType == PushTypeEnum.Update)
            {
                var dealModel = (DealModel)dtoDeal;

                if (dealModel.Status == DealStatusEnum.Deleted || dealModel.Status == DealStatusEnum.Settled)
                {
                    if (dealModel.DealID == dealModel.ExecutionID)
                    {
                        // Spot Forward单
                        this.RemoveDeal(dtoDeal);
                    }
                    else
                    {
                        // 实例化订单服务
                        var service = this.GetSevice<DealService>();
                        var dealAnotherLeg = service.FindAnotherLeg(dealModel);
                        if (dealAnotherLeg != null)
                        {
                            // Swap单要两张都结算或者删除才从仓储中删除
                            if (dealAnotherLeg.Status == DealStatusEnum.Deleted || dealAnotherLeg.Status == DealStatusEnum.Settled)
                            {
                                this.RemoveDeal(dtoDeal);
                                var anotherExistd = this.GetRepository<IDealCacheRepository>().FindByExeID(dealAnotherLeg.ExecutionID);
                                if (anotherExistd != null)
                                {
                                    this.RemoveDeal(anotherExistd.DealModel);
                                }
                            }
                            else
                            {
                                this.AddAndUpdateDeal(dealModel);
                                var anotherExistd = this.GetRepository<IDealCacheRepository>().FindByExeID(dealAnotherLeg.ExecutionID);
                                if (anotherExistd == null)
                                {
                                    this.AddAndUpdateDeal(anotherExistd.DealModel);
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.AddAndUpdateDeal(dealModel);
                }
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.RemoveDeal(dtoDeal);
            }
        }

        #endregion

        #region DtoPendingOrder

        /// <summary>
        /// 挂单推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoPendingOrder")]
        private void HandlePendingOrderCallBack(PushTypeEnum actionType, string contentData)
        {
            var dtoOrder = JsonConvert.DeserializeObject<DtoPendingOrder>(contentData);

            if (this.IsUnderStaffManage(dtoOrder.CustomerNo) == false)
            {
                return;
            }

            // DtoPendingOrder
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                var orderModel = (PendingOrderModel)dtoOrder;

                if (actionType == PushTypeEnum.Create
                    && dtoOrder.StaffID == this.currRunTime.CurrentStaff.StaffBaseInfo.StaffID)
                {
                    var customer = this.GetRepository<ICustomerCacheRepository>().FindByID(orderModel.CustomerNo);
                    var businessUnit =
                        this.GetRepository<IBusinessUnitCacheRepository>().FindByID(customer.BusinessUnitID);
                    DateTime businessUnitExpiryTime = DateTime.Now;

                    if (orderModel.TimeInForce == TimeinForceEnum.GTD
                        || orderModel.TimeInForce == TimeinForceEnum.ON)
                    {
                        businessUnitExpiryTime =
                            orderModel.ExpiryTime.AddHours(businessUnit.GetBuTimeZoneSavingsByGmtTime(orderModel.ExpiryTime));
                    }

                    string orderSuccessMsg = string.Format(
                                    RunTime.FindStringResource("MSG_SUC_108"),
                                    orderModel.OrderID,
                                    orderModel.TransactionType == TransactionTypeEnum.Buy ? TransactionTypeEnum.Sell : TransactionTypeEnum.Buy,
                                    this.currencyRep.GetNameByID(orderModel.CCY1),
                                    orderModel.CCY1Amount,
                                    this.currencyRep.GetNameByID(orderModel.CCY2),
                                    orderModel.OrderType.ToString().ToLower(),
                                    orderModel.OrderRate,
                                    orderModel.TimeInForce.ToShowInfo(businessUnitExpiryTime));

                    Application.Current.Dispatcher.Invoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new ShowInfo(
                            () =>
                                RunTime.ShowContractNoteInfoDialogWithoutRes("LimitStopPendingOrder", orderSuccessMsg, orderModel, this.OwnerId)
                    ));
                }

                this.AddAndUpdatePendingOrder(orderModel);
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.RemovePendingOrder(dtoOrder);
            }
        }

        #endregion

        #region DtoDepositWithdraw

        /// <summary>
        /// 出入金推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoDepositWithdraw")]
        private void HandleDepositWithdrawCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoDepositWithdraw
            if (actionType == PushTypeEnum.Create)
            {
                var dtoDWDeal = JsonConvert.DeserializeObject<DtoDepositWithdraw>(contentData);
                this.AddAndUpdateDepositWithdraw(dtoDWDeal);
                RunTime.GetDealCreatedNotifier().NotifyDepositWithdrawCreated((DepositWithdrawModel)dtoDWDeal);
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.RemoveDepositWithdraw(JsonConvert.DeserializeObject<DtoDepositWithdraw>(contentData));
            }
            else if (actionType == PushTypeEnum.Update)
            {
                this.AddAndUpdateDepositWithdraw(JsonConvert.DeserializeObject<DtoDepositWithdraw>(contentData));
            }
        }

        #endregion

        #region DtoBTBDeal

        /// <summary>
        /// 出入金推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoBTBDeal")]
        private void HandleBtbDealCallBack(PushTypeEnum actionType, string contentData)
        {
            // 内部交易
            if (actionType == PushTypeEnum.Create)
            {
                this.AddAndUpdateBTBDeal(JsonConvert.DeserializeObject<DtoBTBDeal>(contentData));
            }
        }

        #endregion

        #region DtoHedgeAccount

        /// <summary>
        /// 对冲账户推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoHedgeAccount")]
        private void HandleHedgeAccountCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoHedgeAccount
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                this.AddAndUpdateHedgeAccount(JsonConvert.DeserializeObject<DtoHedgeAccount>(contentData));
            }
        }

        #endregion

        #region DtoHedgeDeal

        /// <summary>
        /// 对冲交易账户推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoHedgeDeal")]
        private void HandleHedgeDealCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoHedgeAccount
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                this.AddAndUpdateHedgeDeal(JsonConvert.DeserializeObject<DtoHedgeDeal>(contentData));
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.DeleteHedgeDeal(JsonConvert.DeserializeObject<DtoHedgeDeal>(contentData));
            }
        }

        #endregion

        #region DtoCredit

        /// <summary>
        /// 授信推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCredit")]
        private void HandleCreditCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoCredit
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                this.AddAndUpdateCredit(JsonConvert.DeserializeObject<DtoCredit>(contentData));
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.RemoveCredit(JsonConvert.DeserializeObject<DtoCredit>(contentData));
            }
        }

        #endregion

        #region DtoCustomer

        /// <summary>
        /// 客户推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoCustomer")]
        private void HandleCustomerCallBack(PushTypeEnum actionType, string contentData)
        {
            // DtoCustomer
            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                this.AddCustomer(JsonConvert.DeserializeObject<DtoCustomer>(contentData));
            }
        }

        #endregion

        #region DtoVIPCustSettings

        /// <summary>
        /// Vip账户设置推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoVIPCustSettings")]
        private void HandleVipCustSettingsCallBack(PushTypeEnum actionType, string contentData)
        {
            switch (actionType)
            {
                case PushTypeEnum.Create:
                case PushTypeEnum.Update:
                    this.AddAndUpdateVipSettings(JsonConvert.DeserializeObject<DtoVIPCustSettings>(contentData));
                    break;
                case PushTypeEnum.Delete:
                    this.DeleteVipSettings(JsonConvert.DeserializeObject<DtoVIPCustSettings>(contentData));
                    break;
            }
        }

        #endregion

        #region DtoAdHocFee

        /// <summary>
        /// 费用单推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoAdHocFee")]
        private void HandleAdHocFeeCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create)
            {
                var dtoAdHocFeeDeal = JsonConvert.DeserializeObject<DtoAdHocFee>(contentData);
                this.AddAndUpdateAdHocFee(dtoAdHocFeeDeal);
                RunTime.GetDealCreatedNotifier().NotifyAdHocFeeCreated((AdHocFeeModel)dtoAdHocFeeDeal);
            }
            else if (actionType == PushTypeEnum.Update)
            {
                this.AddAndUpdateAdHocFee(JsonConvert.DeserializeObject<DtoAdHocFee>(contentData));
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.DeleteAdHocFee(JsonConvert.DeserializeObject<DtoAdHocFee>(contentData));
            }
        }

        #endregion

        #region DtoInternalAcctTransfer

        /// <summary>
        /// 内部转账单推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoInternalAcctTransfer")]
        private void HandleInternalAcctCallBack(PushTypeEnum actionType, string contentData)
        {
            if (actionType == PushTypeEnum.Create)
            {
                var dtoAdHocFeeDeal = JsonConvert.DeserializeObject<DtoInternalAcctTransfer>(contentData);
                this.AddAndUpdateInternalAcctTransfer(dtoAdHocFeeDeal);
                RunTime.GetDealCreatedNotifier().NotifyInternalTransferCreated((InternalAcctTransferModel)dtoAdHocFeeDeal);
            }
            else if (actionType == PushTypeEnum.Update)
            {
                this.AddAndUpdateInternalAcctTransfer(JsonConvert.DeserializeObject<DtoInternalAcctTransfer>(contentData));
            }
            else if (actionType == PushTypeEnum.Delete)
            {
                this.DeleteInternalAcctTransfer(JsonConvert.DeserializeObject<DtoInternalAcctTransfer>(contentData));
            }
        }

        #endregion

        #region DtoForwardPointTenor

        /// <summary>
        /// 远期Tenor推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoForwardPointTenor")]
        private void HandleForwardPointTenorCallBack(PushTypeEnum actionType, string contentData)
        {
            this.UpdateForwardPoint(JsonConvert.DeserializeObject<DtoForwardPointTenor>(contentData));
        }

        #endregion

        #region DtoDealingLog

        /// <summary>
        /// 交易日志推送处理
        /// </summary>
        /// <param name="actionType">
        /// 操作类型
        /// </param>
        /// <param name="contentData">
        /// 内容数据
        /// </param>
        [CallBackHandle("DtoDealingLog")]
        private void HandleDealingLogCallBack(PushTypeEnum actionType, string contentData)
        {
            this.AddDealingLog(JsonConvert.DeserializeObject<DtoDealingLog>(contentData));
        }

        #endregion

        #region DtoStaffEvent

        /// <summary>
        /// 员工提醒推送处理
        /// </summary>
        /// <param name="actionType">操作类型</param>
        /// <param name="contentData">内容数据</param>
        [CallBackHandle("DtoStaffEvent")]
        private void HandleStaffEventCallBack(PushTypeEnum actionType, string contentData)
        {
            BaseStaffEventVM baseStaffEventVm = BaseStaffEventVM.Factory(contentData);
            if (baseStaffEventVm == null || baseStaffEventVm.StaffId != ServiceRunTime.Instance.OperatorId)
            {
                return;
            }

            if (actionType == PushTypeEnum.Create || actionType == PushTypeEnum.Update)
            {
                this.GetRepository<IStaffEventCacheRepository>().AddOrUpdate(baseStaffEventVm);
            }
            else
            {
                this.GetRepository<IStaffEventCacheRepository>().Remove(baseStaffEventVm.GetID());
            }
        }

        #endregion

        /// <summary>
        /// 根据账户编号判断是否在当前Staff的管辖范围内
        /// </summary>
        /// <param name="custNo">账户编号</param>
        /// <returns>返回true 为该账户在当前的Staff管辖范围</returns>
        private bool IsUnderStaffManage(string custNo)
        {
            var custReps = this.GetRepository<ICustomerCacheRepository>();
            var findCust = custReps.FindByID(custNo);

            if (findCust == null)
            {
                return false;
            }

            return true;
        }
    }
}
