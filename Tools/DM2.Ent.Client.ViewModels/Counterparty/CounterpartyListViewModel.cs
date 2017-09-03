

namespace DM2.Ent.Client.ViewModels.Counterparty
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// The counterparty list view model.
    /// </summary>
    public class CounterpartyListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        /// The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The counterparty list.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterpartyList;

        private ObservableCollection<BusinessUnitModel> businessUnits;

        /// <summary>
        /// The keyword.
        /// </summary>
        private string searchName;

        /// <summary>
        /// The selected business unit id.
        /// </summary>
        private string searchBusinessUnitId;

        /// <summary>
        /// The selected counterparty.
        /// </summary>
        private CounterPartyModel selectedCounterparty;

        private ICounterPartyRepository counterpartyRepository;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartyListViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public CounterpartyListViewModel(string ownerId)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("CounterpartyListTitle");

            IBusinessUnitRepository businessUnitRepository = this.GetRepository<IBusinessUnitRepository>();

            this.BusinessUnits = businessUnitRepository.GetBindCollection().ToComboboxBinding(true);

            this.counterpartyRepository = this.GetRepository<ICounterPartyRepository>();
            this.Search();

            this.counterpartyRepository.SubscribeAddEvent(
                model =>
                {
                    this.counterpartyList.Insert(0, model);
                });
            this.counterpartyRepository.SubscribeUpdateEvent(
                (oldModel, newModel) =>
                {
                    var item = this.counterpartyList.FirstOrDefault(m => m.Id == oldModel.Id);
                    if (item != null)
                    {
                        newModel.Copy(item);
                    }
                });
            this.counterpartyRepository.SubscribeRemoveEvent(
                model =>
                {
                    var removedItem = this.counterpartyList.FirstOrDefault(m => m.Id == model.Id);
                    if (removedItem != null)
                    {
                        this.counterpartyList.Remove(removedItem);
                    }
                });
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     BU列表
        /// </summary>
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
        ///     交易对手列表
        /// </summary>
        public ObservableCollection<CounterPartyModel> CounterpartyList
        {
            get
            {
                return this.counterpartyList;
            }

            set
            {
                this.counterpartyList = value;
                this.NotifyOfPropertyChange();
            }
        }
        
        /// <summary>
        ///     填写的交易对手
        /// </summary>
        public string SearchName
        {
            get
            {
                return this.searchName;
            }

            set
            {
                this.searchName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     选中的BuID
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
        ///     选中的交易对手
        /// </summary>
        public CounterPartyModel SelectedCounterparty
        {
            get
            {
                return this.selectedCounterparty;
            }

            set
            {
                this.selectedCounterparty = value;
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
        /// The edit.
        /// </summary>
        public void Edit()
        {
            if (this.selectedCounterparty == null)
            {
                return;
            }

            var vm = new CounterpartyEditViewModel(this.OwnerId, this.selectedCounterparty);
            vm.DisplayName = string.Format("{0}{1}",
                RunTime.FindStringResource("Modify"),
                RunTime.FindStringResource("Counterparty"));

            this.windowManager.ShowWindow(vm);
        }

        /// <summary>
        /// The new.
        /// </summary>
        public void New()
        {
            var vm = new CounterpartyAddViewModel(this.OwnerId);
            vm.DisplayName = string.Format("{0}{1}",
                RunTime.FindStringResource("New"),
                RunTime.FindStringResource("Counterparty"));

            this.windowManager.ShowWindow(vm);
        }

        /// <summary>
        /// The search.
        /// </summary>
        public void Search()
        {
            this.CounterpartyList =
                this.counterpartyRepository.Filter(this.Filter)
                    .OrderBy(cp => cp.BusinessUnitId)
                    .ThenBy(cp => cp.Name)
                    .ToObservableCollection();
        }

        #endregion

        private bool Filter(CounterPartyModel model)
        {
            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && model.BusinessUnitId != this.SearchBusinessUnitId)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchName)
                && model.Name.IndexOf(this.SearchName, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            return true;
        }
    }
}