// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewFinancialHedgeSpotForwardViewModel.cs" company="">
//   
// </copyright>
// <author>fangz</author>
// <date> 2017/05/18 07:14:02 </date>
// <modify>
//      修改人：fangz
//      修改时间：2017/05/18 07:14:02
//      修改描述：新建 NewFinancialHedgeSpotForwardViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels.Deal
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Command;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;

    using Microsoft.Practices.ObjectBuilder2;

    #endregion

    /// <summary>
    ///     直通交易VM
    /// </summary>
    public class NewFinancialHedgeSpotForwardViewModel : FxHedgingDealModel
    {
        #region Fields

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     报价信息
        /// </summary>
        private readonly PriceItemModel priceInfo;

        /// <summary>
        ///     可选的日历交割日列表
        /// </summary>
        private readonly ObservableCollection<CalendarDateRange> valuedateCanSelectInCalendar =
            new ObservableCollection<CalendarDateRange>();

        /// <summary>
        ///     询价后客户应答时间
        /// </summary>
        private int answeringTime;

        /// <summary>
        ///     ask报价块高亮
        /// </summary>
        private bool askHight;

        /// <summary>
        ///     在选择数据源中的bd项
        /// </summary>
        private ValueDateItemModel bdvaluedateItemInSelectSource;

        /// <summary>
        ///     所属商品
        /// </summary>
        private ContractModel belongContract;

        /// <summary>
        ///     bid报价块高亮
        /// </summary>
        private bool bidHight;

        /// <summary>
        ///     所属Bu
        /// </summary>
        private BusinessUnitModel businessUnitModel;

        /// <summary>
        ///     日历选择时间
        /// </summary>
        private DateTime calendarSelectDate;

        /// <summary>
        ///     选择的交易对手
        /// </summary>
        private CounterPartyModel counterParty;

        /// <summary>
        ///     所属企业
        /// </summary>
        private EnterpriseModel currentEnterprise;

        /// <summary>
        ///     The value date.
        /// </summary>
        /// <summary>
        ///     The we buy amount.
        /// </summary>
        private decimal custBuyAmount;

        /// <summary>
        ///     The we buy ccy.
        /// </summary>
        private CurrencyModel custBuyCcy;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal custSellAmount;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private CurrencyModel custSellCcy;

        /// <summary>
        ///     是否启用客户买入金额对话框
        /// </summary>
        private bool enaleCustBuyAmount;

        /// <summary>
        ///     是否启用客户卖出金额对话框
        /// </summary>
        private bool enaleCustSellAmount;

        /// <summary>
        ///     The is we buy amount enable.
        /// </summary>
        private bool isCustBuyAmountEnable = true;

        /// <summary>
        ///     The is we sell amount enable.
        /// </summary>
        private bool isCustSellAmountEnable = true;

        /// <summary>
        ///     是否手写买入货币金额
        /// </summary>
        private bool isDealtBuyCCY;

        /// <summary>
        ///     是否弹出显示日历选择控件
        /// </summary>
        private bool isShowCalender;

        /// <summary>
        ///     是否开始倒计时
        /// </summary>
        private bool isStart;

        /// <summary>
        ///     市场汇率
        /// </summary>
        private PriceItemModel marketrateInfo;

        /// <summary>
        ///     下单按钮显示控制
        /// </summary>
        private Visibility placeVisibility;

        /// <summary>
        ///     询价按钮显示控制
        /// </summary>
        private Visibility requestVisibility;

        /// <summary>
        ///     选中交割日
        /// </summary>
        private ValueDateItemModel selectValueDate;

        /// <summary>
        /// The sell account.
        /// </summary>
        private SellBankAcctBalanceVM sellAccount;

        /// <summary>
        ///     可选交割日的结束时间
        /// </summary>
        private DateTime valueDateSelectEndDay;

        /// <summary>
        ///     可选交割日的起始时间
        /// </summary>
        private DateTime valueDateSelectStartDay;

        /// <summary>
        ///     交割日列表
        /// </summary>
        private ObservableCollection<ValueDateItemModel> valueDatesForSelect =
            new ObservableCollection<ValueDateItemModel>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewFinancialHedgeSpotForwardViewModel"/> class.
        /// </summary>
        /// <param name="varPriceInfo">
        /// The var Price Info.
        /// </param>
        /// <param name="isBuy">
        /// The is Buy.
        /// </param>
        public NewFinancialHedgeSpotForwardViewModel(PriceItemModel varPriceInfo, bool isBuy)
        {
            this.DisplayName = RunTime.FindStringResource("NewFinancialHedgeDeal");
            this.priceInfo = varPriceInfo;
            this.BelongContract = varPriceInfo.OwnerBlock.BelongContract;
            this.currencyRep = this.GetRepository<ICurrencyRepository>();
            this.currentEnterprise =
                this.GetRepository<IEnterpriseRepository>().FindByID(RunTime.GetCurrentRunTime().CurrentLoginUser.EntId);
            if (isBuy)
            {
                this.CustBuyCcy = this.currencyRep.FindByID(this.priceInfo.OwnerBlock.BelongContract.Ccy1Id);
                this.CustSellCcy = this.currencyRep.FindByID(this.priceInfo.OwnerBlock.BelongContract.Ccy2Id);
            }
            else
            {
                this.CustSellCcy = this.currencyRep.FindByID(this.priceInfo.OwnerBlock.BelongContract.Ccy1Id);
                this.CustBuyCcy = this.currencyRep.FindByID(this.priceInfo.OwnerBlock.BelongContract.Ccy2Id);
            }

            this.CounterParty = this.priceInfo.BelongCounterPartyModel;

            this.InitialValueDates();
            this.Reset();
            this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(this.CounterParty.BusinessUnitId);
            this.SetSellAcount();
            this.GetRepository<IFxHedgingDealRepository>().SubscribeAddEvent(
                (newDeal) => this.SetSellAcount());
            this.GetRepository<IFxHedgingDealRepository>().SubscribeRemoveEvent(
                (newDeal) => this.SetSellAcount());
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     询价后客户应答时间
        /// </summary>
        public int AnsweringTime
        {
            get
            {
                return this.answeringTime;
            }

            set
            {
                this.answeringTime = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     ask报价块高亮
        /// </summary>
        public bool AskHight
        {
            get
            {
                return this.askHight;
            }

            set
            {
                this.askHight = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     所属商品
        /// </summary>
        public ContractModel BelongContract
        {
            get
            {
                return this.belongContract;
            }

            set
            {
                this.belongContract = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     bid报价块高亮
        /// </summary>
        public bool BidHight
        {
            get
            {
                return this.bidHight;
            }

            set
            {
                this.bidHight = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     日历选择时间
        /// </summary>
        public DateTime CalendarSelectDate
        {
            get
            {
                return this.calendarSelectDate;
            }

            set
            {
                this.calendarSelectDate = value;
                this.NotifyOfPropertyChange();
                this.HandleCalenderSelectDateChange();
            }
        }

        /// <summary>
        ///     选中CounterParty
        /// </summary>
        public CounterPartyModel CounterParty
        {
            get
            {
                return this.counterParty;
            }

            set
            {
                this.counterParty = value;
                this.NotifyOfPropertyChange(() => this.CounterParty);
            }
        }

        /// <summary>
        ///     CounterParty 选中事件
        /// </summary>
        public RelayCommand CounterPartySelectionChangedCommand { get; private set; }

        /// <summary>
        ///     CCY2Amount
        /// </summary>
        public string CustBuyAmount
        {
            get
            {
                return this.custBuyAmount.ToString();
            }

            set
            {
                decimal inSetValue;
                if (!decimal.TryParse(value, out inSetValue))
                {
                    inSetValue = 0;
                }

                this.custBuyAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.CustBuyAmount);
                this.RefreshCustSellAmount();
            }
        }

        /// <summary>
        ///     CustBuyCCY
        /// </summary>
        public CurrencyModel CustBuyCcy
        {
            get
            {
                return this.custBuyCcy;
            }

            set
            {
                this.custBuyCcy = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     客户卖出货币数量
        ///     禁止界面输入，完全由买入量进行计算得到
        /// </summary>
        public string CustSellAmount
        {
            get
            {
                return this.custSellAmount.ToString();
            }

            set
            {
                decimal inSetValue;
                if (!decimal.TryParse(value, out inSetValue))
                {
                    inSetValue = 0;
                }

                this.custSellAmount = inSetValue;
                this.NotifyOfPropertyChange();
                this.RefreshCustBuyAmount();
            }
        }

        /// <summary>
        ///     WeSellCCY
        /// </summary>
        public CurrencyModel CustSellCcy
        {
            get
            {
                return this.custSellCcy;
            }

            set
            {
                this.custSellCcy = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     是否启用客户买入金额对话框
        /// </summary>
        public bool EnaleCustBuyAmount
        {
            get
            {
                return this.enaleCustBuyAmount;
            }

            set
            {
                this.enaleCustBuyAmount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     是否启用客户卖出金额对话框
        /// </summary>
        public bool EnaleCustSellAmount
        {
            get
            {
                return this.enaleCustSellAmount;
            }

            set
            {
                this.enaleCustSellAmount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CCY2Amount 是否可用
        /// </summary>
        public bool IsCustBuyAmountEnable
        {
            get
            {
                return this.isCustBuyAmountEnable;
            }

            set
            {
                this.isCustBuyAmountEnable = value;
                this.NotifyOfPropertyChange(() => this.IsCustBuyAmountEnable);
            }
        }

        /// <summary>
        ///     CCY1Amount 是否可用
        /// </summary>
        public bool IsCustSellAmountEnable
        {
            get
            {
                return this.isCustSellAmountEnable;
            }

            set
            {
                this.isCustSellAmountEnable = value;
                this.NotifyOfPropertyChange(() => this.IsCustSellAmountEnable);
            }
        }

        /// <summary>
        ///     是否弹出显示日历选择控件
        /// </summary>
        public bool IsShowCalender
        {
            get
            {
                return this.isShowCalender;
            }

            set
            {
                this.isShowCalender = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     是否开始倒计时
        /// </summary>
        public bool IsStart
        {
            get
            {
                return this.isStart;
            }

            set
            {
                this.isStart = value;

                // 如果Manager倒计时
                if (value == false)
                {
                    this.MarketrateInfo = null;
                }

                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     市场汇率
        /// </summary>
        public PriceItemModel MarketrateInfo
        {
            get
            {
                return this.marketrateInfo;
            }

            set
            {
                this.marketrateInfo = value;
                this.NotifyOfPropertyChange();
                this.HandleMarkeRateChange();
            }
        }

        /// <summary>
        ///     可下单时显示控制
        /// </summary>
        public Visibility PlaceVisibility
        {
            get
            {
                return this.placeVisibility;
            }

            set
            {
                this.placeVisibility = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     需询价时显示控制
        /// </summary>
        public Visibility RequestVisibility
        {
            get
            {
                return this.requestVisibility;
            }

            set
            {
                this.requestVisibility = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     选中交割日
        /// </summary>
        public ValueDateItemModel SelectValueDate
        {
            get
            {
                return this.selectValueDate;
            }

            set
            {
                ////TODO:此处的处理是因为无法解决BD不在列表中却希望显示的问题，做到无法通过下拉选中BD
                if (value.ValueDate == DateTime.MinValue && value.Tenor == TenorEnum.BD)
                {
                    ////使用线程延时通知，解决直接通知Combox显示空的问题，显示空原因不明
                    ThreadPool.QueueUserWorkItem(arg => this.NotifyOfPropertyChange("SelectValueDate"));
                    return;
                }

                if (this.selectValueDate != null && this.selectValueDate.Tenor == TenorEnum.BD
                    && value.Tenor != TenorEnum.BD)
                {
                    this.selectValueDate.ValueDate = DateTime.MinValue;
                }

                this.selectValueDate = value;
                this.NotifyOfPropertyChange();
                this.HandleValueDateUpdate();
            }
        }

        /// <summary>
        ///     Gets or sets the sell account.
        /// </summary>
        public SellBankAcctBalanceVM SellAccount
        {
            get
            {
                return this.sellAccount;
            }

            set
            {
                this.sellAccount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     可选Valuedate的结束时间
        /// </summary>
        public DateTime ValueDateSelectEndDay
        {
            get
            {
                return this.valueDateSelectEndDay;
            }

            set
            {
                this.valueDateSelectEndDay = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     可选Valuedate的起始时间
        /// </summary>
        public DateTime ValueDateSelectStartDay
        {
            get
            {
                return this.valueDateSelectStartDay;
            }

            set
            {
                this.valueDateSelectStartDay = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     交割日列表
        /// </summary>
        public ObservableCollection<ValueDateItemModel> ValueDatesForSelect
        {
            get
            {
                return this.valueDatesForSelect;
            }

            set
            {
                this.valueDatesForSelect = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     可选的日历交割日列表
        /// </summary>
        public ObservableCollection<CalendarDateRange> ValuedateCanSelectInCalendar
        {
            get
            {
                return this.valuedateCanSelectInCalendar;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     弹出显示日历选择控件函数
        /// </summary>
        public void CalenderShow()
        {
            this.IsShowCalender = true;
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     下单请求
        /// </summary>
        public void Place()
        {
            if (!this.ValidateforSumbit())
            {
                RunTime.ShowInfoDialogWithoutRes(this.Error, string.Empty, this.OwnerId);
                return;
            }

            var hedgeDealVm = new FxHedgingDealModel();
            hedgeDealVm.HedgingDealId = Guid.NewGuid().ToString();
            hedgeDealVm.HedgingExecutionId = hedgeDealVm.HedgingDealId;
            hedgeDealVm.BusinessUnitId = this.counterParty.BusinessUnitId;
            hedgeDealVm.UserId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;
            hedgeDealVm.ValueDate = this.SelectValueDate.ValueDate;
            hedgeDealVm.LocalTradeDate = this.LocalTradeDate.Date;

            ////var instruments = InstrumentTool.GetInstruments(
            ////    TradableInstrumentEnum.FX_HEDGING_SPOT_FORWARD, 
            ////    this.currentEnterprise);
            ////if (instruments.Any())
            ////{
            ////    hedgeDealVm.Instrument = instruments.First().Key;
            ////}
            hedgeDealVm.Instrument = this.Instrument;
            hedgeDealVm.Comment = this.Comment;
            hedgeDealVm.CounterpartyId = this.priceInfo.BelongCounterPartyModel.Id;

            hedgeDealVm.Ccy1Id = this.priceInfo.OwnerBlock.BelongContract.Ccy1Id;
            hedgeDealVm.Ccy2Id = this.priceInfo.OwnerBlock.BelongContract.Ccy2Id;
            if (this.CustSellCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id)
            {
                hedgeDealVm.Ccy1Amount = this.custSellAmount;
                hedgeDealVm.Ccy2Amount = this.custBuyAmount;
                hedgeDealVm.Typing = TypingEnum.CCY2_AMOUNT;
            }
            else
            {
                hedgeDealVm.Ccy1Amount = this.custBuyAmount;
                hedgeDealVm.Ccy2Amount = this.custSellAmount;
                hedgeDealVm.Typing = TypingEnum.CCY1_AMOUNT;
            }

            hedgeDealVm.ContractId = this.priceInfo.OwnerBlock.BelongContract.Id;
            hedgeDealVm.TransactionType = this.CustSellCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                              ? TransactionTypeEnum.Sell
                                              : TransactionTypeEnum.Buy;
            hedgeDealVm.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            hedgeDealVm.StaffId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;
            hedgeDealVm.UserId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;

            hedgeDealVm.HedgingDealId = this.HedgingDealId;
            hedgeDealVm.HedgingExecutionId = this.HedgingDealId;
            hedgeDealVm.ContractRate = this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                           ? this.MarketrateInfo.Ask
                                           : this.MarketrateInfo.Bid;
            hedgeDealVm.DealerSpotRate = this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                             ? this.MarketrateInfo.Ask
                                             : this.MarketrateInfo.Bid;
            hedgeDealVm.ForwardPoint = 0;
            hedgeDealVm.IsNearLeg = (int)IsNearLegEnum.NONE;
            hedgeDealVm.InstitutionId = this.CounterParty.InstitutionId;
            string msg = this.BuilderMsg(hedgeDealVm);
            if (RunTime.ShowConfirmDialogWithoutRes(msg, string.Empty, this.OwnerId))
            {
                var service = new HedgingDealService(this.OwnerId);
                CmdResult drs = service.NewHedgeSpotForward(hedgeDealVm);
                if (drs.Success)
                {
                    this.Reset();
                    msg = RunTime.FindStringResource("MSG_00001");

                    RunTime.ShowSuccessInfoDialogWithoutRes(msg, string.Empty, this.OwnerId);
                }
                else
                {
                    RunTime.ShowFailInfoDialog(drs.ErrorCode, string.Empty, this.OwnerId);
                }
            }
        }

        /// <summary>
        ///     询价请求
        /// </summary>
        public void Request()
        {
            ////TODO:此处的验证需要进一步规划处理，当前只是验证了buyamount和sellamount值，并且没有使用同一验证方法，后期需要单独添加
            ////if (!this.ValidateforSumbit())
            ////{
            ////    return;
            ////}
            if (this.custBuyAmount <= decimal.Zero && this.custSellAmount <= decimal.Zero)
            {
                if (!this.ValidateforSumbit())
                {
                    RunTime.ShowInfoDialogWithoutRes(this.Error, string.Empty, this.OwnerId);
                    return;
                }
            }

            var info = new PriceItemModel(
                this.priceInfo.OwnerBlock,
                this.priceInfo.BelongCounterPartyModel,
                this.priceInfo.ConnectionType);
            info.SetAskNoNotify(this.priceInfo.Ask);
            info.SetBidNoNotify(this.priceInfo.Bid);
            this.MarketrateInfo = info;
            this.AnsweringTime = 10;
            this.IsStart = true;
        }

        /// <summary>
        ///     所选Valuedate变化
        /// </summary>
        public void SelectValueDateChange()
        {
            this.SetSellAcount();
        }

        /// <summary>
        ///     买入和卖出切换动作
        /// </summary>
        public void SwitchOver()
        {
            CurrencyModel oldBuy = this.custBuyCcy;
            this.CustBuyCcy = this.CustSellCcy;
            this.CustSellCcy = oldBuy;
            this.custBuyAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustBuyAmount);
            this.custSellAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustSellAmount);
            this.UpdatePriceHightSetting();
            this.SetSellAcount();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 设置sell货币账户余额
        /// </summary>
        private void SetSellAcount()
        {
            this.SellAccount = new SellBankAcctBalanceVM();
            this.SellAccount.Currency = this.CustSellCcy;
            var tempBankAccts = new List<BankAccountModel>();
            this.CounterParty.SettlementAccounts.ForEach(
                s =>
                {
                    BankAccountModel bankAcct =
                        this.GetRepository<IBankAccountRepository>().FindByID(s.Value.ToString());
                    if (bankAcct != null)
                    {
                        tempBankAccts.Add(bankAcct);
                    }
                });
            if (tempBankAccts.Any())
            {
                this.SellAccount.BankAccount =
                    tempBankAccts.FirstOrDefault(o => o.CurrencyId == this.CustSellCcy.Id && o.Enabled);
                this.SellAccount.CalcTodayBalance(this.ValueDate.Date, this.CounterParty.Id);
            }
        }

        /// <summary>
        /// The on validate.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "CustBuyAmount")
            {
                if (this.custBuyAmount <= decimal.Zero)
                {
                    return RunTime.FindStringResource("MSG_00010");
                }
            }

            if (propertyName == "CustSellAmount")
            {
                if (this.custSellAmount <= decimal.Zero)
                {
                    return RunTime.FindStringResource("MSG_00010");
                }
            }

            ////if (propertyName == "ValueDate")
            ////{
            ////    if (this.ValueDate == default(DateTime) || this.ValueDate.DayOfWeek == DayOfWeek.Saturday
            ////        || this.ValueDate.DayOfWeek == DayOfWeek.Sunday)
            ////    {
            ////        return RunTime.FindStringResource("MSG_10021");
            ////    }
            ////}

            ////if (propertyName == "LocalTradeDay")
            ////{
            ////    if (this.LocalTradeDate == default(DateTime) || this.LocalTradeDate.DayOfWeek == DayOfWeek.Saturday
            ////        || this.LocalTradeDate.DayOfWeek == DayOfWeek.Sunday)
            ////    {
            ////        return RunTime.FindStringResource("MSG_10052");
            ////    }
            ////}

            ////if (propertyName == "CounterParty")
            ////{
            ////    if (this.CounterParty == null)
            ////    {
            ////        return RunTime.FindStringResource("交易对手不存在，请重新选择");
            ////    }
            ////}

            ////if (propertyName == "SpotRate")
            ////{
            ////    if (this.DealerSpotRate <= 0)
            ////    {
            ////        return RunTime.FindStringResource("MSG_XXXXXXX");
            ////    }
            ////}
            return null;
        }

        /// <summary>
        ///     The on validated.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnValidated()
        {
            ////if (this.LocalTradeDate.Date > this.ValueDate.Date)
            ////{
            ////    return RunTime.FindStringResource("MSG_10021");
            ////}
            ////var instruments = InstrumentTool.GetInstruments(
            ////    TradableInstrumentEnum.FX_HEDGING_SPOT_FORWARD, 
            ////    this.currentEnterprise);
            ////if (!instruments.Any())
            ////{
            ////    return RunTime.FindStringResource("MSG_10020");
            ////}
            return string.Empty;
        }

        /// <summary>
        /// The builder msg.
        /// </summary>
        /// <param name="hedgeDealVM">
        /// The hedge deal vm.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string BuilderMsg(FxHedgingDealModel hedgeDealVM)
        {
            string dealtBuySell = string.Empty;
            string dealtBuySellAmount = string.Empty;
            string counterBuySell = string.Empty;
            string counterBuySellAmount = string.Empty;

            string buySell = string.Empty;
            ContractModel symbolTemp = this.priceInfo.OwnerBlock.BelongContract;

            if (this.isDealtBuyCCY)
            {
                if (symbolTemp.Ccy1Id == this.CustBuyCcy.Id)
                {
                    buySell = RunTime.FindStringResource("MsgBuy");
                }

                dealtBuySell = this.CustBuyCcy.Id;
                dealtBuySellAmount = this.CustBuyAmount;
                counterBuySell = this.CustSellCcy.Id;
                counterBuySellAmount = this.CustSellAmount;
            }
            else
            {
                if (symbolTemp.Ccy1Id == this.CustSellCcy.Id)
                {
                    buySell = RunTime.FindStringResource("MsgSell");
                }

                dealtBuySell = this.CustSellCcy.Id;
                dealtBuySellAmount = this.CustSellAmount;
                counterBuySell = this.CustBuyCcy.Id;
                counterBuySellAmount = this.CustBuyAmount;
            }

            string msg = string.Format(
                RunTime.FindStringResource("MSG_10025"),
                this.CounterParty.Name,
                buySell,
                this.currencyRep.GetName(dealtBuySell),
                dealtBuySellAmount,
                this.currencyRep.GetName(counterBuySell),
                counterBuySellAmount,
                hedgeDealVM.ContractRate,
                hedgeDealVM.ValueDate.Date.FormatDateByBu());
            return msg;
        }

        /// <summary>
        ///     处理日历选择时间变更
        /// </summary>
        private void HandleCalenderSelectDateChange()
        {
            foreach (ValueDateItemModel item in this.valueDatesForSelect)
            {
                if (item.ValueDate.Date == this.CalendarSelectDate.Date)
                {
                    this.SelectValueDate = item;
                    return;
                }
            }

            this.bdvaluedateItemInSelectSource.ValueDate = this.CalendarSelectDate;
            this.SelectValueDate = this.bdvaluedateItemInSelectSource;
        }

        /// <summary>
        ///     处理市场价变更
        /// </summary>
        private void HandleMarkeRateChange()
        {
            if (this.MarketrateInfo == null)
            {
                this.PlaceVisibility = Visibility.Collapsed;
                this.RequestVisibility = Visibility.Visible;
            }
            else
            {
                this.PlaceVisibility = Visibility.Visible;
                this.RequestVisibility = Visibility.Collapsed;
            }

            if (this.EnaleCustBuyAmount)
            {
                this.RefreshCustSellAmount();
            }
            else
            {
                if (this.EnaleCustSellAmount)
                {
                    this.RefreshCustBuyAmount();
                }
            }

            this.UpdatePriceHightSetting();
        }

        /// <summary>
        ///     处理ValueDate的变更
        /// </summary>
        private void HandleValueDateUpdate()
        {
            if (this.SelectValueDate.Tenor == TenorEnum.ON)
            {
                this.MarketrateInfo = this.priceInfo;
                this.Instrument = "1001";
            }
            else if (this.SelectValueDate.Tenor == TenorEnum.SP)
            {
                this.MarketrateInfo = null;
                this.Instrument = "1000";
            }
            else
            {
                this.MarketrateInfo = null;

                this.Instrument = "1001";
            }
        }

        /// <summary>
        ///     初始化Valuedate
        /// </summary>
        private void InitialValueDates()
        {
            // 初始化可选ValueDate
            foreach (object tenorObj in Enum.GetValues(typeof(TenorEnum)))
            {
                var tenorValue = (TenorEnum)tenorObj;
                ValueDateItemModel valuedateItem = null;
                if (tenorValue != TenorEnum.BD)
                {
                    DateTime? tenorValueDate =
                        RunTime.GetCurrentRunTime(this.OwnerId)
                            .CurrentValueDateCore.GetValueDateForCurrentBusinessUnit(
                                tenorValue,
                                this.priceInfo.OwnerBlock.BelongContract.Id);
                    if (tenorValueDate.HasValue)
                    {
                        valuedateItem = new ValueDateItemModel();
                        valuedateItem.Tenor = tenorValue;
                        valuedateItem.ValueDate = tenorValueDate.Value;
                        this.ValueDatesForSelect.Add(valuedateItem);
                    }
                }

                if (tenorValue == TenorEnum.ON)
                {
                    this.SelectValueDate = valuedateItem;
                    if (valuedateItem != null)
                    {
                        this.ValueDateSelectStartDay = valuedateItem.ValueDate;
                        this.CalendarSelectDate = valuedateItem.ValueDate;
                    }
                }

                if (tenorValue == TenorEnum.Y1)
                {
                    if (valuedateItem != null)
                    {
                        this.ValueDateSelectEndDay = valuedateItem.ValueDate;
                    }
                }
            }

            if (this.ValueDatesForSelect.Count > 0)
            {
                ////TODO:添加BD，为了解决BD不在列表中无法显示的问题
                this.bdvaluedateItemInSelectSource = new ValueDateItemModel();
                this.bdvaluedateItemInSelectSource.ValueDate = DateTime.MinValue;
                this.bdvaluedateItemInSelectSource.Tenor = TenorEnum.BD;
                this.ValueDatesForSelect.Add(this.bdvaluedateItemInSelectSource);
            }
        }

        /// <summary>
        ///     刷新客户买入货币量
        /// </summary>
        private void RefreshCustBuyAmount()
        {
            if (this.custSellAmount == decimal.Zero)
            {
                this.EnaleCustBuyAmount = true;
            }
            else
            {
                this.EnaleCustBuyAmount = false;
            }

            if (this.custSellAmount >= 0 && this.MarketrateInfo != null)
            {
                decimal rate = this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                   ? this.MarketrateInfo.Ask
                                   : this.MarketrateInfo.Bid;
                if (rate == 0)
                {
                    this.custBuyAmount = decimal.Zero;
                }

                if (this.CustBuyCcy == this.CustSellCcy)
                {
                    this.custBuyAmount = this.custSellAmount;
                }

                decimal result = this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                     ? this.custSellAmount / rate
                                     : this.custSellAmount * rate;
                this.custBuyAmount = result.FormatAmountToDecimalByCurrency(this.CustBuyCcy);
                this.NotifyOfPropertyChange("CustBuyAmount");
            }
            else
            {
                if (this.custSellAmount == decimal.Zero)
                {
                    this.custBuyAmount = decimal.Zero;
                }
                else
                {
                    this.custBuyAmount = decimal.MinValue;
                }

                this.NotifyOfPropertyChange("CustBuyAmount");
            }
        }

        /// <summary>
        ///     刷新客户卖出货币量
        /// </summary>
        private void RefreshCustSellAmount()
        {
            if (this.custBuyAmount == decimal.Zero)
            {
                this.EnaleCustSellAmount = true;
            }
            else
            {
                this.EnaleCustSellAmount = false;
            }

            if (this.custBuyAmount >= 0 && this.MarketrateInfo != null)
            {
                decimal rate = this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                   ? this.MarketrateInfo.Ask
                                   : this.MarketrateInfo.Bid;
                if (rate == 0)
                {
                    this.custSellAmount = decimal.Zero;
                }

                if (this.CustBuyCcy == this.CustSellCcy)
                {
                    this.custSellAmount = this.custBuyAmount;
                }

                decimal result = this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id
                                     ? this.custBuyAmount * rate
                                     : this.custBuyAmount / rate;
                this.custSellAmount = result.FormatAmountToDecimalByCurrency(this.CustSellCcy);
                this.NotifyOfPropertyChange("CustSellAmount");
            }
            else
            {
                if (this.custBuyAmount == decimal.Zero)
                {
                    this.custSellAmount = decimal.Zero;
                }
                else
                {
                    this.custSellAmount = decimal.MinValue;
                }

                this.NotifyOfPropertyChange("CustSellAmount");
            }
        }

        /// <summary>
        ///     重置处理
        /// </summary>
        private void Reset()
        {
            // this.SpotRate = decimal.Zero.ToString();
            this.ForwardPoint = decimal.Zero;

            // this.OpenRate = decimal.Zero;
            this.Comment = string.Empty;
            this.HedgingDealId = string.Empty;
            this.custBuyAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustBuyAmount);
            this.EnaleCustBuyAmount = true;
            this.custSellAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustSellAmount);
            this.EnaleCustSellAmount = true;
            this.DealerSpotRate = decimal.Zero;
            this.ContractRate = decimal.Zero;
            this.IsCustSellAmountEnable = false;
            this.IsCustBuyAmountEnable = true;
        }

        /// <summary>
        ///     更新报价亮色设置
        /// </summary>
        private void UpdatePriceHightSetting()
        {
            if (this.MarketrateInfo == null)
            {
                this.AskHight = false;
                this.BidHight = false;
            }
            else
            {
                if (this.CustBuyCcy.Id == this.priceInfo.OwnerBlock.BelongContract.Ccy1Id)
                {
                    this.AskHight = true;
                    this.BidHight = false;
                }
                else
                {
                    this.AskHight = false;
                    this.BidHight = true;
                }
            }
        }

        #endregion
    }
}