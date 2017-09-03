// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DailyExposureViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Caliburn.Micro;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Infrastructure.Common.Enums;
    using Infrastructure.Log;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    /// <summary>
    ///     每日资金敞口
    /// </summary>
    public class DailyExposureViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The business unit repository.
        /// </summary>
        protected readonly IBusinessUnitRepository businessUnitRepository;

        /// <summary>
        ///     The bank account repository.
        /// </summary>
        private readonly IBankAccountRepository bankAccountRepository;

        /// <summary>
        ///     The counter party repository.
        /// </summary>
        private readonly ICounterPartyRepository counterPartyRepository;

        /// <summary>
        ///     The currency repository.
        /// </summary>
        private readonly ICurrencyRepository currencyRepository;

        /// <summary>
        ///     The hedging deal repository.
        /// </summary>
        private readonly IFxHedgingDealRepository hedgingDealRepository;

        /// <summary>
        ///     The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The daily statistics list by business unit.
        /// </summary>
        private ObservableCollection<DailyStatisticsViewModel> dailyStatisticsListByBusinessUnit;

        /// <summary>
        ///     The daily statistics list by counter party.
        /// </summary>
        private ObservableCollection<DailyStatisticsViewModel> dailyStatisticsListByCounterParty;

        /// <summary>
        /// The display hkd column.
        /// </summary>
        private Visibility displayHkdColumn = Visibility.Collapsed;

        /// <summary>
        /// The display jpy column.
        /// </summary>
        private Visibility displayJpyColumn = Visibility.Collapsed;

        /// <summary>
        ///     The selected contract summary.
        /// </summary>
        private DailyStatisticsViewModel selectedDailyStatisticsByCounterParty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyExposureViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public DailyExposureViewModel(string ownerId = null)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("DailyExposure");

            this.hedgingDealRepository = this.GetRepository<IFxHedgingDealRepository>();
            this.bankAccountRepository = this.GetRepository<IBankAccountRepository>();
            this.counterPartyRepository = this.GetRepository<ICounterPartyRepository>();
            this.businessUnitRepository = this.GetRepository<IBusinessUnitRepository>();
            this.currencyRepository = this.GetRepository<ICurrencyRepository>();

            RunTime currentRunTime = RunTime.GetCurrentRunTime(ownerId);

            this.ContainHedgingDealPosition = true;
            this.SearchValueDate = currentRunTime.GetCurrentTimeForCurrentUserBu().Date;

            this.Initialize(currentRunTime);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the daily statistics list by business unit.
        /// </summary>
        public ObservableCollection<DailyStatisticsViewModel> DailyStatisticsListByBusinessUnit
        {
            get
            {
                return this.dailyStatisticsListByBusinessUnit;
            }

            set
            {
                this.dailyStatisticsListByBusinessUnit = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the daily statistics list by counter party.
        /// </summary>
        public ObservableCollection<DailyStatisticsViewModel> DailyStatisticsListByCounterParty
        {
            get
            {
                return this.dailyStatisticsListByCounterParty;
            }

            set
            {
                this.dailyStatisticsListByCounterParty = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the display hkd column.
        /// </summary>
        public Visibility DisplayHkdColumn
        {
            get
            {
                return this.displayHkdColumn;
            }

            set
            {
                this.displayHkdColumn = value;
                this.NotifyOfPropertyChange(() => this.DisplayHkdColumn);
            }
        }

        /// <summary>
        /// Gets or sets the display jpy column.
        /// </summary>
        public Visibility DisplayJpyColumn
        {
            get
            {
                return this.displayJpyColumn;
            }

            set
            {
                this.displayJpyColumn = value;
                this.NotifyOfPropertyChange(() => this.DisplayJpyColumn);
            }
        }

        /// <summary>
        /// Gets or sets the selected daily statistics by counter party.
        /// </summary>
        public DailyStatisticsViewModel SelectedDailyStatisticsByCounterParty
        {
            get
            {
                return this.selectedDailyStatisticsByCounterParty;
            }

            set
            {
                this.selectedDailyStatisticsByCounterParty = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether business unit statistics enabled.
        /// </summary>
        protected bool BusinessUnitStatisticsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether contain hedging deal position.
        /// </summary>
        protected bool ContainHedgingDealPosition { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether contain internal deal position.
        /// </summary>
        protected bool ContainInternalDealPosition { get; set; }

        /// <summary>
        ///     Gets or sets the search business unit id.
        /// </summary>
        protected string SearchBusinessUnitId { get; set; }

        /// <summary>
        ///     Gets or sets the search value date.
        /// </summary>
        public DateTime SearchValueDate { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The new bank account tranfer.
        /// </summary>
        /// <param name="counterPartyId">
        /// The counter party id.
        /// </param>
        /// <param name="currencyId">
        /// The currency id.
        /// </param>
        public void NewBankAccountTranfer(string counterPartyId, string currencyId)
        {
            if (string.IsNullOrEmpty(counterPartyId))
            {
                return;
            }

            var bankAccountTransfer = new BankAccountTransferViewModel();
            BankAccountModel bankAccount =
                this.bankAccountRepository.Filter(
                    item => item.CounterpartyId == counterPartyId && item.CurrencyId == currencyId).FirstOrDefault();
            if (bankAccount != null)
            {
                bankAccountTransfer.ToBankAccount = bankAccount;
            }

            this.windowManager.ShowWindow(bankAccountTransfer);
        }

        public void ShowDailyExposurePositionDetail(string counterPartyId, string currencyId)
        {
            if (string.IsNullOrEmpty(counterPartyId))
            {
                return;
            }

            var vm = new DealListViewModel(this.OwnerId);
            this.windowManager.ShowWindow(vm);
            vm.InitWithDailyExposureByCounterParty(counterPartyId, currencyId, this.SearchValueDate);
        }

        public void ShowDailyExposurePositionDetailByBusinessUnit(string businessUnitId, string currencyId)
        {
            if (string.IsNullOrEmpty(businessUnitId))
            {
                return;
            }

            var vm = new DealListViewModel(this.OwnerId);
            this.windowManager.ShowWindow(vm);
            vm.InitWithDailyExposureByBusinessUnit(businessUnitId, currencyId, this.SearchValueDate);
        }

        public void ShowSettlementAccountDetail(string businessUnitId, string currencyId)
        {
            if (string.IsNullOrEmpty(businessUnitId))
            {
                return;
            }

            var vm = new BankAccountDetailViewModel(this.OwnerId);
            this.windowManager.ShowWindow(vm);
            vm.InitWithSettlementAccount(businessUnitId, currencyId);
        }

        public void ShowNonSettlementAccountDetail(string businessUnitId, string currencyId)
        {
            if (string.IsNullOrEmpty(businessUnitId))
            {
                return;
            }

            var vm = new BankAccountDetailViewModel(this.OwnerId);
            this.windowManager.ShowWindow(vm);
            vm.InitWithNonSettlementAccount(businessUnitId, currencyId);
        }
        #endregion

        #region Methods

        /// <summary>
        ///     The data binding for business unit.
        /// </summary>
        protected void DataBindingForBusinessUnit()
        {
            if (!this.BusinessUnitStatisticsEnabled)
            {
                return;
            }

            List<Summary> bankAccounts =
                this.bankAccountRepository.Filter(this.IsSettlementAccount)
                    .Join(
                        this.counterPartyRepository.Filter(this.FilteCounterParty), 
                        left => left.CounterpartyId, 
                        right => right.Id, 
                        (left, right) => new { right.BusinessUnitId, left.CurrencyId, left.AvailableBalance })
                    .GroupBy(item => new { item.BusinessUnitId, item.CurrencyId })
                    .Select(
                        group =>
                        new Summary
                            {
                                Id = group.Key.BusinessUnitId, 
                                CurrencyId = group.Key.CurrencyId, 
                                SettlementAccountBalance = group.Sum(item => item.AvailableBalance)
                            })
                    .ToList();

            this.bankAccountRepository.Filter(this.FilteNonSettlementAccount)
                .Select(item => new { item.BusinessUnitId, item.CurrencyId, item.AvailableBalance })
                .GroupBy(item => new { item.BusinessUnitId, item.CurrencyId })
                .Select(
                    group =>
                    new
                        {
                            group.Key.BusinessUnitId, 
                            group.Key.CurrencyId, 
                            NonSettlementAccountBalance = group.Sum(item => item.AvailableBalance)
                        })
                .ForEach(
                    item =>
                        {
                            Summary bankAccount =
                                bankAccounts.FirstOrDefault(
                                    acc => acc.Id == item.BusinessUnitId && acc.CurrencyId == item.CurrencyId);
                            if (bankAccount == null)
                            {
                                bankAccount = new Summary { Id = item.BusinessUnitId, CurrencyId = item.CurrencyId };
                                bankAccounts.Add(bankAccount);
                            }

                            bankAccount.NonSettlementAccountBalance = item.NonSettlementAccountBalance;
                        });

            // var bankAccounts =
            // this.bankAccountRepository.Filter(this.FilteBankAccount)
            // .Select(this.ConvertForBusinessUnit)
            // .GroupBy(item => new { item.Id, item.CurrencyId })
            // .Select(
            // group =>
            // new Summary
            // {
            // Id = group.Key.Id, 
            // CurrencyId = group.Key.CurrencyId, 
            // SettlementAccountBalance = group.Sum(item => item.SettlementAccountBalance), 
            // NonSettlementAccountBalance = group.Sum(item => item.NonSettlementAccountBalance)
            // })
            // .ToArray();
            var hedgingDeals = new Summary[0];
            if (this.ContainHedgingDealPosition)
            {
                hedgingDeals =
                    this.hedgingDealRepository.Filter(item => this.FilteHedgingDeal(item, StatusEnum.OPERN))
                        .SelectMany(this.ConvertForBusinessUnit)
                        .GroupBy(item => new { item.Id, item.CurrencyId })
                        .Select(
                            group =>
                            new Summary
                                {
                                    Id = group.Key.Id, 
                                    CurrencyId = group.Key.CurrencyId, 
                                    Position = group.Sum(item => item.Position)
                                })
                        .ToArray();
            }

            var query = from bankAccount in bankAccounts
                        join hedgingDeal in hedgingDeals on new { bankAccount.Id, bankAccount.CurrencyId } equals
                            new { hedgingDeal.Id, hedgingDeal.CurrencyId } into temp
                        from deal in temp.DefaultIfEmpty()
                        select
                            new
                                {
                                    BusinessUnitId = bankAccount.Id, 
                                    bankAccount.CurrencyId, 
                                    bankAccount.SettlementAccountBalance, 
                                    bankAccount.NonSettlementAccountBalance, 
                                    DealPosition = deal == null ? decimal.Zero : deal.Position
                                };

            List<DailyStatisticsViewModel> list = query.GroupBy(item => item.BusinessUnitId).Select(
                group =>
                    {
                        DailyStatisticsViewModel dailyStatistics = this.CreateByBusinessUnitId(group.Key);
                        dailyStatistics.CurrencyDetails =
                            group.ToDictionary(
                                item => this.currencyRepository.GetName(item.CurrencyId), 
                                item =>
                                new CurrencyDetailModel(
                                    item.CurrencyId, 
                                    item.SettlementAccountBalance, 
                                    item.DealPosition, 
                                    item.NonSettlementAccountBalance));
                        return dailyStatistics;
                    })
                .Where(item => item.CurrencyDetails.Values.Any(p => !p.IsEmpty()))
                .OrderBy(item => item.CounterPartyName)
                .ToList();

            list.Sort(DailyStatisticsViewModel.Comparer);

            if (list.Count > 0)
            {
                Dictionary<string, CurrencyDetailModel> total = list.SelectMany(item => item.CurrencyDetails.Values)
                    .GroupBy(item => item.CurrencyId)
                    .Select(
                        group =>
                            {
                                decimal sumSettlementAccountBalance = group.Sum(item => item.SettlementAccountBalance);
                                decimal sumDealPosition = group.Sum(item => item.Position);
                                decimal sumNonSettlementAccountBalance =
                                    group.Sum(item => item.NonSettlementAccountBalance);
                                return new CurrencyDetailModel(
                                    group.Key, 
                                    sumSettlementAccountBalance, 
                                    sumDealPosition, 
                                    sumNonSettlementAccountBalance);
                            }).ToDictionary(item => this.currencyRepository.GetName(item.CurrencyId));

                list.Add(
                    new DailyStatisticsViewModel
                        {
                            BusinessUnitId = string.Empty,
                            BusinessUnitName = RunTime.FindStringResource("Total"), 
                            CurrencyDetails = total
                        });
            }

            this.DailyStatisticsListByBusinessUnit = new ObservableCollection<DailyStatisticsViewModel>(list);
        }

        /// <summary>
        ///     绑定数据.
        /// </summary>
        protected void DataBindingForCounterParty()
        {
            // var bankAccounts =
            // this.bankAccountRepository.Filter(this.FilteSettlementAccount)
            // .Select(this.ConvertForCounterParty)
            // .ToArray();
            IEnumerable<Summary> bankAccounts =
                this.bankAccountRepository.Filter(this.IsSettlementAccount)
                    .Join(
                        this.counterPartyRepository.Filter(this.FilteCounterParty), 
                        left => left.CounterpartyId, 
                        right => right.Id, 
                        (left, right) => this.ConvertForCounterParty(left));

            var hedgingDeals = new Summary[0];
            if (this.ContainHedgingDealPosition)
            {
                hedgingDeals =
                    this.hedgingDealRepository.Filter(item => this.FilteHedgingDeal(item, StatusEnum.OPERN))
                        .SelectMany(this.ConvertForCounterParty)
                        .GroupBy(item => new { item.Id, item.CurrencyId })
                        .Select(
                            group =>
                            new Summary
                                {
                                    Id = group.Key.Id, 
                                    CurrencyId = group.Key.CurrencyId, 
                                    Position = group.Sum(item => item.Position)
                                })
                        .ToArray();
            }

            var query = from bankAccount in bankAccounts
                        join hedgingDeal in hedgingDeals on new { bankAccount.Id, bankAccount.CurrencyId } equals
                            new { hedgingDeal.Id, hedgingDeal.CurrencyId } into temp
                        from deal in temp.DefaultIfEmpty()
                        select
                            new
                                {
                                    CounterpartyId = bankAccount.Id, 
                                    bankAccount.CurrencyId, 
                                    bankAccount.SettlementAccountBalance, 
                                    DealPosition = deal == null ? decimal.Zero : deal.Position
                                };

            List<DailyStatisticsViewModel> list = query.GroupBy(item => item.CounterpartyId).Select(
                group =>
                    {
                        DailyStatisticsViewModel dailyStatistics = this.CreateByCounterpartyId(group.Key);
                        dailyStatistics.CurrencyDetails =
                            group.ToDictionary(
                                item => this.currencyRepository.GetName(item.CurrencyId), 
                                item =>
                                new CurrencyDetailModel(
                                    item.CurrencyId, 
                                    item.SettlementAccountBalance, 
                                    item.DealPosition));
                        return dailyStatistics;
                    })
                .Where(item => item.CurrencyDetails.Values.Any(p => !p.IsEmpty()))
                .OrderBy(item => item.CounterPartyName)
                .ToList();

            if (list.Count > 0)
            {
                list.Sort(DailyStatisticsViewModel.Comparer);

                Dictionary<string, CurrencyDetailModel> total = list.SelectMany(item => item.CurrencyDetails.Values)
                    .GroupBy(item => item.CurrencyId)
                    .Select(
                        group =>
                            {
                                decimal sumAccountBalance = group.Sum(item => item.SettlementAccountBalance);
                                decimal sumDealPosition = group.Sum(item => item.Position);
                                return new CurrencyDetailModel(group.Key, sumAccountBalance, sumDealPosition);
                            }).ToDictionary(item => this.currencyRepository.GetName(item.CurrencyId));

                list.Add(
                    new DailyStatisticsViewModel
                        {
                            CounterPartyId = string.Empty, 
                            CounterPartyName = RunTime.FindStringResource("Total"), 
                            CurrencyDetails = total
                        });
            }

            this.DailyStatisticsListByCounterParty = new ObservableCollection<DailyStatisticsViewModel>(list);

            this.DisplayJpyColumn = list.All(item => item.JPY == null) ? Visibility.Collapsed : Visibility.Visible;
            this.DisplayHkdColumn = list.All(item => item.HKD == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// 运行时初始化（首页数据需要登录后等待所有数据加载完毕后才进行相关计算）
        /// </summary>
        /// <param name="runTime">
        /// The run time.
        /// </param>
        protected virtual void Initialize(RunTime runTime)
        {
            Task.Factory.StartNew(runTime.CurrentRepositoryCore.WaitAllInitial).ContinueWith(task => this.Initialize());
        }

        /// <summary>
        ///     初始化
        /// </summary>
        protected void Initialize()
        {
            this.DataBindingForCounterParty();
            this.DataBindingForBusinessUnit();

            this.hedgingDealRepository.SubscribeAddEvent(this.Add);
            this.hedgingDealRepository.SubscribeUpdateEvent(this.Update);
            this.hedgingDealRepository.SubscribeRemoveEvent(this.Remove);

            this.bankAccountRepository.SubscribeUpdateEvent(this.Update);
            this.bankAccountRepository.SubscribeRemoveEvent(this.Remove);
        }

        /// <summary>
        /// 订阅添加交易单
        /// </summary>
        /// <param name="deal">
        /// The deal.
        /// </param>
        private void Add(FxHedgingDealModel deal)
        {
            if (!this.ContainHedgingDealPosition || !this.FilteHedgingDeal(deal, StatusEnum.OPERN))
            {
                return;
            }

            string ccy1Name = this.currencyRepository.GetName(deal.Ccy1Id);
            string ccy2Name = this.currencyRepository.GetName(deal.Ccy2Id);

            decimal ccy1Amount = deal.TransactionType == TransactionTypeEnum.Buy
                                     ? deal.Ccy1Amount
                                     : deal.Ccy1Amount * -1;
            decimal ccy2Amount = deal.TransactionType == TransactionTypeEnum.Buy
                                     ? deal.Ccy2Amount * -1
                                     : deal.Ccy2Amount;

            DailyStatisticsViewModel cpRow =
                this.DailyStatisticsListByCounterParty.FirstOrDefault(
                    item => item.CounterPartyId == deal.CounterpartyId);
            if (cpRow == null)
            {
                cpRow = this.CreateByCounterpartyId(deal.CounterpartyId);
                cpRow.CurrencyDetails = new Dictionary<string, CurrencyDetailModel>();
                this.InsertToListByCounterParty(cpRow);
            }

            this.ChangeWithDeal(cpRow, deal.Ccy1Id, ccy1Name, ccy1Amount);
            this.ChangeWithDeal(cpRow, deal.Ccy2Id, ccy2Name, ccy2Amount);

            DailyStatisticsViewModel cpTotal = this.DailyStatisticsListByCounterParty.Last();
            this.ChangeWithDeal(cpTotal, deal.Ccy1Id, ccy1Name, ccy1Amount);
            this.ChangeWithDeal(cpTotal, deal.Ccy2Id, ccy2Name, ccy2Amount);

            if (!this.BusinessUnitStatisticsEnabled)
            {
                return;
            }

            DailyStatisticsViewModel buRow =
                this.DailyStatisticsListByBusinessUnit.FirstOrDefault(
                    item => item.BusinessUnitId == deal.BusinessUnitId);
            if (buRow == null)
            {
                buRow = this.CreateByBusinessUnitId(deal.CounterpartyId);
                buRow.CurrencyDetails = new Dictionary<string, CurrencyDetailModel>();
                this.InsertToListByBusinessUnit(buRow);
            }

            this.ChangeWithDeal(buRow, deal.Ccy1Id, ccy1Name, ccy1Amount);
            this.ChangeWithDeal(buRow, deal.Ccy2Id, ccy2Name, ccy2Amount);

            DailyStatisticsViewModel buTotal = this.DailyStatisticsListByBusinessUnit.Last();
            this.ChangeWithDeal(buTotal, deal.Ccy1Id, ccy1Name, ccy1Amount);
            this.ChangeWithDeal(buTotal, deal.Ccy2Id, ccy2Name, ccy2Amount);
        }

        /// <summary>
        /// The change with deal.
        /// </summary>
        /// <param name="record">
        /// The record.
        /// </param>
        /// <param name="ccyId">
        /// The ccy id.
        /// </param>
        /// <param name="ccyName">
        /// The ccy name.
        /// </param>
        /// <param name="ccyAmount">
        /// The ccy amount.
        /// </param>
        private void ChangeWithDeal(DailyStatisticsViewModel record, string ccyId, string ccyName, decimal ccyAmount)
        {
            if (record == null)
            {
                return;
            }

            CurrencyDetailModel detail;
            if (record.CurrencyDetails.TryGetValue(ccyName, out detail))
            {
                detail.IncreaseDealPosition(ccyAmount);
            }
            else
            {
                detail = new CurrencyDetailModel(ccyId, 0m, ccyAmount);
                record.CurrencyDetails.Add(ccyName, detail);
            }

            record.NotifyOfPropertyChange(ccyName);
        }

        /// <summary>
        /// The change with non settlement account.
        /// </summary>
        /// <param name="record">
        /// The record.
        /// </param>
        /// <param name="ccyId">
        /// The ccy id.
        /// </param>
        /// <param name="ccyName">
        /// The ccy name.
        /// </param>
        /// <param name="ccyAmount">
        /// The ccy amount.
        /// </param>
        private void ChangeWithNonSettlementAccount(
            DailyStatisticsViewModel record, 
            string ccyId, 
            string ccyName, 
            decimal ccyAmount)
        {
            CurrencyDetailModel detail;
            if (record.CurrencyDetails.TryGetValue(ccyName, out detail))
            {
                detail.IncreaseNonSettlementAccountBalance(ccyAmount);
            }
            else
            {
                detail = new CurrencyDetailModel(ccyId, 0m, 0m, ccyAmount);
                record.CurrencyDetails.Add(ccyName, detail);
            }

            record.NotifyOfPropertyChange(ccyName);
        }

        /// <summary>
        /// The change with settlement account.
        /// </summary>
        /// <param name="record">
        /// The record.
        /// </param>
        /// <param name="ccyId">
        /// The ccy id.
        /// </param>
        /// <param name="ccyName">
        /// The ccy name.
        /// </param>
        /// <param name="ccyAmount">
        /// The ccy amount.
        /// </param>
        private void ChangeWithSettlementAccount(
            DailyStatisticsViewModel record, 
            string ccyId, 
            string ccyName, 
            decimal ccyAmount)
        {
            CurrencyDetailModel detail;
            if (record.CurrencyDetails.TryGetValue(ccyName, out detail))
            {
                detail.IncreaseSettlementAccountBalance(ccyAmount);
            }
            else
            {
                detail = new CurrencyDetailModel(ccyId, ccyAmount, 0m);
                record.CurrencyDetails.Add(ccyName, detail);
            }

            record.NotifyOfPropertyChange(ccyName);
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        /// <returns>
        /// The <see cref="Summary"/>.
        /// </returns>
        private Summary ConvertForBusinessUnit(BankAccountModel bankAccount)
        {
            return new Summary
                       {
                           Id = bankAccount.BusinessUnitId, 
                           CurrencyId = bankAccount.CurrencyId, 
                           SettlementAccountBalance =
                               this.IsSettlementAccount(bankAccount) ? bankAccount.AvailableBalance : 0m, 
                           NonSettlementAccountBalance =
                               this.IsSettlementAccount(bankAccount) ? 0m : bankAccount.AvailableBalance
                       };
        }

        /// <summary>
        /// The convert for business unit.
        /// </summary>
        /// <param name="deal">
        /// The deal.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<Summary> ConvertForBusinessUnit(FxHedgingDealModel deal)
        {
            return new[]
                       {
                           new Summary
                               {
                                   Id = deal.BusinessUnitId, 
                                   CurrencyId = deal.Ccy1Id, 
                                   Position =
                                       deal.TransactionType == TransactionTypeEnum.Buy
                                           ? deal.Ccy1Amount
                                           : deal.Ccy1Amount * -1
                               }, 
                           new Summary
                               {
                                   Id = deal.BusinessUnitId, 
                                   CurrencyId = deal.Ccy2Id, 
                                   Position =
                                       deal.TransactionType == TransactionTypeEnum.Buy
                                           ? deal.Ccy2Amount * -1
                                           : deal.Ccy2Amount
                               }
                       };
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        /// <returns>
        /// The <see cref="Summary"/>.
        /// </returns>
        private Summary ConvertForCounterParty(BankAccountModel bankAccount)
        {
            return new Summary
                       {
                           Id = bankAccount.CounterpartyId, 
                           CurrencyId = bankAccount.CurrencyId, 
                           SettlementAccountBalance = bankAccount.AvailableBalance
                       };
        }

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="deal">
        /// The deal.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<Summary> ConvertForCounterParty(FxHedgingDealModel deal)
        {
            return new[]
                       {
                           new Summary
                               {
                                   Id = deal.CounterpartyId, 
                                   CurrencyId = deal.Ccy1Id, 
                                   Position =
                                       deal.TransactionType == TransactionTypeEnum.Buy
                                           ? deal.Ccy1Amount
                                           : deal.Ccy1Amount * -1
                               }, 
                           new Summary
                               {
                                   Id = deal.CounterpartyId, 
                                   CurrencyId = deal.Ccy2Id, 
                                   Position =
                                       deal.TransactionType == TransactionTypeEnum.Buy
                                           ? deal.Ccy2Amount * -1
                                           : deal.Ccy2Amount
                               }
                       };
        }

        /// <summary>
        /// The create by business unit id.
        /// </summary>
        /// <param name="businessUnitId">
        /// The business unit id.
        /// </param>
        /// <returns>
        /// The <see cref="DailyStatisticsViewModel"/>.
        /// </returns>
        private DailyStatisticsViewModel CreateByBusinessUnitId(string businessUnitId)
        {
            var model = new DailyStatisticsViewModel();
            model.BusinessUnitId = businessUnitId;
            model.BusinessUnitName = this.businessUnitRepository.GetName(businessUnitId);

            return model;
        }

        /// <summary>
        /// The create by counterparty id.
        /// </summary>
        /// <param name="counterPartyId">
        /// The counter party id.
        /// </param>
        /// <returns>
        /// The <see cref="DailyStatisticsViewModel"/>.
        /// </returns>
        private DailyStatisticsViewModel CreateByCounterpartyId(string counterPartyId)
        {
            var model = new DailyStatisticsViewModel();
            model.CounterPartyId = counterPartyId;

            CounterPartyModel counterParty = this.counterPartyRepository.FindByID(counterPartyId);
            if (counterParty == null)
            {
                TraceManager.Error.Write(
                    "DailyExposure", 
                    "Cannot find the CounterpartyModel of id '{0}'.", 
                    counterPartyId);
            }
            else
            {
                model.CounterPartyName = counterParty.Name;
                model.BusinessUnitId = counterParty.BusinessUnitId;
                model.BusinessUnitName = this.businessUnitRepository.GetName(counterParty.BusinessUnitId);
            }

            return model;
        }

        /// <summary>
        /// The filte counter party.
        /// </summary>
        /// <param name="counterParty">
        /// The counter party.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FilteCounterParty(CounterPartyModel counterParty)
        {
            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId))
            {
                return counterParty.BusinessUnitId == this.SearchBusinessUnitId;
            }

            return true;
        }

        /// <summary>
        /// The filte hedging deal.
        /// </summary>
        /// <param name="deal">
        /// The deal.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FilteHedgingDeal(FxHedgingDealModel deal, StatusEnum status)
        {
            if (!this.ContainHedgingDealPosition)
            {
                return false;
            }

            if (deal.Status != status)
            {
                return false;
            }

            if (deal.ValueDate > this.SearchValueDate)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && deal.BusinessUnitId != this.SearchBusinessUnitId)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 过滤非结算帐户
        /// </summary>
        /// <param name="bankAccount">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FilteNonSettlementAccount(BankAccountModel bankAccount)
        {
            if (!bankAccount.Enabled)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(bankAccount.CounterpartyId))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId))
            {
                return bankAccount.BusinessUnitId == this.SearchBusinessUnitId;
            }

            return true;
        }

        /// <summary>
        /// The insert to list by business unit.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        private void InsertToListByBusinessUnit(DailyStatisticsViewModel viewModel)
        {
            int count = this.dailyStatisticsListByBusinessUnit.Count;

            if (count == 0)
            {
                this.dailyStatisticsListByBusinessUnit.Add(viewModel);
                this.dailyStatisticsListByBusinessUnit.Add(
                    new DailyStatisticsViewModel
                        {
                            CounterPartyName = RunTime.FindStringResource("Total"), 
                            CurrencyDetails = new Dictionary<string, CurrencyDetailModel>()
                        });
                return;
            }

            bool isInsert = false;
            for (int posi = 0; posi < count - 1; posi++)
            {
                if (viewModel.CompareTo(this.dailyStatisticsListByBusinessUnit[posi]) > 0)
                {
                    this.dailyStatisticsListByBusinessUnit.Insert(posi, viewModel);
                    isInsert = true;
                    break;
                }
            }

            if (!isInsert)
            {
                this.dailyStatisticsListByBusinessUnit.Insert(count - 1, viewModel);
            }
        }

        /// <summary>
        /// The insert to list by counter party.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        private void InsertToListByCounterParty(DailyStatisticsViewModel viewModel)
        {
            int count = this.dailyStatisticsListByCounterParty.Count;

            if (count == 0)
            {
                this.dailyStatisticsListByCounterParty.Add(viewModel);
                this.dailyStatisticsListByCounterParty.Add(
                    new DailyStatisticsViewModel
                        {
                            CounterPartyName = RunTime.FindStringResource("Total"), 
                            CurrencyDetails = new Dictionary<string, CurrencyDetailModel>()
                        });
                return;
            }

            bool isInsert = false;
            for (int posi = 0; posi < count - 1; posi++)
            {
                if (viewModel.CompareTo(this.dailyStatisticsListByCounterParty[posi]) > 0)
                {
                    this.dailyStatisticsListByCounterParty.Insert(posi, viewModel);
                    isInsert = true;
                    break;
                }
            }

            if (!isInsert)
            {
                this.dailyStatisticsListByCounterParty.Insert(count - 1, viewModel);
            }
        }

        /// <summary>
        /// The is settlement account.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsSettlementAccount(BankAccountModel bankAccount)
        {
            return bankAccount.Enabled && !string.IsNullOrEmpty(bankAccount.CounterpartyId)
                   && bankAccount.CounterpartyId != "0";
        }

        /// <summary>
        /// 订阅移除交易单
        /// </summary>
        /// <param name="deal">
        /// The deal.
        /// </param>
        private void Remove(FxHedgingDealModel deal)
        {
            if (!this.ContainHedgingDealPosition || !this.FilteHedgingDeal(deal, StatusEnum.SETTLE))
            {
                return;
            }

            string ccy1Name = this.currencyRepository.GetName(deal.Ccy1Id);
            string ccy2Name = this.currencyRepository.GetName(deal.Ccy2Id);

            decimal ccy1Amount = deal.TransactionType == TransactionTypeEnum.Buy
                                     ? deal.Ccy1Amount * -1
                                     : deal.Ccy1Amount;
            decimal ccy2Amount = deal.TransactionType == TransactionTypeEnum.Buy
                                     ? deal.Ccy2Amount
                                     : deal.Ccy2Amount * -1;

            DailyStatisticsViewModel cpRow =
                this.DailyStatisticsListByCounterParty.FirstOrDefault(
                    item => item.CounterPartyId == deal.CounterpartyId);
            if (cpRow != null)
            {
                this.ChangeWithDeal(cpRow, deal.Ccy1Id, ccy1Name, ccy1Amount);
                this.ChangeWithDeal(cpRow, deal.Ccy2Id, ccy2Name, ccy2Amount);

                DailyStatisticsViewModel cpTotal = this.DailyStatisticsListByCounterParty.Last();
                this.ChangeWithDeal(cpTotal, deal.Ccy1Id, ccy1Name, ccy1Amount);
                this.ChangeWithDeal(cpTotal, deal.Ccy2Id, ccy2Name, ccy2Amount);
            }

            if (!this.BusinessUnitStatisticsEnabled)
            {
                return;
            }

            DailyStatisticsViewModel buRow =
                this.DailyStatisticsListByBusinessUnit.FirstOrDefault(
                    item => item.BusinessUnitId == deal.BusinessUnitId);
            if (buRow != null)
            {
                this.ChangeWithDeal(buRow, deal.Ccy1Id, ccy1Name, ccy1Amount);
                this.ChangeWithDeal(buRow, deal.Ccy2Id, ccy2Name, ccy2Amount);

                DailyStatisticsViewModel buTotal = this.DailyStatisticsListByBusinessUnit.Last();
                this.ChangeWithDeal(buTotal, deal.Ccy1Id, ccy1Name, ccy1Amount);
                this.ChangeWithDeal(buTotal, deal.Ccy2Id, ccy2Name, ccy2Amount);
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        private void Remove(BankAccountModel bankAccount)
        {
            string ccyName = this.currencyRepository.GetName(bankAccount.CurrencyId);

            if (string.IsNullOrEmpty(bankAccount.CounterpartyId))
            {
                if (!this.BusinessUnitStatisticsEnabled)
                {
                    return;
                }

                DailyStatisticsViewModel buRow =
                    this.DailyStatisticsListByBusinessUnit.FirstOrDefault(
                        item => item.BusinessUnitId == bankAccount.BusinessUnitId);
                if (buRow != null)
                {
                    this.ChangeWithNonSettlementAccount(
                        buRow, 
                        bankAccount.CurrencyId, 
                        ccyName, 
                        bankAccount.AvailableBalance);

                    DailyStatisticsViewModel buTotal = this.DailyStatisticsListByBusinessUnit.Last();
                    this.ChangeWithNonSettlementAccount(
                        buTotal, 
                        bankAccount.CurrencyId, 
                        ccyName, 
                        bankAccount.AvailableBalance);
                }

                return;
            }

            DailyStatisticsViewModel cpRow =
                this.DailyStatisticsListByCounterParty.FirstOrDefault(
                    item => item.CounterPartyId == bankAccount.CounterpartyId);
            if (cpRow != null)
            {
                this.ChangeWithSettlementAccount(cpRow, bankAccount.CurrencyId, ccyName, bankAccount.AvailableBalance);

                DailyStatisticsViewModel cpTotal = this.DailyStatisticsListByCounterParty.Last();
                this.ChangeWithSettlementAccount(cpTotal, bankAccount.CurrencyId, ccyName, bankAccount.AvailableBalance);
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="oldModel">
        /// The old model.
        /// </param>
        /// <param name="newModel">
        /// The new model.
        /// </param>
        private void Update(FxHedgingDealModel oldModel, FxHedgingDealModel newModel)
        {
            // if (newModel.Status != StatusEnum.CANCELLED && newModel.Status != StatusEnum.DELETED
            // && newModel.Status != StatusEnum.SETTLE)
            // {
            // return;
            // }
            this.Remove(newModel);
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="oldModel">
        /// The old model.
        /// </param>
        /// <param name="newModel">
        /// The new model.
        /// </param>
        private void Update(BankAccountModel oldModel, BankAccountModel newModel)
        {
            if (newModel.AvailableBalance == decimal.Zero && oldModel.AvailableBalance == decimal.Zero)
            {
                return;
            }

            string ccyName = this.currencyRepository.GetName(newModel.CurrencyId);

            if (string.IsNullOrEmpty(newModel.CounterpartyId))
            {
                if (!this.BusinessUnitStatisticsEnabled || !this.FilteNonSettlementAccount(newModel))
                {
                    return;
                }

                DailyStatisticsViewModel buRow =
                    this.DailyStatisticsListByBusinessUnit.FirstOrDefault(
                        item => item.BusinessUnitId == newModel.BusinessUnitId);
                if (buRow == null)
                {
                    buRow = this.CreateByBusinessUnitId(newModel.BusinessUnitId);
                    buRow.CurrencyDetails = new Dictionary<string, CurrencyDetailModel>();
                    this.InsertToListByBusinessUnit(buRow);
                }

                this.ChangeWithNonSettlementAccount(
                    buRow, 
                    newModel.CurrencyId, 
                    ccyName, 
                    newModel.AvailableBalance - oldModel.AvailableBalance);

                DailyStatisticsViewModel buTotal = this.DailyStatisticsListByCounterParty.Last();
                this.ChangeWithNonSettlementAccount(
                    buTotal, 
                    newModel.CurrencyId, 
                    ccyName, 
                    newModel.AvailableBalance - oldModel.AvailableBalance);

                return;
            }


            //if (!this.counterPartyRepository.Filter(this.FilteCounterParty).Any())
            //{
            //    return;
            //}
            var counterparty = this.counterPartyRepository.FindByID(newModel.CounterpartyId);
            if (counterparty == null
                || (!string.IsNullOrEmpty(this.SearchBusinessUnitId)
                    && counterparty.BusinessUnitId != this.SearchBusinessUnitId))
            {
                return;
            }

            decimal changedBalance;
            DailyStatisticsViewModel cpRow =
                this.DailyStatisticsListByCounterParty.FirstOrDefault(
                    item => item.CounterPartyId == newModel.CounterpartyId);
            if (cpRow == null)
            {
                cpRow = new DailyStatisticsViewModel()
                            {
                                CounterPartyId = counterparty.Id,
                                CounterPartyName = counterparty.Name,
                                BusinessUnitId = counterparty.BusinessUnitId,
                                BusinessUnitName =
                                    this.businessUnitRepository.GetName(
                                        counterparty.BusinessUnitId),
                                CurrencyDetails = new Dictionary<string, CurrencyDetailModel>()
                            };
                changedBalance = newModel.AvailableBalance;
                this.InsertToListByCounterParty(cpRow);
            }
            else
            {
                changedBalance = newModel.AvailableBalance - oldModel.AvailableBalance;
            }

            this.ChangeWithSettlementAccount(cpRow, newModel.CurrencyId, ccyName, changedBalance);

            DailyStatisticsViewModel cpTotal = this.DailyStatisticsListByCounterParty.Last();
            this.ChangeWithSettlementAccount(cpTotal, newModel.CurrencyId, ccyName, changedBalance);
        }

        #endregion

        /// <summary>
        ///     The summary.
        /// </summary>
        private class Summary : CurrencyDetailModel
        {
            ///// <summary>
            ///// Gets or sets the amount.
            ///// </summary>
            // public decimal Amount { get; set; }

            ///// <summary>
            ///// Gets or sets the currency id.
            ///// </summary>
            // public string CurrencyId { get; set; }
            #region Public Properties

            /// <summary>
            ///     Gets or sets the id.
            /// </summary>
            public string Id { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <returns>
            /// The <see cref="bool"/>.
            /// </returns>
            public override bool Equals(object obj)
            {
                var other = obj as Summary;
                if (other == null)
                {
                    return false;
                }

                return other.Id == this.Id && other.CurrencyId == this.CurrencyId;
            }

            /// <summary>
            /// The get hash code.
            /// </summary>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public override int GetHashCode()
            {
                return string.Concat(this.Id, "@", this.CurrencyId).GetHashCode();
            }

            #endregion
        }
    }
}