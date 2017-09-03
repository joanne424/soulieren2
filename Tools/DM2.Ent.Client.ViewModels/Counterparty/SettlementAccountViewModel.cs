// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettlementAccountViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Counterparty
{
    using Caliburn.Micro;

    /// <summary>
    /// The settlement account view model.
    /// </summary>
    public class SettlementAccountViewModel : PropertyChangedBase
    {
        #region Fields

        /// <summary>
        ///     银行帐户ID
        /// </summary>
        private string bankAccountId;

        /// <summary>
        /// The bank account no.
        /// </summary>
        private string bankAccountNo;

        /// <summary>
        ///     货币ID
        /// </summary>
        private string currencyId;

        /// <summary>
        /// The currency name.
        /// </summary>
        private string currencyName;

        /// <summary>
        ///     金融机构
        /// </summary>
        private string institution;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the bank account id.
        /// </summary>
        public string BankAccountId
        {
            get
            {
                return this.bankAccountId;
            }

            set
            {
                this.bankAccountId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the bank account no.
        /// </summary>
        public string SettlementAccount
        {
            get
            {
                return this.bankAccountNo;
            }

            set
            {
                this.bankAccountNo = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the currency id.
        /// </summary>
        public string CurrencyId
        {
            get
            {
                return this.currencyId;
            }

            set
            {
                this.currencyId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the currency name.
        /// </summary>
        public string CurrencyName
        {
            get
            {
                return this.currencyName;
            }

            set
            {
                this.currencyName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the financial institution.
        /// </summary>
        public string FinancialInstitution
        {
            get
            {
                return this.institution;
            }

            set
            {
                this.institution = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion
    }
}