//// <copyright file="YesNoOrCancelWindowViewModel.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> wangrx </author>
//// <date> 2016/05/03 10:22:25 </date>
//// <modify>
////   修改人：wangrx
////   修改时间：2016/05/03 10:22:25
////   修改描述：新建 YesNoOrCancelWindowViewModel
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
    /// Yes No or Cancel 选择窗口
    /// </summary>
    public class YesNoOrCancelWindowViewModel : BaseVm
    {
        #region 字段

        /// <summary>
        /// 确认信息 字段
        /// </summary>
        private string confirmInfo;
        #endregion

        #region 构造函数
        /// <summary>
        /// Initializes a new instance of the <see cref="YesNoOrCancelWindowViewModel"/> class.
        /// </summary>
        /// <param name="confirmInfo">要显示的文字</param>
        /// <param name="varOwnerId">拥有者Id</param>
        public YesNoOrCancelWindowViewModel(string confirmInfo, string varOwnerId = null)
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

        ConfirmResults result = ConfirmResults.Non;
        /// <summary>
        /// 选择结果
        /// </summary>
        public ConfirmResults Result
        {
            get
            {
                return this.result;
            }
            protected set
            {
                this.result = value;
                this.NotifyOfPropertyChange("Result");
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 取消动作
        /// </summary>
        public void ConfirmCancel()
        {
            this.result = ConfirmResults.Cancel;
            this.TryClose(false);
        }

        /// <summary>
        /// 确认动作
        /// </summary>
        public void ConfirmYes()
        {
            this.result = ConfirmResults.Yes;
            this.TryClose(true);
        }

        /// <summary>
        /// 确认动作
        /// </summary>
        public void ConfirmNo()
        {
            this.result = ConfirmResults.No;
            this.TryClose(true);
        }

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

        ///// <summary>
        ///// 关闭
        ///// </summary>
        //public override void TryClose()
        //{
        //    this.result = ConfirmResults.Non;
        //    base.TryClose();
        //}
        #endregion

        /// <summary>
        /// 选择结果枚举
        /// </summary>
        public enum ConfirmResults
        {
            /// <summary>
            /// 默认值，表示用户什么都没选择
            /// </summary>
            Non = 0,
            /// <summary>
            /// 用户选择了此界面的Yes选项
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 用户选择了No选项
            /// </summary>
            No = 2,
            /// <summary>
            /// 用户选择了Cancel 选项
            /// </summary>
            Cancel = 3,
        }
    }
}
