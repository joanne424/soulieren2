// <copyright file="StaffAlertCallBackModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/03/31 03:48:52 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/03/31 03:48:52
//      修改描述：新建 StaffAlertCallBackModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models
{
    using System;
    using System.IO;
    using System.Media;

    using BaseViewModel;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Log;
    using Infrastructure.Models;

    /// <summary>
    ///     The staff alert call back model.
    /// </summary>
    public class StaffAlertCallBackModel : BaseModel
    {
        #region Fields

        /// <summary>
        ///     The staff alert reps.
        /// </summary>
        private readonly IStaffAlertCacheRepository staffAlertReps;

        /// <summary>
        ///     运行实例
        /// </summary>
        private RunTime runtime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffAlertCallBackModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public StaffAlertCallBackModel(string ownerId)
            : base(ownerId)
        {
            this.runtime = RunTime.GetCurrentRunTime(ownerId);
            this.staffAlertReps = this.GetRepository<IStaffAlertCacheRepository>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The push back.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        public void PushBack(StaffAlertModel model)
        {
            var baseVm = new BaseStaffAlertVM(model);
            this.staffAlertReps.AddOrUpdate(baseVm);

            // 弹框播放声音
            if (model.EventMethod == StaffEventMethodEnum.Popup && !model.IsRead && model.EventType != StaffEventTypeEnum.System)
            {
                this.PopupNotification(model);
                this.PlaySound(model.Sound);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The play sound.
        /// </summary>
        /// <param name="soundFile">
        /// The sound file.
        /// </param>
        private void PlaySound(string soundFile)
        {
            if (string.IsNullOrEmpty(soundFile))
            {
                return;
            }

            try
            {
                var player =
                    new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Sounds", soundFile));
                player.Play();
            }
            catch (Exception ex)
            {
                TraceManager.Error.Write("StaffAlertCallBackModel", ex, "接收到StaffAlertCallBackModel,播放声音提醒时找不到文件");
            }
        }

        /// <summary>
        /// The popup notification.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void PopupNotification(StaffAlertModel model)
        {
            string header = string.Empty;
            switch (model.EventType)
            {
                case StaffEventTypeEnum.StaffPriceAlert:
                    header = "Price Alert";
                    break;
                case StaffEventTypeEnum.StaffTimeAlert:
                    header = "Time Alert";
                    break;
                case StaffEventTypeEnum.System:
                    header = "System Alert";
                    break;
                default:
                    break;
            }

            // 目前Alert标题全部用Notification
            header = RunTime.FindStringResource("Notification");
            RunTime.ShowNotification(header, model.Content, this.OwnerId);
        }

        #endregion
    }
}