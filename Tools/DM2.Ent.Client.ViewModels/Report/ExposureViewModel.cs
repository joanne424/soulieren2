// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExposureViewModel.cs" company="">
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

    using Caliburn.Micro;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Infrastructure.Common.Enums;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The exposure view model.
    /// </summary>
    public class ExposureViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The bank account repository.
        /// </summary>
        private readonly IBankAccountRepository bankAccountRepository;

        /// <summary>
        ///     The currency repository.
        /// </summary>
        private readonly ICurrencyRepository currencyRepository;

        /// <summary>
        ///     The current run time.
        /// </summary>
        private readonly RunTime currentRunTime;

        /// <summary>
        ///     The hedging deal repository.
        /// </summary>
        private readonly IFxHedgingDealRepository hedgingDealRepository;

        /// <summary>
        ///     The price repository.
        /// </summary>
        private readonly IPriceBlockRepository priceRepository;

        /// <summary>
        ///     The window manager.
        /// </summary>
        private readonly IWindowManager windowManager;

        /// <summary>
        ///     The currency summary list.
        /// </summary>
        private ObservableCollection<CurrencySummaryViewModel> currencySummaryList;

        private string selectedCurrencyId;

        /// <summary>
        ///     The selected currency summary.
        /// </summary>
        private CurrencySummaryViewModel selectedCurrencySummary;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposureViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public ExposureViewModel(string ownerId = null)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("Exposure");

            this.windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();
            this.currentRunTime = RunTime.GetCurrentRunTime(ownerId);

            this.hedgingDealRepository = this.GetRepository<IFxHedgingDealRepository>();
            this.bankAccountRepository = this.GetRepository<IBankAccountRepository>();
            this.currencyRepository = this.GetRepository<ICurrencyRepository>();
            this.priceRepository = this.GetRepository<IPriceBlockRepository>();

            this.ContainHedgingDealPosition = true;

            this.currencySummaryList = new ObservableCollection<CurrencySummaryViewModel>();

            this.Initialize(this.currentRunTime);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the currency summary list.
        /// </summary>
        public ObservableCollection<CurrencySummaryViewModel> CurrencySummaryList
        {
            get
            {
                return this.currencySummaryList;
            }

            set
            {
                this.currencySummaryList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the selected currency id.
        /// </summary>
        public string SelectedCurrencyId
        {
            get
            {
                return this.selectedCurrencyId;
            }

            set
            {
                this.selectedCurrencyId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the selected currency summary.
        /// </summary>
        public CurrencySummaryViewModel SelectedCurrencySummary
        {
            get
            {
                return this.selectedCurrencySummary;
            }

            set
            {
                this.selectedCurrencySummary = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether contain bank balance.
        /// </summary>
        protected bool ContainBankBalance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether contain hedging deal position.
        /// </summary>
        protected bool ContainHedgingDealPosition { get; set; }

        /// <summary>
        /// Gets or sets the search business unit id.
        /// </summary>
        protected string SearchBusinessUnitId { get; set; }

        /// <summary>
        /// Gets or sets the search value date.
        /// </summary>
        protected DateTime? SearchValueDate { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The open detailed list.
        /// </summary>
        public void OpenDetailedList()
        {
            if (this.selectedCurrencySummary == null)
            {
                return;
            }

            var vm = new ExposureDetailedViewModel(
                this.selectedCurrencySummary.CurrencyId,
                this.SearchBusinessUnitId,
                this.SearchValueDate,
                this.ContainHedgingDealPosition,
                this.ContainBankBalance);
            vm.DisplayName = RunTime.FindStringResource("ExposureDetail");

            this.windowManager.ShowWindow(vm);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     绑定数据.
        /// </summary>
        protected void DataBinding()
        {
            var allList = new List<Tuple<string, decimal>>();

            if (this.ContainHedgingDealPosition)
            {
                var dealTuples =
                    this.hedgingDealRepository.Filter(item => this.FilteHedgingDeal(item, StatusEnum.OPERN))
                        .SelectMany(this.GetTuples);

                allList.AddRange(dealTuples);
            }

            if (this.ContainBankBalance)
            {
                var bankTuples =
                    this.bankAccountRepository.Filter(this.FilteBankAccount).Select(this.GetTuple);

                allList.AddRange(bankTuples);
            }

            var list = allList.GroupBy(item => item.Item1).Select(
                item =>
                {
                    var summary = new CurrencySummaryViewModel
                    {
                        CurrencyId = item.Key,
                        CurrencyName =
                            this.currencyRepository.GetName(item.Key),
                        Amount = item.Sum(p => p.Item2)
                    };
                    this.Fill(summary);

                    return summary;
                }).OrderBy(item => item.CurrencyName).ToList();

            this.CurrencySummaryList = new ObservableCollection<CurrencySummaryViewModel>(list);
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
            CurrencyModel ccy = this.currencyRepository.FindByName("CNY");
            if (ccy != null)
            {
                this.SelectedCurrencyId = ccy.Id;
            }


            this.DataBinding();

            this.hedgingDealRepository.SubscribeAddEvent(this.Add);
            this.hedgingDealRepository.SubscribeUpdateEvent(this.Update);
            this.hedgingDealRepository.SubscribeRemoveEvent(this.Remove);

            this.bankAccountRepository.SubscribeUpdateEvent(this.Update);
            this.bankAccountRepository.SubscribeRemoveEvent(this.Remove);

            this.priceRepository.SubscribeAddEvent(this.SubscribePrice);
            this.priceRepository.SubscribeUpdateEvent((oldModel, newModel) => this.SubscribePrice(newModel));
        }

        /// <summary>
        /// The add.
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

            var summary = this.currencySummaryList.FirstOrDefault(p => p.CurrencyId == deal.Ccy1Id);
            if (summary == null)
            {
                summary = new CurrencySummaryViewModel
                {
                    CurrencyId = deal.Ccy1Id,
                    CurrencyName = this.currencyRepository.GetName(deal.Ccy1Id),
                };
                this.Insert(summary);
            }

            summary.Amount += deal.TransactionType == TransactionTypeEnum.Buy ? deal.Ccy1Amount : deal.Ccy1Amount * -1;

            this.Fill(summary);

            summary = this.currencySummaryList.FirstOrDefault(p => p.CurrencyId == deal.Ccy2Id);
            if (summary == null)
            {
                summary = new CurrencySummaryViewModel
                {
                    CurrencyId = deal.Ccy2Id,
                    CurrencyName = this.currencyRepository.GetName(deal.Ccy2Id),
                };
                this.Insert(summary);
            }

            summary.Amount += deal.TransactionType == TransactionTypeEnum.Buy ? deal.Ccy2Amount * -1 : deal.Ccy2Amount;

            this.Fill(summary);
        }

        /// <summary>
        /// The fill.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void Fill(CurrencySummaryViewModel model)
        {
            var selectedCurrencyName = this.currencyRepository.GetName(this.selectedCurrencyId);

            if ((model.CurrencyName == "CNH" && selectedCurrencyName == "CNY")
                || (model.CurrencyName == "CNY" && selectedCurrencyName == "CNH")
                || model.CurrencyId == this.selectedCurrencyId)
            {
                model.MarketRate = 1;
                model.TransferAmount = model.Amount;
            }
            else
            {
                model.UpdateMarketRate(this.selectedCurrencyId, this.currentRunTime);
            }
        }

        /// <summary>
        /// The filte bank account.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FilteBankAccount(BankAccountModel bankAccount)
        {
            if (!bankAccount.Enabled)
            {
                return false;
            }

            if (bankAccount.AvailableBalance == decimal.Zero)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && bankAccount.BusinessUnitId != this.SearchBusinessUnitId)
            {
                return false;
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
            if (deal.Status != status)
            {
                return false;
            }

            if (this.SearchValueDate.HasValue && deal.ValueDate > this.SearchValueDate)
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
        /// The get tuple.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        private Tuple<string, decimal> GetTuple(BankAccountModel bankAccount)
        {
            return new Tuple<string, decimal>(bankAccount.CurrencyId, bankAccount.AvailableBalance);
        }

        /// <summary>
        /// The get tuples.
        /// </summary>
        /// <param name="deal">
        /// The deal.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<Tuple<string, decimal>> GetTuples(FxHedgingDealModel deal)
        {
            return new[]
                       {
                           new Tuple<string, decimal>(
                               deal.Ccy1Id, 
                               deal.TransactionType == TransactionTypeEnum.Buy ? deal.Ccy1Amount : deal.Ccy1Amount * -1), 
                           new Tuple<string, decimal>(
                               deal.Ccy2Id, 
                               deal.TransactionType == TransactionTypeEnum.Buy ? deal.Ccy2Amount * -1 : deal.Ccy2Amount)
                       };
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        private void Insert(CurrencySummaryViewModel viewModel)
        {
            if (this.currencySummaryList.Count == 0)
            {
                this.currencySummaryList.Add(viewModel);
                return;
            }

            bool isInsert = false;
            for (int posi = 0; posi < this.currencySummaryList.Count; posi++)
            {
                if (viewModel.CurrencyName.CompareTo(this.currencySummaryList[posi].CurrencyName) > 0)
                {
                    this.currencySummaryList.Insert(posi, viewModel);
                    isInsert = true;
                    break;
                }
            }

            if (!isInsert)
            {
                this.currencySummaryList.Add(viewModel);
            }
        }

        /// <summary>
        /// The remove.
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

            var summary = this.currencySummaryList.FirstOrDefault(p => p.CurrencyId == deal.Ccy1Id);
            if (summary != null)
            {
                summary.Amount += deal.TransactionType == TransactionTypeEnum.Buy
                                      ? deal.Ccy1Amount * -1
                                      : deal.Ccy1Amount;
            }

            this.Fill(summary);

            summary = this.currencySummaryList.FirstOrDefault(p => p.CurrencyId == deal.Ccy2Id);
            if (summary != null)
            {
                summary.Amount += deal.TransactionType == TransactionTypeEnum.Buy
                                      ? deal.Ccy1Amount
                                      : deal.Ccy1Amount * -1;
            }

            this.Fill(summary);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="bankAccount">
        /// The bank Account.
        /// </param>
        private void Remove(BankAccountModel bankAccount)
        {
            if (!this.ContainBankBalance || !this.FilteBankAccount(bankAccount))
            {
                return;
            }

            var summary =
                this.currencySummaryList.FirstOrDefault(p => p.CurrencyId == bankAccount.CurrencyId);
            if (summary != null)
            {
                summary.Amount -= bankAccount.AvailableBalance;
            }

            this.Fill(summary);
        }

        /// <summary>
        /// 订阅报价
        /// </summary>
        /// <param name="price">
        /// </param>
        private void SubscribePrice(PriceBlockModel price)
        {
            if (this.currencySummaryList.Count == 0)
            {
                return;
            }

            var currencySummaries =
                this.currencySummaryList.Where(item => item.ContractId == price.BelongContract.Id).ToArray();
            foreach (CurrencySummaryViewModel item in currencySummaries)
            {
                item.UpdateMarketRate(this.selectedCurrencyId, this.currentRunTime);
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
            //if (newModel.Status != StatusEnum.CANCELLED && newModel.Status != StatusEnum.DELETED
            //    && newModel.Status != StatusEnum.SETTLE)
            //{
            //    return;
            //}

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
            if (!this.ContainBankBalance || !this.FilteBankAccount(newModel))
            {
                return;
            }

            var summary =
                this.currencySummaryList.FirstOrDefault(p => p.CurrencyId == oldModel.CurrencyId);
            if (summary == null)
            {
                summary = new CurrencySummaryViewModel
                {
                    CurrencyId = newModel.CurrencyId,
                    CurrencyName =
                        this.currencyRepository.GetName(newModel.CurrencyId),
                    Amount = newModel.AvailableBalance
                };
                this.Insert(summary);
            }
            else
            {
                summary.Amount += newModel.AvailableBalance - oldModel.AvailableBalance;
            }

            this.Fill(summary);
        }

        #endregion
    }
}