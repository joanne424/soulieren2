// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserEditViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.User
{
    using System.Linq;
    using System.Windows;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common;

    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    /// The user edit view model.
    /// </summary>
    public class UserEditViewModel : UserAddViewModel
    {
        /// <summary>
        /// 锁定图标显示状态.
        /// </summary>
        private Visibility displayLockIcon;

        /// <summary>
        /// 解锁图标显示状态.
        /// </summary>
        private Visibility displayUnlockIcon;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public UserEditViewModel(string ownerId, UserModel model)
            : base(ownerId)
        {
            this.EntId = model.EntId;
            this.BuId = model.BuId;
            this.GroupId = model.GroupId;
            this.CreationTime = model.CreationTime;
            this.LoginName = model.LoginName;
            this.Id = model.Id;
            this.LastUpdateTime = model.LastUpdateTime;
            this.FirstName = model.FirstName;
            this.LastName = model.LastName;
            this.Version = model.Version;
            this.RoleId = model.RoleId;
            this.Email = model.Email;
            this.Locked = model.Locked;

            this.DisplayLockIcon = model.Locked ? Visibility.Visible : Visibility.Hidden;
            this.DisplayUnlockIcon = model.Locked ? Visibility.Hidden : Visibility.Visible;

            if (model.CounterpartyGroupIds == null)
            {
                this.IsAllChecked = true;
                this.CounterpartyGroups.ForEach(item => item.IsChecked = true);
            }
            else
            {
                this.CounterpartyGroupIds = model.CounterpartyGroupIds;
                this.CounterpartyGroups.ForEach(item => item.IsChecked = this.CounterpartyGroupIds.Contains(item.GroupId));
            }
        }

        #endregion


        /// <summary>
        ///     锁定图标显示状态
        /// </summary>
        public Visibility DisplayLockIcon
        {
            get
            {
                return this.displayLockIcon;
            }

            set
            {
                this.displayLockIcon = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     解锁图标显示状态
        /// </summary>
        public Visibility DisplayUnlockIcon
        {
            get
            {
                return this.displayUnlockIcon;
            }

            set
            {
                this.displayUnlockIcon = value;
                this.NotifyOfPropertyChange();
            }
        }

        #region Public Methods and Operators

        /// <summary>
        ///     The on saved.
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

            if (this.IsAllChecked)
            {
                this.CounterpartyGroupIds = null;
            }
            else
            {
                this.CounterpartyGroupIds =
                    this.CounterpartyGroups.Where(item => item.IsChecked).Select(item => item.GroupId).ToList();
            }

            var result = this.GetSevice<UserService>().UpdateUser(this);

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


        public void OnLocked()
        {
            if (!RunTime.ShowConfirmDialog("MSG_10054", string.Empty, this.OwnerId))
            {
                return;
            }


            var cmdResult = this.GetSevice<UserService>().LockUser(this.Id, true);
            if (cmdResult.Success)
            {
                this.DisplayLockIcon = Visibility.Visible;
                this.DisplayUnlockIcon = Visibility.Hidden;

                this.Locked = true;

                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
            }
            else
            {
                RunTime.ShowFailInfoDialog(cmdResult.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        public void OnUnlocked()
        {
            if (!RunTime.ShowConfirmDialog("MSG_10055", string.Empty, this.OwnerId))
            {
                return;
            }


            var cmdResult = this.GetSevice<UserService>().LockUser(this.Id, false);
            if (cmdResult.Success)
            {
                this.DisplayLockIcon = Visibility.Hidden;
                this.DisplayUnlockIcon = Visibility.Visible;

                this.Locked = false;

                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
            }
            else
            {
                RunTime.ShowFailInfoDialog(cmdResult.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        public void ResetPassword()
        {
            if (string.IsNullOrEmpty(this.Email) || !this.Email.IsEmail())
            {
                RunTime.ShowFailInfoDialog("MSG_00005", null, this.OwnerId);
                return;
            }

            if (!RunTime.ShowConfirmDialog("MSG_10031", null, this.OwnerId))
            {
                return;
            }

            var result = this.GetSevice<UserService>().ResetUserPassword(this.Id);
            if (result.Success)
            {
                RunTime.ShowSuccessInfoDialog("MSG_00001", null, this.OwnerId);
            }
            else
            {
                RunTime.ShowFailInfoDialog(result.ErrorCode, string.Empty, this.OwnerId);
            }
        }
        #endregion
    }
}