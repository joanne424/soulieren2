// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewCounterpartyGroupViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Counterparty
{

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;

    using System.Collections.ObjectModel;

    using DM2.Ent.Presentation.Service;

    /// <summary>
    /// The new counterparty group view model.
    /// </summary>
    public class NewCounterpartyGroupViewModel : CounterPartyGroupModel
    {

        #region Fields

        private ObservableCollection<BusinessUnitModel> businessUnits;

        private ObservableCollection<CounterPartyGroupModel> allGroups;

        private readonly ICounterPartyGroupRepository counterpartyGroupRepository;
        #endregion
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewCounterpartyGroupViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public NewCounterpartyGroupViewModel(string ownerId)
            : base(ownerId)
        {
            IBusinessUnitRepository businessUnitRepository = this.GetRepository<IBusinessUnitRepository>();
            this.BusinessUnits = businessUnitRepository.GetBindCollection().ToComboboxBinding();

            this.counterpartyGroupRepository = this.GetRepository<ICounterPartyGroupRepository>();
            //this.AllGroups = this.counterpartyGroupRepository.GetBindCollection().ToComboboxBinding();


            //businessUnitRepository.SubscribeAddEvent(
            //    model => this.businessUnits.Insert(0, model));
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


            //counterpartyGroupRepository.SubscribeAddEvent(
            //    model =>
            //        {
            //            if (this.BusinessUnitId == model.BusinessUnitId)
            //            {
            //                this.allGroups.Insert(0, model);
            //            }
            //        });
            //counterpartyGroupRepository.SubscribeUpdateEvent(
            //    (oldModel, newModel) =>
            //    {
            //        var item = this.allGroups.FirstOrDefault(m => m.Id == oldModel.Id);
            //        if (item != null)
            //        {
            //            newModel.Copy(item);
            //        }
            //    });
            //counterpartyGroupRepository.SubscribeRemoveEvent(
            //    model =>
            //    {
            //        var removedItem = this.allGroups.FirstOrDefault(m => m.Id == model.Id);
            //        if (removedItem != null)
            //        {
            //            this.allGroups.Remove(removedItem);
            //        }
            //    });


            this.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            this.IsReadied = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the all groups.
        /// </summary>
        public ObservableCollection<CounterPartyGroupModel> AllGroups
        {
            get
            {
                return this.allGroups;
            }
            set
            {
                this.allGroups = value;
                this.NotifyOfPropertyChange();
            }
        }


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
        /// 页面数据是否准备就绪
        /// </summary>
        protected bool IsReadied { get; set; }

        #endregion

        #region Public Methods and Operators


        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }


        public void OnBusinessUnitChanged()
        {
            this.AllGroups =
                this.counterpartyGroupRepository.Filter(
                    cpg => cpg.BusinessUnitId == this.BusinessUnitId && cpg.Id != this.Id).ToComboboxBinding();

            this.ParentId = string.Empty;
        }
        

        /// <summary>
        ///     The on saved.
        /// </summary>
        public virtual void OnSaved()
        {
            this.IsReadied = true;

            if (!this.ValidateforSumbit())
            {
                return;
            }

            if (!RunTime.ShowConfirmDialog("MSG_00002", string.Empty, this.OwnerId))
            {
                return;
            }

            CmdResult result = this.GetSevice<CounterPartyGroupService>().AddCounterpartyGroup(this);

            if (result.Success)
            {
                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
                this.Reset();
            }
            else
            {
                RunTime.ShowFailInfoDialog(result.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on validate.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected override string OnValidate(string propertyName)
        {
            if (!this.IsReadied)
            {
                return null;
            }

            if (propertyName == "BusinessUnitId" && string.IsNullOrEmpty(this.BusinessUnitId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "Name" && string.IsNullOrEmpty(this.Name))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            return null;
        }

        /// <summary>
        ///     重置
        /// </summary>
        protected void Reset()
        {
            this.IsReadied = false;

            this.Name = string.Empty;
            this.ParentId = string.Empty;
            this.BusinessUnitId = string.Empty;

            this.allGroups.Clear();
        }

        #endregion
    }
}