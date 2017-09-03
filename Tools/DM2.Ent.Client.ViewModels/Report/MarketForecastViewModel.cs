// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketForecastViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels.Report
{
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
    ///     The market forecast view model.
    /// </summary>
    public class MarketForecastViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The contract repository.
        /// </summary>
        protected readonly IContractRepository contractRepository;

        /// <summary>
        /// The current run time.
        /// </summary>
        private readonly RunTime currentRunTime;

        /// <summary>
        ///     The hedging deal repository.
        /// </summary>
        private readonly IFxHedgingDealRepository hedgingDealRepository;

        /// <summary>
        ///     The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The contract summary list.
        /// </summary>
        private ObservableCollection<ContractSummaryViewModel> contractSummaryList;

        /// <summary>
        ///     The selected contract summary.
        /// </summary>
        private ContractSummaryViewModel selectedContractSummary;

        /// <summary>
        ///     The selected currency id.
        /// </summary>
        private string selectedCurrencyId;

        /// <summary>
        /// The total forecast pl.
        /// </summary>
        private decimal totalForecastPl;

        /// <summary>
        /// The total pl.
        /// </summary>
        private decimal totalPl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketForecastViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner Id.
        /// </param>
        public MarketForecastViewModel(string ownerId = null)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("MarketForecast");

            this.currentRunTime = RunTime.GetCurrentRunTime(ownerId);

            this.hedgingDealRepository = this.GetRepository<IFxHedgingDealRepository>();
            this.contractRepository = this.GetRepository<IContractRepository>();
            this.contractSummaryList = new ObservableCollection<ContractSummaryViewModel>();

            this.Initialize(this.currentRunTime);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the contract summary list.
        /// </summary>
        public ObservableCollection<ContractSummaryViewModel> ContractSummaryList
        {
            get
            {
                return this.contractSummaryList;
            }

            set
            {
                this.contractSummaryList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the selected contract summary.
        /// </summary>
        public ContractSummaryViewModel SelectedContractSummary
        {
            get
            {
                return this.selectedContractSummary;
            }

            set
            {
                this.selectedContractSummary = value;
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
        /// Gets or sets the total forecast pl.
        /// </summary>
        public decimal TotalForecastPl
        {
            get
            {
                return this.totalForecastPl;
            }

            set
            {
                this.totalForecastPl = value;
                this.NotifyOfPropertyChange(() => this.TotalForecastPl);
            }
        }

        /// <summary>
        /// Gets or sets the total pl.
        /// </summary>
        public decimal TotalPl
        {
            get
            {
                return this.totalPl;
            }

            set
            {
                this.totalPl = value;
                this.NotifyOfPropertyChange(() => this.TotalPl);
            }
        }

        /// <summary>
        /// Gets or sets the search business unit id.
        /// </summary>
        protected string SearchBusinessUnitId { get; set; }

        /// <summary>
        /// Gets or sets the search business unit id.
        /// </summary>
        protected string SearchContractId { get; set; }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The modify forecast rate.
        /// </summary>
        /// <param name="contractId">
        /// The contract Id.
        /// </param>
        public void ModifyForecastRate(string contractId = null)
        {
            var vm = new ModifyForecastRateViewModel(this.OwnerId, contractId);
            vm.DisplayName = RunTime.FindStringResource("ModifyForecastRateTitle");

            if (this.windowManager.ShowDialog(vm) == true)
            {
                this.RefreshForecastRate(vm);
            }
        }

        /// <summary>
        /// The open deal list.
        /// </summary>
        public void OpenDealList()
        {
            if (this.SelectedContractSummary == null)
            {
                return;
            }

            var vm = new DealListViewModel(this.OwnerId);
            vm.DisplayName = RunTime.FindStringResource("HedgeDealList");
            this.windowManager.ShowWindow(vm);
            vm.InitWithForecast(this.SelectedContractSummary.ContractId, this.SelectedContractSummary.ValueDate);
        }

        /// <summary>
        /// The refresh forecast rate.
        /// </summary>
        /// <param name="forecastRate">
        /// The forecast rate.
        /// </param>
        public void RefreshForecastRate(ModifyForecastRateViewModel forecastRate)
        {
            ContractSummaryViewModel[] contractSummaries =
                this.contractSummaryList.Where(item => item.ContractId == forecastRate.SelectedContractId).ToArray();
            foreach (ContractSummaryViewModel item in contractSummaries)
            {
                item.UpdateForecastRate(
                    forecastRate.Bid, 
                    forecastRate.Ask, 
                    this.selectedCurrencyId, 
                    this.currentRunTime);
            }

            this.Sum();
        }

        #endregion

        #region Methods

        private bool FilteHedgingDeal(FxHedgingDealModel deal, StatusEnum status)
        {
            if (deal.Status != status)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchContractId) && deal.ContractId != this.SearchContractId)
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
        ///     绑定数据.
        /// </summary>
        protected void DataBinding()
        {
            IEnumerable<FxHedgingDealModel> deals = this.hedgingDealRepository.Filter(item => this.FilteHedgingDeal(item, StatusEnum.OPERN));

            List<ContractSummaryViewModel> list =
                deals.GroupBy(deal => new { deal.ContractId, deal.ValueDate }).Select(
                    group =>
                        {
                            var summary = new ContractSummaryViewModel
                                              {
                                                  ContractId = group.Key.ContractId,
                                                  ValueDate = group.Key.ValueDate
                                              };
                            ContractModel contract = this.contractRepository.FindByID(summary.ContractId);
                            summary.ContractName = contract.Name;
                            summary.Ccy1Id = contract.Ccy1Id;
                            summary.Ccy2Id = contract.Ccy2Id;

                            summary.Ccy1Amount =
                                group.Sum(
                                    deal =>
                                    deal.TransactionType == TransactionTypeEnum.Buy
                                        ? deal.Ccy1Amount
                                        : deal.Ccy1Amount * -1);
                            summary.Ccy2Amount =
                                group.Sum(
                                    deal =>
                                    deal.TransactionType == TransactionTypeEnum.Buy
                                        ? deal.Ccy2Amount * -1
                                        : deal.Ccy2Amount);

                            summary.UpdateAboutRate(this.selectedCurrencyId, this.currentRunTime);

                            return summary;
                        }).ToList();

            list.Sort(ContractSummaryViewModel.Comparer);

            this.ContractSummaryList = new ObservableCollection<ContractSummaryViewModel>(list);

            this.Sum();
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
            CurrencyModel ccy = this.GetRepository<ICurrencyRepository>().FindByName("CNY");
            if (ccy != null)
            {
                this.selectedCurrencyId = ccy.Id;
            }

            this.DataBinding();

            this.hedgingDealRepository.SubscribeAddEvent(this.Add);
            this.hedgingDealRepository.SubscribeUpdateEvent(this.Update);
            this.hedgingDealRepository.SubscribeRemoveEvent(this.Remove);

            var priceRepository = this.GetRepository<IPriceBlockRepository>();
            priceRepository.SubscribeAddEvent(this.SubscribePrice);
            priceRepository.SubscribeUpdateEvent((oldModel, newModel) => this.SubscribePrice(newModel));
        }

        /// <summary>
        ///     汇总数据
        /// </summary>
        private void Sum()
        {
            this.TotalPl = this.contractSummaryList.Sum(item => item.Pl);
            this.TotalForecastPl = this.contractSummaryList.Sum(item => item.ForecastPl);
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void Add(FxHedgingDealModel model)
        {
            if (!this.FilteHedgingDeal(model, StatusEnum.OPERN))
            {
                return;
            }

            ContractSummaryViewModel summary =
                this.contractSummaryList.FirstOrDefault(
                    vm => vm.ContractId == model.ContractId && vm.ValueDate == model.ValueDate.Date);
            if (summary == null)
            {
                summary = new ContractSummaryViewModel
                              {
                                  ContractId = model.ContractId, 
                                  ValueDate = model.ValueDate.Date, 
                                  Ccy1Id = model.Ccy1Id, 
                                  Ccy2Id = model.Ccy2Id, 
                              };
                summary.ContractName = this.contractRepository.GetName(summary.ContractId);
                this.Insert(summary);
            }

            if (model.TransactionType == TransactionTypeEnum.Buy)
            {
                summary.Ccy1Amount += model.Ccy1Amount;
                summary.Ccy2Amount += model.Ccy2Amount * -1;
            }
            else
            {
                summary.Ccy1Amount += model.Ccy1Amount * -1;
                summary.Ccy2Amount += model.Ccy2Amount;
            }

            summary.UpdateAboutRate(this.selectedCurrencyId, this.currentRunTime);

            this.Sum();
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        private void Insert(ContractSummaryViewModel viewModel)
        {
            if (this.contractSummaryList.Count == 0)
            {
                this.contractSummaryList.Add(viewModel);
                return;
            }

            bool isInsert = false;
            for (int posi = 0; posi < this.contractSummaryList.Count; posi++)
            {
                if (viewModel.CompareTo(this.contractSummaryList[posi]) < 0)
                {
                    this.contractSummaryList.Insert(posi, viewModel);
                    isInsert = true;
                    break;
                }
            }

            if (!isInsert)
            {
                this.contractSummaryList.Add(viewModel);
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void Remove(FxHedgingDealModel model)
        {
            if (!this.FilteHedgingDeal(model, StatusEnum.SETTLE))
            {
                return;
            }

            ContractSummaryViewModel summary =
                this.contractSummaryList.FirstOrDefault(
                    vm => vm.ContractId == model.ContractId && vm.ValueDate == model.ValueDate.Date);
            if (summary != null)
            {
                if (model.TransactionType == TransactionTypeEnum.Buy)
                {
                    summary.Ccy1Amount -= model.Ccy1Amount;
                    summary.Ccy2Amount -= model.Ccy2Amount * -1;
                }
                else
                {
                    summary.Ccy1Amount -= model.Ccy1Amount * -1;
                    summary.Ccy2Amount -= model.Ccy2Amount;
                }

                // if (summary.Ccy1Amount == decimal.Zero && summary.Ccy2Amount == decimal.Zero)
                // {
                // this.contractSummaryList.Remove(summary);
                // return;
                // }
                summary.UpdateAboutRate(this.selectedCurrencyId, this.currentRunTime);
            }

            this.Sum();
        }

        /// <summary>
        /// 订阅报价
        /// </summary>
        /// <param name="price">
        /// </param>
        private void SubscribePrice(PriceBlockModel price)
        {
            if (this.contractSummaryList.Count == 0)
            {
                return;
            }

            ContractSummaryViewModel[] contractSummaries =
                this.contractSummaryList.Where(item => item.ContractId == price.BelongContract.Id).ToArray();
            foreach (ContractSummaryViewModel item in contractSummaries)
            {
                item.UpdateAboutRate(this.selectedCurrencyId, this.currentRunTime);
            }

            this.Sum();
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

        #endregion
    }
}