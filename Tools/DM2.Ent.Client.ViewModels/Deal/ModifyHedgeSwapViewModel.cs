// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyHedgeSwapViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/18 05:30:30 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/18 05:30:30
//      修改描述：新建 ModifyHedgeSwapViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using Caliburn.Micro;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common.Enums;

    /// <summary>
    ///     The new hedge spot forward view model.
    /// </summary>
    public class ModifyHedgeSwapViewModel : BaseVm
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
        /// The far deal model.
        /// </summary>
        private FxHedgingDealModel farDealModel;

        /// <summary>
        /// The near deal model.
        /// </summary>
        private FxHedgingDealModel nearDealModel;

        /// <summary>
        /// The title.
        /// </summary>
        private string title;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyHedgeSwapViewModel"/> class.
        ///     Initializes a new instance of the <see cref="ModifySpotForwardViewModel"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="ownerID">
        /// The owner id.
        /// </param>
        public ModifyHedgeSwapViewModel(FxHedgingDealModel model, string ownerID = null)
        {
            this.DisplayName = RunTime.FindStringResource("HedgeDeal") + " - " + model.ExecutionId;
            this.NearDeal = new FxHedgingDealModel();
            this.FarDeal = new FxHedgingDealModel();
            this.Title = RunTime.FindStringResource("HedgeDeal") + " - " + model.Id;
            if (model.IsNearLeg == (int)IsNearLegEnum.NEAR_LEG)
            {
                this.NearDeal.Copy(model);
                var tempDeal = this.GetOtherDealByIsNear(model.ExecutionId, (int)IsNearLegEnum.FAR_LEG);
                if (tempDeal != null)
                {
                    this.FarDeal.Copy(tempDeal);
                }
            }
            else
            {
                this.FarDeal.Copy(model);
                var tempDeal = this.GetOtherDealByIsNear(model.ExecutionId, (int)IsNearLegEnum.NEAR_LEG);
                if (tempDeal != null)
                {
                    this.NearDeal.Copy(tempDeal);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the far deal model.
        /// </summary>
        public FxHedgingDealModel FarDeal
        {
            get
            {
                return this.farDealModel;
            }

            set
            {
                this.farDealModel = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the near deal model.
        /// </summary>
        public FxHedgingDealModel NearDeal
        {
            get
            {
                return this.nearDealModel;
            }

            set
            {
                this.nearDealModel = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the title.
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
        /// <param name="p">
        /// The p.
        /// </param>
        /// <returns>
        /// The <see cref="FxHedgingDealModel"/>.
        /// </returns>
        private FxHedgingDealModel GetOtherDealByIsNear(string executeId, int isNearleg)
        {
            var deal = this.GetSevice<HedgingDealService>().GetSwapDeal(executeId, isNearleg);
            return deal;
        }

        #endregion
    }
}