// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessUnitAddViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.BusinessUnit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common.Enums;

    /// <summary>
    /// The business unit add view model.
    /// </summary>
    public class BusinessUnitAddViewModel : BusinessUnitModel
    {
        #region Fields


        private ObservableCollection<CountryModel> countries;

        private ObservableCollection<CurrencyModel> currencies;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitAddViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public BusinessUnitAddViewModel(string ownerId)
            : base(ownerId)
        {
            ICountryRepository countryRepository = this.GetRepository<ICountryRepository>();
            this.Countries = countryRepository.GetBindCollection().ToComboboxBinding();

            ICurrencyRepository currencyRepository = this.GetRepository<ICurrencyRepository>();
            this.Currencies = currencyRepository.GetBindCollection().ToComboboxBinding();

            var cn = this.Countries.FirstOrDefault(country => country.Name == "CN");
            if (cn != null)
            {
                this.CountryId = cn.Id;
            }

            var cny = this.Currencies.FirstOrDefault(country => country.Name == "CNY");
            if (cny != null)
            {
                this.LocalCcyId = cny.Id;
            }

            this.TimeZone = TimeZoneEnum.GMT8;

            this.DateFormat = DateFormatEnum.YYYY_MM_DD;

            this.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            this.IsReadied = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the business unit groups.
        /// </summary>
        public IDictionary<string, string> BusinessUnitGroups { get; set; }

        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        public ObservableCollection<CountryModel> Countries
        {
            get
            {
                return this.countries;
            }
            set
            {
                this.countries = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the currencies.
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
        /// 页面数据是否准备就绪
        /// </summary>
        protected bool IsReadied { get; set; }
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
        /// The on saved.
        /// </summary>
        public virtual void OnSaved()
        {
            this.IsReadied = true;

            if (!this.ValidateforSumbit())
            {
                return;
            }

            if (!RunTime.ShowConfirmDialog("MSG_00002", string.Empty, this.OwnerId))
            {
                return;
            }

            var result = this.GetSevice<BusinessUnitService>().AddBusinessUnit(this);

            if (result.Success)
            {
                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
                this.Reset();
            }
            else
            {
                RunTime.ShowFailInfoDialog(result.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 重置
        /// </summary>
        protected void Reset()
        {
            this.IsReadied = false;

            this.Name = string.Empty;
            this.GroupId = string.Empty;
            this.CountryId = string.Empty;
            this.TimeZone = TimeZoneEnum.GMT8;
            this.DateFormat = DateFormatEnum.YYYY_MM_DD;
            this.LocalCcyId = string.Empty;
        }

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
            if (!this.IsReadied)
            {
                return null;
            }

            if (propertyName == "Name" && string.IsNullOrEmpty(this.Name))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "CountryId" && string.IsNullOrEmpty(this.CountryId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "LocalCcyId" && string.IsNullOrEmpty(this.LocalCcyId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            return null;
        }

        #endregion
    }
}