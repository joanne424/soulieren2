// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserListViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.User
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// The user list view model.
    /// </summary>
    public class UserListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        /// The window manager.
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        private ObservableCollection<BusinessUnitModel> businessUnits;

        /// <summary>
        /// The input login name.
        /// </summary>
        private string searchLoginName;

        /// <summary>
        /// The selected business unit id.
        /// </summary>
        private string searchBusinessUnitId;

        /// <summary>
        /// The selected user.
        /// </summary>
        private UserModel selectedUser;

        /// <summary>
        /// The selected user group id.
        /// </summary>
        private string searchUserGroupId;

        /// <summary>
        /// The user list.
        /// </summary>
        private ObservableCollection<UserModel> userList;


        private readonly IUserRepository userRepository;
        #endregion


        public UserListViewModel(string ownerId)
            : base(ownerId)
        {
            this.DisplayName = RunTime.FindStringResource("UserListTitle");

            this.BusinessUnits = this.GetRepository<IBusinessUnitRepository>().GetBindCollection().ToComboboxBinding(true);

            this.userRepository = this.GetRepository<IUserRepository>();
            this.Search();

            this.userRepository.SubscribeAddEvent(
                model => this.userList.Insert(0, model));
            this.userRepository.SubscribeUpdateEvent(
                (oldModel, newModel) =>
                    {
                        var item = this.userList.FirstOrDefault(m => m.Id == oldModel.Id);
                        if (item != null)
                        {
                            newModel.Copy(item);
                        }
                    });
            this.userRepository.SubscribeRemoveEvent(
                model =>
                    {
                        var removedUser = this.userList.FirstOrDefault(m => m.Id == model.Id);
                        if (removedUser != null)
                        {
                            this.userList.Remove(removedUser);
                        }
                    });
        }

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
        /// Gets or sets the input login name.
        /// </summary>
        public string SearchLoginName
        {
            get
            {
                return this.searchLoginName;
            }

            set
            {
                this.searchLoginName = value;
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

        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        public UserModel SelectedUser
        {
            get
            {
                return this.selectedUser;
            }

            set
            {
                this.selectedUser = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected user group id.
        /// </summary>
        public string SearchUserGroupId
        {
            get
            {
                return this.searchUserGroupId;
            }

            set
            {
                this.searchUserGroupId = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the user groups.
        /// </summary>
        public IDictionary<string, string> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the user list.
        /// </summary>
        public ObservableCollection<UserModel> UserList
        {
            get
            {
                return this.userList;
            }

            set
            {
                this.userList = value;
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
        ///     The edit.
        /// </summary>
        public void Edit()
        {
            if (this.selectedUser == null)
            {
                return;
            }

            var vm = new UserEditViewModel(this.OwnerId, this.selectedUser);
            vm.DisplayName = string.Format(
                "{0}{1}",
                RunTime.FindStringResource("Modify"),
                RunTime.FindStringResource("User"));

            this.windowManager.ShowWindow(vm);
        }

        /// <summary>
        ///     The new.
        /// </summary>
        public void New()
        {
            var vm = new UserAddViewModel(this.OwnerId);
            vm.DisplayName = string.Format(
                "{0}{1}",
                RunTime.FindStringResource("New"),
                RunTime.FindStringResource("User"));

            this.windowManager.ShowWindow(vm);
        }
        
        /// <summary>
        ///     The search.
        /// </summary>
        public void Search()
        {
            this.UserList =
                this.userRepository.Filter(this.Filter)
                    .OrderBy(user => user.BuId)
                    .ThenBy(user => user.LoginName)
                    .ToObservableCollection();
        }

        #endregion


        private bool Filter(UserModel model)
        {
            if (!string.IsNullOrEmpty(this.SearchBusinessUnitId) && model.BuId != this.SearchBusinessUnitId)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchLoginName)
                && model.LoginName.IndexOf(this.SearchLoginName, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SearchUserGroupId) && model.GroupId != this.SearchUserGroupId)
            {
                return false;
            }

            return true;
        }
    }
}