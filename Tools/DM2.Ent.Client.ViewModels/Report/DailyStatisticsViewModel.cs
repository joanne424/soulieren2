// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DailyStatisticsViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.Generic;

    using Caliburn.Micro;

    /// <summary>
    ///     每日资金统计
    /// </summary>
    public class DailyStatisticsViewModel : PropertyChangedBase, IComparable<DailyStatisticsViewModel>
    {
        #region Static Fields

        /// <summary>
        ///     比较器
        /// </summary>
        public static readonly IComparer<DailyStatisticsViewModel> Comparer = new DailyStatisticsComparer();

        #endregion

        #region Fields

        /// <summary>
        ///     分公司ID
        /// </summary>
        private string businessUnitId;

        /// <summary>
        /// The business unit name.
        /// </summary>
        private string businessUnitName;

        /// <summary>
        ///     交易对手ID
        /// </summary>
        private string counterPartyId;

        /// <summary>
        /// The counter party name.
        /// </summary>
        private string counterPartyName;

        /// <summary>
        /// The currency details.
        /// </summary>
        private IDictionary<string, CurrencyDetailModel> currencyDetails;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the aud.
        /// </summary>
        public CurrencyDetailModel AUD
        {
            get
            {
                return this.GetCurrencyDetailModel("AUD");
            }
        }

        /// <summary>
        ///     分公司ID
        /// </summary>
        public string BusinessUnitId
        {
            get
            {
                return this.businessUnitId;
            }

            set
            {
                this.businessUnitId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the business unit name.
        /// </summary>
        public string BusinessUnitName
        {
            get
            {
                return this.businessUnitName;
            }

            set
            {
                this.businessUnitName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the cad.
        /// </summary>
        public CurrencyDetailModel CAD
        {
            get
            {
                return this.GetCurrencyDetailModel("CAD");
            }
        }

        /// <summary>
        /// Gets the chf.
        /// </summary>
        public CurrencyDetailModel CHF
        {
            get
            {
                return this.GetCurrencyDetailModel("CHF");
            }
        }

        /// <summary>
        /// Gets the cnh.
        /// </summary>
        public CurrencyDetailModel CNH
        {
            get
            {
                return this.GetCurrencyDetailModel("CNH");
            }
        }

        /// <summary>
        /// Gets the cny.
        /// </summary>
        public CurrencyDetailModel CNY
        {
            get
            {
                return this.GetCurrencyDetailModel("CNY");
            }
        }

        /// <summary>
        ///     交易对手ID
        /// </summary>
        public string CounterPartyId
        {
            get
            {
                return this.counterPartyId;
            }

            set
            {
                this.counterPartyId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the counter party name.
        /// </summary>
        public string CounterPartyName
        {
            get
            {
                return this.counterPartyName;
            }

            set
            {
                this.counterPartyName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the currency details.
        /// </summary>
        public IDictionary<string, CurrencyDetailModel> CurrencyDetails
        {
            get
            {
                return this.currencyDetails;
            }

            set
            {
                this.currencyDetails = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the eur.
        /// </summary>
        public CurrencyDetailModel EUR
        {
            get
            {
                return this.GetCurrencyDetailModel("EUR");
            }
        }

        /// <summary>
        /// Gets the gbp.
        /// </summary>
        public CurrencyDetailModel GBP
        {
            get
            {
                return this.GetCurrencyDetailModel("GBP");
            }
        }

        /// <summary>
        /// Gets the hkd.
        /// </summary>
        public CurrencyDetailModel HKD
        {
            get
            {
                return this.GetCurrencyDetailModel("HKD");
            }
        }

        /// <summary>
        /// Gets the jpy.
        /// </summary>
        public CurrencyDetailModel JPY
        {
            get
            {
                return this.GetCurrencyDetailModel("JPY");
            }
        }

        /// <summary>
        /// Gets the nzd.
        /// </summary>
        public CurrencyDetailModel NZD
        {
            get
            {
                return this.GetCurrencyDetailModel("NZD");
            }
        }

        /// <summary>
        /// Gets the sgd.
        /// </summary>
        public CurrencyDetailModel SGD
        {
            get
            {
                return this.GetCurrencyDetailModel("SGD");
            }
        }

        /// <summary>
        /// Gets the usd.
        /// </summary>
        public CurrencyDetailModel USD
        {
            get
            {
                return this.GetCurrencyDetailModel("USD");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(DailyStatisticsViewModel other)
        {
            return Comparer.Compare(this, other);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get currency detail model.
        /// </summary>
        /// <param name="currency">
        /// The currency.
        /// </param>
        /// <returns>
        /// The <see cref="CurrencyDetailModel"/>.
        /// </returns>
        private CurrencyDetailModel GetCurrencyDetailModel(string currency)
        {
            CurrencyDetailModel detail;
            if (this.currencyDetails.TryGetValue(currency, out detail))
            {
                if (detail.IsEmpty())
                {
                    detail = null;
                }
            }

            return detail;
        }

        #endregion

        /// <summary>
        /// The daily statistics comparer.
        /// </summary>
        private class DailyStatisticsComparer : IComparer<DailyStatisticsViewModel>
        {
            // public static readonly IComparer<DailyStatisticsViewModel> Instance = new DailyStatisticsComparer();
            #region Public Methods and Operators

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// The <see cref="int"/>.
            /// </returns>
            public int Compare(DailyStatisticsViewModel x, DailyStatisticsViewModel y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                int stringComparResult = string.Compare(
                    x.CounterPartyName, 
                    y.CounterPartyName, 
                    StringComparison.OrdinalIgnoreCase);
                if (stringComparResult != 0)
                {
                    return stringComparResult;
                }

                return string.Compare(x.BusinessUnitName, y.BusinessUnitName, StringComparison.OrdinalIgnoreCase);
            }

            #endregion
        }

        // public void Transfer(string currencyId)
        // {
        // if (string.IsNullOrEmpty(this.CounterPartyId) && string.IsNullOrEmpty(this.BusinessUnitId))
        // {
        // return;
        // }

        // var windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        // if (!string.IsNullOrEmpty(this.CounterPartyId))
        // {
        // var bankAccountTransfer = new BankAccountTransferViewModel();
        // var parameter = new ParameterOverrides { { "varOwnerId", string.Empty } };
        // var bankAccountRepository = IocContainer.Instance.Container.Resolve<IBankAccountRepository>(parameter);
        // var bankAccount = bankAccountRepository.Filter(item => item.CounterpartyId == this.CounterPartyId).FirstOrDefault();
        // if (bankAccount != null)
        // {
        // bankAccountTransfer.ToBankAccount = bankAccount;
        // }
        // bankAccountTransfer.CcyId = currencyId;

        // windowManager.ShowWindow(bankAccountTransfer);
        // return;
        // }
        // }
    }
}