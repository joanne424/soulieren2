// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterpartyAddViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Counterparty
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Caliburn.Micro;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The counterparty add view model.
    /// </summary>
    public class CounterpartyAddViewModel : CounterPartyModel
    {
        ///// <summary>
        ///// Email格式
        ///// </summary>
        // private const string emailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        #region Fields

        /// <summary>
        /// The counterparty group repository.
        /// </summary>
        private readonly ICounterPartyGroupRepository counterpartyGroupRepository;

        /// <summary>
        /// The institution repository.
        /// </summary>
        private readonly IFinancialInstitutionRepository institutionRepository;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The business units.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnits;

        /// <summary>
        /// The counterparty groups.
        /// </summary>
        private ObservableCollection<CounterPartyGroupModel> counterpartyGroups;

        /// <summary>
        ///     The currencies.
        /// </summary>
        private ObservableCollection<CurrencyModel> currencies;

        /// <summary>
        /// The display connection page.
        /// </summary>
        private Visibility displayConnectionPage = Visibility.Collapsed;

        /// <summary>
        ///     The display connection parameters panel.
        /// </summary>
        private Visibility displayConnectionParametersPanel = Visibility.Hidden;

        /// <summary>
        /// The display main page.
        /// </summary>
        private Visibility displayMainPage = Visibility.Visible;

        /// <summary>
        /// The financial institutions.
        /// </summary>
        private ObservableCollection<FinancialInstitutionModel> financialInstitutions;

        /// <summary>
        /// The selected currency id.
        /// </summary>
        private string selectedCurrencyId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartyAddViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public CounterpartyAddViewModel(string ownerId)
            : base(ownerId)
        {
            this.counterpartyGroupRepository = this.GetRepository<ICounterPartyGroupRepository>();
            this.institutionRepository = this.GetRepository<IFinancialInstitutionRepository>();

            this.BusinessUnits = this.GetRepository<IBusinessUnitRepository>().GetBindCollection().ToComboboxBinding();
            this.Currencies = this.GetRepository<ICurrencyRepository>().GetBindCollection().ToComboboxBinding();
            this.FinancialInstitutions =
                this.GetRepository<IFinancialInstitutionRepository>().GetBindCollection().ToComboboxBinding();
            this.SettlementAccountList = new ObservableCollection<SettlementAccountViewModel>();

            this.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            this.IsReadied = false;

            var dynamicCodeRepository = this.GetRepository<IDynamicCodeRepository>();

            DynamicCodeModel connectionType =
                dynamicCodeRepository.Filter(p => p.Name == DynamicCodeNameEnum.CONNECTION_TYPE.ToString())
                    .FirstOrDefault();

            if (connectionType != null)
            {
                this.ConnectionTypes =
                    connectionType.GetDictionary()
                        .ToDictionary(item => (ConnectionTypeEnum)item.Key.ToInt32(), item => item.Value);
            }

            DynamicCodeModel emailTemplate =
                dynamicCodeRepository.Filter(p => p.Name == DynamicCodeNameEnum.EMAIL_IMPORT_TEMPLATE.ToString())
                    .FirstOrDefault();
            if (emailTemplate != null)
            {
                this.EmailTemplates = emailTemplate.GetDictionary();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the business units.
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
        /// Gets or sets the connection types.
        /// </summary>
        public IDictionary<ConnectionTypeEnum, string> ConnectionTypes { get; set; }

        /// <summary>
        ///     Group列表
        /// </summary>
        public ObservableCollection<CounterPartyGroupModel> CounterpartyGroups
        {
            get
            {
                return this.counterpartyGroups;
            }

            set
            {
                this.counterpartyGroups = value;
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
        ///     显示连接类型页
        /// </summary>
        public Visibility DisplayConnectionPage
        {
            get
            {
                return this.displayConnectionPage;
            }

            set
            {
                this.displayConnectionPage = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     显示连接参数面板状态
        /// </summary>
        public Visibility DisplayConnectionParametersPanel
        {
            get
            {
                return this.displayConnectionParametersPanel;
            }

            set
            {
                this.displayConnectionParametersPanel = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     显示主页
        /// </summary>
        public Visibility DisplayMainPage
        {
            get
            {
                return this.displayMainPage;
            }

            set
            {
                this.displayMainPage = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the email templates.
        /// </summary>
        public IDictionary<string, string> EmailTemplates { get; set; }

        /// <summary>
        ///     金融机构
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
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected currency id.
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
        /// Gets or sets the settlement account list.
        /// </summary>
        public ObservableCollection<SettlementAccountViewModel> SettlementAccountList { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     页面数据是否准备就绪
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
        /// The find bank account.
        /// </summary>
        public void FindBankAccount()
        {
            var findBankAcct = new FindBankAcctViewModel(
                this.BusinessUnitId, 
                this.InstitutionId, 
                this.SelectedCurrencyId);
            this.windowManager.ShowDialog(findBankAcct);

            if (findBankAcct.BankAccount == null)
            {
                return;
            }

            SettlementAccountViewModel account =
                this.SettlementAccountList.FirstOrDefault(p => p.CurrencyId == findBankAcct.BankAccount.CurrencyId);
            if (account == null)
            {
                account = new SettlementAccountViewModel { CurrencyId = findBankAcct.BankAccount.CurrencyId };
                account.CurrencyName = this.Currencies.First(p => p.Id == account.CurrencyId).Name;
                this.Fill(account, findBankAcct.BankAccount);

                this.SettlementAccountList.Add(account);
                return;
            }

            if (account.BankAccountId == findBankAcct.BankAccount.Id)
            {
                return;
            }

            if (!RunTime.ShowConfirmDialog("MSG_10057", string.Empty, this.OwnerId))
            {
                return;
            }

            this.Fill(account, findBankAcct.BankAccount);
        }

        /// <summary>
        ///     当Bu变更时
        /// </summary>
        public void OnBusinessUnitChanged()
        {
            this.CounterpartyGroups =
                this.counterpartyGroupRepository.Filter(o => o.BusinessUnitId == this.BusinessUnitId)
                    .ToComboboxBinding();

            this.GroupId = string.Empty;
        }

        /// <summary>
        ///     当连接类型变更时
        /// </summary>
        public void OnConnectionTypeChanged()
        {
            this.DisplayConnectionParametersPanel = this.ConnectionType == ConnectionTypeEnum.EMAIL
                                                        ? Visibility.Visible
                                                        : Visibility.Collapsed;

            // if (this.ConnectionType == ConnectionTypeEnum.EMAIL)
            // {
            // if (this.EmailTimeZone == TimeZoneEnum.GMT8)
            // {
            // this.EmailTimeZone = TimeZoneEnum.GMT8;
            // }
            // }
        }

        /// <summary>
        ///     点击下一页
        /// </summary>
        public void OnNextPaged()
        {
            this.IsReadied = true;

            if (!this.ValidateforSumbit())
            {
                return;
            }

            this.DisplayMainPage = Visibility.Collapsed;
            this.DisplayConnectionPage = Visibility.Visible;
        }

        /// <summary>
        ///     点击上一页
        /// </summary>
        public void OnPrevPaged()
        {
            this.DisplayMainPage = Visibility.Visible;
            this.DisplayConnectionPage = Visibility.Collapsed;
        }

        /// <summary>
        ///     点击保存
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

            this.SettlementAccounts = this.SettlementAccountList.ToDictionary(
                item => item.CurrencyId,
                item => item.BankAccountId.ToInt32());

            CmdResult result = this.GetSevice<CounterPartyService>().AddCounterparty(this);

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

            if (propertyName == "BusinessUnitId" && string.IsNullOrEmpty(this.BusinessUnitId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "Name" && string.IsNullOrEmpty(this.Name))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "GroupId" && string.IsNullOrEmpty(this.GroupId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (this.DisplayConnectionPage == Visibility.Visible && this.ConnectionType == ConnectionTypeEnum.EMAIL)
            {
                if (propertyName == "EmailAddress" && !this.EmailAddress.IsEmail())
                {
                    return RunTime.FindStringResource("MSG_00005");
                }

                if (propertyName == "EmailLoginName" && string.IsNullOrEmpty(this.EmailLoginName))
                {
                    return RunTime.FindStringResource("MSG_00010");
                }

                if (propertyName == "EmailPassword" && string.IsNullOrEmpty(this.EmailPassword))
                {
                    return RunTime.FindStringResource("MSG_00010");
                }

                if (propertyName == "EmailImportTemplate" && string.IsNullOrEmpty(this.EmailImportTemplate))
                {
                    return RunTime.FindStringResource("MSG_00010");
                }
            }

            return null;
        }

        /// <summary>
        ///     重置
        /// </summary>
        protected void Reset()
        {
            this.IsReadied = false;

            this.BusinessUnitId = string.Empty;
            this.Name = string.Empty;
            this.FullName = string.Empty;
            this.ConnectionType = ConnectionTypeEnum.MANUAL;
            this.EmailAddress = string.Empty;
            this.EmailLoginName = string.Empty;
            this.EmailPassword = string.Empty;
            this.EmailImportTemplate = string.Empty;
            this.EmailTimeZone = TimeZoneEnum.GMT8;
            this.GroupId = string.Empty;
            this.InstitutionId = string.Empty;
            this.SelectedCurrencyId = string.Empty;
            this.SettlementAccountList.Clear();

            this.OnPrevPaged();
        }

        /// <summary>
        /// The fill.
        /// </summary>
        /// <param name="settlementAccount">
        /// The settlement account.
        /// </param>
        /// <param name="bankAccount">
        /// The bank account.
        /// </param>
        protected void Fill(SettlementAccountViewModel settlementAccount, BankAccountModel bankAccount)
        {
            settlementAccount.BankAccountId = bankAccount.Id;
            settlementAccount.SettlementAccount = string.Format(
                "'{0}' - {1}", 
                bankAccount.AccountNo, 
                bankAccount.AccountName);
            settlementAccount.FinancialInstitution = this.institutionRepository.GetName(bankAccount.InstitutionId);
        }

        #endregion
    }
}