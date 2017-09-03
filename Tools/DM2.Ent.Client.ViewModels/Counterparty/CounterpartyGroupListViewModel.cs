// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterpartyGroupListViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Counterparty
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The counterparty group list view model.
    /// </summary>
    public class CounterpartyGroupListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        /// The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The counterparty group repository.
        /// </summary>
        private readonly ICounterPartyGroupRepository counterpartyGroupRepository;

        private ObservableCollection<BusinessUnitModel> businessUnits;

        /// <summary>
        /// The counterparty group list.
        /// </summary>
        private ObservableCollection<CounterPartyGroupModel> counterpartyGroupList;

        /// <summary>
        /// The search business unit id.
        /// </summary>
        private string searchBusinessUnitId;

        /// <summary>
        /// The search group name.
        /// </summary>
        private string searchGroupName;

        /// <summary>
        /// The selected counterparty group.
        /// </summary>
        private CounterPartyGroupModel selectedCounterpartyGroup;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartyGroupListViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public CounterpartyGroupListViewModel(string ownerId)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("CounterpartyGroupListTitle");

            IBusinessUnitRepository businessUnitRepository = this.GetRepository<IBusinessUnitRepository>();
            this.BusinessUnits = businessUnitRepository.GetBindCollection().ToComboboxBinding(true);

            this.counterpartyGroupRepository = this.GetRepository<ICounterPartyGroupRepository>();
            this.Search();

            //businessUnitRepository.SubscribeAddEvent(
            //    model => this.businessUnits.Insert(1, model));
            //businessUnitRepository.SubscribeUpdateEvent(
            //    (oldModel, newModel) =>
            //    {
            //        var item = this.businessUnits.FirstOrDefault(m => m.Id == oldModel.Id);
            //        if (item != null)
            //        {
            //            newModel.Copy(item);
            //        }
            //    });
            //businessUnitRepository.SubscribeRemoveEvent(
            //    model =>
            //    {
            //        var removedItem = this.businessUnits.FirstOrDefault(m => m.Id == model.Id);
            //        if (removedItem != null)
            //        {
            //            this.businessUnits.Remove(removedItem);
            //        }
            //    });

            this.counterpartyGroupRepository.SubscribeAddEvent(
                model => this.counterpartyGroupList.Insert(0, model));
            this.counterpartyGroupRepository.SubscribeUpdateEvent(
                (oldModel, newModel) =>
                {
                    var item = this.counterpartyGroupList.FirstOrDefault(m => m.Id == oldModel.Id);
                    if (item != null)
                    {
                        newModel.Copy(item);
                    }
                });
            this.counterpartyGroupRepository.SubscribeRemoveEvent(
                model =>
                {
                    var removedItem = this.counterpartyGroupList.FirstOrDefault(m => m.Id == model.Id);
                    if (removedItem != null)
                    {
                        this.counterpartyGroupList.Remove(removedItem);
                    }
                });
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
        /// Gets or sets the counterparty group list.
        /// </summary>
        public ObservableCollection<CounterPartyGroupModel> CounterpartyGroupList
        {
            get
            {
                return this.counterpartyGroupList;
            }

            set
            {
                this.counterpartyGroupList = value;
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
        /// Gets or sets the search group name.
        /// </summary>
        public string SearchGroupName
        {
            get
            {
                return this.searchGroupName;
            }

            set
            {
                this.searchGroupName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected counter party group.
        /// </summary>
        public CounterPartyGroupModel SelectedCounterpartyGroup
        {
            get
            {
                return this.selectedCounterpartyGroup;
            }

            set
            {
                this.selectedCounterpartyGroup = value;
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
        /// The edit.
        /// </summary>
        public void Edit()
        {
            if (this.selectedCounterpartyGroup == null)
            {
                return;
            }

            var vm = new ModifyCounterpartyGroupViewModel(this.OwnerId, this.selectedCounterpartyGroup);
            vm.DisplayName = string.Format("{0}{1}",
                RunTime.FindStringResource("Modify"),
                RunTime.FindStringResource("CounterpartyGroup"));

            this.windowManager.ShowWindow(vm);
        }

        /// <summary>
        /// The new.
        /// </summary>
        public void New()
        {
            var vm = new NewCounterpartyGroupViewModel(this.OwnerId);
            vm.DisplayName = string.Format("{0}{1}",
                RunTime.FindStringResource("New"),
                RunTime.FindStringResource("CounterpartyGroup"));

            this.windowManager.ShowWindow(vm);
        }
       

        /// <summary>
        /// The search.
        /// </summary>
        public void Search()
        {
            this.CounterpartyGroupList =
                this.counterpartyGroupRepository.Filter(this.Filter).OrderBy(cpg => cpg.Name).ToObservableCollection();
        }

        #endregion


        private bool Filter(CounterPartyGroupModel model)
        {
            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && model.BusinessUnitId != this.SearchBusinessUnitId)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchGroupName)
                && model.Name.IndexOf(this.SearchGroupName, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            return true;
        }
    }
}