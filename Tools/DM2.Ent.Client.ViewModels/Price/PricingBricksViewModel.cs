// <copyright file="PricingBricksViewModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2017/05/10 04:44:43 </date>
// <summary> 报价块列表VM </summary>
// <modify>
//      修改人：fangz
//      修改时间：2017/05/10 04:44:43
//      修改描述：新建 PricingBricksViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Ent.Client.ViewModels.Price
{
    #region

    using System;
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    #endregion

    /// <summary>
    /// 报价块列表VM
    /// </summary>
    public class PricingBricksViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        /// 当前运行时
        /// </summary>
        private RunTime currentRunTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingBricksViewModel"/> class.
        /// </summary>
        /// <param name="owmerId">
        /// The owmer id.
        /// </param>
        public PricingBricksViewModel(string owmerId = null)
            : base(owmerId)
        {
            this.currentRunTime = RunTime.GetCurrentRunTime(this.OwnerId);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the block list.
        /// </summary>
        public ObservableCollection<PriceBlockModel> BlockList
        {
            get
            {
                return this.currentRunTime.CurrentPriceHandleCore.PriceListForBind;
            }
        }

        #endregion

        /// <summary>
        /// 鼠标点击报价动作
        /// </summary>
        public void PriceClick()
        {
            Console.WriteLine("ss");
        }
    }
}