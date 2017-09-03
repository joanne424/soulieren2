// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewCustomerViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/25 09:44:11 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/25 09:44:11
//      修改描述：新建 NewCustomerViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels
{
    using System;
    using System.Windows;

    using DM2.Ent.Presentation.Models.Base;

    using Infrastracture.Data;

    using Infrastructure.Common;

    using SOU.Model;

    /// <summary>
    ///     The counterparty add view model.
    /// </summary>
    public class NewCustomerViewModel : BaseVm
    {
        #region Fields

        private int count;

        private string password;

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                return this.count;
            }

            set
            {
                this.count = value;
                this.NotifyOfPropertyChange();
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
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
        ///     关闭窗口
        /// </summary>
        public void OnClosed()
        {
            this.TryClose(true);
        }

        /// <summary>
        ///     最小化
        /// </summary>
        /// <param name="window">
        ///     The window.
        /// </param>
        public void OnMinimizeWindowCommand(object window)
        {
            var win = window as Window;
            win.WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     The save.
        /// </summary>
        public void Save()
        {
            //if (!this.ValidateforSumbit())
            //{
            //    // RunTime.ShowInfoDialogWithoutRes(this.Error, string.Empty, this.OwnerId);
            //    return;
            //}

            if (this.Count < 1)
            {
                return;
            }

            CustomerRepository rep = new CustomerRepository();
            for (int i = 0; i < this.Count; i++)
            {
                var name = Infrastructure.Utils.Util.GenerateSurname();
                var phone = Infrastructure.Utils.Util.GetRandomTel();
                var email = Infrastructure.Utils.Util.GetEmail(name);
                Customer customer = new Customer()
                {
                    Is_send_sms = 0,
                    is_send_email = 0,
                    actualName = string.Empty,
                    address = string.Empty,
                    birthday = string.Empty,
                    city = string.Empty,
                    name = name,
                    county = "beijing",
                    create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    last_login_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    email = email,
                    nickname = name,
                    phone = phone,
                    isvirtual = 1,
                    pwd = this.Password,
                    rpwd = this.Password,
                    role = 0,
                    reg_channel = 0,
                    sex = 0,
                    state1 = string.Empty,
                    state = 0,
                    status = 2,
                    province = string.Empty,
                    headerimg = string.Empty,
                    ur = string.Empty,
                };
                rep.Add(customer);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The on validate.
        /// </summary>
        /// <param name="propertyName">
        ///     The property name.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnValidate(string propertyName)
        {
            //if (propertyName == "AccountNo")
            //{
            //    if (string.IsNullOrEmpty(this.AccountNo))
            //    {
            //        return RunTime.FindStringResource("MSG_00010");
            //    }
            //}

            return null;
        }

        /// <summary>
        ///     The on validated.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnValidated()
        {
            return base.OnValidated();
        }

        #endregion
    }
}