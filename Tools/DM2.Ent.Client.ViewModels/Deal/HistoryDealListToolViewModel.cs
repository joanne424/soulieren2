// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryDealListToolViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 05:10:14 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 05:10:14
//      修改描述：新建 HistoryDealListToolViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
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

    using Infrastructure.Common.Enums;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The deal list tool view model.
    /// </summary>
    public class HistoryDealListToolViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IFxHedgingDealRepository dealReps;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The deal item.
        /// </summary>
        private FxHedgingDealModel dealItem;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<FxHedgingDealModel> dealList;

        /// <summary>
        ///     运行时
        /// </summary>
        private readonly RunTime runTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryDealListToolViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// The var owner id.
        /// </param>
        public HistoryDealListToolViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.runTime = RunTime.GetCurrentRunTime(varOwnerId);

            this.DisplayName = RunTime.FindStringResource("HedgeDealList");
            this.dealReps = this.GetRepository<IFxHedgingDealRepository>();
            Task.Factory.StartNew(runTime.CurrentRepositoryCore.WaitAllInitial)
                .ContinueWith(this.WaitLoad);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the deal item.
        /// </summary>
        public FxHedgingDealModel DealItem
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
        ///     交易单列表
        /// </summary>
        public ObservableCollection<FxHedgingDealModel> DealList
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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The modify deal_ double click.
        /// </summary>
        public void ModifyDeal_DoubleClick()
        {
            if (this.DealItem != null)
            {
                string spotInstrumentId = string.Empty;
                string forwardInstrumentId = string.Empty;
                var spotInstrument = InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_HEDGING_SWAP_FORWARD);
                var forwordInstrument = InstrumentTool.GetInstruments(TradableInstrumentEnum.FX_HEDGING_SWAP_FORWARD);
                if (spotInstrument != null && spotInstrument.Any())
                {
                    spotInstrumentId = spotInstrument.Keys.First();
                }

                if (forwordInstrument != null && forwordInstrument.Any())
                {
                    forwardInstrumentId = forwordInstrument.Keys.First();
                }

                // swap
                if (this.DealItem.Instrument == spotInstrumentId || this.DealItem.Instrument == forwardInstrumentId)
                {
                    var vm = new ModifyHedgeSwapViewModel(this.DealItem);
                    this.windowManager.ShowWindow(vm);
                }
                else
                {
                    var vm = new ModifyHedgeSpotForwardViewModel(this.DealItem);
                    this.windowManager.ShowWindow(vm);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The load.
        /// </summary>
        private void Load()
        {
            var tempDeals = this.GetSevice<HedgingDealService>().Search(runTime.CurrentLoginUser.EntId);
            RunTime.GetCurrentRunTime()
                .ActionOnUiThread(
                    () =>
                    {
                        this.DealList = tempDeals
                                .OrderByDescending(o => o.LocalTradeDate)
                                .ThenByDescending(o => o.Id)
                                .ToObservableCollection();
                    });
        }

        /// <summary>
        /// The wait load.
        /// </summary>
        /// <param name="lastTask">
        /// The last task.
        /// </param>
        private void WaitLoad(Task lastTask)
        {
            this.Load();
            this.dealReps.SubscribeAddEvent(model => this.Load());
            this.dealReps.SubscribeUpdateEvent((oldModel, newModel) => this.Load());
            this.dealReps.SubscribeRemoveEvent(model => this.Load());
        }

        #endregion
    }
}