// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryInternalDealListViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 03:23:23 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 03:23:23
//      修改描述：新建 HistoryInternalDealListViewModel.cs
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
    using System.Threading.Tasks;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Infrastructure.Common.Enums;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The deal list view model.
    /// </summary>
    public class HistoryInternalDealListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The bu rep.
        /// </summary>
        private readonly IBusinessUnitRepository buRep;

        /// <summary>
        ///     商品仓储
        /// </summary>
        private readonly IContractRepository contractRep;

        /// <summary>
        ///     The counter party rep.
        /// </summary>
        private readonly ICounterPartyRepository counterPartyRep;

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IFxInternalDealRepository dealReps;

        /// <summary>
        ///     DealService
        /// </summary>
        private readonly InternalDealService dealService;

        /// <summary>
        /// The deal item.
        /// </summary>
        private FxInternalDealModel dealItem;

        /// <summary>
        ///     运行时
        /// </summary>
        private readonly RunTime runTime;

        /// <summary>
        ///     The user bu model.
        /// </summary>
        private readonly BusinessUnitModel userBuModel;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

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
        ///     symbolList
        /// </summary>
        private ObservableCollection<ContractModel> contractList;

        /// <summary>
        ///     The hedge account.
        /// </summary>
        private CounterPartyModel counterParty;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterPartys;

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
        ///     查询DealID条件
        /// </summary>
        private string dealId;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<FxInternalDealModel> dealList;

        /// <summary>
        ///     The external deal set id.
        /// </summary>
        private string externalDealSetId;

        /// <summary>
        ///     字段 货币
        /// </summary>
        private ContractModel selectedContract;

        /// <summary>
        ///     被选中Deal
        /// </summary>
        private FxInternalDealModel selectedDeal;

        /// <summary>
        ///     查询OpenTime条件
        /// </summary>
        private DateTime? tradeFrom;

        /// <summary>
        ///     查询OpenTimeTo到期时间条件
        /// </summary>
        private DateTime? tradeTo;

        /// <summary>
        ///     查询ValueDate条件
        /// </summary>
        private DateTime? valueDateFrom;

        /// <summary>
        ///     查询ValueDateTo到期时间条件
        /// </summary>
        private DateTime? valueDateTo;

        private EnterpriseModel currentEnterprise;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryInternalDealListViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public HistoryInternalDealListViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("HedgeDealList");
            if (RunTime.GetCurrentRunTime().CurrentLoginUser == null)
            {
                return;
            }

            this.contractRep = this.GetRepository<IContractRepository>();
            this.buRep = this.GetRepository<IBusinessUnitRepository>();
            this.dealReps = this.GetRepository<IFxInternalDealRepository>();
            this.counterPartyRep = this.GetRepository<ICounterPartyRepository>();
            this.currentEnterprise =
                this.GetRepository<IEnterpriseRepository>().FindByID(RunTime.GetCurrentRunTime().CurrentLoginUser.EntId);
            this.runTime = RunTime.GetCurrentRunTime(varOwnerId);
            this.dealService = this.GetSevice<InternalDealService>();
            this.userBuModel = this.buRep.FindByID(RunTime.GetCurrentRunTime().CurrentLoginUser.BuId);
            this.DealList = new ObservableCollection<FxInternalDealModel>();
            Task.Factory.StartNew(RunTime.GetCurrentRunTime().CurrentRepositoryCore.WaitAllInitial)
                .ContinueWith(this.WaitLoad);
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the deal item.
        /// </summary>
        public FxInternalDealModel DealItem
        {
            get
            {
                return this.dealItem;
            }

            set
            {
                this.dealItem = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CounterParty 选中事件
        /// </summary>
        public RelayCommand BuSelectionChangedCommand { get; private set; }

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
        public ObservableCollection<ContractModel> ContractList
        {
            get
            {
                return this.contractList;
            }

            set
            {
                this.contractList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     HedgeAccount实体
        /// </summary>
        public CounterPartyModel CounterParty
        {
            get
            {
                return this.counterParty;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.counterParty = null;
                    this.NotifyOfPropertyChange(() => this.CounterParty);
                    return;
                }

                this.counterParty = value;
                this.NotifyOfPropertyChange(() => this.CounterParty);
            }
        }

        /// <summary>
        ///     对冲账户列表
        /// </summary>
        public ObservableCollection<CounterPartyModel> CounterPartys
        {
            get
            {
                return this.counterPartys;
            }

            set
            {
                this.counterPartys = value;
                this.NotifyOfPropertyChange(() => this.CounterPartys);
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
        ///     交易单列表
        /// </summary>
        public ObservableCollection<FxInternalDealModel> DealList
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
        ///     属性 货币
        /// </summary>
        public ContractModel SelectedContract
        {
            get
            {
                return this.selectedContract;
            }

            set
            {
                this.selectedContract = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     selectDealVm FxHedgingDealModel实体
        /// </summary>
        public FxInternalDealModel SelectedDeal
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

        /// <summary>
        ///     查询ValueDate条件
        /// </summary>
        public DateTime? ValueDateFrom
        {
            get
            {
                return this.valueDateFrom;
            }

            set
            {
                this.valueDateFrom = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     查询ValueDateTo到期时间条件
        /// </summary>
        public DateTime? ValueDateTo
        {
            get
            {
                return this.valueDateTo;
            }

            set
            {
                this.valueDateTo = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The modify deal_ double click.
        /// </summary>
        public void ModifyDeal_DoubleClick()
        {
            if (this.DealItem != null)
            {
                //var vm = new ModifyHedgeSpotForwardViewModel(this.DealItem);
                //this.windowManager.ShowWindow(vm);
            }
        }

        /// <summary>
        ///     The new_ click.
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
            var deals =
                this.dealService.FindHistory(
                    this.selectedContract == null ? string.Empty : this.selectedContract.Id,
                    this.currentEnterprise.Id,
                    this.BusinessUnit == null ? string.Empty : this.BusinessUnit.Id,
                    this.CounterParty == null ? string.Empty : this.CounterParty.Id,
                    this.DealId,
                    this.TradeFrom,
                    this.TradeTo,
                    this.ValueDateFrom,
                    this.ValueDateTo).OrderByDescending(p => p.LocalTradeDate.Date).ThenByDescending(o => o.Id).ToList();
            RunTime.GetCurrentRunTime().ActionOnUiThread(
                () =>
                {
                    if (this.DealList.Count != 0)
                    {
                        this.DealList.Clear();
                    }

                    deals.ForEach(s => this.DealList.Add(s));
                });
        }

        #endregion

        #region Methods

        /// <summary>
        /// The bind combobox.
        /// </summary>
        private void BindCombobox()
        {
            this.BusinessUnitList = this.buRep.GetBindCollection().ToComboboxBinding(true);
            this.CounterPartys =
                this.GetRepository<ICounterPartyRepository>().GetBindCollection().ToComboboxBinding(true);
            this.ContractList = this.contractRep.GetBindCollection().ToComboboxBinding(true);
            this.BuSelectionChangedCommand = new RelayCommand(this.BuSelectionChanged);
        }

        /// <summary>
        ///     下拉联动事件
        /// </summary>
        private void BuSelectionChanged()
        {
            if (this.CounterPartys.Any())
            {
                this.CounterPartys.Clear();
            }

            this.CounterPartys = this.BusinessUnit == null
                                     ? this.counterPartyRep.GetBindCollection().ToComboboxBinding(true)
                                     : this.counterPartyRep.Filter(o => o.BusinessUnitId == this.BusinessUnit.Id)
                                           .ToComboboxBinding(true);
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        /// <param name="changedDeal">
        /// The changed deal.
        /// </param>
        private void Refresh(FxHedgingDealModel changedDeal)
        {
            if (this.dealList == null || changedDeal == null)
            {
            }
        }

        /// <summary>
        /// The wait load.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        private void WaitLoad(Task task)
        {
            this.BindCombobox();
            this.dealReps.SubscribeAddEvent(model => this.Search());
            this.dealReps.SubscribeUpdateEvent((oldModel, newModel) => this.Search());
            this.dealReps.SubscribeRemoveEvent(model => this.Search());
        }

        #endregion
    }
}