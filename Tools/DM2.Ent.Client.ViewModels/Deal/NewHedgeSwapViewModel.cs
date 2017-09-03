// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewHedgeSwapViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/18 05:30:30 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/18 05:30:30
//      修改描述：新建 NewHedgeSwapViewModel.cs
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
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Command;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Log;

    using Microsoft.Practices.ObjectBuilder2;

    using Util = Infrastructure.Utils.Util;

    /// <summary>
    ///     The new hedge spot forward view model.
    /// </summary>
    public class NewHedgeSwapViewModel : BaseVm
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
        ///     The far rate change command.
        /// </summary>
        private RelayCommand FarRateChangeCommand;

        /// <summary>
        ///     The near rate change command.
        /// </summary>
        private RelayCommand NearRateChangeCommand;

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
        ///     The ent instrument.
        /// </summary>
        private IDictionary<string, string> entInstrument;

        /// <summary>
        /// The far buy amount enabled.
        /// </summary>
        private bool farBuyAmountEnabled;

        /// <summary>
        ///     The value date.
        /// </summary>
        /// <summary>
        ///     The we buy amount.
        /// </summary>
        private decimal farCustBuyAmount;

        /// <summary>
        ///     The we buy ccy.
        /// </summary>
        private string farCustBuyCCY;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal farCustSellAmount;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private string farCustSellCCY;

        /// <summary>
        ///     The far deal.
        /// </summary>
        private FxHedgingDealModel farDeal;

        /// <summary>
        ///     The sell account.
        /// </summary>
        private SellBankAcctBalanceVM farSellAccount;

        /// <summary>
        /// The far sell amount enabled.
        /// </summary>
        private bool farSellAmountEnabled;

        /// <summary>
        /// The far value date change command.
        /// </summary>
        private RelayCommand farValueDateChangeCommand;

        /// <summary>
        ///     The hedging deal id.
        /// </summary>
        private string hedgingDealId;

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
        ///     The local trade date.
        /// </summary>
        private DateTime localTradeDate;

        /// <summary>
        ///     The value date.
        /// </summary>
        /// <summary>
        ///     The we buy amount.
        /// </summary>
        private decimal nearCustBuyAmount;

        /// <summary>
        ///     The we buy ccy.
        /// </summary>
        private string nearCustBuyCCY;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal nearCustSellAmount;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private string nearCustSellCCY;

        /// <summary>
        ///     The near deal.
        /// </summary>
        private FxHedgingDealModel nearDeal;

        /// <summary>
        ///     The sell account.
        /// </summary>
        private SellBankAcctBalanceVM nearSellAccount;

        /// <summary>
        /// The near value date change command.
        /// </summary>
        private RelayCommand nearValueDateChangeCommand;

        /// <summary>
        ///     Sell  CCy  选中的index
        /// </summary>
        private int sellCCYIndex = -1;

        /// <summary>
        ///     The typing.
        /// </summary>
        private TypingEnum typing = TypingEnum.CCY1_AMOUNT;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewHedgeSwapViewModel"/> class.
        ///     Initializes a new instance of the <see cref="ModifySpotForwardViewModel"/> class.
        /// </summary>
        /// <param name="ownerID">
        /// The owner id.
        /// </param>
        public NewHedgeSwapViewModel(string ownerID = null)
        {
            this.DisplayName = RunTime.FindStringResource("NewHedgeSwapDeal");
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

            this.CounterPartySelectionChangedCommand = new RelayCommand(this.CounterPartySelectionChanged);
            this.BuyCurrencySelectionChangedCommand = new RelayCommand(this.BuyCurrencySelectionChanged);
            this.SellCurrencySelectionChangedCommand = new RelayCommand(this.SellCurrencySelectionChanged);
            this.LocalTradeDateChangedCommand = new RelayCommand(this.SellCurrencySelectionChanged);
            this.BuyClickCommand = new RelayCommand(this.BuyClick);
            this.SellClickCommand = new RelayCommand(this.SellClick);
            this.NearRateChangeCommand = new RelayCommand(this.NearRateChange);
            this.FarRateChangeCommand = new RelayCommand(this.FarRateChange);
            this.NearBuyAmountChangeCommand = new RelayCommand(this.NearBuyAmountChange);
            this.NearSellAmountChangeCommand = new RelayCommand(this.NearSellAmountChange);
            this.NearValueDateChangeCommand = new RelayCommand(this.NearValueDateChange);
            this.FarValueDateChangeCommand = new RelayCommand(this.FarValueDateChange);
            this.GetRepository<IFxHedgingDealRepository>().SubscribeAddEvent(newDeal => this.SetSellAcount());
            this.GetRepository<IFxHedgingDealRepository>().SubscribeRemoveEvent(removeDeal => this.SetSellAcount());
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
        ///     key=name,value=id
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
        /// Gets or sets a value indicating whether far buy amount enabled.
        /// </summary>
        public bool FarBuyAmountEnabled
        {
            get
            {
                return this.farBuyAmountEnabled;
            }

            set
            {
                this.farBuyAmountEnabled = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CCY2Amount
        /// </summary>
        public string FarCustBuyAmount
        {
            get
            {
                return this.farCustBuyAmount.ToString();
            }

            set
            {
                decimal inSetValue;
                if (!decimal.TryParse(value, out inSetValue))
                {
                    inSetValue = 0;
                }

                this.farCustBuyAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.FarCustBuyAmount);

                if (this.IsDealtSellCCY)
                {
                    if (inSetValue > 0)
                    {
                        this.farCustSellAmount = this.GetCounterAmountByCCY(
                            this.FarCustBuyCCY,
                            this.FarCustSellCCY,
                            inSetValue,
                            this.FarOpenRate);
                        this.NotifyOfPropertyChange(() => this.FarCustSellAmount);
                        this.NotifyOfPropertyChange(() => this.FarCustBuyAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     CustBuyCCY
        /// </summary>
        public string FarCustBuyCCY
        {
            get
            {
                return this.farCustBuyCCY;
            }

            set
            {
                this.farCustBuyCCY = value;
                this.NotifyOfPropertyChange(() => this.FarCustBuyCCY);
            }
        }

        /// <summary>
        ///     CCY1Amount
        /// </summary>
        public string FarCustSellAmount
        {
            get
            {
                return this.farCustSellAmount.ToString();
            }

            set
            {
                decimal inSetValue;
                if (!decimal.TryParse(value, out inSetValue))
                {
                    inSetValue = 0;
                }

                this.farCustSellAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.FarCustSellAmount);

                if (this.IsDealtBuyCCY)
                {
                    if (inSetValue > 0)
                    {
                        this.farCustBuyAmount = this.GetCounterAmountByCCY(
                            this.farCustSellCCY,
                            this.farCustBuyCCY,
                            inSetValue,
                            this.FarOpenRate);
                        this.NotifyOfPropertyChange(() => this.FarCustBuyAmount);
                        this.NotifyOfPropertyChange(() => this.FarCustSellAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     WeSellCCY
        /// </summary>
        public string FarCustSellCCY
        {
            get
            {
                return this.farCustSellCCY;
            }

            set
            {
                this.farCustSellCCY = value;
                this.NotifyOfPropertyChange(() => this.FarCustSellCCY);
            }
        }

        /// <summary>
        ///     Gets or sets the far deal.
        /// </summary>
        public FxHedgingDealModel FarDeal
        {
            get
            {
                return this.farDeal;
            }

            set
            {
                this.farDeal = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the spot rate.
        /// </summary>
        public string FarDealerSpotRate
        {
            get
            {
                return this.FarDeal.DealerSpotRate.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                this.FarDeal.DealerSpotRate = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange(() => this.FarDealerSpotRate);
                this.CalcInputOpenRate(false);
            }
        }

        /// <summary>
        ///     Gets or sets the farward point.
        /// </summary>
        public string FarForwardPointStr
        {
            get
            {
                return this.FarDeal.ForwardPoint.ToString();
            }

            set
            {
                decimal farPoint;
                if (decimal.TryParse(value, out farPoint))
                {
                    this.FarDeal.ForwardPoint = farPoint;
                    this.NotifyOfPropertyChange(() => this.FarForwardPointStr);

                    this.CalcInputOpenRate(false);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the open rate.
        /// </summary>
        public decimal FarOpenRate
        {
            get
            {
                return this.FarDeal.ContractRate;
            }

            set
            {
                this.FarDeal.ContractRate = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange(() => this.FarOpenRate);
                if (this.FarCustSellCCY == null || this.FarCustBuyCCY == null)
                {
                    return;
                }

                if (this.IsDealtSellCCY)
                {
                    if (this.farCustBuyAmount > 0)
                    {
                        this.farCustSellAmount = this.GetCounterAmountByCCY(
                            this.farCustBuyCCY,
                            this.farCustSellCCY,
                            this.farCustBuyAmount,
                            this.FarOpenRate);
                        this.NotifyOfPropertyChange(() => this.FarCustSellAmount);
                    }
                }
                else
                {
                    if (this.farCustSellAmount > 0)
                    {
                        this.farCustBuyAmount = this.GetCounterAmountByCCY(
                            this.farCustSellCCY,
                            this.farCustBuyCCY,
                            this.farCustSellAmount,
                            this.FarOpenRate);
                        this.NotifyOfPropertyChange(() => this.FarCustBuyAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets the sell account.
        /// </summary>
        public SellBankAcctBalanceVM FarSellAccount
        {
            get
            {
                return this.farSellAccount;
            }

            set
            {
                this.farSellAccount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether far sell amount enabled.
        /// </summary>
        public bool FarSellAmountEnabled
        {
            get
            {
                return this.farSellAmountEnabled;
            }

            set
            {
                this.farSellAmountEnabled = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the far value date change command.
        /// </summary>
        public RelayCommand FarValueDateChangeCommand
        {
            get
            {
                return this.farValueDateChangeCommand;
            }

            set
            {
                this.farValueDateChangeCommand = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the hedging deal id.
        /// </summary>
        public string HedgingDealId
        {
            get
            {
                return this.hedgingDealId;
            }

            set
            {
                this.hedgingDealId = value;
                this.NearDeal.HedgingDealId = value;
                this.FarDeal.HedgingDealId = value;
                this.NotifyOfPropertyChange();
            }
        }

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
            }
        }

        /// <summary>
        ///     Gets or sets the local trade date.
        /// </summary>
        public DateTime LocalTradeDate
        {
            get
            {
                return this.localTradeDate;
            }

            set
            {
                this.localTradeDate = value;
                this.NearDeal.LocalTradeDate = value;
                this.FarDeal.LocalTradeDate = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets the local trade date changed command.
        /// </summary>
        public RelayCommand LocalTradeDateChangedCommand { get; private set; }

        /// <summary>
        ///     Gets the near buy amount change command.
        /// </summary>
        public RelayCommand NearBuyAmountChangeCommand { get; private set; }

        /// <summary>
        ///     CCY2Amount
        /// </summary>
        public string NearCustBuyAmount
        {
            get
            {
                return this.nearCustBuyAmount.ToString();
            }

            set
            {
                decimal inSetValue;
                if (!decimal.TryParse(value, out inSetValue))
                {
                    inSetValue = 0;
                }

                this.nearCustBuyAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.NearCustBuyAmount);

                if (this.IsDealtBuyCCY)
                {
                    if (inSetValue > 0 && this.NearCustSellCCY != null)
                    {
                        this.nearCustSellAmount = this.GetCounterAmountByCCY(
                            this.nearCustBuyCCY,
                            this.nearCustSellCCY,
                            inSetValue,
                            this.NearOpenRate);
                        this.NotifyOfPropertyChange(() => this.NearCustBuyAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     CustBuyCCY
        /// </summary>
        public string NearCustBuyCCY
        {
            get
            {
                return this.nearCustBuyCCY;
            }

            set
            {
                this.nearCustBuyCCY = value;
                this.NotifyOfPropertyChange(() => this.NearCustBuyCCY);
            }
        }

        /// <summary>
        ///     CCY1Amount
        /// </summary>
        public string NearCustSellAmount
        {
            get
            {
                return this.nearCustSellAmount.ToString();
            }

            set
            {
                decimal inSetValue;
                if (!decimal.TryParse(value, out inSetValue))
                {
                    inSetValue = 0;
                }

                this.nearCustSellAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.NearCustSellAmount);

                if (this.IsDealtSellCCY)
                {
                    if (inSetValue > 0)
                    {
                        this.nearCustBuyAmount = this.GetCounterAmountByCCY(
                            this.nearCustSellCCY,
                            this.nearCustBuyCCY,
                            inSetValue,
                            this.NearOpenRate);
                        this.NotifyOfPropertyChange(() => this.NearCustBuyAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     WeSellCCY
        /// </summary>
        public string NearCustSellCCY
        {
            get
            {
                return this.nearCustSellCCY;
            }

            set
            {
                this.nearCustSellCCY = value;
                this.NotifyOfPropertyChange(() => this.NearCustSellCCY);
            }
        }

        /// <summary>
        ///     Gets or sets the near deal.
        /// </summary>
        public FxHedgingDealModel NearDeal
        {
            get
            {
                return this.nearDeal;
            }

            set
            {
                this.nearDeal = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the spot rate.
        /// </summary>
        public string NearDealerSpotRate
        {
            get
            {
                return this.NearDeal.DealerSpotRate.ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                this.NearDeal.DealerSpotRate = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange(() => this.NearDealerSpotRate);
                this.CalcInputOpenRate(true);
            }
        }

        /// <summary>
        ///     Gets or sets the farward point.
        /// </summary>
        public string NearForwardPointStr
        {
            get
            {
                return this.NearDeal.ForwardPoint.ToString();
            }

            set
            {
                decimal nearPoint;
                if (decimal.TryParse(value, out nearPoint))
                {
                    this.NearDeal.ForwardPoint = nearPoint;
                    this.NotifyOfPropertyChange(() => this.NearForwardPointStr);

                    this.CalcInputOpenRate(true);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the open rate.
        /// </summary>
        public decimal NearOpenRate
        {
            get
            {
                return this.NearDeal.ContractRate;
            }

            set
            {
                this.NearDeal.ContractRate = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange(() => this.NearOpenRate);
                if (this.NearCustSellCCY == null || this.NearCustBuyCCY == null)
                {
                    return;
                }

                if (this.isDealtBuyCCY)
                {
                    if (this.nearCustBuyAmount > 0)
                    {
                        this.nearCustSellAmount = this.GetCounterAmountByCCY(
                            this.nearCustBuyCCY,
                            this.nearCustSellCCY,
                            this.nearCustBuyAmount,
                            this.NearOpenRate);
                        this.NotifyOfPropertyChange(() => this.NearCustSellAmount);
                    }
                }
                else
                {
                    if (this.nearCustSellAmount > 0)
                    {
                        this.nearCustBuyAmount = this.GetCounterAmountByCCY(
                            this.nearCustSellCCY,
                            this.nearCustBuyCCY,
                            this.nearCustSellAmount,
                            this.NearOpenRate);
                        this.NotifyOfPropertyChange(() => this.NearCustBuyAmount);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets the sell account.
        /// </summary>
        public SellBankAcctBalanceVM NearSellAccount
        {
            get
            {
                return this.nearSellAccount;
            }

            set
            {
                this.nearSellAccount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets the near sell amount change command.
        /// </summary>
        public RelayCommand NearSellAmountChangeCommand { get; private set; }

        /// <summary>
        /// Gets or sets the near value date change command.
        /// </summary>
        public RelayCommand NearValueDateChangeCommand
        {
            get
            {
                return this.nearValueDateChangeCommand;
            }

            set
            {
                this.nearValueDateChangeCommand = value;
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
            this.ResetAmount();
            ContractModel tempSymbol = this.contractRep.GetSymbol(this.nearCustBuyCCY, this.nearCustSellCCY);
            this.typing = this.nearCustBuyCCY == tempSymbol.Ccy1Id ? TypingEnum.CCY1_AMOUNT : TypingEnum.CCY2_AMOUNT;
            this.FarBuyAmountEnabled = true;
            this.FarSellAmountEnabled = false;
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

            ContractModel nearSymbol = this.contractRep.GetSymbol(this.NearCustSellCCY, this.NearCustBuyCCY);
            ContractModel farSymbol = this.contractRep.GetSymbol(this.FarCustSellCCY, this.FarCustBuyCCY);

            this.SetCustomerTyping();

            // all
            this.NearDeal.BusinessUnitId = this.counterParty.BusinessUnitId;
            this.NearDeal.HedgingDealId = this.HedgingDealId;
            this.NearDeal.Comment = this.Comment;
            this.NearDeal.ContractId = nearSymbol.Id;
            this.NearDeal.CounterpartyId = this.CounterParty.Id;
            this.NearDeal.EnterpriseId = this.CounterParty.EnterpriseId;
            this.NearDeal.Typing = this.typing;
            this.NearDeal.InstitutionId = this.CounterParty.InstitutionId;
            this.NearDeal.LocalTradeDate = this.LocalTradeDate.Date;

            // NearLeg
            this.NearDeal.IsNearLeg = (int)IsNearLegEnum.NEAR_LEG;
            this.NearDeal.CounterpartyId = this.CounterParty.Id;
            this.NearDeal.Ccy1Id = nearSymbol.Ccy1Id;
            this.NearDeal.Ccy2Id = nearSymbol.Ccy2Id;
            if (this.NearCustSellCCY == nearSymbol.Ccy1Id)
            {
                this.NearDeal.Ccy1Amount = this.NearCustSellAmount.ToDecimal();
                this.NearDeal.Ccy2Amount = this.NearCustBuyAmount.ToDecimal();
            }
            else
            {
                this.NearDeal.Ccy1Amount = this.NearCustBuyAmount.ToDecimal();
                this.NearDeal.Ccy2Amount = this.NearCustSellAmount.ToDecimal();
            }

            this.NearDeal.TransactionType = this.NearCustSellCCY == nearSymbol.Ccy1Id
                                                ? TransactionTypeEnum.Sell
                                                : TransactionTypeEnum.Buy;
            this.NearDeal.StaffId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;
            this.NearDeal.UserId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;

            // FarLeg
            this.FarDeal.IsNearLeg = (int)IsNearLegEnum.FAR_LEG;
            this.FarDeal.CounterpartyId = this.CounterParty.Id;
            this.FarDeal.Ccy1Id = farSymbol.Ccy1Id;
            this.FarDeal.Ccy2Id = farSymbol.Ccy2Id;
            this.FarDeal.ContractId = farSymbol.Id;
            if (this.FarCustSellCCY == farSymbol.Ccy1Id)
            {
                this.FarDeal.Ccy1Amount = this.FarCustSellAmount.ToDecimal();
                this.FarDeal.Ccy2Amount = this.FarCustBuyAmount.ToDecimal();
            }
            else
            {
                this.FarDeal.Ccy1Amount = this.FarCustBuyAmount.ToDecimal();
                this.FarDeal.Ccy2Amount = this.FarCustSellAmount.ToDecimal();
            }

            this.FarDeal.TransactionType = this.FarCustSellCCY == farSymbol.Ccy1Id
                                               ? TransactionTypeEnum.Sell
                                               : TransactionTypeEnum.Buy;
            this.FarDeal.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            this.FarDeal.StaffId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;
            this.NearDeal.UserId = RunTime.GetCurrentRunTime().CurrentLoginUser.Id;

            string msg = this.BuilderMsg();
            if (RunTime.ShowConfirmDialogWithoutRes(msg, string.Empty, this.OwnerId))
            {
                var service = new HedgingDealService(this.OwnerId);
                CmdResult drs = service.NewSwap(this.NearDeal, this.FarDeal);
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
        public void FarContractRatePlus_Click(string txt)
        {
            this.FarOpenRate = Util.TxtAdd(txt);
        }

        /// <summary>
        /// Contract向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void FarContractRateSub_Click(string txt)
        {
            this.FarOpenRate = Util.TxtSub(txt);
        }

        /// <summary>
        /// ForwardPoint 向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void FarForwardPointPlus_Click(string txt)
        {
            this.FarForwardPointStr = Util.TxtAdd(txt).ToString();
        }

        /// <summary>
        /// ForwardPoint 向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void FarForwardPointSub_Click(string txt)
        {
            this.FarForwardPointStr = Util.TxtSub(txt).ToString();
        }

        /// <summary>
        ///     The far rate change.
        /// </summary>
        public void FarRateChange()
        {
            this.CalcInputOpenRate(false);
        }

        /// <summary>
        /// SpotRate向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void FarSpotRatePlus_Click(string txt)
        {
            this.FarDealerSpotRate = Util.TxtAdd(txt).ToString();
        }

        /// <summary>
        /// SpotRate向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void FarSpotRateSub_Click(string txt)
        {
            this.FarDealerSpotRate = Util.TxtSub(txt).ToString();
        }

        /// <summary>
        /// Contract 向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void NearContractRatePlus_Click(string txt)
        {
            this.NearOpenRate = Util.TxtAdd(txt);
        }

        /// <summary>
        /// Contract向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void NearContractRateSub_Click(string txt)
        {
            this.NearOpenRate = Util.TxtSub(txt);
        }

        /// <summary>
        /// ForwardPoint 向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void NearForwardPointPlus_Click(string txt)
        {
            this.NearForwardPointStr = Util.TxtAdd(txt).ToString();
        }

        /// <summary>
        /// ForwardPoint 向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void NearForwardPointSub_Click(string txt)
        {
            this.NearForwardPointStr = Util.TxtSub(txt).ToString();
        }

        /// <summary>
        ///     The near rate change.
        /// </summary>
        public void NearRateChange()
        {
            this.CalcInputOpenRate(true);
        }

        /// <summary>
        /// SpotRate向上加
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void NearSpotRatePlus_Click(string txt)
        {
            this.NearDealerSpotRate = Util.TxtAdd(txt).ToString();
        }

        /// <summary>
        /// SpotRate向下减
        /// </summary>
        /// <param name="txt">
        /// 传入要修改的参数
        /// </param>
        public void NearSpotRateSub_Click(string txt)
        {
            this.NearDealerSpotRate = Util.TxtSub(txt).ToString();
        }

        /// <summary>
        ///     The sell click.
        /// </summary>
        public void SellClick()
        {
            this.ResetAmount();
            ContractModel tempSymbol = this.contractRep.GetSymbol(this.nearCustBuyCCY, this.nearCustSellCCY);
            this.typing = this.nearCustSellCCY == tempSymbol.Ccy1Id ? TypingEnum.CCY1_AMOUNT : TypingEnum.CCY2_AMOUNT;
            this.FarSellAmountEnabled = true;
            this.FarBuyAmountEnabled = false;
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
                if (this.NearDeal.ValueDate == default(DateTime)
                    || this.NearDeal.ValueDate.DayOfWeek == DayOfWeek.Saturday
                    || this.NearDeal.ValueDate.DayOfWeek == DayOfWeek.Sunday
                    || this.FarDeal.ValueDate == default(DateTime)
                    || this.FarDeal.ValueDate.DayOfWeek == DayOfWeek.Saturday
                    || this.FarDeal.ValueDate.DayOfWeek == DayOfWeek.Sunday)
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
                    return RunTime.FindStringResource("MSG_00010");
                }
            }

            if (propertyName == "NearDealerSpotRate")
            {
                if (string.IsNullOrEmpty(this.NearDealerSpotRate) || Convert.ToDecimal(this.NearDealerSpotRate) <= 0)
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "FarDealerSpotRate")
            {
                if (string.IsNullOrEmpty(this.FarDealerSpotRate) || Convert.ToDecimal(this.FarDealerSpotRate) <= 0)
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "NearCustBuyCCY")
            {
                if (string.IsNullOrEmpty(this.NearCustBuyCCY))
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "NearCustSellCCY")
            {
                if (string.IsNullOrEmpty(this.NearCustSellCCY))
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "NearCustBuyAmount")
            {
                if (string.IsNullOrEmpty(this.NearCustBuyAmount) || Convert.ToDecimal(this.NearCustBuyAmount) <= 0)
                {
                    return RunTime.FindStringResource("MSG_00009");
                }
            }

            if (propertyName == "NearCustSellAmount")
            {
                if (string.IsNullOrEmpty(this.NearCustSellAmount) || Convert.ToDecimal(this.NearCustSellAmount) <= 0)
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
            if (this.LocalTradeDate.Date > this.NearDeal.ValueDate.Date
                || this.LocalTradeDate.Date > this.FarDeal.ValueDate.Date)
            {
                return RunTime.FindStringResource("MSG_10021");
            }

            if (this.NearDeal.ValueDate.Date >= this.FarDeal.ValueDate.Date)
            {
                return RunTime.FindStringResource("MSG_10024");
            }

            ContractModel nearSymbol = this.contractRep.GetSymbol(this.NearCustSellCCY, this.NearCustBuyCCY);
            ContractModel farSymbol = this.contractRep.GetSymbol(this.FarCustSellCCY, this.FarCustBuyCCY);
            if (nearSymbol == null || farSymbol == null)
            {
                return RunTime.FindStringResource("MSG_10059");
            }

            return string.Empty;
        }

        /// <summary>
        ///     The builder msg.
        /// </summary>
        /// <param name="hedgeDealVM">
        ///     The hedge deal vm.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private string BuilderMsg()
        {
            string dealtBuySell = string.Empty;
            string dealtBuySellAmount = string.Empty;
            string counterBuySell = string.Empty;
            string counterBuySellAmount = string.Empty;
            string buySell = string.Empty;
            if (this.isDealtBuyCCY)
            {
                buySell = RunTime.FindStringResource("MsgBuy");
                dealtBuySell = this.NearCustBuyCCY;
                dealtBuySellAmount = this.NearCustBuyAmount;
                counterBuySell = this.NearCustSellCCY;
                counterBuySellAmount = this.NearCustSellAmount;
            }
            else
            {
                buySell = RunTime.FindStringResource("MsgSell");
                dealtBuySell = this.NearCustSellCCY;
                dealtBuySellAmount = this.NearCustSellAmount;
                counterBuySell = this.NearCustBuyCCY;
                counterBuySellAmount = this.NearCustBuyAmount;
            }

            string msg = string.Format(
                RunTime.FindStringResource("MSG_10023"),
                this.GetRepository<ICounterPartyRepository>().GetName(this.nearDeal.CounterpartyId),
                this.nearDeal.HedgingDealId,
                buySell,
                this.currencyRep.GetName(dealtBuySell),
                dealtBuySellAmount,
                this.currencyRep.GetName(counterBuySell),
                counterBuySellAmount,
                this.NearDeal.ContractRate,
                this.FarDeal.ContractRate,
                this.NearDeal.ValueDate.Date.ToString("yyyy-MM-dd"),
                this.FarDeal.ValueDate.Date.ToString("yyyy-MM-dd"));
            return msg;
        }

        /// <summary>
        /// </summary>
        private void BuyCurrencySelectionChanged()
        {
            this.FarCustSellCCY = this.NearCustBuyCCY;
            this.ResetAll();
            this.SetSellAcount();
        }

        /// <summary>
        /// 根据dealer rate+forward point计算 openrate
        /// </summary>
        /// <param name="isNear">
        /// The is Near.
        /// </param>
        private void CalcInputOpenRate(bool isNear)
        {
            decimal forwardPointMathValue;
            if (isNear)
            {
                if (this.NearCustSellCCY == this.NearCustBuyCCY)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.NearCustSellCCY) || string.IsNullOrWhiteSpace(this.NearCustBuyCCY))
                {
                    return;
                }

                ContractModel nearSymbolTemp = this.contractRep.GetSymbol(this.NearCustSellCCY, this.NearCustBuyCCY);
                if (nearSymbolTemp != null)
                {
                    forwardPointMathValue =
                        Convert.ToDecimal(
                            (double)this.NearDeal.ForwardPoint * Math.Pow(10d, 0 - (double)nearSymbolTemp.BasisPoint));
                }
                else
                {
                    forwardPointMathValue = this.NearDeal.DealerSpotRate;
                }

                this.NearOpenRate = this.NearDeal.DealerSpotRate + forwardPointMathValue;
                return;
            }

            if (this.FarCustSellCCY == this.FarCustBuyCCY)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.FarCustSellCCY) || string.IsNullOrWhiteSpace(this.FarCustBuyCCY))
            {
                return;
            }

            ContractModel farSymbolTemp = this.contractRep.GetSymbol(this.FarCustSellCCY, this.FarCustBuyCCY);
            if (farSymbolTemp != null)
            {
                forwardPointMathValue =
                    Convert.ToDecimal(
                        (double)this.FarDeal.ForwardPoint * Math.Pow(10d, 0 - (double)farSymbolTemp.BasisPoint));
            }
            else
            {
                forwardPointMathValue = this.FarDeal.ForwardPoint;
            }

            this.FarOpenRate = this.FarDeal.DealerSpotRate + forwardPointMathValue;
        }

        /// <summary>
        /// The calc input amount.
        /// </summary>
        /// <param name="isBuy">
        /// The is buy.
        /// </param>
        private void CalcNearInputAmount(bool isBuy)
        {
            if (isBuy)
            {
                if (this.isDealtBuyCCY)
                {
                    this.FarCustSellAmount = this.NearCustBuyAmount;
                    if (this.NearOpenRate >= decimal.Zero)
                    {
                        this.CalcInputOpenRate(true);
                    }
                }

                return;
            }

            if (this.IsDealtSellCCY)
            {
                this.FarCustBuyAmount = this.NearCustSellAmount;
                if (this.NearOpenRate >= decimal.Zero)
                {
                    this.CalcInputOpenRate(true);
                }
            }
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
            if (this.businessUnitModel != null)
            {
                this.NearDeal.BusinessUnitId = this.businessUnitModel.Id;
                this.FarDeal.BusinessUnitId = this.businessUnitModel.Id;
            }

            this.SetSellAcount();
        }

        /// <summary>
        /// The far value date change.
        /// </summary>
        private void FarValueDateChange()
        {
            this.SetSellAcount();
        }

        /// <summary>
        /// 根据input amount 计算 cust amount
        /// </summary>
        /// <param name="dealtId">
        /// The dealt Id.
        /// </param>
        /// <param name="countId">
        /// The count Id.
        /// </param>
        /// <param name="ccy1Amount">
        /// 已输入的CCY1Amount
        /// </param>
        /// <param name="rate">
        /// Rate
        /// </param>
        /// <returns>
        /// 自动计算结果
        /// </returns>
        private decimal GetCounterAmountByCCY(string dealtId, string countId, decimal ccy1Amount, decimal rate)
        {
            decimal result;

            if (rate == 0)
            {
                return decimal.Zero;
            }

            if (string.IsNullOrWhiteSpace(dealtId) || string.IsNullOrWhiteSpace(countId))
            {
                return decimal.Zero;
            }

            if (dealtId == countId)
            {
                return ccy1Amount;
            }

            ContractModel symbolTemp = this.contractRep.GetSymbol(dealtId, countId);
            result = dealtId == symbolTemp.Ccy1Id ? ccy1Amount * rate : ccy1Amount / rate;
            CurrencyModel currencyTemp = this.currencyRep.Filter(p => p.Id == countId).FirstOrDefault();
            if (currencyTemp == null)
            {
                return decimal.Zero;
            }

            return result.FormatAmountToDecimalByCurrency(currencyTemp);
        }

        /// <summary>
        ///     The clear.
        /// </summary>
        private void Init()
        {
            // all
            this.NearDeal = new FxHedgingDealModel();
            this.FarDeal = new FxHedgingDealModel();
            this.EntInstrument = new Dictionary<string, string>();
            string spot = string.Empty;
            string forward = string.Empty;
            InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_HEDGING_SWAP_SPOT, this.currentEnterprise)
                .ForEach(
                    o =>
                    {
                        spot = o.Key;
                        this.EntInstrument.Add(o.Key, o.Value);
                    });
            InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_HEDGING_SWAP_FORWARD, this.currentEnterprise)
                .ForEach(
                    o =>
                    {
                        forward = o.Key;
                        this.EntInstrument.Add(o.Key, o.Value);
                    });
            this.CCYList = this.currencyRep.GetBindCollection();
            this.IsDealtBuyCCY = true;
            this.FarBuyAmountEnabled = false;
            this.FarSellAmountEnabled = false;
            if (this.ccyList.Count > 1)
            {
                this.NearCustBuyCCY = this.ccyList[0].Id;
                this.NearCustSellCCY = this.ccyList[1].Id;
                this.FarCustBuyCCY = this.ccyList[1].Id;
                this.FarCustSellCCY = this.ccyList[0].Id;
            }

            if (this.EntInstrument.Any())
            {
                this.NearDeal.Instrument = this.EntInstrument.First(s => s.Key == spot).Key;
                this.FarDeal.Instrument = this.EntInstrument.First(s => s.Key == forward).Key;
            }

            if (this.CounterPartys.Any())
            {
                this.CounterParty = this.CounterPartys[0];
                this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(this.CounterParty.BusinessUnitId);
                this.CounterPartySelectionChanged();
            }

            this.Comment = string.Empty;

            // near
            this.NearDeal.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
            this.NearDeal.ValueDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
            this.NearDealerSpotRate = decimal.Zero.ToString();
            this.NearDeal.ForwardPoint = decimal.Zero;
            this.NearOpenRate = decimal.Zero;
            this.NearDeal.HedgingDealId = string.Empty;
            this.nearCustBuyAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.NearCustBuyAmount);
            this.nearCustSellAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.NearCustSellAmount);
            this.NearDeal.DealerSpotRate = decimal.Zero;
            this.NearDeal.ContractRate = decimal.Zero;

            // far
            this.FarDeal.LocalTradeDate = this.NearDeal.LocalTradeDate.AddDays(7);
            this.FarDeal.ValueDate = this.NearDeal.ValueDate.AddDays(7);
            this.FarDealerSpotRate = decimal.Zero.ToString();
            this.FarDeal.ForwardPoint = decimal.Zero;
            this.FarOpenRate = decimal.Zero;
            this.FarDeal.HedgingDealId = string.Empty;
            this.farCustBuyAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.FarCustBuyAmount);
            this.farCustSellAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.FarCustSellAmount);
            this.FarDeal.DealerSpotRate = decimal.Zero;
            this.FarDeal.ContractRate = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.FarDealerSpotRate);

            this.BuyClick();
        }

        /// <summary>
        ///     The near buy amount change.
        /// </summary>
        private void NearBuyAmountChange()
        {
            this.CalcNearInputAmount(true);
        }

        /// <summary>
        ///     The near sell amount change.
        /// </summary>
        private void NearSellAmountChange()
        {
            this.CalcNearInputAmount(false);
        }

        /// <summary>
        /// The near value date change.
        /// </summary>
        private void NearValueDateChange()
        {
            this.SetSellAcount();
        }

        /// <summary>
        ///     重设所有计算，选择货币时使用
        /// </summary>
        private void ResetAll()
        {
            this.NearCustBuyAmount = decimal.Zero.ToString();
            this.FarCustBuyAmount = decimal.Zero.ToString();

            this.NearCustSellAmount = decimal.Zero.ToString();
            this.FarCustSellAmount = decimal.Zero.ToString();

            this.NearDealerSpotRate = decimal.Zero.ToString();
            this.NearForwardPointStr = decimal.Zero.ToString();

            this.FarDealerSpotRate = decimal.Zero.ToString();
            this.FarForwardPointStr = decimal.Zero.ToString();
        }

        /// <summary>
        ///     重置amount，选中dealt，counter的时候使用
        /// </summary>
        private void ResetAmount()
        {
            this.NearCustBuyAmount = decimal.Zero.ToString();
            this.FarCustBuyAmount = decimal.Zero.ToString();

            this.NearCustSellAmount = decimal.Zero.ToString();
            this.FarCustSellAmount = decimal.Zero.ToString();
        }

        /// <summary>
        /// </summary>
        private void SellCurrencySelectionChanged()
        {
            this.FarCustBuyCCY = this.NearCustSellCCY;
            this.ResetAll();
            this.SetSellAcount();
        }

        /// <summary>
        ///     TODO 这里用的是旧逻辑，需要更新
        ///     The update customer typing.
        /// </summary>
        private void SetCustomerTyping()
        {
            ContractModel nearSymbolTemp = this.contractRep.GetSymbol(this.NearCustSellCCY, this.NearCustBuyCCY);
            if (nearSymbolTemp.Ccy1Id == this.NearCustBuyCCY)
            {
                this.NearDeal.Typing = this.isDealtBuyCCY ? TypingEnum.CCY1_AMOUNT : TypingEnum.CCY2_AMOUNT;
            }
            else
            {
                this.NearDeal.Typing = this.isDealtBuyCCY ? TypingEnum.CCY2_AMOUNT : TypingEnum.CCY1_AMOUNT;
            }

            ContractModel FarSymbolTemp = this.contractRep.GetSymbol(this.FarCustSellCCY, this.FarCustBuyCCY);
            if (FarSymbolTemp.Ccy1Id == this.FarCustBuyCCY)
            {
                this.FarDeal.Typing = this.isDealtBuyCCY ? TypingEnum.CCY1_AMOUNT : TypingEnum.CCY2_AMOUNT;
            }
            else
            {
                this.FarDeal.Typing = this.isDealtBuyCCY ? TypingEnum.CCY2_AMOUNT : TypingEnum.CCY1_AMOUNT;
            }
        }

        /// <summary>
        ///     The set sell acount.
        /// </summary>
        private void SetSellAcount()
        {
            this.NearSellAccount = new SellBankAcctBalanceVM();
            this.FarSellAccount = new SellBankAcctBalanceVM();

            this.NearSellAccount.Currency = this.currencyRep.FindByID(this.NearCustSellCCY);
            this.FarSellAccount.Currency = this.currencyRep.FindByID(this.FarCustSellCCY);
            if (this.CounterParty == null)
            {
                TraceManager.Warn.Write("SetSellAcount", "CounterParty is null");
                return;
            }

            this.NearSellAccount.BankAccount =
                this.settlementBankAccounts.FirstOrDefault(o => o.CurrencyId == this.NearCustSellCCY && o.Enabled);
            this.NearSellAccount.CalcTodayBalance(this.NearDeal.ValueDate.Date, this.counterParty.Id);
            this.FarSellAccount.BankAccount =
                this.settlementBankAccounts.FirstOrDefault(o => o.CurrencyId == this.FarCustSellCCY && o.Enabled);
            this.FarSellAccount.CalcTodayBalance(this.FarDeal.ValueDate.Date, this.counterParty.Id);
        }

        #endregion
    }
}