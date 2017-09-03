// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencySummaryViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Report
{
    using Caliburn.Micro;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;

    using Infrastructure.Common;

    /// <summary>
    /// The currency summary view model.
    /// </summary>
    public class CurrencySummaryViewModel : PropertyChangedBase
    {
        // , IComparable<ContractSummaryViewModel>
        #region Fields

        /// <summary>
        ///     The ccy 1 amount.
        /// </summary>
        private decimal amount;

        /// <summary>
        ///     the ccy1 Id
        /// </summary>
        private string ccyId;

        /// <summary>
        /// The ccy name.
        /// </summary>
        private string ccyName;

        /// <summary>
        ///     The contract id.
        /// </summary>
        private string contractId;

        /// <summary>
        ///     The market rate.
        /// </summary>
        private decimal marketRate;

        /// <summary>
        ///     The transfer amount.
        /// </summary>
        private decimal transferAmount;

        #endregion

        #region Public Properties

        /// <summary>
        ///     CCY交易量
        /// </summary>
        public decimal Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
                this.NotifyOfPropertyChange();

                // this.NotifyOfPropertyChange("AverageRate");
            }
        }

        /// <summary>
        ///     货币对id
        /// </summary>
        public string ContractId
        {
            get
            {
                return this.contractId;
            }

            set
            {
                this.contractId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CCYID
        /// </summary>
        public string CurrencyId
        {
            get
            {
                return this.ccyId;
            }

            set
            {
                this.ccyId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CCYName
        /// </summary>
        public string CurrencyName
        {
            get
            {
                return this.ccyName;
            }

            set
            {
                this.ccyName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     市价
        /// </summary>
        public decimal MarketRate
        {
            get
            {
                return this.marketRate;
            }

            set
            {
                this.marketRate = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     转换交易量
        /// </summary>
        public decimal TransferAmount
        {
            get
            {
                return this.transferAmount;
            }

            set
            {
                this.transferAmount = value;
                this.NotifyOfPropertyChange();

                // this.NotifyOfPropertyChange("AverageRate");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The update market rate.
        /// </summary>
        /// <param name="transferCcyId">
        /// The transfer ccy id.
        /// </param>
        /// <param name="currentRunTime">
        /// The current run time.
        /// </param>
        public void UpdateMarketRate(string transferCcyId, RunTime currentRunTime)
        {
            if (this.CurrencyId == transferCcyId)
            {
                this.MarketRate = 1;
                this.TransferAmount = this.Amount;
                return;
            }

            PriceInfoModel price = currentRunTime.GetTransferPrice(this.CurrencyId, transferCcyId);
            if (price == null || price.BelongContract == null)
            {
                var contract = currentRunTime.GetRepository<IContractRepository>().GetSymbol(this.CurrencyId, transferCcyId);
                if (contract != null)
                {
                    this.ContractId = contract.Id;
                }

                return;
            }

            this.ContractId = price.BelongContract.Id;
            decimal marketPrice;
            if (this.Amount > decimal.Zero)
            {
                marketPrice = price.BelongContract.Ccy1Id == this.CurrencyId ? price.DsBid : price.DsAsk;
            }
            else
            {
                marketPrice = price.BelongContract.Ccy2Id == this.CurrencyId ? price.DsBid : price.DsAsk;
            }

            this.MarketRate = marketPrice.ToFixed(price.BelongContract.DecimalPlace);

            if (this.MarketRate == decimal.Zero)
            {
                return;
            }

            if (price.BelongContract.Ccy1Id == transferCcyId)
            {
                this.TransferAmount = this.Amount / this.MarketRate;
            }
            else
            {
                this.TransferAmount = this.Amount * this.MarketRate;
            }
        }

        #endregion
    }
}