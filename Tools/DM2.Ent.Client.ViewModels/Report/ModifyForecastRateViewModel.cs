// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyForecastRateViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    /// <summary>
    /// The modify forecast rate view model.
    /// </summary>
    public class ModifyForecastRateViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        /// The ask.
        /// </summary>
        private decimal ask;

        /// <summary>
        /// The bid.
        /// </summary>
        private decimal bid;

        /// <summary>
        /// The selected contract id.
        /// </summary>
        private string selectedContractId;

        /// <summary>
        /// 价格小数位
        /// </summary>
        private int decimalPlace;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyForecastRateViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        /// <param name="contractId">
        /// The contract id.
        /// </param>
        public ModifyForecastRateViewModel(string ownerId, string contractId = null)
            : base(ownerId)
        {
            this.Contracts =
                this.GetRepository<IContractRepository>()
                    .Filter(o => true)
                    .OrderBy(o => o.Name)
                    .ToList();

            if (!string.IsNullOrEmpty(contractId))
            {
                this.SelectedContractId = contractId;
                this.OnContractChanged();
            }
            else
            {
                this.DecimalPlace = 4;
            }

        }

        #endregion

        #region Public Properties

        public int DecimalPlace
        {
            get
            {
                return this.decimalPlace;
            }

            set
            {
                this.decimalPlace = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the ask.
        /// </summary>
        public decimal Ask
        {
            get
            {
                return this.ask;
            }

            set
            {
                this.ask = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the bid.
        /// </summary>
        public decimal Bid
        {
            get
            {
                return this.bid;
            }

            set
            {
                this.bid = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the contracts.
        /// </summary>
        public IList<ContractModel> Contracts { get; set; }

        /// <summary>
        /// Gets or sets the selected contract id.
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
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        /// The modify.
        /// </summary>
        public void Modify()
        {
            if (!this.ValidateforSumbit())
            {
                return;
            }

            ForecastPriceDao.Instance.Save(
                new ForecastPriceModel { ContractId = this.selectedContractId, Bid = this.bid, Ask = this.ask });

            this.Close();
        }

        /// <summary>
        /// The on contract changed.
        /// </summary>
        public void OnContractChanged()
        {
            if (string.IsNullOrEmpty(this.selectedContractId))
            {
                return;
            }

            var contract = this.Contracts.FirstOrDefault(item => item.Id == this.selectedContractId);
            if (contract == null)
            {
                this.DecimalPlace = 4;
            }
            else
            {
                this.DecimalPlace = contract.DecimalPlace;
            }

            var price = ForecastPriceDao.Instance.Get(this.selectedContractId);
            if (price != null)
            {
                this.Bid = price.Bid;
                this.Ask = price.Ask;
            }
            else
            {
                this.Bid = decimal.Zero;
                this.Ask = decimal.Zero;
            }
        }

        #endregion

        protected override string OnValidate(string propertyName)
        {
            if (propertyName == "SelectedContractId" && string.IsNullOrEmpty(this.SelectedContractId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "Ask" && this.Ask < decimal.Zero)
            {
                return RunTime.FindStringResource("MSG_00009");
            }

            if (propertyName == "Bid" && this.Bid < decimal.Zero)
            {
                return RunTime.FindStringResource("MSG_00009");
            }

            return null;
        }
    }
}