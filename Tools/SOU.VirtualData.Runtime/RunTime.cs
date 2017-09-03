// <copyright file="RunTime.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> liyc </author>
// <date> 2014/1/9 18:03:33 </date>
// <modify>
//   修改人：liyc
//   修改时间：2014/1/9 18:03:33
//   修改描述：新建 SettleCCYChanged
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review >
namespace DM2.Manager.Models
{
    #region

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows;

    using Infrastructure.Utils;

    #endregion

    /// <summary>
    /// 运行时类
    /// </summary>
    public class RunTime
    {
        #region Static Fields

        /// <summary>
        /// 运行中心存储字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, RunTime> RunTimeDic =
            new ConcurrentDictionary<string, RunTime>();

        /// <summary>
        /// 资源字典
        /// </summary>
        private static Dictionary<string, string> resouceDictionary = new Dictionary<string, string>();

        #endregion

        #region Fields

        /// <summary>
        /// 运行中心消息处理者存储字典
        /// </summary>
        private readonly ConcurrentDictionary<string, object> runTimeMessageHandleDic =
            new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 运行中心无标识消息处理者存储字典
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> runTimeNoKeyMessageHandleDic =
            new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 账户窗口打开的窗口列表
        /// </summary>
        private HashSet<string> openWindowCustomerList;

        /// <summary>
        /// 拥有者Id
        /// </summary>
        private string ownerId;

        /// <summary>
        /// <summary>
        /// 资源初始化标识
        /// </summary>
        private ManualResetEvent resourceInitialFlag = new ManualResetEvent(false);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RunTime"/> class. 
        /// </summary>
        /// <param name="user">
        /// PropSetUser 实例
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        private RunTime(string varOwnerId)
        {
            this.ownerId = varOwnerId;
            this.openWindowCustomerList = new HashSet<string>();
            this.RegisterComunicatorEvent();
        }

        #endregion

        #region Public Events

        /// <summary>
        /// 通讯状态变更事件
        /// </summary>
        public event Action<bool> ComunicationStateChangeEvetn;

        #endregion

        #region Public Properties


        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 查找资源
        /// </summary>
        /// <param name="key">
        /// 资源键值
        /// </param>
        /// <returns>
        /// 资源值
        /// </returns>
        public static string FindStringResource(string key)
        {
            //if (mode == ExecuteModeEnum.Normal)
            //{
            //    var content = Application.Current.TryFindResource(key);
            //    if (content == null)
            //    {
            //        return string.Empty;
            //    }

            //    return content.ToString();
            //}

            return string.Empty;
        }

        /// <summary>
        /// 获取当前运行时
        /// </summary>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        /// <returns>
        /// 当前运行时
        /// </returns>
        public static RunTime GetCurrentRunTime(string varOwnerId = null)
        {
            if (string.IsNullOrEmpty(varOwnerId))
            {
                varOwnerId = ConfigParameter.DefaultOwnerId;
            }

            RunTime varRunTime;
            if (RunTimeDic.TryGetValue(varOwnerId, out varRunTime))
            {
                return varRunTime;
            }

            ////return new RunTime(new StaffModel() { StaffID = "test", BusinessUnitID = "1" }, varOwnerId);
            return null;
        }

        /// <summary>
        /// 获取GMT系统时间
        /// </summary>
        /// <returns>系统当前GMT时间</returns>
        public static DateTime GetGMTSystemTime()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <param name="globalTimezone">
        /// The global Timezone.
        /// </param>
        /// <returns>
        /// 系统当前时间
        /// </returns>
        public static DateTime GetGlobalSystemTime(int globalTimezone)
        {
            return GetTimeZone(globalTimezone);
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns>系统当前时间</returns>
        public static DateTime GetGlobalSystemTime()
        {
            int globalTimezone = 8;
            return GetTimeZone(globalTimezone);
        }

        /// <summary>
        /// 临时方法。取GMT时间
        /// </summary>
        /// <param name="timeZone">
        /// The time Zone.
        /// </param>
        /// <returns>
        /// 返回GMT时间
        /// </returns>
        public static DateTime GetTimeZone(int timeZone)
        {
            return DateTime.UtcNow.AddHours(timeZone);
        }

        /// <summary>
        /// 临时方法。取GMT时间
        /// </summary>
        /// <param name="timeZone">
        /// The time Zone.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// 返回GMT时间
        /// </returns>
        public static DateTime GetTimeZone(int timeZone, DateTime time)
        {
            return time.AddHours(timeZone);
        }

        /// <summary>
        /// The get cust config path.
        /// </summary>
        /// <param name="customerNo">
        /// The customer no.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetCustConfigPath(string customerNo)
        {
            return string.Empty;
            // return GetUserConfigPath() + @"\" + customerNo;
        }

        /// <summary>
        /// 获取临时文件目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempConfigPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"StaffSetting\temp\";
        }

        /// <summary>
        /// The get cust user config path.
        /// </summary>
        /// <param name="customerNo">
        /// The customer no.
        /// </param>
        /// <param name="userNo">
        /// The user no.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetCustUserConfigPath(string customerNo, string userNo)
        {
            return GetCustConfigPath(customerNo) + @"\" + userNo;
        }

        /// <summary>
        /// 初始化运行时
        /// </summary>
        /// <param name="varCurrentUser">
        /// 当前用户
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        //public static void Initial(string varOwnerId = null)
        //{
        //    if (string.IsNullOrEmpty(varOwnerId))
        //    {
        //        varOwnerId = ConfigParameter.DefaultOwnerId;
        //    }

        //    var varRunTime = new RunTime(varCurrentUser, varOwnerId);
        //    RunTimeDic.TryAdd(varOwnerId, varRunTime);
        //}

        /// <summary>
        /// 初始化资源字典
        /// </summary>
        /// <param name="resouceUri">
        /// 资源字典路径
        /// </param>
        public static void InitialResourceDictionar(params Uri[] resouceUri)
        {
            resouceDictionary = Util.GetStringResource(resouceUri);
        }

        /// <summary>
        /// 初始化资源存储
        /// </summary>
        /// <param name="resourcePaths">
        /// 资源路径列表
        /// </param>
        public static void InitialResourceDictionar(params string[] resourcePaths)
        {
            resouceDictionary = Util.GetStringResource(resourcePaths);
        }

        /// <summary>
        /// 根据服务端返回的枚举弹出消息框
        /// </summary>
        /// <param name="value">
        /// 服务调用返回的枚举
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        /// <returns>
        /// 错误信息
        /// </returns>
        public static string Message(string value, string varOwnerId)
        {
            string str = value;
            object resMsg = Application.Current.TryFindResource(str);
            string errorMsg = resMsg == null ? "Can't find resource key : " + str : resMsg.ToString();

            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", errorMsg);
            msgDic.Add("PromptEnum", str);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "ErrorDialogWithoutRes");

            return errorMsg;
        }

        /// <summary>
        /// 设置模式
        /// </summary>
        /// <param name="varMode">
        /// 模式
        /// </param>
        //public static void SetMode(ExecuteModeEnum varMode)
        //{
        //    mode = varMode;
        //}


        /// <summary>
        /// 弹出[确认提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="title">
        /// 提示框标题
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        /// <returns>
        /// 确认框的返回值
        /// </returns>
        //public static bool ShowConfirmDialog(string value, string title, string varOwnerId)
        //{
        //    bool result = false;

        //    var confirmDialog = new DialogMessage(
        //        value,
        //        (MessageBoxResult msgResult) =>
        //        {
        //            if (msgResult == MessageBoxResult.OK)
        //            {
        //                result = true;
        //            }
        //            else
        //            {
        //                result = false;
        //            }
        //        });

        //    confirmDialog.Caption = title;

        //    GetCurrentRunTime(varOwnerId).SendMessege(confirmDialog, "ConfirmDialog");

        //    return result;
        //}

        /// <summary>
        /// 弹出[确认提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="title">
        /// 提示框标题
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        /// <returns>
        /// 确认框的返回值
        /// </returns>
        //public static bool ShowConfirmDialogWithoutRes(string value, string title, string varOwnerId)
        //{
        //    bool result = false;

        //    if (varOwnerId == null)
        //    {
        //        varOwnerId = ConfigParameter.DefaultOwnerId;
        //    }

        //    var confirmDialog = new DialogMessage(
        //        value,
        //        (MessageBoxResult msgResult) =>
        //        {
        //            if (msgResult == MessageBoxResult.OK)
        //            {
        //                result = true;
        //            }
        //            else
        //            {
        //                result = false;
        //            }
        //        });

        //    confirmDialog.Caption = title;

        //    GetCurrentRunTime(varOwnerId).SendMessege(confirmDialog, "ConfirmDialogWithoutRes");

        //    return result;
        //}

        /// <summary>
        /// 弹出[信息提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="promptEnum">
        /// 错误返回值的Key
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public static void ShowFailInfoDialog(string value, string promptEnum, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", value);
            msgDic.Add("PromptEnum", promptEnum);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "ErrorDialog");
        }

        /// <summary>
        /// 弹出[信息提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="promptEnum">
        /// 错误返回值的Key
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public static bool ShowFailInfoDialogWithoutRes(string value, string promptEnum, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", value);
            msgDic.Add("PromptEnum", promptEnum);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "ErrorDialogWithoutRes");
            return true;
        }

        /// <summary>
        /// 弹出[信息提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容 
        /// </param>
        /// <param name="promptEnum">
        /// 错误返回值的Key 
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID 
        /// </param>
        /// <returns>
        /// 执行结果
        /// </returns>
        public static bool ShowInfoDialog(string value, string promptEnum, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", value);
            msgDic.Add("PromptEnum", promptEnum);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "InfoDialog");
            return true;
        }

        /// <summary>
        /// 弹出[信息提示框]，不用去资源文件获取提示文字
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="promptEnum">
        /// 错误返回值的Key
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public static void ShowInfoDialogWithoutRes(string value, string promptEnum, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", value);
            msgDic.Add("PromptEnum", promptEnum);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "InfoDialogWithoutRes");
        }

        /// <summary>
        /// 弹出[信息提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="promptEnum">
        /// 错误返回值的Key
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public static void ShowSuccessInfoDialog(string value, string promptEnum, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", value);
            msgDic.Add("PromptEnum", promptEnum);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "SuccessDialog");
        }

        /// <summary>
        /// 弹出[信息提示框]
        /// </summary>
        /// <param name="value">
        /// 信息的内容
        /// </param>
        /// <param name="promptEnum">
        /// 错误返回值的Key
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者ID
        /// </param>
        public static void ShowSuccessInfoDialogWithoutRes(string value, string promptEnum, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Content", value);
            msgDic.Add("PromptEnum", promptEnum);

            GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "SuccessDialogWithoutRes");
        }

        /// <summary>
        /// 弹出提醒
        /// </summary>
        /// <param name="header">标题</param>
        /// <param name="content">内容</param>
        /// <param name="varOwnerId">Owner ID</param>
        public static void ShowNotification(string header, string content, string varOwnerId)
        {
            var msgDic = new Dictionary<string, string>();

            msgDic.Add("Header", header);
            msgDic.Add("Content", content);

            Application.Current.Dispatcher.Invoke(new Action(() => GetCurrentRunTime(varOwnerId).SendMessege(msgDic, "PopupNotificationWithoutRes")));
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            //this.repositoryModel.Clear();
            //this.repositoryModel = null;

            //RunTime varRunTime;
            //RunTimeDic.TryRemove(this.ownerId, out varRunTime);

            //this.runTimeMessageHandleDic.Clear();
            //this.runTimeNoKeyMessageHandleDic.Clear();
            //this.currEventAggregator = null;
        }

        /// <summary>
        /// 清空仓储
        /// </summary>
        public void ClearRepository()
        {
            //this.repositoryModel.Clear();
        }

        /// <summary>
        /// 关闭账户窗口
        /// </summary>
        /// <param name="customerNo">
        /// 涉及账户的Id
        /// </param>
        public void CloseAcctWindow(string customerNo)
        {
            this.openWindowCustomerList.Remove(customerNo);
        }

        /// <summary>
        /// 完成资源初始化
        /// </summary>
        public void CompleteResourceInitial()
        {
            this.resourceInitialFlag.Set();
        }

        /// <summary>
        /// 获取当前的事件聚合器
        /// </summary>
        /// <returns>返回当前的事件聚合器</returns>
        //public IEventAggregator GetCurrentEventAggregator()
        //{
        //    if (mode == ExecuteModeEnum.Normal)
        //    {
        //        return IOCContainer.Instance.Container.Resolve<IEventAggregator>();
        //    }

        //    if (mode == ExecuteModeEnum.Test)
        //    {
        //        return this.currEventAggregator;
        //    }

        //    return null;
        //}

        /// <summary>
        /// 获取仓储实现
        /// 如果此实例已经获取过一次会自动缓存
        /// </summary>
        /// <typeparam name="T">仓储接口</typeparam>
        /// <returns>接口实现</returns>
        //public T GetRepository<T>() where T : IRepositoryCommon
        //{
        //    var parameter = new ParameterOverrides { { "varOwnerId", this.ownerId } };
        //    var rep = IOCContainer.Instance.Container.Resolve<T>(parameter);
        //    return rep;
        //}

        /// <summary>
        /// 初始化仓储及注册回调
        /// </summary>
        public void Initial()
        {
            // 初始化仓储
            //TraceManager.Info.Write("RunTime.InitialResponse", "Storage initialization is beginning.");
            //this.repositoryModel.Intital(); 
            //ComunicatorCore.GetComunicatorCore(this.ownerId)
            //     .InitialErrorMsgNotify(
            //         (value, promptEnum) =>
            //         DispatcherHelper.UIDispatcher.Invoke(
            //             new System.Action(() => ShowFailInfoDialogWithoutRes(value, promptEnum, this.ownerId)),
            //             null),
            //         ShowConfirmDialogWithoutRes);

            //this.requestCallBackModel = new RequestCallBackModel(this.ownerId);
            //this.orderbookCallBackModel = new OrderBookCallBackModel(this.ownerId);
            //this.callBackModel = new CallBackModel(this, this.ownerId);
            //this.staffAlertCallBackModel = new StaffAlertCallBackModel(this.ownerId);
            //this.customerOnlineCallBackModel = new CustomerUserOnlineCallBackModel(this.ownerId);

            //// 启动向router的长连接并订阅推送
            //ComunicatorCore.GetComunicatorCore(this.ownerId)
            //    .SubscritePushBack(
            //        this.requestCallBackModel.PushBack,
            //        this.orderbookCallBackModel.PushBack,
            //        this.callBackModel.PushBack,
            //        c => this.reportCalculateModel.PublishEvent(
            //            new ReportCalculateEvent
            //            {
            //                EvntArg = c,
            //                EvntType = ReportCalculateEvent.ReportCalEvtTypeEnum.PushBack
            //            }),
            //        this.staffAlertCallBackModel.PushBack,
            //        this.customerOnlineCallBackModel.PushBack);
        }

        /// <summary>
        /// 添加进MarginCall列表
        /// </summary>
        /// <param name="customerNo">
        /// 涉及账户的Id
        /// </param>
        //public void AddMarginCallList(string customerNo)
        //{
        //    this.realtimeCaculateModel.PublishEvent(
        //        new CalculateEvent
        //        {
        //            EventType = CalculateEvent.EnumEventType.AddMarginCall,
        //            MarginCallCustomerNo = customerNo
        //        });
        //}

        /// <summary>
        /// 是否注册成功
        /// </summary>
        /// <typeparam name="TMessage">
        /// 消息类型
        /// </typeparam>
        /// <param name="recipient">
        /// 处理者
        /// </param>
        /// <param name="token">
        /// 标识
        /// </param>
        /// <param name="action">
        /// 动作
        /// </param>
        public void RegistMessageMaskHandle<TMessage>(object recipient, string token, Action<TMessage> action)
        {
            //if (mode == ExecuteModeEnum.Normal)
            //{
            //    Messenger.Default.Register(recipient, token, action);
            //    return;
            //}

            //if (!this.runTimeMessageHandleDic.TryAdd(token, action))
            //{
            //    throw new Exception("Repeat message key");
            //}
        }

        /// <summary>
        /// 是否注册成功
        /// </summary>
        /// <typeparam name="TMessage">
        /// 消息类型
        /// </typeparam>
        /// <param name="recipient">
        /// 处理者
        /// </param>
        /// <param name="action">
        /// 动作
        /// </param>
        public void RegistMessageMaskHandle<TMessage>(object recipient, Action<TMessage> action)
        {
            //if (mode == ExecuteModeEnum.Normal)
            //{
            //    Messenger.Default.Register(recipient, action);

            //    return;
            //}

            //this.runTimeNoKeyMessageHandleDic.AddOrUpdate(typeof(TMessage), action, (key, oldValue) => action);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">
        /// 消息类型
        /// </typeparam>
        /// <param name="msgDic">
        /// 消息字典
        /// </param>
        /// <param name="key">
        /// 键值
        /// </param>
        public void SendMessege<T>(T msgDic, string key)
        {
            //if (mode == ExecuteModeEnum.Normal)
            //{
            //    Messenger.Default.Send(msgDic, key);
            //}

            //object objAct;
            //if (!this.runTimeMessageHandleDic.TryGetValue(key, out objAct))
            //{
            //    return;
            //}

            //var action = objAct as Action<T>;
            //if (action != null)
            //{
            //    action(msgDic);
            //}
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">
        /// 消息类型
        /// </typeparam>
        /// <param name="msgDic">
        /// 消息字典
        /// </param>
        public void SendMessege<T>(T msgDic)
        {
            //if (mode == ExecuteModeEnum.Normal)
            //{
            //    Messenger.Default.Send(msgDic);
            //}

            //object objAct;
            //if (!this.runTimeNoKeyMessageHandleDic.TryGetValue(typeof(T), out objAct))
            //{
            //    return;
            //}

            //var action = objAct as Action<T>;
            //if (action != null)
            //{
            //    action(msgDic);
            //}
        }

        /// <summary>
        /// 设置事件聚合器
        /// </summary>
        /// <param name="eventAggr">
        /// 时间聚合器
        /// </param>
        //public void SetEventAggregator(IEventAggregator eventAggr)
        //{
        //    this.currEventAggregator = eventAggr;
        //}

        /// <summary>
        /// 等待资源初始化完毕
        /// </summary>
        public void WaitResourceInitial()
        {
            this.resourceInitialFlag.WaitOne();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 接收系统刷新时间
        /// </summary>
        private void RegisterComunicatorEvent()
        {
            ////this.IsConnected = Comunicator.GetComunicator(this.ownerId).IsConnected;
            ////Comunicator.SubscriteSysTimeReresh(datetime => { this.SysTime = datetime; }, this.ownerId);
            ////Comunicator.SubscriteCommunicationStateChange(
            ////    c =>
            ////    {
            ////        if (c != this.IsConnected)
            ////        {
            ////            try
            ////            {
            ////                this.IsConnected = c;
            ////                if (this.ComunicationStateChangeEvetn != null)
            ////                {
            ////                    this.ComunicationStateChangeEvetn(c);
            ////                }
            ////            }
            ////            catch (Exception exception)
            ////            {
            ////                LogService.Instance.Error("Runtime中执行通讯状态变更的通知时发生异常", exception);
            ////            }
            ////        }
            ////    },
            ////    this.ownerId);
        }

        #endregion
    }
}