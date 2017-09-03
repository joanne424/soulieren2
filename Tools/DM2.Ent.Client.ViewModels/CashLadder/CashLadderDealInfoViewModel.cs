// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashLadderDealInfoViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 05:10:14 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 05:10:14
//      修改描述：新建 CashLadderDealInfoViewModel.cs
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
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    /// <summary>
    ///     The deal list tool view model.
    /// </summary>
    public class CashLadderDealInfoViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     DealCacheRepository
        /// </summary>
        private readonly IFxHedgingDealRepository dealReps;

        /// <summary>
        ///     交易单列表
        /// </summary>
        private ObservableCollection<FxHedgingDealModel> dealList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CashLadderDealInfoViewModel"/> class.
        /// </summary>
        /// <param name="DealIdList">
        /// The Deal Id List.
        /// </param>
        /// <param name="varOwnerId">
        /// The var owner id.
        /// </param>
        public CashLadderDealInfoViewModel(List<string> DealIdList, string varOwnerId = null)
            : base(varOwnerId)
        {
            this.dealReps = this.GetRepository<IFxHedgingDealRepository>();
            this.DealList = new ObservableCollection<FxHedgingDealModel>();
            foreach (string dealid in DealIdList)
            {
                FxHedgingDealModel tempDeal = this.dealReps.FindByID(dealid);
                if (tempDeal != null)
                {
                    this.DealList.Add(tempDeal);
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     交易单列表
        /// </summary>
        public ObservableCollection<FxHedgingDealModel> DealList
        {
            get
            {
                return this.dealList;
            }

            set
            {
                this.dealList = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The new_ click.
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        #endregion
    }
}