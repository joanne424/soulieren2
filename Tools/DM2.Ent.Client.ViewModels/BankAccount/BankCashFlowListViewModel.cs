// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankCashFlowListViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 03:23:23 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 03:23:23
//      修改描述：新建 BankCashFlowListViewModel.cs
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
    using System.Linq;

    using Banclogix.Controls.PagedDataGrid;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The deal list view model.
    /// </summary>
    public class BankCashFlowListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The from bank acct model.
        /// </summary>
        private BankAccountModel bankAcctModel;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<BankCashFlowModel> bankCashFlowList;

        /// <summary>
        /// The bank cash flow list data context.
        /// </summary>
        private BankCashFlowListViewModel bankCashFlowListDataContext;

        /// <summary>
        ///     The current search page index.
        /// </summary>
        private int currentSearchPageIndex = 1;

        /// <summary>
        ///     The current search page size.
        /// </summary>
        private int currentSearchPageSize = 15;

        /// <summary>
        ///     分页控件的命令处理属性
        /// </summary>
        private string pageCommand;

        /// <summary>
        ///     查询订单数据的总页数
        /// </summary>
        private int pageCount;

        /// <summary>
        ///     查询OpenTime条件
        /// </summary>
        private DateTime? tradeFrom;

        /// <summary>
        ///     查询OpenTimeTo到期时间条件
        /// </summary>
        private DateTime? tradeTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BankCashFlowListViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public BankCashFlowListViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("BankCashFlowList");
            this.BankCashFlowList = new ObservableCollection<BankCashFlowModel>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the from bank acct model.
        /// </summary>
        public BankAccountModel BankAcctModel
        {
            get
            {
                return this.bankAcctModel;
            }

            set
            {
                this.bankAcctModel = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     交易单列表
        /// </summary>
        public ObservableCollection<BankCashFlowModel> BankCashFlowList
        {
            get
            {
                return this.bankCashFlowList;
            }

            set
            {
                this.bankCashFlowList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the bankank cash flow list data context.
        /// </summary>
        public BankCashFlowListViewModel BankankCashFlowListDataContext
        {
            get
            {
                return this.bankCashFlowListDataContext;
            }

            set
            {
                this.bankCashFlowListDataContext = value;
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
        ///     查询OpenTime条件
        /// </summary>
        public DateTime? TradeFrom
        {
            get
            {
                return this.tradeFrom;
            }

            set
            {
                this.tradeFrom = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     查询OpenTimeTo到期时间条件
        /// </summary>
        public DateTime? TradeTo
        {
            get
            {
                return this.tradeTo;
            }

            set
            {
                this.tradeTo = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The new_ click.
        /// </summary>
        public void Close()
        {
            this.TryClose();
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

        /// <summary>
        ///     The dispose.
        /// </summary>
        public override void Dispose()
        {
            Messenger.Default.Unregister<string>(this, "UpdateLanguage");
        }

        /// <summary>
        ///     The find from acct bnt.
        /// </summary>
        public void FindAcctBnt()
        {
            var findBankAcct = new FindBankAcctViewModel();
            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.BankAcctModel = findBankAcct.BankAccount;
            }
        }

        /// <summary>
        /// Search(查询)按钮点击事件
        /// </summary>
        /// <param name="pageIndex">
        /// The page Index.
        /// </param>
        /// <param name="pageSize">
        /// The page Size.
        /// </param>
        public void Search(int pageIndex, int pageSize)
        {
            if (this.BankAcctModel == null)
            {
                return;
            }

            if (this.BankCashFlowList.Any())
            {
                this.BankCashFlowList.Clear();
            }

            this.currentSearchPageIndex = pageIndex;
            this.currentSearchPageSize = pageSize;
            int count;

            DateTime? tempFrom = this.TradeFrom == null
                                     ? this.TradeFrom
                                     : RunTime.GetCurrentRunTime().GetGmtTimeFromCurrentUserBu(this.TradeFrom.Value);
            DateTime? tempTo = this.TradeTo == null
                                   ? this.TradeTo
                                   : RunTime.GetCurrentRunTime()
                                         .GetGmtTimeFromCurrentUserBu(this.TradeTo.Value.AddDays(1));

            IList<BankCashFlowModel> reslut =
                this.GetSevice<BankCashFlowService>()
                    .Search(
                        RunTime.GetCurrentRunTime().CurrentLoginUser.EntId,
                        this.BankAcctModel.Id,
                        tempFrom,
                        tempTo,
                        pageIndex,
                        pageSize,
                        out count);

            this.PageCount = count;
            if (pageIndex == 1)
            {
                if (string.IsNullOrEmpty(this.PageCommand) == false)
                {
                    this.PageCommand = string.Empty;
                    this.PageCommand = string.Format("Reload,{0},{1}", pageIndex, this.PageCount);
                }
                else
                {
                    this.PageCommand = string.Format("Init,{0},{1}", pageIndex, this.PageCount);
                }
            }

            foreach (BankCashFlowModel bankCashTransferModel in reslut.ToList().OrderByDescending(o => o.Id))
            {
                this.BankCashFlowList.Add(bankCashTransferModel);
            }

            this.BankankCashFlowListDataContext = this;
        }

        #endregion
    }
}