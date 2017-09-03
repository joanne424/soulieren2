// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DailyExposureReportViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;

    /// <summary>
    /// The daily exposure report view model.
    /// </summary>
    public class DailyExposureReportViewModel : DailyExposureViewModel
    {
        #region Fields

        /// <summary>
        ///     The business units.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnits;

        /// <summary>
        /// The is hedging deal position checked.
        /// </summary>
        private bool isHedgingDealPositionChecked;

        /// <summary>
        /// The is internal deal position checked.
        /// </summary>
        private bool isInternalDealPositionChecked;

        /// <summary>
        ///     The selected business unit id.
        /// </summary>
        private string selectedBusinessUnitId;

        /// <summary>
        /// The selected value date.
        /// </summary>
        private DateTime selectedValueDate;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyExposureReportViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public DailyExposureReportViewModel(string ownerId = null)
            : base(ownerId)
        {
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
        /// Gets or sets a value indicating whether is hedging deal position checked.
        /// </summary>
        public bool IsHedgingDealPositionChecked
        {
            get
            {
                return this.isHedgingDealPositionChecked;
            }

            set
            {
                this.isHedgingDealPositionChecked = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is internal deal position checked.
        /// </summary>
        public bool IsInternalDealPositionChecked
        {
            get
            {
                return this.isInternalDealPositionChecked;
            }

            set
            {
                this.isInternalDealPositionChecked = value;
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
        /// Gets or sets the selected value date.
        /// </summary>
        public DateTime SelectedValueDate
        {
            get
            {
                return this.selectedValueDate;
            }

            set
            {
                this.selectedValueDate = value;
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
        /// The search.
        /// </summary>
        public void Search()
        {
            this.SearchBusinessUnitId = this.SelectedBusinessUnitId;
            this.SearchValueDate = this.SelectedValueDate;

            this.ContainHedgingDealPosition = this.IsHedgingDealPositionChecked;
            this.ContainInternalDealPosition = this.IsInternalDealPositionChecked;

            this.DataBindingForBusinessUnit();
            this.DataBindingForCounterParty();
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
            this.BusinessUnitStatisticsEnabled = true;
            this.IsHedgingDealPositionChecked = true;
            this.SelectedValueDate = this.SearchValueDate;
            this.BusinessUnits = this.businessUnitRepository.GetBindCollection().ToComboboxBinding(true);

            this.Initialize();
        }

        #endregion
    }
}