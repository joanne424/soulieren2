//// <copyright file="FailInfoWinViewModel.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> zhangwz </author>
//// <date> 2014/1/20 15:49:24 </date>
//// <modify>
////   修改人：zhangwz
////   修改时间：2014/1/20 15:49:24
////   修改描述：新建 FailInfoWinView
////   版本：1.0
//// </modify>
//// <review>
////   review人：
////   review时间：
//// </review >

namespace DM2.Ent.Client.ViewModels.Common
{
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models.Base;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
   /// 错误信息反馈窗口
   /// </summary>
    public class FailInfoWinViewModel : BaseVm
    {
       /// <summary>
       /// 操作成功窗口
       /// </summary>
        #region 字段

       /// <summary>
       /// 确认信息 字段
       /// </summary>
        private string confirmInfo;

       /// <summary>
       /// 返回信息码的字符串形式
       /// </summary>
        private string prompt;

        #endregion

        #region 构造函数

       /// <summary>
       /// Initializes a new instance of the <see cref="FailInfoWinViewModel"/> class.
       /// </summary>
       /// <param name="confirmInfo">要显示的文字</param>
       ///  <param name="propFlagStr">信息码</param>
       /// <param name="varOwnerId">拥有者Id</param>
        public FailInfoWinViewModel(string confirmInfo, string propFlagStr, string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("OperationFailed");
            this.confirmInfo = confirmInfo;
            this.prompt = propFlagStr;
            Messenger.Default.Register<string>(this, "UpdateLanguage", msg => this.SetDisplayName(RunTime.FindStringResource("OperationFailed")));
        }

        #endregion

        #region 属性
       /// <summary>
       /// 确认信息 属性
       /// </summary>
        public string ConfirmInfo
        {
            get
            {
                return this.confirmInfo;
            }

            set
            {
                this.confirmInfo = value;
                this.NotifyOfPropertyChange("ConfirmInfo");
            }
        }

       /////// <summary>
       /////// 信息码的字符串形式，属性
       /////// </summary>
       //// public PromptEnum Prompt
       //// {
       ////     get
       ////     {
       ////         if (string.IsNullOrEmpty(this.prompt))
       ////         {
       ////             return PromptEnum.SystemUnknownFailture;
       ////         }

       ////         return (PromptEnum)Enum.Parse(typeof(PromptEnum), this.prompt);
       ////     }
       //// }

       /// <summary>
       /// 取消动作
       /// </summary>
        public void ConfirmCancel()
        {
            this.TryClose(false);
        }

        #endregion

        /// <summary>
        /// The dispose.
        /// </summary>
        public override void Dispose()
        {
            Messenger.Default.Unregister<string>(this, "UpdateLanguage");
        }

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
    }
}
