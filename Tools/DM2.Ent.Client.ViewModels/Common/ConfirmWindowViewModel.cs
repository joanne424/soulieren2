//// <copyright file="ConfirmWindowViewModel.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> zhangwz </author>
//// <date> 2013/11/26 14:22:25 </date>
//// <modify>
////   修改人：zhangwz
////   修改时间：2013/11/26 14:22:25
////   修改描述：新建 ConfirmWindowViewModel
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
   /// 交易确认窗口
   /// </summary>
    public class ConfirmWindowViewModel : BaseVm
    {
        #region 字段

       /// <summary>
       /// 确认信息 字段
       /// </summary>
        private string confirmInfo;
        #endregion

        #region 构造函数
       /// <summary>
       /// Initializes a new instance of the <see cref="ConfirmWindowViewModel"/> class.
       /// </summary>
       /// <param name="confirmInfo">要显示的文字</param>
       /// <param name="varOwnerId">拥有者Id</param>
        public ConfirmWindowViewModel(string confirmInfo, string varOwnerId = null)
            : base(varOwnerId)
        {
            this.DisplayName = RunTime.FindStringResource("Confirmation");
            this.confirmInfo = confirmInfo;
            Messenger.Default.Register<string>(this, "UpdateLanguage", msg => this.SetDisplayName(RunTime.FindStringResource("Confirmation")));
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

       /// <summary>
       /// 取消动作
       /// </summary>
        public void ConfirmCancel()
        {
            this.TryClose(false);
        }

       /// <summary>
       /// 确认动作
       /// </summary>
        public void ConfirmOK()
        {
            this.TryClose(true);
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
