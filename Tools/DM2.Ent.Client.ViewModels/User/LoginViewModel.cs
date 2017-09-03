// <copyright file="LoginViewModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2017/04/25 09:05:20 </date>
// <summary> 登录VM </summary>
// <modify>
//      修改人：fangz
//      修改时间：2017/04/25 09:05:20
//      修改描述：新建 LoginViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Ent.Client.ViewModels.User
{
    #region

    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Xml;
    using System.Xml.Serialization;

    using Caliburn.Micro;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;
    using DM2.Ent.Presentation.Service.Base;

    using Infrastructure.Log;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    /// 登录ViewModel
    /// </summary>
    public class LoginViewModel : BaseVm
    {
        #region Constants

        /// <summary>
        /// 历史用户记录文件名
        /// </summary>
        private const string UsersHistoryFileName = "userhistory.dat";

        #endregion

        #region Fields

        /// <summary>
        /// Caliburn.Micro Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// 当前用户
        /// </summary>
        private LoginInfo currentUser;

        /// <summary>
        /// 错误信息
        /// </summary>
        private string errorMsg;

        /// <summary>
        /// 登陆按钮是否获取焦点
        /// </summary>
        private bool isLoginFocused;

        /// <summary>
        /// The is not change password.
        /// </summary>
        private bool isNotChangePassword = false;

        /// <summary>
        /// 密码框是否获取焦点
        /// </summary>
        private bool isPasswordFocused;

        /// <summary>
        /// 是否保存登录信息
        /// </summary>
        private bool isSaveLoginInfo;

        /// <summary>
        /// 账号框是否获取焦点
        /// </summary>
        private bool isUserIdFocused;

        /// <summary>
        /// 当前ViewModel对应的LoginView
        /// </summary>
        private Window loginView;

        /// <summary>
        /// 密码
        /// </summary>
        private string password;

        /// <summary>
        /// 用户名
        /// </summary>
        private string userId;

        /// <summary>
        /// 用户列表
        /// </summary>
        private ObservableCollection<LoginInfo> userList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class. 
        /// </summary>
        public LoginViewModel()
        {
            this.DisplayName = RunTime.FindStringResource("Login");


            //this.UserId = "admin";
            this.InitUserList();
            this.SetFocus();
            this.Password = "Pa123456!";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 设置或获取当前用户
        /// </summary>
        public LoginInfo CurrentUser
        {
            get
            {
                return this.currentUser;
            }

            set
            {
                this.currentUser = value;
                if (this.currentUser != null)
                {
                    this.UserId = this.currentUser.UserId;
                    this.Password = this.currentUser.Password;
                    this.IsSaveLoginInfo = this.currentUser.IsRemembered;
                    this.SetFocus();
                }

                this.NotifyOfPropertyChange(() => this.CurrentUser);
            }
        }

        /// <summary>
        /// 设置或获取错误信息
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this.errorMsg;
            }

            set
            {
                this.errorMsg = value;
                this.NotifyOfPropertyChange(() => this.ErrorMsg);
            }
        }

        /// <summary>
        /// 属性 标识是否是BancLogix显示不同的程序Logo和Loading页面的Logo，true为是BancLogix，false为是ForexStar
        /// </summary>
        public bool IsBancLogix { get; set; }

        /// <summary>
        /// 设置或获取Login按钮是否获取焦点
        /// </summary>
        public bool IsLoginFocused
        {
            get
            {
                return this.isLoginFocused;
            }

            set
            {
                this.isLoginFocused = value;
                this.NotifyOfPropertyChange(() => this.IsLoginFocused);
            }
        }

        /// <summary>
        /// 设置或获取密码框是否获取输入焦点
        /// </summary>
        public bool IsPasswordFocused
        {
            get
            {
                return this.isPasswordFocused;
            }

            set
            {
                this.isPasswordFocused = value;
                this.NotifyOfPropertyChange(() => this.IsPasswordFocused);
            }
        }

        /// <summary>
        /// 设置或获取是否保存登录信息
        /// </summary>
        public bool IsSaveLoginInfo
        {
            get
            {
                return this.isSaveLoginInfo;
            }

            set
            {
                this.isSaveLoginInfo = value;
                this.NotifyOfPropertyChange(() => this.IsSaveLoginInfo);
            }
        }

        /// <summary>
        /// 设置或获取UserID框是否获取输入焦点
        /// </summary>
        public bool IsUserIdFocused
        {
            get
            {
                return this.isUserIdFocused;
            }

            set
            {
                this.isUserIdFocused = value;
                this.NotifyOfPropertyChange(() => this.IsUserIdFocused);
            }
        }

        /// <summary>
        /// 设置或获取密码
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value.Trim();
                this.ErrorMsg = string.Empty;
                this.NotifyOfPropertyChange(() => this.Password);
            }
        }

        /// <summary>
        /// 设置或获取用户名
        /// </summary>
        public string UserId
        {
            get
            {
                return this.userId;
            }

            set
            {
                this.userId = value.Trim();
                this.ErrorMsg = string.Empty;
                this.NotifyOfPropertyChange(() => this.UserId);
            }
        }

        /// <summary>
        /// 设置或获取用户列表
        /// </summary>
        public ObservableCollection<LoginInfo> UserList
        {
            get
            {
                if (this.userList == null)
                {
                    this.userList = new ObservableCollection<LoginInfo>();
                }

                return this.userList;
            }

            set
            {
                this.userList = value;
                this.NotifyOfPropertyChange(() => this.UserList);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 点击X按钮退出进程
        /// </summary>
        public void Exit()
        {
            this.TryClose(false);
        }

        /// <summary>
        /// 历史用户列表下拉菜单打开事件
        /// </summary>
        public void HistoryBoxOpened()
        {
            this.CurrentUser = null;
        }

        /// <summary>
        /// 登陆按钮失去焦点事件
        /// </summary>
        public void LoginLostFocus()
        {
            this.IsLoginFocused = false;
        }

        /// <summary>
        /// 密码框失去焦点事件
        /// </summary>
        public void PasswordLostFocus()
        {
            this.IsPasswordFocused = false;
        }

        /// <summary>
        /// 登录按钮响应事件
        /// </summary>
        public void TryLogin()
        {
            if (string.IsNullOrEmpty(this.UserId) || string.IsNullOrEmpty(this.Password))
            {
                this.ErrorMsg = RunTime.FindStringResource("MSG_10001");
                return;
            }

            // 登陆中
            this.loginView.IsEnabled = false;
            ThreadPool.QueueUserWorkItem(state => this.Login());
        }

        /// <summary>
        /// 用户名框失去焦点事件
        /// </summary>
        public void UserIdLostFocus()
        {
            this.IsUserIdFocused = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// View加载完成
        /// </summary>
        /// <param name="view">
        /// View对象
        /// </param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            // Window绑定属性无效，故此处添加了View的依赖
            this.loginView = view as Window;
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="datas">
        /// 字节流
        /// </param>
        /// <returns>
        /// 解密后的XML字符串
        /// </returns>
        private static string BytesToString(byte[] datas)
        {
            var sb = new StringBuilder();
            for (int i = datas.Length - 1; i >= 0; i--)
            {
                var item = (char)datas[i];
                sb.Append(item);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="strData">
        /// 待加密的XML字符串
        /// </param>
        /// <returns>
        /// 加密后的字节流
        /// </returns>
        private static byte[] StrToBytes(string strData)
        {
            byte[] datas = new byte[strData.Length];
            char[] charData = strData.ToCharArray();
            for (int i = 0; i < charData.Length; i++)
            {
                datas[charData.Length - 1 - i] = (byte)charData[i];
            }

            return datas;
        }

        /// <summary>
        /// 像历史存储中添加新用户
        /// </summary>
        private void AddNewUser()
        {
            bool isNotExist = true;
            foreach (var item in this.UserList)
            {
                if (item.UserId == this.UserId)
                {
                    isNotExist = false;
                    item.UserId = this.UserId;
#if DEBUG
                    item.Password = this.Password;
#else
                    item.Password = string.Empty;
#endif
                    item.IsRemembered = this.IsSaveLoginInfo;
                    item.LastLoginTime = RunTime.GetCurrentRunTime(this.OwnerId).GetGmtSystemTime();
                    break;
                }
            }

            if (isNotExist && !string.IsNullOrEmpty(this.UserId))
            {
                var newUser = new LoginInfo();
                newUser.UserId = this.UserId;
#if DEBUG
                newUser.Password = this.Password;
#else
                newUser.Password = string.Empty;
#endif
                newUser.Password = string.Empty;
                newUser.IsRemembered = this.IsSaveLoginInfo;
                newUser.LastLoginTime = RunTime.GetCurrentRunTime(this.OwnerId).GetGmtSystemTime();
                newUser.ItemClicked += (o, e) => this.DelUserItem(e.ItemId);
                this.UserList.Add(newUser);
            }

            this.SaveUsersLoginInfo();
        }

        /// <summary>
        /// 删除历史用户项
        /// </summary>
        /// <param name="userIdArgs">
        /// 用户名
        /// </param>
        private void DelUserItem(string userIdArgs)
        {
            if (!string.IsNullOrEmpty(userIdArgs))
            {
                if (this.UserId == userIdArgs)
                {
                    this.UserId = string.Empty;
                    this.Password = string.Empty;
                }

                int index = -1;
                for (int i = 0; i < this.UserList.Count; i++)
                {
                    if (this.UserList[i].UserId == userIdArgs)
                    {
                        index = i;
                        break;
                    }
                }

                if (index >= 0)
                {
                    this.UserList.RemoveAt(index);
                    this.SaveUsersLoginInfo();
                }
            }
        }

        /// <summary>
        /// The first login handle.
        /// </summary>
        /// <param name="msgCode">
        /// The msg code.
        /// </param>
        /// <param name="newPassword">
        /// The new Password.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FirstLoginHandle(string msgCode, out string newPassword)
        {
            string tempNewPassword = string.Empty;
            bool changeResult = false;
            RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                () =>
                {
                    this.loginView.WindowState = WindowState.Normal;
                    var model = new ConfirmWindowViewModel(RunTime.FindStringResource(msgCode), this.OwnerId);
                    var confirmResult = this.windowManager.ShowDialog(model);
                    if (confirmResult != null && confirmResult.Value)
                    {
                        var pmodel = new FirstLoginChaPwdViewModel(this.UserId, this.OwnerId);

                        this.windowManager.ShowDialog(pmodel);
                        changeResult = pmodel.IsClosed;
                        if (changeResult)
                        {
                            tempNewPassword = pmodel.NewPassword;
                        }
                    }
                    else
                    {
                        this.isNotChangePassword = true;
                    }
                });

            newPassword = tempNewPassword;
            return changeResult;
        }

        /// <summary>
        /// 初始化用户列表
        /// </summary>
        private void InitUserList()
        {
            // 检查历史存储文件
            if (!File.Exists(UsersHistoryFileName))
            {
                return;
            }

            // 读取历史存储文件内容
            byte[] datas = File.ReadAllBytes(UsersHistoryFileName);
            string usersHistoryData = BytesToString(datas);
            if (string.IsNullOrEmpty(usersHistoryData))
            {
                return;
            }

            // 反序列化
            var userListTemp = LoginInfo.XmlToObject<ObservableCollection<LoginInfo>>(usersHistoryData);

            // 过滤得到用户历史列表
            (from item in userListTemp where item.IsRemembered orderby item.LastLoginTime descending select item)
                .ForEach(
                    item =>
                    {
                        this.UserList.Add(item);
                        item.ItemClicked += (o, e) => this.DelUserItem(e.ItemId);
                    });

            if (this.UserList.Count > 0)
            {
                this.CurrentUser = this.UserList[0];
            }
        }

        /// <summary>
        /// 登陆成功后的初始化赋值工作
        /// </summary>
        /// <param name="rst">
        /// 结果
        /// </param>
        /// <returns>
        /// 初始化结果
        /// </returns>
        private bool InitWhenLoginSuccess(CmdResult rst)
        {

            RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(this.AddNewUser);
            var sessionInfo = ServiceRunTime.Instance.GetInstanceFromeJsonContext<UserSession>(rst.ExtensionData);
            ServiceRunTime.Instance.Token = sessionInfo.Id;
            ServiceRunTime.Instance.UserName = sessionInfo.LoginName;

            // 【不能删除此代码】这里登录之后去查询推送查询Staff信息，用这个Staff初始化RunTime
            UserModel userModel = this.GetSevice<UserService>().Get(sessionInfo.AccountId.ToString(CultureInfo.InvariantCulture));
            if (userModel == null)
            {
                RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                    () =>
                    {
                        this.loginView.WindowState = WindowState.Normal;
                        var failModel = new FailInfoWinViewModel(
                            "Get staff information is failed.",
                            string.Empty,
                            this.OwnerId);
                        this.windowManager.ShowDialog(failModel);
                    });

                return false;
            }

            // 记录登陆成功的日志
            TraceManager.Info.Write("Login", "Login success, StaffId:{0}, Token:{1}", this.UserId, sessionInfo.Id);

            // 初始化RunTime
            RunTime.GetCurrentRunTime(this.OwnerId).LoginNotify(userModel, sessionInfo.Id);

            // var logModel = ActiveLogGenerator.Instance.GenerateByStaffLogin(this.UserId, rst);
            // this.GetSevice<StaffService>().SendActiveLog(logModel);
            return true;
        }

        /// <summary>
        /// 登录逻辑
        /// </summary>
        private void Login()
        {
            var userService = this.GetSevice<UserService>();

            try
            {
                this.ErrorMsg = string.Empty;
                ConfirmWindowViewModel model = null;

                TraceManager.Info.Write("Login", "User login，userName:{0}", this.UserId);

                CmdResult loginResult = userService.Login(this.UserId, this.Password, false);

                //// 登陆失败弹出相应的错误信息
                if (!loginResult.Success)
                {
                    switch (loginResult.ErrorCode)
                    {
                        case "MSG_10001":
                            {
                                // 用户名不存在 
                                // 用户名 或 密码为空
                                // 用户名 或 密码错误
                                int remainTimes = 0;

                                if (loginResult.ExtensionData != null)
                                {
                                    int.TryParse(loginResult.ExtensionData, out remainTimes);
                                }

                                RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                                    () =>
                                    {
                                        this.loginView.IsEnabled = true;
                                        this.ErrorMsg = RunTime.GetParamentersStringResource(
                                            loginResult.ErrorCode,
                                            remainTimes);
                                    });
                            }

                            break;

                        case "MSG_10003":
                            {
                                // 该员工用户已被禁用
                                RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                                    () =>
                                    {
                                        this.loginView.IsEnabled = true;
                                        this.ErrorMsg = RunTime.FindStringResource(loginResult.ErrorCode);
                                    });
                            }

                            break;

                        case "MSG_10004":
                            {
                                // 用户无该端的任何权限
                                RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                                    () =>
                                    {
                                        this.loginView.IsEnabled = true;
                                        this.ErrorMsg = RunTime.FindStringResource(loginResult.ErrorCode);
                                    });
                            }

                            break;

                        case "MSG_10009":
                            {
                                // 判断该员工用户已经登录，提示是否踢掉前一个登录者
                                bool? forceLogin = null;
                                CmdResult result = loginResult;
                                RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                                    () =>
                                    {
                                        // 设置弹框在前端显示
                                        dynamic setting = new ExpandoObject();
                                        setting.Topmost = true;

                                        model =
                                            new ConfirmWindowViewModel(
                                                RunTime.FindStringResource(result.ErrorCode),
                                                this.OwnerId);
                                        forceLogin = this.windowManager.ShowDialog(model, null, setting);
                                    });

                                // Common.Instance.WriteLog(LogEnum.Debug, "LoginViewModel", "LOG_ACT_M504", this.UserId);
                                if (forceLogin != null)
                                {
                                    if (!forceLogin.Value)
                                    {
                                        this.Exit();
                                    }
                                    else
                                    {
                                        loginResult = userService.Login(this.UserId, this.Password, true);
                                        if (loginResult.ErrorCode == "MSG_10005" || loginResult.ErrorCode == "MSG_10010"
                                            || loginResult.ErrorCode == "MSG_10011")
                                        {
                                            string newPassword = string.Empty;
                                            bool changeResult = this.FirstLoginHandle(
                                                loginResult.ErrorCode,
                                                out newPassword);
                                            if (!changeResult)
                                            {
                                                return;
                                            }

                                            ////if (this.isNotChangePassword)
                                            ////{
                                            ////    // 退出登录
                                            ////    userService.Logout();
                                            ////    ComunicatorCore.GetComunicatorCore(this.OwnerId).Close();

                                            ////    // 重启
                                            ////    System.Windows.Forms.Application.Restart();

                                            ////    // 关掉当前进程
                                            ////    Process.GetCurrentProcess().Kill();
                                            ////    return;
                                            ////}

                                            // Autho新的实现策略,改密码前是临时Token,修改密码后需要重新登陆
                                            // loginResult = userService.Login(this.UserId, newPassword, false, out token);
                                        }

                                        if (!this.InitWhenLoginSuccess(loginResult))
                                        {
                                            return;
                                        }

                                        this.TryClose(true);
                                        return;
                                    }
                                }
                            }

                            break;
                        default:
                            RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                                () =>
                                {
                                    this.loginView.IsEnabled = true;
                                    this.ErrorMsg = RunTime.FindStringResource(loginResult.ErrorCode);
                                });

                            break;
                    }
                }
                else
                {
                    // 首次登陆修改密码("MSG_ERROR_M040")
                    // 密码被重置，需要修改密码("MSG_ERROR_M041")
                    // 用户密码过期，需要修改密码("MSG_ERROR_M042")
                    if (loginResult.ErrorCode == "MSG_10005" || loginResult.ErrorCode == "MSG_10010"
                        || loginResult.ErrorCode == "MSG_10011")
                    {
                        string newPassword = string.Empty;
                        bool changeResult = this.FirstLoginHandle(loginResult.ErrorCode, out newPassword);
                        if (!changeResult)
                        {
                            return;
                        }

                        ////if (this.isNotChangePassword)
                        ////{
                        ////    // 退出登录
                        ////    userService.Logout();
                        ////    ComunicatorCore.GetComunicatorCore(this.OwnerId).Close();

                        ////    // 重启
                        ////    System.Windows.Forms.Application.Restart();

                        ////    // 关掉当前进程
                        ////    Process.GetCurrentProcess().Kill();
                        ////    return;
                        ////}

                        // Autho新的实现策略,改密码前是临时Token,修改密码后需要重新登陆
                        ////loginResult = userService.Login(this.UserId, newPassword, false);
                    }

                    if (!this.InitWhenLoginSuccess(loginResult))
                    {
                        return;
                    }

                    this.TryClose(true);
                }
            }
            catch (Exception ex)
            {
                RunTime.GetCurrentRunTime(this.OwnerId).ActionOnUiThread(
                    () =>
                    {
                        this.loginView.IsEnabled = true;
                        this.windowManager.ShowDialog(ex.Message);
                    });
            }
        }

        /// <summary>
        /// 保存用户登录记录
        /// </summary>
        private void SaveUsersLoginInfo()
        {
            string usersHistoryData = LoginInfo.ObjectToXml(this.UserList);
            byte[] datas = StrToBytes(usersHistoryData);
            File.WriteAllBytes(UsersHistoryFileName, datas);
        }

        /// <summary>
        /// 设置各个控件焦点
        /// </summary>
        private void SetFocus()
        {
            if (string.IsNullOrEmpty(this.UserId))
            {
                this.IsUserIdFocused = true;
            }
            else if (string.IsNullOrEmpty(this.Password))
            {
                this.IsPasswordFocused = true;
            }
            else
            {
                this.IsLoginFocused = true;
            }
        }

        #endregion

        /// <summary>
        /// 登陆信息类
        /// </summary>
        [Serializable]
        public class LoginInfo : INotifyPropertyChanged
        {
            #region Fields

            /// <summary>
            /// 是否记住用户名
            /// </summary>
            private bool isRemembered;

            /// <summary>
            /// 上次登陆时间
            /// </summary>
            private DateTime lastLoginTime;

            /// <summary>
            /// 密码
            /// </summary>
            private string password;

            /// <summary>
            /// 用户名
            /// </summary>
            private string userId;

            #endregion

            #region Public Events

            /// <summary>
            /// The item clicked.
            /// </summary>
            public event EventHandler<ItemClickedArgs> ItemClicked;

            /// <summary>
            /// The property changed.
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            #region Public Properties

            /// <summary>
            /// 设置或获取是否记住用户名
            /// </summary>
            public bool IsRemembered
            {
                get
                {
                    return this.isRemembered;
                }

                set
                {
                    this.isRemembered = value;
                    this.NotifyPropertyChanged("IsRemembered");
                }
            }

            /// <summary>
            /// 设置或获取上次登陆时间
            /// </summary>
            public DateTime LastLoginTime
            {
                get
                {
                    return this.lastLoginTime;
                }

                set
                {
                    this.lastLoginTime = value;
                    this.NotifyPropertyChanged("LastLoginTime");
                }
            }

            /// <summary>
            /// 设置或获取密码
            /// </summary>
            public string Password
            {
                get
                {
                    return this.password;
                }

                set
                {
                    this.password = value;
                    this.NotifyPropertyChanged("Password");
                }
            }

            /// <summary>
            /// 设置或获取用户名
            /// </summary>
            public string UserId
            {
                get
                {
                    return this.userId;
                }

                set
                {
                    this.userId = value;
                    this.NotifyPropertyChanged("UserId");
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// 解密方法
            /// </summary>
            /// <param name="bytes">
            /// 加密的字节流
            /// </param>
            /// <returns>
            /// 解密后的字符串
            /// </returns>
            public static string ByteToStr(byte[] bytes)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    char item = (char)(byte.MaxValue - bytes[i]);
                    sb.Append(item);
                }

                return sb.ToString();
            }

            /// <summary>
            /// 反序列化对象为XML
            /// </summary>
            /// <typeparam name="T">
            /// 反序列化对象类型
            /// </typeparam>
            /// <param name="serialObject">
            /// 对象
            /// </param>
            /// <returns>
            /// XML字符串
            /// </returns>
            public static string ObjectToXml<T>(T serialObject)
            {
                string result = string.Empty;
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (XmlTextWriter xmlWriter = new XmlTextWriter(ms, Encoding.UTF8))
                        {
                            XmlSerializer xs = new XmlSerializer(serialObject.GetType());
                            xs.Serialize(xmlWriter, serialObject);

                            byte[] buffer = ms.ToArray();
                            result = ByteToStr(buffer);
                        }
                    }
                }
                catch (Exception exception)
                {
                    TraceManager.Error.WriteAdditional("Login", serialObject, exception, "Exception when ObjectToXml.");
                }

                return result;
            }

            /// <summary>
            /// 加密方法
            /// </summary>
            /// <param name="charsString">
            /// 待加密字符串
            /// </param>
            /// <returns>
            /// 加密后的字节流
            /// </returns>
            public static byte[] StrToByte(string charsString)
            {
                var bytes = new byte[charsString.Length];
                for (int i = 0; i < charsString.Length; i++)
                {
                    char item = charsString[i];
                    bytes[i] = (byte)(byte.MaxValue - item);
                }

                return bytes;
            }

            /// <summary>
            /// 序列化XML为对象
            /// </summary>
            /// <typeparam name="T">
            /// 序列化对象类型
            /// </typeparam>
            /// <param name="xmlObject">
            /// XML字符串
            /// </param>
            /// <returns>
            /// 序列化后的对象
            /// </returns>
            public static T XmlToObject<T>(string xmlObject)
            {
                T result = default(T);
                try
                {
                    byte[] buffer = StrToByte(xmlObject);
                    using (var ms = new MemoryStream(buffer))
                    {
                        using (var sr = new StreamReader(ms, Encoding.UTF8))
                        {
                            var xs = new XmlSerializer(typeof(T));
                            result = (T)xs.Deserialize(sr);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Infrastructure.Log.TraceManager.Error.WriteAdditional(
                        "Login",
                        xmlObject,
                        exception,
                        "Exception when XmlToObject when login.");
                }

                return result;
            }

            /// <summary>
            /// The on item clicked.
            /// </summary>
            /// <param name="itemId">
            /// The item id.
            /// </param>
            public void OnItemClicked(object itemId)
            {
                if (itemId != null && itemId is string)
                {
                    this.OnItemClicked(itemId as string);
                }
            }

            /// <summary>
            /// The on item clicked.
            /// </summary>
            /// <param name="itemId">
            /// The item id.
            /// </param>
            public void OnItemClicked(string itemId)
            {
                if (this.ItemClicked != null)
                {
                    this.ItemClicked(this, new ItemClickedArgs(itemId));
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// The notify property changed.
            /// </summary>
            /// <param name="propertyName">
            /// The property name.
            /// </param>
            protected void NotifyPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

            /// <summary>
            /// The item clicked args.
            /// </summary>
            public class ItemClickedArgs : EventArgs
            {
                #region Constructors and Destructors

                /// <summary>
                /// Initializes a new instance of the <see cref="ItemClickedArgs"/> class.
                /// </summary>
                /// <param name="itemId">
                /// The item id.
                /// </param>
                public ItemClickedArgs(string itemId)
                {
                    this.ItemId = itemId;
                }

                #endregion

                #region Public Properties

                /// <summary>
                /// Gets or sets the item id.
                /// </summary>
                public string ItemId { get; set; }

                #endregion
            }
        }
    }
}