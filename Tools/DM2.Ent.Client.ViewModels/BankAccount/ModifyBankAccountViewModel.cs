// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyBankAccountViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/25 09:44:11 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/25 09:44:11
//      修改描述：新建 ModifyBankAccountViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common;

    /// <summary>
    ///     The counterparty add view model.
    /// </summary>
    public class ModifyBankAccountViewModel : BankAccountModel
    {
        #region Fields

        /// <summary>
        ///     The business unit.
        /// </summary>
        private BusinessUnitModel businessUnit;

        /// <summary>
        ///     The business unit list.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnitList;

        /// <summary>
        ///     The hedge account.
        /// </summary>
        private FinancialInstitutionModel financialInstitution;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<FinancialInstitutionModel> financialInstitutions;

        /// <summary>
        /// The currency list.
        /// </summary>
        private ObservableCollection<CurrencyModel> currencyList;

        /// <summary>
        /// The selected currency.
        /// </summary>
        private CurrencyModel selectedCurrency;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyBankAccountViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public ModifyBankAccountViewModel(BankAccountModel model)
        {
            try
            {
                this.BusinessUnitList = this.GetRepository<IBusinessUnitRepository>().GetBindCollection().ToComboboxBinding();
                this.FinancialInstitutions = this.GetRepository<IFinancialInstitutionRepository>().GetBindCollection().ToComboboxBinding();
                this.CurrencyList = this.GetRepository<ICurrencyRepository>().GetBindCollection().ToComboboxBinding();
                this.Copy(model);
                this.SelectedCurrency = this.CurrencyList.FirstOrDefault(o => o.Id == model.CurrencyId);
                this.BusinessUnit = this.BusinessUnitList.FirstOrDefault(o => o.Id == model.BusinessUnitId);
                this.FinancialInstitution = this.FinancialInstitutions.FirstOrDefault(o => o.Id == model.InstitutionId);
            }
            catch (Exception ex)
            {
                Infrastructure.Log.TraceManager.Error.Write("ModifyBankAccountViewModel", ex);
            }

            this.DisplayName = RunTime.FindStringResource("ModifyBankAccount");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the business unit.
        /// </summary>
        public BusinessUnitModel BusinessUnit
        {
            get
            {
                return this.businessUnit;
            }

            set
            {
                this.businessUnit = value;
                this.BusinessUnitId = value.Id;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     symbolList 货币对列表
        /// </summary>
        public ObservableCollection<BusinessUnitModel> BusinessUnitList
        {
            get
            {
                return this.businessUnitList;
            }

            set
            {
                this.businessUnitList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the FinancialInstitution.
        /// </summary>
        public FinancialInstitutionModel FinancialInstitution
        {
            get
            {
                return this.financialInstitution;
            }

            set
            {
                this.financialInstitution = value;
                this.NotifyOfPropertyChange(() => this.FinancialInstitution);
            }
        }

        /// <summary>
        ///     金融机构列表
        /// </summary>
        public ObservableCollection<FinancialInstitutionModel> FinancialInstitutions
        {
            get
            {
                return this.financialInstitutions;
            }

            set
            {
                this.financialInstitutions = value;
                this.NotifyOfPropertyChange(() => this.FinancialInstitutions);
            }
        }

        /// <summary>
        ///     currencyList 货币对列表
        /// </summary>
        public ObservableCollection<CurrencyModel> CurrencyList
        {
            get
            {
                return this.currencyList;
            }

            set
            {
                this.currencyList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the business unit.
        /// </summary>
        public CurrencyModel SelectedCurrency
        {
            get
            {
                return this.selectedCurrency;
            }

            set
            {
                this.selectedCurrency = value;
                this.CurrencyId = value.Id;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     取消操作
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void OnClosed()
        {
            this.TryClose(true);
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        public void OnMinimizeWindowCommand(object window)
        {
            var win = window as Window;
            win.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// The save.
        /// </summary>
        public void Save()
        {
            if (!this.ValidateforSumbit())
            {
                return;
            }

            if (RunTime.ShowConfirmDialogWithoutRes(RunTime.FindStringResource("MSG_00003"), string.Empty, this.OwnerId))
            {
                var service = new BankAccountService(this.OwnerId);
                BankAccountModel model = EmitMap.Map<BankAccountModel, BankAccountModel>(this);
                model.BusinessUnitId = this.BusinessUnit.Id;
                model.InstitutionId = this.FinancialInstitution.Id;
                model.CurrencyId = this.SelectedCurrency.Id;
                model.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
                CmdResult drs = service.Modify(model);
                if (drs.Success)
                {
                    RunTime.ShowSuccessInfoDialogWithoutRes(
                        RunTime.FindStringResource("MSG_00001"),
                        string.Empty,
                        this.OwnerId);
                }
                else
                {
                    RunTime.ShowFailInfoDialog(drs.ErrorCode, string.Empty, this.OwnerId);
                }
            }
        }

        #endregion

        #region Methods

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
            if (propertyName == "AccountNo")
            {
                if (string.IsNullOrEmpty(this.AccountNo))
                {
                    return RunTime.FindStringResource("MSG_00010");
                }
            }

            if (propertyName == "AccountName")
            {
                if (string.IsNullOrEmpty(this.AccountName))
                {
                    return RunTime.FindStringResource("MSG_00010");
                }
            }

            return null;
        }

        /// <summary>
        /// The on validated.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected override string OnValidated()
        {
            return base.OnValidated();
        }

        #endregion
    }
}