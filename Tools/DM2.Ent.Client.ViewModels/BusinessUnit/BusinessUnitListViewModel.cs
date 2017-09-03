// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessUnitListViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.BusinessUnit
{
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
    /// The business unit list view model.
    /// </summary>
    public class BusinessUnitListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        /// The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The business unit list.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnitList;

        /// <summary>
        /// The selected business unit.
        /// </summary>
        private BusinessUnitModel selectedBusinessUnit;

        /// <summary>
        /// The selected business unit id.
        /// </summary>
        private string searchBusinessUnitId;


        private ObservableCollection<BusinessUnitModel> businessUnits;

        private IBusinessUnitRepository businessUnitRepository;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitListViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public BusinessUnitListViewModel(string ownerId)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("BusinessUnitListTitle");

            this.businessUnitRepository = this.GetRepository<IBusinessUnitRepository>();

            this.BusinessUnits =
                this.businessUnitRepository.GetBindCollection().ToComboboxBinding(true);

            this.Search();

            this.businessUnitRepository.SubscribeAddEvent(
                model =>
                    {
                        this.businessUnits.Insert(1, model);
                        this.businessUnitList.Insert(0, model);
                    });
            this.businessUnitRepository.SubscribeUpdateEvent(
                (oldModel, newModel) =>
                    {
                        var item = this.businessUnits.FirstOrDefault(m => m.Id == oldModel.Id);
                        if (item != null)
                        {
                            newModel.Copy(item);
                        }

                        item = this.businessUnitList.FirstOrDefault(m => m.Id == oldModel.Id);
                        if (item != null)
                        {
                            newModel.Copy(item);
                        }
                    });
            this.businessUnitRepository.SubscribeRemoveEvent(
                model =>
                    {
                        var removedItem = this.businessUnits.FirstOrDefault(m => m.Id == model.Id);
                        if (removedItem != null)
                        {
                            this.businessUnits.Remove(removedItem);
                        }

                        removedItem = this.businessUnitList.FirstOrDefault(m => m.Id == model.Id);
                        if (removedItem != null)
                        {
                            this.businessUnitList.Remove(removedItem);
                        }
                    });
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the business unit list.
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
        /// Gets or sets the business units.
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
        /// Gets or sets the selected business unit.
        /// </summary>
        public BusinessUnitModel SelectedBusinessUnit
        {
            get
            {
                return this.selectedBusinessUnit;
            }

            set
            {
                this.selectedBusinessUnit = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected business unit id.
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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     The edit.
        /// </summary>
        public void Edit()
        {
            var vm = new BusinessUnitEditViewModel(this.OwnerId, this.SelectedBusinessUnit);
            vm.DisplayName = string.Format(
                "{0}{1}", 
                RunTime.FindStringResource("Modify"), 
                RunTime.FindStringResource("BusinessUnit"));

            this.windowManager.ShowWindow(vm);
        }

        /// <summary>
        ///     The new.
        /// </summary>
        public void New()
        {
            var vm = new BusinessUnitAddViewModel(this.OwnerId);
            vm.DisplayName = string.Format(
                "{0}{1}",
                RunTime.FindStringResource("New"),
                RunTime.FindStringResource("BusinessUnit"));

            this.windowManager.ShowWindow(vm);
        }
        
        /// <summary>
        /// The search.
        /// </summary>
        public void Search()
        {
            this.BusinessUnitList =
                this.businessUnitRepository.Filter(this.Filter).OrderBy(bu => bu.Name).ToObservableCollection();
        }

        #endregion


        private bool Filter(BusinessUnitModel model)
        {
            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && model.Id != this.SearchBusinessUnitId)
            {
                return false;
            }

            return true;
        }
    }
}