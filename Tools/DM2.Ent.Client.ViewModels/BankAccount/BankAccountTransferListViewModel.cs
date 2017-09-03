// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankAccountTransferListViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 03:23:23 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 03:23:23
//      修改描述：新建 BankAccountTransferListViewModel.cs
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
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The deal list view model.
    /// </summary>
    public class BankAccountTransferListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IFxHedgingDealRepository bankCashTransferReps;

        /// <summary>
        ///     The bu rep.
        /// </summary>
        private readonly IBusinessUnitRepository buRep;

        /// <summary>
        ///     The counter party rep.
        /// </summary>
        private readonly ICounterPartyRepository counterPartyRep;

        /// <summary>
        ///     The ent id.
        /// </summary>
        private readonly string entId;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The current search page index.
        /// </summary>
        private int currentSearchPageIndex = 1;

        /// <summary>
        ///     The current search page size.
        /// </summary>
        private int currentSearchPageSize = 15;

        /// <summary>
        ///     查询DealID条件
        /// </summary>
        private string dealId;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<BankCashTransferModel> dealList;

        /// <summary>
        ///     The from bank acct model.
        /// </summary>
        private BankAccountModel fromBankAcctModel;

        /// <summary>
        ///     The business unit.
        /// </summary>
        private BusinessUnitModel fromBusinessUnit;

        /// <summary>
        ///     The business unit list.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> fromBusinessUnitList;

        /// <summary>
        ///     The hedging deal id.
        /// </summary>
        private string hedgingDealId;

        /// <summary>
        ///     分页控件的命令处理属性
        /// </summary>
        private string pageCommand;

        /// <summary>
        ///     查询订单数据的总页数
        /// </summary>
        private int pageCount;

        /// <summary>
        ///     被选中Deal
        /// </summary>
        private BankCashTransferModel selectedDeal;

        /// <summary>
        ///     The to bank acct model.
        /// </summary>
        private BankAccountModel toBankAcctModel;

        /// <summary>
        ///     The business unit.
        /// </summary>
        private BusinessUnitModel toBusinessUnit;

        /// <summary>
        ///     The business unit list.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> toBusinessUnitList;

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
        /// Initializes a new instance of the <see cref="BankAccountTransferListViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public BankAccountTransferListViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("BankCashTransferReport");
            if (RunTime.GetCurrentRunTime().CurrentLoginUser == null)
            {
                return;
            }

            this.buRep = this.GetRepository<IBusinessUnitRepository>();
            this.counterPartyRep = this.GetRepository<ICounterPartyRepository>();
            this.FromBusinessUnitList = this.buRep.GetBindCollection().ToComboboxBinding(true);
            this.ToBusinessUnitList = this.buRep.GetBindCollection().ToComboboxBinding(true);
            this.BankCashTransferList = new ObservableCollection<BankCashTransferModel>();
            this.entId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     交易单列表
        /// </summary>
        public ObservableCollection<BankCashTransferModel> BankCashTransferList
        {
            get
            {
                return this.dealList;
            }

            set
            {
                this.dealList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     查询DealID条件
        /// </summary>
        public string DealId
        {
            get
            {
                return this.dealId;
            }

            set
            {
                this.dealId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the from bank acct model.
        /// </summary>
        public BankAccountModel FromBankAcctModel
        {
            get
            {
                return this.fromBankAcctModel;
            }

            set
            {
                this.fromBankAcctModel = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the business unit.
        /// </summary>
        public BusinessUnitModel FromBusinessUnit
        {
            get
            {
                return this.fromBusinessUnit;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.fromBusinessUnit = null;
                    this.NotifyOfPropertyChange();
                    return;
                }

                this.fromBusinessUnit = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     symbolList 货币对列表
        /// </summary>
        public ObservableCollection<BusinessUnitModel> FromBusinessUnitList
        {
            get
            {
                return this.fromBusinessUnitList;
            }

            set
            {
                this.fromBusinessUnitList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the hedging deal id.
        /// </summary>
        public string HedgingDealId
        {
            get
            {
                return this.hedgingDealId;
            }

            set
            {
                this.hedgingDealId = value;
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
        ///     selectDealVm FxHedgingDealModel实体
        /// </summary>
        public BankCashTransferModel SelectedDeal
        {
            get
            {
                return this.selectedDeal;
            }

            set
            {
                this.selectedDeal = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the to bank acct model.
        /// </summary>
        public BankAccountModel ToBankAcctModel
        {
            get
            {
                return this.toBankAcctModel;
            }

            set
            {
                this.toBankAcctModel = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the business unit.
        /// </summary>
        public BusinessUnitModel ToBusinessUnit
        {
            get
            {
                return this.toBusinessUnit;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.toBusinessUnit = null;
                    this.NotifyOfPropertyChange();
                    return;
                }

                this.toBusinessUnit = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     symbolList 货币对列表
        /// </summary>
        public ObservableCollection<BusinessUnitModel> ToBusinessUnitList
        {
            get
            {
                return this.toBusinessUnitList;
            }

            set
            {
                this.toBusinessUnitList = value;
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
        /// The clear from bank account.
        /// </summary>
        public void ClearFromBankAccount()
        {
            if (this.FromBankAcctModel == null)
            {
                return;
            }

            this.FromBankAcctModel.AccountNo = string.Empty;
            this.FromBankAcctModel = null;
        }

        /// <summary>
        /// The clear to bank account.
        /// </summary>
        public void ClearToBankAccount()
        {
            if (this.ToBankAcctModel == null)
            {
                return;
            }

            this.ToBankAcctModel.AccountNo = string.Empty;
            this.ToBankAcctModel = null;
        }

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
        ///     双击事件 dategrid1
        /// </summary>
        public void DoubleClicked()
        {
            if (this.SelectedDeal == null)
            {
                return;
            }

            var vm = new ModifyBankAccountTransferViewModel(this.SelectedDeal);
            this.windowManager.ShowWindow(vm);
        }

        /// <summary>
        ///     The find from acct bnt.
        /// </summary>
        public void FindFromAcctBnt()
        {
            var findBankAcct = new FindBankAcctViewModel();
            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.FromBankAcctModel = findBankAcct.BankAccount.Clone();
            }
        }

        /// <summary>
        ///     The find to acct bnt.
        /// </summary>
        public void FindToAcctBnt()
        {
            var findBankAcct = new FindBankAcctViewModel();
            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.ToBankAcctModel = findBankAcct.BankAccount.Clone();
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
            if (this.BankCashTransferList.Any())
            {
                this.BankCashTransferList.Clear();
            }

            this.currentSearchPageIndex = pageIndex;
            this.currentSearchPageSize = pageSize;
            int count;

            string fromBuid = string.Empty;
            string toBuid = string.Empty;
            string fromAcctId = string.Empty;
            string toAcctId = string.Empty;

            if (this.FromBusinessUnit != null)
            {
                fromBuid = this.FromBusinessUnit.Id;
            }

            if (this.ToBusinessUnit != null)
            {
                toBuid = this.ToBusinessUnit.Id;
            }

            if (this.FromBankAcctModel != null)
            {
                fromAcctId = this.FromBankAcctModel.Id;
            }

            if (this.ToBankAcctModel != null)
            {
                toAcctId = this.ToBankAcctModel.Id;
            }

            IList<BankCashTransferModel> reslut = this.GetSevice<BankCashTransferService>()
                .Search(
                    this.DealId, 
                    this.HedgingDealId, 
                    this.entId, 
                    fromBuid, 
                    toBuid, 
                    fromAcctId, 
                    toAcctId, 
                    this.TradeFrom, 
                    this.TradeTo, 
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

            foreach (
                BankCashTransferModel bankCashTransferModel in
                    reslut.OrderByDescending(o => o.LocalTradeDate).ThenByDescending(o => o.Id))
            {
                this.BankCashTransferList.Add(bankCashTransferModel);
            }
        }

        #endregion
    }
}