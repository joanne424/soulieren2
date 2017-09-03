// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterpartyGroupViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.User
{
    using Caliburn.Micro;

    /// <summary>
    /// The counterparty group view model.
    /// </summary>
    public class CounterpartyGroupViewModel : PropertyChangedBase
    {
        #region Fields

        /// <summary>
        /// The group id.
        /// </summary>
        private string groupId;

        /// <summary>
        /// The group name.
        /// </summary>
        private string groupName;

        /// <summary>
        /// bu name
        /// </summary>
        private string businessUnitName;

        /// <summary>
        /// The is checked.
        /// </summary>
        private bool isChecked;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        public string GroupId
        {
            get
            {
                return this.groupId;
            }

            set
            {
                this.groupId = value;
                this.NotifyOfPropertyChange(() => this.GroupId);
            }
        }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName
        {
            get
            {
                return this.groupName;
            }

            set
            {
                this.groupName = value;
                this.NotifyOfPropertyChange(() => this.GroupName);
            }
        }

        /// <summary>
        /// bu name
        /// </summary>
        public string BusinessUnitName
        {
            get
            {
                return this.businessUnitName;
            }

            set
            {
                this.businessUnitName = value;
                this.NotifyOfPropertyChange(() => this.BusinessUnitName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is checked.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;
                this.NotifyOfPropertyChange(() => this.IsChecked);
            }
        }

        #endregion


        //public void OnChecked()
        //{
        //    this.IsChecked = !this.isChecked;
        //}
    }
}