// <copyright file="RequestCallBackModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2015/08/07 01:35:26 </date>
// <summary> 请求回调处理 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2015/08/07 01:35:26
//      修改描述：新建 RequestCallBackModel.cs
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
    using System.Collections;
    using System.Linq;

    using BaseViewModel;

    using Caliburn.Micro;

    using DestributeService.Command;
    using DestributeService.Dto;
    using DestributeService.Seedwork;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Models;
    using Infrastructure.Service;
    using Infrastructure.Log;
    using Microsoft.Practices.ObjectBuilder2;
    using GalaSoft.MvvmLight.Messaging;
    using Infrastructure.Common;

    #endregion

    /// <summary>
    /// 请求服务推送处理类
    /// </summary>
    public class RequestCallBackModel : BaseModel
    {
        #region Fields
        /// <summary>
        /// 请求缓存仓储
        /// </summary>
        private readonly IRequestCacheRepository requestReps;

        ///// <summary>
        ///// 当前请求缓存仓储
        ///// </summary>
        //private readonly ICurrentRequestCacheRepository currentRequestReps;

        ///// <summary>
        ///// 历史请求缓存仓储
        ///// </summary>
        //private readonly IHistoryRequestCacheRepository historyRequestReps;

        /// <summary>
        /// 客户信息缓存仓储
        /// </summary>
        private readonly ICustomerCacheRepository customerReps;

        /// <summary>
        /// 请求服务
        /// </summary>
        private readonly RequestPriceService requestService;

        /// <summary>
        /// 运行实例
        /// </summary>
        private RunTime runtime;

        /// <summary>
        /// IEventAggregator事件
        /// </summary>
        private IEventAggregator eventAggregator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestCallBackModel"/> class. 
        /// </summary>
        /// <param name="ownerId">
        /// 拥有者ID
        /// </param>
        public RequestCallBackModel(string ownerId)
            : base(ownerId)
        {
            this.runtime = RunTime.GetCurrentRunTime(ownerId);
            this.eventAggregator = /*RunTime.GetCurrentRunTime(this.OwnerId)*/this.runtime.GetCurrentEventAggregator();
            this.requestReps = this.GetRepository<IRequestCacheRepository>();
            //this.currentRequestReps = new CurrentRequestCacheRepository(ownerId);
            //this.historyRequestReps = new HistoryRequestCacheRepository(ownerId);
            this.requestService = new RequestPriceService(ownerId);
            this.customerReps = this.GetRepository<ICustomerCacheRepository>();

            //requestReps.SubscribeAddEvent(
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 接受请求的推送
        /// </summary>
        /// <param name="backContextType">
        /// 推送的类型
        /// </param>
        /// <param name="backContext">
        /// 推送数据
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public void PushBack(RequestPushTypeEnum backContextType, RequestModel backContext, DtoResultParas result)
        {
            var request = new BaseRequestVM(backContext);

            switch (backContextType)
            {

                case RequestPushTypeEnum.DistributeRequest:
                    this.HandleDistributeRequest(request);
                    break;
                ////TODO..
                case RequestPushTypeEnum.RequestResult:
                    this.HandleCompleteRequest(request);

                    // 请求顺利，没有出现错误或异常，会走以下逻辑交互
                    if ((backContext.Status == ExecuteFlagEnum.Success || backContext.Status == ExecuteFlagEnum.DealingComplete) && (result == null || result.result == true))
                    {
                        if (RunTime.GetCurrentRunTime(this.OwnerId).CheckStaffIsDealer() == false)
                        {
                            if (request.Deal[0].RequestBackFlag == RequestBackFlagEnum.CloseOut)
                            {
                                // 处理前台接到的交易请求的结果
                                Messenger.Default.Send<BaseRequestVM>(request, "RequestCallBack_CloseOut");
                            }
                            else if (request.Deal != null && request.Deal.Count == 2 && request.TradeInstrument == TradInstrumentEnum.FXRolloverSwap && (string.IsNullOrWhiteSpace(request.Deal[0].RelatedDealID) == false || string.IsNullOrWhiteSpace(request.Deal[1].RelatedDealID) == false))
                            {
                                // 按单展期Swap的情况
                                Messenger.Default.Send<BaseRequestVM>(request, "RequestCallBack_AddSwapByDeal");
                            }
                            else if (request.Deal != null && request.Deal.Count == 2 && request.TradeInstrument == TradInstrumentEnum.FXSwap)
                            {
                                Messenger.Default.Send<BaseRequestVM>(request, "RequestCallBack_AddSwap");
                            }
                            else if (request.Deal != null && request.Deal.Count == 2 && request.TradeInstrument == TradInstrumentEnum.FXRolloverSwap && string.IsNullOrWhiteSpace(request.Deal[0].RelatedDealID) == true && string.IsNullOrWhiteSpace(request.Deal[1].RelatedDealID) == true)
                            {
                                Messenger.Default.Send<BaseRequestVM>(request, "RequestCallBack_AddSwapByPos");
                            }
                            else
                            {
                                Messenger.Default.Send<BaseRequestVM>(request, "RequestCallBack_SpotForward");
                            }
                        }

                        break;
                    }

                    #region 下单或询价失败了
                    string errorMsgKey = string.Empty;

                    if (backContext.Status == ExecuteFlagEnum.TimeOut)
                    {
                        errorMsgKey = "RequestTimeOut";
                    }
                    else if (backContext.Status == ExecuteFlagEnum.ThrowBackCountOut)
                    {
                        errorMsgKey = "RequestFail";
                    }
                    else if (backContext.Status == ExecuteFlagEnum.Rejected)
                    {
                        errorMsgKey = "RequestFail";
                    }
                    else if (backContext.Status == ExecuteFlagEnum.NoUserOnline)
                    {
                        errorMsgKey = "NoDealerOnline";
                    }
                    else if (backContext.Status == ExecuteFlagEnum.DealingComplete && result != null && result.result == false)
                    {
                        errorMsgKey = result.promptStr;
                    }

                    if (string.IsNullOrWhiteSpace(errorMsgKey) == false)
                    {
                        // 处理前台接到的下单失败的原因
                        if (request.PropSet.TradeInstrument == TradInstrumentEnum.FXSpot || request.PropSet.TradeInstrument == TradInstrumentEnum.FXForward)
                        {
                            if (request.Deal[0].RequestBackFlag == RequestBackFlagEnum.CloseOut)
                            {
                                Messenger.Default.Send<string>(errorMsgKey, "AddDealFail_CloseOut");
                            }
                            else
                            {
                                Messenger.Default.Send<string>(errorMsgKey, "AddDealFail");
                            }
                        }
                        else if (request.PropSet.TradeInstrument == TradInstrumentEnum.FXSwap)
                        {
                            Messenger.Default.Send<string>(errorMsgKey, "AddSwapDealFail");
                        }
                        else if (string.IsNullOrWhiteSpace(request.Deal[0].RelatedDealID) == true && string.IsNullOrWhiteSpace(request.Deal[1].RelatedDealID) == true)
                        {
                            Messenger.Default.Send<string>(errorMsgKey, "AddRolloverSwapPosFail");
                        }
                        else
                        {
                            Messenger.Default.Send<string>(errorMsgKey, "AddRolloverSwapDealFail");
                        }
                    }
                    #endregion

                    break;
                case RequestPushTypeEnum.CompleteRequest:
                    this.HandleCompleteRequest(request);
                    break;
                case RequestPushTypeEnum.NewRequest:
                case RequestPushTypeEnum.TimeWarnRequest:
                case RequestPushTypeEnum.ThrowBackRequest:
                    // 这些状态相关的处理都直接通过订阅仓储状态变化进行界面显示响应
                    break;
                default:
                    throw new ArgumentOutOfRangeException("backContextType");
            }


            request.RequestMsg = this.Translation(backContext);
            this.requestReps.AddOrUpdate(request);
        }

        /// <summary>
        /// 翻译成标题
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <returns>标题的字符串形式</returns>
        public string Translation(RequestModel request)
        {
            try
            {
                if (request.RequestType == RequestTypeEnum.InquiryRequest)
                {
                    return this.GetInquiryRequestTitle(request);
                }
                else if (request.RequestType == RequestTypeEnum.Immediately)
                {
                    return this.GetImmediatelyRequestTitle(request);
                }
                else if (request.RequestType == RequestTypeEnum.DealConfirm)
                {
                    return this.GetDealerConfimTitle(request);
                }
                else if (request.RequestType == RequestTypeEnum.RolloverSwapFreeMarginNotEnough)
                {
                    return this.GetRolloverSwapFreeMarginNotEnoughTitle(request);
                }
            }
            catch (Exception ex)
            {
                TraceManager.Error.Write("RequestCallBackModel", ex, "genral");

                return string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// 询价请求
        /// </summary>
        private string GetInquiryRequestTitle(RequestModel request)
        {
            if (request.Deal == null || request.Deal.Count == 0)
                return string.Empty;

            var currency = this.GetRepository<ICurrencyCacheRepository>().FindByID(request.CCY);
            var symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(request.Symbol);
            switch (request.TradeInstrument)
            {
                case TradInstrumentEnum.FXSpot:
                case TradInstrumentEnum.FXForward:
                    return string.Format("Quote for we {0} {1} {2} ({3}), Value {4}, {5}",
                        request.TransactionType == RequestTransTypeEnum.Buy ? "Sell" : "Buy",
                        currency.CurrencyName,
                        request.Amount.ToString("N" + currency.AmountDecimals),
                        symbol.SymbolName,
                        GetTenorString(request.Deal[0].Tenor),
                        request.Deal[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty));
                case TradInstrumentEnum.FXSwap:
                case TradInstrumentEnum.FXRolloverSwap:
                    return string.Format("Quote for we {0} {1} {2} ({3}), Value {4} vs {5}",
                        request.TransactionType == RequestTransTypeEnum.BuySell ? "Sell-Buy" : "Buy-Sell",
                        currency.CurrencyName,
                        request.Amount.ToString("N" + currency.AmountDecimals),
                        symbol.SymbolName,
                        request.Deal[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty),
                        request.Deal[1].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty));
            }

            return string.Empty;
        }

        /// <summary>
        /// 市价请求
        /// </summary>
        private string GetImmediatelyRequestTitle(RequestModel request)
        {
            if (request.ClientRequestInfo == null || request.ClientRequestInfo.Count != 2)
                return string.Empty;

            var currency = this.GetRepository<ICurrencyCacheRepository>().FindByID(request.CCY);
            var symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(request.Symbol);

            ClientRequestInfoModel clientRequest;
            if (request.TransactionType == RequestTransTypeEnum.Buy || request.TransactionType == RequestTransTypeEnum.BuySell)
            {
                clientRequest = request.ClientRequestInfo[0];
            }
            else
            {
                clientRequest = request.ClientRequestInfo[1];
            }

            switch (request.TradeInstrument)
            {
                case TradInstrumentEnum.FXSpot:
                case TradInstrumentEnum.FXForward:
                    return string.Format("We {0} {1} {2} ({3}) at {4}(cost rate {5}), Value {6}, {7}",
                        request.TransactionType == RequestTransTypeEnum.Buy ? "Sell" : "Buy",
                        currency.CurrencyName,
                        request.Amount.ToString("N" + currency.AmountDecimals),
                        symbol.SymbolName,
                        clientRequest.TraderRate,
                        clientRequest.TraderSpotRate,
                        GetTenorString(request.Deal[0].Tenor),
                        request.Deal[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty));
                case TradInstrumentEnum.FXSwap:
                case TradInstrumentEnum.FXRolloverSwap:
                    decimal tradeNearLeg;

                    if (request.Deal[0].Tenor == TenorEnum.ON || request.Deal[0].Tenor == TenorEnum.TN)
                    {
                        tradeNearLeg = clientRequest.TraderSpotRate - clientRequest.NearLegFwdPoint.SpreadToDecimal(symbol.BasisPoint);
                    }
                    else
                    {
                        tradeNearLeg = clientRequest.TraderSpotRate + clientRequest.NearLegFwdPoint.SpreadToDecimal(symbol.BasisPoint);
                    }

                    int place = request.Deal[0].Tenor == TenorEnum.SP ? symbol.DecimalPlace : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;

                    tradeNearLeg = tradeNearLeg.FormatPriceBySymbolPoint(place);

                    return string.Format("We {0} {1} {2} ({3}) at {4}/{5} ({6} {7}/{8}),  Value {9} vs {10}",
                        request.TransactionType == RequestTransTypeEnum.BuySell ? "Sell-Buy" : "Buy-Sell",
                        currency.CurrencyName,
                        request.Amount.ToString("N" + currency.AmountDecimals),
                        symbol.SymbolName,
                        clientRequest.TraderRate,
                        tradeNearLeg,
                        clientRequest.TraderSpotRate,
                        clientRequest.NearLegFwdPoint,
                        clientRequest.FarLegFwdPoint,
                        request.ClientRequestInfo[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty),
                        request.ClientRequestInfo[1].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty));
            }

            return string.Empty;
        }

        /// <summary>
        /// 订单确认
        /// </summary>
        private string GetDealerConfimTitle(RequestModel request)
        {
            AnswerInfoModel bid = null;
            AnswerInfoModel ask = null;

            if (request.AnswerInfo == null || request.AnswerInfo.Count == 0)
                return string.Empty;

            switch (request.TradeInstrument)
            {
                case TradInstrumentEnum.FXSpot:
                case TradInstrumentEnum.FXForward:
                    bid = request.AnswerInfo.First(p => p.Type == AnswerTypeEnum.Sell);
                    ask = request.AnswerInfo.First(p => p.Type == AnswerTypeEnum.Buy);
                    break;
                case TradInstrumentEnum.FXSwap:
                case TradInstrumentEnum.FXRolloverSwap:
                    bid = request.AnswerInfo.First(p => p.Type == AnswerTypeEnum.SellBuy);
                    ask = request.AnswerInfo.First(p => p.Type == AnswerTypeEnum.BuySell);
                    break;
            }

            var currency = this.GetRepository<ICurrencyCacheRepository>().FindByID(request.CCY);
            var symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(request.Symbol);

            switch (request.TradeInstrument)
            {
                case TradInstrumentEnum.FXSpot:
                case TradInstrumentEnum.FXForward:
                    return string.Format("We {0} {1} {2} ({3}) at {4}(cost rate {5}), Value {6}, {7}",
                        request.TransactionType == RequestTransTypeEnum.Buy ? "Sell" : "Buy",
                        currency.CurrencyName,
                        request.Amount.ToString("N" + currency.AmountDecimals),
                        symbol.SymbolName,
                        (request.TransactionType == RequestTransTypeEnum.Buy ? ask : bid).DefaultTraderRate,
                        request.Deal[0].TraderSpotRate,
                        GetTenorString(request.Deal[0].Tenor),
                        request.Deal[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty));
                case TradInstrumentEnum.FXSwap:
                case TradInstrumentEnum.FXRolloverSwap:
                    var replyPrice = request.TransactionType == RequestTransTypeEnum.BuySell ? ask : bid;

                    decimal tradeNearLeg;
                    if (request.Deal[0].Tenor == TenorEnum.ON || request.Deal[0].Tenor == TenorEnum.TN)
                    {
                        tradeNearLeg = replyPrice.DefaultTraderSpotRate - replyPrice.DefaultNearLegFwdPoint;
                    }
                    else
                    {
                        tradeNearLeg = replyPrice.DefaultTraderSpotRate + replyPrice.DefaultNearLegFwdPoint;
                    }

                    int place = request.Deal[0].Tenor == TenorEnum.SP ? symbol.DecimalPlace : symbol.BasisPoint + symbol.ForwardPointDecimalPlace;
                    tradeNearLeg = tradeNearLeg.FormatPriceBySymbolPoint(place);

                    return string.Format("We {0} {1} {2} ({3}) at {4}/{5} ({6} {7}/{8}),  Value {9} vs {10}",
                        request.TransactionType == RequestTransTypeEnum.BuySell ? "Sell-Buy" : "Buy-Sell",
                        currency.CurrencyName,
                        request.Amount.ToString("N" + currency.AmountDecimals),
                        symbol.SymbolName,
                        replyPrice.DefaultTraderRate,
                        tradeNearLeg,
                        replyPrice.DefaultTraderSpotRate,
                        replyPrice.DefaultNearLegFwdPoint,
                        replyPrice.DefaultFarLegFwdPoint,
                        request.Deal[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty),
                        request.Deal[1].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty));
            }

            return string.Empty;
        }

        /// <summary>
        /// 保证金不足的展期显示标题
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <returns>返回标题字符串</returns>
        private string GetRolloverSwapFreeMarginNotEnoughTitle(RequestModel request)
        {
            var symbolName = GetRepository<ISymbolCacheRepository>().GetName(request.Symbol);
            var ccyName = GetRepository<ICurrencyCacheRepository>().GetName(request.CCY);

            return
                string.Format(
                    "Rollover we {0} {1} {2}({3}) Value {4} to {5} at {6}/{7} Margin required {8}, Free Margin {9}",
                    request.TransactionType,
                    ccyName,
                    request.Amount.ToString("F"),
                    symbolName,
                    request.Deal[0].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty),
                    request.Deal[1].ValueDate.FormatDateTimeByBuID(runtime.CurrentStaff.StaffBaseInfo.BusinessUnitID, string.Empty),
                    request.AnswerInfo[0].AllInRate,
                    request.AnswerInfo[0].TraderNearLegRate,
                    request.AnswerInfo[0].TraderSpotRate,
                    request.AnswerInfo[0].NearLegFwdPoint);
        }

        /// <summary>
        /// 获取远期类型的字符串形式
        /// </summary>
        /// <param name="tenor">远期类型</param>
        /// <returns>远期类型的字符串形式</returns>
        private string GetTenorString(TenorEnum tenor)
        {
            string info = string.Empty;

            switch (tenor)
            {
                case TenorEnum.BD:
                    info = "Broken Dates";
                    break;
                case TenorEnum.M1:
                    info = "1M";
                    break;
                case TenorEnum.M10:
                    info = "10M";
                    break;
                case TenorEnum.M11:
                    info = "11M";
                    break;
                case TenorEnum.M2:
                    info = "2M";
                    break;
                case TenorEnum.M3:
                    info = "3M";
                    break;
                case TenorEnum.M4:
                    info = "4M";
                    break;
                case TenorEnum.M5:
                    info = "5M";
                    break;
                case TenorEnum.M6:
                    info = "6M";
                    break;
                case TenorEnum.M7:
                    info = "7M";
                    break;
                case TenorEnum.M8:
                    info = "8M";
                    break;
                case TenorEnum.M9:
                    info = "9M";
                    break;
                case TenorEnum.ON:
                    info = "ON";
                    break;
                case TenorEnum.SN:
                    info = "SN";
                    break;
                case TenorEnum.SP:
                    info = "SP";
                    break;
                case TenorEnum.TN:
                    info = "TN";
                    break;
                case TenorEnum.W1:
                    info = "1W";
                    break;
                case TenorEnum.W2:
                    info = "2W";
                    break;
                case TenorEnum.W3:
                    info = "3W";
                    break;
                case TenorEnum.Y1:
                    info = "1Y";
                    break;
                default:
                    break;
            }

            return info;
        }

        /// <summary>
        /// 分配请求处理
        /// </summary>
        /// <param name="newValue">新的请求</param>
        private void HandleDistributeRequest(BaseRequestVM newValue)
        {
            bool automation = false;

            ////当启用自动化配置及启用请求配置
            if (this.runtime.CurrentDealerConfig.Automation)
            {
                var customer = this.customerReps.FindByID(newValue.CustomerNo);

                if (newValue.RequestType == RequestTypeEnum.InquiryRequest)
                {
                    automation = this.AutomaticProcessingRequest(
                            customer.Account,
                            this.runtime.CurrentDealerConfig.RequestAutomationConfig,
                            newValue.PropSet);
                }
                else if (newValue.RequestType == RequestTypeEnum.DealConfirm)
                {
                    automation = this.AutomaticProcessingRequest(
                            customer.Account,
                            this.runtime.CurrentDealerConfig.ConfirmationAutomationConfig,
                            newValue.PropSet);
                }
                else if (newValue.RequestType == RequestTypeEnum.Immediately)
                {
                    automation = this.AutomaticProcessingRequest(
                            customer.Account,
                            this.runtime.CurrentDealerConfig.BuySellMarketAutomationConfig,
                            newValue.PropSet);
                }
            }


            newValue.DealerManual = !automation;
            newValue.DealerAuto = automation;
        }

        /// <summary>
        /// 是否自动处理请求
        /// </summary>
        /// <param name="customer">客户信息</param>
        /// <param name="requestConfig">配置信息</param>
        /// <param name="request">请求信息</param>
        private bool AutomaticProcessingRequest(
            CustomerModel customer,
            AutomationConfigModel requestConfig,
            RequestModel request)
        {
            if (requestConfig == null || !requestConfig.Enabled)
                return false;

            var localCCY = base.GetRepository<ICurrencyCacheRepository>().GetName(customer.BaseInfo.LocalCCY);
            var currencySetting = requestConfig.CurrencySettings.FirstOrDefault(p => p.Currency == localCCY);

            if (currencySetting == null || currencySetting.MaxAmount <= 0)
            {
                return false;
            }

            string prompt;
            var valueDate = ValueDateCore.Instance.GetValueDateByTenor(customer.BaseInfo.BusinessUnitID, requestConfig.Tenor, request.Symbol, out prompt);

            switch (request.TradeInstrument)
            {
                case TradInstrumentEnum.FXRolloverSwap:
                case TradInstrumentEnum.FXSwap:
                    if (request.Deal[0].ValueDate.Date > valueDate.Date || request.Deal[0].PerDealPosition > currencySetting.MaxAmount
                        || request.Deal[1].ValueDate.Date > valueDate.Date || request.Deal[1].PerDealPosition > currencySetting.MaxAmount)
                    {
                        return false;
                    }
                    break;
                case TradInstrumentEnum.FXSpot:
                case TradInstrumentEnum.FXForward:
                    if (request.Deal[0].ValueDate.Date > valueDate.Date || request.Deal[0].PerDealPosition > currencySetting.MaxAmount)
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            request.Status = ExecuteFlagEnum.Success;
            this.requestService.Replay(request, true);
            return true;
        }

        /// <summary>
        /// 请求完成处理
        /// </summary>
        /// <param name="newValue">新的请求</param>
        private void HandleCompleteRequest(BaseRequestVM newValue)
        {
            newValue.Completed = true;
        }

        #endregion

        ////TODO：此处的处理待整理
        #region Methods

        /// <summary>
        /// 推送 添加挂单请求
        /// </summary>
        /// <param name="request">
        /// 请求
        /// </param>
        private void AddOrderRequest(DtoPendingOrder request)
        {
            // 获取挂单仓储
            var orderReps = this.GetRepository<IOrderCacheRepository>();

            var req = (PendingOrderModel)request;
            req.StaffID = RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo.StaffID;
            BaseOrderVM baseOrderVM = new BaseOrderVM(req);
            baseOrderVM.IsActiveOrder = true;

            // 添加或更新
            orderReps.AddOrUpdate(baseOrderVM);
        }

        #endregion
    }
}