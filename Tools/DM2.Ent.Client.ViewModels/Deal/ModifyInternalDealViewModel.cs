// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyInternalDealViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/18 05:30:30 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/18 05:30:30
//      修改描述：新建 ModifyInternalDealViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System.Collections.Generic;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    /// <summary>
    ///     The new hedge spot forward view model.
    /// </summary>
    public class ModifyInternalDealViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The contract rep.
        /// </summary>
        private readonly IContractRepository contractRep;

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     The 1 deal model.
        /// </summary>
        private FxInternalDealModel dealModel1;

        /// <summary>
        ///     The 2 deal model.
        /// </summary>
        private FxInternalDealModel dealModel2;

        /// <summary>
        ///     The title.
        /// </summary>
        private string title;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyInternalDealViewModel"/> class.
        ///     Initializes a new instance of the <see cref="ModifySpotForwardViewModel"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="ownerID">
        /// The owner id.
        /// </param>
        public ModifyInternalDealViewModel(FxInternalDealModel model, string ownerID = null)
        {
            this.DisplayName = RunTime.FindStringResource("InternalDeal") + " - " + model.ExecutionId;
            this.DealModel1 = new FxInternalDealModel();
            this.DealModel2 = new FxInternalDealModel();
            this.Title = RunTime.FindStringResource("InternalDeal") + " - " + model.Id;
            FxInternalDealModel tempDeal = this.GetOtherDealByexecuteid(model.ExecutionId);
            if (tempDeal != null)
            {
                this.DealModel2.Copy(tempDeal);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the near deal model.
        /// </summary>
        public FxInternalDealModel DealModel1
        {
            get
            {
                return this.dealModel1;
            }

            set
            {
                this.dealModel1 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the far deal model.
        /// </summary>
        public FxInternalDealModel DealModel2
        {
            get
            {
                return this.dealModel2;
            }

            set
            {
                this.dealModel2 = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The cancel_ click.
        /// </summary>
        public void Cancel_Click()
        {
            this.TryClose();
        }

        /// <summary>
        ///     发布已取消事件
        /// </summary>
        /// <summary>
        ///     发布已确认事件
        /// </summary>
        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get other deal by is near.
        /// </summary>
        /// <param name="executeId">
        /// The execute Id.
        /// </param>
        /// <returns>
        /// The <see cref="FxHedgingDealModel"/>.
        /// </returns>
        private FxInternalDealModel GetOtherDealByexecuteid(string executeId)
        {
            IList<FxInternalDealModel> deal = this.GetSevice<InternalDealService>().FindByEnt(executeId);
            return null;
        }

        #endregion
    }
}