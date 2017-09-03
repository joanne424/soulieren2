// <copyright file="ShellViewModel.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>董国君</author>
// <date> 2013-08-12 </date>
// <summary>主窗体的VM</summary>
// <modify>
//      修改人：董国君
//      修改时间：2013-08-12
//      修改描述：新建
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

// #define Publish
namespace DM2.Ent.Client.Views
{
    #region

    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Windows;

    using ActiproSoftware.Windows.Controls.Docking;

    using Caliburn.Micro;

    using DM2.Ent.Client.ViewModels;
    using DM2.Ent.Presentation.Models.Base;

    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Threading;

    using Infrastructure.Log;

    #endregion

    /// <summary>
    ///     主窗体VM
    /// </summary>
    [Export(typeof(IShell))]
    public partial class ShellViewModel : BaseVm, IShell
    {
        #region Fields

        /// <summary>
        ///     M端主页面标题的前缀
        /// </summary>
        private readonly string mainTitlePrefix = string.Empty;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager;

        /// <summary>
        /// The cash ladder tool vm.
        /// </summary>
        private ShopListToolViewModel shopListToolVM;

        /// <summary>
        ///     交易单VM
        /// </summary>
        private OrderListToolViewModel orderListToolVM;

        /// <summary>
        /// HistoryDealListToolVM
        /// </summary>
        private GoodsListToolViewModel goodsListToolVM;

        /// <summary>
        /// HistoryDealListToolVM
        /// </summary>
        private CustomerListToolViewModel customerListToolVM;

        /// <summary>
        ///     字段 是否显示菜单按钮
        /// </summary>
        private bool isExpanded = true;

        /// <summary>
        ///     RunTime 实例
        /// </summary>
        //private RunTime runtime;

        /// <summary>
        ///     服务连接状态信息
        /// </summary>
        private string serverConnectionInfo;

        /// <summary>
        /// 系统时间显示刷新定时器
        /// </summary>
        private Timer serverDateTiemRefreshTimer;

        /// <summary>
        /// 系统时间显示
        /// </summary>
        private string severDateTime;

        /// <summary>
        ///     主页面View
        /// </summary>
        private ShellView shellView;

        /// <summary>
        /// The size.
        /// </summary>
        private string systemNetInfo;

        /// <summary>
        ///     字段 用户ID
        /// </summary>
        private string userID;

        /// <summary>
        ///     字段 用户名称
        /// </summary>
        private string userName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        [ImportingConstructor]
        public ShellViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = "DM2 Ent Client";


            //RunTime.Initial(varOwnerId);
            //this.runtime = RunTime.GetCurrentRunTime(this.OwnerId);
            //if (this.runtime == null)
            //{
            //    TraceManager.Error.Write("ViewModel", "获取当前Runtime失败");
            //    return;
            //}

            //this.runtime.RegisterLoginLogoutNotifyAction(this.LoginLogoutAction);

            // this.runtime.RegisterLoginLogoutNotifyAction(());
            //this.windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

            //this.runtime.SetCultureInfo();

            // 初始化界面线程
            //DispatcherHelper.Initialize();
            //PerformanceCore.Instance.InitialInPerformanceThread();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the cash ladder tool vm.
        /// </summary>
        //public ShopListToolViewModel ShopListToolVM
        //{
        //    get
        //    {
        //        return this.shopListToolVM;
        //    }

        //    set
        //    {
        //        this.shopListToolVM = value;
        //        this.NotifyOfPropertyChange(() => this.ShopListToolVM);
        //    }
        //}

        ///// <summary>
        /////     属性 订单VM
        ///// </summary>
        //public OrderListToolViewModel OrderListToolVM
        //{
        //    get
        //    {
        //        return this.orderListToolVM;
        //    }

        //    set
        //    {
        //        this.orderListToolVM = value;
        //        this.NotifyOfPropertyChange(() => this.OrderListToolVM);
        //    }
        //}

        ///// <summary>
        /////     属性 订单VM
        ///// </summary>
        //public GoodsListToolViewModel GoodsListToolVM
        //{
        //    get
        //    {
        //        return this.goodsListToolVM;
        //    }

        //    set
        //    {
        //        this.goodsListToolVM = value;
        //        this.NotifyOfPropertyChange(() => this.GoodsListToolVM);
        //    }
        //}

        /// <summary>
        ///     主页面标题
        /// </summary>
        public string MainTitle
        {
            get
            {
                return this.mainTitlePrefix + " " + this.UserID;
            }
        }

        /// <summary>
        /// 市场推演
        /// </summary>
        public CustomerListToolViewModel CustomerListToolVM
        {
            get
            {
                return this.customerListToolVM;
            }

            set
            {
                this.customerListToolVM = value;
                this.NotifyOfPropertyChange(() => this.CustomerListToolVM);
            }
        }

        /// <summary>
        ///     服务连接状态信息
        /// </summary>
        public string ServerConnectionInfo
        {
            get
            {
                return this.serverConnectionInfo;
            }

            set
            {
                this.serverConnectionInfo = value;
                this.NotifyOfPropertyChange("ServerConnectionInfo");
            }
        }

        /// <summary>
        /// Gets or sets the sever date time.
        /// </summary>
        public string SeverDateTime
        {
            get
            {
                return this.severDateTime;
            }

            set
            {
                this.severDateTime = value;
                this.NotifyOfPropertyChange("SeverDateTime");
            }
        }

        /// <summary>
        /// 系统网络信息
        /// </summary>
        public string SystemNetInfo
        {
            get
            {
                return this.systemNetInfo;
            }

            set
            {
                this.systemNetInfo = value;
                this.NotifyOfPropertyChange("SystemNetInfo");
            }
        }

        /// <summary>
        ///     属性 用户ID
        /// </summary>
        public string UserID
        {
            get
            {
                return this.userID;
            }

            set
            {
                this.userID = value;
                this.NotifyOfPropertyChange("UserID");
                this.NotifyOfPropertyChange("MainTitle");
            }
        }

        /// <summary>
        ///     属性 用户名称
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                this.userName = value;
                this.NotifyOfPropertyChange("UserName");
            }
        }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        ///     关闭登陆窗体（隐藏界面）
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        public void ExitSys()
        {
            try
            {
                this.ExitSystem();
                // 弹出确认框
                //if (RunTime.ShowConfirmDialog("MSG_10012", string.Empty, this.OwnerId))
                //{
                //    this.ExitSystem();
                //}
            }
            catch (Exception exception)
            {
                TraceManager.Error.Write("ShellViewModel", exception, "Exception when ExitSys。");
            }
        }

        /// <summary>
        /// 获取选中的TabControl
        /// </summary>
        /// <param name="tabbe">
        /// The tabbe.
        /// </param>
        /// <param name="itemName">
        /// 加载界面的名字
        /// </param>
        public void GetSelectItem(TabbedMdiContainer tabbe, string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                for (int i = 0; i < tabbe.Items.Count; i++)
                {
                    var one = tabbe.Items[i] as DockingWindow;
                    if (tabbe is TabbedMdiContainer && one != null && one.Name == itemName)
                    {
                        tabbe.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 图标菜单响应函数,尚未进一步完善
        /// </summary>
        /// <param name="methodName">
        /// 方法名称
        /// </param>
        /// <param name="tabb">
        /// The tabb.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public void MenuItemClick(string methodName)
        {
            // 根据反射调用本类的方法
            MethodInfo method;
            method = typeof(ShellViewModel).GetMethod(methodName);

            if (method != null)
            {
                method.Invoke(this, null);
            }
        }

        /// <summary>
        /// The open bank acct list click.
        /// </summary>
        public void OpenBankAcctListClick()
        {
            //if (this.runtime.CurrentLoginUser == null)
            //{
            //    var loginViewModel = new LoginViewModel { IsBancLogix = ConfigParameter.IsBancLogix };
            //    var dialog = this.windowManager.ShowDialog(loginViewModel);

            //    if (dialog.HasValue && !dialog.Value)
            //    {
            //        return;
            //    }
            //}

            //var vm = new BankAccountListViewModel();
            //this.windowManager.ShowWindow(vm);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 当主页面的View加载完成的时候，弹出登录框
        /// </summary>
        /// <param name="view">
        /// 主窗体View
        /// </param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            this.shellView = view as ShellView;
            this.Initial();
        }

        /// <summary>
        /// 异常退出程序
        /// </summary>
        /// <param name="value">
        /// 提示信息的Key
        /// </param>
        /// <param name="parameters">
        /// 提示信息参数
        /// </param>
        /// <param name="prompt">
        /// 提示信息头
        /// </param>
        /// <param name="isRestart">
        /// 是否重新启动程序
        /// </param>
        private void ErrorExitSystem(string value, object[] parameters, string prompt, bool isRestart = false)
        {
            //string message = RunTime.FindStringResource(value);
            //if (parameters != null && parameters.Length > 0)
            //{
            //    message = string.Format(message, parameters);
            //}

            //// 记录Log
            //// Common.Instance.WriteLog(LogEnum.Debug, "LoginViewModel", "LOG_ACT_M509", this.UserID);

            //// 删除临时文件
            //this.DeleteTempFile();

            //// 保存窗口布局
            //this.SaveLayout();

            //// 根据router的实现，logout有顺序要求 (1). session logout (2). command logout (3). close comunicator
            //if (this.runtime.CurrentLoginUser != null)
            //{
            //    ComunicatorCore.GetComunicatorCore(this.OwnerId).LogoutSession();
            //    this.GetSevice<UserService>().Logout();
            //    ComunicatorCore.GetComunicatorCore(this.OwnerId).Close();
            //}

            //// 此时才弹框
            //RunTime.ShowFailInfoDialogWithoutRes(message, prompt, this.OwnerId);

            //// 关闭所有窗体
            //foreach (Window window in Application.Current.Windows)
            //{
            //    window.Close();
            //}

            //// 重启或关闭程序
            //if (isRestart)
            //{
            //    // 重启应用程序(关闭并启动一个新实例)
            //    System.Windows.Forms.Application.Restart();
            //}
            //else
            //{
            //    // 关闭应用程序
            //    Application.Current.Shutdown();
            //}

            //// 立即结束进程 避免未回收线程、资源报异常（必须Kill  Close方法仍有未结束线程报错的现象）
            //Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 关闭/重启应用程序
        /// </summary>
        /// <param name="isRestart">
        /// true:重启应用, false:关闭应用(默认)
        /// </param>
        private void ExitSystem(bool isRestart = false)
        {
            // 关闭所有窗体
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }

            // 重启或关闭程序
            if (isRestart)
            {
                // 重启应用程序(关闭并启动一个新实例)
                // System.Windows.Forms.Application.Restart();
                System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath);
            }
            else
            {
                // 关闭应用程序
                Application.Current.Shutdown();
            }

            //// 立即结束进程 避免未回收线程、资源报异常（必须Kill  Close方法仍有未结束线程报错的现象）
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        ///     第一次初始化加载
        /// </summary>
        //[PerformanceStatistic]
        private void Initial()
        {

            // 这里首先设置Topmost再关闭的目的是确保推出登录时，主窗口一定能弹出
            //this.LoadLayout();
            this.shellView.Topmost = true;
            this.shellView.Activate();
            this.shellView.Topmost = false;
            //this.ShopListToolVM = new ShopListToolViewModel();
            //this.OrderListToolVM = new OrderListToolViewModel();
            //this.GoodsListToolVM = new GoodsListToolViewModel();
            this.CustomerListToolVM = new CustomerListToolViewModel();

            // 完成启动后的初始化
            //this.runtime.InitialAfterStart();
        }
        #endregion
    }
}