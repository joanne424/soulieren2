// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewHedgeSpotForwardViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/18 05:30:30 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/18 05:30:30
//      修改描述：新建 NewHedgeSpotForwardViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Command;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Utils;

    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    ///     The new hedge spot forward view model.
    /// </summary>
    public class NewHedgeSpotForwardViewModel : FxHedgingDealModel
    {
        #region Fields

        /// <summary>
        ///     The bank account rep.
        /// </summary>
        private readonly IBankAccountRepository bankAccountRep;

        /// <summary>
        ///     The contract rep.
        /// </summary>
        private readonly IContractRepository contractRep;

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     The current enterprise.
        /// </summary>
        private readonly EnterpriseModel currentEnterprise;

        /// <summary>
        ///     The settlement bank accounts.
        /// </summary>
        private readonly List<BankAccountModel> settlementBankAccounts = new List<BankAccountModel>();

        /// <summary>
        ///     The business unit model.
        /// </summary>
        private BusinessUnitModel businessUnitModel;

        /// <summary>
        ///     Buy CCY index
        /// </summary>
        private int buyCCYIndex = -1;

        /// <summary>
        ///     The ccy list.
        /// </summary>
        private ObservableCollection<CurrencyModel> ccyList;

        /// <summary>
        ///     The comment.
        /// </summary>
        private string comment = string.Empty;

        /// <summary>
        ///     The hedge account.
        /// </summary>
        private CounterPartyModel counterParty;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterPartys;

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
        private string custBuyCCY;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal custSellAmount;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private string custSellCCY;

        /// <summary>
        ///     The ent instrument.
        /// </summary>
        private IDictionary<string, string> entInstrument;

        // private TradableInstrumentEnum? instrument;
        /// <summary>
        ///     The instrument.
        /// </summary>
        /// <summary>
        ///     instrument 下拉列表
        /// </summary>
        private Dictionary<int, string> instrumentEnum;

        // private bool isCustBuyAmountEnable = true;
        // private bool isCustSellAmountEnable = true;
        /// <summary>
        ///     The is we buy amount enable.
        /// </summary>
        /// <summary>
        ///     The is we sell amount enable.
        /// </summary>
        /// <summary>
        ///     The is dealt wbccy.
        /// </summary>
        private bool isDealtBuyCCY;

        /// <summary>
        ///     The is hedge accounts enable.
        /// </summary>
        private bool isHedgeAccountsEnable;

        /// <summary>
        ///     The is open rate enable.
        /// </summary>
        private bool isOpenRateEnable;

        /// <summary>
        ///     The sell account.
        /// </summary>
        private SellBankAcctBalanceVM sellAccount;

        /// <summary>
        ///     Sell  CCy  选中的index
        /// </summary>
        private int sellCCYIndex = -1;

        /// <summary>
        ///     The near value date change command.
        /// </summary>
        private RelayCommand valueDateChangeCommand;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewHedgeSpotForwardViewModel"/> class.
        ///     Initializes a new instance of the <see cref="ModifySpotForwardViewModel"/> class.
        /// </summary>
        /// <param name="ownerID">
        /// The owner id.
        /// </param>
        public NewHedgeSpotForwardViewModel(string ownerID = null)
        {
            this.DisplayName = RunTime.FindStringResource("NewHedgeSpotForward");
            if (RunTime.GetCurrentRunTime().CurrentLoginUser == null)
            {
                return;
            }

            this.currencyRep = this.GetRepository<ICurrencyRepository>();
            this.contractRep = this.GetRepository<IContractRepository>();
            this.CounterPartys = this.GetRepository<ICounterPartyRepository>().GetBindCollection().ToComboboxBinding();
            this.currentEnterprise =
                this.GetRepository<IEnterpriseRepository>().FindByID(RunTime.GetCurrentRunTime().CurrentLoginUser.EntId);
            this.bankAccountRep = this.GetRepository<IBankAccountRepository>();
            this.EntInstrument = new Dictionary<string, string>();
            InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_HEDGING_SPOT, this.currentEnterprise)
                .ForEach(o => this.EntInstrument.Add(o.Key, o.Value));
            InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_HEDGING_FORWARD, this.currentEnterprise)
                .ForEach(o => this.EntInstrument.Add(o.Key, o.Value));
            this.CCYList = this.currencyRep.GetBindCollection();
            this.IsDealtBuyCCY = true;
            this.CounterPartySelectionChangedCommand = new RelayCommand(this.CounterPartySelectionChanged);
            this.BuyCurrencySelectionChangedCommand = new RelayCommand(this.BuyCurrencySelectionChanged);
            this.SellCurrencySelectionChangedCommand = new RelayCommand(this.SellCurrencySelectionChanged);
            this.LocalTradeDateChangedCommand = new RelayCommand(this.SellCurrencySelectionChanged);
            this.ValueDateChangeCommand = new RelayCommand(this.ValueDateChange);
            this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
            this.ValueDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
            this.BuyClickCommand = new RelayCommand(this.BuyClick);
            this.SellClickCommand = new RelayCommand(this.SellClick);
            this.GetRepository<IFxHedgingDealRepository>().SubscribeAddEvent(
                (newDeal) => this.SetSellAcount());
            this.GetRepository<IFxHedgingDealRepository>().SubscribeRemoveEvent(
                (newDeal) => this.SetSellAcount());
            this.Init();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     已取消事件
        /// </summary>
        /// <summary>
        ///     已确认事件
        /// </summary>
        /// <summary>
        ///     BuyCCY在CCYList中的索引号
        /// </summary>
        public int BuyCCYIndex
        {
            get
            {
                return this.buyCCYIndex;
            }

            set
            {
                this.buyCCYIndex = value;
                this.NotifyOfPropertyChange("BuyCCYIndex");
            }
        }

        /// <summary>
        ///     SellClick 事件
        /// </summary>
        public RelayCommand BuyClickCommand { get; private set; }

        /// <summary>
        ///     Buy  Currency  选中事件
        /// </summary>
        public RelayCommand BuyCurrencySelectionChangedCommand { get; private set; }

        ///// <summary>
        ///// 获取buy的CCYList
        ///// </summary>
        // public Dictionary<string, string> BuyCCYList
        // {
        // get
        // {
        // var currency = new Dictionary<string, string>();
        // ObservableCollection<BaseCurrencyVM> currencys = this.BusinessUnit.GetEnableCurrencyList();
        // foreach (BaseCurrencyVM item in currencys)
        // {
        // currency.Add(item.CurrencyID, item.CurrencyName);
        // }

        // return currency;
        // }
        // }

        /// <summary>
        ///     可选货币列表
        /// </summary>
        public ObservableCollection<CurrencyModel> CCYList
        {
            get
            {
                return this.ccyList;
            }

            set
            {
                this.ccyList = value;
                this.NotifyOfPropertyChange(() => this.CCYList);
            }
        }

        /// <summary>
        ///     备注
        /// </summary>
        public string Comment
        {
            get
            {
                return this.comment;
            }

            set
            {
                this.comment = value;
                this.NotifyOfPropertyChange(() => this.Comment);
            }
        }

        /// <summary>
        ///     HedgeAccount实体
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
                value.SettlementAccounts.Values.ForEach(
                    s =>
                    {
                        BankAccountModel bankAcct = this.bankAccountRep.FindByID(s.ToString());
                        if (bankAcct != null)
                        {
                            this.settlementBankAccounts.Add(bankAcct);
                        }
                    });

                this.NotifyOfPropertyChange(() => this.CounterParty);
            }
        }

        /// <summary>
        ///     CounterParty 选中事件
        /// </summary>
        public RelayCommand CounterPartySelectionChangedCommand { get; private set; }

        /// <summary>
        ///     对冲账户列表
        /// </summary>
        public ObservableCollection<CounterPartyModel> CounterPartys
        {
            get
            {
                return this.counterPartys;
            }

            set
            {
                this.counterPartys = value;
                this.NotifyOfPropertyChange(() => this.CounterPartys);
            }
        }

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

                if (this.IsDealtBuyCCY)
                {
                    if (inSetValue > 0 && this.CustSellCCY != null)
                    {
                        this.custSellAmount = this.GetCCY1AmountByCCY2Rate(inSetValue, this.OpenRate);
                        this.NotifyOfPropertyChange(() => this.CustSellAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     CustBuyCCY
        /// </summary>
        public string CustBuyCCY
        {
            get
            {
                return this.custBuyCCY;
            }

            set
            {
                this.custBuyCCY = value;
                this.NotifyOfPropertyChange(() => this.CustBuyCCY);
            }
        }

        /// <summary>
        ///     CCY1Amount
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
                this.NotifyOfPropertyChange(() => this.CustSellAmount);

                if (this.IsDealtSellCCY)
                {
                    if (inSetValue > 0 && this.CustSellCCY != null)
                    {
                        this.custBuyAmount = this.GetCCY2AmountByCCY1Rate(inSetValue, this.OpenRate);
                        this.NotifyOfPropertyChange(() => this.CustBuyAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     WeSellCCY
        /// </summary>
        public string CustSellCCY
        {
            get
            {
                return this.custSellCCY;
            }

            set
            {
                this.custSellCCY = value;
                this.NotifyOfPropertyChange(() => this.CustSellCCY);
            }
        }

        /// <summary>
        ///     Gets or sets the ent instrument.
        /// </summary>
        public IDictionary<string, string> EntInstrument
        {
            get
            {
                return this.entInstrument;
            }

            set
            {
                this.entInstrument = value;
                this.NotifyOfPropertyChange(() => this.EntInstrument);
            }
        }

        /// <summary>
        ///     Gets or sets the farward point.
        /// </summary>
        public string ForwardPointStr
        {
            get
            {
                return this.ForwardPoint.ToString();
            }

            set
            {
                decimal farPoint;
                if (decimal.TryParse(value, out farPoint))
                {
                    this.ForwardPoint = farPoint;
                    this.NotifyOfPropertyChange("ForwardPointStr");

                    this.AutoCalOpenRate();
                }
            }
        }

        /// <summary>
        ///     对冲交易单类型
        /// </summary>
        /// <summary>
        ///     Gets or sets the instrument enum.
        /// </summary>
        public Dictionary<int, string> Instruments
        {
            get
            {
                return this.instrumentEnum;
            }

            set
            {
                this.instrumentEnum = value;
                this.NotifyOfPropertyChange("InstrumentEnum");
            }
        }

        // public bool IsCustBuyAmountEnable
        // {
        // get
        // {
        // return this.isCustBuyAmountEnable;
        // }

        // set
        // {
        // this.isCustBuyAmountEnable = value;
        // this.NotifyOfPropertyChange(() => this.IsCustBuyAmountEnable);
        // }
        // }
        // public bool IsCustSellAmountEnable
        // {
        // get
        // {
        // return this.isCustSellAmountEnable;
        // }

        // set
        // {
        // this.isCustSellAmountEnable = value;
        // this.NotifyOfPropertyChange(() => this.IsCustSellAmountEnable);
        // }
        // }
        /// <summary>
        ///     CCY2Amount 是否可用
        /// </summary>
        /// <summary>
        ///     CCY1Amount 是否可用
        /// </summary>
        /// <summary>
        ///     手书WeBuyCCYAmount
        /// </summary>
        public bool IsDealtBuyCCY
        {
            get
            {
                return this.isDealtBuyCCY;
            }

            set
            {
                this.isDealtBuyCCY = value;
                this.NotifyOfPropertyChange(() => this.IsDealtBuyCCY);
                this.NotifyOfPropertyChange(() => this.IsDealtSellCCY);
                this.UpdateCustomerTyping();
            }
        }

        // bool isDealtWSCCY;
        /// <summary>
        ///     手书CustSellCCYAmount
        /// </summary>
        public bool IsDealtSellCCY
        {
            get
            {
                return !this.isDealtBuyCCY;
            }

            set
            {
                this.isDealtBuyCCY = !value;
                this.NotifyOfPropertyChange(() => this.IsDealtSellCCY);
                this.NotifyOfPropertyChange(() => this.IsDealtBuyCCY);
                this.UpdateCustomerTyping();
            }
        }

        /// <summary>
        ///     Gets the local trade date changed command.
        /// </summary>
        public RelayCommand LocalTradeDateChangedCommand { get; private set; }

        /// <summary>
        ///     Gets or sets the open rate.
        /// </summary>
        public decimal OpenRate
        {
            get
            {
                return this.ContractRate;
            }

            set
            {
                this.ContractRate = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange("OpenRate");
                if (this.CustSellCCY == null || this.CustBuyCCY == null)
                {
                    return;
                }

                if (this.isDealtBuyCCY)
                {
                    if (this.custBuyAmount > 0)
                    {
                        this.custSellAmount = this.GetCCY1AmountByCCY2Rate(this.custBuyAmount, this.OpenRate);
                        this.NotifyOfPropertyChange(() => this.CustSellAmount);
                    }
                }
                else
                {
                    if (this.custSellAmount > 0)
                    {
                        this.custBuyAmount = this.GetCCY2AmountByCCY1Rate(this.custSellAmount, this.OpenRate);
                        this.NotifyOfPropertyChange(() => this.CustBuyAmount);
                    }
                }
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
        ///     Gets or sets the sell ccy index.
        /// </summary>
        public int SellCCYIndex
        {
            get
            {
                return this.sellCCYIndex;
            }

            set
            {
                this.sellCCYIndex = value;
                this.NotifyOfPropertyChange("SellCCYIndex");
            }
        }

        /// <summary>
        ///     SellClick 事件
        /// </summary>
        public RelayCommand SellClickCommand { get; private set; }

        /// <summary>
        ///     Sell  Currency2 选中事件
        /// </summary>
        public RelayCommand SellCurrencySelectionChangedCommand { get; private set; }

        /// <summary>
        ///     Gets or sets the spot rate.
        /// </summary>
        public string SpotRate
        {
            get
            {
                return this.DealerSpotRate.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                this.DealerSpotRate = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange("SpotRate");
                this.AutoCalOpenRate();
            }
        }

        /// <summary>
        ///     Gets or sets the near value date change command.
        /// </summary>
        public RelayCommand ValueDateChangeCommand
        {
            get
            {
                return this.valueDateChangeCommand;
            }

            set
            {
                this.valueDateChangeCommand = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The buy amount changed.
        /// </summary>
        public void BuyAmountChanged()
        {
            Console.WriteLine("BuyAmount");
        }

        /// <summary>
        ///     The buy click.
        /// </summary>
        public void BuyClick()
        {
            this.CustBuyAmount = decimal.Zero.ToString();
            this.CustSellAmount = decimal.Zero.ToString();

            // this.IsCustSellAmountEnable = false;
            // this.IsCustBuyAmountEnable = true;
        }

        /// <summary>
        ///     The cancel_ click.
        /// </summary>
        public void Cancel_Click()
        {
            this.TryClose();
        }

        /// <summary>
        ///     发布已取消事件
        /// </summary>
        /// <summary>
        ///     发布已确认事件
        /// </summary>
        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     提交事件
        /// </summary>
        public void Confirm_Click()
        {
            if (!this.ValidateforSumbit())
            {
                RunTime.ShowInfoDialogWithoutRes(this.Error, string.Empty, this.OwnerId);
                return;
            }

            var hedgeDealVM = new FxHedgingDealModel
            {
                IsNearLeg = (int)IsNearLegEnum.NONE,
                BusinessUnitId = this.BusinessUnitId,
                ValueDate = this.ValueDate,
                LocalTradeDate = this.LocalTradeDate,
                Instrument = this.Instrument,
                Comment = this.Comment,
                CounterpartyId = this.CounterParty.Id
            };

            ContractModel symbol = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);

            hedgeDealVM.Ccy1Id = symbol.Ccy1Id;
            hedgeDealVM.Ccy2Id = symbol.Ccy2Id;
            if (this.CustSellCCY == symbol.Ccy1Id)
            {
                hedgeDealVM.Ccy1Amount = this.custSellAmount;
                hedgeDealVM.Ccy2Amount = this.custBuyAmount;
            }
            else
            {
                hedgeDealVM.Ccy1Amount = this.custBuyAmount;
                hedgeDealVM.Ccy2Amount = this.custSellAmount;
            }

            hedgeDealVM.ContractId = symbol.Id;
            hedgeDealVM.TransactionType = this.CustSellCCY == symbol.Ccy1Id
                                              ? TransactionTypeEnum.Sell
                                              : TransactionTypeEnum.Buy;
            hedgeDealVM.DealerSpotRate = this.DealerSpotRate;
            hedgeDealVM.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            hedgeDealVM.StaffId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;
            hedgeDealVM.UserId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;
            hedgeDealVM.Typing = this.Typing;
            hedgeDealVM.HedgingDealId = this.HedgingDealId;
            hedgeDealVM.HedgingExecutionId = this.HedgingDealId;
            hedgeDealVM.ContractRate = this.OpenRate;
            hedgeDealVM.DealerSpotRate = this.DealerSpotRate;
            hedgeDealVM.ForwardPoint = this.ForwardPoint;
            hedgeDealVM.InstitutionId = this.CounterParty.InstitutionId;
            string msg = this.BuilderMsg(hedgeDealVM);
            if (RunTime.ShowConfirmDialogWithoutRes(msg, string.Empty, this.OwnerId))
            {
                var service = new HedgingDealService(this.OwnerId);
                CmdResult drs = service.NewHedgeSpotForward(hedgeDealVM);
                if (drs.Success)
                {
                    this.Init();
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
        /// Contract 向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void ContractRatePlus_Click(string txt)
        {
            this.OpenRate = Util.TxtAdd(txt);
        }

        /// <summary>
        /// Contract向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void ContractRateSub_Click(string txt)
        {
            this.OpenRate = Util.TxtSub(txt);
        }

        /// <summary>
        ///     客户输入CCY2Amount
        /// </summary>
        public void FocusedBuyAmount()
        {
            this.UpdateCustomerTyping();

            // if (string.IsNullOrEmpty(this.WeSellCCY) || string.IsNullOrEmpty(this.WeBuyCCY))
            // {
            // return;
            // }
            // if (string.IsNullOrWhiteSpace(this.WeBuyAmount) || this.WeBuyAmount.ToDecimal() == 0)
            // {
            // return;
            // }
            // var symbolTemp = this.GetRepository<ISymbolCacheRepository>().GetSymbol(this.WeSellCCY, this.WeBuyCCY);
            // if (symbolTemp == null)
            // {
            // return;
            // }
            // if (symbolTemp.CCY1 == this.WeBuyCCY)
            // {
            // this.HedgeDealVM.CustomerTyping = CustomerTradeTypingEnum.CCY1Amount;
            // }
            // else
            // {
            // this.HedgeDealVM.CustomerTyping = CustomerTradeTypingEnum.CCY2Amount;
            // }
        }

        /// <summary>
        ///     客户输入CCY1Amount
        /// </summary>
        public void FocusedSellAmount()
        {
            this.UpdateCustomerTyping();

            // if (string.IsNullOrEmpty(this.WeSellCCY) || string.IsNullOrEmpty(this.WeBuyCCY))
            // {
            // return;
            // }
            // if (string.IsNullOrWhiteSpace(this.WeSellAmount) || this.WeSellAmount.ToDecimal() == 0)
            // {
            // return;
            // }
            // var symbolTemp = this.GetRepository<ISymbolCacheRepository>().GetSymbol(this.WeSellCCY, this.WeBuyCCY);
            // if (symbolTemp == null)
            // {
            // return;
            // }
            // if (symbolTemp.CCY1 == this.WeSellCCY)
            // {
            // this.HedgeDealVM.CustomerTyping = CustomerTradeTypingEnum.CCY1Amount;
            // }
            // else
            // {
            // this.HedgeDealVM.CustomerTyping = CustomerTradeTypingEnum.CCY2Amount;
            // }
        }

        /// <summary>
        /// ForwardPoint 向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void ForwardPointPlus_Click(string txt)
        {
            this.ForwardPointStr = Util.TxtAdd(txt).ToString();
        }

        /// <summary>
        /// ForwardPoint 向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void ForwardPointSub_Click(string txt)
        {
            this.ForwardPointStr = Util.TxtSub(txt).ToString();
        }

        /// <summary>
        ///     The sell click.
        /// </summary>
        public void SellClick()
        {
            this.CustBuyAmount = decimal.Zero.ToString();
            this.CustSellAmount = decimal.Zero.ToString();

            // this.IsCustSellAmountEnable = true;
            // this.IsCustBuyAmountEnable = false;
        }

        /// <summary>
        /// SpotRate向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void SpotRatePlus_Click(string txt)
        {
            this.SpotRate = Util.TxtAdd(txt).ToString();
        }

        /// <summary>
        /// SpotRate向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void SpotRateSub_Click(string txt)
        {
            this.SpotRate = Util.TxtSub(txt).ToString();
        }

        /// <summary>
        ///     TODO 这里用的是旧逻辑，需要更新
        ///     The update customer typing.
        /// </summary>
        public void UpdateCustomerTyping()
        {
            if (string.IsNullOrEmpty(this.CustSellCCY) || string.IsNullOrEmpty(this.CustBuyCCY))
            {
                return;
            }

            ContractModel symbolTemp = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);
            if (symbolTemp == null)
            {
                return;
            }

            if (symbolTemp.Ccy1Id == this.CustBuyCCY)
            {
                this.Typing = this.isDealtBuyCCY ? TypingEnum.CCY1_AMOUNT : TypingEnum.CCY2_AMOUNT;
            }
            else
            {
                this.Typing = this.isDealtBuyCCY ? TypingEnum.CCY2_AMOUNT : TypingEnum.CCY1_AMOUNT;
            }
        }

        #endregion

        #region Methods

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
            if (propertyName == "ValueDate")
            {
                if (this.ValueDate == default(DateTime) || this.ValueDate.DayOfWeek == DayOfWeek.Saturday
                    || this.ValueDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return RunTime.FindStringResource("MSG_10021");
                }
            }

            if (propertyName == "LocalTradeDate")
            {
                if (this.LocalTradeDate == default(DateTime) || this.LocalTradeDate.DayOfWeek == DayOfWeek.Saturday
                    || this.LocalTradeDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    return RunTime.FindStringResource("MSG_10052");
                }
            }

            if (propertyName == "CounterParty")
            {
                if (this.CounterParty == null)
                {
                    return RunTime.FindStringResource("交易对手不存在，请重新选择");
                }
            }

            if (propertyName == "SpotRate")
            {
                if (this.DealerSpotRate <= 0)
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "CustBuyAmount")
            {
                if (this.custBuyAmount <= 0)
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "CustSellAmount")
            {
                if (this.custSellAmount <= 0)
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

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
            if (this.LocalTradeDate.Date > this.ValueDate.Date)
            {
                return RunTime.FindStringResource("MSG_10021");
            }

            ContractModel symbol = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);
            if (symbol == null)
            {
                return RunTime.FindStringResource("MSG_10059");
            }

            return string.Empty;
        }

        /// <summary>
        ///     自动计算开仓价
        /// </summary>
        private void AutoCalOpenRate()
        {
            decimal forwardPointMathValue;

            if (this.CustSellCCY == this.CustBuyCCY)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.CustSellCCY) || string.IsNullOrWhiteSpace(this.CustBuyCCY))
            {
                return;
            }

            ContractModel symbolTemp = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);
            if (symbolTemp != null)
            {
                forwardPointMathValue =
                    Convert.ToDecimal((double)this.ForwardPoint * Math.Pow(10d, 0 - (double)symbolTemp.BasisPoint));
            }
            else
            {
                forwardPointMathValue = this.ForwardPoint;
            }

            this.OpenRate = this.DealerSpotRate + forwardPointMathValue;
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
            if (this.isDealtBuyCCY)
            {
                buySell = RunTime.FindStringResource("MsgBuy");
                dealtBuySell = this.CustBuyCCY;
                dealtBuySellAmount = this.CustBuyAmount;
                counterBuySell = this.CustSellCCY;
                counterBuySellAmount = this.CustSellAmount;
            }
            else
            {
                buySell = RunTime.FindStringResource("MsgSell");
                dealtBuySell = this.CustSellCCY;
                dealtBuySellAmount = this.CustSellAmount;
                counterBuySell = this.CustBuyCCY;
                counterBuySellAmount = this.CustBuyAmount;
            }

            string msg = string.Format(
                RunTime.FindStringResource("MSG_10022"),
                this.GetRepository<ICounterPartyRepository>().GetName(hedgeDealVM.CounterpartyId),
                hedgeDealVM.HedgingDealId,
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
        /// </summary>
        private void BuyCurrencySelectionChanged()
        {
            // TODO
        }

        /// <summary>
        /// </summary>
        private void CounterPartySelectionChanged()
        {
            if (this.counterParty == null)
            {
                return;
            }

            this.businessUnitModel =
                this.GetRepository<IBusinessUnitRepository>().FindByID(this.counterParty.BusinessUnitId);
            if (this.businessUnitModel == null)
            {
                // this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
                // this.ValueDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
                return;
            }

            this.BusinessUnitId = this.businessUnitModel.Id;
            this.SetSellAcount();

            // this.LocalTradeDate =
            // this.businessUnitModel.GetLocalTradeDate(
            // RunTime.GetCurrentRunTime(this.OwnerId).GetCurrentTimeForBu(this.counterParty.BusinessUnitId));
            // if (this.ValueDate == default(DateTime) || this.ValueDate == Infrastructure.Common.Util.GetDefaultDateTime())
            // {
            // this.ValueDate = this.LocalTradeDate;
            // }
        }

        /// <summary>
        /// 根据CCY2Amount自动计算CCY1Amount
        /// </summary>
        /// <param name="ccy2Amount">
        /// 已输入的CCY2Amount
        /// </param>
        /// <param name="rate">
        /// Rate
        /// </param>
        /// <returns>
        /// 自动计算结果
        /// </returns>
        private decimal GetCCY1AmountByCCY2Rate(decimal ccy2Amount, decimal rate)
        {
            decimal result;

            if (rate == 0)
            {
                return decimal.Zero;
            }

            if (string.IsNullOrWhiteSpace(this.CustBuyCCY) || string.IsNullOrWhiteSpace(this.CustSellCCY))
            {
                return decimal.Zero;
            }

            if (this.CustBuyCCY == this.CustSellCCY)
            {
                return ccy2Amount;
            }

            ContractModel symbolTemp = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);
            if (symbolTemp == null)
            {
                return 0;
            }

            result = this.CustBuyCCY == symbolTemp.Ccy1Id ? ccy2Amount * rate : ccy2Amount / rate;
            CurrencyModel currencyTemp = this.currencyRep.Filter(p => p.Id == this.CustSellCCY).FirstOrDefault();
            if (currencyTemp == null)
            {
                return result;
            }

            return result.FormatAmountToDecimalByCurrency(currencyTemp);
        }

        /// <summary>
        /// 根据CCY1Amount自动计算CCY2Amount
        /// </summary>
        /// <param name="ccy1Amount">
        /// 已输入的CCY1Amount
        /// </param>
        /// <param name="rate">
        /// Rate
        /// </param>
        /// <returns>
        /// 自动计算结果
        /// </returns>
        private decimal GetCCY2AmountByCCY1Rate(decimal ccy1Amount, decimal rate)
        {
            decimal result;

            if (rate == 0)
            {
                return decimal.Zero;
            }

            if (string.IsNullOrWhiteSpace(this.CustBuyCCY) || string.IsNullOrWhiteSpace(this.CustSellCCY))
            {
                return decimal.Zero;
            }

            if (this.CustBuyCCY == this.CustSellCCY)
            {
                return ccy1Amount;
            }

            ContractModel symbolTemp = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);
            result = this.CustSellCCY == symbolTemp.Ccy1Id ? ccy1Amount * rate : ccy1Amount / rate;
            CurrencyModel currencyTemp = this.currencyRep.Filter(p => p.Id == this.CustBuyCCY).FirstOrDefault();
            if (currencyTemp == null)
            {
                return result;
            }

            return result.FormatAmountToDecimalByCurrency(currencyTemp);
        }

        /// <summary>
        ///     The clear.
        /// </summary>
        private void Init()
        {
            if (this.ccyList.Count > 1)
            {
                this.CustBuyCCY = this.ccyList[0].Id;
                this.CustSellCCY = this.ccyList[1].Id;
            }

            if (this.CounterPartys.Any())
            {
                this.CounterParty = this.CounterPartys[0];
                this.CounterPartySelectionChanged();
            }

            if (this.EntInstrument.Any())
            {
                this.Instrument = this.EntInstrument.First().Key;
            }

            this.SpotRate = decimal.Zero.ToString();
            this.ForwardPoint = decimal.Zero;
            this.OpenRate = decimal.Zero;
            this.Comment = string.Empty;
            this.HedgingDealId = string.Empty;
            this.custBuyAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustBuyAmount);
            this.custSellAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustSellAmount);
            this.DealerSpotRate = decimal.Zero;
            this.ContractRate = decimal.Zero;
            this.NotifyOfPropertyChange("SpotRate");

            // this.Instrument = (int)TradableInstrumentEnum.FX_HEDGING_SPOT;
            this.BuyClick();
        }

        /// <summary>
        /// </summary>
        private void SellCurrencySelectionChanged()
        {
            this.SetSellAcount();
        }

        /// <summary>
        ///     The set sell acount.
        /// </summary>
        private void SetSellAcount()
        {
            this.SellAccount = new SellBankAcctBalanceVM();
            this.SellAccount.Currency = this.currencyRep.FindByID(this.CustSellCCY);
            if (this.CounterParty == null)
            {
                return;
            }

            this.SellAccount.BankAccount =
                this.settlementBankAccounts.FirstOrDefault(o => o.CurrencyId == this.CustSellCCY && o.Enabled);
            this.sellAccount.CalcTodayBalance(this.ValueDate.Date, this.counterParty.Id);
        }

        /// <summary>
        ///     The near value date change.
        /// </summary>
        private void ValueDateChange()
        {
            this.SetSellAcount();
        }

        #endregion
    }
}