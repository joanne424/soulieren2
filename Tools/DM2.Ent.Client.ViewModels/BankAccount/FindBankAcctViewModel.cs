// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindBankAcctViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/25 09:44:11 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/25 09:44:11
//      修改描述：新建 FindBankAcctViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;

    /// <summary>
    ///     The counterparty add view model.
    /// </summary>
    public class FindBankAcctViewModel : BankCashTransferModel
    {
        #region Fields

        private readonly BankAccountModel relevanceBankAcctId = null;

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IBankAccountRepository bankAcctReps;

        /// <summary>
        /// The bu rep.
        /// </summary>
        private readonly IBusinessUnitRepository buRep;


        /// <summary>
        /// The currency reps.
        /// </summary>
        private readonly ICurrencyRepository currencyReps;

        private readonly ICounterPartyRepository counterpartyRepository;

        private readonly IFinancialInstitutionRepository institutionRepository;

        /// <summary>
        ///     The bank account.
        /// </summary>
        private BankAccountModel bankAccount;

        /// <summary>
        ///     The bank account list.
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
        ///     The currency list.
        /// </summary>
        private ObservableCollection<CurrencyModel> currencyList;

        /// <summary>
        /// The financial institutions.
        /// </summary>
        private ObservableCollection<FinancialInstitutionModel> financialInstitutions;

        /// <summary>
        /// The search key.
        /// </summary>
        private string searchKey;

        /// <summary>
        ///     The selected currency.
        /// </summary>
        private CurrencyModel selectedCurrency;

        private FinancialInstitutionModel selectedInstitution;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FindBankAcctViewModel"/> class.
        /// </summary>
        /// <param name="currencyId">
        /// The currency Id.
        /// </param>
        public FindBankAcctViewModel(BankAccountModel bankAccount = null)
            : this()
        {
            if (bankAccount != null)
            {
                this.relevanceBankAcctId = bankAccount;
            }

            this.Load();
        }

        private FindBankAcctViewModel()
        {
            this.DisplayName = RunTime.FindStringResource("FindBankAccount");
            this.bankAcctReps = this.GetRepository<IBankAccountRepository>();
            this.currencyReps = this.GetRepository<ICurrencyRepository>();
            this.buRep = this.GetRepository<IBusinessUnitRepository>();
            this.counterpartyRepository = this.GetRepository<ICounterPartyRepository>();
            this.institutionRepository = this.GetRepository<IFinancialInstitutionRepository>();

            if (this.BankAccountList == null)
            {
                this.BankAccountList = new ObservableCollection<BankAccountModel>();
            }
        }


        public FindBankAcctViewModel(string businessUnitId, string institutionId, string currencyId)
            : this()
        {
            if (!string.IsNullOrEmpty(businessUnitId))
            {
                this.BusinessUnit = this.buRep.FindByID(businessUnitId);
            }

            if (!string.IsNullOrEmpty(currencyId))
            {
                this.SelectedCurrency = this.currencyReps.FindByID(currencyId);
            }

            if (!string.IsNullOrEmpty(institutionId))
            {
                this.SelectedInstitution = this.institutionRepository.FindByID(institutionId);
            }

            this.Load();
        }
        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the bank account.
        /// </summary>
        public BankAccountModel BankAccount
        {
            get
            {
                return this.bankAccount;
            }

            set
            {
                this.bankAccount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     BankAccountList
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
        ///     BusinessUnitList列表
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
        /// Gets or sets the search key.
        /// </summary>
        public string SearchKey
        {
            get
            {
                return this.searchKey;
            }

            set
            {
                this.searchKey = value;
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
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.selectedCurrency = null;
                    this.NotifyOfPropertyChange();
                    return;
                }

                this.selectedCurrency = value;
                this.NotifyOfPropertyChange();
            }
        }

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


        public FinancialInstitutionModel SelectedInstitution
        {
            get
            {
                return this.selectedInstitution;
            }

            set
            {
                if (string.IsNullOrEmpty(value.Name))
                {
                    this.selectedInstitution = null;
                }
                else
                {
                    this.selectedInstitution = value;
                }

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
        /// The search.
        /// </summary>
        public void Search()
        {
            RunTime.GetCurrentRunTime().ActionOnUiThread(
                () =>
                {
                    if (this.BankAccountList.Count != 0)
                    {
                        this.BankAccountList.Clear();
                    }

                    var tempBankAcctList = new List<BankAccountModel>();
                    IEnumerable<BankAccountModel> tempBankAcctSearch = this.bankAcctReps.Filter(
                        o =>
                        {
                            if (!o.Enabled)
                            {
                                return false;
                            }

                            // 判断Bu
                            if (this.BusinessUnit != null && o.BusinessUnitId != this.BusinessUnit.Id)
                            {
                                return false;
                            }

                            // 判断货币对
                            if (this.SelectedCurrency != null && o.CurrencyId != this.SelectedCurrency.Id)
                            {
                                return false;
                            }

                            if (this.SelectedInstitution != null && o.InstitutionId != this.SelectedInstitution.Id)
                            {
                                return false;
                            }

                            return true;
                        });

                    if (!string.IsNullOrEmpty(this.searchKey))
                    {
                        var tempKey = this.searchKey.ToUpper();
                        tempBankAcctList =
                            tempBankAcctSearch.Where(
                                o =>
                                o.AccountNo.ToUpper().IndexOf(tempKey) >= 0 ||
                                o.AccountName.ToUpper().IndexOf(tempKey) >= 0 ||
                                this.currencyReps.GetName(o.CurrencyId).ToUpper().IndexOf(tempKey) >= 0 ||
                                this.counterpartyRepository.GetName(o.InstitutionId).ToUpper().IndexOf(tempKey) >= 0 ||
                                this.buRep.GetName(o.BusinessUnitId).ToUpper().IndexOf(this.searchKey) >= 0).ToList();
                    }
                    else
                    {
                        tempBankAcctList = tempBankAcctSearch.ToList();
                    }

                    tempBankAcctList.ForEach(s => this.BankAccountList.Add(s));
                    if (this.relevanceBankAcctId != null)
                    {
                        var tempRelevanceBankAcct =
                            this.BankAccountList.FirstOrDefault(o => o.Id == this.relevanceBankAcctId.Id);
                        if (tempRelevanceBankAcct != null)
                        {
                            this.BankAccountList.Remove(tempRelevanceBankAcct);
                        }
                    }
                });
        }

        /// <summary>
        ///     The selected bank acct click.
        /// </summary>
        public void SelectedBankAcctClick()
        {
            this.TryClose();
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        private void Load()
        {
            this.BusinessUnitList = this.buRep.GetBindCollection().ToComboboxBinding(true);
            this.CurrencyList = this.currencyReps.GetBindCollection().ToComboboxBinding(true);
            this.FinancialInstitutions = this.institutionRepository.GetBindCollection().ToComboboxBinding(true);
            if (this.relevanceBankAcctId != null)
            {
                this.SelectedCurrency = this.currencyReps.FindByID(this.relevanceBankAcctId.CurrencyId);
            }

            this.bankAcctReps.SubscribeAddEvent(model => this.Search());
            this.bankAcctReps.SubscribeUpdateEvent((oldModel, newModel) => this.Search());
            this.bankAcctReps.SubscribeRemoveEvent(model => this.Search());
            this.Search();
        }
        #endregion
    }
}