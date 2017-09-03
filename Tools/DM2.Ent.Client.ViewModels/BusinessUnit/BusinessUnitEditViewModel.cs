// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessUnitEditViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.BusinessUnit
{
    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    /// <summary>
    /// The business unit edit view model.
    /// </summary>
    public class BusinessUnitEditViewModel : BusinessUnitAddViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitEditViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public BusinessUnitEditViewModel(string ownerId, BusinessUnitModel model)
            : base(ownerId)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.GroupId = model.GroupId;
            this.EnterpriseId = model.EnterpriseId;
            this.CountryId = model.CountryId;
            this.TimeZone = model.TimeZone;
            this.DateFormat = model.DateFormat;
            this.LocalCcyId = model.LocalCcyId;
            this.CreationTime = model.CreationTime;
            this.LastUpdateTime = model.LastUpdateTime;
            this.Version = model.Version;
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

            var result = this.GetSevice<BusinessUnitService>().UpdateBusinessUnit(this);

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