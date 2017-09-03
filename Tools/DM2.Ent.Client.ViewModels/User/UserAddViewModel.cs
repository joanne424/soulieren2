// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserAddViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.User
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common;

    /// <summary>
    /// The user add view model.
    /// </summary>
    public class UserAddViewModel : UserModel
    {
        #region Fields
        /// <summary>
        /// 登录名合法性检查
        /// </summary>
        private const string LoginNameRegex = "^[a-zA-Z][a-zA-Z0-9-_.]{0,18}[a-zA-Z0-9]$";

        private readonly ICounterPartyGroupRepository counterpartyGroupRepository;

        /// <summary>
        /// The all is checked.
        /// </summary>
        private bool allIsChecked;

        private ObservableCollection<BusinessUnitModel> businessUnits;

        private ObservableCollection<CounterpartyGroupViewModel> counterpartyGroups;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAddViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public UserAddViewModel(string ownerId)
            : base(ownerId)
        {
            this.BusinessUnits = this.GetRepository<IBusinessUnitRepository>().GetBindCollection().ToComboboxBinding();

            this.counterpartyGroupRepository = this.GetRepository<ICounterPartyGroupRepository>();

            //this.CounterpartyGroups =
            //    this.GetRepository<ICounterPartyGroupRepository>()
            //        .Filter(cpg => !string.IsNullOrEmpty(cpg.Id))
            //        .OrderBy(cpg => cpg.Name)
            //        .Select(cpg => new CounterpartyGroupViewModel()
            //                           {
            //                               IsChecked = false,
            //                               GroupId = cpg.Id,
            //                               GroupName = cpg.Name
            //                           })
            //        .ToList();
            this.CounterpartyGroups =
                this.counterpartyGroupRepository.Filter(cpg => !string.IsNullOrEmpty(cpg.Id)).Select(
                    cpg =>
                        {
                            string buName = string.Empty;
                            BusinessUnitModel bu = this.BusinessUnits.FirstOrDefault(p => p.Id == cpg.BusinessUnitId);
                            if (bu != null)
                            {
                                buName = bu.Name;
                            }
                            return new CounterpartyGroupViewModel()
                                       {
                                           IsChecked = false,
                                           GroupId = cpg.Id,
                                           GroupName = cpg.Name,
                                           BusinessUnitName = buName
                                       };
                        }).OrderBy(cpg => cpg.BusinessUnitName).ThenBy(cpg => cpg.GroupName).ToObservableCollection();

            this.EntId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;
            this.IsReadied = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether all is checked.
        /// </summary>
        public bool IsAllChecked
        {
            get
            {
                return this.allIsChecked;
            }
            set
            {
                this.allIsChecked = value;
                this.NotifyOfPropertyChange(() => this.IsAllChecked);
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
        /// Gets or sets the counterparty groups.
        /// </summary>
        public ObservableCollection<CounterpartyGroupViewModel> CounterpartyGroups
        {
            get
            {
                return this.counterpartyGroups;
            }
            set
            {
                this.counterpartyGroups = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the user groups.
        /// </summary>
        public IDictionary<string, string> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        public IDictionary<string, string> UserRoles { get; set; }

        /// <summary>
        /// 页面数据是否准备就绪
        /// </summary>
        protected bool IsReadied { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The cancel.
        /// </summary>
        public void Cancel()
        {
            this.TryClose();
        }

        /// <summary>
        /// The on all checked.
        /// </summary>
        public void OnAllChecked(bool isChecked)
        {
            this.IsAllChecked = isChecked;

            this.CounterpartyGroups.ForEach(item => item.IsChecked = isChecked);
        }


        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
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


            if (this.IsAllChecked)
            {
                this.CounterpartyGroupIds = null;
            }
            else
            {
                this.CounterpartyGroupIds =
                    this.CounterpartyGroups.Where(item => item.IsChecked).Select(item => item.GroupId).ToList();
            }
           

            var result = this.GetSevice<UserService>().AddUser(this);

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

        public void OnChecked(string groupId)
        {
            var group = this.CounterpartyGroups.FirstOrDefault(cpg => cpg.GroupId == groupId);
            if (group != null)
            {
                group.IsChecked = !group.IsChecked;
            }

            this.IsAllChecked = this.CounterpartyGroups.All(cpg => cpg.IsChecked);

            //this.IsChecked = !this.isChecked;
        }
        #endregion

        #region Methods
        protected void Reset()
        {
            this.IsReadied = false;

            this.BuId = string.Empty;
            this.GroupId = string.Empty;
            this.LoginName = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;
            this.RoleId = string.Empty;
            this.GroupId = string.Empty;

            this.CounterpartyGroups.ForEach(item => item.IsChecked = false);
            this.IsAllChecked = false;
        }

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
            if (this.IsReadied == false)
            {
                return null;
            }


            if (propertyName == "BuId" && string.IsNullOrEmpty(this.BuId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }
            
            if (propertyName == "LoginName" && !Regex.IsMatch(this.LoginName, LoginNameRegex))
            {
                return RunTime.FindStringResource("MSG_10032");
            }

            if (propertyName == "FirstName" && string.IsNullOrEmpty(this.FirstName))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "LastName" && string.IsNullOrEmpty(this.LastName))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "Email" && !string.IsNullOrEmpty(this.Email) && !this.Email.IsEmail())
            {
                return RunTime.FindStringResource("MSG_00005");
            }

            //if (propertyName == "RoleId" && string.IsNullOrEmpty(this.RoleId))
            //{
            //    return RunTime.FindStringResource("MSG_00010");
            //}


            return null;
        }

        #endregion
    }
}