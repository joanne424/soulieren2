// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActualCashReportViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Banclogix.Controls.PagedDataGrid;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.BankAccount;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common.Enums;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The actual cash report view model.
    /// </summary>
    public class ActualCashReportViewModel : BaseVm
    {
        #region Fields


        /// <summary>
        /// The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The actual cash list.
        /// </summary>
        private ObservableCollection<ActualCashModel> actualCashList;

        private string enterpriseId;

        /// <summary>
        /// The local trade date from.
        /// </summary>
        private DateTime? localTradeDateFrom;

        /// <summary>
        /// The local trade date to.
        /// </summary>
        private DateTime? localTradeDateTo;

        /// <summary>
        ///     分页控件的命令处理属性
        /// </summary>
        private string pageCommand;

        /// <summary>
        ///     查询订单数据的总页数
        /// </summary>
        private int pageCount;

        /// <summary>
        /// The search bank account.
        /// </summary>
        private string searchBankAccount;

        /// <summary>
        /// The search bank account id.
        /// </summary>
        private string searchBankAccountId;

        /// <summary>
        /// The search business unit id.
        /// </summary>
        private string searchBusinessUnitId;

        /// <summary>
        /// The search deal id.
        /// </summary>
        private string searchDealId;

        /// <summary>
        /// The search hedge deal id.
        /// </summary>
        private string searchHedgeDealId;

        /// <summary>
        /// The search instrument id.
        /// </summary>
        private string searchInstrumentId;

        /// <summary>
        /// The selected actual cash.
        /// </summary>
        private ActualCashModel selectedActualCash;


        private ObservableCollection<BusinessUnitModel> businessUnits;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActualCashReportViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public ActualCashReportViewModel(string ownerId)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("BankAccountActualCashReport");
            //this.BusinessUnits =
            //    this.GetRepository<IBusinessUnitRepository>()
            //        .Filter(bu => !string.IsNullOrEmpty(bu.Id))
            //        .ToDictionary(bu => bu.Id, bu => bu.Name);
            this.BusinessUnits =
                this.GetRepository<IBusinessUnitRepository>().GetBindCollection().ToComboboxBinding(true);

            //this.Instruments =
            //    this.GetRepository<IInstrumentRepository>()
            //        .Filter(item => item.BusinessType == BusinessTypeEnum.ACTUAL_CASH)
            //        .ToDictionary(item => item.Id, item => item.Name);
            this.Instruments = new Dictionary<string, string>() { { string.Empty, string.Empty } };
            this.GetRepository<IInstrumentRepository>()
                    .Filter(item => item.BusinessType == BusinessTypeEnum.ACTUAL_CASH)
                    .OrderBy(item => item.Name)
                    .ForEach(item => this.Instruments.Add(item.Id, item.Name));


            this.ActualCashList = new ObservableCollection<ActualCashModel>();

            this.searchBankAccountId = string.Empty;
            this.enterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the actual cash list.
        /// </summary>
        public ObservableCollection<ActualCashModel> ActualCashList
        {
            get
            {
                return this.actualCashList;
            }

            set
            {
                this.actualCashList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// 组织机构列表
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
        ///     业务方式列表
        /// </summary>
        public IDictionary<string, string> Instruments { get; set; }

        /// <summary>
        /// Gets or sets the local trade date from.
        /// </summary>
        public DateTime? LocalTradeDateFrom
        {
            get
            {
                return this.localTradeDateFrom;
            }

            set
            {
                this.localTradeDateFrom = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the local trade date to.
        /// </summary>
        public DateTime? LocalTradeDateTo
        {
            get
            {
                return this.localTradeDateTo;
            }

            set
            {
                this.localTradeDateTo = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     分页控件的命令处理属性
        /// </summary>
        public string PageCommand
        {
            get
            {
                return this.pageCommand;
            }

            set
            {
                this.pageCommand = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the page count.
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.pageCount;
            }

            set
            {
                this.pageCount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the search bank account.
        /// </summary>
        public string SearchBankAccount
        {
            get
            {
                return this.searchBankAccount;
            }

            set
            {
                this.searchBankAccount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the search bank account id.
        /// </summary>
        public string SearchBankAccountId
        {
            get
            {
                return this.searchBankAccountId;
            }

            set
            {
                this.searchBankAccountId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the search business unit id.
        /// </summary>
        public string SearchBusinessUnitId
        {
            get
            {
                return this.searchBusinessUnitId;
            }

            set
            {
                this.searchBusinessUnitId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the search deal id.
        /// </summary>
        public string SearchDealId
        {
            get
            {
                return this.searchDealId;
            }

            set
            {
                this.searchDealId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the search hedge deal id.
        /// </summary>
        public string SearchHedgeDealId
        {
            get
            {
                return this.searchHedgeDealId;
            }

            set
            {
                this.searchHedgeDealId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the search instrument id.
        /// </summary>
        public string SearchInstrumentId
        {
            get
            {
                return this.searchInstrumentId;
            }

            set
            {
                this.searchInstrumentId = value;
                this.NotifyOfPropertyChange();
            }
        }


        public ActualCashModel SelectedActualCash
        {
            get
            {
                return this.selectedActualCash;
            }

            set
            {
                this.selectedActualCash = value;
                this.NotifyOfPropertyChange();
            }
        }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The cancel.
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
            var findBankAcct = new FindBankAcctViewModel();
            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.SearchBankAccountId = findBankAcct.BankAccount.Id;
                this.SearchBankAccount = findBankAcct.BankAccount.AccountName;
            }
        }

        /// <summary>
        /// clear bank account.
        /// </summary>
        public void ClearBankAccount()
        {
            this.SearchBankAccountId = string.Empty;
            this.SearchBankAccount = string.Empty;
        }
        
        /// <summary>
        /// The preview.
        /// </summary>
        public void Preview()
        {
            if (this.selectedActualCash == null)
            {
                return;
            }

            var vm = new PreviewActualCashViewModel(this.OwnerId, this.selectedActualCash);
            this.windowManager.ShowDialog(vm);
        }

        /// <summary>
        /// The search.
        /// </summary>
        public void Search(int pageIndex, int pageSize)
        {
            this.actualCashList.Clear();

            var reslut = this.GetSevice<ActualCashService>()
                .Search(
                    this.enterpriseId,
                    this.searchBusinessUnitId,
                    this.searchBankAccountId,
                    this.searchDealId,
                    this.searchHedgeDealId,
                    this.localTradeDateFrom,
                    this.localTradeDateTo,
                    this.searchInstrumentId,
                    pageIndex,
                    pageSize,
                    out this.pageCount);

            this.NotifyOfPropertyChange(() => this.PageCount);
            if (pageIndex == 1)
            {
                if (string.IsNullOrEmpty(this.PageCommand) == false)
                {
                    this.PageCommand = string.Format("Reload,{0},{1}", pageIndex, this.PageCount);
                }
                else
                {
                    this.PageCommand = string.Format("Init,{0},{1}", pageIndex, this.PageCount);
                }
            }

            reslut.ForEach(this.actualCashList.Add);
        }

        /// <summary>
        /// 分页的DataGrid页数变更事件
        /// </summary>
        /// <param name="args">
        /// 变更事件参数
        /// </param>
        public void DateGridPageChanged(PagingChangedEventArgs args)
        {
            this.Search(args.PageIndex, args.PageSize);
        }
        #endregion

        #region Methods

        /// <summary>
        /// The filter.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool Filter(ActualCashModel model)
        {
            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && model.BusinessUnitId != this.SearchBusinessUnitId)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchBankAccountId) && model.BankAccountId != this.SearchBankAccountId)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchDealId) && !model.Id.Contains(this.SearchDealId))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchHedgeDealId) && !model.HedgeDealId.Contains(this.SearchHedgeDealId))
            {
                return false;
            }

            if (this.LocalTradeDateFrom.HasValue && model.LocalTradeDate < this.LocalTradeDateFrom.Value)
            {
                return false;
            }

            if (this.LocalTradeDateTo.HasValue && model.LocalTradeDate > this.LocalTradeDateTo.Value)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchInstrumentId) && model.InstrumentId != this.SearchInstrumentId)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}