// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankAccountListViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 03:23:23 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 03:23:23
//      修改描述：新建 BankAccountListViewModel.cs
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The deal list view model.
    /// </summary>
    public class BankAccountListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IBankAccountRepository bankAcctReps;

        /// <summary>
        ///     The bu rep.
        /// </summary>
        private readonly IBusinessUnitRepository buRep;

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     DealService
        /// </summary>
        private readonly HedgingDealService dealService;

        /// <summary>
        ///     运行时
        /// </summary>
        private readonly RunTime runTime;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The hedging deal id.
        /// </summary>
        private string accountName;

        /// <summary>
        ///     查询DealID条件
        /// </summary>
        private string accountNo;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<BankAccountModel> bankAccountList;

        /// <summary>
        ///     The business unit.
        /// </summary>
        private BusinessUnitModel businessUnit;

        /// <summary>
        ///     The business unit list.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnitList;

        /// <summary>
        ///     查询Channal条件
        /// </summary>
        private string channel;

        /// <summary>
        ///     The hedge account.
        /// </summary>
        private FinancialInstitutionModel financialInstitution;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<FinancialInstitutionModel> financialInstitutions;

        /// <summary>
        ///     symbolList
        /// </summary>
        private ObservableCollection<CurrencyModel> currencyList;

        /// <summary>
        ///     The current search page index.
        /// </summary>
        private int currentSearchPageIndex = 1;

        /// <summary>
        ///     The current search page size.
        /// </summary>
        private int currentSearchPageSize = 15;

        /// <summary>
        ///     查询CustomerNo条件
        /// </summary>
        private string customerId;

        /// <summary>
        ///     The external deal set id.
        /// </summary>
        private string externalDealSetId;

        /// <summary>
        ///     The select item.
        /// </summary>
        private BankAccountModel selectItem;

        /// <summary>
        ///     字段 货币
        /// </summary>
        private string selectedContract;

        /// <summary>
        ///     The selected currency.
        /// </summary>
        private CurrencyModel selectedCurrency;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BankAccountListViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public BankAccountListViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("BankAccountList");
            this.currencyRep = this.GetRepository<ICurrencyRepository>();
            this.buRep = this.GetRepository<IBusinessUnitRepository>();
            this.bankAcctReps = this.GetRepository<IBankAccountRepository>();

            this.runTime = RunTime.GetCurrentRunTime(varOwnerId);

            this.BusinessUnitList = this.buRep.GetBindCollection().ToComboboxBinding(true);
            this.CurrencyList = this.currencyRep.GetBindCollection().ToComboboxBinding(true);
            if (this.BankAccountList == null)
            {
                this.BankAccountList = new ObservableCollection<BankAccountModel>();
            }

            this.FinancialInstitutions = this.GetRepository<IFinancialInstitutionRepository>().GetBindCollection().ToComboboxBinding();
            this.BuSelectionChangedCommand = new RelayCommand(this.BuSelectionChanged);
            Task.Factory.StartNew(RunTime.GetCurrentRunTime().CurrentRepositoryCore.WaitAllInitial)
                .ContinueWith(this.Load);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the hedging deal id.
        /// </summary>
        public string AccountName
        {
            get
            {
                return this.accountName;
            }

            set
            {
                this.accountName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     查询AccountNo条件
        /// </summary>
        public string AccountNo
        {
            get
            {
                return this.accountNo;
            }

            set
            {
                this.accountNo = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     交易单列表
        /// </summary>
        public ObservableCollection<BankAccountModel> BankAccountList
        {
            get
            {
                return this.bankAccountList;
            }

            set
            {
                this.bankAccountList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CounterParty 选中事件
        /// </summary>
        public RelayCommand BuSelectionChangedCommand { get; private set; }

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
        ///     symbolList 货币对列表
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
        ///     Gets or sets the select item.
        /// </summary>
        public BankAccountModel SelectItem
        {
            get
            {
                return this.selectItem;
            }

            set
            {
                this.selectItem = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the business unit.
        /// </summary>
        public BusinessUnitModel SelectedBusinessUnit
        {
            get
            {
                return this.businessUnit;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.businessUnit = null;
                    this.NotifyOfPropertyChange();
                    return;
                }

                this.businessUnit = value;
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
        ///     HedgeAccount实体
        /// </summary>
        public CurrencyModel SelectedCurrency
        {
            get
            {
                return this.selectedCurrency;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.selectedCurrency = null;
                    this.NotifyOfPropertyChange(() => this.SelectedCurrency);
                    return;
                }

                this.selectedCurrency = value;
                this.NotifyOfPropertyChange(() => this.SelectedCurrency);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The close.
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public override void Dispose()
        {
            Messenger.Default.Unregister<string>(this, "UpdateLanguage");
        }

        /// <summary>
        ///     延迟初始化
        /// </summary>
        public void LazyInitial()
        {
            this.bankAcctReps.SubscribeAddEvent(this.Refresh);
            this.bankAcctReps.SubscribeUpdateEvent((oldDeal, newDeal) => this.Refresh(newDeal));
            this.bankAcctReps.SubscribeRemoveEvent(this.Refresh);
        }


        /// <summary>
        ///     The modify bank acct.
        /// </summary>
        public void ModifyBankAcct()
        {
            if (this.selectItem == null)
            {
                return;
            }

            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            // settings.Topmost = true;

            var vm = new ModifyBankAccountViewModel(this.selectItem);
            this.windowManager.ShowWindow(vm, null, settings);
        }

        /// <summary>
        ///     The new_ click.
        /// </summary>
        public void New_Click()
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            // settings.Topmost = true;

            var vm = new NewBankAccountViewModel();
            this.windowManager.ShowWindow(vm, null, settings);
        }

        /// <summary>
        ///     Search(查询)按钮点击事件
        /// </summary>
        /// <param name="pageIndex">
        ///     The page Index.
        /// </param>
        /// <param name="pageSize">
        ///     The page Size.
        /// </param>
        public void Search()
        {
            RunTime.GetCurrentRunTime().ActionOnUiThread(
                () =>
                {
                    if (this.BankAccountList.Count != 0)
                    {
                        this.BankAccountList.Clear();
                    }

                    // 根据条件获取所有符合条件的Deal
                    List<BankAccountModel> tempBankAcctList = this.bankAcctReps.Filter(
                        o =>
                        {
                            // 判断DealID(模糊查询)
                            if (!string.IsNullOrEmpty(this.AccountNo)
                                && o.AccountNo.IndexOf(this.AccountNo, StringComparison.Ordinal) < 0)
                            {
                                return false;
                            }

                            if (!string.IsNullOrEmpty(this.AccountName)
                                && o.AccountName.IndexOf(this.AccountName, StringComparison.Ordinal) < 0)
                            {
                                return false;
                            }

                            // 判断Bu
                            if (this.SelectedBusinessUnit != null
                                && o.BusinessUnitId != this.SelectedBusinessUnit.Id)
                            {
                                return false;
                            }

                            // 判断Bu
                            if (this.FinancialInstitution != null
                                && o.InstitutionId != this.FinancialInstitution.Id)
                            {
                                return false;
                            }

                            // 判断货币对
                            if (this.SelectedCurrency != null && o.CurrencyId != this.SelectedCurrency.Id)
                            {
                                return false;
                            }

                            return true;
                        })
                        .OrderBy(p => p.BusinessUnitId)
                        .ThenBy(p => p.InstitutionId)
                        .ThenBy(p => p.AccountName)
                        .ToList();

                    tempBankAcctList.ForEach(s => { this.BankAccountList.Add(s); });
                });
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void BuSelectionChanged()
        {
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        private void Load(Task task)
        {
            this.bankAcctReps.SubscribeAddEvent(model => this.Search());
            this.bankAcctReps.SubscribeUpdateEvent((oldModel, newModel) => this.Search());
            this.bankAcctReps.SubscribeRemoveEvent(model => this.Search());
            this.Search();
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        /// <param name="changedBankAcct">
        /// The changed Bank Acct.
        /// </param>
        private void Refresh(BankAccountModel changedBankAcct)
        {
            if (this.bankAccountList == null || changedBankAcct == null)
            {
            }
        }

        #endregion
    }
}