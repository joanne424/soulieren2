// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketForecastReportViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels.Report
{
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;

    /// <summary>
    ///     The market forecast report view model.
    /// </summary>
    public class MarketForecastReportViewModel : MarketForecastViewModel
    {
        #region Fields

        /// <summary>
        /// The business units.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnits;

        /// <summary>
        ///     The checked auto refresh.
        /// </summary>
        private bool checkedAutoRefresh;

        /// <summary>
        /// The contracts.
        /// </summary>
        private ObservableCollection<ContractModel> contracts;

        /// <summary>
        /// The currencies.
        /// </summary>
        private ObservableCollection<CurrencyModel> currencies;

        /// <summary>
        ///     The selected business unit id.
        /// </summary>
        private string selectedBusinessUnitId;

        /// <summary>
        ///     The selected contract id.
        /// </summary>
        private string selectedContractId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketForecastReportViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner Id.
        /// </param>
        public MarketForecastReportViewModel(string ownerId = null)
            : base(ownerId)
        {
            this.checkedAutoRefresh = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     BU列表
        /// </summary>
        public ObservableCollection<BusinessUnitModel> BusinessUnits
        {
            get
            {
                return this.businessUnits;
            }

            set
            {
                this.businessUnits = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether checked auto refresh.
        /// </summary>
        public bool CheckedAutoRefresh
        {
            get
            {
                return this.checkedAutoRefresh;
            }

            set
            {
                this.checkedAutoRefresh = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     货币对列表
        /// </summary>
        public ObservableCollection<ContractModel> Contracts
        {
            get
            {
                return this.contracts;
            }

            set
            {
                this.contracts = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     货币列表.
        /// </summary>
        public ObservableCollection<CurrencyModel> Currencies
        {
            get
            {
                return this.currencies;
            }

            set
            {
                this.currencies = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the selected business unit id.
        /// </summary>
        public string SelectedBusinessUnitId
        {
            get
            {
                return this.selectedBusinessUnitId;
            }

            set
            {
                this.selectedBusinessUnitId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the selected contract id.
        /// </summary>
        public string SelectedContractId
        {
            get
            {
                return this.selectedContractId;
            }

            set
            {
                this.selectedContractId = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     关闭操作
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     The search.
        /// </summary>
        public void Search()
        {
            this.SearchBusinessUnitId = this.SelectedBusinessUnitId;
            this.SelectedContractId = this.SelectedContractId;

            this.DataBinding();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="runTime">
        /// The run time.
        /// </param>
        protected override void Initialize(RunTime runTime)
        {
            this.BusinessUnits =
                this.GetRepository<IBusinessUnitRepository>().GetBindCollection().ToComboboxBinding(true);
            this.Contracts = this.contractRepository.GetBindCollection().ToComboboxBinding(true);
            this.Currencies = this.GetRepository<ICurrencyRepository>().GetBindCollection().ToComboboxBinding();

            this.Initialize();
        }

        #endregion
    }
}