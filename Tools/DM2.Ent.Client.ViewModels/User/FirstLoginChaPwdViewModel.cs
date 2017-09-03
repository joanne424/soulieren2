// <copyright file="FirstLoginChaPwdViewModel.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> zhanggq </author>
// <date> 2016/4/18 16:04:41 </date>
// <modify>
//   修改人：zhanggq
//   修改时间：2016/4/18 16:04:41
//   修改描述：新建 FirstLoginChaPwdViewModel.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>
namespace DM2.Ent.Client.ViewModels.User
{
    #region

    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;

    using Caliburn.Micro;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// FirstLoginChaPwdViewModel
    /// </summary>
    public class FirstLoginChaPwdViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// 校验新密码
        /// </summary>
        private string confirmPassword;

        /// <summary>
        /// 原始密码
        /// </summary>
        private string currentPassword = string.Empty;

        /// <summary>
        /// 是否明文显示密码
        /// </summary>
        private bool ischecked;

        /// <summary>
        /// 新密码
        /// </summary>
        private string newPassword = string.Empty;

        /// <summary>
        /// 交易员ID
        /// </summary>
        private string staffid = string.Empty;

        /// <summary>
        /// 是否可见密码
        /// </summary>
        private Visibility visiby = Visibility.Hidden;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstLoginChaPwdViewModel"/> class. 
        /// "Initializes a new instance of the <see cref="ChangePasswordViewModel"/> class."
        /// </summary>
        /// <param name="staffid">
        /// 交易员ID
        /// </param>
        /// <param name="varOwnerId">
        /// null
        /// </param>
        public FirstLoginChaPwdViewModel(string staffid, string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("ChangePassword");
            Messenger.Default.Register<string>(
                this, 
                "UpdateLanguage", 
                msg => this.SetDisplayName(RunTime.FindStringResource("ChangePassword")));
            this.staffid = staffid;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 与前台绑定校验的密码
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                this.confirmPassword = value;
                this.NotifyOfPropertyChange("ConfirmPassword");
            }
        }

        /// <summary>
        /// 与前台绑定原始密码数据
        /// </summary>
        public string CurrentPassword
        {
            get
            {
                return this.currentPassword;
            }

            set
            {
                this.currentPassword = value;
                this.NotifyOfPropertyChange("CurrentPassword");
            }
        }

        /// <summary>
        /// 是否修改成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 是否明文显示密码
        /// </summary>
        public bool Ischecked
        {
            get
            {
                return this.ischecked;
            }

            set
            {
                this.ischecked = value;
                if (this.ischecked)
                {
                    this.Visiby = Visibility.Visible;
                }
                else
                {
                    this.Visiby = Visibility.Hidden;
                }

                this.NotifyOfPropertyChange("Ischecked");
            }
        }

        /// <summary>
        /// 与前台绑定新密码
        /// </summary>
        public string NewPassword
        {
            get
            {
                return this.newPassword;
            }

            set
            {
                this.newPassword = value;
                this.NotifyOfPropertyChange("NewPassword");
            }
        }

        /// <summary>
        /// 是否可见密码
        /// </summary>
        public Visibility Visiby
        {
            get
            {
                return this.visiby;
            }

            set
            {
                this.visiby = value;
                this.NotifyOfPropertyChange("Visiby");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 清空文本空中的内容
        /// </summary>
        public void ClearTextbox()
        {
            this.CurrentPassword = string.Empty;
            this.NewPassword = string.Empty;
            this.ConfirmPassword = string.Empty;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public override void Dispose()
        {
            Messenger.Default.Unregister<string>(this, "UpdateLanguage");
        }

        /// <summary>
        /// 校验输入值的合法性
        /// </summary>
        /// <param name="propertyName">
        /// 输入的值得名字
        /// </param>
        /// <returns>
        /// 返回是否合法
        /// </returns>
        public string FristProfileCheck(string propertyName)
        {
            string contentfigure = "[A-Z]+";
            string contentlcase = "[a-z]+";
            string contentcapital = "[0-9]+";
            string allowChar = "[0-9]|[A-Z]|[a-z]|[!@#$%^&*()_]";

            MatchCollection contentfigures;
            MatchCollection contentlcases;
            MatchCollection contentcapitals;
            MatchCollection allowChars;

            int mixPlaces = 6;
            if (propertyName == "NewPassword")
            {
                if (string.IsNullOrWhiteSpace(this.NewPassword))
                {
                    return RunTime.FindStringResource("MSG_10007");
                }

                contentfigures = Regex.Matches(this.NewPassword, contentfigure);
                contentlcases = Regex.Matches(this.NewPassword, contentlcase);
                contentcapitals = Regex.Matches(this.NewPassword, contentcapital);
                allowChars = Regex.Matches(this.NewPassword, allowChar);

                if (string.IsNullOrEmpty(this.NewPassword) || this.NewPassword.Length < mixPlaces
                    || contentcapitals.Count <= 0 || contentlcases.Count <= 0 || contentfigures.Count <= 0)
                {
                    return RunTime.FindStringResource("MSG_10007");
                }

                if (allowChars.Count != this.NewPassword.Length)
                {
                    return RunTime.FindStringResource("MSG_10007");
                    // return this.GetInvalidChars(this.NewPassword, allowChar);
                }
            }

            if (propertyName == "ConfirmPassword")
            {
                if (this.ConfirmPassword != this.NewPassword)
                {
                    return RunTime.FindStringResource("MSG_10006");
                }
            }

            if (propertyName == "CurrentPassword")
            {
                if (string.IsNullOrWhiteSpace(this.CurrentPassword))
                {
                    return RunTime.FindStringResource("MSG_10016");
                }

                contentfigures = Regex.Matches(this.CurrentPassword, contentfigure);
                contentlcases = Regex.Matches(this.CurrentPassword, contentlcase);
                contentcapitals = Regex.Matches(this.CurrentPassword, contentcapital);
                allowChars = Regex.Matches(this.CurrentPassword, allowChar);

                if (string.IsNullOrEmpty(this.CurrentPassword) || contentcapitals.Count <= 0 || contentlcases.Count <= 0
                    || contentfigures.Count <= 0 || this.CurrentPassword.Length < mixPlaces)
                {
                    return RunTime.FindStringResource("MSG_10016");
                }

                if (allowChars.Count != this.CurrentPassword.Length)
                {
                    return RunTime.FindStringResource("MSG_10007");
                    // return this.GetInvalidChars(this.CurrentPassword, allowChar);
                }
            }

            return null;
        }

        /// <summary>
        /// 密码修改确定事件
        /// </summary>
        public void OK()
        {
            if (!string.Equals(this.newPassword, this.confirmPassword))
            {
                RunTime.ShowInfoDialog("Login failed. Inconsistent password.", null, this.OwnerId);
                this.IsSuccess = false;
                return;
            }

            if (!this.ValidateforSumbit())
            {
                this.IsSuccess = false;
                return;
            }

            try
            {
                var service = this.GetSevice<UserService>();
                CmdResult rst = service.ChangeUserPwd(
                this.staffid,
                this.currentPassword,
                this.NewPassword);
                if (rst.Success)
                {
                    var model = new SuccessInfoWinViewModel(
                    RunTime.FindStringResource(rst.ErrorCode),
                    rst.ErrorCode,
                    this.OwnerId);
                    this.windowManager.ShowDialog(model);

                    this.IsSuccess = true;
                    this.TryClose(false);
                }
                else
                {
                    var model = new FailInfoWinViewModel(RunTime.FindStringResource(rst.ErrorCode), null, this.OwnerId);
                    this.IsSuccess = false;
                    this.windowManager.ShowDialog(model);
                }
            }
            catch (Exception exception)
            {
                this.IsSuccess = false;
                Infrastructure.Log.TraceManager.Error.Write("ChangePwd", exception, "Excepiton when changepwd.");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 当窗体关闭时
        /// </summary>
        /// <param name="close">
        /// 是否关闭
        /// </param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 验证输入框消息
        /// </summary>
        /// <param name="propertyName">
        /// 输入的值得名字
        /// </param>
        /// <returns>
        /// 返回是否合法
        /// </returns>
        protected override string OnValidate(string propertyName)
        {
            string message = null;
            message = this.FristProfileCheck(propertyName);
            return message;
        }

        /// <summary>
        /// 获取输入字符串中的非法字符
        /// </summary>
        /// <param name="inputSource">
        /// 输入字符串
        /// </param>
        /// <param name="allowChar">
        /// 允许的合法字符的正则表达式
        /// </param>
        /// <returns>
        /// 返回输入值中包含的非法字符
        /// </returns>
        private string GetInvalidChars(string inputSource, string allowChar)
        {
            StringBuilder sb = new StringBuilder();
            string exceptStr = Regex.Replace(inputSource, allowChar, string.Empty);
            foreach (var item in exceptStr)
            {
                sb.Replace(item.ToString(), string.Empty);
                sb.Append(item);
            }

            string errorMsg = string.Format("Input has invalid char : \"{0}\"", sb);
            return errorMsg;
        }

        #endregion
    }
}