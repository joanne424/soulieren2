// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalDealListToolViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 05:10:14 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 05:10:14
//      修改描述：新建 InternalDealListToolViewModel.cs
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

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The deal list tool view model.
    /// </summary>
    public class InternalDealListToolViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IFxInternalDealRepository dealReps;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The deal item.
        /// </summary>
        private FxInternalDealModel dealItem;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<FxInternalDealModel> dealList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalDealListToolViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// The var owner id.
        /// </param>
        public InternalDealListToolViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("CurrentInternalDealList");
            this.dealReps = this.GetRepository<IFxInternalDealRepository>();
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

        #endregion

        #region Methods

        /// <summary>
        /// The load.
        /// </summary>
        private void Load()
        {
            RunTime.GetCurrentRunTime()
                .ActionOnUiThread(
                    () =>
                    {
                        this.DealList =
                            this.dealReps.GetBindCollection()
                                .OrderByDescending(o => o.LocalTradeDate)
                                .OrderByDescending(o => o.Id)
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
            this.dealReps.SubscribeAddEvent(model => { this.Load(); });
            this.dealReps.SubscribeUpdateEvent((oldModel, newModel) => { this.Load(); });
            this.dealReps.SubscribeRemoveEvent(model => { this.Load(); });
        }

        #endregion
    }
}