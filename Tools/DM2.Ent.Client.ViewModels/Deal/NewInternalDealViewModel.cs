// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewInternalDealViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/18 05:30:30 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/18 05:30:30
//      修改描述：新建 NewInternalDealViewModel.cs
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
    using Infrastructure.Data;

    /// <summary>
    ///     The new hedge spot forward view model.
    /// </summary>
    public class NewInternalDealViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The bank account rep.
        /// </summary>
        private readonly IBankAccountRepository bankAccountRep;

        /// <summary>
        ///     The bu rep.
        /// </summary>
        private readonly IBusinessUnitRepository buRep;

        /// <summary>
        ///     The contract rep.
        /// </summary>
        private readonly IContractRepository contractRep;

        /// <summary>
        ///     The counter party rep.
        /// </summary>
        private readonly ICounterPartyRepository counterPartyRep;

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     The current enterprise.
        /// </summary>
        private readonly EnterpriseModel currentEnterprise;

        /// <summary>
        ///     The business unit model.
        /// </summary>
        private BusinessUnitModel businessUnitModel;

        /// <summary>
        ///     The business units.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnits1;

        /// <summary>
        ///     The business units.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnits2;

        /// <summary>
        ///     The buy amount enabled.
        /// </summary>
        private bool buyAmountEnabled;

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
        private CounterPartyModel counterParty1;

        /// <summary>
        ///     The hedge account.
        /// </summary>
        private CounterPartyModel counterParty2;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterPartys1;

        /// <summary>
        ///     The counter partys 2.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterPartys2;

        /// <summary>
        ///     The value date.
        /// </summary>
        /// <summary>
        ///     The we buy amount.
        /// </summary>
        private decimal custBuyAmount;

        /// <summary>
        ///     The value date.
        /// </summary>
        /// <summary>
        ///     The we buy amount.
        /// </summary>
        private decimal custBuyAmount1;

        /// <summary>
        ///     The we buy ccy.
        /// </summary>
        private string custBuyCCY;

        /// <summary>
        ///     The we buy ccy.
        /// </summary>
        private string custBuyCCY1;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal custSellAmount;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal custSellAmount1;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private string custSellCCY;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private string custSellCCY1;

        /// <summary>
        ///     The cealer spot rate.
        /// </summary>
        private decimal dealerSpotRate;

        /// <summary>
        ///     The ent instrument.
        /// </summary>
        private IDictionary<string, string> entInstrument;

        /// <summary>
        ///     The instrument.
        /// </summary>
        /// <summary>
        ///     instrument 下拉列表
        /// </summary>
        private Dictionary<int, string> instrumentEnum;

        /// <summary>
        ///     The is we sell amount enable.
        /// </summary>
        /// <summary>
        ///     The is dealt wbccy.
        /// </summary>
        private bool? isDealtBuyCCY;

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
        ///     The selected business unit 1.
        /// </summary>
        private BusinessUnitModel selectedBusinessUnit1;

        /// <summary>
        ///     The selected business unit 2.
        /// </summary>
        private BusinessUnitModel selectedBusinessUnit2;

        /// <summary>
        ///     The sell account.
        /// </summary>
        private SellBankAcctBalanceVM sellAccount;

        /// <summary>
        ///     The sell amount enabled.
        /// </summary>
        private bool sellAmountEnabled;

        /// <summary>
        ///     Sell  CCy  选中的index
        /// </summary>
        private int sellCCYIndex = -1;

        /// <summary>
        ///     The typing.
        /// </summary>
        private TypingEnum typing;

        /// <summary>
        ///     The value date.
        /// </summary>
        private DateTime valueDate;

        /// <summary>
        ///     The near value date change command.
        /// </summary>
        private RelayCommand valueDateChangeCommand;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewInternalDealViewModel"/> class.
        /// </summary>
        /// <param name="ownerID">
        /// The owner id.
        /// </param>
        public NewInternalDealViewModel(string ownerID = null)
        {
            this.DisplayName = RunTime.FindStringResource("NewInternalDeal");
            if (RunTime.GetCurrentRunTime().CurrentLoginUser == null)
            {
                return;
            }

            this.currencyRep = this.GetRepository<ICurrencyRepository>();
            this.contractRep = this.GetRepository<IContractRepository>();
            this.buRep = this.GetRepository<IBusinessUnitRepository>();
            this.counterPartyRep = this.GetRepository<ICounterPartyRepository>();
            this.currentEnterprise =
                this.GetRepository<IEnterpriseRepository>().FindByID(RunTime.GetCurrentRunTime().CurrentLoginUser.EntId);
            this.bankAccountRep = this.GetRepository<IBankAccountRepository>();
            this.CCYList = this.currencyRep.GetBindCollection();
            this.Bu1SelectionChangedCommand = new RelayCommand(this.Bu1SelectionChanged);
            this.Bu2SelectionChangedCommand = new RelayCommand(this.Bu2SelectionChanged);
            this.BuyCurrencySelectionChangedCommand = new RelayCommand(this.BuyCurrencySelectionChanged);
            this.SellCurrencySelectionChangedCommand = new RelayCommand(this.SellCurrencySelectionChanged);
            this.RateChangeCommand = new RelayCommand(this.RateChange);
            this.BusinessUnits1 = new ObservableCollection<BusinessUnitModel>();
            this.BusinessUnits2 = new ObservableCollection<BusinessUnitModel>();
            this.CounterPartys1 = new ObservableCollection<CounterPartyModel>();
            this.CounterPartys2 = new ObservableCollection<CounterPartyModel>();

            this.BusinessUnits1 = new ObservableCollection<BusinessUnitModel>();
            this.BusinessUnits2 = new ObservableCollection<BusinessUnitModel>();
            this.buRep.GetBindCollection().ForEach(s => this.BusinessUnits1.Add(s));
            if (this.BusinessUnits1 != null && this.BusinessUnits1.Any())
            {
                this.SelectedBusinessUnit1 = this.BusinessUnits1[0];
            }

            this.CounterPartys1 = this.SelectedBusinessUnit1 == null
                                      ? this.counterPartyRep.GetBindCollection().ToComboboxBinding()
                                      : this.counterPartyRep.Filter(
                                          o => o.BusinessUnitId == this.SelectedBusinessUnit1.Id).ToComboboxBinding();
            if (this.CounterPartys1.Any())
            {
                this.CounterParty1 = this.CounterPartys1[0];
            }

            this.buRep.GetBindCollection().ForEach(
                s =>
                {
                    if (this.SelectedBusinessUnit1.Id != s.Id)
                    {
                        this.BusinessUnits2.Add(s);
                    }
                });
            if (this.BusinessUnits2 != null && this.BusinessUnits2.Any())
            {
                this.SelectedBusinessUnit2 = this.BusinessUnits2[0];
            }

            this.CounterPartys2 = this.SelectedBusinessUnit2 == null
                                      ? this.counterPartyRep.GetBindCollection().ToComboboxBinding()
                                      : this.counterPartyRep.Filter(
                                          o => o.BusinessUnitId == this.SelectedBusinessUnit2.Id).ToComboboxBinding();
            if (this.CounterPartys2.Any())
            {
                this.CounterParty2 = this.CounterPartys2[0];
            }

            this.Init();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the bu 1 selection changed command.
        /// </summary>
        public RelayCommand Bu1SelectionChangedCommand { get; private set; }

        /// <summary>
        ///     Gets the bu 2 selection changed command.
        /// </summary>
        public RelayCommand Bu2SelectionChangedCommand { get; private set; }

        /// <summary>
        ///     已取消事件
        /// </summary>
        /// <summary>
        ///     已确认事件
        /// </summary>
        /// <summary>
        ///     BuyCCY在CCYList中的索引号
        /// </summary>
        public ObservableCollection<BusinessUnitModel> BusinessUnits1
        {
            get
            {
                return this.businessUnits1;
            }

            set
            {
                this.businessUnits1 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the business units 2.
        /// </summary>
        public ObservableCollection<BusinessUnitModel> BusinessUnits2
        {
            get
            {
                return this.businessUnits2;
            }

            set
            {
                this.businessUnits2 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     WeSellCCY
        /// </summary>
        public bool BuyAmountEnabled
        {
            get
            {
                return this.buyAmountEnabled;
            }

            set
            {
                this.buyAmountEnabled = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the buy ccy index.
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
        public CounterPartyModel CounterParty1
        {
            get
            {
                return this.counterParty1;
            }

            set
            {
                this.counterParty1 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     HedgeAccount实体
        /// </summary>
        public CounterPartyModel CounterParty2
        {
            get
            {
                return this.counterParty2;
            }

            set
            {
                this.counterParty2 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     对冲账户列表
        /// </summary>
        public ObservableCollection<CounterPartyModel> CounterPartys1
        {
            get
            {
                return this.counterPartys1;
            }

            set
            {
                this.counterPartys1 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     对冲账户列表
        /// </summary>
        public ObservableCollection<CounterPartyModel> CounterPartys2
        {
            get
            {
                return this.counterPartys2;
            }

            set
            {
                this.counterPartys2 = value;
                this.NotifyOfPropertyChange();
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

                if (inSetValue == 0 && this.isDealtBuyCCY != null)
                {
                    this.isDealtBuyCCY = false;
                    this.BuyAmountEnabled = false;
                    this.SellAmountEnabled = true;
                    this.ResetAmount();
                    return;
                }

                if (inSetValue != 0 && this.isDealtBuyCCY == null)
                {
                    this.isDealtBuyCCY = true;
                    this.BuyAmountEnabled = true;
                    this.SellAmountEnabled = false;
                }

                this.custBuyAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.CustBuyAmount);
                if (inSetValue > 0 && this.CustSellCCY != null)
                {
                    this.custSellAmount = this.GetCCY1AmountByCCY2Rate(inSetValue, this.dealerSpotRate);
                    this.NotifyOfPropertyChange(() => this.CustSellAmount);
                }
            }
        }

        /// <summary>
        ///     CCY2Amount
        /// </summary>
        public string CustBuyAmount1
        {
            get
            {
                return this.custBuyAmount1.ToString();
            }

            set
            {
                this.custBuyAmount1 = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange();
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
        ///     CustBuyCCY
        /// </summary>
        public string CustBuyCCY1
        {
            get
            {
                return this.custBuyCCY1;
            }

            set
            {
                this.custBuyCCY1 = value;
                this.NotifyOfPropertyChange();
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

                if (inSetValue == 0 && this.isDealtBuyCCY != null)
                {
                    this.isDealtBuyCCY = true;
                    this.BuyAmountEnabled = true;
                    this.SellAmountEnabled = false;
                    this.ResetAmount();
                    return;
                }

                if (inSetValue != 0 && this.isDealtBuyCCY == null)
                {
                    this.isDealtBuyCCY = false;
                    this.BuyAmountEnabled = false;
                    this.SellAmountEnabled = true;
                }

                this.custSellAmount = inSetValue;
                this.NotifyOfPropertyChange(() => this.CustSellAmount);
                if (inSetValue > 0 && this.CustSellCCY != null)
                {
                    this.custBuyAmount = this.GetCCY2AmountByCCY1Rate(inSetValue, this.dealerSpotRate);
                    this.NotifyOfPropertyChange(() => this.CustBuyAmount);
                }
            }
        }

        /// <summary>
        ///     CCY2Amount
        /// </summary>
        public string CustSellAmount1
        {
            get
            {
                return this.custSellAmount1.ToString();
            }

            set
            {
                this.custSellAmount1 = Convert.ToDecimal(value);
                this.NotifyOfPropertyChange();
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
        ///     WeSellCCY
        /// </summary>
        public string CustSellCCY1
        {
            get
            {
                return this.custSellCCY1;
            }

            set
            {
                this.custSellCCY1 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the open rate.
        /// </summary>
        public string DealerSpotRate
        {
            get
            {
                return this.dealerSpotRate.ToString();
            }

            set
            {
                this.dealerSpotRate = Convert.ToDecimal(value);
                this.RateChange();
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     交易日
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
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     The near rate change command.
        /// </summary>
        public RelayCommand RateChangeCommand { get; private set; }

        /// <summary>
        ///     Gets or sets the selected business unit 1.
        /// </summary>
        public BusinessUnitModel SelectedBusinessUnit1
        {
            get
            {
                return this.selectedBusinessUnit1;
            }

            set
            {
                this.selectedBusinessUnit1 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the selected business unit 2.
        /// </summary>
        public BusinessUnitModel SelectedBusinessUnit2
        {
            get
            {
                return this.selectedBusinessUnit2;
            }

            set
            {
                this.selectedBusinessUnit2 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     WeSellCCY
        /// </summary>
        public bool SellAmountEnabled
        {
            get
            {
                return this.sellAmountEnabled;
            }

            set
            {
                this.sellAmountEnabled = value;
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
        ///     Sell  Currency2 选中事件
        /// </summary>
        public RelayCommand SellCurrencySelectionChangedCommand { get; private set; }

        /// <summary>
        ///     交割日
        /// </summary>
        public DateTime ValueDate
        {
            get
            {
                return this.valueDate;
            }

            set
            {
                this.valueDate = value;
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
        ///     The cancel_ click.
        /// </summary>
        public void Cancel_Click()
        {
            this.TryClose();
        }

        /// <summary>
        ///     The change other amount.
        /// </summary>
        public void ChangeOtherAmount()
        {
            this.custBuyAmount1 = this.custSellAmount;
            this.custSellAmount1 = this.custBuyAmount;
            this.NotifyOfPropertyChange("CustBuyAmount1");
            this.NotifyOfPropertyChange("CustSellAmount1");
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

            ContractModel symbol = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);

            string ccy1Id = symbol.Ccy1Id;
            string ccy2Id = symbol.Ccy2Id;
            decimal ccy1Amount = decimal.Zero;
            decimal ccy2Amount = decimal.Zero;
            if (this.CustSellCCY == symbol.Ccy1Id)
            {
                ccy1Amount = this.custSellAmount;
                ccy2Amount = this.custBuyAmount;
            }
            else
            {
                ccy1Amount = this.custBuyAmount;
                ccy2Amount = this.custSellAmount;
            }

            TransactionTypeEnum transactionType = this.CustSellCCY == symbol.Ccy1Id
                                                      ? TransactionTypeEnum.Sell
                                                      : TransactionTypeEnum.Buy;

            string msg = this.BuilderMsg();
            if (RunTime.ShowConfirmDialogWithoutRes(msg, string.Empty, this.OwnerId))
            {
                var service = new InternalDealService(this.OwnerId);
                CmdResult drs = service.NewInternalDeal(
                    this.SelectedBusinessUnit1.Id.ToInt32(),
                    ccy1Amount.ToString(),
                    ccy1Id.ToInt32(),
                    ccy2Amount.ToString(),
                    ccy2Id.ToInt32(),
                    this.Comment,
                    symbol.Id.ToInt32(),
                    this.DealerSpotRate,
                    this.CounterParty1.Id.ToInt32(),
                    this.DealerSpotRate,
                    this.currentEnterprise.Id.ToInt32(),
                    "0",
                    this.counterParty1.InstitutionId.ToInt32(),
                    InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_INTERNAL_SPOT)
                        .FirstOrDefault()
                        .Key.ToInt32(),
                    this.LocalTradeDate.Date,
                    this.SelectedBusinessUnit2.Id.ToInt32(),
                    this.CounterParty2.Id.ToInt32(),
                    this.counterParty2.InstitutionId.ToInt32(),
                    0,
                    (int)transactionType,
                    (int)this.typing,
                    this.ValueDate);
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
        ///     客户输入CCY2Amount
        /// </summary>
        public void FocusedBuyAmount()
        {
            this.UpdateCustomerTyping();
            this.ChangeOtherAmount();
        }

        /// <summary>
        ///     客户输入CCY1Amount
        /// </summary>
        public void FocusedSellAmount()
        {
            this.UpdateCustomerTyping();
            this.ChangeOtherAmount();
        }

        /// <summary>
        ///     The rate change.
        /// </summary>
        public void RateChange()
        {
            if (this.CustSellCCY == null || this.CustBuyCCY == null || this.isDealtBuyCCY == null)
            {
                return;
            }

            if (this.isDealtBuyCCY.Value)
            {
                if (this.custBuyAmount > 0)
                {
                    this.custSellAmount = this.GetCCY1AmountByCCY2Rate(this.custBuyAmount, this.dealerSpotRate);
                    this.custBuyAmount1 = this.custSellAmount;
                    this.NotifyOfPropertyChange(() => this.CustBuyAmount1);
                    this.NotifyOfPropertyChange(() => this.CustSellAmount);
                }
            }
            else
            {
                if (this.custSellAmount > 0)
                {
                    this.custBuyAmount = this.GetCCY2AmountByCCY1Rate(this.custSellAmount, this.dealerSpotRate);
                    this.custSellAmount1 = this.custBuyAmount;
                    this.NotifyOfPropertyChange(() => this.custSellAmount1);
                    this.NotifyOfPropertyChange(() => this.CustBuyAmount);
                }
            }
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
                this.typing = this.isDealtBuyCCY.Value ? TypingEnum.CCY1_AMOUNT : TypingEnum.CCY2_AMOUNT;
            }
            else
            {
                this.typing = this.isDealtBuyCCY.Value ? TypingEnum.CCY2_AMOUNT : TypingEnum.CCY1_AMOUNT;
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

            if (propertyName == "CounterParty1" || propertyName == "CounterParty2")
            {
                if (this.CounterParty1 == null || this.CounterParty2 == null)
                {
                    return RunTime.FindStringResource("交易对手不存在，请重新选择");
                }
            }

            if (propertyName == "DealerSpotRate")
            {
                if (this.dealerSpotRate <= 0)
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

            if (this.CounterParty1 == null || this.CounterParty2 == null)
            {
                return RunTime.FindStringResource("MSG_10051");
            }

            ContractModel symbol = this.contractRep.GetSymbol(this.CustSellCCY, this.CustBuyCCY);
            if (symbol == null)
            {
                return RunTime.FindStringResource("MSG_10059");
            }

            return string.Empty;
        }

        /// <summary>
        ///     The bu 1 selection changed.
        /// </summary>
        private void Bu1SelectionChanged()
        {
            this.buRep.GetBindCollection().ForEach(
                s =>
                {
                    if (this.BusinessUnits2.Any(o => o.Id == s.Id) == false)
                    {
                        this.BusinessUnits2.Add(s);
                    }
                });
            BusinessUnitModel temp = this.BusinessUnits2.FirstOrDefault(o => o.Id == this.SelectedBusinessUnit1.Id);
            if (temp != null)
            {
                this.BusinessUnits2.Remove(temp);
            }

            // if (this.CounterPartys1.Any())
            // {
            // this.CounterPartys1.Clear();
            // }
            this.CounterPartys1 = this.SelectedBusinessUnit1 == null
                                      ? this.counterPartyRep.GetBindCollection().ToComboboxBinding()
                                      : this.counterPartyRep.Filter(
                                          o => o.BusinessUnitId == this.SelectedBusinessUnit1.Id).ToComboboxBinding();
        }

        /// <summary>
        ///     The bu 2 selection changed.
        /// </summary>
        private void Bu2SelectionChanged()
        {
            this.buRep.GetBindCollection().ForEach(
                s =>
                {
                    if (this.BusinessUnits1.Any(o => o.Id == s.Id) == false)
                    {
                        this.BusinessUnits1.Add(s);
                    }
                });
            BusinessUnitModel temp = this.BusinessUnits1.FirstOrDefault(o => o.Id == this.SelectedBusinessUnit2.Id);
            if (temp != null)
            {
                this.BusinessUnits1.Remove(temp);
            }

            // if (this.CounterPartys2.Any())
            // {
            // this.CounterPartys2.Clear();
            // }
            this.CounterPartys2 = this.SelectedBusinessUnit2 == null
                                      ? this.counterPartyRep.GetBindCollection().ToComboboxBinding()
                                      : this.counterPartyRep.Filter(
                                          o => o.BusinessUnitId == this.SelectedBusinessUnit2.Id).ToComboboxBinding();
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
            if (this.isDealtBuyCCY.Value)
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
                RunTime.FindStringResource("MSG_10046"),
                this.GetRepository<ICounterPartyRepository>().GetName(this.CounterParty1.Id),
                this.GetRepository<ICounterPartyRepository>().GetName(this.CounterParty2.Id),
                buySell,
                this.currencyRep.GetName(dealtBuySell),
                dealtBuySellAmount,
                this.currencyRep.GetName(counterBuySell),
                counterBuySellAmount,
                this.DealerSpotRate,
                this.ValueDate.Date.FormatDateByBu());
            return msg;
        }

        /// <summary>
        /// </summary>
        private void BuyCurrencySelectionChanged()
        {
            this.custBuyAmount = decimal.Zero;
            this.custSellAmount = decimal.Zero;
            this.custBuyAmount1 = decimal.Zero;
            this.custSellAmount1 = decimal.Zero;
            this.CustSellCCY1 = this.custBuyCCY;
            this.NotifyOfPropertyChange("CustBuyAmount1");
            this.NotifyOfPropertyChange("CustSellAmount1");
            this.NotifyOfPropertyChange("CustBuyAmount");
            this.NotifyOfPropertyChange("CustSellAmount");
            this.NotifyOfPropertyChange("CustSellCCY1");
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
            this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;
            this.ValueDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu().Date;

            if (this.ccyList.Count > 1)
            {
                this.CustBuyCCY = this.ccyList[0].Id;
                this.CustSellCCY = this.ccyList[1].Id;
                this.CustBuyCCY1 = this.ccyList[1].Id;
                this.CustSellCCY1 = this.ccyList[0].Id;
            }

            this.Comment = string.Empty;
            this.custBuyAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustBuyAmount);
            this.custSellAmount = decimal.Zero;
            this.NotifyOfPropertyChange(() => this.CustSellAmount);
            this.dealerSpotRate = decimal.Zero;
            this.NotifyOfPropertyChange("DealerSpotRate");
            this.isDealtBuyCCY = null;
            this.BuyAmountEnabled = true;
            this.SellAmountEnabled = true;
        }

        /// <summary>
        ///     The reset amount.
        /// </summary>
        private void ResetAmount()
        {
            this.custBuyAmount = decimal.Zero;
            this.custSellAmount = decimal.Zero;
            this.custBuyAmount1 = decimal.Zero;
            this.custSellAmount1 = decimal.Zero;
            this.NotifyOfPropertyChange("CustBuyAmount");
            this.NotifyOfPropertyChange("CustSellAmount");
            this.NotifyOfPropertyChange("CustBuyAmount1");
            this.NotifyOfPropertyChange("CustSellAmount1");
        }

        /// <summary>
        /// </summary>
        private void SellCurrencySelectionChanged()
        {
            this.custBuyAmount = decimal.Zero;
            this.custSellAmount = decimal.Zero;
            this.custBuyAmount1 = decimal.Zero;
            this.custSellAmount1 = decimal.Zero;
            this.CustBuyCCY1 = this.custSellCCY;
            this.NotifyOfPropertyChange("CustBuyAmount1");
            this.NotifyOfPropertyChange("CustSellAmount1");
            this.NotifyOfPropertyChange("CustBuyAmount");
            this.NotifyOfPropertyChange("CustSellAmount");
            this.NotifyOfPropertyChange("CustBuyCCY1");
        }

        #endregion
    }
}