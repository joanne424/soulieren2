// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DealListToolViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 05:10:14 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 05:10:14
//      修改描述：新建 DealListToolViewModel.cs
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

    using Caliburn.Micro;

    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models.Base;

    using Infrastracture.Data;

    using SOU.Model;

    /// <summary>
    ///     The deal list tool view model.
    /// </summary>
    public class CustomerListToolViewModel : BaseVm
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomerListToolViewModel" /> class.
        /// </summary>
        /// <param name="varOwnerId">
        ///     The var owner id.
        /// </param>
        public CustomerListToolViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = "虚拟客户列表";
            this.windowManager = new WindowManager();
            var rep = new CustomerRepository();
            this.Customers = rep.Roots().ToObservableCollection();

            //this.dealReps = this.GetRepository<IFxHedgingDealRepository>();
            //Task.Factory.StartNew(RunTime.GetCurrentRunTime().CurrentRepositoryCore.WaitAllInitial)
            //    .ContinueWith(this.WaitLoad);
        }

        #endregion

        #region Public Methods and Operators

        public void NewCustomerCommand()
        {
            var custVM = new NewCustomerViewModel();
            this.windowManager.ShowWindow(custVM);
        }

        /*
        /// <summary>
        /// The modify deal_ double click.
        /// </summary>
        public void ModifyDeal_DoubleClick()
        {
            if (this.DealItem != null)
            {
                // swap
                if (this.DealItem.IsNearLeg == (int)IsNearLegEnum.FAR_LEG || this.DealItem.IsNearLeg == (int)IsNearLegEnum.NEAR_LEG)
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
        */

        #endregion

        #region Fields

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager;

        private ObservableCollection<Customer> customers;

        #endregion

        #region Public Properties

        public ObservableCollection<Customer> Customers
        {
            get
            {
                return this.customers;
            }

            set
            {
                this.customers = value;
                this.NotifyOfPropertyChange();
            }
        }

        private List<Customer> selectedCustomers;

        private List<Customer> SelectedCustomers
        {
            get
            {
                return this.selectedCustomers;
            }

            set
            {
                this.selectedCustomers = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion
    }
}