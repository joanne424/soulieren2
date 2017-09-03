// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyCounterpartyGroupViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.Counterparty
{
    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    /// <summary>
    /// The modify counterparty group view model.
    /// </summary>
    public class ModifyCounterpartyGroupViewModel : NewCounterpartyGroupViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyCounterpartyGroupViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public ModifyCounterpartyGroupViewModel(string ownerId, CounterPartyGroupModel model)
            : base(ownerId)
        {
            this.BusinessUnitId = model.BusinessUnitId;
            this.EnterpriseId = model.EnterpriseId;
            this.Id = model.Id;
            this.Name = model.Name;

            this.OnBusinessUnitChanged();

            this.ParentId = model.ParentId;

            this.CreationTime = model.CreationTime;
            this.LastUpdateTime = model.LastUpdateTime;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The on saved.
        /// </summary>
        public override void OnSaved()
        {
            this.IsReadied = true;

            if (!this.ValidateforSumbit())
            {
                return;
            }

            if (!RunTime.ShowConfirmDialog("MSG_00003", string.Empty, this.OwnerId))
            {
                return;
            }

            CmdResult result = this.GetSevice<CounterPartyGroupService>().UpdateCounterpartyGroup(this);

            if (result.Success)
            {
                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
                this.Close();
            }
            else
            {
                RunTime.ShowFailInfoDialog(result.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        #endregion
    }
}